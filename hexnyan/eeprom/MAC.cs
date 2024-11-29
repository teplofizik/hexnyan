using System;
using System.Collections.Generic;
using System.Text;

namespace hexnyan.eeprom
{
    class MAC : BasicElement
    {
        public MAC(byte[] Data)
        {
            InternalData = new byte[6];

            InternalData[0] = Data[0];
            InternalData[1] = Data[1];
            InternalData[2] = Data[2];
            InternalData[3] = Data[3];
            InternalData[4] = Data[4];
            InternalData[5] = Data[5];
        }

        public new string Field
        {
            get { return "MAC"; }
        }
    }
}
