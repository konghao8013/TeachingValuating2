using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALOS.Model;

namespace Valuating.Controllers.teacher
{
    /// <summary>
    /// 打回材料控制器
    /// </summary>
    public class StudentRetreatController : GlobalTeacherController
    {
        //
        // GET: /StudentRetreat/

        public ActionResult Index()
        {
            return View("~/views/teacher/studentretreat.cshtml");
        }
        [AcceptVerbs("POST","GET")]
        public void ExitStudentData(int paperId,string content)
        {
            var paper = new ParperSendType();
            paper.PaperId = paperId;
            paper.TeacherId = VT.Users.Id;
            paper.CreateTime = DateTime.Now;
            paper.Content = content;
            DB.ParperSendServer.SaveSend(paper);
        }

    }
}
