using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.Entity.ReplyModule
{
    /// <summary>
    /// 业务类型 实体类
    /// @author arvin 
    /// @date 2013-10-15
    /// </summary>
    public static class KeyWordBizType
    {
        /// <summary>
        /// 微官网 优先级别 -3
        /// </summary>
        public const string WebSite = "WebSite";

        /// <summary>
        /// 微活动 优先级别 -1
        /// </summary>
        public const string Activity = "Activity";

        /// <summary>
        /// 文本回复 优先级别 -4
        /// </summary>
        public const string TextReply = "TextReply";

        /// <summary>
        /// 图文回复 优先级别 -2
        /// </summary>
        public const string ImgTextReply = "ImgTextReply";

        /// <summary>
        /// 微相册（相册）
        /// </summary>
        public const string Album = "Album";

        /// <summary>
        /// 微留言
        /// </summary>
        public const string Message = "Message";

        /// <summary>
        /// 微预约
        /// </summary>
        public const string WeiResv = "WeiResv";

        /// <summary>
        /// 微会员卡
        /// </summary>
        public const string MemberCard = "MemberCard";

    }


}
