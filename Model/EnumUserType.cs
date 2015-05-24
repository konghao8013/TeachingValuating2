using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALOS.Model
{
    /// <summary>
    /// 用户权限类别
    /// </summary>
    public enum EnumUserType
    {
        /// <summary>
        /// 默认用户 无角色
        /// </summary>
        Default = 0,
        /// <summary>
        /// 学生
        /// </summary>
        Student = 1,
        /// <summary>
        /// 教师
        /// </summary>
        Teacher = 2,
        /// <summary>
        /// 管理员
        /// </summary>
        Admin = 3
    }
}
