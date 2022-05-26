using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.WeChatModule
{
    /// <summary>
    /// 微信公众号商户信息
    /// @author yewei 
    /// add by @date 2015-10-20
    /// </summary>
    [Serializable]
    [Table(Name = "tb_wx_merchant")]
    public class WeChatMerchant
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 支付方式
        /// 11 微信付款码支付,12 微信JSAPI支付,13 微信小程序支付,
        /// 14 微信扫码支付,15 微信APP支付,16 微信APP支付
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// APPID
        /// </summary>
        public string app_id { get; set; }

        /// <summary>
        /// 商户号ID
        /// </summary>
        public string mch_id { get; set; }

        /// <summary>
        /// app秘钥
        /// </summary>
        public string app_secret { get; set; }

        /// <summary>
        /// app支付key
        /// </summary>
        public string pay_key { get; set; }

        /// <summary>
        /// 是否是子商户
        /// </summary>
        public bool is_child_merchant { get; set; }

        /// <summary>
        /// 子商户app_id
        /// </summary>
        public string sub_appid { get; set; }

        /// <summary>
        /// 子商户号
        /// </summary>
        public string sub_mch_id { get; set; }

    }
}
