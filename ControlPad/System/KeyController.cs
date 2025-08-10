using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace ControlPad
{
    class KeyController
    {
        // ---- Win32 ----
        [StructLayout(LayoutKind.Sequential)]
        struct INPUT { public uint type; public InputUnion U; }
        [StructLayout(LayoutKind.Explicit)]
        struct InputUnion
        {
            [FieldOffset(0)] public KEYBDINPUT ki;
            [FieldOffset(0)] public MOUSEINPUT mi;
            [FieldOffset(0)] public HARDWAREINPUT hi;
        }
        [StructLayout(LayoutKind.Sequential)]
        struct KEYBDINPUT
        {
            public ushort wVk;      // bei Scancode-Injection = 0
            public ushort wScan;    // Scancode
            public uint dwFlags;    // SCANCODE / KEYUP / EXTENDED
            public uint time;
            public IntPtr dwExtraInfo;
        }
        [StructLayout(LayoutKind.Sequential)] struct MOUSEINPUT { public int dx, dy, mouseData; public uint dwFlags, time; public IntPtr dwExtraInfo; }
        [StructLayout(LayoutKind.Sequential)] struct HARDWAREINPUT { public uint uMsg; public ushort wParamL, wParamH; }

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")] static extern uint MapVirtualKey(uint uCode, uint uMapType);
        const uint MAPVK_VK_TO_VSC = 0;

        [DllImport("user32.dll")] static extern bool SystemParametersInfo(uint uiAction, uint uiParam, out uint pvParam, uint fWinIni);
        const uint SPI_GETKEYBOARDDELAY = 0x0016;  // 0..3  -> (idx+1)*250ms
        const uint SPI_GETKEYBOARDSPEED = 0x000A;  // 0..31 -> ~2.5..31 cps

        const uint INPUT_KEYBOARD = 1;
        const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        const uint KEYEVENTF_KEYUP = 0x0002;
        const uint KEYEVENTF_SCANCODE = 0x0008;

        // VKs, die nur als VK funktionieren (Media/Browser/Launch…)
        static readonly HashSet<ushort> VkOnly = new()
    {
        0xAE, 0xAF, 0xAD,             // VOLUME_DOWN/UP/MUTE
        0xB0, 0xB1, 0xB2, 0xB3,       // MEDIA_NEXT/PREV/STOP/PLAY
        0xA6,0xA7,0xA8,0xA9,0xAA,0xAB,0xAC, // Browser Back/Forward/Refresh/Stop/Search/Favorites/Home
        0xB4,0xB5,0xB6,0xB7           // Launch Mail/Media/App1/App2
    };

        static INPUT MakeVkKey(ushort vk, bool keyUp) => new INPUT
        {
            type = INPUT_KEYBOARD,
            U = new InputUnion
            {
                ki = new KEYBDINPUT
                {
                    wVk = vk,
                    wScan = 0,
                    dwFlags = (keyUp ? KEYEVENTF_KEYUP : 0) | KEYEVENTF_EXTENDEDKEY,
                    time = 0,
                    dwExtraInfo = IntPtr.Zero
                }
            }
        };

        static INPUT MakeScanKey(ushort vk, bool keyUp)
        {
            ushort sc = (ushort)MapVirtualKey(vk, MAPVK_VK_TO_VSC);
            bool extended = IsExtended(vk);
            return new INPUT
            {
                type = INPUT_KEYBOARD,
                U = new InputUnion
                {
                    ki = new KEYBDINPUT
                    {
                        wVk = 0, // wichtig bei SCANCODE
                        wScan = sc,
                        dwFlags = KEYEVENTF_SCANCODE
                                  | (extended ? KEYEVENTF_EXTENDEDKEY : 0)
                                  | (keyUp ? KEYEVENTF_KEYUP : 0),
                        time = 0,
                        dwExtraInfo = IntPtr.Zero
                    }
                }
            };
        }

        static INPUT MakeKey(ushort vk, bool keyUp)
            => VkOnly.Contains(vk) ? MakeVkKey(vk, keyUp) : MakeScanKey(vk, keyUp);

        static bool IsExtended(ushort vk) =>
            vk is 0x25 or 0x26 or 0x27 or 0x28   // LEFT/UP/RIGHT/DOWN
            or 0x2D or 0x2E or 0x24 or 0x23      // INS/DEL/HOME/END
            or 0x21 or 0x22                      // PGUP/PGDN
            or 0x6F                               // NUMPAD DIVIDE
            or 0xA3 or 0xA5;                     // RCTRL / RALT

        static void Send(params INPUT[] inputs) =>
            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf<INPUT>());

        // --- Windows-Repeat auslesen (Delay + cps) ---
        static (int initialDelayMs, double repeatHz) GetSystemRepeatSettings()
        {
            int initialDelayMs = 300; // Fallback
            if (SystemParametersInfo(SPI_GETKEYBOARDDELAY, 0, out uint delayIdx, 0))
                initialDelayMs = (int)((delayIdx + 1) * 250);

            double repeatHz = 25.0; // Fallback
            if (SystemParametersInfo(SPI_GETKEYBOARDSPEED, 0, out uint speedIdx, 0))
            {
                // Windows-Doku: 0 ~ 2.5 cps ... 31 ~ ~31 cps (annähernd linear)
                repeatHz = 2.5 + (speedIdx * (31.0 - 2.5) / 31.0);
            }
            return (initialDelayMs, repeatHz);
        }

        // ---- Hold-Management ----
        class HoldState
        {
            public ushort Vk;
            public bool TapMode;            // true = Down+Up-Impulse (für Media-Keys)
            public bool LeftDown;           // ob wir logisch unten gelassen haben (für Scan-Keys)
            public int InitialDelayMs;
            public double RepeatHz;         // jetzt double für exaktere Periodik
            public CancellationTokenSource Cts = new();
            public Task? Worker;
        }

        static readonly ConcurrentDictionary<ushort, HoldState> Holds = new();

        public static void HoldStart(ushort vk, int? initialDelayMs = null, int? repeatHz = null)
        {
            if (Holds.ContainsKey(vk)) return;

            var (sysDelayMs, sysHz) = GetSystemRepeatSettings();
            bool tap = VkOnly.Contains(vk); // Media-Keys als Taps wiederholen

            var state = new HoldState
            {
                Vk = vk,
                TapMode = tap,
                InitialDelayMs = initialDelayMs ?? sysDelayMs,
                // WICHTIG: immer Windows-Rate als Default – auch für Media-Keys
                RepeatHz = repeatHz.HasValue ? Math.Max(1, (double)repeatHz.Value) : sysHz
            };
            if (!Holds.TryAdd(vk, state)) return;

            var token = state.Cts.Token;

            // Erstes "Feuern"
            if (state.TapMode)
            {
                Send(MakeKey(vk, false), MakeKey(vk, true)); // Tap
                state.LeftDown = false;
            }
            else
            {
                Send(MakeKey(vk, false)); // Taste bleibt unten
                state.LeftDown = true;
            }

            state.Worker = Task.Run(() =>
            {
                // Initial-Delay (ohne Exceptions abbrechbar)
                if (token.WaitHandle.WaitOne(state.InitialDelayMs)) return;

                int periodMs = Math.Max(5, (int)Math.Round(1000.0 / state.RepeatHz));

                while (!token.IsCancellationRequested)
                {
                    if (token.WaitHandle.WaitOne(periodMs)) break;

                    if (state.TapMode)
                    {
                        // Media-Keys: Down+Up-Impuls in Windows-Rate
                        Send(MakeKey(vk, false), MakeKey(vk, true));
                    }
                    else
                    {
                        // Normale Tasten: zusätzlicher KeyDown für Autorepeat
                        Send(MakeKey(vk, false));
                    }
                }
            }, token);
        }

        public static void HoldStop(ushort vk)
        {
            if (!Holds.TryRemove(vk, out var state)) return;

            try
            {
                state.Cts.Cancel();
                state.Worker?.Wait(300);
            }
            catch { /* ignorieren */ }

            if (!state.TapMode && state.LeftDown)
                Send(MakeKey(vk, true)); // finales KeyUp nur bei "Taste bleibt unten"

            state.Cts.Dispose();
        }

        public static void StopHoldingAllKeys()
        {
            foreach (var vk in new List<ushort>(Holds.Keys))
                HoldStop(vk);
        }
    }

    public sealed class KeyGroup
    {
        public KeyGroup(string name, IEnumerable<KeyCode> items)
        {
            Name = name;
            Items = new ObservableCollection<KeyCode>(items);
        }
        public string Name { get; }
        public ObservableCollection<KeyCode> Items { get; }
    }

    public class KeyCode
    {
        public uint Code { get; set; }
        public string Label { get; set; }
        public KeyCode(uint code, string label)
        {
            Code = code;
            Label = label;
        }
    }
}