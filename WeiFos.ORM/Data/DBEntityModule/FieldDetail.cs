using System;
using System.Collections.Generic;
using System.Text;

namespace WeiFos.ORM.Data.DBEntityModule
{
    /// <summary>
    /// 字段信息类
    /// @author yewei
    /// @date 2014-02-14
    /// </summary>
    public class FieldDetail
    {

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int order_index { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 是否自动增长
        /// </summary>
        public bool is_identity { get; set; }

        /// <summary>
        /// 是否是主键
        /// </summary>
        public bool is_primary { get; set; }

        /// <summary>
        /// 是否可以为空
        /// </summary>
        public bool is_nullable { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string typename { get; set; }

        /// <summary>
        /// 精度
        /// </summary>
        public int length { get; set; }

        /// <summary>
        /// 备注说明
        /// </summary>
        public string remark { get; set; }

    }
}