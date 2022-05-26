using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WeiFos.WeChat.Models
{
    /// <summary>
    /// 微信统一下单接口对象
    /// @author yewei 
    /// @date 2015-11-01
    /// </summary>
    [Serializable]
    [XmlRoot("xml")]
    public class UnifiedOrder
    {
        public UnifiedOrder()
        {
        }

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
        /// 子商户公众账号ID
        /// </summary>
        public string sub_appid { get; set; }

        /// <summary>
        /// 子商户号
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
        /// 商品或支付单简要描述
        /// </summary>
        public string body { get; set; }

        /// <summary>
        /// 商品名称明细列表
        /// </summary>
        public string detail { get; set; }

        /// <summary>
        /// 附加数据，在查询API和支付通知中原样返回，
        /// 该字段主要用于商户携带订单的自定义数据
        /// </summary>
        public string attach { get; set; }

        /// <summary>
        /// 商户系统内部的订单号,32个字符内、可包含字母, 
        /// 其他说明见商户订单号
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 符合ISO 4217标准的三位字母代码，
        /// 默认人民币：CNY，其他值列表详见货币类型
        /// </summary>
        public string fee_type { get; set; }

        /// <summary>
        /// 订单总金额，单位为分，详见支付金额
        /// </summary>
        public string total_fee { get; set; }
        
        /// <summary>
        /// APP和网页支付提交用户端ip，
        /// Native支付填调用微信支付API的机器IP
        /// </summary>
        public string spbill_create_ip { get; set; }
        
        /// <summary>
        /// 订单生成时间，格式为yyyyMMddHHmmss，
        /// 如2009年12月25日9点10分10秒表示为20091225091010。其他详见时间规则
        /// </summary>
        public string time_start { get; set; }
        
        /// <summary>
        /// 订单失效时间，格式为yyyyMMddHHmmss，
        /// 如2009年12月27日9点10分10秒表示为20091227091010。
        /// 其他详见时间规则注意：最短失效时间间隔必须大于5分钟
        /// </summary>
        public string time_expire { get; set; }

        /// <summary>
        /// 商品标记，代金券或立减优惠功能的参数，说明详见代金券或立减优惠
        /// https://pay.weixin.qq.com/wiki/doc/api/sp_coupon.php?chapter=12_1
        /// </summary>
        public string goods_tag { get; set; }
        
        /// <summary>
        /// 接收微信支付异步通知回调地址
        /// </summary>
        public string notify_url { get; set; }

        /// <summary>
        /// 取值如下：JSAPI，NATIVE，APP，详细说明见参数规定
        /// https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=4_2
        /// </summary>
        public string trade_type { get; set; }

        /// <summary>
        /// trade_type=NATIVE，此参数必传。此id为二维码中包含的商品ID，商户自行定义
        /// </summary>
        public string product_id { get; set; }

        /// <summary>
        /// no_credit--指定不能使用信用卡支付
        /// </summary>
        public string limit_pay { get; set; }

        /// <summary>
        /// trade_type=JSAPI，此参数必传，用户在商户appid下的唯一标识。
        /// openid如何获取，可参考 https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=4_4 【获取openid】。
        /// 企业号请使用 http://qydev.weixin.qq.com/wiki/index.php?title=OAuth%E9%AA%8C%E8%AF%81%E6%8E%A5%E5%8F%A3 【企业号OAuth2.0接口】
        /// 获取企业号内成员userid http://qydev.weixin.qq.com/wiki/index.php?title=Userid%E4%B8%8Eopenid%E4%BA%92%E6%8D%A2%E6%8E%A5%E5%8F%A3 
        /// 再调用【企业号userid转openid接口】进行转换
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// trade_type=JSAPI，此参数必传，用户在子商户appid下的唯一标识。
        /// openid和sub_openid可以选传其中之一，如果选择传sub_openid,则必须传sub_appid
        /// </summary>
        public string sub_openid { get; set; }

        /// <summary>
        /// 授权码
        /// 扫码支付授权码，设备读取用户微信中的条码或者二维码信息
        /// （注：用户付款码条形码规则：18位纯数字，以10、11、12、13、14、15开头）
        /// </summary>
        public string auth_code { get; set; }


        /// <summary>
        /// 开发票入口开放标识
        /// </summary>
        public string receipt { get; set; }

        /// <summary>
        /// 支付场景
        /// 该字段用于上报场景信息，目前支持上报实际门店信息。该字段为JSON对象数据，
        /// 对象格式为{"store_info":{"id": "门店ID","name": "名称","area_code": "编码","address": "地址" }}
        /// </summary>
        public string scene_info { get; set; }

        

    }
}
