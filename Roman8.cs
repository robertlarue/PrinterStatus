using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterStatus
{
    class Roman8
    {
        public static string ConvertFromRoman8(string hex)
        {
            List<char> outputList = new List<char>();
            byte[] inputBytes = StringToByteArrayFastest(hex);
            foreach(byte roman8char in inputBytes)
            {
                switch (roman8char)
                {
                    case 0x20:
                        outputList.Add(' ');
                        break;
                    case 0x21:
                        outputList.Add('!');
                        break;
                    case 0x22:
                        outputList.Add('"');
                        break;
                    case 0x23:
                        outputList.Add('#');
                        break;
                    case 0x24:
                        outputList.Add('$');
                        break;
                    case 0x25:
                        outputList.Add('%');
                        break;
                    case 0x26:
                        outputList.Add('&');
                        break;
                    case 0x27:
                        outputList.Add('\'');
                        break;
                    case 0x28:
                        outputList.Add('(');
                        break;
                    case 0x29:
                        outputList.Add(')');
                        break;
                    case 0x2A:
                        outputList.Add('*');
                        break;
                    case 0x2B:
                        outputList.Add('+');
                        break;
                    case 0x2C:
                        outputList.Add(',');
                        break;
                    case 0x2D:
                        outputList.Add('-');
                        break;
                    case 0x2E:
                        outputList.Add('.');
                        break;
                    case 0x2F:
                        outputList.Add('/');
                        break;
                    case 0x30:
                        outputList.Add('0');
                        break;
                    case 0x31:
                        outputList.Add('1');
                        break;
                    case 0x32:
                        outputList.Add('2');
                        break;
                    case 0x33:
                        outputList.Add('3');
                        break;
                    case 0x34:
                        outputList.Add('4');
                        break;
                    case 0x35:
                        outputList.Add('5');
                        break;
                    case 0x36:
                        outputList.Add('6');
                        break;
                    case 0x37:
                        outputList.Add('7');
                        break;
                    case 0x38:
                        outputList.Add('8');
                        break;
                    case 0x39:
                        outputList.Add('9');
                        break;
                    case 0x3A:
                        outputList.Add(':');
                        break;
                    case 0x3B:
                        outputList.Add(';');
                        break;
                    case 0x3C:
                        outputList.Add('<');
                        break;
                    case 0x3D:
                        outputList.Add('=');
                        break;
                    case 0x3E:
                        outputList.Add('>');
                        break;
                    case 0x3F:
                        outputList.Add('?');
                        break;
                    case 0x40:
                        outputList.Add('@');
                        break;
                    case 0x41:
                        outputList.Add('A');
                        break;
                    case 0x42:
                        outputList.Add('B');
                        break;
                    case 0x43:
                        outputList.Add('C');
                        break;
                    case 0x44:
                        outputList.Add('D');
                        break;
                    case 0x45:
                        outputList.Add('E');
                        break;
                    case 0x46:
                        outputList.Add('F');
                        break;
                    case 0x47:
                        outputList.Add('G');
                        break;
                    case 0x48:
                        outputList.Add('H');
                        break;
                    case 0x49:
                        outputList.Add('I');
                        break;
                    case 0x4A:
                        outputList.Add('J');
                        break;
                    case 0x4B:
                        outputList.Add('K');
                        break;
                    case 0x4C:
                        outputList.Add('L');
                        break;
                    case 0x4D:
                        outputList.Add('M');
                        break;
                    case 0x4E:
                        outputList.Add('N');
                        break;
                    case 0x4F:
                        outputList.Add('O');
                        break;
                    case 0x50:
                        outputList.Add('P');
                        break;
                    case 0x51:
                        outputList.Add('Q');
                        break;
                    case 0x52:
                        outputList.Add('R');
                        break;
                    case 0x53:
                        outputList.Add('S');
                        break;
                    case 0x54:
                        outputList.Add('T');
                        break;
                    case 0x55:
                        outputList.Add('U');
                        break;
                    case 0x56:
                        outputList.Add('V');
                        break;
                    case 0x57:
                        outputList.Add('W');
                        break;
                    case 0x58:
                        outputList.Add('X');
                        break;
                    case 0x59:
                        outputList.Add('Y');
                        break;
                    case 0x5A:
                        outputList.Add('Z');
                        break;
                    case 0x5B:
                        outputList.Add('[');
                        break;
                    case 0x5C:
                        outputList.Add('\\');
                        break;
                    case 0x5D:
                        outputList.Add(']');
                        break;
                    case 0x5E:
                        outputList.Add('^');
                        break;
                    case 0x5F:
                        outputList.Add('_');
                        break;
                    case 0x60:
                        outputList.Add('`');
                        break;
                    case 0x61:
                        outputList.Add('a');
                        break;
                    case 0x62:
                        outputList.Add('b');
                        break;
                    case 0x63:
                        outputList.Add('c');
                        break;
                    case 0x64:
                        outputList.Add('d');
                        break;
                    case 0x65:
                        outputList.Add('e');
                        break;
                    case 0x66:
                        outputList.Add('f');
                        break;
                    case 0x67:
                        outputList.Add('g');
                        break;
                    case 0x68:
                        outputList.Add('h');
                        break;
                    case 0x69:
                        outputList.Add('i');
                        break;
                    case 0x6A:
                        outputList.Add('j');
                        break;
                    case 0x6B:
                        outputList.Add('k');
                        break;
                    case 0x6C:
                        outputList.Add('l');
                        break;
                    case 0x6D:
                        outputList.Add('m');
                        break;
                    case 0x6E:
                        outputList.Add('n');
                        break;
                    case 0x6F:
                        outputList.Add('o');
                        break;
                    case 0x70:
                        outputList.Add('p');
                        break;
                    case 0x71:
                        outputList.Add('q');
                        break;
                    case 0x72:
                        outputList.Add('r');
                        break;
                    case 0x73:
                        outputList.Add('s');
                        break;
                    case 0x74:
                        outputList.Add('t');
                        break;
                    case 0x75:
                        outputList.Add('u');
                        break;
                    case 0x76:
                        outputList.Add('v');
                        break;
                    case 0x77:
                        outputList.Add('w');
                        break;
                    case 0x78:
                        outputList.Add('x');
                        break;
                    case 0x79:
                        outputList.Add('y');
                        break;
                    case 0x7A:
                        outputList.Add('z');
                        break;
                    case 0x7B:
                        outputList.Add('{');
                        break;
                    case 0x7C:
                        outputList.Add('|');
                        break;
                    case 0x7D:
                        outputList.Add('}');
                        break;
                    case 0x7E:
                        outputList.Add('~');
                        break;
                    case 0x7F:
                        outputList.Add(' ');
                        break;
                }
            }
            return string.Concat(outputList.ToArray());
        }
        public static byte[] StringToByteArrayFastest(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }
        public static int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            //return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }
    }
}
