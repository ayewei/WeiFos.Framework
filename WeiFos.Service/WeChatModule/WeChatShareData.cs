using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.Service.WeChatModule
{
    /// <summary>
    /// 微信分享按钮
    /// </summary>
    [Serializable]
    public class WeChatShareData
    {

        /// <summary>
        /// 分享图片地址
        /// </summary>
        public string img_url { get; set; }

        /// <summary>
        /// 页面链接地址
        /// </summary>
        public string link_url { get; set; } 

        /// <summary>
        /// 分享标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 分享标题内容
        /// </summary>
        public string desc { get; set; }   

    }
}
