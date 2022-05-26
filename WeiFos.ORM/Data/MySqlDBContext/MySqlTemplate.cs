using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.ORM.Data.MySqlDBContext
{
    /// <summary>
    /// @author yewei 
    /// MySql 脚本模板
    /// </summary>
    public class MySqlTemplate
    {

        /// <summary>
        /// 插入数据脚本
        /// </summary>
        internal const  string InsertSqlTemplete = "insert into {0} ({1}) values({2})";

        /// <summary>
        /// 获取自动增长ID
        /// </summary>
        internal const  string GetIdSql = "select @@identity";

        /// <summary>
        ///  查询模板  {0} 字段  {1} 表  {2} Where  {3} Order By 
        /// </summary>
        internal const  string SelectSqlTemplete = "select {0} from {1} {2} {3} ";

        /// <summary>
        ///  总数模板 {0} 表   {1} Where  {2} 条件 
        /// </summary>
        internal const  string SelectCountSqlTemplete = "select count(*) from {0} {1} ";

        /// <summary>
        /// 获取顶端数据
        /// </summary>
        internal const string SelectTopByWhereSqlTemplete = "select top {0} {1} from {2} {3}";

        /// <summary>
        /// 翻页取数据MySql模板    
        ///  select * from tb limit 0 
        ///  select * from (select top PageSize * from (select top PageSize*StartPage 字段 from 表 where 条件 order by[asc、desc]) t2 )
        /// </summary>   
        internal const  string SelectByPageTemplete = "select * from (select * from (select {2} from {3} {4} {5}) t2 {6} limit {1} )t1 {5} limit {0}";

        /// <summary>
        /// 根据ID获取
        /// </summary>
        internal const  string SelectByIDSqlTemplete = "select {0} from {1} where {2}";

        /// <summary>
        /// 修改
        /// </summary>
        internal const  string UpdateSqlTemplete = "update {0} set {1} where {2}";

        /// <summary>
        /// 删除数据脚本 
        /// </summary>
        internal const  string DeleteTemplete = "delete from {0} where {1}";

        /// <summary>
        /// 数据是否存在
        /// </summary>
        internal const string ExistTemplete = "if exists (select {0} from {1} {2}) select '1' else select '0'";


    }
}
