using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALOS.Expand
{
    
    public class DBLog
    {
        /// <summary>
        /// 日志名称
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { set; get; }
        /// <summary>
        /// 日志说明
        /// </summary>
        public string Explain { set; get; }
        /// <summary>
        /// 操作的SQL
        /// </summary>
        public string OperateSql { set; get; }
        /// <summary>
        /// 操作SQL 的参数
        /// </summary>
        public Object[] OperateParameter { set; get; }
        /// <summary>
        /// 返回值
        /// </summary>
        public Object ReturnValue { set; get; }
        /// <summary>
        /// 当出错的时候保存的错误信息
        /// </summary>
        public Exception Exception { set; get; }
        /// <summary>
        /// 方法名称
        /// </summary>
        public string Method { set; get; }
        /// <summary>
        /// 方法参数
        /// </summary>
        public List<Object> MethodParameter { set; get; }
        /// <summary>
        /// 泛型类型
        /// </summary>
        public Type[] GenericityParameter { set; get; }
        /// <summary>
        /// 函数耗时
        /// </summary>
        public long TimeConsuming { set; get; }
        public override string ToString()
        {
            return base.ToString();
        }
        /// <summary>
        /// 是否执行成功
        /// </summary>
        public bool IsOk { set; get; }

        /// <summary>
        /// 数据库流程日志初始化
        /// </summary>
        /// <param name="method">方法名称</param>
        /// <param name="methodParameter">方法参数</param>
        /// <param name="genericityParameter">如果方法是泛型 方法泛型的类型</param>
        /// <returns></returns>
        public static DBLog Init(string method, Object[] methodParameter, params Type[] genericityParameter) {
            var log=new DBLog() { Method=method,GenericityParameter=genericityParameter,CreateDate=DateTime.Now };
            log.MethodParameter = new List<Object>();
            if (methodParameter != null)
            {
                log.MethodParameter.AddRange(methodParameter);
            }
            log.MethodParameter.Add(log);
            return log;
        }

    }
}
