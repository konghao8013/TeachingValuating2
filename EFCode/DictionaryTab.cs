using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace EFCode
{
    public static class DictionaryTab
    {
        private static Dictionary<string, string> dic;

        private static Dictionary<string, string> Dic
        {
            get
            {
                if (dic == null)
                {
                    dic=new Dictionary<string, string>();
                    var xml = XElement.Load("conf/modelTypes.xml");
                    foreach (XElement x in xml.Nodes())
                    {
                        dic.Add(x.Attribute("key").Value, x.Attribute("value").Value);
                    }
                   
                }
                return dic;
            }
        }
       

        public static string GetTabValue(this string key)
        {
            key = key.ToLower();
            if (Dic.ContainsKey(key))
            {
                return Dic[key];
            }
            return key;

        }

    }
}
