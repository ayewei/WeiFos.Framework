using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Const;

namespace WeiFos.ORM.Data.Attributes
{                  
    /// <summary>
    /// @author yewei 
    /// 主键生成策略
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Property, Inherited=false,AllowMultiple=true),Serializable]
    public class IDAttribute : BaseFieldAttribute
    {
        /// <summary>
        /// 记录ID的生成策略
        /// </summary>
        public KeyGenerator Generator { get; set; }
 
    }

}
