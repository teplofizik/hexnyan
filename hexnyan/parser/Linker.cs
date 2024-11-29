using System;
using System.Collections.Generic;
using System.Text;

namespace hexnyan.parser
{
    class Linker
    {
        static public byte[] Link(List<eeprom.Element> Elements)
        {
            List<byte> Temp = new List<byte>();

            foreach (eeprom.Element E in Elements)
            {
                byte[] T = E.Data;
                if (T == null) continue;

                for (int i = 0; i < T.Length; i++) Temp.Add(T[i]);
            }

            return Temp.ToArray();
        }
    }
}
