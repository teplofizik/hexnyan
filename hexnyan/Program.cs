using System;
using System.Collections.Generic;
using System.Text;

namespace hexnyan
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Intel HEX and binary file generator v1.1 [dev.]");

            IDBase.Load();

            ParseArgs(args);
            if (args.Length == 0) ShowHelp();
        }

        static void ShowHelp()
        {
            Console.WriteLine("Help:");
            Console.WriteLine("Intel HEX and binary file generator");
            Console.WriteLine("");
            Console.WriteLine("-n - generate new file:");
            Console.WriteLine("  H=<value> - byte in hex representation (H=A5);");
            Console.WriteLine("  u8=<value> - unsigned byte (u8=12);");
            Console.WriteLine("  u16=<value> - unsigned short (u16=1234);");
            Console.WriteLine("  u32=<value> - unsigned long (u32=123456);");
            Console.WriteLine("  u48=<value> - unsigned 6 bytes width number (u48=12345678);");
            Console.WriteLine("  u64=<value> - unsigned 8 bytes width number (u64=1234567890);");
            Console.WriteLine("  U16=<value> - unsigned short big endian (U16=1234);");
            Console.WriteLine("  U32=<value> - unsigned long big endian (U32=123456);");
            Console.WriteLine("  U48=<value> - unsigned 6 bytes width number big endian(U48=12345678);");
            Console.WriteLine("  U64=<value> - unsigned 8 bytes width number big endian(U64=1234567890);");
            Console.WriteLine("  MAC=<value> - mac-address (MAC=00:22:33:AA:BB:FF);");
            Console.WriteLine("  IP4=<value> - ipv4-address (IP4=123.122.0.12);");
            Console.WriteLine("  P<count>=<value> - fill 'count' of bytes with value in hex representation (P6=FF);");
            Console.WriteLine("  PH<count>=<value> - fill 'count' of u16 with value in hex representation, BE (PH6=FFFF);"); ;
            Console.WriteLine("  Ph<count>=<value> - fill 'count' of u16 with value in hex representation, LE (Ph6=FFFF);");
            Console.WriteLine("  PW<count>=<value> - fill 'count' of u32 with value in hex representation, BE (PW6=FFFFFFFF);"); ;
            Console.WriteLine("  Pw<count>=<value> - fill 'count' of u32 with value in hex representation, LE (Pw6=FFFFFFFF);");
            Console.WriteLine("  X<count>=<value> - fill 'count' of bytes with hex value (X6=AABBCCDDEEFF);");
            Console.WriteLine("  S<count>=<value> - fill 'count' of bytes with string (S32=nyanyanya);");
            Console.WriteLine("");
            Console.WriteLine("-m<file> - modify data of xml file file:");
            Console.WriteLine("  <field name>=<new value> - replace default value of field by specified value");
            Console.WriteLine("");

            Console.WriteLine("-r - reset id fields to specified values");
            Console.WriteLine("  <field>=<value>");
            Console.WriteLine("");  

            Console.WriteLine("-s<file> - save result to specified file:");
            Console.WriteLine("  format=[bin|hex] - file format (binary or Intel HEX)U;");
            Console.WriteLine("  offset=0xXXXXXXXX - data offset;");
        }

        static void ParseArgs(string[] args)
        {
            ArgumentParser Parser = new ArgumentParser(args);
            List<Argument> ArgList = Parser.Arguments;
            byte[] Data = null;

            for (int i = 0; i < ArgList.Count; i++)
            {
                switch (ArgList[i].Name)
                {
                    case "n":
                        Data = GenerateNew(ArgList[i].Value, ArgList[i].Arguments);
                        break;
                    case "m":
                        Data = Modify(ArgList[i].Value, ArgList[i].Arguments);
                        break;
                    case "s":
                        Save(ArgList[i].Value, ArgList[i].Arguments, Data);
                        break;
                    case "r":
                        Reset(ArgList[i].Value, ArgList[i].Arguments);
                        break;
                }
            }
        }

        static int GetFieldWidth(string Name)
        {
            int W = 0;

            try
            {
                if ((Name[0] == 'P') && (Name[1] > '9'))
                {
                    //int Mul = (Name[1] == 'H') ? 2 : ((Name[1] == 'W') ? 4 : 1);
                    W = Convert.ToInt16(Name.Substring(2));
                }
                else
                    W = Convert.ToInt16(Name.Substring(1));
            }
            catch { }

            return W;
        }

        static void Reset(string Argument, List<Argument> ArgList)
        {
            foreach (Argument A in ArgList)
            {
                Int64 Default = 0;

                try
                {
                    Default = Convert.ToInt64(A.Value);
                }
                catch { }

                IDBase.SetValue(A.Name, Default);
            }
        }

        static int GetOffset(string Value)
        {
            try
            {
                if ("0x".CompareTo(Value.Substring(0, 2)) == 0)
                    return Convert.ToInt32(Value.Substring(2), 16);
                else
                    return Convert.ToInt32(Value);
            }
            catch(Exception E)
            {
                Console.WriteLine("Error: Invalid offset value (" + E.Message + ")");

                return 0;
            }
        }

        static void Save(string Argument, List<Argument> ArgList, byte[] Data)
        {
            string FileName = Argument;
            string FileType = "bin";
            string Offset = "0x00000000";

            if (Data == null) return;

            foreach (Argument A in ArgList)
            {
                string Value = A.Value;
                switch (A.Name)
                {
                    case "format": FileType = Value; break;
                    case "offset": Offset = Value.Trim(); break;
                }
            }

            switch (FileType)
            {
                case "hex":
                case "ihex":
                    parser.Output.IntelHex(FileName, Data, GetOffset(Offset));
                    break;
                case "bin":
                default:
                    parser.Output.Binary(FileName, Data, GetOffset(Offset));
                    break;
            }
        }

        static byte[] Modify(string Argument, List<Argument> ArgList)
        {
            List<parser.PreparsedElement> Preparsed = parser.Parser.Load(Argument);
            if (Preparsed == null) return null;

            foreach (Argument A in ArgList)
            {
                for (int i = 0; i < Preparsed.Count; i++)
                {
                    parser.PreparsedElement PE = Preparsed[i];

                    if(PE.Name.Length == 0) continue;
                    if (PE.Name.CompareTo(A.Name) == 0) PE.Value = A.Value;
                }
            }

            List<eeprom.Element> Elements = parser.Parser.Parse(Preparsed);
            return parser.Linker.Link(Elements);
        }

        static byte[] GenerateNew(string Argument, List<Argument> ArgList)
        {
            List<parser.PreparsedElement> Preparsed = new List<parser.PreparsedElement>();

            foreach (Argument A in ArgList)
            {
                string Value = A.Value;
                switch (A.Name)
                {
                    case "H": Preparsed.Add(new parser.PreparsedElement("H", Value.Substring(0, 2))); break;
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
                        Preparsed.Add(new parser.PreparsedElement(A.Name, Value));
                        break;
                    default:
                        switch (A.Name[0])
                        {
                            case 'S': // string
                            case 'X': // hex
                                Preparsed.Add(new parser.PreparsedElement(A.Name[0] + "", Value, GetFieldWidth(A.Name)));
                                break;
                            case 'P': // padding
                                {
                                    switch(A.Name[1])
                                    {
                                        case 'B': // Byte
                                            Preparsed.Add(new parser.PreparsedElement(A.Name[0] + "" + A.Name[1], Value, GetFieldWidth(A.Name)));
                                            break;
                                        case 'H': // Halfword
                                        case 'h': // Halfword
                                            Preparsed.Add(new parser.PreparsedElement(A.Name[0] + "" + A.Name[1], Value, GetFieldWidth(A.Name)));
                                            break;
                                        case 'W': // Word
                                        case 'w': // Word
                                            Preparsed.Add(new parser.PreparsedElement(A.Name[0] + "" + A.Name[1], Value, GetFieldWidth(A.Name)));
                                            break;
                                        default:
                                            Preparsed.Add(new parser.PreparsedElement(A.Name[0] + "", Value, GetFieldWidth(A.Name)));
                                            break;
                                    }
                                }
                                break;
                        }
                        break;
                }
            }

            List<eeprom.Element> Elements = parser.Parser.Parse(Preparsed);
            return parser.Linker.Link(Elements);
        }
    }
}
