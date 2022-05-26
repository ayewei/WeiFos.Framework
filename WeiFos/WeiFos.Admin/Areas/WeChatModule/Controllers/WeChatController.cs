using WeiFos.Core.Extensions;
using System.Linq;
using WeiFos.Core;
using WeiFos.Core.XmlHelper;
using WeiFos.NetCore.Extensions;
using WeiFos.Entity.BizTypeModule;
using WeiFos.WeChat.WXBase;
using WeiFos.WeChat.TickeModule;
using WeiFos.Service;
using WeiFos.Service.ResourceModule;
using WeiFos.WeChat.Helper;
using Newtonsoft.Json;
using WeiFos.Entity.SystemModule;
using WeiFos.ORM.Data;
using System;
using System.Data;
using WeiFos.Service.ReplyModule;
using WeiFos.ORM.Data.Restrictions;
using WeiFos.ORM.Data.Const;
using WeiFos.Entity.ReplyModule;
using System.Collections.Generic;
using WeiFos.Entity.ResourceModule;
using WeiFos.Entity.WeChatModule;
using WeiFos.Service.WeChatModule;
using WeiFos.Entity.Enums;
using WeiFos.Admin.Controllers;
using WeiFos.Entity.WeChatModule.WXOpen;
using WeiFos.WeChat.WXBase.WXOpen;
using WeiFos.Service.WeChatModule.WXOpen;
using WeiFos.Entity.WeChatModule.EntModule;
using Microsoft.AspNetCore.Mvc;
using WeiFos.Admin.Code.Authorization;
using WeiFos.Core.SettingModule;
using Solution.Service.WeChatModule.EntModule;

namespace WeiFos.Admin.Areas.WeChatModule.Controllers
{


    /// <summary>
    /// System 控制器
    /// @author yewei 
    /// add by @date 2015-01-11
    /// </summary>
    [LoginAuth]
    [Area(AreaNames.WeChatModule)]
    public class WeChatController : BaseController
    {

        #region 关键词是否存在

        public int IsExist(string keywords)
        {
            return ServiceIoc.Get<KeyWordService>().CheckIsExsit(keywords, bid);
        }

        #endregion


        #region 粉丝管理模块

        /// <summary>
        /// 粉丝列表页
        /// </summary>
        /// <returns></returns>
        public IActionResult FansTagManage()
        {
            //获取所有粉丝标签
            List<WeChatUserTag> tags = ServiceIoc.Get<WeChatUserTagService>().GetList("", "");
            ViewBag.userTags = tags;
            return View();
        }



        /// <summary>
        /// 获取粉丝翻页信息
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public JsonResult GetFansTags(int pageSize, int currentPage)
        {
            //查询对象
            Criteria ct = new Criteria();

            //查询表达式
            MutilExpression me = new MutilExpression();

            ct.SetPageSize(pageSize)
            .SetStartPage(currentPage)
            .SetFields(new string[] { "*" })
            .AddOrderBy(new OrderBy("id", "desc"));

            if (me.Expressions.Count > 0)
            {
                ct.SetWhereExpression(me);
            }

            List<WeChatUserTag> data = ServiceIoc.Get<WeChatUserTagService>().GetList(ct);

            return PageResult(StateCode.State_200, ct.TotalRow, data);
        }


        /// <summary>
        /// 保存公众账号
        /// </summary>
        /// <param name="user"></param>
        /// <param name="fans_tag"></param>
        /// <returns></returns>
        public JsonResult SaveFanTags(SysUser user, WeChatUserTag fans_tag)
        {
            StateCode state = ServiceIoc.Get<WeChatUserTagService>().Save(fans_tag);
            return Json(GetResult(state));
        }


        /// <summary>
        /// 删除粉丝标签
        /// </summary>
        /// <param name="user"></param>
        /// <param name="idstr"></param>
        /// <returns></returns>
        public JsonResult DeleteFansTag(SysUser user)
        {
            try
            {
                ServiceIoc.Get<WeChatUserTagService>().Delete(bid);
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
            return Json(GetResult(StateCode.State_200));
        }


        /// <summary>
        /// 粉丝列表页
        /// </summary>
        /// <returns></returns>
        public IActionResult FansManage()
        {
            //Dictionary<int, string> types = new Dictionary<int, string>();
            //types.Add(-1, "——用户类型——");
            //foreach (var k in EnumHelper.GetEnums(typeof(UserType)))
            //{
            //    types.Add(k.Key, k.Value);
            //}
            //ViewBag.UserTypes = types;

            //获取所有粉丝标签
            ViewBag.userTags = ServiceIoc.Get<WeChatUserTagService>().GetList("", "");
            return View();
        }



        /// <summary>
        /// 获取粉丝翻页
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="nickname"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public ContentResult GetFans(int pageSize, int currentPage, string nickname, int usertype, string tags)
        {
            //查询对象
            Criteria ct = new Criteria();

            //查询表达式
            MutilExpression me = new MutilExpression();

            ct.SetFromTables("v_wx_user")
            .SetPageSize(pageSize)
            .SetStartPage(currentPage)
            .SetFields(new string[] { "*" })
            .AddOrderBy(new OrderBy("id", "desc"));

            //标签
            if (!string.IsNullOrEmpty(tags))
            {
                me.Add(new SingleExpression("tag", LogicOper.LIKE, tags));
            }

            //微信昵称
            if (!string.IsNullOrEmpty(nickname))
            {
                me.Add(new SingleExpression("nickname", LogicOper.LIKE, nickname));
            }

            //用户类型
            if (usertype != -1)
            {
                me.Add(new SingleExpression("type", LogicOper.LIKE, usertype));
            }

            if (me.Expressions.Count > 0)
            {
                ct.SetWhereExpression(me);
            }

            DataTable dt = ServiceIoc.Get<WeChatUserService>().Fill(ct);

            return PageResult(StateCode.State_200, ct.TotalRow, dt);
        }


        #endregion


        #region 公众号设置模块


        /// <summary>
        /// 公众号设置
        /// </summary>
        /// <param name="user"></param>
        /// <param name="auth_code"></param>
        /// <param name="expires_in"></param>
        /// <returns></returns>
        public IActionResult AccountSetting(SysUser user, string auth_code = "", string expires_in = "")
        {
            //当前用户加密ID
            ViewBag.Ticket = StringHelper.GetEncryption(bid.ToString());
            //用户图片路径
            ViewBag.imgurl = string.Empty;

            ViewBag.OpenToken = "";
            ViewBag.AscKey = "";

            //缺省图片路劲
            ViewBag.defimgurl = ResXmlConfig.Instance.DefaultImgSrc(AppGlobal.Res, ImgType.WX_Account);
            ViewBag.wx_account_imgurl = ViewBag.defimgurl;
            WeChatAccount account = ServiceIoc.Get<WeChatAccountService>().Get();
            if (account != null)
            {
                Img img = ServiceIoc.Get<ImgService>().GetImg(ImgType.WX_Account, account.id);
                if (img != null)
                {
                    ViewBag.wx_account_imgurl = string.IsNullOrEmpty(img.getImgUrl()) ? ViewBag.defimgurl : img.getImgUrl();
                }
                ViewBag.account = JsonConvert.SerializeObject(account);
            }
            else
            {
                ViewBag.Token = StringHelper.CreateRandomCode(10);
            }

            //微信JSAPI支付
            WeChatMerchant jsapi_merchant = ServiceIoc.Get<WeChatMerchantService>().Get(PayMethod.WeChat_JsApi);
            if (jsapi_merchant != null)
            {
                ViewBag.jsapi_merchant = JsonConvert.SerializeObject(jsapi_merchant);
            }

            //微信付款码支付
            WeChatMerchant code_merchant = ServiceIoc.Get<WeChatMerchantService>().Get(PayMethod.WeChat_Code);
            if (code_merchant != null)
            {
                ViewBag.code_merchant = JsonConvert.SerializeObject(code_merchant);
            }

            //APP支付
            WeChatMerchant app_merchant = ServiceIoc.Get<WeChatMerchantService>().Get(PayMethod.WeChat_App);
            if (app_merchant != null)
            {
                ViewBag.app_merchant = JsonConvert.SerializeObject(app_merchant);
            }


            WeChatAccountEnt weChatAccountEnt = ServiceIoc.Get<WeChatAccountEntService>().Get();
            if (weChatAccountEnt != null)
            {
                ViewBag.accountEnt = JsonConvert.SerializeObject(weChatAccountEnt);
            }

            WXOpenAccount openAcount = ServiceIoc.Get<WXOpenAccountService>().Get();
            if (openAcount != null)
            {
                ViewBag.openAcount = JsonConvert.SerializeObject(openAcount);
            }

            WXOpenSetting openSetting = ServiceIoc.Get<WXOpenSettingService>().Get();
            if (openSetting != null)
            {
                ViewBag.opensetting = JsonConvert.SerializeObject(openSetting);
            }
            else
            {
                ViewBag.OpenToken = StringHelper.GetRandomCode(10);
                ViewBag.AscKey = StringHelper.GetRandomCode(43);
            }

            ViewBag.AuthTitle = "待微信推送票据";
            ViewBag.url = "javascript:;";

            //获取当前凭据
            WXOpenCmptVerifyTicket ticket = ServiceIoc.Get<WXOpenAuthService>().GetCmptVerifyTicket();
            if (ticket != null && ConfigManage.AppSettings<bool>("WeChatSettings:IsOpenAuthUrl"))
            {
                if (ticket.ComponentVerifyTicket != null)
                {
                    ViewBag.WXOpenTicket = ticket.ComponentVerifyTicket.Value;
                    string cmpt_access_token = ServiceIoc.Get<WXOpenAuthService>().GetCmptAccessToken(openSetting, ViewBag.WXOpenTicket);
                    string pre_auth_code = ServiceIoc.Get<WXOpenAuthService>().GetOpenPreAuthCode(cmpt_access_token, openSetting.component_appid);
                    string redirect_uri = AppGlobal.Admin + "WeChat/AccountSetting";

                    ViewBag.AuthTitle = "点击授权公众号";
                    //授权地址
                    ViewBag.url = WeChatOpenHelper.GetOpenOuthUrl(openSetting.component_appid, pre_auth_code, redirect_uri);
                }
            }

            //授权回调
            if (!string.IsNullOrEmpty(auth_code) && !string.IsNullOrEmpty(expires_in))
            {
                //组件Token
                string cmpt_access_token = ServiceIoc.Get<WXOpenAuthService>().GetCmptAccessToken(openSetting, ticket.ComponentVerifyTicket.Value);
                //使用授权码换取公众号的接口调用凭据和授权信息
                WXOpenAuthFun auth_fun = ServiceIoc.Get<WXOpenAuthService>().GetAuthInfo(cmpt_access_token, openSetting.component_appid, auth_code);
                //组件ID
                string cmpt_token = ServiceIoc.Get<WXOpenAuthService>().GetCmptAccessToken(openSetting, ticket.ComponentVerifyTicket.Value);
                //成功
                if (auth_fun != null && !string.IsNullOrEmpty(cmpt_token))
                {
                    ServiceIoc.Get<WXOpenAccountService>().AuthWeChatAccount(user.id, cmpt_token, openSetting.component_appid, auth_fun.authorization_info.authorizer_appid);
                }

                return Redirect(AppGlobal.Admin + "WeChat/AccountSetting");
            }

            return View();
        }


        /// <summary>
        /// 保存公众账号
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dyn"></param>  
        /// <returns></returns>
        public JsonResult SaveAccount(SysUser user, [FromBody] dynamic dyn)
        {
            //微信公众号类型验证
            //if (account.type != WXAccountType.Subscription)
            //{
            //    AccessToken token = WeChatBaseHelper.GetAccessToken(account.appid, account.app_secret);
            //    if (token.access_token == null)
            //    {
            //        return Json(GetResult(StateCode.State_11001, token.errcode.ToString()), JsonRequestBehavior.AllowGet);
            //    }
            //}

            string imgmsg = dyn.imgmsg.ToStrng();
            WeChatAccount entity = JsonConvert.DeserializeObject<WeChatAccount>(dyn.entity.ToStrng());
            StateCode state = ServiceIoc.Get<WeChatAccountService>().Save(user.id, entity, imgmsg);
            return Json(GetResult(state));
        }



        /// <summary>
        /// 保存开放平台配置
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public JsonResult SaveOpenSetting(SysUser user, [FromBody] WXOpenSetting entity)
        {
            StateCode state = ServiceIoc.Get<WeChatAccountService>().Save(user.id, entity);
            return Json(GetResult(state));
        }


        /// <summary>
        /// 保存企业号配置
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public JsonResult SaveAccountEnt(SysUser user, [FromBody] WXOpenSetting entity)
        {
            StateCode state = ServiceIoc.Get<WeChatAccountService>().Save(user.id, entity);
            return Json(GetResult(state));
        }


        /// <summary>
        /// 保存微信支付配置
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public JsonResult SaveWeChatPay(SysUser user, [FromBody] WeChatMerchant entity)
        {
            StateCode state = ServiceIoc.Get<WeChatAccountService>().Save(user.id, entity);
            return Json(GetResult(state));
        }


        /// <summary>
        /// 保存APP支付配置
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public JsonResult SaveMini(SysUser user, [FromBody] WeChatAccount entity)
        {
            StateCode state = ServiceIoc.Get<WeChatAccountService>().Save(user.id, entity, "");
            return Json(GetResult(state));
        }


        /// <summary>
        /// 公众号原始ID是否存在
        /// </summary>
        /// <param name="account_original_id"></param>
        /// <returns></returns>
        public int ExistOriginalId(string account_original_id)
        {
            return ServiceIoc.Get<WeChatAccountService>().ExistOriginalId(account_original_id);
        }


        #endregion


        #region 自定义菜单



        /// <summary>
        /// 微信菜单设置
        /// </summary>
        /// <returns></returns>
        public IActionResult WeChatMenuManage()
        {
            List<DefineMenuGroup> Menus = ServiceIoc.Get<DefineMenuGroupService>().GetAll();
            ViewBag.Menus = JsonConvert.SerializeObject(Menus);
            return View();
        }


        /// <summary>
        /// 微信菜单设置
        /// </summary>
        /// <returns></returns>
        public IActionResult WeChatMenuForm()
        {
            List<DefineMenuGroup> Menus = ServiceIoc.Get<DefineMenuGroupService>().GetAll();
            ViewBag.Menus = JsonConvert.SerializeObject(Menus);
            return View();
        }



        /// <summary>
        /// 微信菜单设置是否可用
        /// </summary>
        /// <param name="is_enable"></param>
        /// <returns></returns>
        public IActionResult SetEnableMenuGroup(bool is_enable)
        {
            StateCode state = ServiceIoc.Get<DefineMenuGroupService>().SetEnable(bid, is_enable);
            return Json(GetResult(state));
        }




        /// <summary>
        /// 保存微信菜单
        /// </summary>
        /// <param name="menus"></param>
        /// <returns></returns>
        public JsonResult SaveWeChatMenu(SysUser user, DefineMenuGroup menus)
        {
            StateCode state = ServiceIoc.Get<DefineMenuGroupService>().SaveBuild(user.id, menus);
            return Json(GetResult(state));
        }



        /// <summary>
        /// 保存微信菜单
        /// </summary>
        /// <param name="menus"></param>
        /// <returns></returns>
        public JsonResult WeChatMenuInit(SysUser user)
        {
            var data = ServiceIoc.Get<DefineMenuGroupService>().Init(bid);
            return Json(GetResult(StateCode.State_200, data));
        }



        /// <summary>
        /// 生成微信自定义菜单
        /// </summary>
        /// <returns></returns>
        public JsonResult BuildWeChatMenu()
        {
            try
            {
                ComplexButton complexBtn;
                WXDefineMenu wxDefiMenu = new WXDefineMenu();
                wxDefiMenu.button = new List<Button>();
                List<DefineMenu> Menus = ServiceIoc.Get<DefineMenuService>().GetEnableList();
                List<DefineMenu> mainMenus = Menus.Where(m => m.parent_id == 0).ToList();
                if (mainMenus.Count > 0)
                {
                    foreach (var menu in mainMenus)
                    {
                        var subMenus = Menus.Where(m => m.parent_id == menu.id).ToList();
                        if (subMenus.Count > 0)
                        {
                            complexBtn = new ComplexButton();
                            complexBtn.sub_button = new List<Button>();
                            complexBtn.name = menu.name;
                            foreach (var submenu in subMenus)
                                complexBtn.sub_button.Add(GetButton(submenu));
                            wxDefiMenu.button.Add(complexBtn);
                        }
                        else
                        {
                            wxDefiMenu.button.Add(GetButton(menu));
                        }
                    }

                    string postData = JsonConvert.SerializeObject(wxDefiMenu).Replace("%26", "&");
                    WeChatAccount wxAccount = ServiceIoc.Get<WeChatAccountService>().Get();
                    AccessToken token = WeChatBaseHelper.GetAccessToken(wxAccount.appid, wxAccount.app_secret);
                    if (token.access_token == null)
                    {
                        return Json(GetResult(StateCode.State_152, token.errcode));
                    }
                    string data = WeChatBaseHelper.CreateWXMenu(token.access_token, postData);
                    var errcode = JsonConvert.DeserializeObject<WXErrCode>(data);
                    if (errcode.errcode > 0) return Json(GetResult(StateCode.State_152, token.errcode));
                }
                return Json(GetResult(StateCode.State_200));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }



        /// <summary>
        /// 删除微信菜单
        /// </summary>
        /// <returns></returns>
        public JsonResult DelWeChatMenu()
        {
            try
            {
                string data;
                WeChatAccount wxAccount = ServiceIoc.Get<WeChatAccountService>().Get();
                AccessToken token = WeChatBaseHelper.GetAccessToken(wxAccount.appid, wxAccount.app_secret);
                if (token.access_token == null)
                {
                    return Json(GetResult(StateCode.State_152));
                }
                data = WeChatBaseHelper.DeleteWXMenu(token.access_token);
                var errcode = JsonConvert.DeserializeObject<WXErrCode>(data);
                if (errcode.errcode > 0) return Json(GetResult(StateCode.State_152, token.errcode));

                //errcode 等于0时候成功
                return Json(GetResult(StateCode.State_200));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }



        /// <summary>
        /// 获取菜单按钮类型
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        private Button GetButton(DefineMenu menu)
        {
            if (menu.type.Equals("click"))
                return new KeyButton() { name = menu.name, key = menu.key_val, type = menu.type };
            else
                return new UrlButton() { name = menu.name, url = menu.key_val.Replace("&", "%26"), type = menu.type };
        }




        #endregion


        #region LBS回复管理模块


        /// <summary>
        /// LBS回复管理
        /// </summary>
        /// <returns></returns>
        public IActionResult LBSReplyManage()
        {
            return View();
        }


        /// <summary>
        /// 获取LBS地理位置
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ContentResult GetLBSReplys(int pageSize, int currentPage, int type, string keyword)
        {
            //查询对象
            Criteria ct = new Criteria();

            //查询表达式
            MutilExpression me = new MutilExpression();

            ct.SetFromTables("tb_rpy_lbs")
            .SetPageSize(pageSize)
            .SetStartPage(currentPage)
            .SetFields(new string[] { "*" })
            .AddOrderBy(new OrderBy("id", "desc"));

            //商品类型
            if (type == 0)
            {
                me.Add(new SingleExpression("name", LogicOper.LIKE, keyword));
            }
            else
            {
                me.Add(new SingleExpression("lbsaddress", LogicOper.LIKE, keyword));
            }

            if (me.Expressions.Count > 0)
            {
                ct.SetWhereExpression(me);
            }

            DataTable dt = ServiceIoc.Get<ImgTextReplyService>().Fill(ct);

            return PageResult(StateCode.State_200, ct.TotalRow, dt);
        }



        /// <summary>
        /// 保存LBS回复
        /// </summary>
        /// <returns></returns>
        //public JsonResult SaveLBSReply(SysUser user, LbsReply lbsReply, string imgmsg)
        //{
        //    StateCode state = ServiceIoc.Get<LbsReplyService>().Save(user, lbsReply, imgmsg);
        //    return Json(GetResult((int)state));
        //}


        #endregion


        #region 回复关键词模块

        /// <summary>
        /// 关键词是否存在
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="biz_type"></param>
        /// <returns></returns>
        public int ExistKeyword(string keywords, string biz_type)
        {
            return Convert.ToInt32(ServiceIoc.Get<KeyWordService>().CheckIsExsit(keywords, bid));
        }

        #endregion


        #region 默认回复模块


        /// <summary>
        /// 默认回复设置
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public IActionResult DefaultReply(DefaultSet? entity = null)
        {
            if (NHttpContext.Current.Request.IsAjaxRequest())
            {
                StateCode state = ServiceIoc.Get<DefaultSetService>().SaveDefaultSets(entity);
                return Json(GetResult(state));
            }
            else
            {
                //获取账号默认回复设置
                DefaultSet defaultSet = ServiceIoc.Get<DefaultSetService>().GetDefaultSet();
                if (defaultSet != null)
                {
                    //关注回复类型
                    if (!string.IsNullOrEmpty(defaultSet.a_reply_value))
                    {
                        string a_type = defaultSet.a_reply_value.Split('#')[1];

                        //关注回复业务ID
                        int a_bizid = int.Parse(defaultSet.a_reply_value.Split('#')[0].ToString());

                        //对应关键词
                        List<KeyWord> keyWord = ServiceIoc.Get<KeyWordService>().GetKeywordGroup(a_bizid, a_type);
                        if (keyWord != null && keyWord.Count > 0)
                        {
                            ViewBag.attention_content = string.Join(" ", keyWord.Select(k => k.keyword).ToArray());
                        }
                    }

                    //无匹配回复设置
                    if (!string.IsNullOrEmpty(defaultSet.d_reply_value))
                    {
                        string d_type = defaultSet.d_reply_value.Split('#')[1];

                        //关注回复业务ID
                        int d_bizid = int.Parse(defaultSet.d_reply_value.Split('#')[0].ToString());

                        //对应关键词
                        List<KeyWord> keyWord = ServiceIoc.Get<KeyWordService>().GetKeywordGroup(d_bizid, d_type);
                        if (keyWord != null && keyWord.Count > 0)
                        {
                            ViewBag.nomatch_content = string.Join(" ", keyWord.Select(k => k.keyword).ToArray());
                        }
                    }

                    ViewBag.entity = JsonConvert.SerializeObject(defaultSet);
                }
            }

            return View();
        }


        #endregion


        #region 文本回复管理模块

        /// <summary>
        /// 文本回复管理
        /// </summary>
        /// <returns></returns>
        public IActionResult TextReplyManage()
        {
            return View();
        }

        /// <summary>
        /// 文本回复翻页
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ContentResult GetTextReplys(int pageSize, int pageIndex, string keyword)
        {
            //查询对象
            Criteria ct = new Criteria();

            //查询表达式
            MutilExpression me = new MutilExpression();

            ct.SetFromTables("v_rpy_textreply")
            .SetPageSize(pageSize)
            .SetStartPage(pageIndex)
            .SetFields(new string[] { "*" })
            .AddOrderBy(new OrderBy("id", "desc"));

            //类别ID
            if (!string.IsNullOrEmpty(keyword))
            {
                me.Add(new SingleExpression("keywords", LogicOper.LIKE, keyword));
            }

            if (me.Expressions.Count > 0)
            {
                ct.SetWhereExpression(me);
            }

            DataTable dt = ServiceIoc.Get<TextReplyService>().Fill(ct);

            return PageResult(StateCode.State_200, ct.TotalRow, dt);

        }

        /// <summary>
        /// 删除选择文本回复
        /// </summary>
        /// <param name="user"></param>
        /// <param name="idstr"></param>
        /// <returns></returns>
        public JsonResult DelSelectTextReply(SysUser user, long[] ids)
        {
            StateCode state = ServiceIoc.Get<TextReplyService>().Deletes(ids);
            return Json(GetResult(state));
        }



        /// <summary>
        /// 文本回复
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entity"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public IActionResult TextReplyForm(SysUser user, TextReply? entity, string[]? keywords)
        {
            if (Request.IsAjaxRequest())
            {
                StateCode state = ServiceIoc.Get<TextReplyService>().Save(user, entity, keywords);
                return Json(GetResult(state));
            }
            else
            {
                TextReply textReply = ServiceIoc.Get<TextReplyService>().GetById(bid);
                if (textReply != null)
                {
                    ViewBag.reply_contents = textReply.reply_contents;
                    ViewBag.keywords = ServiceIoc.Get<KeyWordService>().GetKeywords(bid, KeyWordBizType.TextReply);
                }
            }
            return View();
        }


        #endregion


        #region 图文回复管理模块


        /// <summary>
        /// 图文回复管理
        /// </summary>
        /// <returns></returns>
        public IActionResult ImgTextReplyManage()
        {
            return View();
        }


        /// <summary>
        /// 图文回复
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entity"></param>
        /// <param name="keywords"></param>
        /// <param name="imgmsg"></param>
        /// <returns></returns>
        public IActionResult ImgTextReplyForm(SysUser user, ImgTextReply? entity = null, string[]? keywords = null, string? imgmsg = null)
        {
            if (Request.IsAjaxRequest())
            {
                StateCode state = ServiceIoc.Get<ImgTextReplyService>().Save(user, entity, keywords, imgmsg);
                return Json(GetResult(state));
            }
            else
            {
                //上传票据ID
                ViewBag.Ticket = StringHelper.GetEncryption(ImgType.ImgTextReply_Title + "#" + bid.ToString());
                //上传票据ID
                ViewBag.DetailsTicket = StringHelper.GetEncryption(ImgType.ImgTextReply_Details + "#" + bid.ToString());
                //缺省图片路径
                ViewBag.defimgurl = ResXmlConfig.Instance.DefaultImgSrc(AppGlobal.Res, ImgType.ImgTextReply_Title);
                //用户图片路径
                ViewBag.imgurl = string.Empty;

                //缺省图片路劲
                ViewBag.defurl = ResXmlConfig.Instance.DefaultImgSrc(AppGlobal.Res, ImgType.ImgTextReply_Title);
                ViewBag.imgurl = ViewBag.defimgurl;

                //地图坐标
                ViewBag.lat_lng = "22.541234,114.06262";
                ViewBag.address_text = "广东省深圳市福田区益田路48号";

                /**********回复外链类型************/
                Dictionary<int, string> imgTextTypes = new Dictionary<int, string>();
                imgTextTypes.Add(MsgContentType.ImgTextDetails, "请选择");
                foreach (var v in MsgContentType.typeList)
                {
                    imgTextTypes.Add(v.Key, v.Value);
                }
                ViewBag.imgTextTypes = imgTextTypes;

                entity = ServiceIoc.Get<ImgTextReplyService>().GetById(bid);
                if (entity != null)
                {
                    Img img = ServiceIoc.Get<ImgService>().GetImg(ImgType.ImgTextReply_Title, entity.id);
                    if (img != null)
                    {
                        ViewBag.imgurl = img.getImgUrl();
                    }
                    ViewBag.keywords = ServiceIoc.Get<KeyWordService>().GetKeywords(bid, KeyWordBizType.ImgTextReply);
                    ViewBag.entity = JsonConvert.SerializeObject(entity);

                    long[] moreIds = StringHelper.StringToLongArray(entity.quote_detailsIds);
                    //绑定多图文
                    ViewBag.moreImgTextReplys = ServiceIoc.Get<ImgTextReplyService>().GetTextReplysByIds(moreIds);

                    long[] recIds = StringHelper.StringToLongArray(entity.rec_detailsIds);
                    //绑定推荐阅读
                    ViewBag.recImgTextReplys = ServiceIoc.Get<ImgTextReplyService>().GetTextReplysByIds(recIds);

                    switch (entity.content_type)
                    {
                        //外链
                        case MsgContentType.OutLink:
                            ViewBag.outlink = entity.content_value;
                            break;
                        //导航
                        case MsgContentType.Navigation:
                            if (!string.IsNullOrEmpty(entity.content_value) && entity.content_value.IndexOf(",") != -1 && entity.content_value.IndexOf("#") != -1)
                            {
                                string val = entity.content_value.Split('#')[0].ToString();
                                if (val.Length > 3)
                                {
                                    ViewBag.lat_lng = val;
                                }
                                ViewBag.address_text = entity.content_value.Split('#')[1].ToString();
                            }
                            break;
                    }
                }

                if (ViewBag.moreImgTextReplys == null)
                {
                    ViewBag.moreImgTextReplys = new List<ImgTextReply>();
                }

                if (ViewBag.recImgTextReplys == null)
                {
                    ViewBag.recImgTextReplys = new List<ImgTextReply>();
                }
            }

            return View();
        }


        /// <summary>
        /// 文本回复翻页
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ContentResult GetImgTextReplys(int pageSize, int pageIndex, string keyword)
        {
            //查询对象
            Criteria ct = new Criteria();

            //查询表达式
            MutilExpression me = new MutilExpression();

            ct.SetFromTables("v_rpy_imgtextreply")
            .SetPageSize(pageSize)
            .SetStartPage(pageIndex)
            .SetFields(new string[] { "*" })
            .AddOrderBy(new OrderBy("id", "desc"));

            //类别ID
            if (!string.IsNullOrEmpty(keyword))
            {
                me.Add(new SingleExpression("keywords", LogicOper.LIKE, keyword));
            }

            if (me.Expressions.Count > 0)
            {
                ct.SetWhereExpression(me);
            }

            DataTable dt = ServiceIoc.Get<ImgTextReplyService>().Fill(ct);

            return PageResult(StateCode.State_200, ct.TotalRow, dt);
        }


        /// <summary>
        /// 删除选择文本回复
        /// </summary>
        /// <param name="user"></param>
        /// <param name="idstr"></param>
        /// <returns></returns>
        public JsonResult DelSelectImgTextReply(SysUser user, long[] ids)
        {
            StateCode state = ServiceIoc.Get<ImgTextReplyService>().Deletes(ids);
            return Json(GetResult(state));
        }


        #endregion


    }
}