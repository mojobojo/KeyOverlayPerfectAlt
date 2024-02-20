using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using SFML.Graphics;
using SFML.Window;

namespace KeyOverlay
{
    public class Key
    {
        [DllImport("user32.dll")]
        private static extern short VkKeyScan(char ch);

        public int Hold { get; set; }
        public List<RectangleShape> BarList = new();
        public string KeyLetter = "";
        public readonly Keyboard.Key KeyboardKey;
        //public readonly Mouse.Button MouseButton;
        public int Counter = 0;
        public readonly bool isKey = true;
        public bool Held = false;
        public bool Error = false;
        public readonly int VKCode;

        public static int ConvertCharToVirtualKey(char ch) {
            short vkey = VkKeyScan(ch);
            int flags = vkey >> 8;
            return (int)(vkey & 0xff);
        }

        public Key(string key)
        {
            KeyLetter = key;
            
            if (!Enum.TryParse(key, out KeyboardKey)) {
            }

            switch (key) {
                case "Num0":
                    VKCode = 0x30;
                    break;
                case "Num1":
                    VKCode = 0x31;
                    break;
                case "Num2":
                    VKCode = 0x32;
                    break;
                case "Num3":
                    VKCode = 0x33;
                    break;
                case "Num4":
                    VKCode = 0x34;
                    break;
                case "Num5":
                    VKCode = 0x35;
                    break;
                case "Num6":
                    VKCode = 0x36;
                    break;
                case "Num7":
                    VKCode = 0x37;
                    break;
                case "Num8":
                    VKCode = 0x38;
                    break;
                case "Num9":
                    VKCode = 0x39;
                    break;
                case "Escape":
                    VKCode = 0x1B;
                    break;
                case "LControl":
                    VKCode = 0xA2;
                    break;
                case "LShift":
                    VKCode = 0xA0;
                    break;
                case "LAlt":
                    VKCode = 0xA4;
                    break;
                case "LSystem":
                    VKCode = 0x5B;
                    break;
                case "RControl":
                    VKCode = 0xA3;
                    break;
                case "RShift":
                    VKCode = 0xA1;
                    break;
                case "RAlt":
                    VKCode = 0xA5;
                    break;
                case "RSystem":
                    VKCode = 0x5C;
                    break;
                case "Menu":
                    VKCode = 0x5D;
                    break;
                case "LBracket":
                    VKCode = 0xDB;
                    break;
                case "RBracket":
                    VKCode = 0xDD;
                    break;
                case "Semicolon":
                    VKCode = 0xBA;
                    break;
                case "Comma":
                    VKCode = 0xBC;
                    break;
                case "Period":
                    VKCode = 0xBE;
                    break;
                case "Quote":
                    VKCode = 0xDE;
                    break;
                case "Slash":
                    VKCode = 0xBF;
                    break;
                case "Backslash":
                    VKCode = 0xDC;
                    break;
                case "Tilde":
                    VKCode = 0xC0;
                    break;
                case "Equal":
                    VKCode = 0xBB;
                    break;
                case "Hyphen":
                    VKCode = 0xBD;
                    break;
                case "Space":
                    VKCode = 0x20;
                    break;
                case "Enter":
                    VKCode = 0x0D;
                    break;
                case "Backspace":
                    VKCode = 0x08;
                    break;
                case "Tab":
                    VKCode = 0x09;
                    break;
                case "PageUp":
                    VKCode = 0x21;
                    break;
                case "PageDown":
                    VKCode = 0x22;
                    break;
                case "End":
                    VKCode = 0x23;
                    break;
                case "Home":
                    VKCode = 0x24;
                    break;
                case "Insert":
                    VKCode = 0x2D;
                    break;
                case "Delete":
                    VKCode = 0x2E;
                    break;
                case "Add":
                    VKCode = 0x6B;
                    break;
                case "Subtract":
                    VKCode = 0x6D;
                    break;
                case "Multiply":
                    VKCode = 0x6A;
                    break;
                case "Divide":
                    VKCode = 0x6F;
                    break;
                case "Left":
                    VKCode = 0x25;
                    break;
                case "Right":
                    VKCode = 0x27;
                    break;
                case "Up":
                    VKCode = 0x26;
                    break;
                case "Down":
                    VKCode = 0x28;
                    break;
                case "Numpad0":
                    VKCode = 0x60;
                    break;
                case "Numpad1":
                    VKCode = 0x61;
                    break;
                case "Numpad2":
                    VKCode = 0x62;
                    break;
                case "Numpad3":
                    VKCode = 0x63;
                    break;
                case "Numpad4":
                    VKCode = 0x64;
                    break;
                case "Numpad5":
                    VKCode = 0x65;
                    break;
                case "Numpad6":
                    VKCode = 0x66;
                    break;
                case "Numpad7":
                    VKCode = 0x67;
                    break;
                case "Numpad8":
                    VKCode = 0x68;
                    break;
                case "Numpad9":
                    VKCode = 0x69;
                    break;
                case "F1":
                    VKCode = 0x70;
                    break;
                case "F2":
                    VKCode = 0x71;
                    break;
                case "F3":
                    VKCode = 0x72;
                    break;
                case "F4":
                    VKCode = 0x73;
                    break;
                case "F5":
                    VKCode = 0x74;
                    break;
                case "F6":
                    VKCode = 0x75;
                    break;
                case "F7":
                    VKCode = 0x76;
                    break;
                case "F8":
                    VKCode = 0x77;
                    break;
                case "F9":
                    VKCode = 0x78;
                    break;
                case "F10":
                    VKCode = 0x79;
                    break;
                case "F11":
                    VKCode = 0x7A;
                    break;
                case "F12":
                    VKCode = 0x7B;
                    break;
                case "F13":
                    VKCode = 0x7C;
                    break;
                case "F14":
                    VKCode = 0x7D;
                    break;
                case "F15":
                    VKCode = 0x7E;
                    break;
                case "Pause":
                    VKCode = 0x13;
                    break;
                case "KeyCount":
                    VKCode = 0x65;
                    break;
                case "Dash":
                    VKCode = 0xBD;
                    break;
                case "BackSpace":
                    VKCode = 0x08;
                    break;
                case "Return":
                    VKCode = 0x0D;
                    break;
                case "BackSlash":
                    VKCode = 0xDC;
                    break;
                case "SemiColon":
                    VKCode = 0xBA;
                    break;
                default: {
                        VKCode = Encoding.ASCII.GetBytes(key)[0];
                        break;
                    }
            }

            VKCode = ConvertCharToVirtualKey((char)VKCode);

            //if (!Enum.TryParse(key, out KeyboardKey))
            //{
            //    if (KeyLetter[0] == 'm')
            //    {
            //        KeyLetter = KeyLetter.Remove(0, 1);
            //    }
            //    if (Enum.TryParse(key.Substring(1), out MouseButton))
            //    //if(!Enum.TryParse(key, out MouseButton))
            //    {
            //        //KeyLetter = key.Substring(1);
            //        isKey = false;
            //    }
            //    else
            //    {
            //        string exceptName = "Invalid key " + key;
            //        throw new InvalidOperationException(exceptName);
            //    }

            //}
        }
    }
}