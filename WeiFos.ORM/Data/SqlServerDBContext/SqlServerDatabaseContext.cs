using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using WeiFos.ORM.Data.Config;
using WeiFos.ORM.Data.Const;
using WeiFos.ORM.AppConfig;
using WeiFos.ORM.Data.Restrictions;
using WeiFos.ORM.Restrictions.LambdaExp;
using System.Linq.Expressions;
using WeiFos.ORM.Data.DBEntityModule;
using System.Reflection;
using System.Text;

namespace WeiFos.ORM.Data.SqlServerDBContext
{

    /// <summary>
    /// Copyright (c) 2013-2022 深圳微狐信息科技有限公司
    /// 描 述：SqlServer数据库上下文 
    /// 创建人：叶委
    /// 创建日期：2013.03.18
    /// </summary>
    public class SqlServerDatabaseContext : DatabaseContext
    {

        /// <summary>
        /// 默认数据库链接
        /// </summary>
        /// <returns></returns>
        public override DbConnection GetConnection()
        {
            return new SqlConnection(ConnectionConfig.GetConnection(ConnectionLink.Link1));
        }



        /// <summary>
        /// 自定义数据库链接
        /// </summary>
        /// <param name="connectionLink"></param>
        /// <returns></returns>
        public override DbConnection GetConnection(ConnectionLink connectionLink)
        {
            return new SqlConnection(ConnectionConfig.GetConnection(connectionLink));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private List<FieldDetail> GetTableColumns(DbConnection conn, DbTransaction trans, string tableName)
        {
            string sql = string.Format("select * from syscolumns inner join sysobjects on syscolumns.id=sysobjects.id where sysobjects.xtype='U' and sysobjects.name='{0}' order by syscolumns.colid asc", tableName);

            List<FieldDetail> columns = new List<FieldDetail>();

            DbCommand selectCommand = PrepareCommand(conn, trans, sql, null, CommandType.Text);
            using (DbDataReader dr = selectCommand.ExecuteReader())
            {
                var properties = typeof(FieldDetail).GetProperties().ToList();
                while (dr.Read())
                {
                    FieldDetail obj = new FieldDetail();
                    foreach (var property in properties)
                    {
                        try
                        {
                            //Oracle字段为大写
                            var id = dr.GetOrdinal(property.Name.ToUpper());
                            if (!dr.IsDBNull(id))
                            {
                                if (dr.GetValue(id) != DBNull.Value)
                                {
                                    property.SetValue(obj, dr.GetValue(id));
                                }
                            }
                        }
                        catch { }
                    }
                    columns.Add(obj);

                    //FieldDetail column = new FieldDetail();
                    //var name_index = dr.GetOrdinal("name");
                    //if (!dr.IsDBNull(name_index))
                    //{
                    //    if (dr.GetValue(name_index) != DBNull.Value)
                    //    {
                    //        column.name = dr.GetValue(name_index).ToString();
                    //    }
                    //}

                    //var colorder_index = dr.GetOrdinal("colorder");
                    //if (!dr.IsDBNull(colorder_index))
                    //{
                    //    if (dr.GetValue(colorder_index) != DBNull.Value)
                    //    {
                    //        column.order_index = int.Parse(dr.GetValue(colorder_index).ToString());
                    //    }
                    //}
                    //columns.Add(column);
                }

                return columns;
            }
        }


        private Type GetUnderlyingType(Type type)
        {
            Type unType = Nullable.GetUnderlyingType(type); ;
            if (unType == null)
                unType = type;

            return unType;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="modelList"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable ToSqlBulkCopyDataTable<TModel>(List<TModel> modelList, DbConnection conn, DbTransaction trans, string tableName)
        {
            DataTable dt = new DataTable();

            Type modelType = typeof(TModel);

            List<FieldDetail> columns = GetTableColumns(conn, trans, tableName);
            List<PropertyInfo> mappingProps = new List<PropertyInfo>();

            var props = modelType.GetProperties();
            for (int i = 0; i < columns.Count; i++)
            {
                var column = columns[i];
                PropertyInfo mappingProp = props.Where(a => a.Name == column.name).FirstOrDefault();
                if (mappingProp == null)
                    throw new Exception(string.Format("model 类型 '{0}'未定义与表 '{1}' 列名为 '{2}' 映射的属性", modelType.FullName, tableName, column.name));

                mappingProps.Add(mappingProp);
                Type dataType = GetUnderlyingType(mappingProp.PropertyType);
                if (dataType.IsEnum) dataType = typeof(int);
                dt.Columns.Add(new DataColumn(column.name, dataType));
            }

            foreach (var model in modelList)
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < mappingProps.Count; i++)
                {
                    PropertyInfo prop = mappingProps[i];
                    object value = prop.GetValue(model);

                    if (GetUnderlyingType(prop.PropertyType).IsEnum)
                    {
                        if (value != null) value = (int)value;
                    }

                    dr[i] = value ?? DBNull.Value;
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }

        /// <summary>
        /// 把当前记录 转成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <param name="pcs"></param>
        /// <param name="obj"></param>
        protected void Mapping<T>(DbDataReader dr, PropertyConfig[] pcs, T obj)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                string filed_name = dr.GetName(i);
                PropertyConfig pconfig = pcs.Where(p => filed_name.Equals(p.PropertyInfo.Name)).SingleOrDefault();
                if (pconfig != null)
                {
                    object value = dr.GetValue(i);
                    if (value != DBNull.Value)
                    {
                        //通过反射 给 对象 obj 的相对应属性 赋值 p 代表类的某个属性， obj是类的实例，value 赋给属性的值
                        pconfig.PropertyInfo.SetValue(obj, value, null);
                    }
                }
            }
        }

        /// <summary>
        /// 把当前记录 转成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <param name="pcs"></param>
        /// <param name="obj"></param>
        /// <param name="ppt"></param>
        protected void Mapping<T>(DbDataReader dr, PropertyConfig[] pcs, T obj, string[] ppt)
        {
            foreach (PropertyConfig p in pcs)
            {
                if (ppt != null && ppt.Count() > 0)
                {
                    for (int i = 0; i < ppt.Count(); i++)
                    {
                        if (p.PropertyInfo.Name.Equals(ppt[i]))
                        {
                            //通过属性名获取 属性值
                            object value = dr[p.PropertyInfo.Name];
                            if (value != DBNull.Value)
                            {
                                //通过反射 给 对象 obj 的相对应属性 赋值 p 代表类的某个属性， obj是类的实例，value 赋给属性的值
                                p.PropertyInfo.SetValue(obj, value, null);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="entity"></param>
        /// <param name="tablename"></param>
        public override void Insert<T>(DbConnection conn, DbTransaction trans, T entity, string tablename)
        {
            //获取实体对象和表映射关系的配置信息
            EntityConfig ec = EntitiesConfigContext[entity.GetType()];

            //获取非主键且非自动增长 
            PropertyConfig[] pcs = ec.PropertyConfigs.Where(p => !p.IsPrimaryKey && p.PropertyInfo.GetValue(entity, null) != null && !p.UnMappingField || (p.IsPrimaryKey && p.Generator == KeyGenerator.Manual)).ToArray();

            string fields = string.Join(",", pcs.Where(p => !p.UnMappingField).Select(p => p.PropertyInfo.Name).ToArray());
            //参数信息
            string paramNames = string.Join(",", pcs.Where(p => !p.UnMappingField).Select(p => "@" + p.PropertyInfo.Name).ToArray());

            //执行sql的参数值
            SqlParameter[] paramValues = pcs.Where(p => !p.UnMappingField).Select(p => new SqlParameter("@" + p.PropertyInfo.Name, p.PropertyInfo.GetValue(entity, null))).ToArray();

            //处理自定义表名
            string tb_name = string.IsNullOrEmpty(tablename) ? ec.TableName : tablename;

            //拼Insert Sql
            string sql = string.Format(SqlTemplate.InsertSqlTemplete, tb_name, fields, paramNames);

            DbCommand insertCommand = PrepareCommand(conn, trans, sql, paramValues, CommandType.Text);

            insertCommand.ExecuteNonQuery();

            //获取自动增长的ID
            PropertyConfig IdentityPc = ec.PropertyConfigs.Where(p => p.IsPrimaryKey && p.Generator == KeyGenerator.Identity).SingleOrDefault();

            if (IdentityPc != null)
            {
                DbCommand IdCommand = PrepareCommand(conn, trans, SqlTemplate.GetIdSql, null, CommandType.Text);
                //反过来 给对象ID属性 赋 自动增长的ID值
                IdentityPc.PropertyInfo.SetValue(entity, Convert.ToInt32(IdCommand.ExecuteScalar()), null);
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="entity"></param>
        public override void Insert<T>(DbConnection conn, DbTransaction trans, T entity)
        {
            Insert<T>(conn, trans, entity, string.Empty);
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
            //获取实体类和表的映射关系
            EntityConfig ec = EntitiesConfigContext[typeof(T)];

            //将要写入的数据
            DataTable dtToWrite = ToSqlBulkCopyDataTable(entitys, conn, trans, ec.TableName);

            using (SqlBulkCopy sbc = new SqlBulkCopy((SqlConnection)conn, SqlBulkCopyOptions.Default, (SqlTransaction)trans))
            {
                sbc.BatchSize = entitys.Count;
                sbc.DestinationTableName = ec.TableName;
                //超时时间
                sbc.BulkCopyTimeout = 60;
                //批量写入
                sbc.WriteToServer(dtToWrite);
            }
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
            //获取实体类和表的映射关系
            EntityConfig ec = EntitiesConfigContext[typeof(T)];

            if (string.IsNullOrEmpty(tablename)) tablename = ec.TableName;

            //将要写入的数据
            DataTable dtToWrite = ToSqlBulkCopyDataTable(entitys, conn, trans, tablename);

            using (SqlBulkCopy sbc = new SqlBulkCopy((SqlConnection)conn, SqlBulkCopyOptions.Default, (SqlTransaction)trans))
            {
                sbc.BatchSize = entitys.Count;
                sbc.DestinationTableName = tablename;
                sbc.BulkCopyTimeout = 60;
                sbc.WriteToServer(dtToWrite);
            }
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
            bool shouldCloseConnection = false;

            if (entity.Rows.Count > 0)
            {
                try
                {
                    using (SqlBulkCopy sbc = new SqlBulkCopy((SqlConnection)conn, SqlBulkCopyOptions.Default, (SqlTransaction)trans))
                    {
                        sbc.BatchSize = entity.Rows.Count;
                        sbc.DestinationTableName = tablename;
                        sbc.BulkCopyTimeout = 60;

                        if (conn.State != ConnectionState.Open)
                        {
                            shouldCloseConnection = true;
                            conn.Open();
                        }

                        sbc.WriteToServer(entity);
                    }
                }
                finally
                {
                    if (shouldCloseConnection && conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="list"></param>
        public override void BatchUpdate<T>(DbConnection conn, DbTransaction trans, List<T> list)
        {
            BatchUpdate(conn, trans, list, null, null, null);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="list"></param>
        public override void BatchUpdate<T>(DbConnection conn, DbTransaction trans, List<T> list, IEnumerable<string> columns)
        {
            BatchUpdate(conn, trans, list, columns, null, null);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="list"></param>
        /// <param name="tablename"></param>
        public override void BatchUpdate<T>(DbConnection conn, DbTransaction trans, List<T> list, string tablename)
        {
            BatchUpdate(conn, trans, list, null, null, tablename);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="list"></param>
        /// <param name="columns"></param>
        /// <param name="tablename"></param>
        public override void BatchUpdate<T>(DbConnection conn, DbTransaction trans, List<T> list, IEnumerable<string> columns, string tablename)
        {
            BatchUpdate(conn, trans, list, columns, null, tablename);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="list"></param>
        /// <param name="columns"></param>
        /// <param name="onRelations"></param>
        /// <param name="tablename"></param>
        public override void BatchUpdate<T>(DbConnection conn, DbTransaction trans, List<T> list, IEnumerable<string> columns, Dictionary<string, string> onRelations)
        {
            BatchUpdate(conn, trans, list, columns, onRelations, null);
        }

        /// <summary>
        /// 修改 批量修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="list"></param>
        /// <param name="columns"></param>
        /// <param name="onRelations"></param>
        public override void BatchUpdate<T>(DbConnection conn, DbTransaction trans, List<T> list, IEnumerable<string> columns, Dictionary<string, string> onRelations, string tablename)
        {
            //需要更新的列
            var updateColumns = new StringBuilder();
            //修改的条件
            var whereSql = new StringBuilder();
            //获取实体类和表的映射关系
            EntityConfig ec = EntitiesConfigContext[typeof(T)];
            //表名
            var tableName = string.IsNullOrEmpty(tablename) ? ec.TableName : tablename;

            //构建需要更新的列
            if (columns == null)
            {
                //修改非主键,非映射所有的列
                foreach (var f in ec.PropertyConfigs.Where(p => !p.IsPrimaryKey && !p.UnMappingField))
                {
                    if (updateColumns.Length > 0) updateColumns.Append(",");
                    updateColumns.AppendFormat("T.{0} = Temp.{0}", f.FileName);
                }
            }
            else
            {
                foreach (var colname in columns)
                {
                    if (updateColumns.Length > 0) updateColumns.Append(",");
                    updateColumns.AppendFormat("T.{0} = Temp.{0}", colname);
                }
            }

            //没给修改条件情况，默认为ID为修改条件
            if (onRelations == null)
            {
                PropertyConfig pkey = ec.PropertyConfigs.Where(p => p.IsPrimaryKey).SingleOrDefault();
                whereSql.AppendFormat("T.{0} = Temp.{0}", pkey.PropertyInfo.Name);
            }
            else
            {
                foreach (var onRelation in onRelations)
                {
                    if (whereSql.Length > 0) whereSql.Append(" AND ");
                    whereSql.AppendFormat("T.{0} = Temp.{1}", onRelation.Key, onRelation.Value);
                }
            }


            //临时表
            var tempTableName = $"Temp{DateTime.Now.ToString("yyMMddHHmmss")}";

            //构建临时表
            string sql = $"SELECT * INTO {tempTableName} FROM {tableName} WHERE 1 = 2;SET IDENTITY_INSERT { tempTableName } OFF ";

            //建临时表命令
            DbCommand tbCommand = PrepareCommand(conn, trans, sql, null, CommandType.Text);
            tbCommand.ExecuteNonQuery();

            //插入临时表
            var dt = ToSqlBulkCopyDataTable(list, conn, trans, tempTableName);

            //SqlBulkCopyOptions.KeepIdentity 保持原有的主键ID不变写入到临时表
            using (SqlBulkCopy sbc = new SqlBulkCopy((SqlConnection)conn, SqlBulkCopyOptions.KeepIdentity, (SqlTransaction)trans))
            {
                sbc.DestinationTableName = tempTableName;
                sbc.BatchSize = list.Count;
                if (dt != null && dt.Rows.Count != 0)
                {
                    sbc.WriteToServer(dt);
                }
            }

            //更新
            //string update_sql = $"update T set {updateColumns.ToString()} FROM {tableName} T INNER JOIN {tempTableName} Temp ON {whereSql.ToString()}; ";
            string update_sql = $"update T set {updateColumns.ToString()} FROM {tableName} T INNER JOIN {tempTableName} Temp ON {whereSql.ToString()}; DROP TABLE {tempTableName};";
            DbCommand command = PrepareCommand(conn, trans, update_sql, null, CommandType.Text);
            command.ExecuteNonQuery();
        }


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="ID"></param>
        /// <param name="tablename"></param>
        public override void Delete<T>(DbConnection conn, DbTransaction trans, long ID, string tablename)
        {
            //获取实体对象和表映射关系的配置信息
            EntityConfig ec = EntitiesConfigContext[typeof(T)];

            string idfield = string.Join(",", ec.PropertyConfigs.Where(p => p.IsPrimaryKey).Select(p => p.PropertyInfo.Name + "=@" + p.PropertyInfo.Name).ToArray());

            SqlParameter[] paramValues = ec.PropertyConfigs.Where(p => p.IsPrimaryKey).Select(p => new SqlParameter("@" + p.PropertyInfo.Name, ID)).ToArray();

            //处理自定义表名
            string tb_name = string.IsNullOrEmpty(tablename) ? ec.TableName : tablename;

            string sql = string.Format(SqlTemplate.DeleteTemplete, tb_name, idfield);

            DbCommand Command = PrepareCommand(conn, trans, sql, paramValues, CommandType.Text);

            Command.ExecuteNonQuery();
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="ID"></param>
        public override void Delete<T>(DbConnection conn, DbTransaction trans, long ID)
        {
            Delete<T>(conn, trans, ID, string.Empty);
        }

        public override void Delete<T>(DbConnection conn, DbTransaction trans, long[] IDS, string tablename)
        {

            //获取实体对象和表映射关系的配置信息
            EntityConfig ec = EntitiesConfigContext[typeof(T)];
            //删除的Id
            string[] idsfield = new string[IDS.Length];
            //获取主键信息
            string primaryKey = ec.PropertyConfigs.Where(p => p.IsPrimaryKey).SingleOrDefault().PropertyInfo.Name;

            SqlParameter[] paramsValues = new SqlParameter[IDS.Length];
            for (int i = 0; i < IDS.Length; i++)
            {
                paramsValues[i] = new SqlParameter("@" + i, IDS[i]);
                idsfield[i] = primaryKey + "=" + IDS[i];
            }

            //条件语句
            string idfield = string.Join(" or ", idsfield);

            //处理自定义表名
            string tb_name = string.IsNullOrEmpty(tablename) ? ec.TableName : tablename;

            string sql = string.Format(SqlTemplate.DeleteTemplete, tb_name, idfield);

            DbCommand Command = PrepareCommand(conn, trans, sql, paramsValues, CommandType.Text);

            Command.ExecuteNonQuery();
        }

        public override void Delete<T>(DbConnection conn, DbTransaction trans, long[] IDS)
        {
            Delete<T>(conn, trans, IDS, string.Empty);
        }

        public override void Update<T>(DbConnection conn, DbTransaction trans, T entity, string tablename)
        {
            //获取实体对象和表映射关系的配置信息
            EntityConfig ec = EntitiesConfigContext[entity.GetType()];

            PropertyConfig[] pcs = ec.PropertyConfigs.Where(p => p.PropertyInfo.GetValue(entity, null) != null).ToArray();

            //获取实体对象非主键属性数据
            string updateField = string.Join(",", pcs.Where(p => !p.IsPrimaryKey && !p.UnMappingField).Select(p => p.PropertyInfo.Name + "=@" + p.PropertyInfo.Name).ToArray());
            //获取实体对象主键属性数据
            string idField = string.Join(" and ", pcs.Where(p => p.IsPrimaryKey && !p.UnMappingField).Select(p => p.PropertyInfo.Name + "=@" + p.PropertyInfo.Name).ToArray());

            //获取修改参数信息
            SqlParameter[] paramValues = pcs.Where(p => !p.UnMappingField).Select(p => new SqlParameter("@" + p.PropertyInfo.Name, p.PropertyInfo.GetValue(entity, null))).ToArray();

            //处理自定义表名
            string tb_name = string.IsNullOrEmpty(tablename) ? ec.TableName : tablename;

            //update {0}##表 set 修改字段 where 条件 {1}
            string sql = string.Format(SqlTemplate.UpdateSqlTemplete, tb_name, updateField, idField);

            DbCommand updateCommand = PrepareCommand(conn, trans, sql, paramValues, CommandType.Text);

            updateCommand.ExecuteNonQuery();
        }

        public override void Update<T>(DbConnection conn, DbTransaction trans, T entity)
        {
            Update<T>(conn, trans, entity, string.Empty);
        }

        /// <summary>
        /// 自定义修改方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="Sql"></param>
        /// <param name="paraments"></param>
        public override void Update<T>(DbConnection conn, DbTransaction trans, string Sql, params object[] paraments)
        {
            SqlParameter[] paramValues = null;

            if (paraments != null && paraments.Length > 0)
            {
                paramValues = new SqlParameter[paraments.Length];
                for (int i = 0; i < paraments.Length; i++)
                {
                    paramValues[i] = new SqlParameter("@" + i, paraments[i]);
                }
            }

            DbCommand updateCommand = PrepareCommand(conn, trans, Sql, paramValues, CommandType.Text);

            updateCommand.ExecuteNonQuery();
        }


        public override T Get<T>(DbConnection conn, DbTransaction trans, long id, string tablename)
        {
            //获取实体对象和表映射关系的配置信息
            EntityConfig ec = EntitiesConfigContext[typeof(T)];

            PropertyConfig[] pcs = ec.PropertyConfigs;

            //拼凑所有字段
            string allField = string.Join(",", pcs.Where(p => !p.UnMappingField).Select(p => p.PropertyInfo.Name).ToArray());

            //拼凑 主键
            string idField = string.Join(",", pcs.Where(p => p.IsPrimaryKey && !p.UnMappingField).Select(p => p.PropertyInfo.Name + "=@" + p.PropertyInfo.Name).ToArray());

            //处理自定义表名
            string tb_name = string.IsNullOrEmpty(tablename) ? ec.TableName : tablename;

            string sql = string.Format(SqlTemplate.SelectByIDSqlTemplete, allField, tb_name, idField);

            SqlParameter[] paramValues = pcs.Where(p => p.IsPrimaryKey).Select(p => new SqlParameter("@" + p.PropertyInfo.Name, id)).ToArray();

            DbCommand selectCommand = PrepareCommand(conn, trans, sql, paramValues, CommandType.Text);

            using (DbDataReader dr = selectCommand.ExecuteReader())
            {
                if (dr.Read())
                {
                    T obj = new T();
                    Mapping(dr, pcs, obj);

                    return obj;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public override T Get<T>(DbConnection conn, DbTransaction trans, long id)
        {
            return Get<T>(conn, trans, id, string.Empty);
        }

        /// <summary>
        /// 根据条件获取单条记录
        /// 存在多条记录则抛出异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="whereSql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override T Get<T>(DbConnection conn, DbTransaction trans, string whereSql, params object[] parameters)
        {
            return Get<T>(conn, trans, null, whereSql, parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="fields"></param>
        /// <param name="whereSql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override T Get<T>(DbConnection conn, DbTransaction trans, string[] fields, string whereSql, params object[] parameters)
        {
            //获取实体类和表的映射关系
            EntityConfig ec = EntitiesConfigContext[typeof(T)];
            //获取所有的属性
            PropertyConfig[] pcs = ec.PropertyConfigs;

            SqlParameter[] paramsValues = null;

            if (parameters != null && parameters.Length > 0)
            {
                paramsValues = new SqlParameter[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                {
                    paramsValues[i] = new SqlParameter("@" + i, parameters[i]);
                }
            }

            string fields_str = "*";
            if (fields != null && fields.Count() > 0)
            {
                fields_str = string.Join(",", fields);
            }

            //处理自定义表名
            string tb_name = string.Empty;

            //处理自定义表名
            if (whereSql.IndexOf('@') == 0)
            {
                int index = whereSql.IndexOf(' ');

                tb_name = whereSql.Substring(1, index);

                whereSql = whereSql.Substring(index, whereSql.Length - index);
            }

            //处理自定义表名
            tb_name = string.IsNullOrEmpty(tb_name) ? ec.TableName : tb_name;

            string sql = string.Format(SqlTemplate.SelectSqlTemplete, fields_str, tb_name, whereSql, "");

            DbCommand selectCommand = PrepareCommand(conn, trans, sql, paramsValues, CommandType.Text);
            using (DbDataReader dr = selectCommand.ExecuteReader())
            {
                List<T> rsList = new List<T>();
                while (dr.Read())
                {
                    T obj = new T();
                    Mapping(dr, pcs, obj);
                    rsList.Add(obj);
                }

                if (rsList != null && rsList.Count > 0)
                {
                    if (rsList.Count == 1)
                    {
                        return rsList[0];
                    }
                    else
                    {
                        throw new Exception("根据该条件查询，数据库存在多条记录");
                    }
                }
                else
                {
                    return default(T);
                }
            }
        }


        /// <summary>
        /// 获取数据库TOP数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="count"></param>
        /// <param name="whereSql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override List<T> GetTop<T>(DbConnection conn, DbTransaction trans, int count, string whereSql, params object[] parameters)
        {
            //获取实体类和表的映射关系
            EntityConfig ec = EntitiesConfigContext[typeof(T)];
            //获取所有的属性
            PropertyConfig[] pcs = ec.PropertyConfigs;

            SqlParameter[] paramsValues = null;

            if (parameters != null && parameters.Length > 0)
            {
                paramsValues = new SqlParameter[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                {
                    paramsValues[i] = new SqlParameter("@" + i, parameters[i]);
                }
            }

            string fields = string.Join(",", pcs.Where(p => !p.UnMappingField).Select(p => p.PropertyInfo.Name).ToArray());

            //处理自定义表名
            string tb_name = string.Empty;

            //处理自定义表名
            if (whereSql.IndexOf('@') == 0)
            {
                int index = whereSql.IndexOf(' ');

                tb_name = whereSql.Substring(1, index);

                whereSql = whereSql.Substring(index, whereSql.Length - index);
            }

            //处理自定义表名
            tb_name = string.IsNullOrEmpty(tb_name) ? ec.TableName : tb_name;

            string sql = string.Format(SqlTemplate.SelectTopByWhereSqlTemplete, count, fields, tb_name, whereSql, "");

            DbCommand selectCommand = PrepareCommand(conn, trans, sql, paramsValues, CommandType.Text);
            using (DbDataReader dr = selectCommand.ExecuteReader())
            {
                List<T> rsList = new List<T>();
                while (dr.Read())
                {
                    T obj = new T();
                    Mapping(dr, pcs, obj);
                    rsList.Add(obj);
                }
                return rsList;
            }
        }


        /// <summary>
        /// Lambda 表达式查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public override List<T> Where<T>(DbConnection conn, DbTransaction trans, Expression<Func<T, bool>> expression)
        {
            //实体类和表的映射关系
            EntityConfig ec = EntitiesConfigContext[typeof(T)];

            //该实体属性
            PropertyConfig[] pcs = ec.PropertyConfigs;

            List<string> condition = new List<string>();

            condition.Add(String.Format("({0})", expression.Analysis()));

            //条件语句
            string whereSql = " where " + string.Join(" AND ", condition.ToArray());

            //SqlParameter[] paramsValues = null;
            //object[] parameters = new object[] { };
            //if (parameters != null && parameters.Length > 0)
            //{
            //    paramsValues = new SqlParameter[parameters.Length];
            //    for (int i = 0; i < parameters.Length; i++)
            //    {
            //        paramsValues[i] = new SqlParameter("@" + i, parameters[i]);
            //    }
            //}

            string fields = string.Join(",", pcs.Where(p => !p.UnMappingField).Select(p => p.PropertyInfo.Name).ToArray());

            //处理自定义表名
            string tb_name = string.Empty;

            //处理自定义表名
            if (whereSql.IndexOf('@') == 0)
            {
                int index = whereSql.IndexOf(' ');

                tb_name = whereSql.Substring(1, index);

                whereSql = whereSql.Substring(index, whereSql.Length - index);
            }

            //处理自定义表名
            tb_name = string.IsNullOrEmpty(tb_name) ? ec.TableName : tb_name;

            string sql = string.Format(SqlTemplate.SelectSqlTemplete, fields, tb_name, whereSql, "");

            DbCommand selectCommand = PrepareCommand(conn, trans, sql, null, CommandType.Text);
            using (DbDataReader dr = selectCommand.ExecuteReader())
            {
                List<T> rsList = new List<T>();
                while (dr.Read())
                {
                    T obj = new T();
                    Mapping(dr, pcs, obj);
                    rsList.Add(obj);
                }
                return rsList;
            }
        }

        public override List<T> List<T>(DbConnection conn, DbTransaction trans, Criteria criteria)
        {

            #region 拼装参数

            SqlDataAdapter adapter = new SqlDataAdapter();
            List<string> param = new List<string>();

            if (criteria.WhereExpress != null)
            {
                MutilExpression mes = (MutilExpression)criteria.WhereExpress;

                if (mes.Expressions != null && mes.Expressions.Count > 0)
                {
                    for (int i = 0; i < mes.Expressions.Count; i++)
                    {
                        SingleExpression se = ((SingleExpression)mes.Expressions[i]);
                        if (se.Values != null && se.Values.Length > 0)
                        {
                            for (int j = 0; j < se.Values.Length; j++)
                            {
                                param.Add(se.Values[j].ToString());
                            }
                        }
                    }
                }
            }

            SqlParameter[] paramsValues = new SqlParameter[param.Count];
            for (int i = 0; i < param.Count; i++)
            {
                paramsValues[i] = new SqlParameter("@" + i, param[i]);
            }

            #endregion

            //获取实体对象和表映射关系的配置信息
            EntityConfig ec = EntitiesConfigContext[typeof(T)];
            //获取所有的属性
            PropertyConfig[] pcs = ec.PropertyConfigs;

            criteria.DatabaseContext = this;

            if ((criteria.Fields == null || criteria.Fields.Count() == 0) || (criteria.Fields != null && criteria.Fields.Count() == 1 && criteria.Fields[0].Equals("*")))
                criteria.Fields = pcs.Where(p => !p.UnMappingField).Select(p => p.PropertyInfo.Name).ToArray();
            if (string.IsNullOrEmpty(criteria.FromTables))
                criteria.FromTables = ec.TableName;

            List<T> rsList = new List<T>();

            //不翻页
            if (criteria.StartPage == -1)
            {
                //拼Sql
                string Sql = string.Format(SqlTemplate.SelectSqlTemplete, criteria.FieldsSql, criteria.FromTables, criteria.WhereSql, criteria.OrderSql, criteria.OrderInvertSql);
                DbCommand selectCommand = PrepareCommand(conn, trans, Sql, paramsValues, CommandType.Text);

                using (DbDataReader dr = selectCommand.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        T obj = new T();
                        Mapping(dr, pcs, obj, criteria.Fields);
                        rsList.Add(obj);
                    }
                }
            }//翻页支持
            else
            {
                string CountSql;
                if (string.IsNullOrEmpty(criteria.GroupBySql))
                {
                    CountSql = string.Format(SqlTemplate.SelectCountSqlTemplete, criteria.FromTables, criteria.WhereSql);
                }
                else
                {
                    string tmp_tb = string.Format(SqlTemplate.SelectSqlTemplete, criteria.GroupFiles, criteria.FromTables, criteria.WhereSql, criteria.GroupBySql);
                    CountSql = string.Format(SqlTemplate.SelectCountSqlTemplete, "(" + tmp_tb + ") as tmp_tb", "");
                }

                //获取总条数
                DbCommand CountCommand = PrepareCommand(conn, trans, CountSql, paramsValues, CommandType.Text);
                int TotalRow = (int)CountCommand.ExecuteScalar();
                CountCommand.Parameters.Clear();

                criteria.TotalRow = TotalRow;
                //计算总页数
                int TotalPage = (TotalRow / criteria.PageSize) + (TotalRow % criteria.PageSize == 0 ? 0 : 1);

                if (criteria.StartPage < 0)
                {
                    criteria.StartPage = 0;
                }

                if (criteria.StartPage > TotalPage - 1)
                {
                    criteria.StartPage = TotalPage - 1;
                }

                int PSize = criteria.PageSize;
                //如果为 最后一页
                if (criteria.StartPage == TotalPage - 1)
                {
                    // 则该页大小 “取余”
                    PSize = criteria.TotalRow - (criteria.PageSize * criteria.StartPage);
                }

                //string sql = string.Format(SelectByPageTemplete, PageSize, criteria.PageSize * criteria.StartPage + criteria.PageSize, criteria.FieldsSql, criteria.FromTables, criteria.WhereSql, criteria.OrderSql, criteria.OrderInvertSql);
                string sql = string.Format(SqlTemplate.SelectByPageTemplete, criteria.OrderSql, criteria.FieldsSql, criteria.FromTables, criteria.WhereSql, criteria.StartPage * criteria.PageSize, criteria.StartPage * criteria.PageSize + criteria.PageSize);

                //获取当前页数据
                DbCommand selectCommand = PrepareCommand(conn, trans, sql, paramsValues, CommandType.Text);

                using (DbDataReader dr = selectCommand.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        T obj = new T();
                        Mapping(dr, pcs, obj, criteria.Fields);
                        rsList.Add(obj);
                    }
                }
            }

            return rsList;
        }

        public override List<T> List<T>(DbConnection conn, DbTransaction trans, string whereSql, params object[] parameters)
        {
            //获取实体类和表的映射关系
            EntityConfig ec = EntitiesConfigContext[typeof(T)];
            //获取所有的属性
            PropertyConfig[] pcs = ec.PropertyConfigs;

            SqlParameter[] paramsValues = null;

            if (parameters != null && parameters.Length > 0)
            {
                paramsValues = new SqlParameter[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                {
                    paramsValues[i] = new SqlParameter("@" + i, parameters[i]);
                }
            }

            string fields = string.Join(",", pcs.Where(p => !p.UnMappingField).Select(p => p.PropertyInfo.Name).ToArray());

            //处理自定义表名
            string tb_name = string.Empty;

            //处理自定义表名
            if (whereSql.IndexOf('@') == 0)
            {
                int index = whereSql.IndexOf(' ');

                tb_name = whereSql.Substring(1, index);

                whereSql = whereSql.Substring(index, whereSql.Length - index);
            }

            //处理自定义表名
            tb_name = string.IsNullOrEmpty(tb_name) ? ec.TableName : tb_name;

            string sql = string.Format(SqlTemplate.SelectSqlTemplete, fields, tb_name, whereSql, "");

            DbCommand selectCommand = PrepareCommand(conn, trans, sql, paramsValues, CommandType.Text);
            using (DbDataReader dr = selectCommand.ExecuteReader())
            {
                List<T> rsList = new List<T>();
                while (dr.Read())
                {
                    T obj = new T();
                    Mapping(dr, pcs, obj);
                    rsList.Add(obj);
                }
                return rsList;
            }
        }


        /// <summary>
        /// 自定义字段查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="fields"></param>
        /// <param name="whereSql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override List<T> List<T>(DbConnection conn, DbTransaction trans, string[] fields, string whereSql, params object[] parameters)
        {
            //获取实体类和表的映射关系
            EntityConfig ec = EntitiesConfigContext[typeof(T)];
            //获取所有的属性
            PropertyConfig[] pcs = ec.PropertyConfigs;

            SqlParameter[] paramsValues = null;

            if (parameters != null && parameters.Length > 0)
            {
                paramsValues = new SqlParameter[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                {
                    paramsValues[i] = new SqlParameter("@" + i, parameters[i]);
                }
            }

            string fields_str = "*";
            if (fields != null && fields.Count() > 0)
            {
                fields_str = string.Join(",", fields);
            }
 
            //处理自定义表名
            string tb_name = string.Empty;

            //处理自定义表名
            if (whereSql.IndexOf('@') == 0)
            {
                int index = whereSql.IndexOf(' ');

                tb_name = whereSql.Substring(1, index);

                whereSql = whereSql.Substring(index, whereSql.Length - index);
            }

            //处理自定义表名
            tb_name = string.IsNullOrEmpty(tb_name) ? ec.TableName : tb_name;
            
            string sql = string.Format(SqlTemplate.SelectSqlTemplete, fields_str, tb_name, whereSql, "");

            DbCommand selectCommand = PrepareCommand(conn, trans, sql, paramsValues, CommandType.Text);
            using (DbDataReader dr = selectCommand.ExecuteReader())
            {
                List<T> rsList = new List<T>();
                while (dr.Read())
                {
                    T obj = new T();
                    Mapping(dr, pcs, obj);
                    rsList.Add(obj);
                }
                return rsList;
            }
        }


        public override DataTable Fill(DbConnection conn, DbTransaction trans, Criteria criteria)
        {
            criteria.DatabaseContext = this;

            DataTable dataTable = new DataTable();

            #region 拼装参数

            SqlDataAdapter adapter = new SqlDataAdapter();
            List<string> param = new List<string>();

            if (criteria.WhereExpress != null)
            {
                MutilExpression mes = (MutilExpression)criteria.WhereExpress;

                if (mes.Expressions != null && mes.Expressions.Count > 0)
                {
                    for (int i = 0; i < mes.Expressions.Count; i++)
                    {
                        SingleExpression se = ((SingleExpression)mes.Expressions[i]);
                        if (se.Values != null && se.Values.Length > 0)
                        {
                            for (int j = 0; j < se.Values.Length; j++)
                            {
                                param.Add(se.Values[j].ToString());
                            }
                        }
                    }
                }
            }

            SqlParameter[] paramsValues = new SqlParameter[param.Count];
            for (int i = 0; i < param.Count; i++)
            {
                paramsValues[i] = new SqlParameter("@" + i, param[i]);
            }

            #endregion

            //不翻页
            if (criteria.StartPage == -1)
            {
                string sql;
                if (string.IsNullOrEmpty(criteria.GroupBySql))
                {
                    sql = string.Format(SqlTemplate.SelectSqlTemplete, criteria.FieldsSql, criteria.FromTables, criteria.WhereSql, criteria.OrderSql);
                }
                else
                {
                    sql = string.Format(SqlTemplate.SelectSqlTemplete, criteria.FieldsSql, criteria.FromTables, criteria.WhereSql, criteria.GroupBySql + " " + criteria.OrderSql);
                }
                adapter.SelectCommand = (SqlCommand)PrepareCommand(conn, trans, sql, paramsValues, CommandType.Text);
                adapter.Fill(dataTable);
            }
            else
            {

                string CountSql;
                if (string.IsNullOrEmpty(criteria.GroupBySql))
                {
                    CountSql = string.Format(SqlTemplate.SelectCountSqlTemplete, criteria.FromTables, criteria.WhereSql);
                }
                else
                {
                    string tmp_tb = string.Format(SqlTemplate.SelectSqlTemplete, criteria.GroupFiles, criteria.FromTables, criteria.WhereSql, criteria.GroupBySql);
                    CountSql = string.Format(SqlTemplate.SelectCountSqlTemplete, "(" + tmp_tb + ") as tmp_tb", "");
                }

                //获取总条数
                DbCommand CountCommand = PrepareCommand(conn, trans, CountSql, paramsValues, CommandType.Text);
                int TotalRow = (int)CountCommand.ExecuteScalar();
                CountCommand.Parameters.Clear();

                criteria.TotalRow = TotalRow;

                //计算总页数
                int TotalPage = (TotalRow / criteria.PageSize) + (TotalRow % criteria.PageSize == 0 ? 0 : 1);

                //翻页控制
                if (criteria.StartPage < 0)
                {
                    criteria.StartPage = 0;
                }
                if (criteria.StartPage > TotalPage - 1)
                {
                    criteria.StartPage = TotalPage - 1;
                }

                int PageSize = criteria.PageSize;
                if (criteria.StartPage == 0 && TotalPage == 1 || criteria.StartPage == TotalPage - 1)
                {
                    PageSize = criteria.TotalRow - criteria.StartPage * criteria.PageSize;
                }

                string sql = string.Format(SqlTemplate.SelectByPageTemplete, criteria.OrderSql, criteria.FieldsSql, criteria.FromTables, criteria.WhereSql, criteria.StartPage * criteria.PageSize, criteria.StartPage * criteria.PageSize + criteria.PageSize);

                //获取当前翻页数据
                adapter.SelectCommand = (SqlCommand)PrepareCommand(conn, trans, sql, paramsValues, CommandType.Text);

                adapter.Fill(dataTable);
            }
            return dataTable;
        }

        public override DataTable Fill(DbConnection conn, DbTransaction trans, string Sql, params object[] parameters)
        {
            SqlParameter[] paramsValues = null;

            if (parameters != null && parameters.Length > 0)
            {
                paramsValues = new SqlParameter[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                {
                    paramsValues[i] = new SqlParameter("@" + i, parameters[i]);
                }
            }

            //实列化表
            DataTable dataTable = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter();

            adapter.SelectCommand = (SqlCommand)PrepareCommand(conn, trans, Sql, paramsValues, CommandType.Text);

            adapter.Fill(dataTable);

            return dataTable;
        }

        public override int ExcuteUpdate(DbConnection conn, DbTransaction trans, string sql, params object[] parameters)
        {
            SqlParameter[] paramsValues = null;

            if (parameters != null && parameters.Length > 0)
            {
                paramsValues = new SqlParameter[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    paramsValues[i] = new SqlParameter("@" + i, parameters[i]);
                }
            }

            SqlCommand sqlCommand = (SqlCommand)PrepareCommand(conn, trans, sql, paramsValues, CommandType.Text);
            return sqlCommand.ExecuteNonQuery();

        }

        public override object ExecuteScalar(DbConnection conn, DbTransaction trans, Criteria criteria)
        {
            criteria.DatabaseContext = this;

            string sql = string.Format(SqlTemplate.SelectSqlTemplete, criteria.FieldsSql, criteria.FromTables, criteria.WhereSql, criteria.OrderSql);

            SqlCommand sqlCommand = (SqlCommand)PrepareCommand(conn, trans, sql, null, CommandType.Text);

            return sqlCommand.ExecuteScalar();
        }


        public override object ExecuteScalar(DbConnection conn, DbTransaction trans, string sql, params object[] parameters)
        {
            SqlParameter[] paramsValues = null;

            if (parameters != null && parameters.Length > 0)
            {
                paramsValues = new SqlParameter[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    paramsValues[i] = new SqlParameter("@" + i, parameters[i]);
                }
            }

            SqlCommand sqlCommand = (SqlCommand)PrepareCommand(conn, trans, sql, paramsValues, CommandType.Text);
            return sqlCommand.ExecuteScalar();
        }

        /// <summary>
        /// 存取过程 返回DataSet
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="sql"></param>
        /// <param name="paraments"></param>
        /// <returns></returns>
        public override DataSet StoredToDataSet(DbConnection conn, DbTransaction trans, string sql, params object[] paraments)
        {
            SqlParameter[] paramsValues = null;

            if (paraments != null && paraments.Length > 0)
            {
                paramsValues = new SqlParameter[paraments.Length];

                for (int i = 0; i < paraments.Length; i++)
                {
                    paramsValues[i] = new SqlParameter("@" + i, paraments[i]);
                }
            }

            //实列化表
            DataSet dataSet = new DataSet();

            SqlDataAdapter adapter = new SqlDataAdapter();

            adapter.SelectCommand = (SqlCommand)PrepareCommand(conn, trans, sql, paramsValues, CommandType.Text);

            adapter.Fill(dataSet);

            return dataSet;
        }

        public override int Exist<T>(DbConnection conn, DbTransaction trans, string whereSql, params object[] parameters)
        {
            return Exist<T>(conn, trans, new string[] { }, whereSql, parameters);
        }

        public override int Exist<T>(DbConnection conn, DbTransaction trans, string[] selectfield, string whereSql, params object[] parameters)
        {
            //获取实体类和表的映射关系
            EntityConfig ec = EntitiesConfigContext[typeof(T)];
            //获取所有的属性
            PropertyConfig[] pcs = ec.PropertyConfigs;

            SqlParameter[] paramsValues = null;

            if (parameters != null && parameters.Length > 0)
            {
                paramsValues = new SqlParameter[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                {
                    paramsValues[i] = new SqlParameter("@" + i, parameters[i]);
                }
            }

            //默认查询主键字段
            string field = "*";
            if (selectfield.Length > 0)
            {
                field = string.Join(",", selectfield);
            }
            else
            {
                field = string.Join(",", pcs.Where(p => p.IsPrimaryKey).Select(p => p.PropertyInfo.Name).ToArray());
            }

            //处理自定义表名
            string tb_name = string.Empty;

            //处理自定义表名
            if (whereSql.IndexOf('@') == 0)
            {
                int index = whereSql.IndexOf(' ');

                tb_name = whereSql.Substring(1, index);

                whereSql = whereSql.Substring(index, whereSql.Length - index);
            }

            //处理自定义表名
            tb_name = string.IsNullOrEmpty(tb_name) ? ec.TableName : tb_name;

            //拼凑sql
            string sql = string.Format(SqlTemplate.ExistTemplete, field, tb_name, whereSql, "");

            //执行查询
            SqlCommand sqlCommand = (SqlCommand)PrepareCommand(conn, trans, sql, paramsValues, CommandType.Text);

            object count = sqlCommand.ExecuteScalar();

            return int.Parse(count.ToString());
        }


        private String AuthCondition(String condition)
        {
            if (String.IsNullOrEmpty(condition))
            {
                condition = String.Empty;
            }
            condition = condition.Trim();
            if (condition.Length >= 6)
            {
                if (condition.ToLower().Substring(0, 6) != "where ")
                {
                    condition = "WHERE " + condition;
                }
            }
            else if (!String.IsNullOrEmpty(condition))
            {
                condition = "WHERE " + condition;
            }
            if (!String.IsNullOrEmpty(condition))
            {
                condition = " " + condition;
            }
            return condition;
        }

        /// <summary>
        /// 拼凑表达式
        /// </summary>
        /// <param name="leftExp"></param>
        /// <param name="oper"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override string FormatExpress(string leftExp, LogicOper oper, object[] values, int index)
        {
            string ExpressStr = "";
            switch (oper)
            {
                case LogicOper.EQ:
                    ExpressStr = leftExp + " = @" + index;
                    break;
                case LogicOper.NEQ:
                    ExpressStr = leftExp + " != @" + index;
                    break;
                case LogicOper.GT:
                    ExpressStr = leftExp + " >@" + index;
                    break;
                case LogicOper.LT:
                    ExpressStr = leftExp + " <@" + index;
                    break;
                case LogicOper.LTEQ:
                    ExpressStr = leftExp + " <=@" + index;
                    break;
                case LogicOper.GTEQ:
                    ExpressStr = leftExp + " >=@" + index;
                    break;
                case LogicOper.IN:
                    ExpressStr = leftExp + " in (";
                    for (int i = 0; i < index - values.Length; i++)
                    {
                        if (i == 0)
                        {
                            ExpressStr += " @" + (index - values.Length + i);
                        }
                        else
                        {
                            ExpressStr += ",@" + (index - values.Length + i);
                        }
                    }
                    ExpressStr += ")";
                    break;
                case LogicOper.L_LIKE:
                    ExpressStr = leftExp + " like '%'+ @" + index;
                    break;
                case LogicOper.R_LIKE:
                    ExpressStr = leftExp + " like @" + index + " + '%'";
                    break;
                case LogicOper.LIKE:
                    ExpressStr = leftExp + " like '%' + @" + index + " + '%'";
                    break;
                case LogicOper.BETWEEN:
                    ExpressStr = leftExp + " between @" + (index - values.Length) + " and  @" + (index - values.Length + 1);
                    break;
                case LogicOper.CUSTOM:
                    ExpressStr = values[0].ToString();
                    break;
            }
            return ExpressStr;
        }


    }

}
