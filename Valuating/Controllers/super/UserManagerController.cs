using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Valuating.Controllers.super
{
    public class UserManagerController :GlobalSuperController
    {
        //
        // GET: /UserManager/

        public ActionResult Index()
        {
            return View("~/views/super/usermanager.cshtml");
        }

    }
}
