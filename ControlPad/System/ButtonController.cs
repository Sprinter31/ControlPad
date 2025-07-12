using System;
using System.Runtime.InteropServices;

namespace ButtonController
{
    public static class ButtonController
    {
        // Virtual-Key Codes
        private const ushort VK_MEDIA_NEXT_TRACK = 0xB0;
        private const ushort VK_MEDIA_PREV_TRACK = 0xB1;
        private const ushort VK_MEDIA_STOP = 0xB2;
        private const ushort VK_MEDIA_PLAY_PAUSE = 0xB3;
        private const ushort VK_LAUNCH_MEDIA_SELECT = 0xB5;
        private const ushort VK_LAUNCH_MAIL = 0xB4;
        private const ushort VK_LAUNCH_APP2 = 0xB7; // Calculator

        private const uint INPUT_KEYBOARD = 1;
        private const uint KEYEVENTF_KEYUP = 0x0002;

        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT
        {
            public uint type;
            public InputUnion u;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct InputUnion
        {
            [FieldOffset(0)]
            public KEYBDINPUT ki;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        private static void SendKey(ushort vkCode)
        {
            INPUT[] inputs = new INPUT[2];

            inputs[0].type = INPUT_KEYBOARD;
            inputs[0].u.ki = new KEYBDINPUT
            {
                wVk = vkCode,
                dwFlags = 0
            };

            inputs[1].type = INPUT_KEYBOARD;
            inputs[1].u.ki = new KEYBDINPUT
            {
                wVk = vkCode,
                dwFlags = KEYEVENTF_KEYUP
            };

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        // Öffentliche Methoden zum Aufruf

        public static void PlayPause() => SendKey(VK_MEDIA_PLAY_PAUSE);
        public static void NextTrack() => SendKey(VK_MEDIA_NEXT_TRACK);
        public static void PreviousTrack() => SendKey(VK_MEDIA_PREV_TRACK);
        public static void Stop() => SendKey(VK_MEDIA_STOP);
        public static void OpenCalculator() => SendKey(VK_LAUNCH_APP2);
        public static void OpenMail() => SendKey(VK_LAUNCH_MAIL);
        public static void OpenMediaApp() => SendKey(VK_LAUNCH_MEDIA_SELECT);
    }
}
