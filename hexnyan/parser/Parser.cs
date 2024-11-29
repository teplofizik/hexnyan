using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Security;

using Extension.Network;
using System.Net;

namespace hexnyan.parser
{
    class Parser
    {
        static string ParseStringKeyCode(string Value)
        {
            switch (Value)
            {
                case "{user}": return System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                default: return null;
            }
        }

        static Int64 ParseKeyCode(string Value)
        {
            if (Value == null) return Int64.MinValue;
            if (Value.IndexOf("{auto:") == 0)
            {
                // Это поле автоинкремента
                int Index = Value.IndexOf(':', 6);
                if (Index < 0) return Int64.MinValue;
                int EndIndex = Value.IndexOf('}', Index);
                if (EndIndex < 0) return Int64.MinValue;

                string FieldName = Value.Substring(6, Index - 6);
                string DefaultValue = Value.Substring(Index + 1, EndIndex - Index - 1);

                Int64 Default = 0;
                try
                {
                    Default = Convert.ToInt64(DefaultValue);
                }
                catch { }

                return IDBase.GetValue(FieldName, Default);
            }

            switch (Value)
            {
                case "{day}": return DateTime.Now.Day;
                case "{month}": return DateTime.Now.Month;
                case "{year}": return DateTime.Now.Year;
                case "{yearshort}": return DateTime.Now.Year % 100;

                default: return Int64.MinValue;
            }
        }

        static byte ParseU8Hex(string Value)
        {
            byte T = 0;
            Int64 Key;

            if ((Key = ParseKeyCode(Value)) != Int64.MinValue)
            {
                if (Key > byte.MaxValue) Key = byte.MaxValue;
                if (Key < byte.MinValue) Key = byte.MinValue;

                return Convert.ToByte(Key);
            }
            
            try
            {
                T = Convert.ToByte(Value, 16);
            }
            catch { }

            return T;
        }

        static byte ParseU8(string Value)
        {
            byte T = 0;
            Int64 Key;

            if ((Key = ParseKeyCode(Value)) != Int64.MinValue)
            {
                if (Key > byte.MaxValue) Key = byte.MaxValue;
                if (Key < byte.MinValue) Key = byte.MinValue;

                return Convert.ToByte(Key);
            }

            if (Value != null)
            {
                try
                {
                    if ((Value.Length > 2) && ("0x".CompareTo(Value.Substring(0, 2)) == 0))
                        T = Convert.ToByte(Value.Substring(2), 16);
                    else
                        T = Convert.ToByte(Value);
                }
                catch { }
            }
            return T;
        }

        static byte[] ParseMAC(string Value)
        {
            MACAddress val;
            if (MACAddress.TryParse(Value, out val))
                return val.Bytes;
            else
                return new byte[6] { 0, 0, 0, 0, 0, 0 };
        }

        static byte[] ParseIP(string Value)
        {
            IPAddress val;
            if (IPAddress.TryParse(Value, out val) && (val.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
                return val.GetAddressBytes();
            else
                return new byte[4] { 0, 0, 0, 0 };
        }

        static UInt16 ParseU16(string Value)
        {
            UInt16 T = 0;
            Int64 Key;

            if ((Key = ParseKeyCode(Value)) != Int64.MinValue)
            {
                if (Key > UInt16.MaxValue) Key = UInt16.MaxValue;
                if (Key < UInt16.MinValue) Key = UInt16.MinValue;

                return Convert.ToUInt16(Key);
            }

            if (Value != null)
            {
                try
                {
                    if ((Value.Length > 2) && ("0x".CompareTo(Value.Substring(0, 2)) == 0))
                        T = Convert.ToUInt16(Value.Substring(2), 16);
                    else
                        T = Convert.ToUInt16(Value);
                }
                catch { }
            }
            return T;
        }

        static UInt32 ParseU32(string Value)
        {
            UInt32 T = 0;
            Int64 Key;

            if ((Key = ParseKeyCode(Value)) != Int64.MinValue)
            {
                if (Key > UInt32.MaxValue) Key = UInt32.MaxValue;
                if (Key < UInt32.MinValue) Key = UInt32.MinValue;

                return Convert.ToUInt32(Key);
            }

            if (Value != null)
            {
                try
                {
                    if ((Value.Length > 2) && ("0x".CompareTo(Value.Substring(0, 2)) == 0))
                        T = Convert.ToUInt32(Value.Substring(2), 16);
                    else
                        T = Convert.ToUInt32(Value);
                }
                catch { }
            }
            return T;
        }

        static UInt64 ParseU64(string Value)
        {
            UInt64 T = 0;
            Int64 Key;

            if ((Key = ParseKeyCode(Value)) != Int64.MinValue)
            {
                if (Key < 0) Key = 0;

                return Convert.ToUInt64(Key);
            }

            if (Value != null)
            {
                try
                {
                    if ((Value.Length > 2) && ("0x".CompareTo(Value.Substring(0, 2)) == 0))
                        T = Convert.ToUInt64(Value.Substring(2), 16);
                    else
                        T = Convert.ToUInt64(Value);
                }
                catch { }
            }
            return T;
        }

        static byte[] ParseHex(string Value)
        {
            if(Value == null) return null;

            List<byte> R = new List<byte>();
            for (int i = 0; i < Value.Length / 2; i++)
            {
                string Hex = Value.Substring(i * 2, 2);

                R.Add(ParseU8Hex(Hex));
            }

            return (R.Count > 0) ? R.ToArray() : null;
        }

        static string ParseString(string Value)
        {
            string Key = ParseStringKeyCode(Value);

            if (Key != null)
            {
                return Key;
            }
            else
                return Value;
        }

        static T[] ConvArray<T>(string[] Text, Func<string,T> F)
        {
            return Array.ConvertAll(Text, E => F(E));
        }

        static public List<eeprom.Element> Parse(List<PreparsedElement> Elements)
        {
            List<eeprom.Element> Result = new List<eeprom.Element>();

            foreach (PreparsedElement E in Elements)
            {
                if (E.Width == 0) continue;

                switch (E.FieldType)
                {
                    case "H": Result.Add(new eeprom.U8(ParseU8Hex(E.Value))); break;
                    case "U8":
                    case "u8": Result.Add(new eeprom.U8(ParseU8(E.Value))); break;
                    case "u16": Result.Add(new eeprom.U16(ParseU16(E.Value))); break;
                    case "u32": Result.Add(new eeprom.U32(ParseU32(E.Value))); break;
                    case "u48": Result.Add(new eeprom.U48(ParseU64(E.Value))); break;
                    case "u64": Result.Add(new eeprom.U64(ParseU64(E.Value))); break;
                    case "U16": Result.Add(new eeprom.U16be(ParseU16(E.Value))); break;
                    case "U32": Result.Add(new eeprom.U32be(ParseU32(E.Value))); break;
                    case "U48": Result.Add(new eeprom.U48be(ParseU64(E.Value))); break;
                    case "U64": Result.Add(new eeprom.U64be(ParseU64(E.Value))); break;
                    case "S": Result.Add(new eeprom.S(E.Width, ParseString(E.Value))); break;
                    case "X": Result.Add(new eeprom.X(E.Width, ParseHex(E.Value))); break;
                    case "PB": 
                    case "P": Result.Add(new eeprom.P(E.Width, ParseU8(E.Value), ConvArray(E.Values, V => ParseU8(V)))); break;
                    case "PH": Result.Add(new eeprom.PHbe(E.Width, ParseU16(E.Value), ConvArray(E.Values, V => ParseU16(V)))); break;
                    case "Ph": Result.Add(new eeprom.PH(E.Width, ParseU16(E.Value), ConvArray(E.Values, V => ParseU16(V)))); break;
                    case "PW": Result.Add(new eeprom.PWbe(E.Width, ParseU32(E.Value), ConvArray(E.Values, V => ParseU32(V)))); break;
                    case "Pw": Result.Add(new eeprom.PW(E.Width, ParseU32(E.Value), ConvArray(E.Values, V => ParseU32(V)))); break;
                    case "MAC": Result.Add(new eeprom.MAC(ParseMAC(E.Value))); break;
                    case "IP4": Result.Add(new eeprom.IP4(ParseIP(E.Value))); break;
                }
            }

            return Result;
        }

        static int GetFieldWidth(string Name)
        {
            int W = 0;

            try
            {
                if ((Name[0] == 'P') && (Name[1] > '9'))
                {
                    W = Convert.ToInt16(Name.Substring(2));
                }
                else
                    W = Convert.ToInt16(Name.Substring(1));
            }
            catch { }

            return W;
        }

        static private void LoadValues(CONF.XmlLoad X, PreparsedElement E)
        {
            var Vals = new List<string>();
            while (X.Read())
            {
                switch (X.ElementName)
                {
                    case "V": Vals.Add(X.GetValue()); break;
                }
            }
            X.Close();
            E.Values = Vals.ToArray();
        }

        static public List<PreparsedElement> Load(string FileName)
        {
            CONF.XmlLoad X = new CONF.XmlLoad();
            if (!X.Load(FileName)) return null;

            List<PreparsedElement> Elements = new List<PreparsedElement>();

            while (X.Read())
            {
                string Name = X.GetAttribute("name");
                string Default = X.GetAttribute("default");
                string Comment = X.GetAttribute("comment");

                switch(X.ElementName)
                {
                    case "u8":
                    case "u16":
                    case "u32":
                    case "u48":
                    case "u64":
                    case "U8":
                    case "U16":
                    case "U32":
                    case "U48":
                    case "U64":
                    case "MAC":
                    case "IP4":
                        {
                            PreparsedElement E = new PreparsedElement(X.ElementName, Default);
                            E.Name = Name;
                            E.Comment = Comment;
                            Elements.Add(E);
                        }
                        break;
                    default:
                        switch (X.ElementName[0])
                        {
                            case 'S': // string
                            case 'X': // hex
                                {
                                    Elements.Add(new PreparsedElement(X.ElementName[0] + "", 
                                                                              Default, 
                                                                              GetFieldWidth(X.ElementName), 
                                                                              Name, Comment));
                                }
                                break;
                            case 'P': // padding
                                {
                                    switch (X.ElementName[1])
                                    {
                                        case 'B': // Byte
                                            {
                                                PreparsedElement E = new PreparsedElement(X.ElementName[0] + "" + X.ElementName[1], Default, GetFieldWidth(X.ElementName), Name, Comment);
                                                LoadValues(X.GetSubtree(), E);
                                                Elements.Add(E);
                                            }
                                            break;
                                        case 'H': // Halfword
                                        case 'h': // Halfword
                                            {
                                                PreparsedElement E = new PreparsedElement(X.ElementName[0] + "" + X.ElementName[1], Default, GetFieldWidth(X.ElementName), Name, Comment);
                                                LoadValues(X.GetSubtree(), E);
                                                Elements.Add(E);
                                            }
                                            break;
                                        case 'W': // Word
                                        case 'w': // Word
                                            {
                                                PreparsedElement E = new PreparsedElement(X.ElementName[0] + "" + X.ElementName[1], Default, GetFieldWidth(X.ElementName), Name, Comment);
                                                LoadValues(X.GetSubtree(), E);
                                                Elements.Add(E);
                                            }
                                            break;
                                        default:
                                            {
                                                PreparsedElement E = new PreparsedElement(X.ElementName[0] + "", Default, GetFieldWidth(X.ElementName), Name, Comment);
                                                LoadValues(X.GetSubtree(), E);
                                                Elements.Add(E);
                                            }
                                            break;
                                    }
                                }
                                break;
                        }
                        break;
                }
            }

            X.Close();

            return Elements;
        }
    }
}
