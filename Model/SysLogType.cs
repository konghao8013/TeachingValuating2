using ALOS.Expand;
using System;

namespace ALOS.Model
{
    ///<summary>
    ///SysLog#系统日志
    ///</summary>
   [TableName("SysLog","系统日志","id")]
    public class SysLogType
    {
        ///<summary>
        ///Id#编号
        ///</summary>
        [MapName("Id",isKey:true,isIdentity:true)]
        public Int64 Id
        {
            get;
            set;
        }
        ///<summary>
        ///LogType#类别
        ///</summary>
        [MapName("LogType")]
        public String LogType
        {
            get;
            set;
        }
        ///<summary>
        ///Message#消息
        ///</summary>
        [MapName("Message")]
        public String Message
        {
            get;
            set;
        }
        ///<summary>
        ///Source#消息来源
        ///</summary>
        [MapName("Source")]
        public String Source
        {
            get;
            set;
        }
        ///<summary>
        ///CreateTime#时间
        ///</summary>
        [MapName("CreateTime")]
        public DateTime CreateTime
        {
            get;
            set;
        }
        ///<summary>
        ///用户登录帐号
        ///</summary>
        [MapName("LoginId")]
        public string LoginId { set; get; }
       /// <summary>
       /// 消耗时间
       /// </summary>
       [MapName("time")]
        public long Time { set; get; }
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
