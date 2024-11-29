using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace hexnyan.parser
{
    class Output
    {
        static public void Binary(string FileName, byte[] Data, int Offset)
        {
            FileStream fs = File.Create(FileName, 128, FileOptions.None);
            BinaryWriter bw = new BinaryWriter(fs);

            // Смещение
            if(Offset > 0)
            {
                byte[] Temp = new byte[Offset];
                for (int i = 0; i < Offset; i++) Temp[i] = 0xFF;
                bw.Write(Temp);
            }

            bw.Write(Data);

            bw.Close();
            fs.Close();
        }

        static string IntelHexLine(byte[] Data, int Line, int Offset)
        {
            const int LineWidth = 16;
            int DataStart = Line * LineWidth & 0xFFFF;
            int Start = (DataStart + Offset) & 0xFFFF;
            if (DataStart > Data.Length) return null;
            int End = DataStart + LineWidth - 1;
            if (End >= Data.Length) End = Data.Length - 1;

            int Length = End - DataStart + 1;

            string Result = "";
            byte Checksum = 0;

            Checksum += Convert.ToByte(Length & 0xFF);
            Checksum += Convert.ToByte((Start >> 8) & 0xFF);
            Checksum += Convert.ToByte((Start >> 0) & 0xFF);

            // Start
            Result += String.Format(":{0:X02}{1:X04}{2:X02}", Length, Start, 0);

            // Data
            for (int i = 0; i < Length; i++)
            {
                Checksum += Data[DataStart + i];
                Result += String.Format("{0:X02}", Data[DataStart + i]);
            }

            // End
            Result += String.Format("{0:X02}", (byte)(0x00 - Checksum));

            return Result;
        }


        static public void IntelHex(string FileName, byte[] Data, int Offset)
        {
            List<string> Out = new List<string>();

            int Line = 0;
            // :020000040800F2 - offset, hi nimble: 0x0800xxxx
            if (Offset > 0xFFFF)
            {
                byte Checksum = 6;
                Checksum += Convert.ToByte((Offset >> 16) & 0xFF);
                Checksum += Convert.ToByte((Offset >> 24) & 0xFF);

                // High nimble of address
                Out.Add(String.Format(":02000004{0:X04}{1:X02}", (Offset >> 16) & 0xFFFF, (byte)(0x00 - Checksum)));
            }
            while (true)
            {
                string L = IntelHexLine(Data, Line++, Offset);
                if (L == null) break;

                Out.Add(L);
            }
            // EOF
            Out.Add(":00000001FF");

            using (StreamWriter SW = new StreamWriter(FileName))
            {
                foreach (string O in Out) SW.WriteLine(O);
            }
        }
    }
}
