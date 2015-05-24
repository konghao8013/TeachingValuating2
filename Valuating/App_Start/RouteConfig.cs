using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Valuating
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

         
          
            routes.MapRoute(
                "super", // 路由名称  
                "super/{controller}/{action}/{id}", // 带有参数的 URL  
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                   new string[] { "Valuating.Controllers.super" }
                // 参数默认值  
            );
            routes.MapRoute(
               "student", // 路由名称  
               "student/{controller}/{action}/{id}", // 带有参数的 URL  
               new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                  new string[] { "Valuating.Controllers.student" }
                // 参数默认值  
           );
            routes.MapRoute(
               "teacher", // 路由名称  
               "teacher/{controller}/{action}/{id}", // 带有参数的 URL  
               new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                  new string[] { "Valuating.Controllers.teacher" }
                // 参数默认值  
           );

            routes.MapRoute(
                  "Default",
                  "{controller}/{action}/{id}",
                  new { controller = "Index", action = "Index", id = UrlParameter.Optional },
                  new string[] { "Valuating.Controllers" }
                  );

          
        }
    }
}