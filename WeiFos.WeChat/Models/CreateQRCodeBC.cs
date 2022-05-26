using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WeiFos.WeChat.Models
{
    /// <summary>
    /// 微信扫码支付地址参数
    /// @author yewei 
    /// @date 2015-11-09
    /// </summary>
    [Serializable]
    [XmlRoot("xml")]
    public class CreateQRCodeBC : CreateQRParam
    {
        /// <summary>
        /// 用户是否关注公众账号，仅在公众账号类型支付有效，
        /// 取值范围：Y或N;Y-关注;N-未关注
        /// </summary>
        public string is_subscribe { get; set; }
    }
}
