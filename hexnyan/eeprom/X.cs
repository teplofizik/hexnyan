﻿using System;
using System.Collections.Generic;
using System.Text;

namespace hexnyan.eeprom
{
    // Hex
    class X : BasicElement
    {
        private int FieldSize = 0;

        public X(int Size, byte[] Value)
        {
            FieldSize = Size;
            InternalData = new byte[Size];
            for (int i = 0; i < FieldSize; i++)
            {
                if ((Value != null) && (i < Value.Length))
                    InternalData[i] = Value[i];
                else
                    InternalData[i] = 0;
            }
        }


        public new string Field
        {
            get { return String.Format("X{0:d}", FieldSize); }
        }
    }
}
