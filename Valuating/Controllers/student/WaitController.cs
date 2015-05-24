using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALOS.Model;

namespace Valuating.Controllers.student
{
    /// <summary>
    /// 等待专家测评完成
    /// </summary>
    public class WaitController : GlobalStudentController
    {
        //
        // GET: /Wait/

        public ActionResult Index()
        {
            return View("~/views/student/wait.cshtml");
        }
        /// <summary>
        /// 获得当前学生测评资料
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("POST", "GET")]
        public string GetData()
        {
            var studentData = DB.StudentRecordServer.RenderRecord(VT.Users.Id,true) ?? new StudentRecordType { Rid = (int)VT.Users.Id };
            return studentData.Serialize();
        }

    }
}
