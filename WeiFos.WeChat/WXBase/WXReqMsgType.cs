using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.WeChat.WXBase
{
    /// <summary>
    /// 微信请求消息类型 实体对象
    /// @author yewei 
    /// @date 2013-11-04
    /// </summary>
    public static class WXReqMsgType
    {
        /// <summary>
        /// 文本消息
        /// </summary>
        public const string text = "text";

        /// <summary>
        /// 图片消息
        /// </summary>
        public const string image = "image";

        /// <summary>
        /// 声音消息
        /// </summary>
        public const string voice = "voice";

        /// <summary>
        /// 视频消息
        /// </summary>
        public const string video = "video";

        /// <summary>
        /// 地理位置消息
        /// </summary>
        public const string location = "location";

        /// <summary>
        /// 链接消息
        /// </summary>
        public const string url = "url";

        /// <summary>
        /// 事件推送
        /// </summary>
        public const string wxevent = "event";

        /// <summary>
        /// 扫码推事件的事件推送
        /// </summary>
        public const string scancode_push = "scancode_push";

        ///扫码推事件且弹出“消息接收中”提示框的事件推送 
        public const string scancode_waitmsg = "scancode_waitmsg";
    }


}
