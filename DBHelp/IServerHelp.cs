using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ALOS.DBHelp
{

    public interface IServerHelp
    {
        event DBStep OnStart;
        event DBStep OnOver;
        event DBStep OnError;
        /// <summary>
        /// 根据类型添加 到数据库
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="t">要添加的对象</param>
        void Add<T>(T t);
        /// <summary>
        /// 根据类型 带的条件删除数据
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="t"></param>
        void Delete<T>(T t);
        /// <summary>
        /// 根据类型 保存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        void Save<T>(T t);
        /// <summary>
        /// 获得 一张表的所有数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> List<T>();
        /// <summary>
        /// 根据SQL获得相关的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        List<T> Select<T>(string sql, CommandType type, params SqlParameter[] parameter);
        /// <summary>
        /// 根据SQL获得相关的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        List<T> Select<T>(string sql, params SqlParameter[] parameter);
        /// <summary>
        /// 根据SQL获得第一个对象 没有返回Null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        T Reader<T>(string sql, CommandType type, params SqlParameter[] parameter);
        /// <summary>
        /// 根据SQL获得第一个对象 没有返回Null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        T Reader<T>(string sql, params SqlParameter[] parameter);
        /// <summary>
        /// 根据SQL获得Table
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        DataTable Table(string sql, CommandType type, params SqlParameter[] parameter);
        /// <summary>
        /// 根据SQL获得Table
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        DataTable Table(string sql, params SqlParameter[] parameter);
        /// <summary>
        /// 根据SQL获得首行首列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        T ExecuteScalar<T>(string sql, CommandType type, params SqlParameter[] parameter);
        /// <summary>
        /// 根据SQL获得首行首列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        T ExecuteScalar<T>(string sql, params SqlParameter[] parameter);
        /// <summary>
        /// 根据SQL获得 首列的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        List<T> ExecuteListScalar<T>(string sql, CommandType type, params SqlParameter[] parameter);
        /// <summary>
        /// 返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameter"></param>
        void ExecuteNonQuery(string sql,CommandType type,params SqlParameter[] parameter);
        /// <summary>
        /// 返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameter"></param>
         void ExecuteNonQuery(string sql,params SqlParameter[] parameter);
        /// <summary>
        /// 事物方式执行SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paprameter"></param>
        void ExecuteTrain(string sql, params SqlParameter[] paprameter);
        /// <summary>
        /// 根据SQL获得 首列的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        List<T> ExecuteListScalar<T>(string sql, params SqlParameter[] parameter);
    }
}
