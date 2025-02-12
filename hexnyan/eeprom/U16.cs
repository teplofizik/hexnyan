﻿using System;
using System.Collections.Generic;
using System.Text;

namespace hexnyan.eeprom
{
    class U16 : BasicElement
    {
        public U16(UInt16 Value)
        {
            InternalData = new byte[2];

            InternalData[0] = Convert.ToByte((Value >> 0) & 0xFF);
            InternalData[1] = Convert.ToByte((Value >> 8) & 0xFF);
        }

        public new string Field
        {
            get { return "u16"; }
        }
    }
}
