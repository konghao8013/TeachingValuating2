using ALOS.DALSERVER;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Optimization;
using WebGrease.Css.Extensions;

namespace Valuating
{
    public class BundleConfig 
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            var path = System.AppDomain.CurrentDomain.BaseDirectory;
          
            var dbserver = new ScriptBundle("~/bundle/super").Include(
                "~/Resource/Scripts/jquery-1.8.3.js",
                "~/Resource/Scripts/lodash.compat.js",
                "~/Resource/Scripts/extjs/js/ext-all-debug.js",
               
                "~/Resource/Scripts/extjs/js/ux/TabCloseMenu.js",
                "~/Resource/Scripts/extjs/js/ux/IFrame.js",
                "~/Resource/Scripts/extjs/zh_CN/ext-locale-zh_CN.js",
                "~/Resource/Scripts/JqueryFn.js",
                "~/Resource/Scripts/model.js",
                "~/Resource/Scripts/DB.js"
              );
            dbserver.Include("~/Resource/Scripts/ExtjsFn.js");
           
            AddServerModel(path, dbserver);


            var commonality = new ScriptBundle("~/bundle/commonality").Include(
                "~/Resource/Scripts/jquery-1.8.3.js",
                "~/Resource/Scripts/lodash.compat.js",
                "~/Resource/Scripts/extjs/js/ext-all-debug.js",
                "~/Resource/Scripts/extjs/js/ux/TabCloseMenu.js",
                "~/Resource/Scripts/extjs/js/ux/IFrame.js",
                "~/Resource/Scripts/extjs/zh_CN/ext-locale-zh_CN.js",
                 "~/Resource/Scripts/ajaxfileupload.js",
                "~/Resource/Scripts/JqueryFn.js",
                "~/Resource/Scripts/model.js",
                "~/Resource/Scripts/DB.js",
                "~/Resource/Scripts/_student.js",
                   "~/Resource/Scripts/ExtjsFn.js"

              );
            AddServerModel(path, commonality);

            var teacher = new ScriptBundle("~/bundle/teacher").Include(
               "~/Resource/Scripts/jquery-1.8.3.js",
               "~/Resource/Scripts/lodash.compat.js",
               "~/Resource/Scripts/extjs/js/ext-all-debug.js",
               "~/Resource/Scripts/extjs/js/ux/TabCloseMenu.js",
               "~/Resource/Scripts/extjs/js/ux/IFrame.js",
               "~/Resource/Scripts/extjs/zh_CN/ext-locale-zh_CN.js",
                "~/Resource/Scripts/ajaxfileupload.js",
               "~/Resource/Scripts/JqueryFn.js",
               "~/Resource/Scripts/model.js",
               "~/Resource/Scripts/DB.js",
               "~/Resource/Scripts/_teacher.js"

             );
            AddServerModel(path, teacher);
            var login = new ScriptBundle("~/bundle/login").Include(
               "~/Resource/Scripts/jquery-1.8.3.js",
               "~/Resource/Scripts/lodash.compat.js",
               "~/Resource/Scripts/extjs/js/ext-all-debug.js",
               "~/Resource/Scripts/extjs/js/ux/TabCloseMenu.js",
               "~/Resource/Scripts/extjs/js/ux/IFrame.js",
               "~/Resource/Scripts/extjs/zh_CN/ext-locale-zh_CN.js",
                "~/Resource/Scripts/ajaxfileupload.js",
               "~/Resource/Scripts/JqueryFn.js",
               "~/Resource/Scripts/model.js",
               "~/Resource/Scripts/DB.js"

             );

            AddServerModel(path, login);


            bundles.Add(dbserver);
            bundles.Add(commonality);
            bundles.Add(teacher);
            bundles.Add(login);
            //~/Resource/Scripts/lodash.compat.js
        //    bundles.Add(new ScriptBundle("~/bundle/super/userManager").Include("~/Resource/Scripts/viewScripts/super/UserMessage.js"));

            bundles.Add(new StyleBundle("~/styles/css/ext", "~/Resource/Scripts/extjs/css/ext-theme-neptune-touch/ext-theme-neptune-touch-all.css"));

            bundles.Add(new ScriptBundle("~/scripts/login").Include("~/Resource/Scripts/jquery-1.8.3.js", "~/Resource/Scripts/JqueryFn.js"));
           
        
            path = Path.GetFullPath(path + "Resource/Scripts/viewScripts");
            
           
             var directory=new DirectoryInfo(path);

            AddBundles(bundles, directory,"~/bundle/",path);
        }

        private static void AddServerModel(string path, Bundle dbserver)
        {
            //var dicModel = new DirectoryInfo(Path.Combine(path, "Resource", "Scripts", "model"));
            //dicModel.GetFiles("*.js").Select(a => a.Name).ForEach(a =>
            //{
            //    dbserver.Include(Path.Combine("~/Resource/Scripts/model", a));
            //});
            //var dicDbserver = new DirectoryInfo(Path.Combine(path, "Resource", "Scripts", "dbserver"));
            //dicDbserver.GetFiles("*.js").Select(a => a.Name).ForEach(a =>
            //{
            //    dbserver.Include(Path.Combine("~/Resource/Scripts/model", a));
            //});
            dbserver.IncludeDirectory("~/Resource/Scripts/model","*.js");
            dbserver.IncludeDirectory("~/Resource/Scripts/dbserver", "*.js");
            dbserver.Include(Path.Combine("~/Resource/Scripts/js/DBSource.js"));
            dbserver.Include(Path.Combine("~/Resource/Scripts/js/WebServerLog.js"));
        }

        public static void AddBundles(BundleCollection bundles, DirectoryInfo directory,string path,string tpath)
        {
            foreach (var f in directory.GetFiles())
            {
                if (f.DirectoryName != null)
                {
                    if (f.DirectoryName.ToLower().IndexOf("student") > -1)
                    {

                    }
                  
                    var dname = f.DirectoryName.Replace(tpath, "").Replace("\\", "/");
                    var jpath = "~/Resource/Scripts/viewScripts" + dname + "/" + f.Name;
                 
                    bundles.Add(new ScriptBundle((path + f.Name.Replace(".js", ""))).Include(jpath));
                }
            }
            foreach (var d in directory.GetDirectories())
            {
              //  path +=d.Name+"/";
                AddBundles(bundles, d, path+d.Name+"/",tpath);
            }
           
        }
    }
}