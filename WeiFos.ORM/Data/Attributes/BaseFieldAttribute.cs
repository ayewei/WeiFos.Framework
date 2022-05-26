using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.ORM.Data.Attributes
{
    /// <summary>
    /// @author yewei 
    /// 自定义属性基类
    /// </summary>
    public abstract class BaseFieldAttribute : Attribute
    {
        public BaseFieldAttribute()
        {

        }

        //字段名
        public string FieldName { get; set; }
    }
}
