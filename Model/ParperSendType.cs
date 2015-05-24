using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALOS.Model
{
    ///<summary>
    ///V00015#试卷退回申请
    ///</summary>
    [Serializable]
    [TableName("V00015", "V00015#试卷退回申请", "id")]
    public class ParperSendType
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
        ///TeacherId#教师编号
        ///</summary>
        [MapName("TeacherId")]
        public Int64 TeacherId
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
        ///CreateTime#申请时间
        ///</summary>
        [MapName("CreateTime")]
        public DateTime CreateTime
        {
            get;
            set;
        }
        ///<summary>
        ///Content#退回原因
        ///</summary>
        [MapName("Content")]
        public String Content
        {
            get;
            set;
        }
        ///<summary>
        ///State#是否同意
        ///</summary>
        [MapName("State")]
        public Boolean State
        {
            get;
            set;
        }
        /// <summary>
        /// 是否同意打回
        /// </summary>
        [MapName("ISOK")]
        public Boolean IsOK { set; get; }
        public string Model_Type
        {
            get { return this.GetType().FullName; }
        }

        [MapName("TeacherName",isSQL:false)]
        public string TeacherName { set; get; }
        [MapName("StudentName", isSQL: false)]
        public string StudentName { set; get; }
        [MapName("TeachingPlan", isSQL: false)]
        public string TeachingPlan { set; get; }
        [MapName("Courseware", isSQL: false)]
        public string Courseware { set; get; }
        [MapName("Reflection", isSQL: false)]
        public string Reflection { set; get; }
        [MapName("Video", isSQL: false)]
        public string Video { set; get; }
        [MapName("TypeName", isSQL: false)]
        public string TypeName { set; get; }
        [MapName("TeachingName", isSQL: false)]
        public string TeachingName { set; get; }
        [MapName("CoursewareName", isSQL: false)]
        public string CoursewareName { set; get; }
        [MapName("VideoName", isSQL: false)]
        public string VideoName { set; get; }
        [MapName("ReflectionName", isSQL: false)]
        public string ReflectionName { set; get; }
    }
}
