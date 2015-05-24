using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALOS.DBHelp;
using ALOS.Expand;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
namespace ALOS.DALSERVER
{
    /// <summary>
    /// 数据库基础操作帮助类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Help<T>
    {
        public IServerHelp _help;
        public Help()
        {
            var fileConfig = ConfigurationManager.ConnectionStrings["ValuationDB"];
            if (fileConfig == null)
            {
                throw new Exception("未找到ValuationDB 数据库链接地址");
            }
            var file = fileConfig.ConnectionString;
            _help = new SqlServerHelp(file);

            _help.OnError += _help_OnError;
        }
        public void _help_OnError(DBLog log)
        {



            StringBuilder sb = new StringBuilder();
            StringBuilder sbsql = new StringBuilder();
            sbsql.Append("declare ");
            foreach (SqlParameter p in log.OperateParameter)
            {
                sbsql.Append("@" + p.ParameterName + " " + p.DbType + ",");
                sb.Append("@" + p.ParameterName + "=" + p.Value + ",");
            }
            string text = "Method:" + log.Method + "\tSQL:" + log.OperateSql + "\tparameter:【" + sbsql.ToString() +
                          "\r\n" + sb.ToString() + "】\r\nMessage:" + log.Exception.InnerException.Message + "\r\n\r\n";

            SysLogServer.InsertMessage(text, "SQLMessage", "Error", time: log.TimeConsuming);
            throw log.Exception.InnerException;
            // sw.Dispose();
        }
        public TableName TableName
        {
            get
            {
                var tab= Attribute.GetCustomAttribute(typeof(T), typeof(TableName)) as TableName;
                if (tab == null)
                {
                    throw new Exception("FullName:" + typeof(T).FullName + "Model未设置TabName ");
                }
                return tab;
            }
        }
        /// <summary>
        /// 获得根据表特殊字段进行排序SQL
        /// </summary>
        /// <param name="tabName">表名</param>
        /// <param name="sortId">排序字段</param>
        /// <param name="pId">参数名称</param>
        /// <returns></returns>
        public string SortTabSql(string tabName, string sortId, string pId)
        {
            string sql = @"declare @maxId int,@{1} int,@exchange int
select top 1 @maxId={1} from {0} order by {1} desc
update {0} set {1}=a.rowNumber from(
select *,@maxId+ROW_NUMBER() over (order by {2}) rowNumber from {0} where {1} in(
select {1} from {0} group by {1} having count({1})>1)) a where a.{2}={0}.{2}
select @{1}={1} from {0} where {2}=@{2}
if(@move=1)
	
begin
select  top 1  @exchange={1} from {0} where {1}<@{1} order by {1} desc
end
else if(@move=3)
begin
		select  top 1  @exchange={1} from {0} where {1}>@{1} order by {1}
end

if(@exchange is not  null)
begin
update {0} set {1}=@{1} where {1}=@exchange
update {0} set {1}=@exchange where {2}=@{2}
end";
            return String.Format(sql, tabName, sortId, pId);
        }
        public virtual T Add(T t)
        {
            _help.Add(t);
            return t;
        }
        public virtual void Delete(T t)
        {
            _help.Delete(t);
        }
        public virtual void Delete(int id)
        {
            _help.ExecuteNonQuery("delete " + TableName.Name + " where id=@id", new SqlParameter("id", id));
        }
        public virtual void Delete(Int64 id)
        {
            _help.ExecuteNonQuery("delete " + TableName.Name + " where id=@id", new SqlParameter("id", id));
        }
        public virtual List<T> PageList(int index, int size)
        {

            var tabname = TableName;
            var min = (index - 1) * size;
            var max = index * size;
            var sql = "select * from (select *,ROW_NUMBER() over(order by " + tabname.ConditionsChangeColumn + ") row_number_db from " + tabname.Name + " ) t1 where row_number_db>@min and row_number_db<=@max";
            return _help.Select<T>(sql, new SqlParameter("min", min), new SqlParameter("max", max));
        }
        /// <summary>
        /// 将对象保存到 表中 并 修改 对象的默认修改对象的值
        /// </summary>
        /// <param name="t"></param>
        public virtual T Save(T t)
        {
            _help.Save(t);
            return t;
        }

        public virtual List<T> List()
        {
            return _help.List<T>();
            //  return new List<T>();
        }
        /// <summary>
        /// 根据指定的列名 和值 获取对应的List集合
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual List<T> List(string name, Object value)
        {
            string sql = "select * from " + TableName.Name + " where " + name + "=@value";
            return _help.Select<T>(sql, new SqlParameter("value", value));
        }
        /// <summary>
        /// 根据默认修改列 获得一个对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T Reader(int value)
        {
            var tabname = TableName;
            return _help.Reader<T>("select * from " + tabname.Name + " where [" + TableName.ConditionsChangeColumn + "]=@" + tabname.ConditionsChangeColumn, new SqlParameter(tabname.ConditionsChangeColumn, value));
        }
        /// <summary>
        /// 根据指定的字段获对应的第一条数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual T Reader(string name, Object value)
        {
            var tabname = TableName;
            return _help.Reader<T>("select * from " + tabname.Name + " where [" + name + "]=@value", new SqlParameter("value", value));
        }
        /// <summary>
        /// 根据SQL语句查询所有的数据条数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int SQLCount(string sql,int pageSize,params  SqlParameter[] parameters)
        {
            string tempSql = "select count(1) from (" + sql + ") a";
            var count = _help.ExecuteScalar<int>(tempSql, parameters);
            return count%pageSize == 0 ? count/pageSize : ((count/pageSize) + 1);

        }
        /// <summary>
        /// 根据SQL语句返回分页数据
        /// </summary>
        /// <typeparam name="TE"></typeparam>
        /// <param name="sql"></param>
        /// <param name="pageSize"></param>
        /// <param name="index"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<TE> SqlList<TE>(string sql, int pageSize, int index, params SqlParameter[] parameters)
        {
            var list = new List<SqlParameter>();
           
            var max = index*pageSize;
            var min = (index - 1)*pageSize;
            list.Add(new SqlParameter("min_row", min));
             list.Add(new SqlParameter("max_row", max));
            if (parameters != null)
            {
                list.AddRange(parameters);
            }
            var tab=Attribute.GetCustomAttribute(typeof(TE), typeof(TableName)) as TableName;
            string tempSql="select * from (select *,ROW_NUMBER()  over(order by "+tab.ConditionsChangeColumn+" desc) SQLRow_Numbers from ("+sql+")a ) a where SQLRow_Numbers>@min_row and SQLRow_Numbers<=@max_row";
            return _help.Select<TE>(tempSql, list.ToArray());
        }
    }
}
