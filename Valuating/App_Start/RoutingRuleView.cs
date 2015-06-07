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


		private static string _Root = HttpContext.Current.Server.MapPath ("~/");



		//holds all of the actual paths to the required files
		private static Dictionary<string, string> _ViewPaths = 
			new Dictionary<string, string> (StringComparer.OrdinalIgnoreCase);

		//update the path to match a real file
		protected override IView CreateView (ControllerContext controllerContext, 
			string viewPath, string masterPath)
		{
			viewPath = this._GetActualFilePath (viewPath);
			masterPath = this._GetActualFilePath (masterPath);
			return base.CreateView (controllerContext, viewPath, masterPath);
		}

		//finds partial views by detecting matches
		protected override IView CreatePartialView (ControllerContext controllerContext, 
			string partialPath)
		{
			partialPath = this._GetActualFilePath (partialPath);
			return base.CreatePartialView (controllerContext, partialPath);
		}

		//perform a case-insensitive file search
		protected override bool FileExists (ControllerContext context, string virtualPath)
		{
			virtualPath = this._GetActualFilePath (virtualPath);
			return base.FileExists (context, virtualPath);
		}

		//determines (and caches) the actual path for a file
		private string _GetActualFilePath (string virtualPath)
		{

			//check if this has already been matched before
			if (RoutingRuleView._ViewPaths.ContainsKey (virtualPath))
				return RoutingRuleView._ViewPaths [virtualPath];

			//break apart the path
			string[] segments = virtualPath.Split (new char[] { '/' });

			//get the root folder to work from
			var folder = new DirectoryInfo (RoutingRuleView._Root);

			//start stepping up the folders to replace with the correct cased folder name
			for (int i = 0; i < segments.Length; i++) {
				string part = segments [i];
				bool last = i == segments.Length - 1;

				//ignore the root
				if (part.Equals ("~"))
					continue;

				//process the file name if this is the last segment
				else if (last)
					part = this._GetFileName (part, folder);

				//step up the directory for another part
				else
					part = this._GetDirectoryName (part, ref folder);

				//if no matches were found, just return the original string
				if (part == null || folder == null)
					return virtualPath;

				//update the segment with the correct name
				segments [i] = part;

			}

			//save this path for later use
			virtualPath = string.Join ("/", segments);
			RoutingRuleView._ViewPaths.Remove (virtualPath);
			RoutingRuleView._ViewPaths.Add (virtualPath, virtualPath);
			return virtualPath;
		}

		//searches for a matching file name in the current directory
		private string _GetFileName (string part, DirectoryInfo folder)
		{

			//try and find a matching file, regardless of case
			FileInfo match = folder.GetFiles ().FirstOrDefault (file => 
				file.Name.Equals (part, StringComparison.OrdinalIgnoreCase));
			return match is FileInfo ? match.Name : null;
		}

		//searches for a folder in the current directory and steps up a level
		private string _GetDirectoryName (string part, ref DirectoryInfo folder)
		{
			folder = folder.GetDirectories ().FirstOrDefault (dir => 
				dir.Name.Equals (part, StringComparison.OrdinalIgnoreCase));
			return folder is DirectoryInfo ? folder.Name : null;
		}

    }
}