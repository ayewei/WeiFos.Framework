using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WeiFos.WeChat.Models
{
    /// <summary>
    /// 申请退款接口对象参数
    /// @author yewei 
    /// @date 2015-11-09
    /// </summary>
    [Serializable]
    [XmlRoot("xml")]
    public class RefundPay
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
        /// 微信支付分配的子商户号
        /// </summary>
        public string sub_mch_id { get; set; }

        /// <summary>
        /// 终端设备号(门店号或收银设备ID)，注意：PC网页或公众号内支付请传"WEB"
        /// </summary>
        public string device_info { get; set; }

        /// <summary>
        /// 随机字符串，不长于32位
        /// </summary>
        public string nonce_str { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        /// 微信生成的订单号，在支付通知中有返回
        /// </summary>
        public string transaction_id { get; set; }
        
        /// <summary>
        /// 商户系统内部的订单号,32个字符内、可包含字母, 
        /// 其他说明见商户订单号
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 商户系统内部的退款单号，商户系统内部唯一，
        /// 同一退款单号多次请求只退一笔
        /// </summary>
        public string out_refund_no { get; set; }
        
        /// <summary>
        /// 订单总金额，单位为分，详见支付金额
        /// </summary>
        public int total_fee { get; set; }

        /// <summary>
        /// 退款总金额，订单总金额，单位为分，只能为整数，详见支付金额
        /// https://pay.weixin.qq.com/wiki/doc/api/app.php?chapter=4_2
        /// </summary>
        public int refund_fee { get; set; }

        /// <summary>
        /// 货币类型，符合ISO 4217标准的三位字母代码，
        /// 默认人民币：CNY，其他值列表详见货币类型
        /// https://pay.weixin.qq.com/wiki/doc/api/app.php?chapter=4_2
        /// </summary>
        public string refund_fee_type { get; set; }
        
        /// <summary>
        /// 操作员帐号, 默认为商户号
        /// </summary>
        public string op_user_id { get; set; }


    }
}
