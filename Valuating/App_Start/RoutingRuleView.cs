using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALOS.DALSERVER;
using Valuating.Controllers;

namespace Valuating.App_Start
{
    public class  RoutingRuleView : RazorViewEngine
    {

        private string[] viewLocationFormats = new[]
        {
            "~/Views/super/{0}.cshtml",
            "~/Views/teacher/{0}.cshtml",
            "~/Views/student/{0}.cshtml",
            "~/Views/{1}/{0}.cshtml",
            "~/Views/Shared/{0}.cshtml"

        };
        
        public RoutingRuleView()
        {
            ;
            //1文件夹名称0 cshtml名称
            ViewLocationFormats = viewLocationFormats;
        }
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            var actionName = controllerContext.Controller.ViewBag.RazorUrl;
            string controllerName = controllerContext.RouteData.GetRequiredString("controller");
          
            if (actionName != null)
            {
                var viewLocations = new List<string>();
                Array.ForEach(viewLocationFormats, format => viewLocations.Add(string.Format(format, controllerName, viewName)));
           
                var tempUri = String.Format("{1}/{0}.cshtml", controllerName, actionName).ToLower();
                var index = viewLocations.IndexOf(a => a.ToLower() == tempUri);
                if (index > 0)
                {
                    viewLocations.RemoveAt(index);
                    viewLocations.Insert(0, tempUri);
                    SysLogServer.InsertMessage(tempUri);
                    ViewLocationFormats = new string[] {tempUri};
                }
                else
                {
                    ViewLocationFormats = viewLocations.ToArray();
                }
               
            }
         
           return base.FindView(controllerContext, viewName, masterName, useCache);
        }

    }
}