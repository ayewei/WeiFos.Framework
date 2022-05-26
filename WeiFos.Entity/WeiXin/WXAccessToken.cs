using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.Entity.WeiXin
{
    /// <summary>
    /// 微信调用接口凭证
    /// @author arvin 
    /// @date 2014-02-11
    /// </summary>
    public class WXAccessToken
    {
        /// <summary>
        /// 微信接口token
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 微信接口过期时间
        /// </summary>
        public int expires_in { get; set; }
    }
}
