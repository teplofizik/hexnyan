﻿using System;
using System.Collections.Generic;
using System.Text;

namespace hexnyan.eeprom
{
    interface Element
    {
        // Тип поля
        string Field
        {
            get;
        }

        // Element name
        string Name
        {
            get;
        }

        // Element description
        string Description
        {
            get;
        }

        // Набор байт
        byte[] Data
        {
            get;
        }
    }
}
