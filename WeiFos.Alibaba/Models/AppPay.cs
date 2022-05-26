using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.Alibaba.Models
{
    /// <summary>
    /// App支付 对象 
    /// @author yewei 
    /// @date 2016-11-14
    /// </summary>
    [Serializable]
    public class AppPay
    { 
        #region 公共参数

        /// <summary>
        /// 接支付宝分配给开发者的应用ID
        /// </summary>
        public string app_id { get; set; }

        /// <summary>
        /// 接口名称 alipay.trade.app.pay
        /// </summary>
        public string method { get; set; }

        /// <summary>
        /// 仅支持JSON 填写JSON
        /// </summary>
        public string format { get; set; }

        /// <summary>
        /// 请求使用的编码格式，如utf-8,gbk,gb2312等  utf-8
        /// </summary>
        public string charset { get; set; }

        /// <summary>
        /// 商户生成签名字符串所使用的签名算法类型，目前支持RSA
        /// </summary>
        public string sign_type { get; set; }

        /// <summary>
        /// 商户请求参数的签名串，详见签名
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        /// 发送请求的时间 2014-07-24 03:07:50
        /// </summary>
        public string timestamp { get; set; }

        /// <summary>
        /// 调用的接口版本，固定为：1.0
        /// </summary>
        public string version { get; set; }

        /// <summary>
        /// 支付宝服务器主动通知商户服务器里指定的页面http/https路径。建议商户使用https
        /// 最多长度 256
        /// </summary>
        public string notify_url { get; set; }

        /// <summary>
        /// 业务请求参数的集合，最大长度不限，除公共参数外所有请求参数都必须放在这个参数中传递，具体参照各产品快速接入文档
        /// </summary>
        public AppPayBizContent biz_content { get; set; }


        #endregion 
    }


    public class AppPayBizContent
    {
        #region 业务参数

        /// <summary>
        /// 对一笔交易的具体描述信息。如果是多种商品，请将商品描述字符串累加传给body
        /// </summary>
        public string body { get; set; }

        /// <summary>
        /// 商品的标题/交易标题/订单标题/订单关键字等
        /// </summary>
        public string subject { get; set; }

        /// <summary>
        /// 商户网站唯一订单号
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 该笔订单允许的最晚付款时间，逾期将关闭交易。取值范围：1m～15d。m-分钟，h-小时，d-天，
        /// 1c-当天（1c-当天的情况下，无论交易何时创建，都在0点关闭）。 该参数数值不接受小数点，
        /// 如 1.5h，可转换为 90m。
        /// </summary>
        public string timeout_express { get; set; }

        /// <summary>
        /// 订单总金额，单位为元，精确到小数点后两位，取值范围[0.01,100000000]
        /// </summary>
        public string total_amount { get; set; }

        /// <summary>
        /// 收款支付宝用户ID。 如果该值为空，则默认为商户签约账号对应的支付宝用户ID
        /// </summary>
        public string seller_id { get; set; }

        /// <summary>
        /// 销售产品码，商家和支付宝签约的产品码，为固定值QUICK_MSECURITY_PAY
        /// </summary>
        public string product_code { get; set; }

        /// <summary>
        /// 商品主类型：0—虚拟类商品，1—实物类商品
        /// 注：虚拟类商品不支持使用花呗渠道
        /// </summary>
        public string goods_type { get; set; }

        /// <summary>
        /// 公用回传参数，如果请求时传递了该参数，则返回给商户时会回传该参数。
        /// 支付宝会在异步通知时将该参数原样返回。本参数必须进行UrlEncode之后才可以发送给支付宝
        /// </summary>
        public string passback_params { get; set; }

        /// <summary>
        /// 优惠参数
        /// 注：仅与支付宝协商后可用
        /// 实例 {"storeIdType":"1"}
        /// </summary>
        public string promo_params { get; set; }

        /// <summary>
        /// 优惠参数
        /// 业务扩展参数，详见下面的“业务扩展参数说明”
        /// 实例 {"sys_service_provider_id":"2088511833207846"}
        /// </summary>
        public string extend_params { get; set; }

        /// <summary>
        /// 可用渠道，用户只能在指定渠道范围内支付
        /// 当有多个渠道时用“,”分隔
        /// 注：与disable_pay_channels互斥
        /// pcredit,moneyFund,debitCardExpress
        /// 
        /// </summary>
        public string enable_pay_channels { get; set; }

        /// <summary>
        /// 禁用渠道，用户不可用指定渠道支付
        /// 当有多个渠道时用“,”分隔
        /// 注：与enable_pay_channels互斥
        /// pcredit,moneyFund,debitCardExpress
        ///  最大长度 128
        /// </summary>
        public string disable_pay_channels { get; set; }


        /// <summary>
        /// 系统商编号，该参数作为系统商返佣数据提取的依据，请填写系统商签约协议的PID
        /// 实例 2088511833207846
        ///  最大长度 64
        /// </summary>
        public string sys_service_provider_id { get; set; }

        #endregion
    }

}
