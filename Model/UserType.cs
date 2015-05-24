using System;
namespace ALOS.Model
{
    /// <summary>
    /// V00001#用户表
    /// </summary>
    [TableName("V00001", "V00001#用户表", "Id")]
    public class UserType
    {
        /// <summary>
        /// Id#编号
        /// </summary>
        [MapName("Id", isKey: true, isIdentity: true)]
        public Int64 Id
        {
            get;
            set;
        }

        /// <summary>
        /// LoginId#用户编号
        /// </summary>
        [MapName("LoginId")]
        public String LoginId
        {
            get;
            set;
        }

        /// <summary>
        /// Name#用户昵称
        /// </summary>
        [MapName("Name")]
        public String Name
        {
            get;
            set;
        }

        /// <summary>
        /// Password#用户密码
        /// </summary>
        [MapName("Password")]
        public String Password
        {
            get;
            set;
        }

        /// <summary>
        /// CreateDate#创建时间
        /// </summary>
        [MapName("CreateDate")]
        public DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// LoginDate#最近登录时间
        /// </summary>
        [MapName("LoginDate")]
        public DateTime LoginDate
        {
            get;
            set;
        }

        /// <summary>
        /// UserGuid#用户唯一标识码
        /// </summary>
        [MapName("UserGuid")]
        public Guid UserGuid
        {
            get;
            set;
        }
     //School,College,Email,Phone
        [MapName("School",isSQL:false)]
        public string School { set; get; }
        [MapName("College", isSQL: false)]
        public string College { set; get; }
        [MapName("Email", isSQL: false)]
        public string Email { set; get; }
        [MapName("Phone", isSQL: false)]
        public string Phone { set; get; }
        /// <summary>
        /// 用户教师权限
        /// </summary>
        [MapName("UserType")]
        public int TypeId { set; get; }

        /// <summary>
        /// 获得用户权限类别
        /// </summary>
        public EnumUserType GetUserType()
        {
            return (EnumUserType)TypeId;
        }
        public string Reurl { set; get; }
        /// <summary>
        /// 该用户是否启用
        /// </summary>
        public bool State { set; get; }

        public string Model_Type
        {
            get { return this.GetType().FullName; }
        }
    }
}
