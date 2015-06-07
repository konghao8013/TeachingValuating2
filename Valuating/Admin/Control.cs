using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;
using Valuating.Controllers;

namespace Valuating.Admin
{
   
    public class Control:Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (VT.Users == null)
                {
                    Response.Redirect("~/index");
                }
                IsNotPostLoad();
            }
        }
        /// <summary>
        /// 第一次加载
        /// </summary>
        public virtual void IsNotPostLoad()
        {
            
        }
    }
}

public static class ScriptOver {
   public static IHtmlString Render(params string[] paths){
       //BundleTable.Bundles
       paths = Adjustment(paths);
       return Scripts.Render(paths);
    }
   static string[] Adjustment(string[] paths) {
       var list = new List<string>();
       foreach (var path in paths) {
           var bundle = BundleTable.Bundles.FirstOrDefault(a => a.Path.ToLower() == path.ToLower());
           if (bundle != null) {
               list.Add(bundle.Path);
           }
       }
       return list.ToArray();
   }
}