using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.WeChat.EnumModule
{

    /// <summary>
    /// 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
    /// Copyright (c) 2013-2018 深圳微狐信息技术有限公司
    /// 创 建：叶委
    /// 日 期：2019-03-15 14:32:37
    /// 描 述：公众号接口授权参数key
    /// </summary>
    public enum WeChatAuthKey
    {


        #region 通用参数部分

        /// <summary>
        /// 微信公众号平台接口调用凭证token（与网页授权接口凭证区分开）、
        /// 微信公众号开放平台组件服务开发方的component_access_token（授权方URL地址接入成功后，每隔10分钟推动WXOpenTicket到该接服务器里面，然后通过该ticket换取component_access_token）
        /// 微信公小程序全局唯一后台接口调用凭据,有效期目前为 2 个小时，需定时刷新，重复获取将导致上次获取的 access_token 失效，建议开发者使用中控服务器统一获取和刷新 access_token
        /// </summary>
        access_token,

        /// <summary>
        /// 微信公众号开放平台组件jsApi_ticket、
        /// 微信公众号平台jsApi_ticket、
        /// 微信企业号jsApi_ticket
        /// </summary> 
        jsApi_ticket,

        /// <summary>
        /// 发票获取授权页ticket
        /// </summary> 
        jsApi_ticket_invoice,

        /// <summary>
        /// 主要用微信登录鉴权
        /// 微信公众号网页授权access_token,可以用于获取用户信息（普通网页未关注情况下）
        /// 微信公众号开放平台access_token ,可以用于获取用户信息（未关注情况下）
        /// 这里通过code换取的是一个特殊的网页授权access_token
        /// </summary> 
        auth_code_access_token,

        /// <summary>
        /// 主要用微信登录鉴权
        /// 微信公众号，用于刷新通过code换取的access_token
        /// 微信公众号开放平台，用于刷新通过code换取的access_token 
        /// </summary>
        auth_code_rfaccess_token,

        #endregion


        #region 微信公众号开放平台API接入

        /// <summary>
        /// 10分钟会推送一次的component_verify_ticket
        /// </summary>
        open_cmpt_verify_ticket,

        /// <summary>
        /// 预授权码
        /// </summary> 
        open_pre_auth_code,

        /// <summary>
        /// authorization_code
        /// 授权code,会在授权成功时返回给第三方平台，详见第三方平台授权流程说明
        /// </summary> 
        open_auth_code,

        /// <summary>
        /// 授权信息
        /// </summary> 
        open_auth_info,

        /// <summary>
        /// 刷新令牌，有效期，为2小时
        /// 以前叫 auth_refresh_token,后更新为open_refresh_token，用户刷新开放平台接口调用令牌
        /// </summary> 
        open_refresh_token,

        #endregion


    }
}

