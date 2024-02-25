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
                    KeyboardKey = Keyboard.Key.Num0;
                    break;
                case "Num1":
                    VKCode = 0x31;
                    KeyboardKey = Keyboard.Key.Num1;
                    break;
                case "Num2":
                    VKCode = 0x32;
                    KeyboardKey = Keyboard.Key.Num2;
                    break;
                case "Num3":
                    VKCode = 0x33;
                    KeyboardKey = Keyboard.Key.Num3;
                    break;
                case "Num4":
                    VKCode = 0x34;
                    KeyboardKey = Keyboard.Key.Num4;
                    break;
                case "Num5":
                    VKCode = 0x35;
                    KeyboardKey = Keyboard.Key.Num5;
                    break;
                case "Num6":
                    VKCode = 0x36;
                    KeyboardKey = Keyboard.Key.Num6;
                    break;
                case "Num7":
                    VKCode = 0x37;
                    KeyboardKey = Keyboard.Key.Num7;
                    break;
                case "Num8":
                    VKCode = 0x38;
                    KeyboardKey = Keyboard.Key.Num8;
                    break;
                case "Num9":
                    VKCode = 0x39;
                    KeyboardKey = Keyboard.Key.Num9;
                    break;
                case "Escape":
                    VKCode = 0x1B;
                    KeyboardKey = Keyboard.Key.Escape;
                    break;
                case "LControl":
                    VKCode = 0xA2;
                    KeyboardKey = Keyboard.Key.LControl;
                    break;
                case "LShift":
                    VKCode = 0xA0;
                    KeyboardKey = Keyboard.Key.LShift;
                    break;
                case "LAlt":
                    VKCode = 0xA4;
                    KeyboardKey = Keyboard.Key.LAlt;
                    break;
                case "LSystem":
                    VKCode = 0x5B;
                    KeyboardKey = Keyboard.Key.LSystem;
                    break;
                case "RControl":
                    VKCode = 0xA3;
                    KeyboardKey = Keyboard.Key.RControl;
                    break;
                case "RShift":
                    VKCode = 0xA1;
                    KeyboardKey = Keyboard.Key.RShift;
                    break;
                case "RAlt":
                    VKCode = 0xA5;
                    KeyboardKey = Keyboard.Key.RAlt;
                    break;
                case "RSystem":
                    VKCode = 0x5C;
                    KeyboardKey = Keyboard.Key.RSystem;
                    break;
                case "Menu":
                    VKCode = 0x5D;
                    KeyboardKey = Keyboard.Key.Menu;
                    break;
                case "[":
                case "{":
                case "LBracket":
                    VKCode = 0xDB;
                    KeyboardKey = Keyboard.Key.LBracket;
                    break;
                case "]":
                case "}":
                case "RBracket":
                    VKCode = 0xDD;
                    KeyboardKey = Keyboard.Key.RBracket;
                    break;
                case ";":
                case "Semicolon":
                    VKCode = 0xBA;
                    KeyboardKey = Keyboard.Key.Semicolon;
                    break;
                case ",":
                case "Comma":
                    VKCode = 0xBC;
                    KeyboardKey = Keyboard.Key.Comma;
                    break;
                case ".":
                case "Period":
                    VKCode = 0xBE;
                    KeyboardKey = Keyboard.Key.Period;
                    break;
                case "\"":
                case "Quote":
                    VKCode = 0xDE;
                    KeyboardKey = Keyboard.Key.Quote;
                    break;
                case "/":
                case "Slash":
                    VKCode = 0xBF;
                    KeyboardKey = Keyboard.Key.Slash;
                    break;
                case "\\":
                case "Backslash":
                    VKCode = 0xDC;
                    KeyboardKey = Keyboard.Key.Backslash;
                    break;
                case "~":
                case "Tilde":
                    VKCode = 0xC0;
                    KeyboardKey = Keyboard.Key.Tilde;
                    break;
                case "=":
                case "Equal":
                    VKCode = 0xBB;
                    KeyboardKey = Keyboard.Key.Equal;
                    break;
                case "-":
                case "Hyphen":
                    VKCode = 0xBD;
                    KeyboardKey = Keyboard.Key.Hyphen;
                    break;
                case " ":
                case "Space":
                    VKCode = 0x20;
                    KeyboardKey = Keyboard.Key.Space;
                    break;
                case "Enter":
                    VKCode = 0x0D;
                    KeyboardKey = Keyboard.Key.Enter;
                    break;
                case "Backspace":
                    VKCode = 0x08;
                    KeyboardKey = Keyboard.Key.Backspace;
                    break;
                case "  ":
                case "Tab":
                    VKCode = 0x09;
                    KeyboardKey = Keyboard.Key.Tab;
                    break;
                case "PageUp":
                    VKCode = 0x21;
                    KeyboardKey = Keyboard.Key.PageUp;
                    break;
                case "PageDown":
                    VKCode = 0x22;
                    KeyboardKey = Keyboard.Key.PageDown;
                    break;
                case "End":
                    VKCode = 0x23;
                    KeyboardKey = Keyboard.Key.End;
                    break;
                case "Home":
                    VKCode = 0x24;
                    KeyboardKey = Keyboard.Key.Home;
                    break;
                case "Insert":
                    VKCode = 0x2D;
                    KeyboardKey = Keyboard.Key.Insert;
                    break;
                case "Delete":
                    VKCode = 0x2E;
                    KeyboardKey = Keyboard.Key.Delete;
                    break;
                case "+":
                case "Add":
                    VKCode = 0x6B;
                    KeyboardKey = Keyboard.Key.Add;
                    break;
                case "Subtract":
                    VKCode = 0x6D;
                    KeyboardKey = Keyboard.Key.Subtract;
                    break;
                case "*":
                case "Multiply":
                    VKCode = 0x6A;
                    KeyboardKey = Keyboard.Key.Multiply;
                    break;
                case "Divide":
                    VKCode = 0x6F;
                    KeyboardKey = Keyboard.Key.Divide;
                    break;
                case "Left":
                    VKCode = 0x25;
                    KeyboardKey = Keyboard.Key.Left;
                    break;
                case "Right":
                    VKCode = 0x27;
                    KeyboardKey = Keyboard.Key.Right;
                    break;
                case "Up":
                    VKCode = 0x26;
                    KeyboardKey = Keyboard.Key.Up;
                    break;
                case "Down":
                    VKCode = 0x28;
                    KeyboardKey = Keyboard.Key.Down;
                    break;
                case "Numpad0":
                    VKCode = 0x60;
                    KeyboardKey = Keyboard.Key.Numpad0;
                    break;
                case "Numpad1":
                    VKCode = 0x61;
                    KeyboardKey = Keyboard.Key.Numpad1;
                    break;
                case "Numpad2":
                    VKCode = 0x62;
                    KeyboardKey = Keyboard.Key.Numpad2;
                    break;
                case "Numpad3":
                    VKCode = 0x63;
                    KeyboardKey = Keyboard.Key.Numpad3;
                    break;
                case "Numpad4":
                    VKCode = 0x64;
                    KeyboardKey = Keyboard.Key.Numpad4;
                    break;
                case "Numpad5":
                    VKCode = 0x65;
                    KeyboardKey = Keyboard.Key.Numpad5;
                    break;
                case "Numpad6":
                    VKCode = 0x66;
                    KeyboardKey = Keyboard.Key.Numpad6;
                    break;
                case "Numpad7":
                    VKCode = 0x67;
                    KeyboardKey = Keyboard.Key.Numpad7;
                    break;
                case "Numpad8":
                    VKCode = 0x68;
                    KeyboardKey = Keyboard.Key.Numpad8;
                    break;
                case "Numpad9":
                    VKCode = 0x69;
                    KeyboardKey = Keyboard.Key.Numpad9;
                    break;
                case "F1":
                    VKCode = 0x70;
                    KeyboardKey = Keyboard.Key.F1;
                    break;
                case "F2":
                    VKCode = 0x71;
                    KeyboardKey = Keyboard.Key.F2;
                    break;
                case "F3":
                    VKCode = 0x72;
                    KeyboardKey = Keyboard.Key.F3;
                    break;
                case "F4":
                    VKCode = 0x73;
                    KeyboardKey = Keyboard.Key.F4;
                    break;
                case "F5":
                    VKCode = 0x74;
                    KeyboardKey = Keyboard.Key.F5;
                    break;
                case "F6":
                    VKCode = 0x75;
                    KeyboardKey = Keyboard.Key.F6;
                    break;
                case "F7":
                    VKCode = 0x76;
                    KeyboardKey = Keyboard.Key.F7;
                    break;
                case "F8":
                    VKCode = 0x77;
                    KeyboardKey = Keyboard.Key.F8;
                    break;
                case "F9":
                    VKCode = 0x78;
                    KeyboardKey = Keyboard.Key.F9;
                    break;
                case "F10":
                    VKCode = 0x79;
                    KeyboardKey = Keyboard.Key.F10;
                    break;
                case "F11":
                    VKCode = 0x7A;
                    KeyboardKey = Keyboard.Key.F11;
                    break;
                case "F12":
                    VKCode = 0x7B;
                    KeyboardKey = Keyboard.Key.F12;
                    break;
                case "F13":
                    VKCode = 0x7C;
                    KeyboardKey = Keyboard.Key.F13;
                    break;
                case "F14":
                    VKCode = 0x7D;
                    KeyboardKey = Keyboard.Key.F14;
                    break;
                case "F15":
                    VKCode = 0x7E;
                    KeyboardKey = Keyboard.Key.F15;
                    break;
                case "Pause":
                    VKCode = 0x13;
                    KeyboardKey = Keyboard.Key.Pause;
                    break;
                case "KeyCount":
                    VKCode = 0x65;
                    KeyboardKey = Keyboard.Key.KeyCount;
                    break;
                case "Dash":
                    VKCode = 0xBD;
                    KeyboardKey = Keyboard.Key.Hyphen;
                    break;
                case "BackSpace":
                    VKCode = 0x08;
                    KeyboardKey = Keyboard.Key.Backspace;
                    break;
                case "Return":
                    VKCode = 0x0D;
                    KeyboardKey = Keyboard.Key.Enter;
                    break;
                case "BackSlash":
                    VKCode = 0xDC;
                    KeyboardKey = Keyboard.Key.Backslash;
                    break;
                case "SemiColon":
                    VKCode = 0xBA;
                    KeyboardKey = Keyboard.Key.Semicolon;
                    break;
                default: {
                        VKCode = Encoding.ASCII.GetBytes(key.ToUpper())[0];
                        VKCode = ConvertCharToVirtualKey((char)VKCode);
                        if (Enum.TryParse(key.ToUpper(), out Keyboard.Key result)) {
                            KeyboardKey = result;
                        }
                        break;
                    }
            }

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