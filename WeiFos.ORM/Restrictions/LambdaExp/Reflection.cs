using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.ComponentModel;
using System.Data.SqlClient;


namespace WeiFos.ORM.Restrictions.LambdaExp
{
    /// <summary>
    /// SqlDataReader To Entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Reflection<T>
    {
        /// <summary>
        /// PropertyInfo[]
        /// </summary>
        private PropertyInfo[] fields;
        /// <summary>
        /// SqlDataReader To Entity
        /// </summary>
        private Reflection() { }
        /// <summary>
        /// 可空类型进行判断转换
        /// </summary>
        /// <param name="value">SqlDataReader</param>
        /// <param name="t">Entity PropertyType</param>
        /// <returns></returns>
        private object GetValue(object value, Type t)
        {
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }
                NullableConverter nc = new NullableConverter(t);
                t = nc.UnderlyingType;
            }
            return Convert.ChangeType(value, t);
        }
        /// <summary>
        /// 实体类赋值
        /// </summary>
        /// <param name="reader">SqlDataReader</param>
        /// <returns></returns>
        public T Build(SqlDataReader reader)
        {
            T entity = Activator.CreateInstance<T>();
            try
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if ((this.fields[i] != null) && !reader.IsDBNull(i))
                    {
                        PropertyInfo pi = entity.GetType().GetProperty(reader.GetName(i), BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                        if (pi != null)
                        {
                            object value = GetValue(reader[i], pi.PropertyType);
                            pi.SetValue(entity, value, null);
                        }
                    }
                }
            }
            catch
            { 
            }
            return entity;
        }
        /// <summary>
        /// 构造实体类
        /// </summary>
        /// <param name="reader">SqlDataReader</param>
        /// <returns></returns>
        public static Reflection<T> CreateBuilder(SqlDataReader reader)
        {
            Reflection<T> result = new Reflection<T>();
            result.fields = new PropertyInfo[reader.FieldCount];
            for (int i = 0; i < reader.FieldCount; i++)
            {
                result.fields[i] = typeof(T).GetProperty(reader.GetName(i));
            }
            return result;
        }
    }
}
