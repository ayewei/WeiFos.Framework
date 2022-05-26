using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WeiFos.Core.EnumHelper
{
    /// <summary>
    /// 枚举 扩展
    /// @author yewei 
    /// @date 2015-02-11
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取字段说明信息
        /// </summary>
        public static string GetDescription(this System.Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            if (field != null)
                return EnumHelper.GetDescription(field);

            return string.Empty;
        }

        /// <summary>
        /// 获得枚举值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object GetValue(this System.Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            if (value != null)
                return field.GetValue(value);

            return string.Empty;
        }

        /// <summary>
        /// 获取键值对字典
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static Dictionary<System.Enum, string> GetDictionary(this System.Enum e)
        {
            var arr = System.Enum.GetValues(e.GetType());

            Dictionary<System.Enum, string> dic = new Dictionary<System.Enum, string>();

            foreach (var item in arr)
            {
                if (e.HasFlag((System.Enum)item))
                {
                    dic.Add((System.Enum)item, EnumHelper.GetDescriptionByValue(e.GetType(), item));
                }
            }

            return dic;
        }
    }
}
