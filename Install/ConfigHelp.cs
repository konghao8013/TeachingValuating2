using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
namespace ConfigurationTool
{
    public class ConfigHelp
    {
        string path;
        public XElement Xml { set; get; }
        public ConfigHelp(string path) {
            this.path = path;
            Xml = XElement.Load(path);
        }
        /// <summary>
        /// 字符串分解函数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public string GetSqlString(string key, string str)
        {
            str = str == null ? "" : str;
            var demos = str.Split(';');
            foreach (var value in demos)
            {
                var values = value.Split('=');
                if (values[0].ToLower() == key.ToLower())
                {
                    return values[1];
                }
            }
            return null;
        }
        public string GetConnectionStrings(string name) {
            string value = null;
            var xml = GetXElement("connectionStrings");
           var x=GetValue("name", name, xml);
           if (x != null) {
               value = x.Attribute("connectionString").Value;
           }
            return value;
        }
        public string GetSystemServiceModel(string name) {
            string value = null;
            var xml = GetXElement("system.serviceModel");
           var x=GetXElement("client", xml);
           if (x != null)
           {
              var xs=GetValue("name", name, x);
              if (xs != null) {
                  value = xs.Attribute("address").Value;
              }
           }
            return value;
        }
        public void SetSystemServiceModel(string name,string value)
        {
          
            var xml = GetXElement("system.serviceModel");
            var x = GetXElement("client", xml);
            if (x != null)
            {
                var xs = GetValue("name", name, x);
                if (xs != null)
                {
                    xs.Attribute("address").Value=value;
                    Xml.Save(path);
                }
            }
           
        }
        public void SetConnectionStrings(string name, string value) {
          
            var xml = GetXElement("connectionStrings");
            var xmls = GetValue("name", name, xml);
            if (xmls == null)
            {
                xmls = new XElement("add", new XAttribute("name", name), new XAttribute("connectionString",value));
                xml.Add(xmls);
            }
            xmls.Attribute("connectionString").Value = value;
            Xml.Save(path);
           
        }
        public void SetAppSetting(string key, string value) {
            var xml = GetXElement("appSettings");
            var x=GetValue("key", key, xml);
            if (x == null) {
                x = new XElement("add",new  XAttribute("key",key),new XAttribute("value",value));
                xml.Add(x);
            }
            x.Attribute("value").Value = value;
            Xml.Save(path);
        }
        public string GetAppSetting(string key) {
            XElement value =null;
            var xml = GetXElement("appSettings");
            value = GetValue("key", key, xml);
           
            return value==null?"":value.Attribute("value").Value;
        }
       

      /// <summary>
      ///  根据属性名 属性值 获得该节点对应其中一个属性的值
      /// </summary>
      /// <param name="attributeName">判断属性名称</param>
      /// <param name="attributeValue">判断属性值</param>
      /// <param name="resutAttribute">得到该节点值名称</param>
      /// <param name="xml">xml节点</param>
      /// <returns></returns>
        XElement GetValue(string attributeName,string attributeValue, XElement xml) {
            XElement value=null;
            foreach (var x in xml.Nodes()) {
                if (x is XComment) {
                    continue;
                }
                XElement xd=(XElement)x;
                if(xd.Attribute(attributeName).Value == attributeValue){
                    value = xd;
                    break;
                }

            }
            //var x = (XElement)xml.Nodes().First(a =>
            //{
            //    if (a is XComment)
            //    {
            //        return false;
            //    }
            //    else
            //    {
            //        XElement xd = (XElement)a;
            //        return xd.Attribute(attributeName).Value == attributeValue;
            //    }
            //});
            //if (x != null)
            //{
            //    value =x;
            //}
            return value;
        
        }
        public XElement GetXElement(string name) {
            var xml =Xml;
            return  GetXElement(name, xml);
           
            //appSettings

        }
       
        XElement GetXElement(string key, XElement xml)
        {
           
            XElement resut = null;
            if (key == xml.Name.LocalName) {
                resut= xml;
                return resut;
            }
            var nodes = xml.Nodes();
            foreach (var node in nodes)
            {
                if (node is XComment)
                {
                    continue;
                }
                var x = (XElement)node;
                resut=GetXElement(key, x);
                if (resut != null) {
                    return resut;
                }
            }
            return resut;
        }
       
    }
}
