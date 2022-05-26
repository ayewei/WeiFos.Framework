using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WeiFos.WeChat.MessageModule
{
    /// <summary>
    /// 企业付款 返回参数
    /// @author yewei 
    /// @date 2018-05-05
    /// </summary>
    [Serializable]
    [XmlRoot("xml")]
    public class EntPayMessage : ReturnMessage
    {

        #region  以下字段在return_code 和result_code都为SUCCESS的时候有返回 

        /// <summary>
        /// 商户订单号，需保持唯一性
        /// (只能是字母或者数字，不能包含有符号)
        /// </summary>
        public string partner_trade_no { get; set; }

        /// <summary>
        /// 微信订单号
        /// 企业付款成功，返回的微信订单号
        /// </summary>
        public string payment_no { get; set; }

        /// <summary>
        /// 微信支付成功时间
        /// 企业付款成功时间
        /// </summary>
        public string payment_time { get; set; }


        #endregion


        #region 以下字段在return_code为SUCCESS的时候有返回 

        /// <summary>
        /// 申请商户号的appid或商户号绑定的appid
        /// 如果当前是子商户，走托管商户托管方式，则这里是托管的主商户号appid
        /// </summary>
        public string mch_appid { get; set; }

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
        /// 错误代码
        /// </summary>
        public string result_code { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string err_code { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string err_code_des { get; set; }


        #endregion


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
