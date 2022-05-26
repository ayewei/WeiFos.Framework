using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.ORM.Data.SqlServerDBContext
{
    /// <summary>
    /// @author yewei 
    /// 脚步模板
    /// </summary>
    public class SqlTemplate
    {

        /// <summary>
        /// 插入数据模板 
        /// </summary>
        internal const string InsertSqlTemplete = "insert into {0} ({1}) values({2})";

        /// <summary>
        /// 获取自动增长ID
        /// </summary>
        internal const string GetIdSql = "select @@identity";

        /// <summary>
        ///  查询模板 
        ///  {0} 字段  {1} 表  {2} Where  {3} Order By 
        /// </summary>
        internal const string SelectSqlTemplete = "select {0} from {1} {2} {3} ";

        /// <summary>
        ///  总数模板 
        ///  {0} 表 {1} Where  {2} 条件 
        /// </summary>
        internal const string SelectCountSqlTemplete = "select count(*) from {0} {1} ";

        /// <summary>
        /// 获取顶端数据
        /// </summary>
        internal const string SelectTopByWhereSqlTemplete = "select top {0} {1} from {2} {3}";

        /// <summary>
        /// 翻页取数据Sql模板
        /// </summary>                    
        internal const string SelectByPageTemplete = "select * from (select ROW_NUMBER() OVER({0}) tmp_num,{1} from {2} {3}) tab where tmp_num>{4} AND tmp_num<={5}";

        /// <summary>
        /// 根据ID获取
        /// </summary>
        internal const string SelectByIDSqlTemplete = "select {0} from {1} where {2}";

        /// <summary>
        /// 修改数据
        /// </summary>
        internal const string UpdateSqlTemplete = "update {0} set {1} where {2}";

        /// <summary>
        /// 删除模板
        /// </summary>
        internal const string DeleteTemplete = "delete {0} where {1}";

        /// <summary>
        /// 数据是否存在模板
        /// </summary>
        internal const string ExistTemplete = "if exists (select {0} from {1} {2}) select '1' else select '0'";


    }
}
