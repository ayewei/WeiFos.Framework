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
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class UnMappedAttribute : Attribute
    {

    }
}
