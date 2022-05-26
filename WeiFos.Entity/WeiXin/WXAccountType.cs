using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.Entity.WeiXin
{
    /// <summary>
    /// 微信公众号类型
    /// @author arvin 
    /// @date 2014-02-11
    /// </summary>
    public static class WXAccountType
    {
        /// <summary>
        /// 订阅号
        /// </summary>
        public const int Subscription = 1;

        /// <summary>
        /// 认证订阅号
        /// </summary>
        public const int Verify_Subscription = 2;

        /// <summary>
        /// 服务号
        /// </summary>
        public const int Service = 3;

        /// <summary>
        /// 认证服务号
        /// </summary>
        public const int Verify_Service = 4;

    }
}
