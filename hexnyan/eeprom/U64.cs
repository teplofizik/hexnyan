﻿using System;
using System.Collections.Generic;
using System.Text;

namespace hexnyan.eeprom
{
    class U64 : BasicElement
    {
        public U64(UInt64 Value)
        {
            InternalData = new byte[8];

            InternalData[0] = Convert.ToByte((Value >>  0) & 0xFF);
            InternalData[1] = Convert.ToByte((Value >>  8) & 0xFF);
            InternalData[2] = Convert.ToByte((Value >> 16) & 0xFF);
            InternalData[3] = Convert.ToByte((Value >> 24) & 0xFF);
            InternalData[4] = Convert.ToByte((Value >> 32) & 0xFF);
            InternalData[5] = Convert.ToByte((Value >> 40) & 0xFF);
            InternalData[6] = Convert.ToByte((Value >> 48) & 0xFF);
            InternalData[7] = Convert.ToByte((Value >> 56) & 0xFF);
        }

        public new string Field
        {
            get { return "u64"; }
        }
    }
}
