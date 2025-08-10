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

        const uint INPUT_KEYBOARD = 1;
        const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        const uint KEYEVENTF_KEYUP = 0x0002;
        const uint KEYEVENTF_SCANCODE = 0x0008;

        // ---- Ergänzungen oben bei den Konstanten ----
        static readonly HashSet<ushort> VkOnly = new()
{
    0xAE, 0xAF, 0xAD,             // VOLUME_DOWN/UP/MUTE
    0xB0, 0xB1, 0xB2, 0xB3,       // MEDIA_NEXT/PREV/STOP/PLAY
    0xA6,0xA7,0xA8,0xA9,0xAA,0xAB,0xAC, // Browser
    0xB4,0xB5,0xB6,0xB7           // Launch Mail/Media/App1/App2
};

        // NICHT rekursiv!
        static INPUT MakeVkKey(ushort vk, bool keyUp)
        {
            return new INPUT
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
        }

        // Unverändert wie vorher (Scancode-Injection)
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

        // Dispatcher – ruft MakeVkKey ODER MakeScanKey (aber niemals MakeKey!)
        static INPUT MakeKey(ushort vk, bool keyUp)
        {
            return VkOnly.Contains(vk) ? MakeVkKey(vk, keyUp)
                                       : MakeScanKey(vk, keyUp);
        }

        static bool IsExtended(ushort vk)
        {
            // typ. E0-Tasten: Pfeile, Insert/Delete, Home/End, PgUp/PgDn, Numpad-Divide, Numpad-Enter, RCtrl, RAlt
            return vk is 0x25 or 0x26 or 0x27 or 0x28 // VK_LEFT/UP/RIGHT/DOWN
                    or 0x2D or 0x2E or 0x24 or 0x23   // INS/DEL/HOME/END
                    or 0x21 or 0x22                   // PGUP/PGDN
                    or 0x6F or 0x0D                   // NUMPAD DIVIDE / NUMPAD ENTER (VK_DIVIDE=0x6F, VK_RETURN=0x0D -> nur NumEnter ist extended)
                    or 0xA3 or 0xA5;                  // RCTRL / RALT
        }

        static void Send(params INPUT[] inputs) => SendInput((uint)inputs.Length, inputs, Marshal.SizeOf<INPUT>());

        // ---- Hold-Management ----
        class HoldState { public ushort Vk; public CancellationTokenSource Cts = new(); public int RepeatHz; }

        static readonly ConcurrentDictionary<ushort, HoldState> Holds = new();

        /// <summary>Startet Gedrückthalten mit Autorepeat.</summary>
        public static void HoldStart(ushort vk, int repeatHz = 25)
        {
            if (Holds.ContainsKey(vk)) return; // schon gehalten

            var state = new HoldState { Vk = vk, RepeatHz = repeatHz };
            if (!Holds.TryAdd(vk, state)) return;

            // 1) Logisch runter
            Send(MakeKey(vk, keyUp: false));

            // 2) Autorepeat per Task
            _ = Task.Run(async () =>
            {
                int delay = Math.Max(10, 1000 / state.RepeatHz);
                try
                {
                    while (!state.Cts.IsCancellationRequested)
                    {
                        await Task.Delay(delay, state.Cts.Token);
                        // zusätzliche KeyDowns (ohne KeyUp) erzeugen Wiederhol-Events für Apps, die darauf hören
                        Send(MakeKey(vk, keyUp: false));
                    }
                }
                catch (TaskCanceledException) { /* ok */ }
            }, state.Cts.Token);
        }

        /// <summary>Beendet Gedrückthalten und sendet KeyUp.</summary>
        public static void HoldStop(ushort vk)
        {
            if (!Holds.TryRemove(vk, out var state)) return;
            state.Cts.Cancel();
            // final KeyUp
            Send(MakeKey(vk, keyUp: true));
            state.Cts.Dispose();
        }

        public static void StopHoldingAllKeys()
        {
            for (ushort i = 0; i < 255; i++)
            {
                KeyController.HoldStop(i);
            }
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