using WeiFos.Entity.WeChatModule.WXOpen; 
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data;
using WeiFos.WeChat.Helper;
using WeiFos.WeChat.WXBase.WXOpen;

namespace WeiFos.Service.WeChatModule.WXOpen
{

    /// <summary>
    /// 开放平台授权公众号 Service
    /// @author yewei 
    /// @date 2018-04-13
    /// </summary>
    public class WXOpenAccountService : BaseService<WXOpenAccount>
    {

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public WXOpenAccount Get()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<WXOpenAccount>("where id != @0", 0);
            }
        }



        /// <summary>
        /// 授权公众号信息
        /// </summary>
        /// <param name="component_access_token"></param>
        /// <param name="component_appid"></param>
        /// <param name="authorizer_appid"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public WXOpenAccount AuthWeChatAccount(long user_id, string component_access_token, string component_appid, string authorizer_appid)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //获取授权信息
                WXOpenAuthorizer auth_info = WeChatOpenHelper.GetAuthorizerInfo(component_access_token, component_appid, authorizer_appid);

                //当前公众号信息
                WXOpenAccount account = s.Get<WXOpenAccount>("where id != @0 ", 0);
                if (account == null) account = new WXOpenAccount();

                //微信公众号APPID
                account.auth_appid = auth_info.authorization_info.authorization_appid ?? auth_info.authorization_info.authorizer_appid;
                //微信号
                account.wechat_no = auth_info.authorizer_info.alias;
                //微信昵称
                account.nick_name = auth_info.authorizer_info.nick_name;
                //公众号原始ID
                account.original_id = auth_info.authorizer_info.user_name;
                //公众号主体
                account.principal_name = auth_info.authorizer_info.principal_name;
                //二维码地址
                account.qrcode_url = auth_info.authorizer_info.qrcode_url;
                //公众号图像
                account.head_img = auth_info.authorizer_info.head_img;

                //授权方公众号类型，0代表订阅号，1代表由历史老帐号升级后的订阅号，2代表服务号
                account.type = auth_info.authorizer_info.service_type_info.id;
                if (auth_info.authorizer_info.service_type_info.id == 2)
                {
                    account.type = 4;
                }
                account.created_user_id = user_id;
                account.created_date = DateTime.Now;

                if (account.id == 0) s.Insert(account);
                else
                {
                    account.updated_date = DateTime.Now;
                    s.Update(account);
                }
                return account;
            }
        }


         




    }
}
