using System;
using System.Collections.Generic;
using System.Text;

namespace hexnyan.eeprom
{
    class U8 : BasicElement
    {
        public U8(byte Value)
        {
            InternalData = new byte[1];

            InternalData[0] = Value;
        }

        public new string Field
        {
            get { return "u8"; }
        }
    }
}
