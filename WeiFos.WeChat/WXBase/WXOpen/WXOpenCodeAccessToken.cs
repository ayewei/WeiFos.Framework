using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.WeChat.WXBase.WXOpen
{
    /// <summary>
    /// 授权后接口调用（UnionID）
    /// 通过code获取access_token
    /// @author yewei 
    /// @date 2018-04-12
    /// </summary>
    public class WXOpenCodeAccessToken : WXErrCode
    {


        /// <summary>
        /// 预授权码
        /// </summary>
        public string access_token { get; set; }


        /// <summary>
        /// 有效期 7200秒
        /// </summary>
        public long expires_in { get; set; }


        /// <summary>
        /// 有效期，为10分钟
        /// </summary>
        public string refresh_token { get; set; }


        /// <summary>
        /// 有效期，为10分钟
        /// </summary>
        public string openid { get; set; }


        /// <summary>
        /// 用户授权的作用域，使用逗号（,）分隔
        /// </summary>
        public string scope { get; set; }


    }
}