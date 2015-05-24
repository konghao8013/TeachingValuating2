using System;
namespace ALOS.Model
{
	/// <summary>
	/// V00002#系统菜单
	/// </summary>
	[TableName("V00002","V00002#系统菜单","Id")]
	public  class SysMenuType
	{
		/// <summary>
		/// Id#菜单编号
		/// </summary>
		[MapName("Id",isKey:true,isIdentity:true)]
		public Int32 Id
		{
			get;
			set;
		}

		/// <summary>
		/// Name#菜单名称
		/// </summary>
		[MapName("Name")]
		public String Name
		{
			get;
			set;
		}

		/// <summary>
		/// Url#菜单地址
		/// </summary>
		[MapName("Url")]
		public String Url
		{
			get;
			set;
		}

		/// <summary>
		/// Type#菜单类别1学生菜单2教师菜单3管理员界面菜单
		/// </summary>
		[MapName("Type")]
		public Int32 Type
		{
			get;
			set;
		}
        /// <summary>
        /// 父级菜单
        /// </summary>
        [MapName("objid")]
	    public int ObjId { set; get; }

	    /// <summary>
        /// 获得菜单类别
        /// </summary>
        /// <returns></returns>
	    public EnumSysMenuType GetMenuType()
	    {
	        return (EnumSysMenuType) Type;
	    }
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
