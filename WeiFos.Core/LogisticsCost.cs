using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.Core
{
    public class LogisticsCost
    {
        /// <summary>
        /// 淘布斯 收件收费计算
        /// </summary>
        /// <param name="type">计算方式 1-,2-</param>
        /// <param name="cost">每单位收费</param>
        /// <param name="proportion">重量上浮比例 如：4表示重量上浮4%；</param>
        /// <param name="unit">多少克为一个单位</param>
        /// <returns></returns>
        public static decimal GetCostByType(int type, decimal cost, decimal proportion, int unit, decimal weight)
        {
            try
            {
                //不足1千克按1千克*每千克多少钱；超过1千克，重量加上浮百分比*每千克多少钱;如：每千克8元，重量上浮4%；固1千克以下收8元；1千克以上重量*(1+4%)*8；
                if (type == 0)
                {
                    if (weight < 1)
                    {
                        return cost;
                    }
                    else
                    {
                        return weight * (1 + proportion / 100) * cost;
                    }
                }//每多少克收多少钱;如：¥20/500克；
                else if (type == 1)
                {
                    int newWeight = (int)(weight * 1000);
                    return (newWeight % unit == 0 ? newWeight / unit : newWeight / unit + 1) * cost;
                }
                else if (type == 2)
                {
                    int newWeight = (int)(weight * 1000);
                    return (newWeight % unit == 0 ? newWeight / unit : newWeight / unit + 1) * cost;
                }

                //var sz = (int)500;var xz = (int)10;
                //if (weight * 1000 <= sz)
                //{
                //    //首重费用；
                //}
                //else
                //{
                //    var we = (int)(weight * 1000);
                //    var c = (we - sz) % xz == 0 ? (we - sz) / xz : (we - sz) / xz + 1;
                //    //首重费用+续重费用(c*续重单位费用)；
                //}
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 规格数量处理
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int StrToInt(string str)
        {
            try
            {
                string s1 = "0123456789";
                string result = "";
                for (int i = 0; i < str.Length; i++)
                {
                    if (s1.Contains(str.Substring(i, 1)))
                    {
                        result += str.Substring(i, 1);
                    }
                    else
                    {
                        break;
                    }
                }
                return int.Parse(result);
            }
            catch
            {
                return 0;
            }
        }
    }
}
