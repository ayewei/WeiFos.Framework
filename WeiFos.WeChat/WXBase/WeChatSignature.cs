using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.WeChat.WXBase
{
    /// <summary>
    /// 微信公众号验证微信签名
    /// @author yewei 
    /// @date 2015-09-15
    /// </summary>
    public class WeChatSignature
    {
        /// <summary>
        /// 微信加密签名
        /// 结合了开发者填写的token参数和请求中的timestamp参数、nonce参数
        /// </summary>
        public string signature { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string timestamp { get; set; }

        /// <summary>
        /// 随机数
        /// </summary>
        public string nonce { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string echostr { get; set; }
    }
}
