using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WeiFos.WeChat.Models
{
    /// <summary>
    /// 微信扫码支付地址参数
    /// @author yewei 
    /// @date 2015-11-09
    /// </summary>
    [Serializable]
    [XmlRoot("xml")]
    public class CreateQRParam
    {

        public string appid { get; set; }

        /// <summary>
        /// 微信支付分配的商户号
        /// </summary>
        public string mch_id { get; set; }

        /// <summary>
        /// 系统当前时间，定义规则详见时间戳
        /// </summary>
        public string time_stamp { get; set; }

        /// <summary>
        /// 随机字符串，不长于32位
        /// </summary>
        public string nonce_str { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public string product_id { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }

    }
}
