using System;
using System.Collections.Generic;
using System.Text;

namespace hexnyan.eeprom
{
    // Padding
    class PHbe : BasicElement
    {
        private int FieldSize = 0;

        public PHbe(int Size, ushort Value)
        {
            FieldSize = Size;
            InternalData = new byte[Size * 2];
            for (int i = 0; i < FieldSize; i++)
            {
                InternalData[i * 2] = Convert.ToByte((Value >> 8) & 0xFF);
                InternalData[i * 2 + 1] = Convert.ToByte(Value & 0xFF);
            }
        }

        public PHbe(int Size, ushort Value, ushort[] Values)
        {
            FieldSize = Size;
            InternalData = new byte[Size * 2];
            for (int i = 0; i < FieldSize; i++)
            {
                var Val = (i < Values.Length) ? Values[i] : Value;
                InternalData[i * 2] = Convert.ToByte((Val >> 8) & 0xFF);
                InternalData[i * 2 + 1] = Convert.ToByte(Val & 0xFF);
            }
        }

        public new string Field
        {
            get { return String.Format("PH{0:d}", FieldSize); }
        }
    }
}
