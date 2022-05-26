using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using WeiFos.WeChat.WXBase;

namespace WeiFos.WeChat.WXRequest
{
    public abstract class WXReqBaseMsg  
    {
        /// <summary>
        /// 接收方
        /// </summary>
        [XmlElement("ToUserName", typeof(CDATA))]
        public CDATA ToUserName { get; set; }

        /// <summary>
        /// 发送方
        /// </summary>
        [XmlElement("FromUserName", typeof(CDATA))]
        public CDATA FromUserName { get; set; }

        /// <summary>
        /// 创建时间(整型)
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 消息类型 文本消息:text,消息类型:image,地理位置:location,链接消息:link,事件推送:event
        /// </summary>
        public string MsgType { get; set; }

        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public Int64 MsgId { get; set; }
 
    }


}
