using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALOS.Model;

namespace Valuating.Controllers.teacher
{
    public class GlobalTeacherController : GlobalController
    {
        public GlobalTeacherController()
        {
            RazorUrl = "~/views/teacher";
        }
        //
        // GET: /GlobalSuper/
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User == null || User.GetUserType() != EnumUserType.Teacher)
            {
                Response.Redirect("/login");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
