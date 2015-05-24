using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;

namespace ALOS.Model
{
    ///<summary>
    ///V00004#学生表
    ///</summary>
    [Serializable]
    [TableName("V00004", "V00004#学生表","id")]
    public class StudentType
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
        ///Rid#帐号外键
        ///</summary>
        [MapName("Rid")]
        public Int64 Rid
        {
            get;
            set;
        }
        ///<summary>
        ///Name#学生名称
        ///</summary>
        [MapName("Name")]
        public String Name
        {
            get;
            set;
        }
        ///<summary>
        ///School#学校名称
        ///</summary>
        [MapName("School")]
        public String School
        {
            get;
            set;
        }
        ///<summary>
        ///SchoolId#学校编号
        ///</summary>
        [MapName("SchoolId")]
        public Int32 SchoolId
        {
            get;
            set;
        }
        ///<summary>
        ///College#学院名称
        ///</summary>
        [MapName("College")]
        public String College
        {
            get;
            set;
        }
        ///<summary>
        ///CollegeId#学院编号
        ///</summary>
        [MapName("CollegeId")]
        public Int32 CollegeId
        {
            get;
            set;
        }
        ///<summary>
        ///Email#学生邮箱
        ///</summary>
        [MapName("Email")]
        public String Email
        {
            get;
            set;
        }
        ///<summary>
        ///Phone#学生手机
        ///</summary>
        [MapName("Phone")]
        public String Phone
        {
            get;
            set;
        }
        ///<summary>
        ///LogDate#最后登录时间
        ///</summary>
        [MapName("LogDate")]
        public DateTime LogDate
        {
            get;
            set;
        }
        ///<summary>
        ///AppraisalNumber#测评次数
        ///</summary>
        [MapName("AppraisalNumber")]
        public Int32 AppraisalNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 学生测评年级
        /// </summary>
        [MapName("Grade")]
        public Int32 Grade
        {
            get;
            set;
        }
        ///<summary>
        ///Status#状态0静止登录1可以登录
        ///</summary>
        [MapName("Status")]
        public Boolean Status
        {
            get;
            set;
        }
        ///<summary>
        ///DataState 测评资料是否在处理中
        ///</summary>
        [MapName("DataState")]
        public Boolean DataState
        {
            get;
            set;
        }

        /// <summary>
        /// 如果类是泛型类 传入参数
        /// </summary>
        //  public Type[] GenericityParameter { set; get; }
        public string Model_Type
        {
            get { return this.GetType().FullName; }
        }
        [MapName("TeachingPlan", isSQL: false)]
        public string TeachingPlan { set; get; }
        [MapName("Courseware", isSQL: false)]
        public string Courseware { set; get; }
        [MapName("Reflection", isSQL: false)]
        public string Reflection { set; get; }
        [MapName("Video", isSQL: false)]
        public string Video { set; get; }
        [MapName("Apply", isSQL: false)]
        public bool Apply { set; get; }
        [MapName("TeachingName", isSQL: false)]
        public string TeachingName { set; get; }
        [MapName("CoursewareName", isSQL: false)]
        public string CoursewareName { set; get; }
        [MapName("ReflectionName", isSQL: false)]
        public string ReflectionName { set; get; }
        [MapName("VideoName", isSQL: false)]
        public string VideoName { set; get; }
        [MapName("LoginId", isSQL: false)]
        public string LoginId { set; get; }
        [MapName("PaperId", isSQL: false)]
        public int PaperId { set; get; }


    }
}
