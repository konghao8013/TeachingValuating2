using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Management;
using System.Web.Mvc;
using ALOS.DALSERVER;
using ALOS.Expand;
using ALOS.Model;

namespace Valuating.Controllers
{
    public class WebDbController : ApiController
    {
        // GET api/<controller>
       
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
       [System.Web.Http.AcceptVerbs("POST", "GET")]
        public String TestJsonp()
        {
            WebServerLog log=new WebServerLog();
            log.ReturnValue = "返回数据的测试";
            log.Name = "名称测试";
            return "jsonpcallback(55)";
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
        [System.Web.Http.AcceptVerbs("POST", "GET")]
        [CheckUser]
        public string Test()
        
        {
            return "测试WEBapi";
        }
       [System.Web.Http.AcceptVerbs("POST","GET")]
        public bool Login(string loginid,string password)
        {
          var user= DB.UserServer.UserLogin(loginid, password);
            var state = false;
            if (user != null)
            {
                VT.Users = user;
                state = true;
            }
            return state;
        }
        [System.Web.Http.AcceptVerbs("POST", "GET")]
        public string Test(string name)
        {
            return name + "2B";
        }

        [CheckUser]
        [System.Web.Http.AcceptVerbs("POST")]
        [ValidateAntiForgeryToken] 
        public string Invoking(dynamic logdata)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
           
            WebServerLog log = null;
            try
            {
                string str = logdata.ToString();
                log = str.Deserialize<WebServerLog>();
            }
            catch (Exception e)
            {
                log = new WebServerLog();
                log.Message = e.Message;
                RadLog(log);
                return log.Serialize();
            }
            Assembly assembly = Assembly.Load("DALSERVER");
            var modelAs = Assembly.Load("Model");
            Type t = assembly.GetType("ALOS.DALSERVER" + "." + log.ClassName);
            
            if (t == null)
            {
                log.Message += "\r\n未找到 相应的数据操作类";
                return log.Serialize();
            }
            Type[] types = null;
            if (log.Parameter != null)
            {
                for (int i = 0; i < log.Parameter.Count(); i++)
                {
                    var a = log.Parameter[i];
                    if (a is Dictionary<string, Object>)
                    {
                        var dic = (Dictionary<string, Object>)a;

                        var modelType = (dic["Model_Type"]).ToString();
                        var temptype = modelAs.GetType(modelType);
                        var tempModel = modelAs.CreateInstance(modelType);
                        var tempProperties = temptype.GetProperties();
                        foreach (var tempPropertie in tempProperties)
                        {
                            if (!dic.ContainsKey(tempPropertie.Name) || dic[tempPropertie.Name] == null || (!tempPropertie.CanWrite))
                                continue;
                            object tempValue = (dic[tempPropertie.Name].ToString()).ConvertCorrect(tempPropertie.PropertyType.FullName);
                            
                            tempPropertie.SetValue(tempModel, tempValue, null);
                        }
                        log.Parameter[i] = tempModel;
                    }

                }
                var list = new List<Type>();
                for (var i=0;i<log.Parameter.Length;i++)
                {
                    var type = log.Parameter[i];
                    if (type == null)
                    {
                        log.Message = "第"+(i+1)+"参数为Null";
                        return log.Serialize();
                    }
                  
                    list.Add(type.GetType());
                }
                types = list.ToArray();




            }
            else
            {
                types = new Type[0];
            }
            if ( DB.GetServer(log.ClassName) == null)
            {
                log.Message = "未找到：" + log.ClassName+"工厂属性，请检查";
                return log.Serialize();
               
            }

            var obj = DB.GetServer(log.ClassName);
            var method = t.GetMethod(log.Name, types);
            if (method == null)
            {
                log.Message += "\r\n未找到 相应的方法";
                return log.Serialize();
            }
            try
            {
                log.ReturnValue = method.Invoke(obj, log.Parameter);
                log.Message = "OK";
            }
            catch (Exception e)
            {
                if (e.InnerException!=null)
                log.Message = e.InnerException.Message;
                else
                {
                    log.Message = e.Message;
                }

            }
            finally
            {
                RadLog(log);

            }
            var parameterString = "";
            foreach (var v in log.Parameter)
            {
                parameterString += v.ToString()+",";
            }
            if (parameterString.Length>0)
            parameterString = parameterString.Remove(parameterString.Length - 1);
            var time = sw.ElapsedTicks;
            var api = Attribute.GetCustomAttribute(method, typeof(APIServer)) as APIServer;
            SysLogServer.InsertMessage(string.Format("WEBAPI:{0} Parameter:{1} returnValue:{2} methodState:{3}", log.Name, parameterString, log.ReturnValue, (api != null ? api.Name + " 参数说明:" + api.Parameter : "")), "method", HttpContext.Current.Request.UserHostAddress,VT.Users.LoginId,time);

            log.Time = sw.ElapsedTicks;
            sw.Reset();
         
            return log.Serialize();

        }

        

        /// <summary>
        /// 记录系统日志
        /// </summary>
        /// <param name="log"></param>
        public void RadLog(WebServerLog log)
        {
            var tempList = Getfiltration();
            var isRed = false;//是否记录系统日志
            foreach (var str in tempList)
            {
                if (log.ClassName + "." + log.Name == str)
                {
                    isRed = true;
                    break;
                }
            }

            var isDebug = false;
            if (ConfigurationManager.AppSettings["IsDebug"] != null)
            {

                isDebug = Boolean.Parse(ConfigurationManager.AppSettings["IsDebug"]);
            }
            if (!isRed && isDebug)
            {
                //UsersServer.Login
                StringBuilder sb = new StringBuilder();
                if (log.Parameter != null)
                {
                    foreach (var p in log.Parameter)
                    {
                        sb.Append(p.ToString() + ",");
                    }
                }
                string type = "Error";
                InsertError("time:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\tMethod:" + log.ClassName + "." + log.Name + " \tparameter:【" + sb.ToString() + "】" + " \tmessage:" + log.Message + "\r\n");
            }
        }
        public List<string> Getfiltration()
        {
            var list = new List<string>();
            list.Add("UsersServer.Login");
            list.Add("UsersServer.SelectGuid");

            return list;
        }
        /// <summary>
        /// 将错误消息写入 文件夹
        /// </summary>
        /// <param name="value"></param>
        public static void InsertError(string value)
        {
            string str = System.AppDomain.CurrentDomain.BaseDirectory;
            str = str + "log/error";
            var errorurl = Path.GetFullPath(str);
            StreamWriter sw = null;
            if (!Directory.Exists(errorurl))
            {
                Directory.CreateDirectory(errorurl);
            }
            try
            {
                sw = new StreamWriter(Path.GetFullPath(errorurl + "/" + DateTime.Now.ToString("yyyyMMdd") + ".txt"), true);
            }
            catch (Exception)
            {

                sw = new StreamWriter(Path.GetFullPath(errorurl + "/" + DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N") + ".txt"), true);
            }
            sw.Write(value);
            sw.Close();
            sw.Dispose();
        }
    }
}