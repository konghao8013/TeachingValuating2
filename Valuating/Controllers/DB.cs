using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using ALOS.DALSERVER;

namespace Valuating.Controllers
{
    /// <summary>
    /// 数据操作方法
    /// </summary>
    public static class DB
    {
        private static UserServer _userServer;

        private static SchoolServer _schoolServer;
        private static StudentServer _studentServer;
        private static TeacherServer _teacherServer;
        private static SysSettingServer _sysSettingServer;
        private static StudentRecordServer _studentRecordServer;
        private static EvaluatingPaperServer _evaluatingPaper;
        private static SysScoreServer _sysScore;
        private static PresentServer _presentServer;
        private static PerformanceServer _performanceServer;
        private static StudentScoreServer _studentScoreServer;
        private static ParperSendServer _parperSendServer;
        /// <summary>
        /// V00015打回材料操作类
        /// </summary>
        public static ParperSendServer ParperSendServer
        {
            get { return ReturnServer(_parperSendServer); }
        }
        /// <summary>
        /// V00014学生成绩表
        /// </summary>
        public static StudentScoreServer StudentScoreServer
        {
            get { return ReturnServer(_studentScoreServer); }
        }

        /// <summary>
        /// V00012评分结果表操作类
        /// </summary>
        public static PerformanceServer PerformanceServer
        {
            get { return ReturnServer(_performanceServer); }
        }
        /// <summary>
        /// V00011指标设计操作类
        /// </summary>
        public static PresentServer PresentServer
        {
            get { return ReturnServer(_presentServer); }
        }

        /// <summary>
        /// V00007分值表操作类
        /// </summary>
        public static SysScoreServer SysScoreServer
        {
            get { return ReturnServer(_sysScore); }
        }

        /// <summary>
        /// V00010抽取教师档案表操作类
        /// </summary>
        public static EvaluatingPaperServer EvaluatingPaperServer
        {
            get { return ReturnServer(_evaluatingPaper); }
        }

        /// <summary>
        /// V00009学生测评档案操作类
        /// </summary>
        public static StudentRecordServer StudentRecordServer
        {
            get { return ReturnServer(_studentRecordServer); }
        }

        /// <summary>
        /// V00008系统配置操作
        /// </summary>
        public static SysSettingServer SysSettingServer
        {
            get { return ReturnServer(_sysSettingServer); }
        }

        /// <summary>
        /// 返回教师操作对象V00005
        /// </summary>
        public static TeacherServer TeacherServer
        {
            get { return ReturnServer(_teacherServer); }
        }

        /// <summary>
        /// 返回学生账户操作对象
        /// </summary>
        public static StudentServer StudentServer
        {
            get { return ReturnServer(_studentServer); }
        }

        /// <summary>
        /// 返回学校操作对象
        /// </summary>
        public static SchoolServer SchoolServer
        {
            get { return ReturnServer(_schoolServer); }
        }

        /// <summary>
        /// 获得用户类操作对象
        /// </summary>
        public static UserServer UserServer
        {
            get { return ReturnServer(_userServer); }
        }

        public static SysLogServer _sysLogServer;
        /// <summary>
        /// 系统日志操作对象
        /// </summary>
        public static SysLogServer SysLogServer
        {
            get { return ReturnServer(_sysLogServer); }
        }
        static Dictionary<string, Object> servers = new Dictionary<string, object>();
        /// <summary>
        /// 反射创建数据库实例操作类别
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetServer<T>(string key)
        {
            return (T)GetServer(key);
        }
        /// <summary>
        /// 反射创建数据库操作对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetServer(string key)
        {
            var fileKey = "ALOS.DALSERVER." + key;
            if (!servers.ContainsKey(key))
            {
                Assembly assembly = Assembly.Load("DALSERVER");
                var type = assembly.GetType(fileKey);
                if (type == null)
                {
                    return null;
                }
                servers.Add(key, assembly.CreateInstance(fileKey));
            }
            return servers[key];
        }

        private static T ReturnServer<T>(T t) where T : new()
        {
            if (t == null)
            {
                t = new T();
            }
            return t;
        }
    }
}