using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALOS.Model
{
    ///<summary>
    ///V00012#评分结果
    ///</summary>
    [Serializable]
    [TableName("V00012", "V00012#评分结果","id")]
    public class PerformanceType
    {
        ///<summary>
        ///Id#编号
        ///</summary>
        [MapName("Id",true,true)]
        public Int64 Id
        {
            get;
            set;
        }
        ///<summary>
        ///DId#档案编号
        ///</summary>
        [MapName("DId")]
        public Int32 DId
        {
            get;
            set;
        }
        ///<summary>
        ///ZId#指标ID
        ///</summary>
        [MapName("ZId")]
        public Int32 ZId
        {
            get;
            set;
        }
        ///<summary>
        ///Grade#成绩比例
        ///</summary>
        [MapName("Grade")]
        public Decimal Grade
        {
            get;
            set;
        }
        ///<summary>
        ///GradeScore#成绩得分
        ///</summary>
        [MapName("GradeScore")]
        public Decimal GradeScore
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
