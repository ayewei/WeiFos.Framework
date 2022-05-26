using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.Alibaba.Models
{
    /// <summary>
    /// 及时支付 对象 
    /// @author yewei 
    /// @date 2016-11-14
    /// </summary>
    [Serializable]
    public class InstantPay
    {

        #region 基本参数

        /// <summary>
        /// 接口名称(不可空)
        /// </summary>
        public string service { get; set; }

        /// <summary>
        /// 合作者身份ID (不可空)
        /// 签约的支付宝账号对应的支付宝唯一用户号。
        /// 以2088开头的16位纯数字组成。
        /// </summary>
        public string partner { get; set; }

        /// <summary>
        /// 参数编码字符集 (不可空)
        /// 商户网站使用的编码格式，如UTF-8、GBK、GB2312等
        /// </summary>
        public string _input_charset { get; set; }

        /// <summary>
        /// 签名方式 (不可空)
        /// DSA、RSA、MD5三个值可选，必须大写。
        /// </summary>
        public string sign_type { get; set; }

        /// <summary>
        /// 签名 (不可空)
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        /// 服务器异步通知页面路径
        /// 支付宝服务器主动通知商户网站里指定的页面http路径。
        /// </summary>
        public string notify_url { get; set; }

        /// <summary>
        /// 页面跳转同步通知页面路径
        /// 支付宝处理完请求后，当前页面自动跳转到商户网站里指定页面的http路径。
        /// </summary>
        public string return_url { get; set; }


        #endregion


        #region 业务参数

        /// <summary>
        /// 商户网站唯一订单号(不可空)
        /// 支付宝合作商户网站唯一订单号。
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 商品名称 (不可空)
        /// 商品的标题/交易标题/订单标题/订单关键字等。
        /// 该参数最长为128个汉字。
        /// </summary>
        public string subject { get; set; }

        /// <summary>
        /// 支付类型 (不可空)
        /// 只支持取值为1（商品购买）
        /// </summary>
        public string payment_type { get; set; }

        /// <summary>
        /// 交易金额 (不可空)
        /// 该笔订单的资金总额，单位为RMB-Yuan。取值范围为[0.01，100000000.00]，
        /// 精确到小数点后两位
        /// </summary>
        public string total_fee { get; set; }

        /// <summary>
        /// 卖家支付宝用户号 (不可空)
        /// seller_id、seller_email、seller_account_name
        /// 三个参数至少必须传递一个 
        /// </summary>
        public string seller_id { get; set; }

        /// <summary>
        /// 卖家支付宝账号 (不可空)
        /// seller_id、seller_email、seller_account_name
        /// 三个参数至少必须传递一个 
        /// </summary>
        public string seller_email { get; set; }

        /// <summary>
        /// 卖家支付宝账号别名 (不可空)
        /// seller_id、seller_email、seller_account_name
        /// 三个参数至少必须传递一个 
        /// </summary>
        public string seller_account_name { get; set; }

        /// <summary>
        /// 买家支付宝用户号
        /// buyer_id、buyer_email、buyer_account_name
        /// 三个参数至少必须传递一个 
        /// </summary>
        public string buyer_id { get; set; }

        /// <summary>
        /// 买家支付宝账号
        /// buyer_id、buyer_email、buyer_account_name
        /// 三个参数至少必须传递一个 
        /// </summary>
        public string buyer_email { get; set; }

        /// <summary>
        /// 买家支付宝账号别名
        /// buyer_id、buyer_email、buyer_account_name
        /// 三个参数至少必须传递一个 
        /// </summary>
        public string buyer_account_name { get; set; }

        /// <summary>
        /// 商品单价
        /// 单位为：RMB Yuan。取值范围为[0.01，100000000.00]，精确到小数点后两位。此参数为单价
        /// </summary>
        public string price { get; set; }

        /// <summary>
        /// 购买数量
        /// price、quantity能代替total_fee。即存在total_fee，就不能存在price和quantity；存在price、quantity，就不能存在total_fee
        /// </summary>
        public string quantity { get; set; }

        /// <summary>
        /// 购买数量
        /// 对一笔交易的具体描述信息。如果是多种商品，请将商品描述字符串累加传给body。
        /// </summary>
        public string body { get; set; }

        /// <summary>
        /// 商品展示网址
        /// 收银台页面上，商品展示的超链接
        /// </summary>
        public string show_url { get; set; }

        /// <summary>
        /// 默认支付方式
        /// 取值范围：
        ///  creditPay（信用支付）  directPay（余额支付） 如果不设置，默认识别为余额支付。
        ///  说 明：
        /// 必须注意区分大小写
        /// </summary>
        public string paymethod { get; set; }

        /// <summary>
        /// 支付渠道
        /// 用于控制收银台支付渠道显示，该值的取值范围请参见支付渠道。
        /// 可支持多种支付渠道显示，以“^”分隔。
        /// </summary>
        public string enable_paymethod { get; set; }

        /// <summary>
        /// 防钓鱼时间戳
        /// 通过时间戳查询接口获取的加密支付宝系统时间戳。
        ///  如果已申请开通防钓鱼时间戳验证，则此字段必填
        /// </summary>
        public string anti_phishing_key { get; set; }

        /// <summary>
        /// 客户端IP
        /// 用户在创建交易时，该用户当前所使用机器的IP。
        /// 如果商户申请后台开通防钓鱼IP地址检查选项，此字段必填，校验用。
        /// </summary>
        public string exter_invoke_ip { get; set; }

        /// <summary>
        /// 公用回传参数
        /// 如果用户请求时传递了该参数，则返回给商户时会回传该参数
        /// </summary>
        public string extra_common_param { get; set; }

        /// <summary>
        /// 超时时间
        /// 设置未付款交易的超时时间，一旦超时，该笔交易就会自动被关闭。
        /// 取值范围：1m～15d。
        /// m-分钟，h-小时，d-天，1c-当天（1c-当天的情况下，无论交易何时创建，都在0点关闭）。
        /// 该参数数值不接受小数点，如1.5h，可转换为90m。
        /// </summary>
        public string it_b_pay { get; set; }

        /// <summary>
        /// 快捷登录授权令牌
        /// 如果开通了快捷登录产品，则需要填写；如果没有开通，则为空
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// 扫码支付方式
        /// 扫码支付的方式，支持前置模式和跳转模式。
        /// 前置模式是将二维码前置到商户的订单确认页的模式。需要商户在自己的页面中以iframe方式请求支付宝页面。具体分为以下4种
        /// 0：订单码-简约前置模式，对应iframe宽度不能小于600px，高度不能小于300px；
        /// 1：订单码-前置模式，对应iframe宽度不能小于300px，高度不能小于600px；
        /// 3：订单码-迷你前置模式，对应iframe宽度不能小于75px，高度不能小于75px。
        /// 4：订单码-可定义宽度的嵌入式二维码，商户可根据需要设定二维码的大小。
        /// 跳转模式下，用户的扫码界面是由支付宝生成的，不在商户的域名下
        /// </summary>
        public string qr_pay_mode { get; set; }

        /// <summary>
        /// 商户自定二维码宽度
        /// 商户自定义的二维码宽度。
        ///当qr_pay_mode=4时，该参数生效
        /// </summary>
        public string qrcode_width { get; set; }

        /// <summary>
        /// 是否需要买家实名认证
        /// 是否需要买家实名认证。
        /// T表示需要买家实名认证；
        /// 不传或者传其它值表示不需要买家实名认证
        /// </summary>
        public string need_buyer_realnamed { get; set; }

        /// <summary>
        /// 花呗分期参数
        /// 参数格式：hb_fq_seller_percent ^卖家承担付费比例|hb_fq_num ^期数。
        /// hb_fq_num：花呗分期数，比如分3期支付；
        /// hb_fq_seller_percent：卖家承担收费比例，比如100代表卖家承担100%。
        /// 两个参数必须一起传入。
        /// 两个参数用“|”间隔。Key和value之间用“^”间隔。
        /// 具体花呗分期期数和卖家承担收费比例可传入的数值请咨询支付宝。
        /// </summary>
        public string hb_fq_param { get; set; }

        /// <summary>
        /// 商品类型
        /// 商品类型：  1表示实物类商品  0表示虚拟类商品
        /// 如果不传，默认为实物类商品
        /// </summary>
        public string goods_type { get; set; }


        #endregion



    }
}
