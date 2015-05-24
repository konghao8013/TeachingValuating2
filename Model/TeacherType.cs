using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALOS.Model
{
    ///<summary>
    ///V00005#教师资料表
    ///</summary>
    [Serializable]
    [TableName("V00005", "V00005#教师资料表","id")]
    public class TeacherType
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
        ///Rid#教师帐号Id
        ///</summary>
        [MapName("Rid")]
        public Int64 Rid
        {
            get;
            set;
        }
        ///<summary>
        ///Name#教师名称
        ///</summary>
        [MapName("Name")]
        public String Name
        {
            get;
            set;
        }
        ///<summary>
        ///Email#教师邮箱
        ///</summary>
        [MapName("Email")]
        public String Email
        {
            get;
            set;
        }
        ///<summary>
        ///Phone#教师手机
        ///</summary>
        [MapName("Phone")]
        public String Phone
        {
            get;
            set;
        }
        ///<summary>
        ///School#所属学校名称
        ///</summary>
        [MapName("School")]
        public String School
        {
            get;
            set;
        }
        ///<summary>
        ///SchoolId#学校ID
        ///</summary>
        [MapName("SchoolId")]
        public Int32 SchoolId
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
        ///TypeId#评测教师类别
        ///</summary>
        [MapName("TypeId")]
        public Int32 TypeId
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
        ///State#是否启用
        ///</summary>
        [MapName("State")]
        public Boolean State
        {
            get;
            set;
        }
        ///<summary>
        ///Number#评卷次数
        ///</summary>
        [MapName("Number")]
        public Int32 Number
        {
            get;
            set;
        }
        ///<summary>
        ///TaskNumber#任务总量
        ///</summary>
        [MapName("TaskNumber")]
        public Int32 TaskNumber
        {
            get;
            set;
        }
        ///<summary>
        ///CollegeName#学院名称
        ///</summary>
        [MapName("CollegeName")]
        public String CollegeName
        {
            get;
            set;
        }
        /// <summary>
        /// 完成工作数量
        /// </summary>
        [MapName("Wnumber",isSQL:false)]
        public int WNumber { set; get; }
        [MapName("LoginId", isSQL: false)]
        public string LoginId { set; get; }
        /// <summary>
        /// 未完成工作数量
        /// </summary>
        [MapName("NWnumber", isSQL: false)]
        public int NWNumber { set; get; }
        /// <summary>
        /// 全部工作量
        /// </summary>
        [MapName("Qnumber", isSQL: false)]
        public int QNumber { set; get; }

        [MapName("teacherLoginId", isSQL: false)]
        public string teacherLoginId { set; get; }
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
