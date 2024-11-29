using System;
using System.Collections.Generic;
using System.Text;

namespace hexnyan.parser
{
    class PreparsedElement
    {
        public string Name = "";
        public string FieldType = "";
        public string Comment = "";
        public string Value = "";
        public int Width = 0;

        // Список значений для массивов
        public string[] Values = new string[] { };

        public PreparsedElement(string T, string V)
        {
            FieldType = T;
            Value = V;
            Width = 1;
        }

        public PreparsedElement(string T, string V, int W)
        {
            FieldType = T;
            Value = V;
            Width = W;
        }

        public PreparsedElement(string T, string V, int W, string N, string C)
        {
            FieldType = T;
            Value = V;
            Width = W;
            Name = N;
            Comment = C;
        }
    }
}
