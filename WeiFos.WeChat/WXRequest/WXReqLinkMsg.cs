using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WeiFos.WeChat.WXRequest
{
    /// <summary>
    /// 链接消息 实体对象
    /// @author yewei 
    /// @date 2013-11-04 
    /// </summary>
    [Serializable]
    [XmlRoot("xml")]
    public class WXReqLinkMsg : WXReqBaseMsg
    {
        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 消息链接
        /// </summary>
        public string Url { get; set; }
    }
}
