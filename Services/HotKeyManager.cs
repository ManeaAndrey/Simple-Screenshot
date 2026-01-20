using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Simple_Screenshot.Services
{
    public class HotKeyManager
    {
        private const int WM_HOTKEY = 0x0312;
        private const uint MOD_CONTROL = 0x0002;
        private const uint MOD_SHIFT = 0x0004;
        private const int HOTKEY_ID = 1;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public static bool RegisterHotkey(IntPtr windowHandle, Keys key, bool controlModifier = false, bool shiftModifier = false)
        {
            uint modifiers = 0;
            if (controlModifier) modifiers |= MOD_CONTROL;
            if (shiftModifier) modifiers |= MOD_SHIFT;

            return RegisterHotKey(windowHandle, HOTKEY_ID, modifiers, (uint)key);
        }

        public static bool UnregisterHotkey(IntPtr windowHandle)
        {
            return UnregisterHotKey(windowHandle, HOTKEY_ID);
        }

        public static int GetHotkeyId()
        {
            return HOTKEY_ID;
        }

        public static int GetHotKeyMessage()
        {
            return WM_HOTKEY;
        }
    }
}
