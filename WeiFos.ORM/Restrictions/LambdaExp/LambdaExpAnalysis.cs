using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.ORM.Restrictions.LambdaExp
{
    /// <summary>
    /// 重写lamda 分析类 
    /// </summary>
    public static class LambdaExpAnalysis
    {

        #region Expression Analysis

        /// <summary>
        /// 
        /// </summary>
        private static bool NotEqual = false;

        /// <summary>
        /// Expression Analysis
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="IsMethodCall"></param>
        /// <returns></returns>
        public static String Analysis(this Expression exp, bool IsMethodCall = false)
        {
            var result = "";
            if (exp == null)
            {
                return result;
            }

            exp = PartialEvaluator.Eval(exp);
            
            switch (exp.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                    result = (exp.Type.IsValueType ? ((UnaryExpression)exp) : ((UnaryExpression)exp).Operand).Analysis(IsMethodCall);
                    break;
                case ExpressionType.Not:
                    NotEqual = true;
                    result = FuncUnary((UnaryExpression)exp, IsMethodCall);
                    NotEqual = false;
                    break;
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.ArrayIndex:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.ExclusiveOr:
                    result = ((BinaryExpression)exp).FuncBinary();
                    break;
                case ExpressionType.Constant:
                    //值
                    result = ((ConstantExpression)exp).FuncConstant();
                    break;
                case ExpressionType.MemberAccess:
                    result = FuncMemberAccess((MemberExpression)exp, IsMethodCall);
                    break;
                case ExpressionType.Call:
                    result = ((MethodCallExpression)exp).FuncMethodCall();
                    break;
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    result = ((NewArrayExpression)exp).FuncNewArray();
                    break;
                case ExpressionType.Lambda:
                    result = ((LambdaExpression)exp).Body.Analysis();
                    break;
                case ExpressionType.Invoke:
                    result = ((InvocationExpression)exp).Expression.Analysis();
                    break;
                    //case ExpressionType.TypeIs:
                    ////return this.VisitTypeIs((TypeBinaryExpression)exp);
                    //case ExpressionType.Conditional:
                    ////return this.VisitConditional((ConditionalExpression)exp);
                    //case ExpressionType.Parameter:
                    ////return this.VisitParameter((ParameterExpression)exp);
                    //case ExpressionType.New:
                    ////return this.VisitNew((NewExpression)exp);
                    //case ExpressionType.MemberInit:
                    ////return this.VisitMemberInit((MemberInitExpression)exp);
                    //case ExpressionType.ListInit:
                    ////return this.VisitListInit((ListInitExpression)exp);
                    //default:
                    //    sb.Append(String.Format("Unhandled expression type: '{0}'", exp.NodeType));
                    //    break;
                    //    throw new Exception(String.Format("Unhandled expression type: '{0}'", exp.NodeType));
            }
            return result;
        }
        #endregion


        #region Funs


        #region Unary Expression
        /// <summary>
        /// Analysis Unary Expression
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="IsMethodCall"></param>
        /// <returns></returns>
        private static String FuncUnary(UnaryExpression expression, bool IsMethodCall)
        {
            return Analysis(expression.Operand, IsMethodCall);
        }
        #endregion


        #region Binary Expression
        /// <summary>
        /// Analysis Binary Expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static String FuncBinary(this BinaryExpression expression)
        {
            if (expression == null)
            {
                return String.Empty;
            }
            var IsMethodCall = false;
            String opr = String.Empty;
            switch (expression.NodeType)
            {
                case ExpressionType.Equal:
                    opr = "=";
                    break;
                case ExpressionType.NotEqual:
                    opr = "<>";
                    break;
                case ExpressionType.GreaterThan:
                    opr = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    opr = ">=";
                    break;
                case ExpressionType.LessThan:
                    opr = "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    opr = "<=";
                    break;
                case ExpressionType.AndAlso:
                    opr = "AND";
                    IsMethodCall = true;
                    break;
                case ExpressionType.And:
                    opr = "AND";
                    IsMethodCall = true;
                    break;
                case ExpressionType.OrElse:
                    opr = "OR";
                    IsMethodCall = true;
                    break;
                case ExpressionType.Add:
                    opr = "+";
                    break;
                case ExpressionType.Subtract:
                    opr = "-";
                    break;
                case ExpressionType.Multiply:
                    opr = "*";
                    break;
                case ExpressionType.Divide:
                    opr = "/";
                    break;
                default:
                    throw new NotSupportedException(expression.NodeType + "is not supported.");
            }
            return String.Format("{0} {1} {2}", expression.Left.Analysis(IsMethodCall), opr, expression.Right.Analysis(IsMethodCall));
        }
        #endregion


        #region Constant Expression
        /// <summary>
        /// Analysis  Constant Expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static String FuncConstant(this ConstantExpression expression)
        {
            if (expression == null)
            {
                return String.Empty;
            }
            if (expression.Type.IsArray)
            {
                var elementType = expression.Type.GetElementType();
                return Expression.NewArrayInit(elementType, ((IEnumerable)expression.Value).OfType<object>().Select(v => (Expression)Expression.Constant(v, elementType))).FuncNewArray();
            }
            else if (expression.Type.IsGenericType && !expression.Type.IsValueType)
            {
                var items = ((IEnumerable)expression.Value).OfType<object>();
                var list = new List<String>();
                foreach (var o in items)
                {
                    list.Add(FormatValue(o));
                }
                return String.Format("({0})", String.Join(",", list));
            }
            return FormatValue(expression.Value);
        }
        #endregion


        #region NewArray Expression
        /// <summary>
        ///  Analysis NewArray Expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static String FuncNewArray(this NewArrayExpression expression)
        {
            if (expression.Expressions.Count == 0)
            {
                return "";
            }
            var result = new List<String>();
            foreach (var o in expression.Expressions)
            {
                result.Add(o.Analysis());
            }
            return String.Format("({0})", String.Join(",", result));
        }
        #endregion


        #region NewArray Expression
        /// <summary>
        /// Analysis NewArray Expression
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="IsMethodCall"></param>
        /// <returns></returns>
        private static String FuncMemberAccess(MemberExpression expression, bool IsMethodCall)
        {
            if (expression == null)
            {
                return "";
            }
            if (IsMethodCall)
            {
                return String.Format("{0} = {1}", expression.Member.Name, NotEqual ? 0 : 1);
            }
            var field = expression.Member as FieldInfo;
            if (expression.ToString().StartsWith("value("))
            {
                var result = Expression.Lambda(expression).Compile().DynamicInvoke();
                if (result == null)
                {
                    return "null";
                }
                if (result is System.Collections.IList)
                {
                    var fds = new List<String>();
                    foreach (var o in (IList)result)
                    {
                        fds.Add(o.FormatValue());
                    }
                    return String.Format("({0})", String.Join(",", fds));
                }
                return result.FormatValue();
            }
            else if (field != null)
            {
                return field.GetValue(expression.Member).FormatValue();
            }
            return String.Format("{0}", expression.Member.Name);
        }
        #endregion


        #region MethodCall Expression
        /// <summary>
        /// Analysis MethodCall Expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static String FuncMethodCall(this MethodCallExpression expression)
        {
            var value = String.Empty;
            var args = String.Empty;
            if (expression.Arguments.Count == 1)
            {
                var IsField = false;
                if (expression.ToString().StartsWith("value("))
                {
                    IsField = true;
                    value = expression.Arguments.FuncExpressionList();
                    args = expression.Object.Analysis();
                }
                else
                {
                    value = expression.Object.Analysis();
                    args = expression.Arguments.FuncExpressionList();
                }
                switch (expression.Method.Name)
                {
                    case "Contains":
                        return IsField ?
                             NotEqual ?
                             String.Format("({0} NOT IN {1})", value, args) : String.Format("({0} IN {1})", value, args)
                            :
                            NotEqual ? String.Format("({0} NOT LIKE '%{1}%')", value, args.Trim('\'')) : String.Format("({0} LIKE '%{1}%')", value, args.Trim('\''));
                    case "StartsWith":
                        return NotEqual ? String.Format("({0} NOT LIKE '{1}%')", value, args.Trim('\'')) : String.Format("({0} LIKE '{1}%')", value, args.Trim('\''));
                    case "EndsWith":
                        return NotEqual ? String.Format("({0} NOT LIKE '%{1}')", value, args.Trim('\'')) : String.Format("({0} LIKE '%{1}')", value, args.Trim('\''));
                    case "Equals":
                        return NotEqual ? String.Format("({0} <> {1})", value, args) : String.Format("({0} = {1})", value, args);
                }
            }
            else if (expression.Arguments.Count == 2)
            {
                #region x => new Int64[] { 1, 2, 3 }.Contains(x.id)
                var result = Expression.Lambda(expression.Arguments[0]).Compile().DynamicInvoke();
                if (result == null)
                {
                    return "";
                }
                var items = new List<String>();
                if (result is IList)
                {
                    foreach (var o in (IList)result)
                    {
                        items.Add(o.FormatValue());
                    }
                    args = String.Format("({0})", String.Join(",", items));
                }
                else if (result is IEnumerable)
                {
                    foreach (var o in (IEnumerable)result)
                    {
                        items.Add(o.FormatValue());
                    }
                    args = String.Format("({0})", String.Join(",", items));
                }
                value = expression.Arguments[1].Analysis();
                switch (expression.Method.Name)
                {
                    case "Contains":
                        return NotEqual ? String.Format("({0} NOT IN {1})", value, args) : String.Format("({0} IN {1})", value, args);
                }
                #endregion
            }
            return "";
        }
        #endregion


        #region ReadOnlyCollection(T)

        /// <summary>
        /// Analysis ReadOnlyCollection(T)
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        private static String FuncExpressionList(this ReadOnlyCollection<Expression> col)
        {
            var result = new List<string>();
            foreach (var o in col)
            {
                result.Add(o.Analysis());
            }
            return String.Join(",", result);
        }

        #endregion


        #region Nullable to UnderlyingType
        /// <summary>
        /// Nullable to UnderlyingType
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static Type ChangeNullable(this Type t)
        {
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                t = new NullableConverter(t).UnderlyingType;
            }
            return t;
        }
        #endregion


        #region 格式化值为数据库值,根据值类型自动加单引号
        /// <summary>
        /// 格式化值为数据库值,根据值类型自动加单引号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static String FormatValue(this object value)
        {
            if (value == null)
            {
                return "";
            }
            else
            {
                Type type = value.GetType();
                if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    type = type.GetGenericArguments()[0x0];
                }
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Boolean:
                        return !(Boolean)value ? "0" : "1";
                    case TypeCode.Char:
                    case TypeCode.DateTime:
                    case TypeCode.String:
                        return String.Format("'{0}'", value.ToString().EscapeSingleQuotes());
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        return value.ToString();
                    case TypeCode.Object:
                        return FormatValue((String)value);
                }
                return "";
            }
        }
        #endregion


        #region 替换单引号为2个单引号
        /// <summary>
        /// 替换单引号为2个单引号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static String EscapeSingleQuotes(this String value)
        {
            if (value.IndexOf('\'') < 0)
            {
                return value;
            }
            var builder = new StringBuilder();
            foreach (char ch in value)
            {
                if (ch == '\'')
                {
                    builder.Append("''");
                }
                else
                {
                    builder.Append(ch);
                }
            }
            return builder.ToString();
        }
        #endregion


        #endregion



    }
}
