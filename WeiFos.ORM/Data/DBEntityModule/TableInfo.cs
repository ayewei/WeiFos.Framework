using System;
using System.Collections.Generic;
using System.Text;

namespace WeiFos.ORM.Data.DBEntityModule
{

    /// <summary>
    /// 表信息类
    /// @author yewei
    /// @date 2014-02-14
    /// </summary>
    public class TableInfo
    {

        /// <summary>
        /// id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 所有者
        /// </summary>
        public string owner { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 对应实体命名空间
        /// </summary>
        public string name_space { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 字段集合
        /// </summary>
        public List<FieldDetail> fields { get; set; }

    }
}