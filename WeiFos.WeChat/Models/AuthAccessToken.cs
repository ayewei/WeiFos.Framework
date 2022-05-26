using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.WeChat.Models
{
    /// <summary>
    /// 网页授权获取的用户基本信息
    /// @Author yewei 
    /// @Date 2015-11-01
    /// </summary>
    public class AuthAccessToken : WXCodeError
    {
        /// <summary>  
        /// 网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同  
        /// </summary>  
        public string access_token { get; set; }

        /// <summary>  
        /// access_token接口调用凭证超时时间，单位（秒）   
        /// </summary>  
        public int expires_in { get; set; }

        /// <summary>  
        /// 用户刷新access_token
        /// </summary>  
        public string refresh_token { get; set; } 
         
    }
}
