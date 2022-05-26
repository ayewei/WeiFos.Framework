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
    /// 微信开放平台接口帮助类
    /// @Author yewei 
    /// @Date 2018-04-13
    /// </summary>
    public class WeChatOpenHelper
    {


        #region 微信公众号托管授权模块


        /// <summary>
        /// 获取公众号授权地址
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="pre_auth_code"></param>
        /// <param name="redirect_uri"></param>
        /// <returns></returns>
        public static string GetOpenOuthUrl(string appid, string pre_auth_code, string redirect_uri)
        {
            string oauthURL = "https://mp.weixin.qq.com/cgi-bin/componentloginpage?component_appid={0}&pre_auth_code={1}&redirect_uri={2}";
            return string.Format(oauthURL, appid, pre_auth_code, System.Web.HttpUtility.UrlEncode(redirect_uri).Replace("+", "%20"));
        }



        /// <summary>
        /// 获取授权方的帐号基本信息
        /// (也就是获取授权的公众号信息)
        /// </summary>
        /// <param name="component_access_token"></param>
        /// <param name="component_appid"></param>
        /// <param name="authorizer_appid"></param>
        /// <returns></returns>
        public static WXOpenAuthorizer GetAuthorizerInfo(string component_access_token, string component_appid, string authorizer_appid)
        {
            //请求的URL
            string posturl = string.Format("https://api.weixin.qq.com/cgi-bin/component/api_get_authorizer_info?component_access_token={0}", component_access_token);

            //请求参数 （第三方平台appid：component_appid，授权方appid：authorizer_appid）
            string postData = JsonConvert.SerializeObject(new { component_appid, authorizer_appid });

            //发送请求返回结果
            return JsonConvert.DeserializeObject<WXOpenAuthorizer>(HttpClientHelper.PostURL(posturl, postData));
        }

        #endregion


        #region 获取微信用户基本信息(第三方应用程序)

        /// <summary>
        /// 获取微信用户基本信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openid">普通用户的标识，对当前公众号唯一 </param>
        /// <returns></returns>
        public static string GetWeChatUserInfo(string accessToken, string openid)
        {
            string posturl = " https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN";
            return HttpClientHelper.GetURL(string.Format(posturl, accessToken, openid));
        }

        #endregion


        #region 微信OAuth2.0鉴权,网页授权获取用户信息,会弹出授权页面

        /// <summary>
        /// 获取授权连接
        /// </summary>
        /// <param name="redirect_uri">重定向地址，需要urlencode，这里填写的应是服务开发方的回调地址</param>
        /// <param name="appid">公众号的appid</param>
        /// <param name="scope">授权作用域，拥有多个作用域用逗号（,）分隔</param>
        /// <param name="state">重定向后会带上state参数，开发者可以填写任意参数值，最多128字节</param>
        /// <param name="component_appid">服务方的appid，在申请创建公众号服务成功后，可在公众号服务详情页找到</param>
        /// <param name="responseType">默认为填code</param>
        /// <returns>URL</returns>
        public static string SnsComponentApiUserInfo(string redirect_uri, string appid, string state, string component_appid)
        {
            string oauthURL = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state={2}&component_appid={3}#wechat_redirect";
            return string.Format(oauthURL, appid, System.Web.HttpUtility.UrlEncode(redirect_uri).Replace("+", "%20"), state, component_appid);
        }

        #endregion


        #region 第三方平台托管的微信公众号通过code换取网页授权access_token

        /// <summary>
        /// 第三方平台托管的微信公众号通过code换取网页授权access_token
        /// </summary>
        /// <param name="code"></param>
        /// <param name="appid">公众号appid</param>
        /// <param name="component_appid">开放平台应用id</param>
        /// <param name="component_access_token">第一步得到的【component_access_token】</param>
        /// <returns></returns>
        public static WXOpenAuthAccessToken GetComponentOAuthToken(string code, string appid, string component_appid, string component_access_token)
        {
            string url = "https://api.weixin.qq.com/sns/oauth2/component/access_token?appid={0}&code={1}&grant_type=authorization_code&component_appid={2}&component_access_token={3}";
            string result = HttpClientHelper.HttpGet(string.Format(url, appid, code, component_appid, component_access_token));
            return JsonConvert.DeserializeObject<WXOpenAuthAccessToken>(result);
        }


        /// <summary>
        /// 第三方平台托管的微信公众号 刷新 access_token
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="cmpt_appid"></param>
        /// <param name="cmpt_token"></param>
        /// <param name="cmpt_refresh_token"></param>
        /// <returns></returns>
        public static WXOpenAuthAccessToken GetComponentOAuthRefreshToken(string appid, string cmpt_appid, string cmpt_token, string cmpt_refresh_token)
        {
            string url = "https://api.weixin.qq.com/sns/oauth2/component/refresh_token?appid={0}&grant_type=refresh_token&component_appid={1}&component_access_token={2}&refresh_token={3}";
            string result = HttpClientHelper.HttpGet(string.Format(url, appid, cmpt_appid, cmpt_token, cmpt_refresh_token));
            return JsonConvert.DeserializeObject<WXOpenAuthAccessToken>(result);
        }

        #endregion


        #region 获取令牌（component_access_token）


        /// <summary>
        /// 获取第三方平台component_access_token令牌
        /// 第一步：获取compoment_access_token
        /// 在第三方应用通过审核后，微信服务器每隔10分钟向第三方的接收地址推送一次component_verify_ticket，用于第三方接口 调用凭据。
        /// 第三方平台通过自己的component_appid（即在微信开放平台管理中心的第三方平台详情页中的AppID和AppSecret）和component_appsecret，
        /// 以及component_verify_ticket来获取自己的接口调用凭据（component_access_token）
        /// <param name="component_appid">第三方平台appid</param>
        /// <param name="component_appsecret">第三方平台appsecret</param>
        /// <param name="component_verify_ticket">微信后台推送的ticket，此ticket会定时推送</param>
        /// 使用规则调用前需要判断缓存的令牌是否过期 没有过期直接用缓存的令牌
        /// </summary>
        public static WXOpenCmptAccessToken GetComponentAccessToken(string component_appid, string component_appsecret, string component_verify_ticket)
        {
            string postData = JsonConvert.SerializeObject(new { component_appid, component_appsecret, component_verify_ticket });
            string result = HttpClientHelper.PostURL("https://api.weixin.qq.com/cgi-bin/component/api_component_token", postData);
            return JsonConvert.DeserializeObject<WXOpenCmptAccessToken>(result);
        }


        #endregion


        #region 获取预授权码



        /// <summary>
        /// 获取预授权码
        /// </summary>
        /// <param name="component_access_token"></param>
        /// <param name="component_appid"></param>
        /// <returns></returns>
        public static WXOpendPreAuthCode GetOpenPreAuthCode(string component_access_token, string component_appid)
        {
            //请求的URL
            string posturl = string.Format("https://api.weixin.qq.com/cgi-bin/component/api_create_preauthcode?component_access_token={0}", component_access_token);
          
            //发送请求返回结果
            string result = HttpHelper.Post(posturl, "{\"component_appid\":\"" + component_appid + "\"}");

            WXOpendPreAuthCode auth_code = JsonConvert.DeserializeObject<WXOpendPreAuthCode>(result);

            return auth_code;
        }


        #endregion


        #region 获取授权权限


        /// <summary>
        /// 使用授权码换取公众号的接口调用凭据和授权信息
        /// </summary>
        /// <param name="component_access_token"></param>
        /// <param name="component_appid"></param>
        /// <param name="authorization_code"></param>
        /// <returns></returns>
        public static WXOpenAuthFun GetAuthorizerAccessToken(string component_access_token, string component_appid, string authorization_code)
        {
            //请求的URL
            string posturl = string.Format("https://api.weixin.qq.com/cgi-bin/component/api_query_auth?component_access_token={0}", component_access_token);
            //请求的参数
            string postData = JsonConvert.SerializeObject(new { component_appid, authorization_code });
            //发送请求返回结果
            return JsonConvert.DeserializeObject<WXOpenAuthFun>(HttpHelper.Post(posturl, postData));
        }

        #endregion



        #region 获取（刷新）授权公众号的接口调用凭据（令牌）


        /// <summary>
        /// 获取（刷新）授权公众号或小程序的接口调用凭据（令牌）
        /// </summary>
        /// <param name="component_access_token"></param>
        /// <param name="component_appid"></param>
        /// <param name="authorizer_appid"></param>
        /// <param name="authorizer_refresh_token"></param>
        /// <returns></returns>
        public static WXOpenRefreshToken RefreshAuthorizerRefreshToken(string component_access_token, string component_appid, string authorizer_appid, string authorizer_refresh_token)
        {
            //请求的URL
            string posturl = string.Format("https://api.weixin.qq.com/cgi-bin/component/api_authorizer_token?component_access_token={0}", component_access_token);
            //请求的参数
            string postData = JsonConvert.SerializeObject(new { component_appid, authorizer_appid, authorizer_refresh_token });
            //发送请求返回结果
            return JsonConvert.DeserializeObject<WXOpenRefreshToken>(HttpClientHelper.PostURL(posturl, postData));
        }


        #endregion



    }
}
