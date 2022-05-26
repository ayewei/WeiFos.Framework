using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WeiFos.WeChat.WXRequest
{
    /// <summary>
    /// 语音消息 实体对象
    /// @author yewei 
    /// @date 2013-11-04 
    /// </summary>
    [Serializable]
    [XmlRoot("xml")]
    public class WXReqVoiceMsg : WXReqBaseMsg
    {

        /// <summary>
        /// 语音消息媒体id，可以调用多媒体文件下载接口拉取数据
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// 语音格式，如amr，speex等 
        /// </summary>
        public string Format { get; set; }

    }
}
