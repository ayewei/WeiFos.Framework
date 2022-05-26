using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.Core
{
    /// <summary>
    /// 扩展方法类
    /// </summary>
    public static class ExtendHelper
    {
        #region 转字符串

        /// <summary>
        /// 转字符串，如果传入的是NULL，返回string.Empty(空字符串)
        /// </summary>
        /// <param name="objValue">要转成String的值</param>
        /// <returns>String的值</returns>
        public static string ToStringHasNull(this object objValue)
        {
            return ToStringHasNull(objValue, string.Empty);
        }

        /// <summary>
        /// 转字符串，如果传入的是NULL，返回设置的默认的字符串
        /// </summary>
        /// <param name="objValue">要转成String的值</param>
        /// <param name="defaultValue">默认的字符串</param>
        /// <returns>String的值</returns>
        public static string ToStringHasNull(this object objValue, string defaultValue)
        {
            if (objValue != null)
            {
                return objValue.ToString();
            }

            return defaultValue;
        }

        #endregion

        public static decimal CutDecimalWithN(this decimal d, int n)
        {
            string strDecimal = d.ToString();
            int index = strDecimal.IndexOf(".");
            if (index == -1 || strDecimal.Length < index + n + 1)
            {
                strDecimal = string.Format("{0:F" + n + "}", d);
            }
            else
            {
                int length = index;
                if (n != 0)
                {
                    length = index + n + 1;
                }
                strDecimal = strDecimal.Substring(0, length);
            }
            return Decimal.Parse(strDecimal);
        }

        /// <summary>
        /// 获取Decimal类型后面n位数(进行四舍五入)
        /// </summary>
        /// <param name="d"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static decimal DecimalWithN(this decimal d, int n)
        {
            //string str1 = d4.ToString("f1");
            return decimal.Round(d, n, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 隐藏字符
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string HideChar(this string name)
        {
            int nameStringLength = name.Length;
            if (nameStringLength >= 11)
            {
                return name.Replace(name.Substring(3, nameStringLength - 7), "****");
            }
            else if (nameStringLength >= 3)
            {
                return string.Format("{0}{1}", name.Substring(0, 3), "****");
            }
            else
            {
                return name + "****";
            }
        }

        /// <summary>
        /// 按指定长度(单字节)截取字符串
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="len">截取字节数</param>
        /// <returns>string</returns>
        public static string SubstringByByte(this string str, int startIndex, int len)
        {
            if (str == null || str.Trim() == "")
            {
                return "";
            }
            if (Encoding.Default.GetByteCount(str) < startIndex + 1 + len)
            {
                return str;
            }
            int i = 0;//字节数
            int j = 0;//实际截取长度
            foreach (char newChar in str)
            {
                if ((int)newChar > 127)
                {
                    //汉字
                    i += 2;
                }
                else
                {
                    i++;
                }
                if (i > startIndex + len)
                {
                    str = str.Substring(startIndex, j);
                    break;
                }
                if (i > startIndex)
                {
                    j++;
                }
            }
            return str;
        }


    }
}
