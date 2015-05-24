using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALOS.Model;

namespace Valuating.Controllers
{
    public class IndexController : GlobalController
    {
        public IndexController()
        {
           
        }

        //
        // GET: /Index/

        public ActionResult Index()
        {
           
            string skipUri = "";
            var user=VT.Users??new UserType();
          
                switch (user.GetUserType())
                {
                    case EnumUserType.Student:
                        skipUri = "student/Index";
                        break;
                    case EnumUserType.Teacher:
                        skipUri = "teacher/Index";
                        break;
                    case EnumUserType.Admin:
                        skipUri = "super/Index";
                       
                        break;
                    default:
                        skipUri = "/login";
                        break;
                }
                return Redirect(skipUri);
              //  return Redirect("teacher/index");
        
          
        }

    }
}
