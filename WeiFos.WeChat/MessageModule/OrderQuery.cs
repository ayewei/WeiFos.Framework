using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WeiFos.WeChat.MessageModule
{
    /// <summary>
    /// 订单查询消息类
    /// @Author yewei 
    /// @Date 2015-11-01
    /// </summary>
    [XmlRoot("xml")]
    public class OrderQuery : ReturnMessage
    {

        #region 以下字段在 Return_Code为 Success时 返回

        /// <summary>
        /// 公众账号ID
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonce_str { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        /// 业务结果（SUCCESS/FAIL）
        /// </summary>
        public string result_code { get; set; }

        /// <summary>
        /// 错误代码（ORDERNOTEXIST订单不存在、SYSTEMERROR系统错误）
        /// </summary>
        public string err_code { get; set; }

        /// <summary>
        /// 错误代码描述
        /// </summary>
        public string err_code_des { get; set; }

        #endregion


        #region 以下字段在Return_Code和Result_Code 都为Success时返回


        /// <summary>
        /// 设备号 非必填
        /// </summary>
        public string device_info { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 是否关注公众账号,Y-关注，N-未关注
        /// </summary>
        public string is_subscribe { get; set; }

        /// <summary>
        /// 交易类型：JSAPI、NATIVE、MICROPAY 、APP
        /// </summary>
        public string trade_type { get; set; }


        /// <summary>
        /// 交易状态
        /// </summary>
        /// <remarks>
        /// SUCCESS—支付成功,REFUND—转入退款,NOTPAY—未支付,CLOSED—已关闭
        /// REVOKED—已撤销,USERPAYING--用户支付中,NOPAY--未支付(输入密码或,确认支付超时)
        /// PAYERROR--支付失败(其他,原因，如银行返回失败)
        /// </remarks>
        public string trade_state { get; set; }

        /// <summary>
        /// 付款银行 银行类型，采用字符串类型的银行标识
        /// </summary>
        public string bank_type { get; set; }
        
        /// <summary>
        /// 总金额,单位分
        /// </summary>
        public int total_fee { get; set; }

        /// <summary>
        /// 货币种类
        /// </summary>
        public string fee_type { get; set; }

        /// <summary>
        /// 现金支付金额订单现金支付金额 
        /// </summary>
        public int cash_fee { get; set; }

        /// <summary>
        /// 货币类型，符合ISO 4217标准的三位字母代码，
        /// 默认人民币：CNY，其他值列表详见货币类型 
        /// </summary>
        public string cash_fee_type { get; set; }
        
        /// <summary>
        /// 现金券金额,现金券支付金额小于等于订单总金额，
        /// 订单总金额-现金券金额为现金支付金额
        /// </summary>
        public int coupon_fee { get; set; }

        /// <summary>
        /// 代金券或立减优惠使用数量
        /// </summary>
        public int coupon_count { get; set; }

        /// <summary>
        /// 代金券或立减优惠批次ID ,$n为下标，从0开始编号
        /// </summary>
        [XmlElement("coupon_batch_id_$n")]
        public string coupon_batch_id_ { get; set; }

        /// <summary>
        /// 代金券或立减优惠ID, $n为下标，从0开始编号
        /// </summary>
        [XmlElement("coupon_id_$n")]
        public string coupon_id_ { get; set; }

        /// <summary>
        /// 单个代金券或立减优惠支付金额, $n为下标，从0开始编号
        /// </summary>
        [XmlElement("coupon_fee_$n")]
        public string coupon_fee_ { get; set; }

        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string transaction_id { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 商家数据包,原样返回
        /// </summary>
        public string attach { get; set; }

        /// <summary>
        /// 交易完成时间
        /// </summary>
        public string time_end { get; set; }

        #endregion

        /// <summary>
        /// 支付是否成功
        /// </summary>
        //[XmlIgnore]
        public bool Success()
        {
            return "success".Equals(return_code.ToLower()) && "success".Equals(this.return_code.ToLower()) && "success".Equals(this.trade_state.ToLower());
        }

        /// <summary>
        /// 没有预支付
        /// </summary>
        //[XmlIgnore]
        public bool NotPrePay()
        {
            return "success".Equals(return_code.ToLower()) && !string.IsNullOrEmpty(err_code) && "ORDERNOTEXIST".Equals(err_code.ToUpper());
        }
    
    }
}
