using System;
using System.Collections.Generic;
using System.Text;

namespace hexnyan.eeprom
{
    class BasicElement : Element
    {
        protected byte[] InternalData;

        public BasicElement()
        {
            InternalData = null;
        }

        public string Field
        {
            get { return "none"; }
        }

        public string Name
        {
            get { return "";  }
        }

        public string Description
        {
            get { return ""; }
        }

        public byte[] Data
        {
            get
            {
                int Length = InternalData.Length;
                byte[] Temp = new byte[Length];

                Buffer.BlockCopy(InternalData, 0, Temp, 0, Length);

                return Temp;
            }
        }
    }
}
