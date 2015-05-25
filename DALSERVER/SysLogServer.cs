using ALOS.Expand;
using ALOS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALOS.DALSERVER
{
    /// <summary>
    /// 系统日志操作类Syslog
    /// </summary>
    [APIServer("系统日志操作类Syslog")]
    public class SysLogServer : Help<SysLogType>
    {
        static SysLogServer log = new SysLogServer();
        /// <summary>
        /// 插入系统消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logType"></param>
        /// <param name="source"></param>
        [APIServer("插入系统消息","日志内容,日志类别,来源")]
        public static void InsertMessage(string message,string logType="",string source="",string loginid="",long time=0) {
            SysLogType sys = new SysLogType();
            sys.CreateTime = DateTime.Now;
            sys.LogType = logType;
            sys.Message = message; 
            sys.Source = source;
            sys.LoginId = loginid;
            sys.Time = time;
            log.Add(sys);
            
        }
        
    }
   
}
