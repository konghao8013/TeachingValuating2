using System;
namespace ALOS.Model
{
	/// <summary>
	/// SysLog#系统日志
	/// </summary>
	[TableName("SysLog","SysLog#系统日志","Id")]
	public partial class syslog
	{
		/// <summary>
		/// Id#编号
		/// </summary>
		[MapName("Id",isKey:true,isIdentity:true)]
		public Int64 Id
		{
			get;
			set;
		}

		/// <summary>
		/// LogType#类别
		/// </summary>
		[MapName("LogType")]
		public String LogType
		{
			get;
			set;
		}

		/// <summary>
		/// Message#消息
		/// </summary>
		[MapName("Message")]
		public String Message
		{
			get;
			set;
		}

		/// <summary>
		/// Source#消息来源
		/// </summary>
		[MapName("Source")]
		public String Source
		{
			get;
			set;
		}

		/// <summary>
		/// CreateTime#时间
		/// </summary>
		[MapName("CreateTime")]
		public DateTime CreateTime
		{
			get;
			set;
		}

	}
}
