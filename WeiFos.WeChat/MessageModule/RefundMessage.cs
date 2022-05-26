using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WeiFos.WeChat.MessageModule
{
    /// <summary>
    /// 退款申请消息 实体
    /// @Author yewei 
    /// @Date 2015-11-09
    /// </summary>
    [XmlRoot("xml")]
    public class RefundMessage : ReturnMessage
    {

        /// <summary>
        /// 业务结果
        /// SUCCESS/FAIL
        /// SUCCESS退款申请接收成功，结果通过退款查询接口查询
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

        /// <summary>
        /// 公众账号ID
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id { get; set; }

        /// <summary>
        /// 微信订单号
        /// </summary>
        public string transaction_id { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 商户退款单号
        /// </summary>
        public string out_refund_no { get; set; }

        /// <summary>
        /// 微信退款单号
        /// </summary>
        public string refund_id { get; set; }

        /// <summary>
        /// 退款总金额,单位为分,可以做部分退款
        /// </summary>
        public string refund_fee { get; set; }

        /// <summary>
        /// 退款申请是否接收成功（不代表退款成功，退款结果需要查询）
        /// </summary>
        public bool Success()
        {
            return return_code.ToLower() == "success"
                && return_code.ToLower() == "success";
        }


    }
}
