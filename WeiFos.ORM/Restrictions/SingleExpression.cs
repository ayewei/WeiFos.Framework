using WeiFos.ORM.Data.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.ORM.Data.Restrictions
{
    /// <summary>
    /// @author yewei 
    /// 单一表达式
    /// </summary>
    public class SingleExpression : Expression
    {
        public string ExpLeft { get; set; }

        public LogicOper Operator { get; set; }

        public object[] Values { get; set; }

        /// <summary>
        /// 第一次缺省为and 
        /// </summary>
        /// <param name="expLeft"></param>
        /// <param name="oper"></param>
        /// <param name="value"></param>
        public SingleExpression(string expLeft, LogicOper oper, object value)
            : this(expLeft, oper, "and ", value)
        {

        }

        public SingleExpression(string expLeft, LogicOper oper, object[] values)
            : this(expLeft, oper, "and ", values)
        {

        }

        public SingleExpression(string expLeft, LogicOper oper, string logicOper, object value)
        {
            this.ExpLeft = expLeft;
            this.Operator = oper;
            this.LogicOper = logicOper;
            this.Values = new object[] { value };
        }

        public SingleExpression(string expLeft, LogicOper oper, string logicOper, object[] values)
        {
            this.ExpLeft = expLeft;
            this.Operator = oper;
            this.LogicOper = logicOper;
            this.Values = values;
        }


        /// <summary>
        /// 拼Where  Sql
        /// </summary>
        /// <param name="context"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public override string FormatSql(DatabaseContext context, int index)
        {
            return context.FormatExpress(this.ExpLeft, this.Operator, this.Values, index);
        }

    }
}
