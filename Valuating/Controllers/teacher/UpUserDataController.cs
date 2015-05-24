using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Valuating.Controllers.teacher
{
    public class UpUserDataController : GlobalTeacherController
    {
        //
        // GET: /UpUserData/

        public ActionResult Index()
        {
            return View("~/views/teacher/upuserdata.cshtml");
        }
        [AcceptVerbs("POST", "GET")]
        public void ReUser(string phone, string email, string password)
        {
            var user = VT.Users;
            user.Password = password != "111111" ? password.SHA512_Encrypt() : user.Password;
            DB.UserServer.Save(user);
            var teacher = DB.TeacherServer.SelectTeacherRid((int)user.Id);
            teacher.Phone = phone;
            teacher.Email = email;
            DB.TeacherServer.Save(teacher);

        }

        [AcceptVerbs("POST", "GET")]
        public string StudentUser()
        {
            return DB.TeacherServer.SelectTeacherRid((int)VT.Users.Id).Serialize();
        }
    }
}
