﻿using System;
using System.Collections.Generic;
using System.Text;

namespace hexnyan.eeprom
{
    class U32be : BasicElement
    {
        public U32be(UInt32 Value)
        {
            InternalData = new byte[4];

            InternalData[3] = Convert.ToByte((Value >>  0) & 0xFF);
            InternalData[2] = Convert.ToByte((Value >>  8) & 0xFF);
            InternalData[1] = Convert.ToByte((Value >> 16) & 0xFF);
            InternalData[0] = Convert.ToByte((Value >> 24) & 0xFF);
        }

        public new string Field
        {
            get { return "U32"; }
        }
    }
}
