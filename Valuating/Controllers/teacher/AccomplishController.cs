using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using ALOS.Expand;
using ALOS.Model;

namespace Valuating.Controllers.teacher
{
    public class AccomplishController : GlobalTeacherController
    {
        //
        // GET: /Accomplish/

        public ActionResult Index()
        {
            return View("~/views/teacher/accomplish.cshtml");
        }

        [AcceptVerbs("POST", "Get")]
        public void SaveTeacherScore(int did, int createScore, string remark)
        {
            var evaluating = DB.EvaluatingPaperServer.Reader(did);

            evaluating.Innovate = createScore;
            evaluating.Remark = remark;
            evaluating.State = true;
            DB.EvaluatingPaperServer.Save(evaluating);
            var check = DB.EvaluatingPaperServer.CheckFinally(evaluating.PaperId);
            //如果评测完成开始比较两个教师评分差值 小于系统阈值 统计结果，否则抽取下一个专家
            if (check)
            {
                if (evaluating.TeacherTypeId < 4)
                {
                    var sys = DB.SysScoreServer.ReaderScoreType();
                    var list = DB.EvaluatingPaperServer.List("paperid", evaluating.PaperId);
                    var array = MinDifference(list, sys.DifferenceValue);
                    if (array != null)
                    {
                        var tlist = new List<EvaluatingPaperType>();
                        tlist.AddRange(array);
                        SaveStudentScore(tlist);
                    }
                    else
                    {
                        //阈值过大抽取下一个专家
                        var type = (EnumTeacherType) (DB.EvaluatingPaperServer.PaperIdCount(evaluating.PaperId) + 1);
                        
                        var teacher = DB.TeacherServer.ExtractTeacher(type, evaluating.StudentId);
                        // throw  new Exception("未完成AccomplishControll");


                        //未抽取到专家
                        teacher = teacher ?? new TeacherType() { TypeId = (int)type };
                        var stuRecord = DB.StudentRecordServer.Reader(evaluating.PaperId);
                        SavePaper(teacher, evaluating.StudentId, stuRecord);
                        if (teacher.Rid != 0)
                        {
                            VT.Send(teacher);
                        }
                        //



                    }

                }
                else
                {
                    var dlist = new List<EvaluatingPaperType>();
                    dlist.Add(evaluating);
                    SaveStudentScore(dlist);
                    //仲裁专家直接保存数据
                }

            }

        }
        /// <summary>
        /// 根据档案ID一票否决
        /// </summary>
        /// <param name="did"></param>
        [AcceptVerbs("POST","GET")]
        public void VoteVown(int did)
        {
            DB.EvaluatingPaperServer.VoteVown(did);
        }
   

        public void SavePaper(TeacherType teacher, long setudentId, StudentRecordType record)
        {
            var paper = new EvaluatingPaperType();
            paper.CreateTime = DateTime.Now;
            paper.TeacherTypeId = teacher.TypeId;
            paper.UserId = teacher.Rid;
            paper.StudentId = setudentId;
            paper.PaperId = record.Id;

            DB.EvaluatingPaperServer.Save(paper);
            //发送信息
            if(!string.IsNullOrEmpty(teacher.Phone))
            VT.Send(teacher);

        }
        /// <summary>
        /// 计算成绩
        /// </summary>
        /// <param name="evaluatings"></param>
        public void SaveStudentScore(List<EvaluatingPaperType> evaluatings)
        {

            var studentScore = DB.StudentScoreServer.Reader("paperId", evaluatings[0].PaperId);
            studentScore = studentScore ?? new StudentScoreType();
            var sturecord = DB.StudentRecordServer.Reader(evaluatings[0].PaperId);
            var student = DB.StudentServer.ReaderStudent(sturecord.Rid);
            studentScore.CoursewareScore = evaluatings.Average(a => a.CoursewareScore);
            studentScore.CreateTime = DateTime.Now;
            studentScore.Id = 0;
            studentScore.Innovate = evaluatings.Average(a => (decimal)a.Innovate);
            studentScore.Name = sturecord.TypeName;
            studentScore.PaperId = evaluatings[0].PaperId;
            studentScore.ReflectionScore = evaluatings.Average(a => a.ReflectionScore);
            studentScore.PresentScore = evaluatings.Average(a => a.RresentScore);
            studentScore.TeachingPlanScore = evaluatings.Average(a => a.TeachingPlanScore);
            studentScore.VideoScore = evaluatings.Average(a => a.VideoScore);

            studentScore.SumScore = studentScore.TeachingPlanScore + studentScore.CoursewareScore +
                                    studentScore.VideoScore + studentScore.ReflectionScore + studentScore.PresentScore + studentScore.Innovate;
            studentScore.Remark = evaluatings.Count == 2
                ? "教师一：" + evaluatings[0].Remark + "教师二：" + evaluatings[1].Remark
                : evaluatings[0].Remark;
            DB.StudentScoreServer.Save(studentScore);
            sturecord.Apply = true;
            sturecord.AppraisalNumber = student.AppraisalNumber + 1;
            DB.StudentRecordServer.Save(sturecord);
            student.AppraisalNumber = student.AppraisalNumber + 1;
            DB.StudentServer.Save(student);

        }

        /// <summary>
        /// 计算数组最小差值 并返回合格的评测档案
        /// </summary>
        /// <param name="array"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public EvaluatingPaperType[] MinDifference(List<EvaluatingPaperType> list, decimal minValue)
        {
            var array = new EvaluatingPaperType[2];
            list = list.OrderBy(a => a.SumScore).ToList();
            decimal min = 100;
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i - 1].SumScore - list[i].SumScore < min)
                {
                    min = list[i].SumScore - list[i-1].SumScore;
                    array[0] = list[i - 1];
                    array[1] = list[i];
                }
            }
            if (min > minValue)
            {
                return null;
            }
            return array;
        }

    }
}
