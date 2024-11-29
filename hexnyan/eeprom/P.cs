using System;
using System.Collections.Generic;
using System.Text;

namespace hexnyan.eeprom
{
    // Padding
    class P : BasicElement
    {
        private int FieldSize = 0;

        public P(int Size, byte Value)
        {
            FieldSize = Size;
            InternalData = new byte[Size];
            for (int i = 0; i < FieldSize; i++)
                InternalData[i] = Value;
        }

        public P(int Size, byte Value, byte[] Values)
        {
            FieldSize = Size;
            InternalData = new byte[Size];
            for (int i = 0; i < FieldSize; i++)
                InternalData[i] = (i < Values.Length) ? Values[i] : Value;
        }
        
        public new string Field
        {
            get { return String.Format("P{0:d}", FieldSize); }
        }
    }
}
