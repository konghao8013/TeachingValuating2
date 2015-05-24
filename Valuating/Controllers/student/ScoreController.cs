using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALOS.Expand;

namespace Valuating.Controllers.student
{
    public class ScoreController : GlobalStudentController
    {
        //
        // GET: /Score/

        public ActionResult Index()
        {
            return View("~/views/student/score.cshtml");
        }
        [AcceptVerbs("POST","GET")]
        public string GetUserScore()
        {
            return DB.StudentScoreServer.ReadStudentScore(VT.Users.Id).Serialize();
        }
        [AcceptVerbs("POST", "GET")]
        public string DowloadFile(int paperId)
        {
            var tab = DB.EvaluatingPaperServer.GroupScore(paperId);
            var path = Server.MapPath("~/resource/upload/") + "score_" + paperId + ".xls";
            ExcelExpand.ExcelUrl(tab, path, "教学评价成绩");
            var stuRecord = DB.StudentRecordServer.Reader(paperId);

            var serPath = Server.MapPath("~/");
            var files = new[] { path, serPath + stuRecord.TeachingPlan, serPath + stuRecord.Courseware, serPath + stuRecord.Reflection };
            var downZip = serPath + "/resource/upload/score_" + paperId + ".zip";
            downZip.CreateZip(files);
            return ("/resource/upload/score_" + paperId + ".zip").Serialize();

        }
        [AcceptVerbs("POST","GET")]
        public string GetStudentScore()
        {
            var paper = DB.StudentScoreServer.ReadStudentScore(VT.Users.Id);
            return DB.PresentServer.SelectPaperList(paper.PaperId).Serialize();
        }

        [AcceptVerbs("POST", "GET")]
        public void ResetPaper()
        {
           var student= DB.StudentServer.ReaderStudent(VT.Users.Id);
            if (student.AppraisalNumber < 2)
            {
                DB.StudentRecordServer.ResetScore(VT.Users.Id);
            }

        }

    }
}
