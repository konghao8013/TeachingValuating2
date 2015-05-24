using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALOS.Model;

namespace Valuating.Controllers.teacher
{
    public class AppraisalController : GlobalTeacherController
    {
        //
        // GET: /Appraisal/

        public ActionResult Index()
        {
            return View("~/views/teacher/appraisal.cshtml");
        }
        /// <summary>
        /// 抽取未评测的试卷
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AcceptVerbs("POST", "GET")]
        public string GetPaperServer(int id)
        {
            return DB.StudentRecordServer.ExtractPaper(VT.Users.Id, id).Serialize();
        }
        [AcceptVerbs("post","get")]
        public string GetTeacher()
        {
            return DB.TeacherServer.Reader((int)VT.Users.Id).Serialize();
        }
        /// <summary>
        /// 根据类别获得指标库 类别 3 获得 说课指标库和视频指标库  3、5
        /// </summary>
        /// <param name="TypeId"></param>
        /// <returns></returns>
        [AcceptVerbs("POST","GET")]
        public string GetPresents(int typeId,int did)
        {
            var type = (EnumStudentDataType) typeId;
            if (type == EnumStudentDataType.Video)
            {
                var list = DB.PresentServer.SelectPresentList(EnumStudentDataType.Videobefclass,did);
                list.AddRange(DB.PresentServer.SelectPresentList(EnumStudentDataType.Video,did));
                return list.Serialize();
            }
            else
            {
                return DB.PresentServer.SelectPresentList(type,did).Serialize();
            }
         
        }
        /// <summary>
        /// 获得试卷成绩
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AcceptVerbs("POST","GET")]
        public string GetScore(int id)
        {
            return DB.EvaluatingPaperServer.Reader(id).Serialize();
        }

        /// <summary>
        /// 保存试卷
        /// </summary>
        /// <param name="list"></param>
        [AcceptVerbs("POST","GET")]
        public void PerformanceSave(List<PerformanceType> list, DateTime date, int typeId)
        {
            foreach (var model in list)
            {
                DB.PerformanceServer.Save(model);
            }
            var type = (EnumStudentDataType) typeId;
            //if (type == EnumStudentDataType.Video)
            //{
            //    DB.EvaluatingPaperServer.UpdateScore(list[0].DId, EnumStudentDataType.Video, date);
            //    DB.EvaluatingPaperServer.UpdateScore(list[0].DId, EnumStudentDataType.Videobefclass, date);
            //}
            
                DB.EvaluatingPaperServer.UpdateScore(list[0].DId,type, date);
           
         
        }

    }
}
