using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALOS.Expand;
using ALOS.Model;

namespace ALOS.DALSERVER
{
    /// <summary>
    /// 系统分值设置V00007
    /// </summary>
    [APIServer("系统分值设置V00007操作类")]
    public class SysScoreServer : Help<SysScoreType>
    {
        /// <summary>
        /// 返回第一条数据
        /// </summary>
        /// <returns></returns>
        [APIServer("获得第一条数据")]
        public SysScoreType ReaderScoreType()
        {
            string sql = "select * from V00007";
            var model= _help.Reader<SysScoreType>(sql);
            return model ?? new SysScoreType();
        }
        [APIServer("保存数据","type对象")]
        public SysScoreType Save(SysScoreType model)
        {
            return base.Save(model);
        }
    }
}
