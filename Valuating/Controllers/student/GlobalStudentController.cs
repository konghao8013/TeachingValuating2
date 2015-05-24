using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALOS.Model;

namespace Valuating.Controllers.student
{
    public class GlobalStudentController : GlobalController
    {
      
        //
        // GET: /GlobalSuper/
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User == null || User.GetUserType() != EnumUserType.Student)
            {
                Response.Redirect("/login");
            }
           
            base.OnActionExecuting(filterContext);
        }

        public GlobalStudentController()
        {
         
            RazorUrl = "~/views/student";
        }
    }
}
