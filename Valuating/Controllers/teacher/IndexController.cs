using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Valuating.Controllers.teacher
{
    public class IndexController : GlobalTeacherController
    {
        //
        // GET: /Index/

        public ActionResult Index()
        {
            return View("~/views/teacher/index.cshtml");
        }
        [AcceptVerbs("post", "get")]
        public string SysName()
        {
            return VT.SystemName.Serialize();
        }
        /// <summary>
        /// 获得当前系统登录的用户
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("POST", "GET")]
        public string GetUser()
        {
            return VT.Users.Serialize();
        }
        [AcceptVerbs("POST","GET")]
        public string GetScoreNumber(bool state)
        {
            return DB.EvaluatingPaperServer.TeacherCount(VT.Users.Id,state).Serialize();
        }
    }
}
