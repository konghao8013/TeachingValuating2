using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALOS.Model
{
    ///<summary>
    ///V00003#学校表
    ///</summary>
    [Serializable]
    [TableName("V00003", "V00003#学校表", "id")]
    public class SchoolType
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
        ///Name#学校名称
        ///</summary>
        [MapName("Name")]
        public String Name
        {
            get;
            set;
        }
        ///<summary>
        ///CreateDate#创建时间
        ///</summary>
        [MapName("CreateDate")]
        public DateTime CreateDate
        {
            get;
            set;
        }
        ///<summary>
        ///ObjId#0代表学校>0代表学院
        ///</summary>
        [MapName("ObjId")]
        public Int32 ObjId
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
