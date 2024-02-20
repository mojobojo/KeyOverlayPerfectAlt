using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace KeyOverlay {
    internal class KeyboardHook {
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;

        [DllImport("user32.dll")]
        public static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpKeyState, StringBuilder lpChar, int uFlags);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public static bool[] KeyStates = new bool[256];
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
            if (nCode >= 0) {
                int vkCode = Marshal.ReadInt32(lParam);
                int flags = (int)wParam;

                Debug.WriteLine(string.Format("{0:X} {1:X} {2:X}", nCode, vkCode, flags));

                bool key_up = (flags == WM_KEYUP) || (flags == WM_SYSKEYUP);
                bool key_down = (flags == WM_KEYDOWN) || (flags == WM_SYSKEYDOWN);

                if (key_up) {
                    KeyStates[vkCode] = false;
                } 
                if (key_down) {
                    KeyStates[vkCode] = true;
                }
            }
            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        public static void CreateHook() {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule) {
                SetWindowsHookEx(13, HookCallback, GetModuleHandle(curModule.ModuleName), 0);
                //Application.Run();
            }
        }
    }
}
