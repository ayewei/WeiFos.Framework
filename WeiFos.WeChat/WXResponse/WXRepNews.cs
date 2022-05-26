using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using WeiFos.WeChat.WXBase;

namespace WeiFos.WeChat.WXResponse
{
    /// <summary>
    /// 回复多图文 实体对象
    /// @author yewei 
    /// @date 2013-11-04
    /// </summary>
    [Serializable]
    [XmlRoot("xml")]
    public class WXRepNews : WXRepBaseMsg
    {
        /// <summary>
        /// 图文消息个数，限制为10条以内
        /// </summary>
        [XmlElement("ArticleCount")]
        public int ArticleCount;

        /// <summary>
        /// 回复关联的图文集合
        /// </summary>
        [XmlArray("Articles")]
        public List<WXRepImgTextReply> imgTextReplys { get; set; }

        /// <summary>
        /// 图文消息标题
        /// </summary>
        [XmlElement("Title", typeof(CDATA))]
        public CDATA Title;

    }


}
