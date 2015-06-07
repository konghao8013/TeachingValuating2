using ALOS.DALSERVER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Security;

namespace Valuating.Controllers
{
    public class CheckUserAttribute : ActionFilterAttribute
    {
        
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (VT.Users==null)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
               
            }
            else
            {
                 base.OnActionExecuting(actionContext);
               
           
            }
        }
      
    }
}