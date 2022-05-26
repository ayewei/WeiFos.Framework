using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Cache.Context;
using WeiFos.Cache.Data;
using WeiFos.Cache.Session;

namespace WeiFos.Cache
{
    /// <summary>
    /// Copyright (c) 2013-2022 深圳微狐信息科技有限公司
    /// 描 述：缓存会发工厂
    /// 创建人：叶委
    /// 日 期：2018.07.07
    /// </summary>
    public class CacheSessionFactory
    {

        #region 单列模式  

        private static CacheSessionFactory instance = null;

        /*私有构造器，不能该类外部new对象*/
        private CacheSessionFactory() {
            cacheContext = new RedisCacheContext();
        }
        public static CacheSessionFactory Instance
        {
            get { return instance = instance ?? new CacheSessionFactory(); }
        }

        #endregion


        /// <summary>
        /// 缓存访问管理层
        /// </summary>
        private CacheContext cacheContext = null;
 

        /// <summary>
        /// 缓存接口
        /// </summary>
        /// <returns></returns>
        public ICacheSession CreateCache()
        {
            return new CacheSession(cacheContext);
        }


    }
}
