using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.WeChat.WXBase
{
    /// <summary>
    /// 微信群发图文消息对象
    /// </summary>
    [Serializable]
    public class WXArticles
    {

        /// <summary>
        /// 图文消息，
        /// 一个图文消息支持1到10条图文 
        /// </summary>
        public string thumb_media_id { get; set; }

        /// <summary>
        /// 媒体文件上传后
        /// 获取时的唯一标识 
        /// </summary>
        public string author { get; set; }

        /// <summary>
        /// 图文消息的标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 在图文消息页面点击
        /// “阅读原文”后的页面 
        /// </summary>
        public string content_source_url { get; set; }

        /// <summary>
        /// 图文消息页面的内容，
        /// 支持HTML标签 
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 图文消息的描述
        /// </summary>
        public string digest { get; set; }

        /// <summary>
        /// 是否显示封面，
        /// 1为显示，0为不显示 
        /// </summary>
        public int show_cover_pic { get; set; }
    }

}
