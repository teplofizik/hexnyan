using System;
using System.Collections.Generic;
using System.Text;

namespace hexnyan.eeprom
{
    class IP4 : BasicElement
    {
        public IP4(byte[] Data)
        {
            InternalData = new byte[4];

            InternalData[0] = Data[0];
            InternalData[1] = Data[1];
            InternalData[2] = Data[2];
            InternalData[3] = Data[3];
        }

        public new string Field
        {
            get { return "IP"; }
        }
    }
}
