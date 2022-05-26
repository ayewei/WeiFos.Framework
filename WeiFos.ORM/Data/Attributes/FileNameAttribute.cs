using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.ORM.Data.Attributes
{
    /// <summary>
    /// @author yewei by 2017-6-7 
    /// 非映射数据库属性
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Property, Inherited = false, AllowMultiple = true), Serializable]
    public class FileNameAttribute : Attribute
    {
        /// <summary>
        /// 自定义表名
        /// </summary>
        public string Name { get; set; }

    }
}
