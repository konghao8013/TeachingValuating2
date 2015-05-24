using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALOS.Model
{
    ///<summary>
    ///V00007#分值设计表
    ///</summary>
    [Serializable]
    [TableName("V00007", "V00007#分值设计表","id")]
    public class SysScoreType
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
        ///TeachingPlan#教案
        ///</summary>
        [MapName("TeachingPlan")]
        public Decimal TeachingPlan
        {
            get;
            set;
        }
        ///<summary>
        ///Courseware#课件
        ///</summary>
        [MapName("Courseware")]
        public Decimal Courseware
        {
            get;
            set;
        }
        ///<summary>
        ///Video#视频
        ///</summary>
        [MapName("Video")]
        public Decimal Video
        {
            get;
            set;
        }
        ///<summary>
        ///Reflection#反思
        ///</summary>
        [MapName("Reflection")]
        public Decimal Reflection
        {
            get;
            set;
        }
        ///<summary>
        ///VideoPass#视频及格线
        ///</summary>
        [MapName("VideoPass")]
        public Decimal VideoPass
        {
            get;
            set;
        }
        ///<summary>
        ///Pass#及格分数
        ///</summary>
        [MapName("Pass")]
        public Decimal Pass
        {
            get;
            set;
        }
        ///<summary>
        ///DifferenceValue#三评差值
        ///</summary>
        [MapName("DifferenceValue")]
        public Decimal DifferenceValue
        {
            get;
            set;
        }
        ///<summary>
        ///BatchName#测评批次
        ///</summary>
        [MapName("BatchName")]
        public String BatchName
        {
            get;
            set;
        }
        ///<summary>
        ///BatchStateDate#批次开始时间
        ///</summary>
        [MapName("BatchStateDate")]
        public DateTime BatchStateDate
        {
            get;
            set;
        }
        ///<summary>
        ///BatchEndDate#批次结束时间
        ///</summary>
        [MapName("BatchEndDate")]
        public DateTime BatchEndDate
        {
            get;
            set;
        }
        /// <summary>
        /// 创意得分
        /// </summary>
        [MapName("Originality")]
        public int Originality { set; get; }
        /// <summary>
        /// 视频说课
        /// </summary>
        [MapName("Present")]
        public decimal Present { set; get; }

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
