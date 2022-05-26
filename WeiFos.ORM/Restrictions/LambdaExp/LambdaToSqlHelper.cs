using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace WeiFos.ORM.Restrictions.LambdaExp
{
    public static class LambdaToSqlHelper
    {

        #region 基础方法


        #region 获取条件语句方法

        private static string GetWhereSql<T>(Expression<Func<T, bool>> func, List<ParMODEL> parModelList) where T : class
        {
            string res = "";
            if (func.Body is BinaryExpression)
            {
                //起始参数
                BinaryExpression be = ((BinaryExpression)func.Body);
                res = BinarExpressionProvider(be.Left, be.Right, be.NodeType, parModelList);
            }
            else if (func.Body is MethodCallExpression)
            {
                MethodCallExpression be = ((MethodCallExpression)func.Body);
                res = ExpressionRouter(func.Body, parModelList);
            }
            else
            {
                res = "  ";
            }

            return res;
        }

        #endregion 获取条件语句方法


        #region 获取排序语句 order by

        private static string GetOrderSql<T>(Expression<Func<T, object>> exp) where T : class
        {
            var res = "";
            if (exp.Body is UnaryExpression)
            {
                UnaryExpression ue = ((UnaryExpression)exp.Body);
                List<ParMODEL> parModelList = new List<ParMODEL>();
                res = "order by `" + ExpressionRouter(ue.Operand, parModelList).ToLower() + "`";
            }
            else
            {
                MemberExpression order = ((MemberExpression)exp.Body);
                res = "order by `" + order.Member.Name.ToLower() + "`";
            }
            return res;
        }

        #endregion 获取排序语句 order by


        #endregion 基础方法


        #region 底层

        public static bool In<T>(this T obj, T[] array)
        {
            return true;
        }

        public static bool NotIn<T>(this T obj, T[] array)
        {
            return true;
        }

        public static bool Like(this string str, string likeStr)
        {
            return true;
        }

        public static bool NotLike(this string str, string likeStr)
        {
            return true;
        }

        private static string GetValueStringByType(object oj)
        {
            if (oj == null)
            {
                return "null";
            }
            else if (oj is ValueType)
            {
                return oj.ToString();
            }
            else if (oj is string || oj is DateTime || oj is char)
            {
                return string.Format("'{0}'", oj.ToString());
            }
            else
            {
                return string.Format("'{0}'", oj.ToString());
            }
        }

        private static string BinarExpressionProvider(Expression left, Expression right, ExpressionType type, List<ParMODEL> parModelList)
        {
            string sb = "(";
            //先处理左边
            string reLeftStr = ExpressionRouter(left, parModelList);
            sb += reLeftStr;

            sb += ExpressionTypeCast(type);

            //再处理右边
            string tmpStr = ExpressionRouter(right, parModelList);
            if (tmpStr == "null")
            {
                if (sb.EndsWith(" ="))
                {
                    sb = sb.Substring(0, sb.Length - 2) + " is null";
                }
                else if (sb.EndsWith("<>"))
                {
                    sb = sb.Substring(0, sb.Length - 2) + " is not null";
                }
            }
            else
            {
                //添加参数
                sb += tmpStr;
            }

            return sb += ")";
        }

        private static string ExpressionRouter(Expression exp, List<ParMODEL> parModelList)
        {
            string sb = string.Empty;

            if (exp is BinaryExpression)
            {
                BinaryExpression be = ((BinaryExpression)exp);
                return BinarExpressionProvider(be.Left, be.Right, be.NodeType, parModelList);
            }
            else if (exp is MemberExpression)
            {
                MemberExpression me = ((MemberExpression)exp);
                if (!exp.ToString().StartsWith("value"))
                {
                    return me.Member.Name;
                }
                else
                {
                    var result = Expression.Lambda(exp).Compile().DynamicInvoke();
                    if (result == null)
                    {
                        return "null";
                    }
                    else if (result is ValueType)
                    {
                        ParMODEL p = new ParMODEL();
                        p.name = "par" + (parModelList.Count + 1);
                        p.value = result.ToString();
                        parModelList.Add(p);
                        //return ce.Value.ToString();
                        return "@par" + parModelList.Count;
                    }
                    else if (result is string || result is DateTime || result is char)
                    {
                        ParMODEL p = new ParMODEL();
                        p.name = "par" + (parModelList.Count + 1);
                        p.value = result.ToString();
                        parModelList.Add(p);
                        //return string.Format("'{0}'", ce.Value.ToString());
                        return "@par" + parModelList.Count;
                    }
                    else if (result is int[])
                    {
                        var rl = result as int[];
                        StringBuilder sbIntStr = new StringBuilder();
                        foreach (var r in rl)
                        {
                            ParMODEL p = new ParMODEL();
                            p.name = "par" + (parModelList.Count + 1);
                            p.value = r.ToString();
                            parModelList.Add(p);
                            //return string.Format("'{0}'", ce.Value.ToString());
                            sbIntStr.Append("@par" + parModelList.Count + ",");
                        }
                        return sbIntStr.ToString().Substring(0, sbIntStr.ToString().Length - 1);
                    }
                    else if (result is string[])
                    {
                        var rl = result as string[];
                        StringBuilder sbIntStr = new StringBuilder();
                        foreach (var r in rl)
                        {
                            ParMODEL p = new ParMODEL();
                            p.name = "par" + (parModelList.Count + 1);
                            p.value = r.ToString();
                            parModelList.Add(p);
                            //return string.Format("'{0}'", ce.Value.ToString());
                            sbIntStr.Append("@par" + parModelList.Count + ",");
                        }
                        return sbIntStr.ToString().Substring(0, sbIntStr.ToString().Length - 1);
                    }
                }
            }
            else if (exp is NewArrayExpression)
            {
                NewArrayExpression ae = ((NewArrayExpression)exp);
                StringBuilder tmpstr = new StringBuilder();
                foreach (Expression ex in ae.Expressions)
                {
                    tmpstr.Append(ExpressionRouter(ex, parModelList));
                    tmpstr.Append(",");
                }
                //添加参数

                return tmpstr.ToString(0, tmpstr.Length - 1);
            }
            else if (exp is MethodCallExpression)
            {
                MethodCallExpression mce = (MethodCallExpression)exp;
                string par = ExpressionRouter(mce.Arguments[0], parModelList);
                if (mce.Method.Name == "Like")
                {
                    //添加参数用
                    return string.Format("({0} like {1})", par, ExpressionRouter(mce.Arguments[1], parModelList));
                }
                else if (mce.Method.Name == "NotLike")
                {
                    //添加参数用
                    return string.Format("({0} Not like {1})", par, ExpressionRouter(mce.Arguments[1], parModelList));
                }
                else if (mce.Method.Name == "In")
                {
                    //添加参数用
                    return string.Format("{0} In ({1})", par, ExpressionRouter(mce.Arguments[1], parModelList));
                }
                else if (mce.Method.Name == "NotIn")
                {
                    //添加参数用
                    return string.Format("{0} Not In ({1})", par, ExpressionRouter(mce.Arguments[1], parModelList));
                }
            }
            else if (exp is ConstantExpression)
            {
                ConstantExpression ce = ((ConstantExpression)exp);
                if (ce.Value == null)
                {
                    return "null";
                }
                else if (ce.Value is ValueType)
                {
                    ParMODEL p = new ParMODEL();
                    p.name = "par" + (parModelList.Count + 1);
                    p.value = ce.Value.ToString();
                    parModelList.Add(p);
                    //return ce.Value.ToString();
                    return "@par" + parModelList.Count;
                }
                else if (ce.Value is string || ce.Value is DateTime || ce.Value is char)
                {
                    ParMODEL p = new ParMODEL();
                    p.name = "par" + (parModelList.Count + 1);
                    p.value = ce.Value.ToString();
                    parModelList.Add(p);
                    //return string.Format("'{0}'", ce.Value.ToString());
                    return "@par" + parModelList.Count;
                }

                //对数值进行参数附加
            }
            else if (exp is UnaryExpression)
            {
                UnaryExpression ue = ((UnaryExpression)exp);

                return ExpressionRouter(ue.Operand, parModelList);
            }
            return null;
        }

        private static string ExpressionTypeCast(ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    return " AND ";

                case ExpressionType.Equal:
                    return " =";

                case ExpressionType.GreaterThan:
                    return " >";

                case ExpressionType.GreaterThanOrEqual:
                    return ">=";

                case ExpressionType.LessThan:
                    return "<";

                case ExpressionType.LessThanOrEqual:
                    return "<=";

                case ExpressionType.NotEqual:
                    return "<>";

                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return " Or ";

                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    return "+";

                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return "-";

                case ExpressionType.Divide:
                    return "/";

                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return "*";

                default:
                    return null;
            }
        }

        #endregion 底层

    }
}


public class SqlParMODEL
{
    public string sql { set; get; }

    private List<ParMODEL> parList { set; get; }
}

public class ParMODEL
{
    public string name { set; get; }

    public object value { set; get; }
}
