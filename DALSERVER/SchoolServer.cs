using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ALOS.Expand;
using ALOS.Model;

namespace ALOS.DALSERVER
{
    /// <summary>
    /// v00003#学校学院管理表
    /// </summary>
    [APIServer(" v00003#学校学院管理表")]
    public class SchoolServer : Help<SchoolType>
    {
        /// <summary>
        /// 获得所有的学院学校信息
        /// </summary>
        /// <returns></returns>
        [APIServer("获得所有的学院学校信息")]
        public List<SchoolType> SelectList()
        {
            return _help.Select<SchoolType>("select * from v00003");
        }
        /// <summary>
        /// 保存学院学校信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [APIServer("保存学院学校信息", "学校对象")]
        public SchoolType SaveSchool(SchoolType type)
        {
            return base.Save(type);
        }
        /// <summary>
        /// 根据学校名称和学院名称查找学校对象
        /// </summary>
        /// <param name="schoolName"></param>
        /// <param name="collegeName"></param>
        /// <returns></returns>
        public SchoolType ReaderSchoolId(string schoolName, string collegeName)
        {
            string sql =
                " select * from V00003 where ObjId in(select id from V00003 where ObjId=0 and Name=@schoolName) and Name=@CollegeName";
            return _help.Reader<SchoolType>(sql, new SqlParameter("CollegeName", collegeName),
                new SqlParameter("schoolName", schoolName));
        }

        /// <summary>
        /// 删除学校信息
        /// </summary>
        /// <param name="id"></param>
        [APIServer("删除学校信息", "学校ID")]
        public void DeleteSchool(int id)
        {
            base.Delete(id);
        }
        /// <summary>
        /// 根据OBJiD获得数据
        /// </summary>
        /// <param name="objid"></param>
        /// <returns></returns>
        [APIServer("根据OBJiD获得数据","objid")]
        public List<SchoolType> SelectTypeSchool(int objid)
        {
            string sql = "select * from v00003 where objid=@objid";
            return _help.Select<SchoolType>(sql, new SqlParameter("objid", objid));
        }
        /// <summary>
        /// 根据ID获得对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [APIServer("根据ID获得对象","Id")]
        public SchoolType RenderSchool(int id)
        {
            string sql = "select * from v00003 where id=@id";
            return _help.Reader<SchoolType>(sql, new SqlParameter("id", id));
        }
    }
}
