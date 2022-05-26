using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using WeiFos.WeChat.WXBase;

namespace WeiFos.WeChat.WXResponse
{
    [Serializable]
    [XmlRoot("xml")]
    public class WXRepBaseMsg
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
        public Int64 CreateTime { get; set; }

        /// <summary>
        /// 消息类型 文本消息:text,消息类型:image,地理位置:location,链接消息:link,事件推送:event
        /// </summary>
        [XmlElement("MsgType", typeof(CDATA))]
        public CDATA MsgType { get; set; }

        /// <summary>
        /// 位0x0001被标志时，星标刚收到的消息 
        /// </summary>
        //public int FuncFlag { get; set; }

    }
}
