using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WeiFos.WeChat.WXBase.WXOpen
{
    /// <summary>
    /// 微信开放平台调用凭据
    /// 微信服务器 每隔10分钟会向第三方的消息
    /// 接收地址推送一次component_verify_ticket
    /// component_verify_ticket的有效时间较component_access_token更长，
    /// 建议保存最近可用的component_verify_ticket，在component_access_token过期之前使用该ticket进行更新，
    /// 避免出现因为ticket接收失败而无法更新component_access_token的情况
    /// @author yewei 
    /// @date 2018-04-12
    /// </summary>
    [Serializable]
    [XmlRoot("xml")]
    public class WXOpenCmptVerifyTicket
    {

        /// <summary>
        /// 第三方平台appid
        /// </summary>
        [XmlElement("AppId", typeof(CDATA))]
        public CDATA AppId { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// component_verify_ticket 该值代值
        /// 是微信开放平台调用凭据
        /// </summary>
        [XmlElement("InfoType", typeof(CDATA))]
        public CDATA InfoType { get; set; }

        /// <summary>
        /// 凭据内容
        /// </summary>
        [XmlElement("ComponentVerifyTicket", typeof(CDATA))]
        public CDATA ComponentVerifyTicket { get; set; }

    }
}
