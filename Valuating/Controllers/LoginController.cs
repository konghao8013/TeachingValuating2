using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALOS.Model;

namespace Valuating.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {

            return View("~/views/login/index.cshtml");
        }
        [AcceptVerbs("POST","GET")]
        public string Reader()
        {
            return DB.SysSettingServer.ReaderSettingType().Serialize();
        }

        /// <summary>
        /// 获得当前系统登录的用户
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("POST","GET")]
        public string GetUser()
        {
            return VT.Users.Serialize();
        }

        [AcceptVerbs("POST", "GET")]
        public void Logout()
        {
            VT.Users = null;
        }

        [AcceptVerbs("POST", "GET")]
        public string LoginUser(string loginId, string password)
        {
            var user = DB.UserServer.UserLogin(loginId, password);
            if (user != null)
            {
                var sys = DB.SysSettingServer.ReaderSettingType();   
                switch (user.GetUserType())
                {
                    case EnumUserType.Student:
                        var student=DB.StudentServer.ReaderStudent(user.Id);
                        user.State = student.Status;
                     
                        if (user.State)
                        {
                            user.State = sys.StudentLogin;
                        }
                        user.Reurl = "student/index";
                        break;
                    case EnumUserType.Teacher:
                        var teacher = DB.TeacherServer.Reader("rid", user.Id);
                        user.State = teacher.State;
                        if (user.State)
                        {
                            user.State = sys.TeacherLogin;
                        }
                        user.Reurl = "teacher/index";
                        break;
                    case EnumUserType.Admin:
                        user.Reurl = "super/Index";
                        user.State = true;
                        break;
                    default:
                        user.Reurl = "index";
                        break;
                }
                if (user.State)
                {
                    VT.Users = user;
                }
                else
                {
                    user.Reurl = "/login";
                }


            }
            return user.Serialize();
        }

        [HttpPost]
        public string SelectUser(UserType user)
        {

            var b = "22";
            return "";
        }

    }
}
