using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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