using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Core.SettingModule;

namespace WeiFos.Cache.Config
{
    /// <summary>
    /// Copyright (c) 2013-2022 深圳微狐信息科技有限公司
    /// 描 述：Redis缓存连接器操作类
    /// 创建人：叶委
    /// 日 期：2018.07.07
    /// </summary>
    public class RedisMultiplexer : Multiplexer
    {

        //获取
        public RedisMultiplexer()
        { 
            //读
            ReadMultiplexer = ConnectionMultiplexer.Connect(ConfigManage.AppSettings<RedisConfig>("RedisConfig").ReadServerList);
            //写
            WriteMultiplexer = ConnectionMultiplexer.Connect(ConfigManage.AppSettings<RedisConfig>("RedisConfig").WriteServerList);
        }

        /// <summary>
        /// 写连接器
        /// </summary>
        public ConnectionMultiplexer WriteMultiplexer { get; set; }

        /// <summary>
        /// 读连接器
        /// </summary>
        public ConnectionMultiplexer ReadMultiplexer { get; set; }


    }
}
