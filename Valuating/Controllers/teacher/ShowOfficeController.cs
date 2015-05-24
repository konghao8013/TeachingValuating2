using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALOS.Model;

namespace Valuating.Controllers.teacher
{
    public class ShowOfficeController : GlobalTeacherController
    {
        //
        // GET: /ShowOffice/

        public ActionResult Index()
        {
            return View("~/views/teacher/showoffice.cshtml");
        }

        [AcceptVerbs("POST", "GET")]
        public String GetUrl(int pid, int typeId)
        {
            var type = (EnumStudentDataType) typeId;
            var record=DB.StudentRecordServer.Reader(pid);
            switch (type)
            {
                case  EnumStudentDataType.TeachingPlan:
                    return record.TeachingPlan.Serialize();
                    break;
                case EnumStudentDataType.Courseware:
                    return record.Courseware.Serialize();
                    break;
                case EnumStudentDataType.Reflection:
                    return record.Reflection.Serialize();
                    break;
                case EnumStudentDataType.Video:
                    return record.Video.Serialize();
                    break;

            }
            return "";
        }

    }
}
