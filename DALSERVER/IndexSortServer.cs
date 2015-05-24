using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ALOS.Expand;
using ALOS.Model;

namespace ALOS.DALSERVER
{
    [APIServer("指标类别操作类")]
    public class IndexSortServer : Help<IndexSortType>
    {
       /// <summary>
       /// 获得所有指标类别
       /// </summary>
       /// <returns></returns>
       [APIServer("获得所有指标类别")]
        public List<IndexSortType> SortList()
        {
            return base.List();
        }
        /// <summary>
        /// 删除指标类别
        /// </summary>
        /// <param name="id"></param>
        [APIServer("根据ID删除指标类别","ID编号")]
        public void DeleteSort(int id)
        {
            base.Delete(id);
        }
        /// <summary>
        /// 保存指标类别
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [APIServer("保存指标类别","类别对象")]
        public IndexSortType SaveSort(IndexSortType type)
        {
            return base.Save(type);
        }
        /// <summary>
        /// 根据ID获得指标套对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [APIServer("根据ID获得指标套对象","ID编号")]
        public IndexSortType ReaderSort(int id)
        {
            return base.Reader(id);
        }

        /// <summary>
        /// 启用指标套
        /// </summary>
        /// <param name="id"></param>
        [APIServer("启用指标禁用其他","id")]
        public void Start(int id)
        {
            _help.ExecuteTrain(@"update v00016 set state=0 where id not in(@id)
update V00016 set state=1 where id=@id",new SqlParameter("id",id));
        }
    }
}
