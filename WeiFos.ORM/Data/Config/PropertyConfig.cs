using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using WeiFos.ORM.Data.Const;

namespace WeiFos.ORM.Data.Config
{
    /// <summary>
    /// @author yewei 
    /// 类的属性和表的字段映射关系
    /// 映射属性的配置信息
    /// </summary>
    public class PropertyConfig
    {
        /// <summary>
        ///  用于反射 记录类的属性
        /// </summary>
        public PropertyInfo PropertyInfo { get; set; }

        /// <summary>
        /// 该建是否主键
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        ///  主键的生成方式
        /// </summary>
        public KeyGenerator Generator { get; set; }

        /// <summary>
        /// 标识该属性是否映射
        /// </summary>
        public bool UnMappingField { get; set; }

        /// <summary>
        /// 自定义列名
        /// </summary>
        public string FileName { get; set; }

    }

}
