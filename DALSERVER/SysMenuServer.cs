using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ALOS.Expand;
using ALOS.Model;

namespace ALOS.DALSERVER
{
    [APIServer("系统菜单数据")]
    public class SysMenuServer:Help<SysMenuType>
    {
        /// <summary>
        /// 获得所有的菜单
        /// </summary>
        /// <returns></returns>
        public List<SysMenuType> List()
        {
            return _help.List<SysMenuType>();
        }
        /// <summary>
        /// 根据菜单类别获得菜单
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [APIServer("根据菜单类别获得菜单", "EnumSysMenuType 1学生2教师3管理员")]
        public List<SysMenuType> List(int type)
        {
            return _help.Select<SysMenuType>("select * from v00002 where type=@type", new SqlParameter("type", type));
        }
        /// <summary>
        /// 获得所有的系统菜单
        /// </summary>
        /// <returns></returns>
        [APIServer("获得所有的系统菜单")]
        public List<SysMenuType> Select()
        {
            return _help.Select<SysMenuType>("select * from v00002");
        }
        /// <summary>
        /// 根据菜单ID获得菜单对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [APIServer("获得所有的系统菜单")]
        public SysMenuType Reader(int id)
        {
            return _help.Reader<SysMenuType>("select * from v00002 where id=@id",new SqlParameter("id",id));
        }
        /// <summary>
        /// 获得OBJ为ID的菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [APIServer("获得OBJ为ID的菜单","ObjId")]
        public List<SysMenuType> SelectObjIdList(int id)
        {
            return _help.Select<SysMenuType>("select * from v00002 where objid=@objid", new SqlParameter("objid", id));
        }
        [APIServer("保存菜单对象","菜单对象")]
        public SysMenuType SaveModel(SysMenuType type)
        {
            return base.Save(type);
        }
        /// <summary>
        /// 根据ID删除菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [APIServer(" 根据ID删除菜单","菜单ID")]
        public bool DeleteModel(int id)
        {
            base.Delete(id);
            return true;
            
        }
    }
}
