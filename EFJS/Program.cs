using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ALOS.Expand;


namespace EFJS
{
    class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">启动参数 下标0 为输出类型，下标1为输出JS的路径 下标2为Dll路径下标3代表是否可调试</param>
        static void Main(string[] args)
        {
            //start EFJS.exe DAL ../../../valuating/Resource/Scripts/dbserver ../../../DALSERVER/bin/Debug/DALSERVER.dll Yes
            var list = new List<string>();
            list.Add("DAL");
            list.Add("../../../valuating/Resource/Scripts/dbserver");
            list.Add("../../../DALSERVER/bin/Debug/DALSERVER.dll");
            list.Add("Yes");
         //   args = list.ToArray();
            if (args == null || args.Length < 4)
            {
                Console.WriteLine("启动参数不正确：启动参数 下标0 为输出类型，下标1为输出JS的路径 下标2为Dll路径 3是否为压缩模式Yse No \r\n  start EFJS.exe Model ../../../valuating/Resource/Scripts/model Model.dll Yes");
              //  return;
                //../efjs/bin/Debug/EFJS.exe DAL ../valuating/Resource/Scripts/dbserver ../DALSERVER/bin/Debug/DALSERVER.dll Yes
                
            }
            args[1] = Path.GetFullPath(args[1]);
            switch (args[0])
            {
                case "Model":
                  
                   GetModelText(args[1],args[2],args[3]);
                   // var sr = new StreamReader(args[1]);
                   // var tstr = sr.ReadToEnd();
                   // sr.Close();
                   // if (strs.ToString().Length ==tstr.Length )
                   // {
                       
                       
                   //     return;
                   // }
                   
                   // var sw = new StreamWriter(args[1],false,Encoding.UTF8);
                   // sw.Write(strs.ToString());
                   //sw.Close();
                    break;
                case "DAL":
                   
                    GetDalText(args[1],args[2], args[3]);
                    break;
            }
            
          
        }
        private static void GetDalText(string outUrl,string path, string compile)
        {
           
            var assembly = Assembly.Load(File.ReadAllBytes(path));
            var types = assembly.GetTypes();
            var length = types.Count();
            for (int i = 0; i < length; i++)
            {
                var type = types[i];
               var tapi=Attribute.GetCustomAttribute(type, typeof (APIServer)) as APIServer;
                if (tapi == null)
                {
                    continue;
                    
                }
                var sb=new StringBuilder();
                sb.AppendLine("//"+tapi.Name+"\r\nDB."+type.Name+" = function() {\r\n};");
                var methods = type.GetMethods();
                var mlength = methods.Count();
                for (int j = 0; j < mlength; j++)
                {
                    var method = methods[j];
                  var mapi=Attribute.GetCustomAttribute(method, typeof(APIServer)) as APIServer;
                    if (mapi == null)
                    {
                        continue;
                        
                    }
                    var state = mapi.Name + " " + mapi.Parameter+"\t\t";
                    var parameters = method.GetParameters();
                    var plength = parameters.Count();
                    var parameterText = "";

                    var arrayText = "";
                    for (int k = 0; k < plength; k++)
                    {
                        parameterText += parameters[k].Name + ",";
                        arrayText += "array[" + k + "]=" + parameters[k].Name + ";\r\n";
                        state += "参数("+(k+1)+") "+parameters[k].ParameterType.FullName+"";
                    }
                    if (parameterText.Length>0)
                    parameterText = parameterText.Remove(parameterText.Length - 1);

                    sb.AppendLine("//" + state + "\r\nDB." + type.Name + "." + method.Name + "=function(" + (parameterText.Length>0?parameterText+",":"") + "success){");
                    sb.AppendLine("var array=new Array();");
                    sb.AppendLine(arrayText);
                    sb.AppendLine("DB.CreateWebLog(\"" + type.Name + "\", \"" + method.Name + "\", array,success);");
                    sb.AppendLine("};");
                    
                }
                if (!Directory.Exists(outUrl))
                {
                    Directory.CreateDirectory(outUrl);
                }


                var outJs = Path.GetFullPath(outUrl + "/" + type.Name + ".js");

                if (File.Exists(outJs))
                {

                    var jsPath = Path.GetFullPath(outJs);
                    var sr = new StreamReader(jsPath, Encoding.UTF8);
                    var values = sr.ReadToEnd();
                    sr.Close();
                    if (values==sb.ToString())
                    {
                        continue;

                    }
                }
                var outValue = sb.ToString();
                if (compile != "Yes")
                    outValue = outValue.JSCompres();
                var sw = new StreamWriter(outJs, false, Encoding.UTF8);
                sw.Write(outValue);
               sw.Close();
            }
        }

     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Dll路径</param>
        /// <returns></returns>
        private static void GetModelText(string outUrl,string path, string compile)
        {
            //WM.prototype.WebServerLog
         
            //sbs.AddAppend("(function(){",compile);
            //sbs.AddAppend("WM=this,",compile);
         
            var assembly = Assembly.Load(File.ReadAllBytes(path));
            var types = assembly.GetTypes();
            var length = types.Count();
            for (var i=0;i<length;i++)
            {
                var sbs = new StringBuilder();
                var type = types[i];
                var tab = Attribute.GetCustomAttribute(type, typeof(TableName)) as TableName;
                if (tab == null)
                {
                    continue;
                    
                }
                if (tab != null && compile == "Yes")
                {
                    sbs.AppendLine("//" + tab.Explain);
                }
                sbs.AppendLine("WM." + type.Name + "=function(){\r\n var obj = new Object();");
                var properties = type.GetProperties();
                var plength = properties.Count();
                var obj=assembly.CreateInstance(type.FullName);
                if (compile == "Yes")
                {
                    for (var pi = 0; pi < plength; pi++)
                    {
                        var propertie = properties[pi];
                        
                        var tvalue = "\""+GetValue(obj,propertie)+"\"";
                        if (propertie.PropertyType.FullName == "System.Object[]")
                        {
                            tvalue = "[]";
                        }
                        sbs.AppendLine("obj." + propertie.Name + "=" + tvalue +
                                       (pi == plength - 1 ? ";\r\n     return obj;\r\n};" : ","));

                    }
                }
                else
                {
                    sbs.AppendLine(" obj.Model_Type=\""+type.FullName+"\";  return obj;};");
                }
                if (Directory.Exists(outUrl))
                {
                    Directory.CreateDirectory(outUrl);
                }
                var turl = Path.GetFullPath(outUrl + "\\" + type.Name + ".js");

                var strs2 = sbs.ToString();
                if (File.Exists(turl))
                {
                    StreamReader sr=new StreamReader(turl,Encoding.UTF8);
                   var strs=sr.ReadToEnd();
                 
                    sr.Close();

                    if (strs== strs2)
                    {
                        continue;
                        
                    }
                }
                StreamWriter sw=new StreamWriter(turl,false,Encoding.UTF8);
                if (compile != "Yes")
                {
                    strs2 = strs2.JSCompres();
                }
                sw.Write(strs2);
                sw.Close();

                //sbs.AddAppend(i == length - 1 ? "}" : "},",compile);
            }
            //sbs.AddAppend("})(window);",compile);
           
        }

       static object GetValue<E>(E type, PropertyInfo info)
        {
            object ischeck;
            var value = info.GetValue(type, null);
            switch (info.PropertyType.FullName)
            {
                case "System.String":
                    ischeck = string.IsNullOrEmpty((string)value) ? "" : info.GetValue(type, null);
                    break;
                case "System.Int32 ":
                    ischeck = (int)value == 0 ? 0 : info.GetValue(type, null);
                    break;
                case "System.Byte":
                    ischeck = (Byte)value == 0 ? 0 : info.GetValue(type, null);
                    break;
                case "System.SByte":
                    ischeck = (SByte)value == 0 ? 0 : info.GetValue(type, null);
                    break;
                case "System.Char":
                    ischeck = (Char)value == '\0' ? '\0' : info.GetValue(type, null);
                    break;
                case "System.Decimal":
                    ischeck = (Decimal)value == 0.0m ? 0.0m : info.GetValue(type, null);
                    break;
                case "System.Double":
                    ischeck = (Double)value == 0.0d ? 0.0d : info.GetValue(type, null);
                    break;
                case "System.Single":
                    ischeck = (Double)value == 0.0f ? 0.0f : info.GetValue(type, null);
                    break;
                case "System.UInt32":
                    ischeck = (UInt32)value == 0 ? 0 : info.GetValue(type, null);
                    break;
                case "System.DateTime":
                    ischeck = (DateTime)value == DateTime.MinValue ? DateTime.Parse("2012-12-12") : info.GetValue(type, null);
                    break;
                default:
                    ischeck = value == null ? "" : info.GetValue(type, null);
                    break;
            }
            return ischeck;
        }






    }


 
}
