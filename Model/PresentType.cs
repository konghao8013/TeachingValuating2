using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALOS.Model
{
    ///<summary>
    ///V00011#教学指标库
    ///</summary>
    [Serializable]
    [TableName("V00011", "V00011#教学指标库","id")]
    public class PresentType
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
        ///OrderId#排序编号
        ///</summary>
        [MapName("OrderId")]
        public Int32 OrderId
        {
            get;
            set;
        }
        ///<summary>
        ///TypeId#类别1教案2课件3视频4反思
        ///</summary>
        [MapName("TypeId")]
        public Int32 TypeId
        {
            get;
            set;
        }
        ///<summary>
        ///Name#指标名称
        ///</summary>
        [MapName("Name")]
        public String Name
        {
            get;
            set;
        }
        ///<summary>
        ///Content#指标说明
        ///</summary>
        [MapName("Content")]
        public String Content
        {
            get;
            set;
        }
        ///<summary>
        ///Ratio#得分比例
        ///</summary>
        [MapName("Ratio")]
        public Decimal Ratio
        {
            get;
            set;
        }
        /// <summary>
        /// 结果分值
        /// </summary>
        [MapName("CGrade",isSQL:false)]
        public Decimal Cgrade { set; get; }
        /// <summary>
        /// 成绩ID
        /// </summary>
        [MapName("Cid",isSQL:false)]
        public long Cid { set; get; }

        ///<summary>
        ///Version#指标类别
        ///</summary>
        [MapName("Version")]
        public Int32 Version
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
       
        /// <summary>
        /// 获得指标类别名称
        /// </summary>
        public string TypeName
        {
            get
            {
                return TypeId == 1
                    ? "教案"
                    : TypeId == 2 ? "课件" : TypeId == 3 ? "视频" : TypeId == 4 ? "反思" : TypeId == 5 ? "说课" : "";
                
              
            }
        }
    }
}
