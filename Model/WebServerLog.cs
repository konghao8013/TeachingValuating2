
using System;

namespace ALOS.Model
{
    /// <summary>
    /// Web调用 日志
    /// </summary>
    [Serializable]
    public class WebServerLog
    {
        /// <summary>
        /// 方法名称
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 方法参数
        /// </summary>
        public Object[] Parameter { set; get; }
        /// <summary>
        /// 方法返回值
        /// </summary>
        
        public Object ReturnValue { set; get; }
        /// <summary>
        /// 要调用的类名
        /// </summary>
        public string ClassName { set; get; }
        /// <summary>
        /// 调用代码产生的错误信息
        /// </summary>
        public string Message { set; get; }

        public string Ip { set; get; }
        /// <summary>
        /// 耗时 
        /// </summary>
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
