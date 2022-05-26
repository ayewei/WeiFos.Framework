using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using WeiFos.WeChat.WXBase;

namespace WeiFos.WeChat.WXRequest
{
    /// <summary>
    /// 文字消息 实体对象
    /// @author yewei 
    /// @date 2013-11-04
    /// </summary>
    [Serializable]
    [XmlRoot("xml")]
    public class WXReqTextMsg : WXReqBaseMsg
    {
        /// <summary>
        /// 文本消息内容
        /// </summary>
        [XmlElement("Content", typeof(CDATA))]
        public CDATA Content { get; set; }
 
    }

}
