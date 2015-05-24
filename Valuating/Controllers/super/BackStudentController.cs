using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Valuating.Controllers.super
{
    public class BackStudentController : GlobalSuperController
    {
        //
        // GET: /BackStudent/

        public ActionResult Index()
        {
            return View("~/views/super/backstudent.cshtml");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        [AcceptVerbs("POST","GET")]
        public string ISOK(int id)
        {
            UpdateSend(true, id);
            return "操作成功".Serialize();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        [AcceptVerbs("POST","GET")]
        public string NOOK(int id)
        {
            UpdateSend(false, id);
            return "操作成功".Serialize();
        }
        
        public void UpdateSend(bool isok,int id)
        {
            var paper = DB.ParperSendServer.Reader(id);
            paper.IsOK = isok;
            paper.State = true;
            DB.ParperSendServer.Save(paper);
            if (isok)
            {
                DB.ParperSendServer.OkSend(paper.Id);
            }
            else
            {
                DB.EvaluatingPaperServer.ResetPaperDisable(paper.PaperId, false);
                //不同意
            }
        }

    }
}
