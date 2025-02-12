﻿using System;
using System.Collections.Generic;
using System.Text;

namespace hexnyan.eeprom
{
    // String
    class S : BasicElement
    {
        private string Value = "";
        private int FieldSize = 0;

        public S(int Size, string Text)
        {
            FieldSize = Size;
            Value = Text;
            InternalData = new byte[Size];

            Compile();
        }

        private void Compile()
        {
            for (int i = 0; i < FieldSize; i++)
            {
                if(Value.Length > i)
                {
                    InternalData[i] = Convert.ToByte(Value[i]);
                }
                else
                {
                    InternalData[i] = 0;
                }
            }

            InternalData[FieldSize - 1] = 0;
        }

        public new string Field
        {
            get { return String.Format("S{0:d}", FieldSize); }
        }
    }
}
