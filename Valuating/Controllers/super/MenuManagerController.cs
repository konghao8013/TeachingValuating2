using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Valuating.Controllers.super
{
    public class MenuManagerController : GlobalSuperController
    {
        //
        // GET: /MenuManager/

        public ActionResult Index()
        {
            return View("~/views/super/menumanager.cshtml");
        }

    }
}
