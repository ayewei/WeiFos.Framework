using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace WeiFos.Core.EnumHelper
{
    /// <summary>
    /// 枚举 帮助类
    /// </summary>
    public class EnumHelper
    {

        /// <summary>
        /// 获得枚举说明
        /// </summary>
        public static Dictionary<System.Enum, string> GetDescriptions(Type t)
        {
            Dictionary<System.Enum, string> dic = new Dictionary<System.Enum, string>();

            if (t.IsEnum == false) return dic;

            var values = System.Enum.GetValues(t);

            foreach (var item in values) dic.Add((System.Enum)item, ((System.Enum)item).GetDescription());

            return dic;
        }
 
        /// <summary>
        /// 获取枚举值 描叙集合
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetEnums(Type t)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();

            if (t.IsEnum == false) return dic;

            var values = System.Enum.GetValues(t);

            foreach (var item in values) dic.Add((int)item, ((System.Enum)item).GetDescription());

            return dic;
        }


        /// <summary>
        /// 通过枚举值获取说明
        /// </summary> 
        public static string GetDescriptionByValue(Type t, object value)
        {
            string result = string.Empty;

            if (value == null) return result;

            if (t.IsEnum)
            {
                string enumName = t.GetEnumName(value);

                if (!string.IsNullOrEmpty(enumName))
                {
                    FieldInfo field = t.GetField(enumName);

                    result = GetDescription(field);
                }
            }

            return result;
        }

        /// <summary>
        /// 根据枚举成员值，获取成员描述说明
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <param name="value">枚举成员值</param>
        /// <returns>描述string</returns>
        public static string GetEnumDescByValue(Type type, object value)
        {
            string result = string.Empty;

            if (value == null) return result;

            if (type.IsEnum)
            {
                foreach (FieldInfo fi in type.GetFields(BindingFlags.Static | BindingFlags.Public))
                {
                    if (((int)System.Enum.Parse(type, fi.Name, true)) == Convert.ToInt32(value))
                    {
                        return GetDescription(fi);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 根据枚举值获取指定枚举类型的枚举项
        /// </summary> 
        /// <param name="value">枚举值</param>
        /// <returns>返回值对应的枚举（没有则为null）</returns>
        public static T GetEnumByValue<T>(object value)
        {
            var values = System.Enum.GetValues(typeof(T));
            foreach (var item in values)
            {
                if (Convert.ToInt32(item) == (int)value)
                {
                    return (T)item;
                }
            }

            return default(T);
        }

        /// <summary>
        /// 获取字段说明信息
        /// </summary>
        public static string GetDescription(FieldInfo field)
        {
            string defaultAttribute = field.Name;

            object[] attributes = field.GetCustomAttributes(false);
            if (attributes.Length > 0)
            {
                for (int i = 0; i < attributes.Length; i++)
                {
                    EnumAttribute attr = attributes[i] as EnumAttribute;

                    if (string.IsNullOrEmpty(attr.Culture)) defaultAttribute = attr.Description;

                    if (string.Compare(attr.Culture, Thread.CurrentThread.CurrentUICulture.Name, true) == 0) return attr.Description;
                }
            }

            return defaultAttribute;
        }

        /// <summary>
        /// 获得枚举值
        /// </summary>
        public static T[] GetValues<T>(params T[] t) where T : struct
        {
            List<T> list = new List<T>();

            var values = System.Enum.GetValues(typeof(T));
            foreach (var item in values)
            {
                if (t.Contains((T)item)) continue;

                list.Add((T)item);
            }

            return list.ToArray();
        }

        /// <summary>
        /// 获得枚举值集合
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <returns></returns>
        public static List<int> GetValues<T>() where T : struct
        {
            Type type = typeof(T);

            if (!type.IsEnum) return null;

            var values = System.Enum.GetValues(type);

            List<int> list = new List<int>();

            foreach (var item in values)
            {
                list.Add((int)item);
            }

            return list;
        }
    }
}

