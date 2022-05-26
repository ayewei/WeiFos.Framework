using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.Core;
using WeiFos.WeChat.Models;
using WeiFos.WeChat.WXBase;
using WeiFos.WeChat.WXBase.WXOpen;

namespace WeiFos.WeChat.Helper
{
    /// <summary>
    /// 微信用户管理接口帮助类
    /// </summary>
    public class WeChatUserHelper
    {

        #region 微信OAuth2.0鉴权

        /// <summary>
        /// http://www.cnblogs.com/txw1958/p/weixin71-oauth20.html
        /// 应用授权作用域,不弹出授权页面，直接跳转，只能获取用户openid
        /// </summary>
        /// <param name="redirect_uri">授权后重定向的回调链接地址</param>
        /// <param name="appid">公众号的唯一标识</param>
        /// <param name="state">重定向后会带上state参数，开发者可以填写任意参数值</param>
        /// <returns></returns>
        public static string SnsApiBase(string redirect_uri, string appid, string state)
        {
            string oauthURL = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state={2}#wechat_redirect";
            return string.Format(oauthURL, appid, System.Web.HttpUtility.UrlEncode(redirect_uri).Replace("+", "%20"), state).Replace("+", "%20");
        }

        /// <summary>
        /// 微信OAuth2.0鉴权,网页授权获取用户信息,会弹出授权页面
        /// 未关注公众号情况下获取用户详细信息情况下
        /// </summary>
        /// <returns></returns>
        public static string SnsApiUserInfo(string redirect_uri, string appid, string state)
        {
            string oauthURL = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state={2}#wechat_redirect";
            return string.Format(oauthURL, appid, System.Web.HttpUtility.UrlEncode(redirect_uri).Replace("+", "%20"));
        }

        #endregion

        #region 微信OAuth2.0授权登录
        /// <summary>
        /// 1. 第三方发起微信授权登录请求，微信用户允许授权第三方应用后，微信会拉起应用或重定向到第三方网站，并且带上授权临时票据code参数；
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        public static string SnsApiLogin(string redirect_uri, string appid)
        {
            string oauthURL = "https://open.weixin.qq.com/connect/qrconnect?appid=APPID&redirect_uri=REDIRECT_URI&response_type=code&scope=SCOPE&state=STATE#wechat_redirect";
            return string.Format(oauthURL, appid, System.Web.HttpUtility.UrlEncode(redirect_uri).Replace("+", "%20"));
        }
        #endregion

        #region 通过code换取网页授权access_token

        public static string GetOAuthToken(string Code, string AppId, string AppSecret)
        {
            return HttpClientHelper.HttpGet("https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + AppId + "&secret=" + AppSecret + "&code=" + Code + "&grant_type=authorization_code");
        }

        #endregion

        #region 刷新access_token

        public static string GetOAuthRefreshToken(string refresh_token, string AppId)
        { 
            return HttpClientHelper.HttpGet("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid=" + AppId + "&grant_type=refresh_token&refresh_token=" + refresh_token);
        }

        #endregion

        #region 网页授权获取用户基本信息(需OAuth2.0鉴权的scope为 snsapi_userinfo),此接口只要用户同意授权后则无须关注就可获取该用户的基本信息

        /// <summary>
        /// 通过网页授权获取用户基本信息(需OAuth2.0鉴权的scope为snsapi_userinfo),此接口只要用户同意授权后则无须关注就可获取该用户的基本信息
        /// </summary>
        /// <param name="ACCESS_TOKEN">网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同</param>
        /// <param name="OPENID">用户的唯一标识</param>
        /// <param name="lang">返回国家地区语言版本，zh_CN 简体，zh_TW 繁体，en 英语,为空默认zh_CN</param>
        /// <returns>返回用户基本信息Json数据包</returns>
        public static string GetUserInfoJsonBySnsApiUserInfo(string access_token, string openid, string lang)
        {
            lang = string.IsNullOrEmpty(lang) ? "zh_CN" : lang;
            string response = HttpClientHelper.HttpGet("https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token + "&openid=" + openid + "&lang=" + lang);
            return response;
        }

        /// <summary>
        /// 通过网页授权获取用户基本信息(需OAuth2.0鉴权的scope为snsapi_userinfo),此接口只要用户同意授权后则无须关注就可获取该用户的基本信息
        /// </summary>
        /// <param name="ACCESS_TOKEN">网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同</param>
        /// <param name="OPENID">用户的唯一标识</param>
        /// <param name="lang">返回国家地区语言版本，zh_CN 简体，zh_TW 繁体，en 英语,为空默认zh_CN</param>
        /// <returns>返回用户基本信息对象</returns>
        public static UserBaseInfo GetUserInfoBySnsApiUserInfo(string access_token, string openid, string lang)
        {
            string response = GetUserInfoJsonBySnsApiUserInfo(access_token, openid, lang);
            UserBaseInfo info =JsonConvert.DeserializeObject<UserBaseInfo>(response);
            return info;
        }

        #endregion

        #region 获取用户基本信息(包括UnionID机制),此接口需要用户关注了公众号后才获取该用户基本信息
        /// <summary>
        /// 获取用户基本信息(包括UnionID机制),此接口需要用户关注了公众号后才获取该用户基本信息
        /// </summary>
        /// <param name="access_token">调用接口凭证</param>
        /// <param name="openid">普通用户的标识，对当前公众号唯一</param>
        /// <param name="lang">返回国家地区语言版本，zh_CN 简体，zh_TW 繁体，en 英语,非必填,为空默认zh_CN</param>
        /// <returns>返回用户基本信息JSON数据包</returns>
        public static string GetUserBaseInfoJson(string access_token, string openid, string lang)
        {
            lang = string.IsNullOrEmpty(lang) ? "zh_CN" : lang;
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang={2}", access_token, openid, lang);
            string response = HttpClientHelper.HttpGet(url);
            return response;
        }

        /// <summary>
        /// 获取用户基本信息(包括UnionID机制),此接口需要用户关注了公众号后才获取该用户基本信息
        /// </summary>
        /// <param name="access_token">调用接口凭证</param>
        /// <param name="openid">普通用户的标识，对当前公众号唯一</param>
        /// <param name="lang">返回国家地区语言版本，zh_CN 简体，zh_TW 繁体，en 英语,非必填,为空默认zh_CN</param>
        /// <returns>返回用户基本信息对象</returns>
        public static UserBaseInfo GetUserBaseInfo(string access_token, string openid, string lang)
        {
            string response = GetUserBaseInfoJson(access_token, openid, lang);
            UserBaseInfo info = JsonConvert.DeserializeObject<UserBaseInfo>(response);
            return info;
        }
        #endregion

        #region 检验授权凭证（access_token）是否有效

        public static string check(string ACCESS_TOKEN, string OPENID)
        {
            return HttpClientHelper.HttpGet("https://api.weixin.qq.com/sns/auth?access_token=" + ACCESS_TOKEN + "&openid=" + OPENID);
        }

        #endregion



        /// <summary>
        /// 关注者列表由一串OpenID（加密后的微信号，每个用户对每个公众号的OpenID是唯一的）组成。
        /// 一次拉取调用最多拉取10000个关注者的OpenID，可以通过多次拉取的方式来满足需求。 
        /// </summary>
        /// <param name="redirect_uri"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        public static string GetWXUserList(string accessToken, string next_openid)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}&next_openid={1}";
            url = string.Format(url, accessToken, next_openid);
            return HttpClientHelper.GetURL(url);
        }

        /// <summary>
        /// 获取公众号所有用户组
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static string GetWxGroups(string access_token)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/groups/get?access_token={0}", access_token);
            return HttpClientHelper.GetURL(url);
        }


        /// <summary>
        /// 获取微信用户基本信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openid">普通用户的标识，对当前公众号唯一 </param>
        /// <returns></returns>
        public static string GetWXUserInfo(string accessToken, string openid)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN";
            url = string.Format(url, accessToken, openid);
            return HttpClientHelper.GetURL(url);
        }
    

    
    }
}
