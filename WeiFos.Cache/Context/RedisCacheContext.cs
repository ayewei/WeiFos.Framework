using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Cache.Config;
using WeiFos.Cache.Data;
using WeiFos.Core.Extensions;
using WeiFos.Core.SettingModule;

namespace WeiFos.Cache.Context
{

    /// <summary>
    /// Copyright (c) 2013-2022 深圳微狐信息科技有限公司
    /// 描 述：Redis缓存上下文
    /// 创建人：叶委
    /// 日 期：2018.07.07
    /// </summary>
    public class RedisCacheContext : CacheContext
    {

        /// <summary>
        /// 获取Redis连接器
        /// </summary>
        /// <returns></returns>
        public override Multiplexer GetMultiplexer()
        {
            return new RedisMultiplexer();
        }

        /// <summary>
        /// 指定缓存key是否存在
        /// </summary>
        /// <param name="multiplexer"></param>
        /// <param name="cacheKey"></param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public override bool Exists(Multiplexer multiplexer, string cacheKey, int dbId = 0)
        {
            RedisMultiplexer redisMultiplexer = (RedisMultiplexer)multiplexer;
            var db = redisMultiplexer.ReadMultiplexer.GetDatabase(dbId);
            return db.KeyExists(cacheKey);
        }

        /// <summary>
        /// 读
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="multiplexer"></param>
        /// <param name="cacheKey"></param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public override T Read<T>(Multiplexer multiplexer, string cacheKey, int dbId = 0)
        {
            RedisMultiplexer redisMultiplexer = (RedisMultiplexer)multiplexer;
            var db = redisMultiplexer.ReadMultiplexer.GetDatabase(dbId);
            var value = db.StringGet(cacheKey);
            return db.StringGet(cacheKey).IsNullOrEmpty ? default(T) : value.ToString().ToObject<T>();
        }


        /// <summary>
        /// 写
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="multiplexer"></param>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="dbId"></param>
        public override void Write<T>(Multiplexer multiplexer, string cacheKey, T value, int dbId = 0)
        {
            RedisMultiplexer redisMultiplexer = (RedisMultiplexer)multiplexer;
            var db = redisMultiplexer.ReadMultiplexer.GetDatabase(dbId);
            db.StringSet(cacheKey, value.ToJson());
        }


        /// <summary>
        /// 写
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="multiplexer"></param>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="expireTime"></param>
        /// <param name="dbId"></param>
        public override void Write<T>(Multiplexer multiplexer, string cacheKey, T value, DateTime expireTime, int dbId = 0)
        {
            RedisMultiplexer redisMultiplexer = (RedisMultiplexer)multiplexer;
            var db = redisMultiplexer.ReadMultiplexer.GetDatabase(dbId);
            string json = value.ToJson();
            db.StringSet(cacheKey, value.ToJson(), expireTime - DateTime.Now);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="multiplexer"></param>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan"></param>
        /// <param name="dbId"></param>
        public override void Write<T>(Multiplexer multiplexer, string cacheKey, T value, TimeSpan timeSpan, int dbId = 0)
        {
            RedisMultiplexer redisMultiplexer = (RedisMultiplexer)multiplexer;
            var db = redisMultiplexer.ReadMultiplexer.GetDatabase(dbId);
            string json = value.ToJson();
            db.StringSet(cacheKey, value.ToJson(), timeSpan);
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="multiplexer"></param>
        /// <param name="cacheKey"></param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public override bool Remove(Multiplexer multiplexer, string cacheKey, int dbId = 0)
        {
            RedisMultiplexer redisMultiplexer = (RedisMultiplexer)multiplexer;
            var db = redisMultiplexer.ReadMultiplexer.GetDatabase(dbId);
            return db.KeyDelete(cacheKey);
        }


        /// <summary>
        /// 删除所有
        /// </summary>
        /// <param name="multiplexer"></param>
        /// <param name="dbId"></param>
        public override void RemoveAll(Multiplexer multiplexer, int dbId = 0)
        {
            RedisMultiplexer redisMultiplexer = (RedisMultiplexer)multiplexer;
            redisMultiplexer.ReadMultiplexer.GetServer(ConfigManage.AppSettings<RedisConfig>("RedisConfig").ReadServerList).FlushDatabase(dbId);
        }


    }
}
