using System;
using System.Collections.Generic;
using System.Text;

namespace hexnyan.eeprom
{
    // Padding
    class PWbe : BasicElement
    {
        private int FieldSize = 0;

        public PWbe(int Size, uint Value)
        {
            FieldSize = Size;
            InternalData = new byte[Size * 4];
            for (int i = 0; i < FieldSize; i++)
            {
                InternalData[i * 2] = Convert.ToByte((Value >> 24) & 0xFF);
                InternalData[i * 2 + 1] = Convert.ToByte((Value >> 16) & 0xFF);
                InternalData[i * 2 + 2] = Convert.ToByte((Value >> 8) & 0xFF);
                InternalData[i * 2 + 3] = Convert.ToByte(Value & 0xFF);
            }
        }

        public PWbe(int Size, uint Value, uint[] Values)
        {
            FieldSize = Size;
            InternalData = new byte[Size * 2];
            for (int i = 0; i < FieldSize; i++)
            {
                var Val = (i < Values.Length) ? Values[i] : Value;
                InternalData[i * 2] = Convert.ToByte((Val >> 24) & 0xFF);
                InternalData[i * 2 + 1] = Convert.ToByte((Val >> 16) & 0xFF);
                InternalData[i * 2 + 2] = Convert.ToByte((Val >> 8) & 0xFF);
                InternalData[i * 2 + 3] = Convert.ToByte(Val & 0xFF);
            }
        }

        public new string Field
        {
            get { return String.Format("PW{0:d}", FieldSize); }
        }
    }
}
