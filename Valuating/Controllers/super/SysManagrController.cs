using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Valuating.Controllers.super
{
    public class SysManagrController : GlobalSuperController
    {
        //
        // GET: /SysManagr/

        public ActionResult Index()
        {
            return View("~/views/super/sysmanagr.cshtml");
        }

    }
}
