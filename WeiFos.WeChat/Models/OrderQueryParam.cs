using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WeiFos.WeChat.Models
{
    /// <summary>
    /// 微信查询订单 接口对象
    /// @author yewei 
    /// @date 2015-11-06
    /// </summary>
    [Serializable]
    [XmlRoot("xml")]
    public class OrderQueryParam
    {
        /// <summary>
        /// 微信分配的公众账号ID（企业号corpid即为此appId）
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 微信支付分配的商户号
        /// </summary>
        public string mch_id { get; set; }

        /// <summary>
        /// 微信分配的子商户公众账号ID
        /// </summary>
        public string sub_appid { get; set; }

        /// <summary>
        /// 微信分配的子商户公众账号ID
        /// </summary>
        public string sub_mch_id { get; set; }


        /// <summary>
        /// 微信的订单号，优先使用
        /// </summary>
        public string transaction_id { get; set; }

        /// <summary>
        /// 商户系统内部的订单号，当没提供transaction_id时需要传这个
        /// </summary>
        public string out_trade_no { get; set; }
        
        /// <summary>
        /// 随机字符串，不长于32位
        /// </summary>
        public string nonce_str { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }
    }
}
