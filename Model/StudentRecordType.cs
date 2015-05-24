using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALOS.Model
{
    ///<summary>
    ///V00009#学生档案表
    ///</summary>
    [Serializable]
    [TableName("V00009", "V00009#学生档案表","id")]
    public class StudentRecordType
    {
        ///<summary>
        ///Rid#UserId
        ///</summary>
        [MapName("Rid")]
        public Int32 Rid
        {
            get;
            set;
        }
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
        ///TeachingPlan#教案
        ///</summary>
        [MapName("TeachingPlan")]
        public String TeachingPlan
        {
            get;
            set;
        }
        ///<summary>
        ///Courseware#课件
        ///</summary>
        [MapName("Courseware")]
        public String Courseware
        {
            get;
            set;
        }
        ///<summary>
        ///Reflection#反思
        ///</summary>
        [MapName("Reflection")]
        public String Reflection
        {
            get;
            set;
        }
        ///<summary>
        ///Video#视频
        ///</summary>
        [MapName("Video")]
        public String Video
        {
            get;
            set;
        }
        ///<summary>
        ///TeachingSize#教案文件大小
        ///</summary>
        [MapName("TeachingSize")]
        public Int64 TeachingSize
        {
            get;
            set;
        }
        ///<summary>
        ///CoursewareSize#课件文件大小
        ///</summary>
        [MapName("CoursewareSize")]
        public Int64 CoursewareSize
        {
            get;
            set;
        }
        ///<summary>
        ///VideoSize#视频文件大小
        ///</summary>
        [MapName("VideoSize")]
        public Int64 VideoSize
        {
            get;
            set;
        }
        ///<summary>
        ///RefiectionSize#反思文件大小
        ///</summary>
        [MapName("RefiectionSize")]
        public Int64 RefiectionSize
        {
            get;
            set;
        }
        ///<summary>
        ///State#是否申请测评
        ///</summary>
        [MapName("State")]
        public Boolean State
        {
            get;
            set;
        }
        ///<summary>
        ///EndDateTime#结束上传时间
        ///</summary>
        [MapName("EndDateTime")]
        public DateTime EndDateTime
        {
            get;
            set;
        }
        ///<summary>
        ///TypeName#归档名称
        ///</summary>
        [MapName("TypeName")]
        public String TypeName
        {
            get;
            set;
        }
        ///<summary>
        ///TeachingName#教案文件名称
        ///</summary>
        [MapName("TeachingName")]
        public String TeachingName
        {
            get;
            set;
        }
        ///<summary>
        ///CoursewareName#课件文件名称
        ///</summary>
        [MapName("CoursewareName")]
        public String CoursewareName
        {
            get;
            set;
        }
        ///<summary>
        ///ReflectionName#反思文件名称
        ///</summary>
        [MapName("ReflectionName")]
        public String ReflectionName
        {
            get;
            set;
        }
        ///<summary>
        ///Video#视频#文件名称
        ///</summary>
        [MapName("VideoName")]
        public String VideoName
        {
            get;
            set;
        }
        ///<summary>
        ///TeachingPlanTime#教案上传时间
        ///</summary>
        [MapName("TeachingPlanTime")]
        public DateTime TeachingPlanTime
        {
            get;
            set;
        }
        ///<summary>
        ///CoursewareTime#课件上传时间
        ///</summary>
        [MapName("CoursewareTime")]
        public DateTime CoursewareTime
        {
            get;
            set;
        }
        ///<summary>
        ///ReflectionTime#反思上传时间
        ///</summary>
        [MapName("ReflectionTime")]
        public DateTime ReflectionTime
        {
            get;
            set;
        }
        ///<summary>
        ///VideoTime#视频上传时间
        ///</summary>
        [MapName("VideoTime")]
        public DateTime VideoTime
        {
            get;
            set;
        }
        /// <summary>
        /// 抽取档案ID
        /// </summary>
        [MapName("Did", isSQL: false)]
        public int Did { set; get; }
        /// <summary>
        /// 教师类别
        /// </summary>
        [MapName("TeacherTypeId",isSQL:false)]
        public int TeacherTypeId { set; get; }
        /// <summary>
        /// 试卷数量
        /// </summary>
        [MapName("Number", isSQL: false)]
        public int Number { set; get; }
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


        /// <summary>
        /// 该档案是否被禁用
        /// </summary>
        [MapName("Disable")]
        public bool Disable { set; get; }



        /// <summary>
        /// 申请的测评资料是否完成
        /// </summary>
        [MapName("apply")]
        public bool Apply { set; get; }

        /// <summary>
        /// 如果类是泛型类 传入参数
        /// </summary>
        //  public Type[] GenericityParameter { set; get; }
        public string Model_Type
        {
            get { return this.GetType().FullName; }
        }
        /// <summary>
        /// 测评次数
        /// </summary>
        [MapName("AppraisalNumber")]
        public int AppraisalNumber { set; get; }
    }
}
