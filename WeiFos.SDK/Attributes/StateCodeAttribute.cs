using System;
using System.Collections.Generic;
using System.Text;

namespace WeiFos.SDK.Attributes
{
    /// <summary>
    /// @author yewei 
    /// API状态码元数据
    /// @date 2022-04-30
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Field, Inherited = false, AllowMultiple = true), Serializable]
    public class StateCodeAttribute : Attribute
    {
        /// <summary>
        /// 接口描叙说明
        /// </summary>
        public string Describe { get; set; }

    }
}
