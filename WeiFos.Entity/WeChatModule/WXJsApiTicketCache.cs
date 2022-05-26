using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.WeChatModule
{
    /// <summary>
    /// 公众号用于调用微信JS接口的临时票据 实体类
    /// @author yewei 
    /// @date 2015-09-16
    /// </summary>
    [Serializable]
    [Table(Name = "tb_wx_jsapi_ticket")]
    public class WXJsApiTicketCache
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 票据
        /// </summary>
        public string ticket { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime expires_time { get; set; }

    }
}