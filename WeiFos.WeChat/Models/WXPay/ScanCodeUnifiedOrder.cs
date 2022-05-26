using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using WeiFos.WeChat.MessageModule;

namespace WeiFos.WeChat.Models.WXPay
{

    /// <summary>
    /// 微信统一下单接口对象
    /// @author yewei 
    /// @date 2015-11-01
    /// </summary>
    [Serializable]
    [XmlRoot("xml")]
    public class ScanCodeUnifiedOrder : UnifiedPrePay
    {

        /// <summary>
        /// 用户标识 openid
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 是否关注公众账号
        /// Y 用户是否关注公众账号，
        /// 仅在公众账号类型支付有效，
        /// 取值范围：Y或N;Y-关注;N-未关注
        /// </summary>
        public string is_subscribe { get; set; }

        /// <summary>
        /// 是否关注公众账号
        /// 子商户appid下用户唯一标识
        /// 如需返回则请求时需要传sub_appid
        /// </summary>
        public string sub_openid { get; set; }

        /// <summary>
        /// 是否关注子公众账号
        /// 用户是否关注子公众账号，
        /// 仅在公众账号类型支付有效，
        /// 取值范围：Y或N;Y-关注;N-未关注
        /// </summary>
        public string sub_is_subscribe { get; set; }
         
        /// <summary>
        /// 付款银行
        /// String(32)  CMC 银行类型，
        /// 采用字符串类型的银行标识，值列表详见银行类型
        /// </summary>
        public string bank_type { get; set; }

        /// <summary>
        /// 标价币种	 String(16)  
        /// CNY 符合ISO 4217标准的三位字母代码，
        /// 默认人民币：CNY，其他值列表详见货币类型
        /// </summary>
        public string fee_type { get; set; }

        /// <summary>
        /// 标价金额
        /// 订单总金额，单位为分，
        /// 只能为整数，详见支付金额
        /// </summary>
        public int total_fee { get; set; }

        /// <summary>
        /// 现金支付币种
        /// String(16)	CNY
        /// 符合ISO 4217标准的三位字母代码，
        /// 默认人民币：CNY，其他值列表详见
        /// </summary>
        public string cash_fee_type { get; set; }

        /// <summary>
        /// 现金支付金额
        /// 订单现金支付金额，详见支付金额,单位为分，
        /// </summary>
        public int cash_fee { get; set; }

        /// <summary>
        /// 当订单使用了免充值型优惠券后返回该参数，
        /// 应结订单金额=订单金额-免充值优惠券金额。
        /// </summary>
        public int settlement_total_fee { get; set; }

        /// <summary>
        /// 代金券金额，
        /// 代金券”金额<=订单金额，订单金额-“代金券”金额=现金支付金额，详见支付金额
        /// </summary>
        public int coupon_fee { get; set; }

        /// <summary>
        /// 微信支付订单号
        /// String(32)  1217752501201407033233368018	
        /// </summary>
        public string transaction_id { get; set; }

        /// <summary>
        /// 商户订单号
        /// 商户系统内部订单号，要求32个字符内，
        /// 只能是数字、大小写字母_-|*且在同一个商户号下唯一。
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 商家数据包，原样返回
        /// </summary>
        public string attach { get; set; }

        /// <summary>
        /// 订单生成时间，格式为yyyyMMddHHmmss，
        /// 如2009年12月25日9点10分10秒表示为20091225091010。详见时间规则
        /// String(14)  20141030133525	
        /// </summary>
        public string time_end { get; set; }

    }
}
