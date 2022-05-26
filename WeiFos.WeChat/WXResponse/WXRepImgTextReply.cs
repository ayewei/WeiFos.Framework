using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using WeiFos.WeChat.WXBase;

namespace WeiFos.WeChat.WXResponse
{
    /// <summary>
    /// 回复图文消息 实体对象
    /// @author yewei 
    /// @date 2013-11-04
    /// </summary>
    [Serializable]
    [XmlType("item")]
    public class WXRepImgTextReply
    {
        /// <summary>
        /// 图文消息标题
        /// </summary>
        [XmlElement("Title", typeof(CDATA))]
        public CDATA Title { get; set; }

        /// <summary>
        /// 图文消息描述
        /// </summary>
        [XmlElement("Description", typeof(CDATA))]
        public CDATA Description { get; set; }

        /// <summary>
        /// 图片链接，支持JPG、PNG格式，较好的效果为大图360*200，小图200*200
        /// </summary>
        [XmlElement("PicUrl", typeof(CDATA))]
        public CDATA PicUrl { get; set; }

        /// <summary>
        /// 点击图文消息跳转链接
        /// </summary>
        [XmlElement("Url", typeof(CDATA))]
        public CDATA Url { get; set; }
    }
}
