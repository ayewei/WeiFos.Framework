using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WeiFos.WeChat.Models
{
    /// <summary>
    /// 企业付款对象参数
    /// @author yewei 
    /// @date 2018-05-05
    /// </summary>
    [Serializable]
    [XmlRoot("xml")]
    public class EntPay
    {

        /// <summary>
        /// 申请商户号的appid或商户号绑定的appid
        /// 如果当前是子商户，走托管商户托管方式，则这里是托管的主商户号appid
        /// </summary>
        //[XmlElement("appid")]
        public string mch_appid { get; set; }

        /// <summary>
        /// 微信支付分配的商户号
        /// </summary>
        public string mchid { get; set; }

        /// <summary>
        /// 微信支付分配的终端设备号
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
        /// 商户订单号，需保持唯一性
        /// (只能是字母或者数字，不能包含有符号)
        /// </summary>
        public string partner_trade_no { get; set; }

        /// <summary>
        /// 用户openid
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 校验用户姓名选项
        /// NO_CHECK：不校验真实姓名
        /// FORCE_CHECK：强校验真实姓名
        /// </summary>
        public string check_name { get; set; }

        /// <summary>
        /// 收款用户真实姓名。
        /// 如果check_name设置为FORCE_CHECK，则必填用户真实姓名
        /// </summary>
        public string re_user_name { get; set; }

        /// <summary>
        /// 企业付款金额，单位为分
        /// </summary>
        public int amount { get; set; }

        /// <summary>
        /// 企业付款描述信息
        /// </summary>
        public string desc { get; set; }

        /// <summary>
        /// 该IP同在商户平台设置的IP白名单中的IP没有关联，
        /// 该IP可传用户端或者服务端的IP
        /// </summary>
        public string spbill_create_ip { get; set; }


    }
}
