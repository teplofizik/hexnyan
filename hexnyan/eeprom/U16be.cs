using System;
using System.Collections.Generic;
using System.Text;

namespace hexnyan.eeprom
{
    class U16be : BasicElement
    {
        public U16be(UInt16 Value)
        {
            InternalData = new byte[2];

            InternalData[0] = Convert.ToByte((Value >> 8) & 0xFF);
            InternalData[1] = Convert.ToByte((Value >> 0) & 0xFF);
        }

        public new string Field
        {
            get { return "U16"; }
        }
    }
}
