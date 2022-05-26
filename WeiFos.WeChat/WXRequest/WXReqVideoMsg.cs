using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WeiFos.WeChat.WXRequest
{
    /// <summary>
    /// 视频消息 实体对象 
    /// @author yewei 
    /// @date 2013-11-04
    /// </summary>
    [Serializable]
    [XmlRoot("xml")]
    public class WXReqVideoMsg : WXReqBaseMsg
    {
        /// <summary>
        /// 视频消息媒体id，可以调用多媒体文件下载接口拉取数据
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// 视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string ThumbMediaId { get; set; }
    }
}
