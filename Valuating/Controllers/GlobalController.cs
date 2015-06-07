using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALOS.Expand;
using ALOS.Model;
using ALOS.DALSERVER;

namespace Valuating.Controllers
{
    public class GlobalController : Controller
    {
        /// <summary>
        /// 设置视图根目录
        /// </summary>
        public string RazorUrl {
            set
            {
                ViewBag.RazorUrl = value;
               
            }
            get { return ViewBag.RazorUrl; }
        }
        /// <summary>
        /// 视图默认Js
        /// </summary>
        public string RazorScript {
            set { ViewBag.RazorScript = value; }
            get { return ViewBag.RazorScript; }
        }

        public GlobalController()
        {
            //Valuating.Controllers.IndexController
         //   RazorScript = "~/bundle/" + GetType().FullName;
            //if (User == null)
            //{
            //    Response.Redirect("/login");
            //}

        }

        

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
          
            if (User == null||User.Id==0)
            {
                Response.Redirect("/login");
            }
            base.OnActionExecuting(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
         //   filterContext.ParentActionViewContext.View=new RazorView("~/View/student/index.cshtml");
            base.OnActionExecuted(filterContext);

        }

        /// <summary>
        /// 获得系统名称
        /// </summary>
        public static string GetSystemName {
            get { return "教师专业能力评估系统(TCTAS)"; }
        }

        /// <summary>
        /// 获得用户对象
        /// </summary>
        public UserType User {
            get
            {
                var user= Session[VT.UserGuid] == null ? null : Session[VT.UserGuid] as UserType;
                //根据Cookie获得用户对象
                if (user == null&&Request.Cookies[VT.UserGuid]!=null)
                {
                    var userguid = Request.Cookies[VT.UserGuid].Value;
                    user=DB.UserServer.UserGuidLogin(userguid);
                }

                return user;
            }
        }
      
    }
    /// <summary>
    /// 全局公共变量
    /// </summary>
    public class VT
    {
        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool Send(TeacherType type)
        {
            var sys = DB.SysSettingServer.ReaderSettingType();
            if (sys.IsDebug)
            {
                //System.DateTime.Now.ToString() + "提交材料," + _sno + "学院" + Schoolid;
                SmsExpand.Send(type.Phone,DateTime.Now.ToString("yyyy年MM月dd日 hh:ss")+"提交材料 学院"+type.School);
            }
            return true;
        }

        public static String SystemName {
            get { return "教师专业能力评估系统   Powered By 重庆雅乐视科技有限公司   All Rights Reserved."; }
        }

        /// <summary>
        /// 获得用户会话中的Guid
        /// </summary>
        public static String UserGuid
        {
            get { return "d2d0860f-c86e-4551-bc02-2f6387004aa3"; }
        }
        /// <summary>
        /// 获得当前网站的用户信息
        /// </summary>
        public static UserType Users
        {
            get
            {
              
                if (HttpContext.Current == null) {
                    return null;
                }

                if (HttpContext.Current.Session!=null&&HttpContext.Current.Session[VT.UserGuid] != null)
                {
                    return (UserType)HttpContext.Current.Session[VT.UserGuid];
                }
                SysLogServer.InsertMessage("row:138" + "| HttpContext.Current.Request.Cookies" + (HttpContext.Current.Request.Cookies == null), "UserType", "Debug");
                if (HttpContext.Current.Request.Cookies[UserGuid] != null)
                {
                   var user= DB.UserServer.UserGuidLogin(HttpContext.Current.Request.Cookies[UserGuid].Value);
                   //HttpContext.Current.Session[VT.UserGuid] = user;
                    return user;
                }
                return null;
            }
            set
            {
                var Session = HttpContext.Current.Session;
                var user = value ?? new UserType();
                var Request = HttpContext.Current.Request;
                var Response = HttpContext.Current.Response;
                Session[UserGuid] = user;
                HttpCookie userCookie;
                if (Request.Cookies[UserGuid] != null)
                {
                    userCookie = Request.Cookies[UserGuid];

                }
                else
                {
                    userCookie = new HttpCookie(UserGuid);
                }

                userCookie.Value = user.UserGuid.ToString();
                var expies = DateTime.Now;
                expies = expies.AddDays(7);
                userCookie.Expires = expies;
                Response.Cookies.Add(userCookie);
            }
        }
       
    }
}