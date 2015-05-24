using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALOS.Expand;
using ALOS.Model;

namespace ALOS.DALSERVER
{
    /// <summary>
    /// 指标评语操作类
    /// </summary>
    [APIServer("指标评语操作类")]
    public class IndexRemarkServer : Help<IndexRemarkType>
    {
        /// <summary>
        /// 保存指标评语
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [APIServer("保存指标评语")]
        public IndexRemarkType SaveRemark(IndexRemarkType type)
        {
            return base.Save(type);
        }

       

        /// <summary>
        /// 根据指标ID获得指标评语
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [APIServer("根据指标ID获得指标评语","指标ID")]
        public IndexRemarkType RenderRemark(int id)
        {
            return base.Reader("zid", id)??new IndexRemarkType(){ZId = id};
        }
    }
}
