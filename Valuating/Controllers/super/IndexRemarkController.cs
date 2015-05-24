using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Valuating.Controllers.super
{
    /// <summary>
    /// 指标评语操作类
    /// </summary>
    public class IndexRemarkController : GlobalSuperController
    {
        //
        // GET: /IndexRemark/

        public ActionResult Index()
        {
            return View("~/views/super/indexremark.cshtml");
        }

    }
}
