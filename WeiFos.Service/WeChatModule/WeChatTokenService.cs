using System;
using WeiFos.Entity.WeChatModule;
using WeiFos.ORM.Data;
using WeiFos.WeChat.Helper;
using WeiFos.WeChat.TickeModule;
 

namespace WeiFos.Service.WeChatModule
{

    /// <summary>
    /// 微信Token
    /// @author yewei
    /// @date 2015-03-24
    /// </summary>
    public class WeChatTokenService : BaseService<WXAccessTokenCache>
    {

        /// <summary>
        /// 获取全局唯一票据
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="appsecret"></param>
        /// <returns></returns>
        public WXAccessTokenCache AccessToken(string appid, string appsecret)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                WXAccessTokenCache token = s.Get<WXAccessTokenCache>(" where type = @0 and id != @1 ", "access_token", 0);
                if (token == null || token.expires_time <= DateTime.Now)
                {
                    if (token == null) token = new WXAccessTokenCache();

                    //从API获取access_token
                    AccessToken access_token = WeChatBaseHelper.GetAccessToken(appid, appsecret);

                    //access_token的有效期目前为2个小时
                    token.expires_time = (DateTime)DateTime.Now.AddSeconds(access_token.expires_in -= 600);
                    token.token = access_token.access_token;
                    token.type = "access_token";

                    if (token.id == 0) s.Insert<WXAccessTokenCache>(token);
                    else s.Update<WXAccessTokenCache>(token);
                }
                return token;
            }
        }

        /// <summary>
        /// 获取全局JS唯一票据
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public WXAccessTokenCache ApiTicket(string access_token)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                WXAccessTokenCache ticket = s.Get<WXAccessTokenCache>(" where type = @0 and id != @1 ", "jsapi_ticket", 0);
                if (ticket == null || ticket.expires_time < DateTime.Now)
                {
                    if (ticket == null) ticket = new WXAccessTokenCache();

                    //从API获取jsapi_ticket
                    JsApiTicket jsapi_ticket = WeChatJsApiHelper.GetJsApiTicket(access_token);

                    ticket.type = "jsapi_ticket";
                    ticket.token = jsapi_ticket.ticket;
                    //提前600秒更新token
                    ticket.expires_time = (DateTime)DateTime.Now.AddSeconds(jsapi_ticket.expires_in -= 600);

                    if (ticket == null) s.Insert<WXAccessTokenCache>(ticket);
                    else s.Update<WXAccessTokenCache>(ticket);
                }
                return ticket;
            }
        }

    }
}
