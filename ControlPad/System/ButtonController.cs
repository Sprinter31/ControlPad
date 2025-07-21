using System;
using System.Runtime.InteropServices;

namespace ControlPad
{
    public static class ButtonController
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void keybd_event(uint bVk, uint bScan, uint dwFlags, uint dwExtraInfo);

        private const int VK_LEFT = 0xB3;

        public static void LeftArrow()
        {
            keybd_event(0xB0, 0, 0, 0);
        }

    }
}
