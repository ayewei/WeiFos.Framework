using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using WeiFos.WeChat.WXBase;

namespace WeiFos.WeChat.WXRequest
{
    /// <summary>
    /// 事件消息 实体对象
    /// @author yewei 
    /// @date 2013-11-27
    /// </summary>
    [Serializable]
    [XmlRoot("xml")]
    public class WXReqEventMsg : WXReqBaseMsg
    {
        /// <summary>
        /// 事件类型EventKey
        /// </summary>
        [XmlElement("Event", typeof(CDATA))]
        public CDATA Event { get; set; }

        /// <summary>
        /// 事件KEY值
        /// </summary>
        [XmlElement("EventKey", typeof(CDATA))]
        public CDATA EventKey { get; set; }

        /// <summary>
        /// 票据
        /// </summary>
        [XmlElement("Ticket", typeof(CDATA))]
        public CDATA Ticket { get; set; }
    }

}
