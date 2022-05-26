using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using WeiFos.ORM.AppConfig;
using WeiFos.ORM.Data.Config;
using System.Linq.Expressions;

namespace WeiFos.ORM.Data
{
    /// <summary>
    /// @author yewei 
    /// 与数据访问层的会话的实现
    /// </summary>
    public class DbSession : ISession
    {
        private DatabaseContext databaseContext = null;

        private DbConnection connection = null;

        private DbTransaction transaction = null;

        public DbConnection Connection
        {
            get
            {
                return connection;
            }
            set
            {
                connection = value;
            }
        }

        public DbTransaction Transaction
        {
            get
            {
                return transaction;
            }
            set
            {
                transaction = value;
            }
        }

        /// <summary>
        /// DbSession构造函数
        /// </summary>
        /// <param name="databaseContext"></param>
        public DbSession(DatabaseContext databaseContext)
        {
            bool is_check = DbSessionSetting.Instance.Get();
            if (is_check)
            {
                this.databaseContext = databaseContext;
                //创建数据库的连接
                this.connection = this.databaseContext.GetConnection();
                if (this.connection.State != ConnectionState.Open)
                {
                    this.connection.Open();
                }
            }
        }

        /// <summary>
        /// DbSession 带参构造函数
        /// </summary>
        /// <param name="databaseContext"></param>
        /// <param name="connectionString"></param>
        public DbSession(DatabaseContext databaseContext, ConnectionLink connectionLink)
        {
            this.databaseContext = databaseContext;
            //创建数据库的连接
            this.connection = this.databaseContext.GetConnection(connectionLink);

            if (this.connection.State != ConnectionState.Open)
            {
                this.connection.Open();
            }
        }

        /// <summary>
        /// 开启事务 默认为 ReadCommitted
        /// </summary>
        public void StartTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            transaction = connection.BeginTransaction(isolationLevel);
        }

        /// <summary>
        /// 可以进行脏读，意思是说，不发布共享锁，也不接受独占锁
        /// </summary>
        public void TransactionUnocmmitted()
        {
            transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);
        }

        /// <summary>
        /// 在正在读取数据时保持共享锁，以避免脏读，但是在事务结束之前可以更改数据，从而导致不可重复的读取或幻像数据。
        /// </summary>
        public void TransactionCommitted()
        {
            transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        /// <summary>
        /// 在查询中使用的所有数据上放置锁，以防止其他用户更新这些数据。防止不可重复的读取，但是仍可以有幻像行
        /// </summary>
        public void TransactionRepeatableRead()
        {
            transaction = connection.BeginTransaction(IsolationLevel.RepeatableRead);
        }

        /// <summary>
        /// 保证所有情况不会发生（锁表）效率低，不建议使用
        /// </summary>
        public void TransactionSerializable()
        {
            transaction = connection.BeginTransaction(IsolationLevel.Serializable);
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            transaction.Commit();
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollBack()
        {
            transaction.Rollback();
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Colse()
        {
            this.connection.Close();
        }

        public void BulkInsert<T>(List<T> list)
        {
            databaseContext.BulkInsert<T>(this.connection, this.transaction, list);
        }

        public void BulkInsert<T>(List<T> list, string tablename)
        {
            databaseContext.BulkInsert<T>(this.connection, this.transaction, list, tablename);
        }

        public void BulkInsert(DataTable dt, string tablename)
        {
            databaseContext.BulkInsert(this.connection, this.transaction, dt, tablename);
        }

        public void Insert<T>(T entity, string tablename)
        {
            databaseContext.Insert<T>(this.connection, this.transaction, entity, tablename);
        }

        public void Insert<T>(T entity)
        {
            databaseContext.Insert<T>(this.connection, this.transaction, entity);
        }

        public void Delete<T>(long ID, string tablename)
        {
            databaseContext.Delete<T>(this.connection, this.transaction, ID, tablename);
        }

        public void Delete<T>(long ID)
        {
            databaseContext.Delete<T>(this.connection, this.transaction, ID);
        }

        public void Delete<T>(long[] IDS, string tablename)
        {
            databaseContext.Delete<T>(this.connection, this.transaction, IDS, tablename);
        }

        public void Delete<T>(long[] IDS)
        {
            databaseContext.Delete<T>(this.connection, this.transaction, IDS);
        }

        public void Update<T>(T entity, string tablename)
        {
            databaseContext.Update<T>(this.connection, this.transaction, entity, tablename);
        }

        public void Update<T>(T entity)
        {
            databaseContext.Update<T>(this.connection, this.transaction, entity);
        }

        public void Update<T>(string sql, params object[] paraments)
        {
            databaseContext.Update<T>(this.connection, this.transaction, sql, paraments);
        }

        public void BatchUpdate<T>(List<T> list) where T : new()
        {
            databaseContext.BatchUpdate(this.connection, this.transaction, list);
        }

        public void BatchUpdate<T>(List<T> list, IEnumerable<string> columns) where T : new()
        {
            databaseContext.BatchUpdate(this.connection, this.transaction, list, columns);
        }

        public void BatchUpdate<T>(List<T> list, IEnumerable<string> columns, Dictionary<string, string> onRelations) where T : new()
        {
            databaseContext.BatchUpdate(this.connection, this.transaction, list, columns, onRelations);
        }

        public void BatchUpdate<T>(List<T> list, string tablename) where T : new()
        {
            databaseContext.BatchUpdate(this.connection, this.transaction, list, tablename);
        }

        public void BatchUpdate<T>(List<T> list, IEnumerable<string> columns, string tablename) where T : new()
        {
            databaseContext.BatchUpdate(this.connection, this.transaction, list, columns, tablename);
        }

        public void BatchUpdate<T>(List<T> list, IEnumerable<string> columns, Dictionary<string, string> onRelations, string tablename) where T : new()
        {
            databaseContext.BatchUpdate(this.connection, this.transaction, list, columns, onRelations, tablename);
        }

        public int ExcuteUpdate(string sql, params object[] paraments)
        {
            return databaseContext.ExcuteUpdate(this.connection, this.transaction, sql, paraments);
        }

        public object ExecuteScalar(string sql, params object[] paraments)
        {
            return databaseContext.ExecuteScalar(this.connection, this.transaction, sql, paraments);
        }

        public object ExecuteScalar(Criteria criteria)
        {
            return databaseContext.ExecuteScalar(this.connection, this.transaction, criteria);
        }

        public DataSet StoredToDataSet(string sql, params object[] paraments)
        {
            return databaseContext.StoredToDataSet(this.connection, this.transaction, sql, paraments);
        }

        public T Get<T>(long id, string tablename) where T : new()
        {
            return databaseContext.Get<T>(this.connection, this.transaction, id, tablename);
        }

        public T Get<T>(long id) where T : new()
        {
            return databaseContext.Get<T>(this.connection, this.transaction, id);
        }

        public T Get<T>(string whereSql, params object[] paraments) where T : new()
        {
            return databaseContext.Get<T>(this.connection, this.transaction, whereSql, paraments);
        }

        public T Get<T>(string[] fields, string whereSql, params object[] paraments) where T : new()
        {
            return databaseContext.Get<T>(this.connection, this.transaction, fields, whereSql, paraments);
        }

        public List<T> Where<T>(Expression<Func<T, bool>> expression) where T : new()
        {
            return databaseContext.Where<T>(this.connection, this.transaction, expression);
        }

        public List<T> GetTop<T>(int count, string whereSql, params object[] paraments) where T : new()
        {
            return databaseContext.GetTop<T>(this.connection, this.transaction, count, whereSql, paraments);
        }

        public List<T> List<T>(Criteria criteria) where T : new()
        {
            return databaseContext.List<T>(this.connection, this.transaction, criteria);
        }

        public List<T> List<T>(string whereSql, params object[] paraments) where T : new()
        {
            return databaseContext.List<T>(this.connection, this.transaction, whereSql, paraments);
        }

        public List<T> List<T>(string[] fileds, string whereSql, params object[] paraments) where T : new()
        {
            return databaseContext.List<T>(this.connection, this.transaction, fileds, whereSql, paraments);
        }

        public DataTable Fill(Criteria criteria)
        {
            return databaseContext.Fill(this.connection, this.transaction, criteria);
        }

        public DataTable Fill(string sql, params object[] paraments)
        {
            return databaseContext.Fill(this.connection, this.transaction, sql, paraments);
        }

        public int Exist<T>(string whereSql, params object[] parameters) where T : new()
        {
            return databaseContext.Exist<T>(this.connection, this.transaction, whereSql, parameters);
        }

        public int Exist<T>(string[] selectfield, string whereSql, params object[] parameters) where T : new()
        {
            return databaseContext.Exist<T>(this.connection, this.transaction, whereSql, parameters);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected bool IsDispose = false;

        protected void Dispose(bool disposing)
        {
            if (!IsDispose)
            {
                if (disposing)
                {
                    if (this.connection != null)
                    {
                        this.connection.Close();
                        this.connection = null;
                    }
                }
            }
            IsDispose = true;
        }


        ~DbSession()
        {
            Dispose(false);
        }

    }
}
