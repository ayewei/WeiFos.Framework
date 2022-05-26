using WeiFos.Entity.LogsModule;
using WeiFos.Entity.WeChatModule.WXOpen; 
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Core.XmlHelper;
using WeiFos.ORM.Data;
using WeiFos.WeChat.Helper;
using WeiFos.WeChat.TickeModule;
using WeiFos.WeChat.WXBase;
using WeiFos.WeChat.WXBase.WXOpen;
using WeiFos.Service;

namespace WeiFos.Service.WeChatModule.WXOpen
{

    /// <summary>
    /// 开放平台授权参数核心 Service
    /// @author yewei 
    /// @date 2018-04-13
    /// </summary>
    public class WXOpenAuthService : BaseService<WXOpenAuth>
    {


        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public WXOpenAuth GetByKey(string key)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<WXOpenAuth>("where auth_key = @0 ", key);
            }
        }


        #region 微信开放平台10分钟推送更新凭据方法

        /// <summary>
        /// 更新凭据
        /// </summary>
        /// <param name="store_id"></param>
        /// <param name="verify_ticket"></param>
        public void UpdateTicket(string verify_ticket)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //当前凭据参数是否存在
                WXOpenAuth entity = s.Get<WXOpenAuth>("where auth_key = @0 ", WXOpenAuthKey.cmpt_verify_ticket.ToString());
                if (entity == null) entity = new WXOpenAuth();

                entity.update_time = DateTime.Now;
                entity.auth_key = WXOpenAuthKey.cmpt_verify_ticket.ToString();
                entity.val = verify_ticket;
                if (entity.id == 0)
                    s.Insert(entity);
                else
                    s.Update(entity);
            }
        }

        #endregion


        #region 获取微信开放平台新凭据方法


        /// <summary>
        /// 获取推送的调用凭据
        /// </summary>
        /// <returns></returns>
        public WXOpenCmptVerifyTicket GetCmptVerifyTicket()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //当前凭据参数是否存在
                WXOpenAuth entity = s.Get<WXOpenAuth>("where auth_key = @0 ", WXOpenAuthKey.cmpt_verify_ticket.ToString());
                if (entity != null) return XmlConvertHelper.DeserializeObject<WXOpenCmptVerifyTicket>(entity.val);

                return null;
            }
        }

        #endregion


        #region 获取微信开放组件Token


        /// <summary>
        /// 获取 WXOpenCmptAccessToken 
        /// </summary>
        /// <param name="opensetting"></param>
        /// <param name="cmpt_verify_ticket"></param>
        /// <returns></returns>
        public string GetCmptAccessToken(WXOpenSetting opensetting, string cmpt_verify_ticket)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //返回的Token字符串 
                string cmpt_access_token = string.Empty;

                //当前预授权码对象
                WXOpenCmptAccessToken cmptAccessToken = null;

                //当前凭据参数是否存在
                WXOpenAuth entity = s.Get<WXOpenAuth>("where auth_key = @0 ", WXOpenAuthKey.cmpt_access_token.ToString());

                if (entity == null) entity = new WXOpenAuth();
                else
                {
                    cmptAccessToken = JsonConvert.DeserializeObject<WXOpenCmptAccessToken>(entity.val);
                    cmpt_access_token = cmptAccessToken.component_access_token;
                }

                //获取授权票据
                if (entity.id == 0 || (cmptAccessToken != null && entity.update_time.AddSeconds(cmptAccessToken.expires_in) < DateTime.Now))
                {
                    cmptAccessToken = WeChatOpenHelper.GetComponentAccessToken(opensetting.component_appid, opensetting.component_appsecret, cmpt_verify_ticket);
                    if (cmptAccessToken.errcode != 0)
                    {
                        string er = "获取微信开放组件Token，方法[GetCmptAccessToken]==>" + JsonConvert.SerializeObject(cmptAccessToken);
                        s.Insert(new APILogs() { type = 1, created_date = DateTime.Now, content = er });
                    }
                    else
                    {
                        //提前10分钟刷新
                        cmptAccessToken.expires_in -= 600;
                        entity.update_time = DateTime.Now;
                        entity.auth_key = WXOpenAuthKey.cmpt_access_token.ToString();
                        entity.val = JsonConvert.SerializeObject(cmptAccessToken);

                        cmpt_access_token = cmptAccessToken.component_access_token;

                        if (entity.id == 0) s.Insert(entity);
                        else s.Update(entity);
                    }
                }

                return cmpt_access_token;
            }
        }

        #endregion


        #region 获取微信授权码


        /// <summary>
        /// 获取预授权码
        /// </summary>
        /// <param name="component_access_token"></param>
        /// <param name="component_appid"></param>
        /// <returns></returns>
        public string GetOpenPreAuthCode(string component_access_token, string component_appid)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //返回的预授权码字符串
                string pre_auth_code = string.Empty;
                //当前预授权码对象
                WXOpendPreAuthCode preAuthCode = null;

                //当前凭据参数是否存在
                WXOpenAuth entity = s.Get<WXOpenAuth>("where auth_key = @0 ", WXOpenAuthKey.pre_auth_code.ToString());
                if (entity == null) entity = new WXOpenAuth();
                else
                {
                    preAuthCode = JsonConvert.DeserializeObject<WXOpendPreAuthCode>(entity.val);
                    pre_auth_code = preAuthCode.pre_auth_code;
                }

                //获取授权票据
                if (entity.id == 0 || (preAuthCode != null && entity.update_time.AddSeconds(preAuthCode.expires_in) < DateTime.Now))
                {
                    preAuthCode = WeChatOpenHelper.GetOpenPreAuthCode(component_access_token, component_appid);
                    if (preAuthCode.errcode != 0)
                    {
                        string er = "获取预授权码错误，方法[GetOpenPreAuthCode]==>" + JsonConvert.SerializeObject(preAuthCode);
                        s.Insert(new APILogs() { type = 1, created_date = DateTime.Now, content = er });
                    }
                    else
                    {
                        //提前2分钟刷新
                        preAuthCode.expires_in -= 120;
                        entity.update_time = DateTime.Now;
                        entity.auth_key = WXOpenAuthKey.pre_auth_code.ToString();
                        entity.val = JsonConvert.SerializeObject(preAuthCode);

                        pre_auth_code = preAuthCode.pre_auth_code;

                        if (entity.id == 0) s.Insert(entity);
                        else s.Update(entity);
                    }
                }

                return pre_auth_code;
            }
        }


        #endregion


        #region 获取微信开放平台授权信息


        /// <summary>
        /// 使用授权码换取公众号或小程序的接口
        /// 调用凭据和授权信息，里面获取到初次获取的Token,以及authorizer_appid
        /// 授权 auth_code_value,会在授权成功时返回给第三方平台，详见第三方平台授权流程说明
        /// </summary>
        /// <param name="component_access_token"></param>
        /// <param name="component_appid"></param>
        /// <param name="auth_code_value"></param>
        /// <returns></returns>
        public WXOpenAuthFun GetAuthInfo(string component_access_token, string component_appid, string auth_code_value)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //返回的预授权码字符串
                string pre_auth_code = string.Empty;
                //当前预授权码对象
                WXOpenAuthFun wxopenAuthFun = null;

                //当前凭据参数是否存在
                WXOpenAuth entity = s.Get<WXOpenAuth>("where auth_key = @0 ", WXOpenAuthKey.auth_info.ToString());
                if (entity == null) entity = new WXOpenAuth();
                else
                {
                    wxopenAuthFun = JsonConvert.DeserializeObject<WXOpenAuthFun>(entity.val);
                }

                //获取授权票据
                if (entity.id == 0 || (wxopenAuthFun != null && entity.update_time.AddSeconds(wxopenAuthFun.authorization_info.expires_in) < DateTime.Now))
                {
                    wxopenAuthFun = WeChatOpenHelper.GetAuthorizerAccessToken(component_access_token, component_appid, auth_code_value);
                    if (wxopenAuthFun.errcode != 0)
                    {
                        string er = "获取预授权码错误，方法[GetAuthInfo]==>" + JsonConvert.SerializeObject(wxopenAuthFun);
                        s.Insert(new APILogs() { type = 1, created_date = DateTime.Now, content = er });
                    }
                    else
                    {
                        //提前10分钟刷新
                        wxopenAuthFun.authorization_info.expires_in -= 600;
                        entity.update_time = DateTime.Now;
                        entity.auth_key = WXOpenAuthKey.auth_info.ToString();
                        entity.val = JsonConvert.SerializeObject(wxopenAuthFun);

                        if (entity.id == 0) s.Insert(entity);
                        else s.Update(entity);
                    }
                }

                return wxopenAuthFun;
            }
        }

        #endregion


        #region 获取微信开放平台 授权方接口调用凭据


        /// <summary>
        /// 授权方接口调用凭据 token，会自动刷新
        /// </summary>
        /// <param name="cmpt_access_token"></param>
        /// <param name="cmpt_appid"></param>
        /// <param name="authorizer_appid"></param>
        /// <param name="auth_refresh_token"></param>
        /// <returns></returns>
        public WXOpenRefreshToken GetAuthAccessToken(string cmpt_access_token, string cmpt_appid, string authorizer_appid)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //当前预授权码对象
                WXOpenRefreshToken wxOpenRefreshToken = null;

                //授权信息，这里一定存在auth_info信息，否则程序数据不完整，里面获取到初次获取的Token,以及authorizer_appid
                WXOpenAuth auth_info = s.Get<WXOpenAuth>("where auth_key = @0 ", WXOpenAuthKey.auth_info.ToString());
                //序列化授权信息
                WXOpenAuthFun wxOpenAuthFun = JsonConvert.DeserializeObject<WXOpenAuthFun>(auth_info.val);
                //第一次授权后如果没过期，则直接返回
                if (auth_info.update_time.AddSeconds(wxOpenAuthFun.authorization_info.expires_in) > DateTime.Now)
                {
                    wxOpenRefreshToken = new WXOpenRefreshToken();
                    wxOpenRefreshToken.authorizer_access_token = wxOpenAuthFun.authorization_info.authorizer_access_token;
                    wxOpenRefreshToken.authorizer_refresh_token = wxOpenAuthFun.authorization_info.authorizer_refresh_token;

                    //则删除刷新的token
                    s.ExcuteUpdate("delete tb_wx_open_auth where auth_key = @0", WXOpenAuthKey.auth_refresh_token.ToString());
                }
                else
                {
                    //刷新的票据
                    WXOpenAuth entity = s.Get<WXOpenAuth>("where auth_key = @0 ", WXOpenAuthKey.auth_refresh_token.ToString());
                    if (entity == null) entity = new WXOpenAuth();
                    else
                    {
                        wxOpenRefreshToken = JsonConvert.DeserializeObject<WXOpenRefreshToken>(entity.val);
                    }

                    //获取授权票据
                    if (entity.id == 0 || (entity != null && entity.update_time.AddSeconds(wxOpenRefreshToken.expires_in) < DateTime.Now))
                    {
                        string token = "";
                        if (entity.id == 0) token = wxOpenAuthFun.authorization_info.authorizer_refresh_token;
                        else token = wxOpenRefreshToken.authorizer_refresh_token;

                        wxOpenRefreshToken = WeChatOpenHelper.RefreshAuthorizerRefreshToken(cmpt_access_token, cmpt_appid, authorizer_appid, token);
                        if (wxOpenRefreshToken.errcode != 0)
                        {
                            string er = "获取授权信息错误，方法[GetAuthAccessToken]==>" + JsonConvert.SerializeObject(wxOpenRefreshToken);
                            s.Insert(new APILogs() { type = 1, created_date = DateTime.Now, content = er }); 
                        }
                        else
                        {
                            //提前10分钟刷新
                            wxOpenRefreshToken.expires_in -= 600;
                            entity.update_time = DateTime.Now;
                            entity.auth_key = WXOpenAuthKey.auth_refresh_token.ToString();
                            entity.val = JsonConvert.SerializeObject(wxOpenRefreshToken);

                            if (entity.id == 0) s.Insert(entity);
                            else s.Update(entity);
                        }
                    }
                }

                return wxOpenRefreshToken;
            }
        }

        #endregion


        #region 获取微信开放平台 授权后接口调用（UnionID）


        #region 获取token缓存数据库

        /// <summary>
        /// 暂时用不上
        /// </summary>
        /// <param name="code"></param>
        /// <param name="appid"></param>
        /// <param name="cmpt_appid"></param>
        /// <param name="cmpt_access_token"></param>
        /// <returns></returns>
        public WXOpenAuthAccessToken GetWXOpenCodeAccessToken1(string code, string appid, string cmpt_appid, string cmpt_access_token)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {

                /// 通过code获取access_token
                WXOpenAuthAccessToken wxOpenAuthAccessToken = null;

                //通过code获取access_token
                WXOpenAuth entity = s.Get<WXOpenAuth>("where auth_key = @0 ", WXOpenAuthKey.auth_code_access_token.ToString());
                if (entity == null) entity = new WXOpenAuth();
                else
                {
                    wxOpenAuthAccessToken = JsonConvert.DeserializeObject<WXOpenAuthAccessToken>(entity.val);
                }

                //未获取或已过期
                if (entity.id == 0 || (entity != null && entity.update_time.AddSeconds(wxOpenAuthAccessToken.expires_in) < DateTime.Now))
                {
                    if (entity.id == 0)
                    {
                        //通过接口获取，用code获取access_token
                        wxOpenAuthAccessToken = WeChatOpenHelper.GetComponentOAuthToken(code, appid, cmpt_appid, cmpt_access_token);
                    }
                    else
                    {
                        //是否已存取刷新的Token
                        WXOpenAuth refresh_entity = s.Get<WXOpenAuth>("where auth_key = @0 ", WXOpenAuthKey.auth_code_refresh_access_token.ToString());

                        //如果未存在刷新的token
                        if (refresh_entity == null)
                        {
                            //通过code获取的refresh_token
                            wxOpenAuthAccessToken = WeChatOpenHelper.GetComponentOAuthRefreshToken(appid, cmpt_appid, cmpt_access_token, wxOpenAuthAccessToken.refresh_token);

                            refresh_entity = new WXOpenAuth();
                            refresh_entity.update_time = DateTime.Now;
                            refresh_entity.auth_key = WXOpenAuthKey.auth_code_refresh_access_token.ToString();
                            refresh_entity.val = JsonConvert.SerializeObject(wxOpenAuthAccessToken);
                            s.Insert(refresh_entity);
                        }
                        else
                        {
                            //此处的 refresh_token存放30天
                            WXOpenAuthAccessToken refreshToken = JsonConvert.DeserializeObject<WXOpenAuthAccessToken>(refresh_entity.val);
                            //通过code获取access_token
                            wxOpenAuthAccessToken = WeChatOpenHelper.GetComponentOAuthRefreshToken(appid, cmpt_appid, cmpt_access_token, refreshToken.refresh_token);
                        }
                    }

                    //获取失败，记录日志
                    if (wxOpenAuthAccessToken.errcode != 0) s.Insert(new APILogs() { type = 1, created_date = DateTime.Now, content = JsonConvert.SerializeObject(wxOpenAuthAccessToken) });
                    else
                    {
                        //提前10分钟刷新
                        wxOpenAuthAccessToken.expires_in -= 600;
                        entity.update_time = DateTime.Now;
                        entity.auth_key = WXOpenAuthKey.auth_code_access_token.ToString();
                        entity.val = JsonConvert.SerializeObject(wxOpenAuthAccessToken);

                        if (entity.id == 0) s.Insert(entity);
                        else s.Update(entity);
                    }
                }

                return wxOpenAuthAccessToken;
            }
        }


        #endregion


        #endregion


        #region 获取微信开放平台 通过access_token获取 jsApiTicket


        /// <summary>
        /// 通过access_token获取 jsApiTicket
        /// </summary>
        /// <param name="token"></param> 
        /// <returns></returns>
        public JsApiTicket GetWXOpenJsApiTicket(string token)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //当前预授权码对象
                JsApiTicket jsApiTicket = null;

                //当前凭据参数是否存在
                WXOpenAuth entity = s.Get<WXOpenAuth>("where auth_key = @0 ", WXOpenAuthKey.wx_open_jsApi_ticket.ToString());
                if (entity == null) entity = new WXOpenAuth();
                else
                {
                    jsApiTicket = JsonConvert.DeserializeObject<JsApiTicket>(entity.val);
                }

                //获取授权票据
                if (entity.id == 0 || (jsApiTicket != null && entity.update_time.AddSeconds(jsApiTicket.expires_in) < DateTime.Now))
                {
                    jsApiTicket = WeChatJsApiHelper.GetJsApiTicket(token);
                    if (jsApiTicket.errcode != 0)
                    {
                        string er = "开放平台接口，通过access_token获取jsApiTicket[GetWXOpenJsApiTicket]==>" + JsonConvert.SerializeObject(jsApiTicket);
                        s.Insert(new APILogs() { type = 1, created_date = DateTime.Now, content = er });
                    }
                    else
                    {
                        //提前2分钟刷新
                        jsApiTicket.expires_in -= 600;
                        entity.update_time = DateTime.Now;
                        entity.auth_key = WXOpenAuthKey.wx_open_jsApi_ticket.ToString();
                        entity.val = JsonConvert.SerializeObject(jsApiTicket);

                        if (entity.id == 0) s.Insert(entity);
                        else s.Update(entity);
                    }
                }

                return jsApiTicket;
            }


        }

        #endregion


    }
}
