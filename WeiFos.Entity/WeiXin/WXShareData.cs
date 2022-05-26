using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.Entity.WeiXin
{
    /// <summary>
    /// 微信分享按钮
    /// </summary>
    [Serializable]
    public class WXShareData
    {
        /// <summary>
        /// 分享图片地址
        /// </summary>
        public string imgUrl { get; set; }

        /// <summary>
        /// 页面链接地址
        /// </summary>
        public string timeLineLink { get; set; }

        /// <summary>
        /// 页面链接地址
        /// </summary>
        public string sendFriendLink { get; set; }

        /// <summary>
        /// 微博链接
        /// </summary>
        public string weiboLink { get; set; }

        /// <summary>
        /// 分享标题
        /// </summary>
        public string tTitle { get; set; }

        /// <summary>
        /// 分享标题内容
        /// </summary>
        public string tContent { get; set; }

        /// <summary>
        /// 分享到微信标题
        /// </summary>
        public string fTitle { get; set; }

        /// <summary>
        /// 分享到微信标题
        /// </summary>
        public string fContent { get; set; }

        /// <summary>
        /// 分享到微信内容
        /// </summary>
        public string wContent { get; set; }
    }

}
