using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Valuating.Controllers.super
{
    public class IndexSettingController : GlobalSuperController
    {
        //
        // GET: /IndexSetting/

        public ActionResult Index()
        {
            return View("~/views/super/indexsetting.cshtml");
        }

    }
}
