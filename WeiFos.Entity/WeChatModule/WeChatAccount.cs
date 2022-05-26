using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.WeChatModule
{
    /// <summary>
    /// 会共号实体类
    /// @author yewei 
    /// @date 2013-09-21
    /// </summary>
    [Serializable]
    [Table(Name = "tb_wx_account")]
    public class WeChatAccount : BaseClass
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 微信号
        /// </summary>
        public string wechat_no { get; set; }

        /// <summary>
        /// 公众号名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 公众号token
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// 公众号url
        /// </summary>
        public string server_url { get; set; }

        /// <summary>
        /// 公众号原始ID
        /// </summary>
        public string original_id { get; set; }

        /// <summary>
        /// 公众号邮箱
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 公众号类型
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 秘钥
        /// </summary>
        public string app_secret { get; set; }

        /// <summary>
        /// 公众号AppId
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 公众号商户ID
        /// </summary>
        //public string mch_id { get; set; }

        /// <summary>
        /// 支付秘钥
        /// </summary>
        //public string pay_key { get; set; }

        /// <summary>
        /// 小程序APPID
        /// </summary>
        public string mini_appid { get; set; }

        /// <summary>
        /// 小程序秘钥
        /// </summary>
        public string mini_app_secret { get; set; }


    }
}

