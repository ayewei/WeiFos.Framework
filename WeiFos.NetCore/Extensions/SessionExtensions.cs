using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeiFos.NetCore.Extensions
{
    /// <summary>
    /// 版 本 Weifos-Framework 微狐敏捷开发框架
    /// Copyright (c) 2013-2022 深圳微狐信息科技有限公司
    /// 创建人：叶委
    /// 日 期：2017.03.04
    /// 描 述：扩展 .net core session
    /// </summary>
    public static partial class SessionExtensions
    {

        /// <summary>
        /// 写入Session
        /// </summary>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set(this ISession session, string key, string str)
        {
            session.SetString(key, str);
        }


        /// <summary>
        /// 写入Session
        /// </summary>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set<T>(this ISession session, string key, T t)
        {
            session.SetString(key, JsonConvert.SerializeObject(t));
        }



        /// <summary>
        /// 获取Session
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }


    }
}
