using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALOS.Expand;
using ALOS.Model;

namespace ALOS.DALSERVER
{
    /// <summary>
    /// 系统参数设置V00008
    /// </summary>
    [APIServer("系统参数设置V00008")]
    public class SysSettingServer : Help<SysSettingType>
    {
        /// <summary>
        /// 获得第一条数据
        /// </summary>
        /// <returns></returns>
        [APIServer("获得第一条数据")]
        public SysSettingType ReaderSettingType()
        {
            string sql = "select * from V00008";
            var model= _help.Reader<SysSettingType>(sql);
            return model ?? new SysSettingType();
        }
        [APIServer("保存数据", "type对象")]
        public SysSettingType Save(SysSettingType model)
        {
            return base.Save(model);
        }
    }
}
