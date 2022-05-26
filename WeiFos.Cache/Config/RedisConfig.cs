using System;
using WeiFos.Cache.Context;
using WeiFos.Core.SettingModule;

namespace WeiFos.Cache.Config
{
    /// <summary>
    /// Copyright (c) 2013-2022 深圳微狐信息科技有限公司
    /// 描 述：redis配置信息
    /// 创建人：叶委
    /// 日 期：2018.07.07
    /// </summary>
    public class RedisConfig
    {


        /// <summary>
        /// 可写的Redis链接地址
        /// </summary>
        public string WriteServerList { get; set; }


        /// <summary>
        /// 可读的Redis链接地址
        /// </summary>
        public string ReadServerList { get; set; }


        /// <summary>
        /// 最大写链接数
        /// </summary>
        public int MaxWritePoolSize { get; set; }


        /// <summary>
        /// 最大读链接数
        /// </summary> 
        public int MaxReadPoolSize { get; set; }


        /// <summary>
        /// 自动重启
        /// </summary>
        public bool AutoStart { get; set; }


        /// <summary>
        /// 本地缓存到期时间，单位:秒
        /// </summary>
        public int LocalCacheTime { get; set; }

        /// <summary>
        /// 是否记录日志,该设置仅用于排查redis运行时出现的问题,如redis工作正常,请关闭该项
        /// </summary>
        public bool RecordeLog { get; set; }


        /// <summary>
        /// 默认开始db
        /// </summary>
        public static long DefaultDb { get; set; }

    }
}