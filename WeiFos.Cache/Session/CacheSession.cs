using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Cache.Config;
using WeiFos.Cache.Data;

namespace WeiFos.Cache.Session
{
    /// <summary>
    /// Copyright (c) 2013-2022 深圳微狐信息科技有限公司
    /// 描 述：缓存会话的实现
    /// 创建人：叶委
    /// 日 期：2018.07.07
    /// </summary>
    public class CacheSession : ICacheSession
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="databaseContext"></param>
        public CacheSession(CacheContext context)
        {
            this.cacheContext = context;
            //根据不同缓存创建不同连接器
            this.multiplexer = this.cacheContext.GetMultiplexer();
        }


        /// <summary>
        /// 是否存在key
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public bool Exists(string cacheKey, int dbId = 0)
        {
            return cacheContext.Exists(multiplexer, cacheKey, dbId);
        }


        /// <summary>
        /// 缓存上下文
        /// </summary>
        private CacheContext cacheContext = null;


        /// <summary>
        /// 连接器
        /// </summary>
        private Multiplexer multiplexer = null;
        public Multiplexer Multiplexer
        {
            get
            {
                return multiplexer;
            }
            set
            {
                multiplexer = value;
            }
        }


        /// <summary>
        /// 读
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public T Read<T>(string cacheKey, int dbId = 0) where T : class
        {
            return cacheContext.Read<T>(multiplexer, cacheKey, dbId);
        }


        /// <summary>
        /// 删除（根据模块ID）
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="dbId"></param>
        public void Remove(string cacheKey, int dbId = 0)
        {
            cacheContext.Remove(multiplexer, cacheKey, dbId);
        }


        /// <summary>
        /// 删除所有
        /// </summary>
        /// <param name="dbId"></param>
        public void RemoveAll(int dbId = 0)
        {
            cacheContext.RemoveAll(multiplexer, dbId);
        }


        /// <summary>
        /// 缓存写入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="dbId"></param>
        public void Write<T>(string cacheKey, T value, int dbId = 0) where T : class
        {
            cacheContext.Write<T>(multiplexer, cacheKey, value, dbId);
        }


        /// <summary>
        /// 缓存写入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="expireTime"></param>
        /// <param name="dbId"></param>
        public void Write<T>(string cacheKey, T value, DateTime expireTime, int dbId = 0) where T : class
        {
            cacheContext.Write<T>(multiplexer, cacheKey, value, expireTime, dbId);
        }


        /// <summary>
        /// 缓存写入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan"></param>
        /// <param name="dbId"></param>
        public void Write<T>(string cacheKey, T value, TimeSpan timeSpan, int dbId = 0) where T : class
        {
            cacheContext.Write<T>(multiplexer, cacheKey, value, timeSpan, dbId);
        }

    }
}
