using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ALOS.DBHelp;

namespace Install
{
    public class Tool
    {
       
        public static string SQLNewDatabaseName { set; get; }

        private static SqlServerHelp _help;
        public static  SqlServerHelp DbHelp {
            get
            {
                if (_help == null)
                {
                    throw new Exception("未初始化SQLSERVERHELP服务");
                }
                return _help;
            }
        }
        /// <summary>
        /// 初始化SQLHELP
        /// </summary>
        /// <param name="server"></param>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <param name="database"></param>
        public static void InitSQLHelp(string database)
        {
            _help = new SqlServerHelp(string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True;Connect Timeout=500",  database));
        }
    }
}
