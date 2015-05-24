using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALOS.Model
{

    ///<summary>
    ///V00010#抽取教师档案表
    ///</summary>
    [Serializable]
    [TableName("V00010", "V00010#抽取教师档案表", "id")]
    public class EvaluatingPaperType
    {
        ///<summary>
        ///Id#编号
        ///</summary>
        [MapName("Id", true, true)]
        public Int32 Id
        {
            get;
            set;
        }
        ///<summary>
        ///UserId#教师编号
        ///</summary>
        [MapName("UserId")]
        public long UserId
        {
            get;
            set;
        }
        ///<summary>
        ///PaperId#试卷编号
        ///</summary>
        [MapName("PaperId")]
        public Int32 PaperId
        {
            get;
            set;
        }
        ///<summary>
        ///StudentId#学生编号
        ///</summary>
        [MapName("StudentId")]
        public long StudentId
        {
            get;
            set;
        }
        ///<summary>
        ///TeachingPlanScore#教案分数
        ///</summary>
        [MapName("TeachingPlanScore")]
        public Decimal TeachingPlanScore
        {
            get;
            set;
        }
        ///<summary>
        ///TeachingPlanTime#教案评测时间
        ///</summary>
        [MapName("TeachingPlanTime")]
        public Int32 TeachingPlanTime
        {
            get;
            set;
        }
        ///<summary>
        ///CoursewareScore#课件分数
        ///</summary>
        [MapName("CoursewareScore")]
        public Decimal CoursewareScore
        {
            get;
            set;
        }
        ///<summary>
        ///CoursewareTime#课件评测时间
        ///</summary>
        [MapName("CoursewareTime")]
        public Int32 CoursewareTime
        {
            get;
            set;
        }
        ///<summary>
        ///VideoScore#视频分数
        ///</summary>
        [MapName("VideoScore")]
        public Decimal VideoScore
        {
            get;
            set;
        }
        /// <summary>
        /// 测评分数
        /// </summary>
        [MapName("RresentScore")]
        public Decimal RresentScore { set; get; }
        /// <summary>
        /// 测评时间
        /// </summary>
        [MapName("PresentTime")]
        public int PresentTime { set; get; }

        ///<summary>
        ///VideoTime#视频评测时间
        ///</summary>
        [MapName("VideoTime")]
        public Int32 VideoTime
        {
            get;
            set;
        }
        ///<summary>
        ///ReflectionScore#反思分数
        ///</summary>
        [MapName("ReflectionScore")]
        public Decimal ReflectionScore
        {
            get;
            set;
        }
        ///<summary>
        ///ReflectionTime#反思评测时间
        ///</summary>
        [MapName("ReflectionTime")]
        public Int32 ReflectionTime
        {
            get;
            set;
        }
        ///<summary>
        ///State#是否评测完成
        ///</summary>
        [MapName("State")]
        public Boolean State
        {
            get;
            set;
        }
        ///<summary>
        ///CreateTime#试卷提交时间
        ///</summary>
        [MapName("CreateTime")]
        public DateTime CreateTime
        {
            get;
            set;
        }


        ///<summary>
        ///EndTime#试卷评测完成时间
        ///</summary>
        [MapName("EndTime")]
        public DateTime EndTime
        {
            get;
            set;
        }
        ///<summary>
        ///Innovate#创新得分
        ///</summary>
        [MapName("Innovate")]
        public Int32 Innovate
        {
            get;
            set;
        }
        /// <summary>
        /// SumScore评测总分
        /// </summary>

        public decimal SumScore
        {
            get
            {
                return TeachingPlanScore + CoursewareScore + VideoScore + ReflectionScore +
                                  RresentScore + Innovate;
            }
        }
        /// <summary>
        /// 返回评测时间
        /// </summary>
        public int SumTime
        {
            get { return (TeachingPlanTime + CoursewareTime + VideoTime + ReflectionTime); }
        }

        ///<summary>
        ///Remark#教师评语
        ///</summary>
        [MapName("Remark")]
        public String Remark
        {
            get;
            set;
        }
        [MapName("UserName", isSQL: false)]
        public string UserName { set; get; }
        /// <summary>
        /// 禁用试卷
        /// </summary>
        [MapName("disable")]
        public bool Disable { set; get; }
        /// <summary>
        /// 评测教师类别 1评教师校内 一评教师校外 二评教师 仲裁专家
        /// </summary>
        [MapName("TeacherTypeId")]
        public int TeacherTypeId { set; get; }

        /// <summary>
        /// 如果类是泛型类 传入参数
        /// </summary>
        //  public Type[] GenericityParameter { set; get; }
        public string Model_Type
        {
            get { return this.GetType().FullName; }
        }

        [MapName("TeacherName", isSQL: false)]
        public string TeacherName { set; get; }
        [MapName("StudentName", isSQL: false)]
        public string StudentName { set; get; }
        [MapName("LoginId", isSQL: false)]
        public string LoginId { set; get; }
        [MapName("TypeName", isSQL: false)]
        public string TypeName { set; get; }
        [MapName("TeacherLoginId", isSQL: false)]
        public string TeacherLoginId { set; get; }
        [MapName("College", isSQL: false)]
        public string College { set; get; }
        [MapName("School", isSQL: false)]
        public string School { set; get; }
          [MapName("TeacherPhone", isSQL: false)]
        public string TeacherPhone { set; get; }
        //b.TeachingPlan,b.TeachingName,b.Courseware,b.CoursewareName,b.Video,b.VideoName,b.Reflection,b.ReflectionName
          [MapName("TeachingPlan", isSQL: false)]
          public string TeachingPlan { set; get; }
          [MapName("TeachingName", isSQL: false)]
          public string TeachingName { set; get; }
          [MapName("Courseware", isSQL: false)]
          public string Courseware { set; get; }
          [MapName("CoursewareName", isSQL: false)]
          public string CoursewareName { set; get; }
          [MapName("Video", isSQL: false)]
          public string Video { set; get; }
          [MapName("VideoName", isSQL: false)]
          public string VideoName { set; get; }
          [MapName("Reflection", isSQL: false)]
          public string Reflection { set; get; }
          [MapName("ReflectionName", isSQL: false)]
          public string ReflectionName { set; get; }
       
    }

}
