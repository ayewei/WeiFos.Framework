using WeiFos.ORM.Data.Config;
using WeiFos.ORM.AppConfig;
using WeiFos.ORM.Data.SqlServerDBContext;

namespace WeiFos.ORM.Data
{
    /// <summary>
    /// @author yewei 
    /// 数据访问工厂
    /// </summary>
    public class SessionFactory
    {


        /// <summary>
        /// 单列模式
        /// </summary>
        private static SessionFactory instance = new SessionFactory();

        public static SessionFactory Instance
        {
            get { return SessionFactory.instance; }
        }

        /// <summary>
        /// ORM数据库访问管理层
        /// </summary>
        private DatabaseContext databaseContext = null;

        public DatabaseContext DatabaseContext
        {
            get { return databaseContext; }
            set { databaseContext = value; }
        }

        /// <summary>
        /// 实体映射管理
        /// </summary>
        private EntitiesConfigContext entitiesConfigContext;

        public SessionFactory()
        {
            //初始化实体映射层管理
            entitiesConfigContext = new EntitiesConfigContext();

            //初始化数据库操作成管理
            databaseContext = new SqlServerDatabaseContext();

            //不同的数据库 不同的类与表 配置 
            databaseContext.EntitiesConfigContext = entitiesConfigContext;
        }

        public ISession CreateSession()
        {
            return new DbSession(databaseContext);
        }


        public ISession CreateSession(ConnectionLink ConnectionLink)
        {
            return new DbSession(databaseContext, ConnectionLink);
        }

    }


}
