using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALOS.Model
{
    ///<summary>
    ///V00014#学生成绩结果分
    ///</summary>
    [Serializable]
    [TableName("V00014", "V00014#学生成绩结果分","id")]
    public class StudentScoreType
    {
        ///<summary>
        ///Id#编号
        ///</summary>
        [MapName("Id",true,true)]
        public Int32 Id
        {
            get;
            set;
        }
        ///<summary>
        ///Name#评测档案评测
        ///</summary>
        [MapName("Name")]
        public String Name
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
        ///TeachingPlanScore#教案分数
        ///</summary>
        [MapName("TeachingPlanScore")]
        public Decimal TeachingPlanScore
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
        ///VideoScore#视频分数
        ///</summary>
        [MapName("VideoScore")]
        public Decimal VideoScore
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
        ///Innovate#创新得分
        ///</summary>
        [MapName("Innovate")]
        public Decimal Innovate
        {
            get;
            set;
        }
        ///<summary>
        ///PresentScore#说课分数
        ///</summary>
        [MapName("PresentScore")]
        public Decimal PresentScore
        {
            get;
            set;
        }
        ///<summary>
        ///SumScore#评测总分
        ///</summary>
        [MapName("SumScore")]
        public Decimal SumScore
        {
            get;
            set;
        }
        ///<summary>
        ///CreateTime#评测完成时间
        ///</summary>
        [MapName("CreateTime")]
        public DateTime CreateTime
        {
            get;
            set;
        }

        /// <summary>
        /// 教师评语
        /// </summary>
        [MapName("Remark")]
        public String Remark { set; get; }

        /// <summary>
        /// 学院名称
        /// </summary>
        [MapName("College", isSQL: false)]
        public string College { set; get; }
        /// <summary>
        /// 学校名称
        /// </summary>
        [MapName("School", isSQL: false)]
        public string School { set; get; }

        /// <summary>
        /// 学生名称
        /// </summary>
        [MapName("StudentName", isSQL: false)]
        public string StudentName { set; get; }

        /// <summary>
        /// 学生登录编号
        /// </summary>

        [MapName("StudentLoginId", isSQL: false)]
        public string StudentLoginId { set; get; }
        [MapName("AppraisalNumber", isSQL: false)]
        public int AppraisalNumber { set; get; }
        
        /// <summary>
        /// 如果类是泛型类 传入参数
        /// </summary>
        //  public Type[] GenericityParameter { set; get; }
        public string Model_Type
        {
            get { return this.GetType().FullName; }
        }
    }
}
