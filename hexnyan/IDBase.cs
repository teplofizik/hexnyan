﻿using System;
using System.Collections.Generic;
using System.Text;

namespace hexnyan
{
    class IDField
    {
        public string Key = null;
        public Int64 Value = 0;

        public IDField(string K, Int64 V)
        {
            Key = K;
            Value = V;
        }
    }

    static class IDBase
    {
        private static List<IDField> Values = new List<IDField>();

        private static IDField GetField(string Key)
        {
            foreach (IDField Value in Values)
            {
                if (Value.Key.CompareTo(Key) == 0) return Value;
            }

            

            return null;
        }

        public static Int64 GetValue(string Key, Int64 Default)
        {
            IDField F = GetField(Key);

            if (F == null)
            {
                IDField NewValue = new IDField(Key, Default);
                Values.Add(NewValue);
                Save();

                return Default;
            }
            else
            {
                F.Value++;
                Save();

                return F.Value;
            }
        }

        public static void SetValue(string Key, Int64 Value)
        {
            IDField F = GetField(Key);

            if (F == null)
            {
                IDField NewValue = new IDField(Key, Value);
                Values.Add(NewValue);
            }
            else
            {
                F.Value = Value;
            }

            Save();
        }

        public static void Load()
        {
            CONF.XmlLoad X = new CONF.XmlLoad();
            if (!X.Load("base.xml")) return;

            while (X.Read())
            {
                switch (X.ElementName)
                {
                    case "field":
                        {
                            Values.Add(new IDField(X.GetAttribute("key"), X.GetInt64Attribute("value")));
                        }
                        break;
                }
            }

            X.Close();
        }

        public static void Save()
        {
            CONF.XmlSave X = new CONF.XmlSave("base.xml");

            X.StartXML("base");

            foreach (IDField Value in Values)
            {
                X.StartTag("field");
                X.Attribute("key", Value.Key);
                X.Attribute("value", Value.Value.ToString());
                X.EndTag();
            }

            X.EndXML();
            X.Close();
        }
    }
}
