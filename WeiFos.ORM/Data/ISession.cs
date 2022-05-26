using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using WeiFos.ORM.Data;
using System.Linq.Expressions;

namespace WeiFos.ORM.Data
{
    /// <summary>
    /// @author yewei 
    /// 数据访问接口
    /// </summary>
    public interface ISession : IDisposable
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        DbConnection Connection { get; set; }

        /// <summary>
        /// 事务接口
        /// </summary>
        DbTransaction Transaction { get; set; }

        /// <summary>
        /// 开启事务
        /// </summary>
        void StartTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);

        /// <summary>
        /// 提交事务
        /// </summary>
        void Commit();

        /// <summary>
        /// 回滚事务
        /// </summary>
        void RollBack();

        /// <summary>
        /// 关闭连接
        /// </summary>
        void Colse();

        /// <summary>
        /// 新增（多表）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="tablename"></param>
        void Insert<T>(T entity, string tablename);

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Insert<T>(T entity);

        /// <summary>
        /// 新增
        /// 批量写入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void BulkInsert<T>(List<T> list);

        /// <summary>
        /// 新增
        /// 批量写入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="tablename"></param>
        void BulkInsert<T>(List<T> list, string tablename);

        /// <summary>
        /// 新增
        /// 批量写入
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="tablename"></param>
        void BulkInsert(DataTable dt, string tablename);

        /// <summary>
        /// 删除（多表）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ID"></param>
        /// <param name="tablename"></param>
        void Delete<T>(long ID, string tablename);

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ID"></param>
        void Delete<T>(long ID);

        /// <summary>
        /// 批量删除（多表）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="IDS"></param>
        /// <param name="tablename"></param>
        void Delete<T>(long[] IDS, string tablename);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="IDS"></param>
        void Delete<T>(long[] IDS);

        /// <summary>
        /// 根据ID更新（多表）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="tablename"></param>
        void Update<T>(T entity, string tablename);

        /// <summary>
        /// 根据ID更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Update<T>(T entity);

        /// <summary>
        /// 自定义条件更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Sql"></param>
        /// <param name="paraments"></param>
        void Update<T>(string Sql, params object[] paraments);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        void BatchUpdate<T>(List<T> list) where T : new();

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="whereSql"></param>
        /// <param name="paraments"></param>
        void BatchUpdate<T>(List<T> list, string tablename) where T : new();

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="columns"></param>
        void BatchUpdate<T>(List<T> list, IEnumerable<string> columns) where T : new();

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="list"></param>
        void BatchUpdate<T>(List<T> list, IEnumerable<string> columns, string tablename) where T : new();

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="columns"></param>
        /// <param name="onRelations"></param>
        void BatchUpdate<T>(List<T> list, IEnumerable<string> columns, Dictionary<string, string> onRelations) where T : new();

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="list"></param>
        /// <param name="columns"></param>
        /// <param name="onRelations"></param>
        void BatchUpdate<T>(List<T> list, IEnumerable<string> columns, Dictionary<string, string> onRelations, string tablename) where T : new();

        /// <summary>
        /// 自定义执行脚本（返回受影响行）
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="paraments"></param>
        /// <returns></returns>
        int ExcuteUpdate(string Sql, params object[] paraments);

        /// <summary>
        /// 自定义执行脚本，返回执行结果
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paraments"></param>
        /// <returns></returns>
        object ExecuteScalar(string sql, params object[] paraments);

        /// <summary>
        /// 自定义表达式执行
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        object ExecuteScalar(Criteria criteria);

        /// <summary>
        /// 执行存取过程 转DataSet 结果集
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paraments"></param>
        /// <returns></returns>
        DataSet StoredToDataSet(string sql, params object[] paraments);

        /// <summary>
        /// 根据ID获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get<T>(long id) where T : new();

        /// <summary>
        /// 根据ID获取(多表)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="tablename"></param>
        /// <returns></returns>
        T Get<T>(long id, string tablename) where T : new();

        /// <summary>
        /// 根据条件获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereSql"></param>
        /// <param name="paraments"></param>
        /// <returns></returns>
        T Get<T>(string whereSql, params object[] paraments) where T : new();

        /// <summary>
        /// 根据条件获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fields"></param>
        /// <param name="whereSql"></param>
        /// <param name="paraments"></param>
        /// <returns></returns>
        T Get<T>(string[] fields, string whereSql, params object[] paraments) where T : new();

        /// <summary>
        /// 获取TOP数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="count"></param>
        /// <param name="whereSql"></param>
        /// <param name="paraments"></param>
        /// <returns></returns>
        List<T> GetTop<T>(int count, string whereSql, params object[] paraments) where T : new();


        /// <summary>
        /// Lambda 表达式条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        List<T> Where<T>(Expression<Func<T, bool>> expression) where T : new();


        /// <summary>
        /// 自定义表达式查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        List<T> List<T>(Criteria criteria) where T : new();

        /// <summary>
        /// 自定义条件查询
        /// 返回泛型结果集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereSql"></param>
        /// <param name="paraments"></param>
        /// <returns></returns>
        List<T> List<T>(string whereSql, params object[] paraments) where T : new();


        /// <summary>
        /// 自定义条件查询
        /// 返回泛型结果集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileds"></param>
        /// <param name="whereSql"></param>
        /// <param name="paraments"></param>
        /// <returns></returns>
        List<T> List<T>(string[] fileds, string whereSql, params object[] paraments) where T : new();

        /// <summary>
        /// 自定义表达式条件查询
        /// 返回DataTable
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        DataTable Fill(Criteria criteria);

        /// <summary>
        /// 自定义条件查询
        /// 返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paraments"></param>
        /// <returns></returns>
        DataTable Fill(string sql, params object[] paraments);

        /// <summary>
        /// 数据是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereSql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int Exist<T>(string whereSql, params object[] parameters) where T : new();

        /// <summary>
        /// 数据是否存在，指定字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selectfield"></param>
        /// <param name="whereSql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int Exist<T>(string[] selectfield, string whereSql, params object[] parameters) where T : new();
    }


}
