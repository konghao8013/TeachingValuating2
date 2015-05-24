using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALOS.Model
{
    ///<summary>
    ///V00008#基本配置
    ///</summary>
    [Serializable]
    [TableName("V00008", "V00008#基本配置","id")]
    public class SysSettingType
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
        ///Name#系统名称
        ///</summary>
        [MapName("Name")]
        public String Name
        {
            get;
            set;
        }
        ///<summary>
        ///LoginHint#登录页提示
        ///</summary>
        [MapName("LoginHint")]
        public String LoginHint
        {
            get;
            set;
        }
        ///<summary>
        ///Phone#监控手机
        ///</summary>
        [MapName("Phone")]
        public String Phone
        {
            get;
            set;
        }
        ///<summary>
        ///TestGrade#运行测评年级
        ///</summary>
        [MapName("TestGrade")]
        public Int32 TestGrade
        {
            get;
            set;
        }
        ///<summary>
        ///IsDebug#收发短信
        ///</summary>
        [MapName("IsDebug")]
        public Boolean IsDebug
        {
            get;
            set;
        }
        ///<summary>
        ///SchoolIco#校徽地址
        ///</summary>
        [MapName("SchoolIco")]
        public String SchoolIco
        {
            get;
            set;
        }
        ///<summary>
        ///HelpVideo#帮助视频地址
        ///</summary>
        [MapName("HelpVideo")]
        public String HelpVideo
        {
            get;
            set;
        }
        /// <summary>
        /// 软件下载
        /// </summary>
        [MapName("DownLoadFile")]
        public string DownLoadFile { set; get; }
        [MapName("StudentLogin")]
        public bool StudentLogin { set; get; }
        [MapName("TeacherLogin")]
        public bool TeacherLogin { set; get; }

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
