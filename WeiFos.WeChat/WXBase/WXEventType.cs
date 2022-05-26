using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.WeChat.WXBase
{
    /// <summary>
    /// 微信事件类型 实体对象
    /// @author yewei 
    /// @date 2014-12-25
    /// </summary>
    public class WXEventType
    {
        /// <summary>
        /// 订阅
        /// </summary>
        public const string subscribe = "subscribe";

        /// <summary>
        /// 取消订阅
        /// </summary>
        public const string unsubscribe = "unsubscribe";

        /// <summary>
        /// 菜单点击
        /// </summary>
        public const string click = "click";

        /// <summary>
        /// 用户扫描二维码
        /// </summary>
        public const string scan = "scan";

        /// <summary>
        /// 群发任务提回调推送事件
        /// </summary>
        public const string masssendjobfinish = "masssendjobfinish";

    }
}
