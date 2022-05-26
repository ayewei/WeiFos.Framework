using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.ORM.Data.Restrictions
{
    /// <summary>
    /// @author yewei 
    /// 复合表达式
    /// </summary>
    public class MutilExpression : Expression
    {
        private List<Expression> expressions = new List<Expression>();

        public List<Expression> Expressions
        {
            get { return expressions; }
            set { expressions = value; }
        }

        public MutilExpression Add(Expression expression)
        {
            expressions.Add(expression);
            return this;
        }

        public MutilExpression() :this("AND ")
        {

        }

        public MutilExpression(string logicOper)
        {
            this.LogicOper = logicOper;
        }

        /// <summary>
        /// 拼复合表达式Where 条件的Sql
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override string FormatSql(DatabaseContext context,int index)
        {
            StringBuilder wSql = new StringBuilder();

            int i = 0;
            foreach (Expression express in expressions)
            {
                bool is_add = true;
                SingleExpression e = (SingleExpression)express;

                if (e.Values.Length > 1)
                {
                    is_add = false;
                    i += e.Values.Length;
                }

                //获取子表达式 Sql
                string tmp = express.FormatSql(context, i);

                if (!string.IsNullOrEmpty(tmp))
                {
                    if (wSql.Length == 0)
                    {
                        wSql.Append(tmp);
                    }
                    else
                    {
                        wSql.Append(" ").Append(express.LogicOper).Append(tmp);
                    }
                }

                if (is_add) i++;
            }

            if (wSql.Length > 0)
            {
                wSql.Insert(0, "(");
                wSql.Append(")");
            }

            return wSql.ToString();
        }





        //public override string FormatSql(DatabaseContext context, int index)
        //{
        //    StringBuilder wSql = new StringBuilder();

        //    List<int> arr = new List<int>();
        //    int i = 0, j = 0;

        //    //处理or查询语句逻辑
        //    for (int ii = 0; ii < expressions.Count; ii++)
        //    {
        //        if (expressions[ii].LogicOper.ToLower().Contains("or"))
        //        {
        //            arr.Add(ii);
        //        }
        //    }

        //    foreach (Expression express in expressions)
        //    {
        //        bool is_add = true;
        //        SingleExpression e = (SingleExpression)express;

        //        if (e.Values.Length > 0)
        //        {
        //            if (e.Values.Length > 1)
        //            {
        //                is_add = false;
        //                i += e.Values.Length;
        //            }
        //        }

        //        //获取子表达式 Sql
        //        string tmp = express.FormatSql(context, i);
        //        if (!string.IsNullOrEmpty(tmp))
        //        {
        //            bool is_or_open = false;
        //            for (int jj = 0; jj < arr.Count; jj++)
        //            {
        //                if (j == arr[jj] - 1)
        //                {
        //                    is_or_open = true;
        //                }
        //            }

        //            if (wSql.Length == 0)
        //            {
        //                wSql.Append(tmp);
        //            }
        //            else
        //            {
        //                wSql.Append(" ").Append(express.LogicOper);
        //                if (is_or_open)
        //                {
        //                    wSql.Append(" ( ");
        //                }
        //                wSql.Append(tmp);
        //            }

        //            for (int jj = 0; jj < arr.Count; jj++)
        //            {
        //                if (j == arr[jj])
        //                {
        //                    wSql.Append(" ) ");
        //                }
        //            }

        //            if (is_add) i++;
        //            j++;
        //        }
        //    }

        //    if (wSql.Length > 0)
        //    {
        //        wSql.Insert(0, "(").Append(")");
        //    }

        //    return wSql.ToString();
        //}




    }
}
