using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.Core.EnumHelper;

namespace WeiFos.Entity.WeChatModule.WXOpen
{

    /// <summary>
    /// 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
    /// Copyright (c) 2013-2018 深圳微狐信息技术有限公司
    /// 创 建：叶委
    /// 日 期：2019-03-15 14:32:37
    /// 描 述：公众号接口授权配细表
    /// </summary>
    public enum WeChatAuthType
    {

        /// <summary>
        /// 配置公众号内部服务器API方式
        /// </summary>
        WeChatAuth = 1,

        /// <summary>
        /// 公众号授权模式
        /// </summary>
        WeChatOpenAuth = 2,

        /// <summary>
        /// 企业号内部api开发模式
        /// </summary>
        WeChatEntAuth = 3,

        /// <summary>
        /// 企业号应用授权模式
        /// </summary>
        WeChatEntAuthAgent = 4,

        /// <summary>
        /// 企业号授权模式
        /// </summary>
        WeChatEntOpen = 5

    }
}
