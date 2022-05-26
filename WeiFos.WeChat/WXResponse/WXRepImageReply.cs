using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using WeiFos.WeChat.WXBase;

namespace WeiFos.WeChat.WXResponse
{
    /// <summary>
    /// 回复图片消息 实体对象
    /// @author yewei 
    /// @date 2013-11-14
    /// </summary>
    [Serializable]
    [XmlRoot("xml")]
    public class WXRepImageReply : WXRepBaseMsg
    {
        /// <summary>
        /// 通过上传多媒体文件，得到的id。
        /// </summary>
        [XmlArray("Image")]
        [XmlElement("MediaId", typeof(CDATA))]
        public CDATA MediaId;


    }

}
