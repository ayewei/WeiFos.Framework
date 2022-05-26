using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data;
using WeiFos.Entity.WeChatModule;

using WeiFos.WeChat.Helper;

namespace WeiFos.Service.WeChatModule
{
    /// <summary>
    /// 微信JS票据 Service
    /// @author yewei 
    /// @date 2015-09-16
    /// </summary>
    public class WXJsApiTicketCacheService : BaseService<WXJsApiTicketCache>
    {
        /// <summary>
        /// 获取js
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public WXJsApiTicketCache Get(string access_token)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                WXJsApiTicketCache jsapiTicketCache = s.Get<WXJsApiTicketCache>(" where id != 0 ");
                if (jsapiTicketCache == null || jsapiTicketCache.expires_time < DateTime.Now)
                {
                    WeiFos.WeChat.TickeModule.JsApiTicket jsapi_ticket = WeChatJsApiHelper.GetJsApiTicket(access_token);
                    jsapi_ticket.expires_in -= 600; //提前600秒更新token
                    if (jsapiTicketCache == null)
                    {
                        jsapiTicketCache = new WXJsApiTicketCache();
                        jsapiTicketCache.ticket = jsapi_ticket.ticket;
                        jsapiTicketCache.expires_time = (DateTime)DateTime.Now.AddSeconds(jsapi_ticket.expires_in);
                        s.Insert<WXJsApiTicketCache>(jsapiTicketCache);
                    }
                    else
                    {
                        jsapiTicketCache.ticket = jsapi_ticket.ticket;
                        jsapiTicketCache.expires_time = (DateTime)DateTime.Now.AddSeconds(jsapi_ticket.expires_in);
                        s.Update<WXJsApiTicketCache>(jsapiTicketCache);
                    }
                }
                return jsapiTicketCache;
            }
        }



    }
}
