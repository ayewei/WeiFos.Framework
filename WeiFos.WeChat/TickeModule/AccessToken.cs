using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.WeChat.Models;

namespace WeiFos.WeChat.TickeModule
{
    /// <summary>
    /// 授权token
    /// @author yewei 
    /// @date 2014-01-22
    /// </summary>
    public class AccessToken : WXCodeError
    {
        /// <summary>
        /// 微信接口token
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 微信接口过期时间
        /// </summary>
        public int expires_in { get; set; }

        /// <summary>
        /// 提前更新时间（秒）
        /// 用于提前过期判断
        /// </summary>
        public int expires_lead { get; set; }
    }
}
