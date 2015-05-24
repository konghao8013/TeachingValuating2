using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace EFCode
{
    public class SQLServerHelp
    {
        private readonly string _url;

        public SQLServerHelp(string url)
        {
            _url = url;
        }

        public List<Table> TabList()
        {
            string sql =
                "SELECT distinct d.Name,f.Value FROM syscolumns a left join systypes b on a.xusertype=b.xusertype inner join sysobjects d on a.id=d.id and d.xtype='U' and d.name<>'dtproperties' left join syscomments e on a.cdefault=e.id left join sys.extended_properties f on d.id=f.major_id and f.minor_id=0";
            return File(sql, com =>
            {
                var dr = com.ExecuteReader();
                var result = new List<Table>();
                while (dr.Read())
                    result.Add(new Table { Name = dr["name"] as string, Content = dr["value"] as string });
                return result;
            });

        }
        /// <summary>
        /// 根据表明查询主键 字段
        /// </summary>
        /// <param name="tabName"></param>
        /// <returns></returns>
        public string KeyName(string tabName)
        {
            string sql = "EXEC sp_pkeys @table_name=@tabName";
            return File(sql, com =>
            {
                var dr = com.ExecuteReader();
                string str =null;
                if (dr.Read())
                    str = dr["COLUMN_NAME"] as string;
                return str;
            },new SqlParameter("tabname",tabName));
        }

        public List<string> IdentityList(string tabName)
        {
            string sql =
                "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.columns  WHERE TABLE_NAME=@tabname AND  COLUMNPROPERTY(       OBJECT_ID(@tabname),COLUMN_NAME,'IsIdentity')=1";
            return File(sql, com =>
            {
                var dr = com.ExecuteReader();
                var result = new List<string>();
                while (dr.Read())
                    result.Add(dr["COLUMN_NAME"] as string);
                return result;
            },new SqlParameter("tabname",tabName));
        }

        public List<Field> FieldList(string tabName)
        {
            string sql =
                "SELECT name=a.name, type=b.name, [default]=isnull(e.text,''), Explain=isnull(g.[value],'') FROM syscolumns a left join systypes b on a.xusertype=b.xusertype inner join sysobjects d on a.id=d.id and d.xtype='U' and d.name<>'dtproperties' left join syscomments e on a.cdefault=e.id left join sys.extended_properties g on a.id=g.major_id and a.colid=g.minor_id left join sys.extended_properties f on d.id=f.major_id and f.minor_id=0 where d.name=@tableName order by a.id,a.colorder";
            return File(sql, com =>
            {
                var dr = com.ExecuteReader();
                var result = new List<Field>();
                while (dr.Read())
                    result.Add(new Field { Name = dr["name"] as string, Explin = dr["Explain"] as string,TypeName = dr["type"] as string});
                return result;
            },new SqlParameter("tablename",tabName));

        }

        private T File<T>(string sql, Func<SqlCommand, T> func, params SqlParameter[] paramets)
        {
            var comm = CreateCommand(sql);
            comm.Connection.Open();
            if (paramets != null&&paramets.Any())
                comm.Parameters.AddRange(paramets);
            var result = func(comm);
            comm.Connection.Close();
            comm.Dispose();
            return result;
        }

        private SqlCommand CreateCommand(string sql)
        {
            return new SqlCommand(sql, GetConn);
        }

        private static SqlConnection conn;
        private SqlConnection GetConn
        {
            get
            {
                if (conn == null)
                {
                    conn = new SqlConnection(_url);
                }
                return conn;
            }
        }

        private Dictionary<string, string> dic;
        public Dictionary<string, string> Dic
        {

            get
            {
                return dic ?? (dic = new Dictionary<string, string>
                {
                    {"bit", "Boolean"},
                    {"tinyint", "Byte"},
                    {"smallint", "Int16"},
                    {"int", "Int32"},
                    {"bigint", "Int64"},
                    {"numeric", "Decimal"},
                    {"decimal", "Decimal"},
                    {"smallmoney", "Decimal"},
                    {"money", "Decimal"},
                    {"float", "Double"},
                    {"real", "Single"},
                    {"datetime", "DateTime"},
                    {"smalldatetime", "DateTime"},
                    {"date", "DateTime"},
                    {"char", "String"},
                    {"varchar", "String"},
                    {"text", "String"},
                    {"nchar", "String"},
                    {"nvarchar", "String"},
                    {"ntext", "String"},
                    {"binary", "Byte[]"},
                    {"varbinary", "Byte[]"},
                    {"image", "Byte[]"},
                    {"timestamp", "DateTime"},
                    {"datetime2", "DateTime"},
                    {"xml", "String"},
                    {"uniqueidentifier", "Guid"}
                });
            }
        }
    }

}
