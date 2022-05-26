using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;
using WeiFos.ORM.AppConfig;
using WeiFos.ORM.Data.Const;

namespace WeiFos.ORM.Data.OracleDBContext
{
    /// <summary>
    /// Copyright (c) 2013-2022 深圳微狐信息科技有限公司
    /// 描 述：Oracle数据库上下文 
    /// 创建人：叶委
    /// 创建日期：2013.03.18
    /// </summary>
    public class OracleDatabaseContext : DatabaseContext
    {
  

        public override void Delete<T>(DbConnection conn, DbTransaction trans, long ID)
        {
            throw new NotImplementedException();
        }

        public override void Delete<T>(DbConnection conn, DbTransaction trans, long ID, string tablename)
        {
            throw new NotImplementedException();
        }

        public override void Delete<T>(DbConnection conn, DbTransaction trans, long[] IDS)
        {
            throw new NotImplementedException();
        }

        public override void Delete<T>(DbConnection conn, DbTransaction trans, long[] IDS, string tablename)
        {
            throw new NotImplementedException();
        }

        public override int ExcuteUpdate(DbConnection conn, DbTransaction trans, string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public override object ExecuteScalar(DbConnection conn, DbTransaction trans, string sql, params object[] paraments)
        {
            throw new NotImplementedException();
        }

        public override object ExecuteScalar(DbConnection conn, DbTransaction trans, Criteria criteria)
        {
            throw new NotImplementedException();
        }

        public override int Exist<T>(DbConnection conn, DbTransaction trans, string whereSql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public override int Exist<T>(DbConnection conn, DbTransaction trans, string[] selectfield, string whereSql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public override DataTable Fill(DbConnection conn, DbTransaction trans, Criteria criteria)
        {
            throw new NotImplementedException();
        }

        public override DataTable Fill(DbConnection conn, DbTransaction trans, string whereSql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public override string FormatExpress(string leftExp, LogicOper oper, object[] values, int index)
        {
            throw new NotImplementedException();
        }

        public override T Get<T>(DbConnection conn, DbTransaction trans, long id)
        {
            throw new NotImplementedException();
        }

        public override T Get<T>(DbConnection conn, DbTransaction trans, long id, string tablename)
        {
            throw new NotImplementedException();
        }

        public override T Get<T>(DbConnection conn, DbTransaction trans, string whereSql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public override T Get<T>(DbConnection conn, DbTransaction trans, string[] fields, string whereSql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public override DbConnection GetConnection()
        {
            throw new NotImplementedException();
        }

        public override DbConnection GetConnection(ConnectionLink ConnectionLink)
        {
            throw new NotImplementedException();
        }

        public override List<T> GetTop<T>(DbConnection conn, DbTransaction trans, int count, string whereSql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public override void Insert<T>(DbConnection conn, DbTransaction trans, T entity)
        {
            throw new NotImplementedException();
        }

        public override void Insert<T>(DbConnection conn, DbTransaction trans, T entity, string tablename)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 新增
        /// 批量写入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="entitys"></param>
        public override void BulkInsert<T>(DbConnection conn, DbTransaction trans, List<T> entitys)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 新增
        /// 批量写入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="entitys"></param>
        /// <param name="tablename"></param>
        public override void BulkInsert<T>(DbConnection conn, DbTransaction trans, List<T> entitys, string tablename)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 新增
        /// 批量写入        
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="entity"></param>
        /// <param name="tablename"></param>
        public override void BulkInsert(DbConnection conn, DbTransaction trans, DataTable entity, string tablename)
        {
            throw new NotImplementedException();
        }

        public override List<T> List<T>(DbConnection conn, DbTransaction trans, Criteria criteria)
        {
            throw new NotImplementedException();
        }

        public override List<T> List<T>(DbConnection conn, DbTransaction trans, string whereSql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public override List<T> List<T>(DbConnection conn, DbTransaction trans, string[] fields, string whereSql, params object[] parameters)
        {
            throw new NotImplementedException();
        }


        public override DataSet StoredToDataSet(DbConnection conn, DbTransaction trans, string sql, params object[] paraments)
        {
            throw new NotImplementedException();
        }

        public override void Update<T>(DbConnection conn, DbTransaction trans, T entity)
        {
            throw new NotImplementedException();
        }

        public override void Update<T>(DbConnection conn, DbTransaction trans, T entity, string tablename)
        {
            throw new NotImplementedException();
        }

        public override void Update<T>(DbConnection conn, DbTransaction trans, string Sql, params object[] paraments)
        {
            throw new NotImplementedException();
        }

        public override List<T> Where<T>(DbConnection conn, DbTransaction trans, Expression<Func<T, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public override void BatchUpdate<T>(DbConnection conn, DbTransaction trans, List<T> list)
        {
            throw new NotImplementedException();
        }

        public override void BatchUpdate<T>(DbConnection conn, DbTransaction trans, List<T> list, IEnumerable<string> columns)
        {
            throw new NotImplementedException();
        }

        public override void BatchUpdate<T>(DbConnection conn, DbTransaction trans, List<T> list, IEnumerable<string> columns, Dictionary<string, string> onRelations)
        {
            throw new NotImplementedException();
        }

        public override void BatchUpdate<T>(DbConnection conn, DbTransaction trans, List<T> list, string tablename)
        {
            throw new NotImplementedException();
        }

        public override void BatchUpdate<T>(DbConnection conn, DbTransaction trans, List<T> list, IEnumerable<string> columns, string tablename)
        {
            throw new NotImplementedException();
        }

        public override void BatchUpdate<T>(DbConnection conn, DbTransaction trans, List<T> list, IEnumerable<string> columns, Dictionary<string, string> onRelations, string tablename)
        {
            throw new NotImplementedException();
        }
    }
}
