using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.Alibaba.Models
{

    /// <summary>
    /// 阿里支付宝
    /// 回调实体
    /// </summary>
    public class AliRefundPay
    {

        #region 公共请求参数


        /// <summary>
        /// 支付宝分配给开发者的应用ID
        /// </summary>
        public string app_id { get; set; }


        /// <summary>
        /// 接口名称
        /// </summary>
        public string method { get; set; }


        /// <summary>
        /// 仅支持JSON
        /// </summary>
        public string format { get; set; }


        /// <summary>
        /// 请求使用的编码格式，如utf-8,gbk,gb2312等
        /// </summary>
        public string charset { get; set; }


        /// <summary>
        /// 商户生成签名字符串所使用的签名算法类型，
        /// 目前支持RSA2和RSA，推荐使用RSA2
        /// </summary>
        public string sign_type { get; set; }


        /// <summary>
        /// 商户请求参数的签名串
        /// </summary>
        public string sign { get; set; }


        /// <summary>
        /// 发送请求的时间，
        /// 格式"yyyy-MM-dd HH:mm:ss"
        /// </summary>
        public string timestamp { get; set; }


        /// <summary>
        /// 调用的接口版本，固定为：1.0
        /// </summary>
        public string version { get; set; }


        /// <summary>
        /// 应用授权
        /// </summary>
        public string app_auth_token { get; set; }
         

        /// <summary>
        /// 业务请求参数的集合，最大长度不限，除公共参数外所有请求参数都必须放在这个参数中传递，具体参照各产品快速接入文档
        /// </summary>
        public AliRefundPayBizContent biz_content { get; set; }


        #endregion

    }


    /// <summary>
    /// 阿里支付宝
    /// 回调实体 业务参数
    /// </summary>
    public class AliRefundPayBizContent
    {

        #region 请求参数


        /// <summary>
        /// 订单支付时传入的商户订单号,
        /// 不能和 trade_no同时为空。
        /// 特殊可选
        /// </summary>
        public string out_trade_no { get; set; }


        /// <summary>
        /// 支付宝交易号，和商户订单号不能同时为空
        /// 特殊可选
        /// </summary>
        public string trade_no { get; set; }


        /// <summary>
        /// 需要退款的金额，该金额不能大于订单金额,单位为元，支持两位小数
        /// 必须
        /// </summary>
        public decimal refund_amount { get; set; }


        /// <summary>
        /// 退款的原因说明
        /// 可选
        /// </summary>
        public string refund_reason { get; set; }


        /// <summary>
        /// 标识一次退款请求，同一笔交易多次退款需要保证唯一，
        /// 如需部分退款，则此参数必传
        /// 可选
        /// </summary>
        public string out_request_no { get; set; }


        /// <summary>
        /// 商户的操作员编号
        /// 可选
        /// </summary>
        public string operator_id { get; set; }


        /// <summary>
        /// 商户的门店编号
        /// 可选
        /// </summary>
        public string store_id { get; set; }


        /// <summary>
        /// 商户的终端编号
        /// 可选
        /// </summary>
        public string terminal_id { get; set; }


        #endregion

    }

}