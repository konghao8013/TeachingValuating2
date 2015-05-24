using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALOS.Model
{
    /// <summary>
    /// 学生测评资料类别枚举
    /// </summary>
    public enum EnumStudentDataType
    {
        Default=0,
        /// <summary>
        /// 教案
        /// </summary>
        TeachingPlan=1,
        /// <summary>
        /// 课件
        /// </summary>
        Courseware=2,
        /// <summary>
        /// 视频
        /// </summary>
        Video=3,
        /// <summary>
        /// 反思
        /// </summary>
        Reflection=4,
        //课前说课
        Videobefclass=5
    }
}
