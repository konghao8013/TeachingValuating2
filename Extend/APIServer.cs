using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALOS.Expand
{
    public class APIServer:Attribute
    {
        /// <summary>
        /// 方法说明
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 参数 和参数说明
        /// </summary>
        public string Parameter { set; get; }
        /// <summary>
        /// [WCFServer("测试方法","姓名,年龄")]
        /// </summary>
        /// <param name="name">方法说明</param>
        /// <param name="parameter">方法参数说明 和参数 </param>
        public APIServer(string name, string parameter = "")
        {
            Name = name;
            Parameter = parameter;
        }
    }
}
