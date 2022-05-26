using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.WeChat.Models;

namespace WeiFos.WeChat.TickeModule
{
    public class JsApiTicket : WXCodeError
    {
        /// <summary>
        /// 票据
        /// </summary>
        public string ticket { get; set; }

        /// <summary>
        /// 凭证的有效时间（秒）
        /// </summary>
        public int expires_in { get; set; }

        /// <summary>
        /// 提前更新时间（秒）
        /// 用于提前过期判断
        /// </summary>
        public int expires_lead { get; set; }
    }
}
