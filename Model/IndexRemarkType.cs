using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALOS.Model
{
    ///<summary>
    ///V00013#指标评语
    ///</summary>
    [Serializable]
    [TableName("V00013", "V00013#指标评语","id")]
    public class IndexRemarkType
    {
        ///<summary>
        ///Id#编号
        ///</summary>
        [MapName("Id",true,true)]
        public Int32 Id
        {
            get;
            set;
        }
        ///<summary>
        ///ZId#指标Id
        ///</summary>
        [MapName("ZId")]
        public Int32 ZId
        {
            get;
            set;
        }
        ///<summary>
        ///A#优
        ///</summary>
        [MapName("A")]
        public String A
        {
            get;
            set;
        }
        ///<summary>
        ///B#良
        ///</summary>
        [MapName("B")]
        public String B
        {
            get;
            set;
        }
        ///<summary>
        ///C#中
        ///</summary>
        [MapName("C")]
        public String C
        {
            get;
            set;
        }
        ///<summary>
        ///D#合格
        ///</summary>
        [MapName("D")]
        public String D
        {
            get;
            set;
        }
        ///<summary>
        ///E#不合格
        ///</summary>
        [MapName("E")]
        public String E
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
