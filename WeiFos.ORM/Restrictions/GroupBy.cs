using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.ORM.Restrictions
{
    /// <summary>
    /// @author yewei 
    /// 分组查询对象
    /// </summary>
    public class GroupBy
    {

        public string[] Fields
        {
            get;
            set;
        }

        public GroupBy(string[] Fields)
        {
            this.Fields = Fields;
        }

        public string FormatSql()
        {
            return string.Join(",", this.Fields);
        }

    }
}
