using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Cache.Config;

namespace WeiFos.Cache.Data
{
    /// <summary>
    /// Copyright (c) 2013-2022 深圳微狐信息科技有限公司
    /// 描 述：缓存访问层上下文
    /// 创建人：叶委
    /// 日 期：2018.07.07
    /// </summary>
    public abstract class CacheContext
    {

        /// <summary>
        /// 获取服务器连接
        /// </summary>
        /// <returns></returns>
        public abstract Multiplexer GetMultiplexer();

        /// <summary>
        /// 是否存在指定key
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <returns></returns>
        public abstract bool Exists(Multiplexer multiplexer, string cacheKey, int dbId = 0);

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <returns></returns>
        public abstract T Read<T>(Multiplexer multiplexer, string cacheKey, int dbId = 0) where T : class;

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="value">对象数据</param>
        /// <param name="cacheKey">键</param>
        public abstract void Write<T>(Multiplexer multiplexer, string cacheKey, T value, int dbId = 0) where T : class;

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="value">对象数据</param>
        /// <param name="cacheKey">键</param>
        /// <param name="expireTime">到期时间</param>
        public abstract void Write<T>(Multiplexer multiplexer, string cacheKey, T value, DateTime expireTime, int dbId = 0) where T : class;

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="value">对象数据</param>
        /// <param name="cacheKey">键</param>
        /// <param name="expireTime">到期时间</param>
        public abstract void Write<T>(Multiplexer multiplexer, string cacheKey, T value, TimeSpan timeSpan, int dbId = 0) where T : class;

        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        public abstract bool Remove(Multiplexer multiplexer, string cacheKey, int dbId = 0);

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public abstract void RemoveAll(Multiplexer multiplexer, int dbId = 0);


    }
}
