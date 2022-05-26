using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Config;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using WeiFos.ORM.AppConfig;
using WeiFos.ORM.Data.Const;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WeiFos.ORM.Data
{
    /// <summary>
    ///  @author yewei 
    ///  数据库访问管理层接口
    /// </summary>
    public abstract class DatabaseContext
    {
        /// <summary>
        /// 管理所有实体类和表映射关系
        /// </summary>
        public EntitiesConfigContext EntitiesConfigContext { get; set; }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns></returns>
        public abstract DbConnection GetConnection();

        /// <summary>
        /// 获取数据库连接（自定义数据链接）
        /// </summary>
        /// <param name="connect"></param>
        /// <returns></returns>
        public abstract DbConnection GetConnection(ConnectionLink ConnectionLink);

        public abstract int ExcuteUpdate(DbConnection conn, DbTransaction trans, string sql, params object[] parameters);

        public abstract object ExecuteScalar(DbConnection conn, DbTransaction trans, string sql, params object[] paraments);

        public abstract object ExecuteScalar(DbConnection conn, DbTransaction trans, Criteria criteria);

        public abstract DataSet StoredToDataSet(DbConnection conn, DbTransaction trans, string sql, params object[] paraments);

        public abstract void Insert<T>(DbConnection conn, DbTransaction trans, T entity);

        //public abstract Task InsertAsync<T>(DbConnection conn, DbTransaction trans, T entity);

        public abstract void Insert<T>(DbConnection conn, DbTransaction trans, T entity, string tablename);

        public abstract void BulkInsert<T>(DbConnection conn, DbTransaction trans, List<T> entitys);

        public abstract void BulkInsert<T>(DbConnection conn, DbTransaction trans, List<T> entitys, string tablename);

        public abstract void BulkInsert(DbConnection conn, DbTransaction trans, DataTable entity, string tablename);

        public abstract void Delete<T>(DbConnection conn, DbTransaction trans, long ID);

        public abstract void Delete<T>(DbConnection conn, DbTransaction trans, long ID, string tablename);

        public abstract void Delete<T>(DbConnection conn, DbTransaction trans, long[] IDS);

        public abstract void Delete<T>(DbConnection conn, DbTransaction trans, long[] IDS, string tablename);

        public abstract void Update<T>(DbConnection conn, DbTransaction trans, T entity);

        public abstract void Update<T>(DbConnection conn, DbTransaction trans, T entity, string tablename);

        public abstract void Update<T>(DbConnection conn, DbTransaction trans, string Sql, params object[] paraments);

        public abstract void BatchUpdate<T>(DbConnection conn, DbTransaction trans, List<T> list) where T : new();

        public abstract void BatchUpdate<T>(DbConnection conn, DbTransaction trans, List<T> list, IEnumerable<string> columns) where T : new();

        public abstract void BatchUpdate<T>(DbConnection conn, DbTransaction trans, List<T> list, IEnumerable<string> columns, Dictionary<string, string> onRelations) where T : new();

        public abstract void BatchUpdate<T>(DbConnection conn, DbTransaction trans, List<T> list, string tablename) where T : new();

        public abstract void BatchUpdate<T>(DbConnection conn, DbTransaction trans, List<T> list, IEnumerable<string> columns, string tablename) where T : new();

        public abstract void BatchUpdate<T>(DbConnection conn, DbTransaction trans, List<T> list, IEnumerable<string> columns, Dictionary<string, string> onRelations, string tablename) where T : new();

        public abstract T Get<T>(DbConnection conn, DbTransaction trans, long id) where T : new();

        public abstract T Get<T>(DbConnection conn, DbTransaction trans, long id, string tablename) where T : new();

        public abstract T Get<T>(DbConnection conn, DbTransaction trans, string whereSql, params object[] parameters) where T : new();

        public abstract T Get<T>(DbConnection conn, DbTransaction trans, string[] fields, string whereSql, params object[] parameters) where T : new();

        public abstract List<T> GetTop<T>(DbConnection conn, DbTransaction trans, int count, string whereSql, params object[] parameters) where T : new();

        public abstract List<T> Where<T>(DbConnection conn, DbTransaction trans, Expression<Func<T, bool>> expression) where T : new();

        public abstract List<T> List<T>(DbConnection conn, DbTransaction trans, Criteria criteria) where T : new();

        public abstract List<T> List<T>(DbConnection conn, DbTransaction trans, string whereSql, params object[] parameters) where T : new();

        public abstract List<T> List<T>(DbConnection conn, DbTransaction trans, string[] fileds, string whereSql, params object[] parameters) where T : new();

        public abstract DataTable Fill(DbConnection conn, DbTransaction trans, Criteria criteria);

        public abstract DataTable Fill(DbConnection conn, DbTransaction trans, string whereSql, params object[] parameters);

        public abstract int Exist<T>(DbConnection conn, DbTransaction trans, string whereSql, params object[] parameters) where T : new();

        public abstract int Exist<T>(DbConnection conn, DbTransaction trans, string[] selectfield, string whereSql, params object[] parameters) where T : new();


        /// <summary>
        /// 形成表达式
        /// </summary>
        /// <param name="leftExp"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public abstract string FormatExpress(string leftExp, LogicOper oper, object[] values, int index);

        /// <summary>
        /// 创建Command
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        protected DbCommand PrepareCommand(DbConnection conn, DbTransaction trans, String commandText, DbParameter[] parameters, CommandType commandType)
        {
            //创建执行的Command 
            DbCommand Command = conn.CreateCommand();

            //设置执行Sql或存储过程
            Command.CommandText = commandText;

            //默认
            Command.CommandTimeout = 100;

            //是执行Sql或存储过程
            Command.CommandType = commandType;

            //Command增加事务处理对象
            if (trans != null)
            {
                Command.Transaction = trans;
            }

            //处理Command执行参数
            if (parameters != null && parameters.Length > 0)
            {
                Command.Parameters.AddRange(parameters);
            }
            return Command;
        }



    }
}
