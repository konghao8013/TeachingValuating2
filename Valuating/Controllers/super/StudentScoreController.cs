using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Valuating.Controllers.super
{
    public class StudentScoreController : GlobalSuperController
    {
        //
        // GET: /StudentScore/

        public ActionResult Index()
        {
            return View("~/views/super/studentscore.cshtml");
        }

    }
}
