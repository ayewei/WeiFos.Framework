using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.ORM.Data.Restrictions
{
    /// <summary>
    /// @author yewei 
    /// 按什么字段排序
    /// </summary>
    public class OrderBy
    {
        public string Field { get; set; }

        public string AscOrDesc { get; set; }

        public OrderBy(string field,string ascordesc)
        {
            this.Field = field;
            this.AscOrDesc = ascordesc;
        }

        /// <summary>
        /// 形成Sql
        /// </summary>
        /// <returns></returns>
        public string FormatSql()
        {
            return Field + " " + AscOrDesc;
        }

        /// <summary>
        /// 获取反序的Sql
        /// </summary>
        /// <returns></returns>
        public string FormatInvertSql()
        {
            return Field + " " + (AscOrDesc.ToLower() == "desc" ? "asc" : "desc");
        }

    }
}
