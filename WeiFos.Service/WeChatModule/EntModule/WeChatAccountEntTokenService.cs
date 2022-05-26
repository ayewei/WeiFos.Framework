using WeiFos.Entity.WeChatModule.EntModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data;
using WeiFos.WeChat.Helper;
using WeiFos.WeChat.Helper.WeChatEnt;
using WeiFos.WeChat.TickeModule;
using WeiFos.Service;

namespace Solution.Service.WeChatModule.EntModule
{
    /// <summary>
    /// 企业号Token信息 Service
    /// @author yewei 
    /// @date 2019-03-15
    /// </summary>
    public class WeChatAccountEntTokenService : BaseService<WeChatAccountEntToken>
    {

        /// <summary>
        /// 获取企业授权token
        /// </summary>
        /// <param name="corpid"></param>
        /// <param name="corpsecret"></param>
        /// <returns></returns>
        public WeChatAccountEntToken Get(string corpid, string corpsecret)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                WeChatAccountEntToken tokenCache = s.Get<WeChatAccountEntToken>(" where id != 0 ");
                if (tokenCache == null || tokenCache.expires_time < DateTime.Now)
                {
                    AccessToken accessToken = WeChatEntHelper.GetAccessToken(corpid, corpsecret);
                    if (accessToken.errcode == 0)
                    {
                        accessToken.expires_in -= 600;
                        if (tokenCache == null)
                        {
                            tokenCache = new WeChatAccountEntToken();
                            tokenCache.access_token = accessToken.access_token;
                            tokenCache.expires_time = DateTime.Now.AddSeconds(accessToken.expires_in);
                            s.Insert(tokenCache);
                        }
                        else
                        {
                            tokenCache.access_token = accessToken.access_token;
                            tokenCache.expires_time = DateTime.Now.AddSeconds(accessToken.expires_in);
                            s.Update(tokenCache);
                        }
                    }
                    else
                    {
                        throw new Exception(accessToken.errmsg);
                    }
                }
                return tokenCache;
            }
        }




    }
}
