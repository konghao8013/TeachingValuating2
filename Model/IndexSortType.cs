using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALOS.Model
{
    ///<summary>
    ///V00016#指标套设计
    ///</summary>
    [Serializable]
    [TableName("V00016", "指标套设计", "id")]
    public class IndexSortType
    {
        ///<summary>
        ///Id#编号
        ///</summary>
        [MapName("Id",true,true)]
        public int Id
        {
            get;
            set;
        }
        ///<summary>
        ///Name#指标套名称
        ///</summary>
        [MapName("Name")]
        public String Name
        {
            get;
            set;
        }
        ///<summary>
        ///State#是否启用
        ///</summary>
        [MapName("State")]
        public Boolean State
        {
            get;
            set;
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
