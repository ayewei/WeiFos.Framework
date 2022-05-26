using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.ORM.Data.Restrictions
{
    /// <summary>
    /// @author yewei 
    /// 表达式抽象类
    /// </summary>
    public abstract class Expression
    {

        /// <summary>
        /// 逻辑运算符
        /// </summary>
        public string LogicOper { get; set; }


        /// <summary>
        /// 获取Where表达
        /// </summary>
        /// <param name="context">数据库链接上下文</param>
        /// <param name="index">参数索引</param>
        /// <returns></returns>
        public abstract string FormatSql(DatabaseContext context, int index);

    }

}
