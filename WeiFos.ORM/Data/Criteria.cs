using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Restrictions;
using WeiFos.ORM.Restrictions;

namespace WeiFos.ORM.Data
{
    /// <summary>
    /// @author yewei 
    /// 管理Sql查询
    /// </summary>
    public class Criteria
    {
        //引用DatabaseContext
        public DatabaseContext DatabaseContext { get; set; }

        //开始页
        private int startPage = -1;

        //每页大小
        public int StartPage
        {
            get { return startPage; }
            set { this.startPage = value; }
        }

        //总条数
        public int TotalRow { get; set; }

        private int pageSize = 10;

        public int PageSize
        {
            get { return pageSize; }
        }

        /// <summary>
        /// 查询的字段
        /// </summary>
        private string[] fields = null;

        public string[] Fields
        {
            get { return fields; }
            set { this.fields = value; }
        }

        /// <summary>
        /// 查询的表或视图
        /// </summary>
        private string fromTables = null;
        public string FromTables
        {
            get { return fromTables; }
            set { fromTables = value; }
        }

        /// <summary>
        /// 自定义查询条件
        /// </summary>
        private string customWhereSql = null;
        public string CustomWhereSql
        {
            get { return customWhereSql; }
            set { customWhereSql = value; }
        }


        private Expression whereExpress;

        public Expression WhereExpress
        {
            get { return whereExpress; }
        }

        private List<OrderBy> orderBys = null;
        public List<OrderBy> OrderBys
        {
            get { return orderBys; }
        }

        private GroupBy groupBy = null;
        public GroupBy GroupBy
        {
            get { return groupBy; }
        }


        public string FieldsSql
        {
            get { return string.Join(",", fields); }
        }

        /// <summary>
        /// 获取Where条件拼凑的Sql
        /// </summary>
        public string WhereSql
        {
            get { return whereExpress == null ? "" : "where " + whereExpress.FormatSql(this.DatabaseContext, 0); }
        }

        /// <summary>
        /// 获取Order By拼凑的Sql 
        /// </summary>
        public string OrderSql
        {
            get { return orderBys == null ? "" : "order by " + string.Join(",", orderBys.Select(o => o.FormatSql()).ToArray()); }
        }

        /// <summary>
        /// 获取Order By 反向 拼凑的Sql 
        /// </summary>
        public string OrderInvertSql
        {
            get { return orderBys == null ? "" : "order by " + string.Join(",", orderBys.Select(o => o.FormatInvertSql()).ToArray()); }
        }

        /// <summary>
        /// 获取分组拼凑的Sql 
        /// </summary>
        public string GroupBySql
        {
            get { return groupBy == null ? "" : " group by " + groupBy.FormatSql(); }
        }

        public string GroupFiles
        {
            get { return groupBy == null ? "" : groupBy.FormatSql(); }
        }

        public Criteria SetWhereExpression(Expression whereExpress)
        {
            this.whereExpress = whereExpress;
            return this;
        }

        public Criteria AddOrderBy(OrderBy order_by)
        {
            if (orderBys == null)
            {
                orderBys = new List<OrderBy>();
            }
            orderBys.Add(order_by);
            return this;
        }

        /// <summary>
        /// 设置分组条件
        /// </summary>
        /// <param name="group_by"></param>
        /// <returns></returns>
        public Criteria SetGroupBy(GroupBy group_by)
        {
            groupBy = group_by;
            return this;
        }

        /// <summary>
        /// 查询字段
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public Criteria SetFields(string[] fields)
        {
            this.fields = fields;
            return this;
        }

        /// <summary>
        /// 开始页
        /// </summary>
        /// <param name="startRow"></param>
        /// <returns></returns>
        public Criteria SetStartPage(int startPage)
        {
            this.startPage = startPage;
            return this;
        }

        /// <summary>
        /// 每页大小
        /// </summary>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public Criteria SetPageSize(int pagesize)
        {
            this.pageSize = pagesize;
            return this;
        }

        /// <summary>
        /// 表或视图
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public Criteria SetFromTables(string table)
        {
            this.fromTables = table;
            return this;
        }


    }
}
