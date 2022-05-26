using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.ORM.Data.Const
{
    /// <summary>
    /// @author yewei 
    /// 逻辑运算符
    /// </summary>
    public enum LogicOper
    {
        /// <summary>
        /// 等于
        /// </summary>
        EQ, 

        /// <summary>
        /// 不等于
        /// </summary>
        NEQ, 

        /// <summary>
        /// 大于
        /// </summary>
        GT,

        /// <summary>
        /// 大于等于
        /// </summary>
        GTEQ,

        /// <summary>
        /// 小于
        /// </summary>
        LT,

        /// <summary>
        /// 小于等于
        /// </summary>
        LTEQ,

        /// <summary>
        /// 里面
        /// </summary>
        IN,

        /// <summary>
        /// 左模糊
        /// </summary>
        L_LIKE,

        /// <summary>
        /// 右模糊
        /// </summary>
        R_LIKE,

        /// <summary>
        /// 模糊
        /// </summary>
        LIKE,

        /// <summary>
        /// 在指定范围
        /// </summary>
        BETWEEN,

        /// <summary>
        /// 自定义条件 
        /// </summary>
        CUSTOM

        //OR
    }
}
