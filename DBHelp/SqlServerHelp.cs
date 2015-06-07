using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ALOS.Expand;
using System.Reflection;
using System.Diagnostics;
namespace ALOS.DBHelp
{
    public class SqlServerHelp : IServerHelp
    {
       
        public string Url { set; get; }
        public SqlServerHelp(string url)
        {
            Url = url;
        }
        #region 工具方法
        object GetValue<E>(E type, PropertyInfo info)
        {
            object ischeck;
            var value = info.GetValue(type, null);
            switch (info.PropertyType.FullName)
            {
                case "System.String":
                    ischeck = string.IsNullOrEmpty((string)value) ? "" : info.GetValue(type, null);
                    break;
                case "System.Int32 ":
                    ischeck = (int)value == 0 ? 0 : info.GetValue(type, null);
                    break;
                case "System.Byte":
                    ischeck = (Byte)value == 0 ? 0 : info.GetValue(type, null);
                    break;
                case "System.SByte":
                    ischeck = (SByte)value == 0 ? 0 : info.GetValue(type, null);
                    break;
                case "System.Char":
                    ischeck = (Char)value == '\0' ? '\0' : info.GetValue(type, null);
                    break;
                case "System.Decimal":
                    ischeck = (Decimal)value == 0.0m ? 0.0m : info.GetValue(type, null);
                    break;
                case "System.Double":
                    ischeck = (Double)value == 0.0d ? 0.0d : info.GetValue(type, null);
                    break;
                case "System.Single":
                    ischeck = (Double)value == 0.0f ? 0.0f : info.GetValue(type, null);
                    break;
                case "System.UInt32":
                    ischeck = (UInt32)value == 0 ? 0 : info.GetValue(type, null);
                    break;
                case "System.DateTime":
                    ischeck = (DateTime)value == DateTime.MinValue ? DateTime.Now : info.GetValue(type, null);
                    break;
                default:
                    ischeck = value == null ? "" : info.GetValue(type, null);
                    break;
            }
            return ischeck;
        }
        TableName Table<T>()
        {
            var tab = Attribute.GetCustomAttribute(typeof(T), typeof(TableName)) as TableName;
            return tab;
        }
        /// <summary>
        /// 判断属性值是否为空 为空返回True
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="info"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsNull<E>(PropertyInfo info, E type)
        {
            bool ischeck = false;
            var value = info.GetValue(type, null);
            switch (info.PropertyType.FullName)
            {
                case "System.String":
                    ischeck = string.IsNullOrEmpty((string)value);
                    break;
                case "System.Int32":
                    ischeck = (int)value == 0 ? true : false;
                    break;
                case "System.Byte":
                    ischeck = (Byte)value == 0 ? true : false;
                    break;
                case "System.SByte":
                    ischeck = (SByte)value == 0 ? true : false;
                    break;
                case "System.Char":
                    ischeck = (Char)value == '\0' ? true : false;
                    break;
                case "System.Decimal":
                    ischeck = (Decimal)value == 0.0m ? true : false;
                    break;
                case "System.Double":
                    ischeck = (Double)value == 0.0d ? true : false;
                    break;
                case "System.Single":
                    ischeck = (Double)value == 0.0f ? true : false;
                    break;
                case "System.UInt32":
                    ischeck = (UInt32)value == 0 ? true : false;
                    break;
                case "System.Guid":
                    ischeck = (Guid)value == new Guid() ? true : false;
                    break;
                case "System.DateTime":
                    ischeck = (DateTime)value == DateTime.MinValue ? true : false;
                    break;
                default:
                    ischeck = value == null ? true : false;
                    break;
            }
            return ischeck;
        }
        #endregion
        #region 私有方法
        public void Invoke(DBLog log)
        {

            if (OnStart != null)
            {
                OnStart(log);
            }
            Stopwatch sw = new Stopwatch();
            sw.Start();
            MethodInfo method = typeof(SqlServerHelp).GetMethod(log.Method, BindingFlags.NonPublic | BindingFlags.Instance);
            if (method.IsGenericMethod)
            {
                method = method.MakeGenericMethod(log.GenericityParameter);
            }
            try
            {
                method.Invoke(this, log.MethodParameter.ToArray());
            }
            catch (Exception e)
            {
                if (OnError != null)
                {
                    
                    log.Exception = e;
                    OnError(log);
                }
            }

            log.TimeConsuming = sw.ElapsedTicks;
            sw.Reset();
            if (OnOver != null)
            {
                OnOver(log);
            }
        }
        void Add_Realize<T>(T t, DBLog log)
        {

            var tab = Table<T>();
            if (tab == null)
            {
                throw new Exception("TabName 标记未设置");
            }
            string CommandText;
            List<SqlParameter> list = new List<SqlParameter>();

            string sql = "insert into " + tab.Name + "(";
            string sqlvalue = ")values(";
            var properties = typeof(T).GetProperties();
            foreach (var propertie in properties)
            {
                var map = Attribute.GetCustomAttribute(propertie, typeof(MapName)) as MapName;
                if (map != null && map.IsIdentity == false&&map.IsSQL)
                {
                    sql += "[" + map.Name + "],";
                    sqlvalue += "@" + map.Name + ",";
                    list.Add(new SqlParameter(map.Name, GetValue<T>(t, propertie)));
                }
            }
            sql = sql.Remove(sql.Length - 1);
            sqlvalue = sqlvalue.Remove(sqlvalue.Length - 1) + ")";
            CommandText = sql + sqlvalue;
            log.OperateSql = CommandText;
            log.OperateParameter = list.ToArray();
            using (var comm = Comm(CommandText, CommandType.Text, list.ToArray()))
            {
                comm.Connection.Open();
                var number = (Int32)comm.ExecuteNonQuery();
                comm.Connection.Close();
                comm.Connection.Dispose();
                comm.Dispose();
                log.IsOk = number > 0 ? true : false;
            }
                

        }
        void Delete_Realize<T>(T t, DBLog log)
        {
            var tabName = Table<T>();
            string deleteSql = "delete "+tabName.Name+" where ";
            var properties = typeof(T).GetProperties();
            List<SqlParameter> list = new List<SqlParameter>();
            foreach (var propertie in properties)
            {
                var map = Attribute.GetCustomAttribute(propertie, typeof(MapName)) as MapName;
                if (map!=null&&IsNull<T>(propertie, t)==false&&map.IsSQL)
                {
                    deleteSql += "[" + map.Name + "]=@" + map.Name + " and";
                    list.Add(new SqlParameter(map.Name,GetValue<T>(t,propertie)));
                }
            }
            deleteSql = deleteSql.Remove(deleteSql.Length-3);
            log.OperateParameter = list.ToArray();
            log.OperateSql = deleteSql;
            using (var comm = Comm(deleteSql, CommandType.Text, list.ToArray())) {
                comm.Connection.Open();
                var number = (Int32)comm.ExecuteNonQuery();
                comm.Connection.Close();
                comm.Connection.Dispose();
                comm.Dispose();
                log.IsOk = number > 0 ? true : false;
            }
          
        }
        void Save_Realize<T>(T t, DBLog log)
        {
            var tabName = Table<T>();
            if (tabName.ConditionsChangeColumn.IsNull())
            {
                throw new Exception("该表没有默认条件修改列无法完成操作：ConditionsChangeColumn");
            }
            string sql = "if exists(select 1 from "+tabName.Name+" where ["+tabName.ConditionsChangeColumn+"]=@"+tabName.ConditionsChangeColumn+")begin ";
            string updateSql = "update "+tabName.Name+" set ";
            string insertSql = "insert into "+tabName.Name+" (";
            string insertSqlValue = ")values(";
            string tempSql = "";
            string tempSqlUpdate = "";
            var properties = typeof(T).GetProperties();
            var list = new List<SqlParameter>();
            SqlParameter temp = null;
            PropertyInfo info=null;
            foreach (var propertie in properties)
            {
                var map = Attribute.GetCustomAttribute(propertie, typeof(MapName)) as MapName;
                if (map==null||map.Name == null)
                    continue;
                if (map.IsIdentity==false&&map.IsSQL)
                {
                    updateSql += "[" + map.Name + "]=@" + map.Name+",";
                    insertSql += "[" + map.Name + "],";
                    insertSqlValue += "@" + map.Name + ",";
                    if (map.Name.ToLower() == tabName.ConditionsChangeColumn.ToLower())
                    {
                        temp = new SqlParameter(map.Name, GetValue<T>(t, propertie));
                        list.Add(temp);
                        continue;
                    }
                    list.Add(new SqlParameter(map.Name, GetValue<T>(t, propertie)));
                    continue;
                }
                if (tabName.ConditionsChangeColumn.ToLower() == map.Name.ToLower()&&map.IsIdentity)
                {
                    temp = new SqlParameter(map.Name, GetValue<T>(t, propertie));
                    info = propertie;
                   temp.Direction = ParameterDirection.InputOutput;
                    temp.Size = 10;
                    list.Add(temp);
                    tempSql = "set @" + tabName.ConditionsChangeColumn + "=@@identity;";
                    tempSqlUpdate = "set @"+tabName.ConditionsChangeColumn+"=@"+tabName.ConditionsChangeColumn;
                }
            }
            updateSql = updateSql.Remove(updateSql.Length - 1) + " where [" + tabName.ConditionsChangeColumn + "]=@" + tabName.ConditionsChangeColumn;
            insertSql = insertSql.Remove(insertSql.Length - 1) + insertSqlValue.Remove(insertSqlValue.Length - 1)+")";
            sql += updateSql + "   end else begin " + insertSql + " "+tempSql+" end";
            log.OperateSql = sql;
            log.OperateParameter = list.ToArray();
            
            using (var comm = Comm(sql, CommandType.Text, list.ToArray())) {
                comm.Connection.Open();
                var number = comm.ExecuteNonQuery();
                comm.Connection.Close();
                comm.Connection.Dispose();
                comm.Dispose();
                log.IsOk =number>0? true:false;
            }
            log.ReturnValue = temp.Value;
            if(info!=null)
            info.SetValue(t, log.ReturnValue, null);
        }
        void List_Realize<T>(DBLog log)where T:new() {
            var tab = Table<T>();
            string sql = "select * from "+tab.Name;
            log.OperateSql = sql;
            List<T> list = new List<T>();
            using (var comm = Comm(sql, CommandType.Text)) {
                comm.Connection.Open();
                var dr = comm.ExecuteReader();
                while (dr.Read())
                { 
                 list.Add(Reader<T>(dr));
                }
                dr.Dispose();
                comm.Connection.Close();
                comm.Connection.Dispose();
                comm.Dispose();
            }
            log.ReturnValue = list;
            log.IsOk = true;
        }
        T Reader<T>(SqlDataReader dr)where T:new() {
            T t = new T();
            var properties = typeof(T).GetProperties();
            foreach (var propertie in properties) {
                var map = Attribute.GetCustomAttribute(propertie, typeof(MapName)) as MapName;
                if (map != null && ISCheckName(dr,map.Name))
                {

                    propertie.SetValue(t, Convert.IsDBNull(dr[map.Name]) ? GetValue<T>(t, propertie) : dr[map.Name], null);
                }
            }
            return t;
        }
        /// <summary>
        /// 检查SqlDataReader中是否有name对象
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool ISCheckName(SqlDataReader dr,string name)
        {
            var length = dr.FieldCount;
            for (int i = 0; i < length; i++)
            {
                if (dr.GetName(i).ToLower() == name.ToLower())
                {
                    return true;
                }
            }
            return false;
           
        }

        void Select_Realize<T>(string sql, CommandType type, SqlParameter[] parameter,DBLog log)where T:new()
        {
            log.OperateSql = sql;
            log.OperateParameter = parameter;
            List<T> list = new List<T>();
            using (var comm = Comm(sql, type, parameter)) {
                comm.Connection.Open();
                var dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(Reader<T>(dr));
                }
                dr.Dispose();
                comm.Connection.Close();
                comm.Connection.Dispose();
                comm.Dispose();
            }
            log.ReturnValue = list;
        }
        void Reader_Realize<T>(string sql, CommandType type, SqlParameter[] parameter, DBLog log) where T : new()         {
            log.OperateSql = sql;
            log.OperateParameter = parameter;
            using (var comm = Comm(sql, type, parameter))
            {
                comm.Connection.Open();
                var dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    log.ReturnValue = Reader<T>(dr);
                }
                dr.Dispose();
                comm.Connection.Close();
                comm.Connection.Dispose();
                
                comm.Dispose();
            }
         
        }
        void Table_Realize(string sql, CommandType type, SqlParameter[] parameter, DBLog log)
        {
            log.OperateSql = sql;
            log.OperateParameter = parameter;
            DataTable table = new DataTable();
            using (var comm = Comm(sql, type, parameter))
            {
                comm.Connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(comm);
                da.Fill(table);
                log.ReturnValue = table;
                da.Dispose();
                comm.Connection.Close();
                comm.Connection.Dispose();
                comm.Dispose();
            }
        }
        void ExecuteScalar_Realize(string sql, CommandType type, SqlParameter[] parameter, DBLog log)
        {
            log.OperateSql = sql;
            log.OperateParameter = parameter;
            using (var comm = Comm(sql, type, parameter))
            {
                comm.Connection.Open();
                log.ReturnValue = comm.ExecuteScalar();
                comm.Connection.Close();
                comm.Connection.Dispose();
                comm.Dispose();
            }
        }
        void ExecuteNonQuery_Realize(string sql, CommandType type, SqlParameter[] parameter, DBLog log) {
            log.OperateSql = sql;
            log.OperateParameter = parameter;
            using (var comm = Comm(sql, type, parameter)) {
                comm.Connection.Open();
             log.ReturnValue= comm.ExecuteNonQuery();
             comm.Connection.Close();
             comm.Connection.Dispose();
             comm.Dispose();
            }
        }
        void ExecuteListScalar_Realize<T>(string sql, CommandType type, SqlParameter[] parameter, DBLog log)where T:new()
        {
            List<T> list = new List<T>();
            log.OperateSql = sql;
            log.OperateParameter = parameter;
            using (var comm = Comm(sql, type, parameter))
            {
                comm.Connection.Open();
                var dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    list.Add((T)dr[0]);
                }
                dr.Dispose();
                comm.Connection.Close();
                comm.Connection.Dispose();
                comm.Dispose();
            }
            log.ReturnValue = list;

        }
        #region  数据库链接操作
        /// <summary>
        /// 获得数据库操作对象
        /// </summary>
        /// <returns></returns>
        SqlConnection Conn()
        {
            return new SqlConnection(Url);
        }
        /// <summary>
        /// 获得SQLServer数据库操作对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        SqlCommand Comm(string sql, CommandType type, params SqlParameter[] parameter)
        {
            var comm = new SqlCommand(sql, Conn());
            if (parameter != null)
            {
                comm.Parameters.AddRange(parameter);
            }
            comm.CommandType = type;
            return comm;
        }
        #endregion
        #endregion
        #region 接口方法的实现

        public void Add<T>(T t)
        {
            DBLog log = DBLog.Init("Add_Realize", new Object[] { t }, typeof(T));
            Invoke(log);
        }
        public void Delete<T>(T t)
        {
            DBLog log = DBLog.Init("Delete_Realize", new Object[] { t }, typeof(T));
            Invoke(log);
        }
        public void Save<T>(T t)
        {
            DBLog log = DBLog.Init("Save_Realize", new Object[] { t }, typeof(T));
            Invoke(log);
        }
        public List<T> List<T>()
        {
            DBLog log = DBLog.Init("List_Realize",null, typeof(T));
            Invoke(log);
            return (List<T>)log.ReturnValue;
        }
        public List<T> Select<T>(string sql, CommandType type, params SqlParameter[] parameter)
        {
            DBLog log = DBLog.Init("Select_Realize", new Object[]{sql,type,parameter}, typeof(T));
            Invoke(log);
            return (List<T>)log.ReturnValue;
        }
        public List<T> Select<T>(string sql, params SqlParameter[] parameter)
        {
            return Select<T>(sql, CommandType.Text, parameter);
        }
        public T Reader<T>(string sql, CommandType type, params SqlParameter[] parameter)
        {
            DBLog log = DBLog.Init("Reader_Realize", new Object[] { sql, type, parameter }, typeof(T));
            Invoke(log);
            return (T)log.ReturnValue;
        }
        public T Reader<T>(string sql, params SqlParameter[] parameter)
        {
            return Reader<T>(sql, CommandType.Text, parameter);
        }
        public DataTable Table(string sql, CommandType type, params SqlParameter[] parameter)
        {
            DBLog log = DBLog.Init("Table_Realize", new Object[] { sql, type, parameter });
            Invoke(log);
            return (DataTable)log.ReturnValue;
        }
        public DataTable Table(string sql, params SqlParameter[] parameter)
        {
            return Table(sql, CommandType.Text, parameter);
        }
        public T ExecuteScalar<T>(string sql, CommandType type, params SqlParameter[] parameter)
        {
            DBLog log = DBLog.Init("ExecuteScalar_Realize", new Object[] { sql, type, parameter });
            Invoke(log);
            return (T)log.ReturnValue;
        }
        public T ExecuteScalar<T>(string sql, params SqlParameter[] parameter)
        {
            return ExecuteScalar<T>(sql, CommandType.Text, parameter);
        }
        public List<T> ExecuteListScalar<T>(string sql, CommandType type, params SqlParameter[] parameter)
        {
            DBLog log = DBLog.Init("ExecuteListScalar_Realize", new Object[] { sql, type, parameter }, typeof(T));
            Invoke(log);
            return (List<T>)log.ReturnValue;
        }
        public void ExecuteNonQuery(string sql, CommandType type, params SqlParameter[] parameter)
        {
            DBLog log = DBLog.Init("ExecuteNonQuery_Realize", new Object[] { sql, type, parameter });
            Invoke(log);
           
        }

        public void ExecuteNonQuery(string sql, params SqlParameter[] parameter)
        {
            ExecuteNonQuery(sql, CommandType.Text, parameter);
        }
        public List<T> ExecuteListScalar<T>(string sql, params SqlParameter[] parameter)
        {
            return ExecuteListScalar<T>(sql, CommandType.Text, parameter);
        }


        #endregion
        public event DBStep OnStart;
        public event DBStep OnOver;
        public event DBStep OnError;





        public void ExecuteTrain(string sql, params SqlParameter[] paprameter)
        {
            string tmsql =string.Format("begin tran  {0} if(@@ERROR<>0) begin rollback tran end else begin commit tran end",sql);
            ExecuteNonQuery(tmsql, CommandType.Text, paprameter);
        }
    }
}
