using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.ORM.Data.Attributes
{
    /// <summary>
    /// @author yewei 
    /// 表或视图属性
    /// 用来记录表名
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Class,Inherited=false, AllowMultiple=false),Serializable]
    public class TableAttribute :Attribute
    {
        /// <summary>
        /// 实体对应表或视图名称
        /// </summary>
        public string Name { get; set; }

    }


}
