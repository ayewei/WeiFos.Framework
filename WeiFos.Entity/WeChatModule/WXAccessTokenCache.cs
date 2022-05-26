using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Attributes;
using WeiFos.WeChat.WXBase;

namespace WeiFos.Entity.WeChatModule
{
    /// <summary>
    /// 微信授权token实体类
    /// @author yewei 
    /// @date 2015-09-16
    /// </summary>
    [Serializable]
    [Table(Name = "tb_wx_token")]
    public class WXAccessTokenCache
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 凭证
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// 凭证类型(access_token,jsapi_ticket两种类型)
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 过期时间(对应微信字段expires_in)
        /// </summary>
        public DateTime expires_time { get; set; }

    }
}
