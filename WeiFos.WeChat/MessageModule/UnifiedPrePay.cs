using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WeiFos.WeChat.MessageModule
{
    /// <summary>
    /// 微信统一支付回调消息
    /// @Author yewei 
    /// @Date 2015-11-01
    /// </summary>
    [Serializable]
    [XmlRoot("xml")]
    public class UnifiedPrePay : ReturnMessage
    {
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
        /// 或者当前调起支付的小程序APPID
        /// </summary>
        public string sub_appid { get; set; }

        /// <summary>
        /// 子商户ID
        /// </summary>
        public string sub_mch_id { get; set; }

        /// <summary>
        /// 调用接口提交的终端设备号
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

        /// <summary>
        /// 调用接口提交的交易类型，
        /// 取值如下：JSAPI，NATIVE，APP
        /// </summary>
        public string trade_type { get; set; }

        /// <summary>
        /// 微信生成的预支付回话标识，
        /// 用于后续接口调用中使用，该值有效期为2小时
        /// </summary>

        public string prepay_id { get; set; }

        /// <summary>
        /// trade_type为NATIVE是有返回，可将该参数值生成二维码展示出来进行扫码支付
        /// </summary>
        public string code_url { get; set; }

        /// <summary>
        /// 场景信息 String(256)
        /// 该字段用于上报场景信息，目前支持上报实际门店信息。该字段为JSON对象数据，
        /// 对象格式为{"store_info":{"id": "门店ID","name": "名称","area_code": "编码","address": "地址" }} ，
        /// 字段详细说明请点击行前的+展开
        /// </summary>
        public string scene_info { get; set; }
        

        public bool ResultSuccess()
        {
                if (result_code == null) return false;
                return this.result_code.ToLower() == "success";
        }

        //[XmlIgnore]
        public bool ReturnSuccess()
        {
            if (return_code == null) return false;
            return return_code.ToLower() == "success";
        }
    }


    
}
