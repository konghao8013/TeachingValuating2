using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Valuating.Controllers.teacher
{
    public class ShowVideoController : GlobalTeacherController
    {
        //
        // GET: /ShowVideo/

        public ActionResult Index()
        {
            return View("~/views/teacher/showvideo.cshtml");
        }

    }
}
