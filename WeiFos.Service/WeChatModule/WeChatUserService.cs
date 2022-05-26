using WeiFos.Entity.WeChatModule;
using WeiFos.ORM.Data;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using WeiFos.WeChat.Helper;
using WeiFos.WeChat.Models;
using System;
using System.Text;
using WeiFos.Core;
using WeiFos.ORM.Data.Restrictions;
using WeiFos.ORM.Data.Const;
using WeiFos.Entity.Enums;
using WeiFos.Entity.WeChat;
using WeiFos.Core.Extensions;

namespace WeiFos.Service.WeChatModule
{
    /// <summary>
    /// 微信用户service 
    /// @author yewei 
    /// @date 2015-05-17
    /// </summary>
    public class WeChatUserService : BaseService<WeChatUser>
    {


        /// <summary>
        /// 获取微信用户图像
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public string GetWeChatUserHeadImg(long user_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                object url = s.ExecuteScalar("select headimgurl from tb_wx_user where user_id = @0", user_id);
                if (url == null)  return ""; 

                return url.ToString(); 
            }
        }


        /// <summary>
        /// 获取终端用户
        /// </summary>
        /// <param name="open_id"></param>
        /// <returns></returns>
        public WeChatUser Get(string open_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<WeChatUser>("where openid = @0 ", open_id);
            }
        }


        /// <summary>
        /// 获取终端用户
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public WeChatUser Get(long user_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<WeChatUser>("where user_id = @0 ", user_id);
            }
        }


        /// <summary>
        /// 根据手机号码获取用户
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public WeChatUser GetByMobilePhone(string phone)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<WeChatUser>("where mobile_phone = @0 ", phone);
            }
        }


        /// <summary>
        /// 同步单一粉丝
        /// 关注时同步
        /// </summary>
        /// <param name="open_id"></param>
        /// <param name="wxAccount"></param>
        public void SyncSingleWXUser(string open_id, WeChatAccount wxAccount)
        {
            //获取ToKen
            WXAccessTokenCache token = ServiceIoc.Get<WeChatTokenService>().AccessToken(wxAccount.appid, wxAccount.app_secret);

            //通过微信接口获取
            string WeChatUser_json = WeChatUserHelper.GetWXUserInfo(token.token, open_id);

            //序列化用户
            WeChatUser WeChatUser = JsonConvert.DeserializeObject<WeChatUser>(WeChatUser_json);

            SyncWXUser(WeChatUser);
        }


        /// <summary>
        /// 更新联系时间
        /// </summary>
        /// <param name="open_id"></param>
        public void UpdateLastContactTime(string open_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                s.ExecuteScalar("update tb_wx_user set last_contact_time = @0 where openid = @1", DateTime.Now, open_id);
            }
        }


        /// <summary>
        /// 同步公众号粉丝
        /// </summary>
        /// <param name="WeChatUser"></param>
        public void SyncWXUser(WeChatUser WeChatUser)
        {
            SyncWXUser(WeChatUser, 0);
        }


        /// <summary>
        /// 同步公众号粉丝(微信端自动注册)
        /// </summary>
        /// <param name="weChatUser"></param>
        /// <param name="r_user_id">推荐用户iD</param>
        public void SyncWXUser(WeChatUser weChatUser, long r_user_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                int exist = s.Exist<WeChatUser>("where openid = @0 ", weChatUser.openid);

                //存在用户数据
                if (exist > 0)
                {
                    //已关注 则修改
                    if (weChatUser.subscribe > 0)
                    {
                        StringBuilder sql = new StringBuilder();
                        sql.Append("update tb_wx_user set ")
                        .Append("subscribe = @0, nickname=@1, country = @2, province = @3, city = @4, language = @5,headimgurl =@6, subscribe_time = @7,last_contact_time = @8 where openid = @9 ");
                        s.ExcuteUpdate(sql.ToString(), weChatUser.subscribe, weChatUser.nickname, weChatUser.country, weChatUser.province, weChatUser.city, weChatUser.language, weChatUser.headimgurl, weChatUser.subscribe_time, DateTime.Now, weChatUser.openid);
                    }
                }
                else
                { 
                    weChatUser.created_date = DateTime.Now;
                    s.Insert<WeChatUser>(weChatUser);
                }
            }
        }





        /// <summary>
        /// 同步微信用户分组
        /// </summary>
        /// <param name="wxaccount"></param>
        /// <param name="open_id"></param>
        /// <param name="tableConfig"></param>
        public void SyncWXGroup(WeChatAccount wxAccount, string open_id)
        {
            //获取token
            WXAccessTokenCache token = ServiceIoc.Get<WeChatTokenService>().AccessToken(wxAccount.appid, wxAccount.app_secret);

            //公众号粉丝集合
            string groups = WeChatUserHelper.GetWxGroups(token.token);

            //序列化数据
            string[] groupslist = JsonConvert.DeserializeObject<string[]>(groups);

            foreach (string s in groupslist)
            {

            }
            //open_ids
        }


        /// <summary>
        /// 获取粉丝openid集合
        /// </summary>
        /// <returns></returns>
        //public string[] GetOpenIds()
        //{
        //    using (ISession s = SessionFactory.Instance.CreateSession())
        //    {
        //        //查询对象
        //        Criteria ct = new Criteria();

        //        //查询表达式
        //        MutilExpression me = new MutilExpression();
        //        ct.SetFromTables("")
        //        .SetFields(new string[] { "openid" });

        //        List<WeChatUser> WeChatUsers = s.List<WeChatUser>(ct);

        //        return WeChatUsers.Select(u => u.openid.ToString()).ToArray();
        //    }
        //}


        /// <summary>
        /// 获取OpenIds
        /// </summary>
        /// <param name="tag_id"></param>
        /// <param name="sex"></param>
        public string[] GetOpenIdsByTagId(string tag, int sex)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                List<string> openids = new List<string>();

                //查询对象
                Criteria ct = new Criteria();

                //查询表达式
                MutilExpression me = new MutilExpression();
                ct.SetFields(new string[] { "openid" });

                List<WeChatUser> userTagUnites = s.List<WeChatUser>("where tag = @0 ", tag);
                foreach (WeChatUser fans in userTagUnites)
                {
                    openids.Add(fans.openid.ToString());
                }
                return openids.ToArray();
            }
        }


        /// <summary>
        /// 根据自定义分组获取
        /// </summary>
        /// <param name="groupTypeId"></param>
        /// <param name="sex"></param>
        /// <returns></returns>
        public string[] GetOpenIdsByCustomGroup(int groupTypeId, int sex)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //查询对象
                Criteria ct = new Criteria();

                //查询表达式
                MutilExpression me = new MutilExpression();
                ct.SetFields(new string[] { "openid" })
                .AddOrderBy(new OrderBy("id", "desc"));

                switch (groupTypeId)
                {
                    //今天新增客户
                    case FansGroupType.add_today:
                        me.Add(new SingleExpression("subscribe_time", LogicOper.GT, TimeZoneExtension.ConvertDateTimeInt(DateTime.Now.Date)));
                        me.Add(new SingleExpression("subscribe_time", LogicOper.LT, TimeZoneExtension.ConvertDateTimeInt(DateTime.Now.Date.AddDays(1).AddSeconds(-1))));
                        break;
                    //今天联系客户
                    case FansGroupType.contact_today:
                        me.Add(new SingleExpression("last_contact_time", LogicOper.BETWEEN, new[] { DateTime.Now.Date.ToString(), DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString() }));
                        break;
                    //近7天联系客户
                    case FansGroupType.contact_seven:
                        me.Add(new SingleExpression("last_contact_time", LogicOper.BETWEEN, new[] { DateTime.Now.Date.AddDays(-7).ToString(), DateTime.Now.Date.ToString() }));
                        break;
                    //近30天联系客户
                    case FansGroupType.contact_thirty:
                        me.Add(new SingleExpression("last_contact_time", LogicOper.BETWEEN, new[] { DateTime.Now.Date.AddDays(-30).ToString(), DateTime.Now.Date.ToString() }));
                        break;
                    //近30天未联系客户
                    case FansGroupType.discontact_thirty:
                        me.Add(new SingleExpression("last_contact_time", LogicOper.LT, DateTime.Now.Date.AddDays(-30)));
                        break;
                    //今天关注客户
                    case FansGroupType.attention_today:
                        me.Add(new SingleExpression("subscribe_time", LogicOper.GT, TimeZoneExtension.ConvertDateTimeInt(DateTime.Now.Date)));
                        me.Add(new SingleExpression("subscribe_time", LogicOper.LT, TimeZoneExtension.ConvertDateTimeInt(DateTime.Now.Date.AddDays(1).AddSeconds(-1))));
                        break;
                    //近7天关注客户
                    case FansGroupType.attention_seven:
                        me.Add(new SingleExpression("subscribe_time", LogicOper.GT, TimeZoneExtension.ConvertDateTimeInt(DateTime.Now.Date.AddDays(-7))));
                        me.Add(new SingleExpression("subscribe_time", LogicOper.LT, TimeZoneExtension.ConvertDateTimeInt(DateTime.Now.Date)));
                        break;
                    //近30天关注客户
                    case FansGroupType.attention_thirty:
                        me.Add(new SingleExpression("subscribe_time", LogicOper.GT, TimeZoneExtension.ConvertDateTimeInt(DateTime.Now.Date.AddDays(-30))));
                        me.Add(new SingleExpression("subscribe_time", LogicOper.LT, TimeZoneExtension.ConvertDateTimeInt(DateTime.Now.Date)));
                        break;
                    //30天前关注客户
                    case FansGroupType.attention_thirty_ago:
                        me.Add(new SingleExpression("subscribe_time", LogicOper.GT, TimeZoneExtension.ConvertDateTimeInt(DateTime.Now.Date.AddDays(-30))));
                        break;

                    default:
                        break;
                }

                me.Add(new SingleExpression("sex", LogicOper.EQ, sex));

                //设置查询条件
                ct.SetWhereExpression(me);

                List<WeChatUser> WeChatUsers = s.List<WeChatUser>(ct);

                return WeChatUsers.Select(u => u.openid.ToString()).ToArray();
            }
        }



        #region 微信端 


        /// <summary>
        /// 扫描二维码关注 同步粉丝信息
        /// </summary>
        /// <param name="open_id"></param>
        /// <param name="phone"></param>
        public void Subscribe(WeChatAccount wxAccount, string open_id, long user_id)
        {
            //获取ToKen
            WXAccessTokenCache token = ServiceIoc.Get<WeChatTokenService>().AccessToken(wxAccount.appid, wxAccount.app_secret);

            //通过微信接口获取
            string WeChatUser_json = WeChatUserHelper.GetWXUserInfo(token.token, open_id);
            //序列化用户
            WeChatUser user = JsonConvert.DeserializeObject<WeChatUser>(WeChatUser_json);

            SyncWXUser(user, user_id);
        }


        /// <summary>
        /// 获取用户Code数组
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<string> GetUsercodesByIds(string ids)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                List<string> usercodes = null;
                List<WeChatUser> WeChatUsers = s.List<WeChatUser>("", "");
                if (WeChatUsers.Count > 0)
                {
                    usercodes = new List<string>();
                    foreach (var u in WeChatUsers)
                        usercodes.Add(u.openid);
                }
                return usercodes;
            }
        }


        /// <summary>
        /// 同步公众号所用粉丝
        /// </summary>
        /// <param name="wxAccount"></param>
        public void SyncAllFans(WeChatAccount wxAccount)
        {
            SyncAllWXUser(wxAccount, "");
        }



        /// <summary>
        /// 同步公众号所用粉丝
        /// </summary>
        /// <param name="wxAccount"></param>
        public void SyncAllWXUser(WeChatAccount wxAccount, string next_openid)
        {
            if (wxAccount.type == WeChatAccountType.AuthSubscriber || wxAccount.type == WeChatAccountType.AuthService)
            {
                //获取ToKen
                WXAccessTokenCache token = ServiceIoc.Get<WeChatTokenService>().AccessToken(wxAccount.appid, wxAccount.app_secret);
                string user_list = WeChatUserHelper.GetWXUserList(token.token, next_openid);
                WeChatUserBase wxUsers = JsonConvert.DeserializeObject<WeChatUserBase>(user_list);

                if (wxUsers.data != null)
                {
                    //open_id 集合
                    string[] open_ids = wxUsers.data.openid;

                    //遍历
                    foreach (string open_id in open_ids)
                    {
                        //通过微信接口获取
                        string WeChatUser_json = WeChatUserHelper.GetWXUserInfo(token.token, open_id);

                        //序列化用户
                        WeChatUser WeChatUser = JsonConvert.DeserializeObject<WeChatUser>(WeChatUser_json);

                        SyncWXUser(WeChatUser);
                    }

                    //存在下一openid
                    if (!string.IsNullOrEmpty(wxUsers.next_openid))
                    {
                        SyncAllWXUser(wxAccount, wxUsers.next_openid);
                    }
                }
            }
        }



        /// <summary>
        /// 微信端登录绑定用户
        /// </summary>
        /// <param name="open_id"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public StateCode BindUser(string open_id, long user_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    s.ExcuteUpdate("update tb_wx_user set user_id = @0 where openid = @1", user_id, open_id);
                    s.ExcuteUpdate("update tb_user set is_bind_wechat = @0 where id = @1", true, user_id);

                    s.Commit();
                }
                catch
                {
                    s.RollBack();
                }
                return StateCode.State_200;
            }
        }



        #endregion

    }
}
