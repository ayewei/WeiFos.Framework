using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WeiFos.WeChat.MessageModule
{
    /// <summary>
    /// 支付结果通用通知
    /// @Author yewei 
    /// @Date 2015-11-01
    /// </summary>
    [XmlRoot("xml")]
    public class NotifyPay : ReturnMessage
    {

        #region 以下在Return_Code 为Success时返回

        /// <summary>
        /// 微信分配的公众账号ID（企业号corpid即为此appId）
        /// </summary>
        //[XmlElement("appid")]
        public string appid { get; set; }

        /// <summary>
        /// 微信支付分配的商户号
        /// </summary>
        public string mch_id { get; set; }

        /// <summary>
        /// 微信支付分配的终端设备号
        /// </summary>
        public string device_info { get; set; }

        /// <summary>
        /// 随机字符串，不长于32位
        /// </summary>
        public string nonce_str { get; set; }

        /// <summary>
        /// 签名，详见签名算法
        /// https://pay.weixin.qq.com/wiki/doc/api/app.php?chapter=4_3
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        /// 业务结果 SUCCESS/FAIL
        /// </summary>
        public string result_code { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string err_code { get; set; }

        /// <summary>
        /// 错误代码描述
        /// </summary>
        public string err_code_des { get; set; }

        #endregion

        #region 以下在Return_Code和Result_Code都为Success时返回

        /// <summary>
        /// 用户标识
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 是否关注公众账号
        /// 用户是否关注公众账号，Y-关注，N-未关注，仅在公众账号类型支付有效
        /// </summary>
        public string is_subscribe { get; set; }

        /// <summary>
        /// 交易类型
        /// JSAPI、NATIVE、MICROPAY、APP
        /// </summary>
        public string trade_type { get; set; }

        /// <summary>
        /// 付款银行
        /// </summary>
        public string bank_type { get; set; }

        /// <summary>
        /// 订单总金额,单位为分
        /// </summary>
        public int total_fee { get; set; }

        /// <summary>
        /// 货币种类
        /// 货币类型，符合ISO 4217 标准的三位字母代码，默认人民币：CNY
        /// 非必填
        /// </summary>
        public string fee_type { get; set; }

        /// <summary>
        /// 现金支付金额订单现金支付金额，详见支付金额
        /// https://pay.weixin.qq.com/wiki/doc/api/app.php?chapter=4_2
        /// </summary>
        public int cash_fee { get; set; }

        /// <summary>
        /// 货币类型，符合ISO4217标准的三位字母代码，默认人民币：CNY，其他值列表详见货币类型
        /// https://pay.weixin.qq.com/wiki/doc/api/app.php?chapter=4_2
        /// </summary>
        public string cash_fee_type { get; set; }
        
        /// <summary>
        /// 代金券或立减优惠金额 订单总金额，订单总金额-代金券或立减优惠金额=现金支付金额，详见支付金额
        /// </summary>
        public int coupon_fee { get; set; }

        /// <summary>
        /// 代金券或立减优惠使用数量
        /// </summary>
        public int coupon_count { get; set; }

        /// <summary>
        /// 代金券或立减优惠使用数量
        /// </summary>
        [XmlElement("coupon_id_$n")]
        public string coupon_id_ { get; set; }
        
        /// <summary>
        /// 单个代金券或立减优惠支付金额,$n为下标，从0开始编号
        /// </summary>
        [XmlElement("coupon_fee_$n")]
        public string coupon_fee_ { get; set; }
        
        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string transaction_id { get; set; }

        /// <summary>
        /// 商户系统的订单号，与请求一致。
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 商家数据包，原样返回
        /// String(128)
        /// </summary>
        public string attach { get; set; }

        /// <summary>
        /// 支付完成时间
        /// 格式为yyyyMMddhhmmss，如2009 年12 月27 日9 点10 分10 秒表示为20091227091010。时区为GMT+8 beijing。该时间取自微信支付服务器
        /// </summary>
        public string time_end { get; set; }
        #endregion

        //[XmlIgnore]
        public bool ResultSuccess()
        {
            return ("success".Equals(this.result_code.ToLower()));
        }

        //[XmlIgnore]
        public bool ReturnSuccess()
        {
            return ("success".Equals(return_code.ToLower()));
        }

    }
}
