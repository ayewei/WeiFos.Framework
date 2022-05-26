using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using WeiFos.WeChat.WXBase;

namespace WeiFos.WeChat.WXResponse
{
    /// <summary>
    /// 回复文本消息 实体对象
    /// @author yewei 
    /// @date 2013-11-14
    /// </summary>
    [Serializable]
    [XmlRoot("xml")]
    public class WXRepTextReply : WXRepBaseMsg
    {

        /// <summary>
        /// 回复的消息内容（换行：在content中能够换行，微信客户端就支持换行显示）
        /// </summary>
        [XmlElement("Content", typeof(CDATA))]
        public CDATA Content;

    }

}
