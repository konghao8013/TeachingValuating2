using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALOS.Model;

namespace Valuating.Controllers.super
{
    public class GlobalSuperController :GlobalController
    {
        //
        // GET: /GlobalSuper/
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User == null || User.GetUserType()!=EnumUserType.Admin)
            {
                Response.Redirect("/login");
            }
            base.OnActionExecuting(filterContext);
        }
        public GlobalSuperController()
        {
             RazorUrl = "~/views/super";
            
        }
       

    }
}
