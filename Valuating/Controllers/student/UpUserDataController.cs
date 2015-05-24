using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Valuating.Controllers.student
{
    public class UpUserDataController : GlobalStudentController
    {
        //
        // GET: /UpUserData/

        public ActionResult Index()
        {
            return View("~/views/student/upuserdata.cshtml");
        }

        [AcceptVerbs("POST", "GET")]
        public void ReUser(string phone, string email, string password)
        {
            var user=VT.Users;
            user.Password = password != "111111" ? password.SHA512_Encrypt() : user.Password;
            DB.UserServer.Save(user);
            var student = DB.StudentServer.ReaderStudent((int)user.Id);
            student.Phone = phone;
            student.Email = email;
            DB.StudentServer.Save(student);

        }

        [AcceptVerbs("POST", "GET")]
        public string StudentUser()
        {
            return DB.StudentServer.ReaderStudent((int)VT.Users.Id).Serialize();
        }

    }
}
