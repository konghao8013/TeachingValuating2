using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using ALOS.Expand;
using ALOS.Model;
using Valuating.Controllers;

namespace Valuating.Admin
{
    /// <summary>
    /// InputUser 的摘要说明
    /// </summary>
    public class InputUser : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (VT.Users == null || VT.Users.TypeId != 3)
            {
                return;
                
            }
            var message = "账户数据导入成功";
            if (context.Request.Files.Count > 0)
            {
                var typeId = (EnumUserType)(context.Request.Params["typeId"].ToInt32());
                var file = context.Request.Files[0];
                var path =
                    context.Server.MapPath("~/resource/upload/" + Guid.NewGuid().ToString("N") + "." +
                                           file.FileName.Split('.').Last());
                file.SaveAs(path);
                message=InputStudent(path, typeId);
            }
            context.Response.Write(message);

        }

        public string InputStudent(string  path,EnumUserType userType)
        {

            var message = new StringBuilder();
            var table = ExcelExpand.GetExcel(path, "inputuser");
            if (table == null)
            {
                message.AppendLine("excel文档不符合导入模版,未找到inputuser表格");
                return message.ToString();
            }
            var rowLength = table.Rows.Count;
            var listUser = new List<UserType>();
            for (var i = 0; i < rowLength; i++)
            {
                var row = table.Rows[i];
                var user = new UserType();
                user.LoginId = (row["LoginId"]+"").Trim();
                user.Name = row["Name"] + "";
                user.Password = row["Password"]+"";
                user.CreateDate = DateTime.Now;
                user.LoginDate = DateTime.Now;
                user.UserGuid = Guid.NewGuid();
                user.TypeId = (int)userType;
                if (DB.UserServer.SelectUser(user.LoginId))
                {
                    message.AppendLine("登录账户：" + user.LoginId + "重复导入失败\r\n");
                   
                    continue;
                    
                }
                user=DB.UserServer.Save(user);
                var schoolName = row["SchoolName"] + "";
                var collegeName = row["CollegeName"] + "";
                var school = DB.SchoolServer.ReaderSchoolId(schoolName, collegeName);
                if (school == null)
                {
                    message.AppendLine("账户:" + user.LoginId + "添加失败,未找到指定的学院");
                    DB.UserServer.Delete(user.Id);
                    continue;
                }
                switch (userType)
                {
                    case EnumUserType.Student:
                        var student = new StudentType();
                        student.Rid = user.Id;
                        student.Name = user.Name;
                        student.School = row["SchoolName"] + "";
                        student.College = row["CollegeName"] + "";
                        student.Email = row["Email"] + "";
                        student.Phone = row["Phone"]+"";
                        student.Grade = row["Grade"].ToString().ToInt32();
                        student.LogDate = DateTime.Now;
                        student.AppraisalNumber = 0;
                        student.Status = row["state"].ToString().ToInt32() == 1;
                       
                        student.SchoolId = school.ObjId;
                        student.CollegeId = school.Id;
                        DB.StudentServer.Save(student);

                        break;
                    case EnumUserType.Teacher:
                        var teacher = new TeacherType();
                        teacher.Rid = user.Id;
                        teacher.Name = user.Name;
                        teacher.Email = row["Email"] + "";
                        teacher.Phone = row["Phone"] + "";
                        teacher.School = row["SchoolName"] + "";
                        teacher.CollegeName = row["CollegeName"] + "";
                        teacher.SchoolId = school.ObjId;
                        teacher.CollegeId = school.Id;
                        teacher.LogDate = DateTime.Now;
                        teacher.State = row["state"].ToString().ToInt32() == 1;
                        teacher.TaskNumber = 0;
                        DB.TeacherServer.Save(teacher);
                        break;
                }

            }
            

            return message.ToString();
        }
       

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}