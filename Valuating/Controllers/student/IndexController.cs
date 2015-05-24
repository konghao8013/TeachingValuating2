using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ALOS.Model;

namespace Valuating.Controllers.student
{
    public class IndexController : GlobalStudentController
    {

        public ActionResult Index()
        {
            var student = DB.StudentServer.ReaderStudent(VT.Users.Id);
            var paperData = DB.StudentRecordServer.Reader("rid", VT.Users.Id);
           
            if (paperData != null && paperData.Apply)
            {
                return Redirect("/student/score");
            }
            if (student.DataState)
            {
                return Redirect("/student/wait");
            }

            return View("~/views/student/index.cshtml");
        }
        [AcceptVerbs("post", "get")]
        public string SysName()
        {
            return VT.SystemName.Serialize();
        }
        /// <summary>
        /// 返回测评资料状态
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("POST","GET")]
        public string GetDataState()
        {
            var studentData = DB.StudentRecordServer.RenderRecord(VT.Users.Id) ?? new StudentRecordType{Rid = (int)VT.Users.Id};

            var state= studentData.TeachingSize==0?1:studentData.CoursewareSize==0?2:studentData.VideoSize==0?3:studentData.RefiectionSize==0?4:0;
            return state.Serialize();
        }
        /// <summary>
        /// 学生测评申请
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("POST","GET")]
        public string Apply()
        {
            var message = new StringBuilder();
            var sys = DB.SysScoreServer.ReaderScoreType();
            var date = DateTime.Now;
            if (date < sys.BatchEndDate && date > sys.BatchStateDate)
            {
                var user = VT.Users;
                var record = DB.StudentRecordServer.RenderRecord(VT.Users.Id);
             
                if (record != null&&record.TeachingSize>0&&record.CoursewareSize>0&&record.VideoSize>0&&record.RefiectionSize>0)
                {
                    var teacher1 = DB.TeacherServer.ExtractTeacher(EnumTeacherType.QAMS1, user.Id);
                    var teacher2 = DB.TeacherServer.ExtractTeacher(EnumTeacherType.QAMS2, user.Id);
                    if (teacher1 != null && teacher2 != null)
                    {
                        var student = DB.StudentServer.ReaderStudent(user.Id);
                        SavePaper(teacher1,User.Id,record);
                        SavePaper(teacher2, User.Id, record);
                        record.TypeName = sys.BatchName;
                        record.State = true;
                        record.AppraisalNumber = student.AppraisalNumber+1;
                      
                        student.DataState = true;

                        DB.StudentServer.Save(student);
                        DB.StudentRecordServer.Save(record);
                        message.AppendLine("1");
                    }
                    else
                    {
                        message.AppendLine("未抽取到合适的测评专家。请稍后再试");
                    }

                }
                else
                {
                    message.AppendLine("请上传所有教学材料");
                }
            }
            else
            {
                message.AppendLine("当前时间不允许申请测评");
                message.AppendLine("开始时间"+sys.BatchStateDate.ToString("yyyy年MM月dd日"));
                message.AppendLine("结束时间"+sys.BatchEndDate.ToString("yyyy年MM月dd日"));
            }
            return message.ToString().Serialize();
        }

        public void SavePaper(TeacherType teacher,long setudentId,StudentRecordType record)
        {
            var paper = new EvaluatingPaperType();
            paper.CreateTime = DateTime.Now;
            paper.TeacherTypeId = teacher.TypeId;
            paper.UserId = teacher.Rid;
            paper.StudentId = setudentId;
            paper.PaperId = record.Id;

            DB.EvaluatingPaperServer.Save(paper);
            //发送信息
            VT.Send(teacher);
         
        }

        /// <summary>
        /// 获得当前学生测评资料
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("POST","GET")]
        public string GetData()
        {
            var studentData = DB.StudentRecordServer.RenderRecord(VT.Users.Id) ?? new StudentRecordType { Rid = (int)VT.Users.Id };
            return studentData.Serialize();
        }

        [AcceptVerbs("post", "get")]
        public string SaveData(string value, UploadFileType data)
        {
            var type = (EnumStudentDataType)value.ToInt32();
            var studentData = DB.StudentRecordServer.RenderRecord(VT.Users.Id) ?? new StudentRecordType(){Rid = (int)VT.Users.Id};
            switch (type)
            {
                case EnumStudentDataType.TeachingPlan:
                    studentData.TeachingName = data.Name;
                    studentData.TeachingPlan = data.Url;
                    studentData.TeachingSize = data.Size;
                    studentData.TeachingPlanTime = data.Date;

                    break;
                case EnumStudentDataType.Courseware:
                    studentData.CoursewareName = data.Name;
                    studentData.Courseware = data.Url;
                    studentData.CoursewareSize = data.Size;
                    studentData.CoursewareTime = data.Date;
                    break;
                case EnumStudentDataType.Video:
                    studentData.VideoName = data.Name;
                    studentData.Video = data.Url;
                    studentData.VideoSize = data.Size;
                    studentData.VideoTime = data.Date;
                    break;
                case EnumStudentDataType.Reflection:
                    studentData.ReflectionName = data.Name;
                    studentData.Reflection = data.Url;
                    studentData.RefiectionSize = data.Size;
                    studentData.ReflectionTime = data.Date;
                    break;

            }
            DB.StudentRecordServer.Save(studentData);
            return "OK".Serialize();
        }

    }
}
