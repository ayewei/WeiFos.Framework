using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.Core.XmlHelper;
using WeiFos.Entity.Common;
using WeiFos.Entity.Enums;
using WeiFos.Entity.ReplyModule;
using WeiFos.Entity.UserModule;
using WeiFos.Entity.WeChat;
using WeiFos.WeChat.WXRequest;
using WeiFos.WeChat.WXResponse;
using WeiFos.ORM.Data;
using WeiFos.Entity.WeChatModule;

namespace WeiFos.Service.ReplyModule
{
    /// <summary>
    /// 默认回复设置Service
    /// @author yewei 
    /// @date 2013-10-22
    /// </summary>
    public class DefaultSetService : BaseService<DefaultSet>
    {

        /// <summary>
        /// 获取账号默认回复设置
        /// </summary>
        /// <returns></returns>
        public DefaultSet GetDefaultSet()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<DefaultSet>("where id != @0", 0);
            }
        }

        /// <summary>
        /// 获取默认无匹配消息
        /// </summary>
        /// <returns></returns>
        public string GetDefaultSetMsg()
        {
            DefaultSet defaultSet;
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                defaultSet = s.Get<DefaultSet>("where id != @0", 0);
            }

            //默认无匹配回复
            if (defaultSet != null && !string.IsNullOrEmpty(defaultSet.d_reply_value))
            {
                //关注回复类型
                string a_type = defaultSet.a_reply_value.Split('#')[1];

                //关注回复业务ID
                int a_bizid = int.Parse(defaultSet.a_reply_value.Split('#')[0].ToString());

                //图文类型
                if (a_type.Equals(KeyWordBizType.ImgTextReply))
                {

                }//文本类型
                else if (a_type.Equals(KeyWordBizType.TextReply))
                {

                }

            }
            return "";
        }


        /// <summary>
        /// 获取默认 无关键词 匹配回复
        /// </summary>
        /// <param name="wxRequest"></param>
        /// <param name="account"></param>
        /// <param name="globalConfig"></param>
        /// <returns></returns>
        public string GetWXResponseDefaultMsg(WXReqBaseMsg wxRequest, WeChatAccount account)
        {
            //获取账号默认回复设置
            DefaultSet defaultSet = ServiceIoc.Get<DefaultSetService>().GetDefaultSet();

            //默认无匹配回复
            if (defaultSet != null && !string.IsNullOrEmpty(defaultSet.d_reply_value) && defaultSet.d_reply_value.IndexOf("#") != -1 && defaultSet.d_reply_isopen)
            {
                //关注回复类型
                string d_type = defaultSet.d_reply_value.Split('#')[1];

                //关注回复业务ID
                int d_bizid = int.Parse(defaultSet.d_reply_value.Split('#')[0].ToString());

                //文本回复
                if (KeyWordBizType.TextReply.Equals(d_type))
                {
                    return ServiceIoc.Get<TextReplyService>().GetWXResponseMsg(wxRequest, account, d_bizid);
                }
                else if (KeyWordBizType.ImgTextReply.Equals(d_type))
                {
                    return ServiceIoc.Get<ImgTextReplyService>().GetWXResponseMsg(wxRequest, account, d_bizid);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取默认 关注时回复
        /// </summary>
        /// <param name="wxRequest"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public string GetWXResponseAttentionMsg(WXReqBaseMsg wxRequest, WeChatAccount account)
        {
            //获取账号默认回复设置
            DefaultSet defaultSet = ServiceIoc.Get<DefaultSetService>().GetDefaultSet();

            //默认无匹配回复
            if (defaultSet != null && !string.IsNullOrEmpty(defaultSet.a_reply_value) && defaultSet.a_reply_value.IndexOf("#") != -1 && defaultSet.a_reply_isopen)
            {
                //关注回复类型
                string a_type = defaultSet.a_reply_value.Split('#')[1];

                //关注回复业务ID
                int a_bizid = int.Parse(defaultSet.a_reply_value.Split('#')[0].ToString());

                //文本回复
                if (KeyWordBizType.TextReply.Equals(a_type))
                {
                    return ServiceIoc.Get<TextReplyService>().GetWXResponseMsg(wxRequest, account, a_bizid);
                }
                else if (KeyWordBizType.ImgTextReply.Equals(a_type))
                {
                    return ServiceIoc.Get<ImgTextReplyService>().GetWXResponseMsg(wxRequest, account, a_bizid);
                }
            }
            return string.Empty;
        }

        public StateCode SaveDefaultSets(DefaultSet defaultSet)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    //关注时回复 设置 
                    if (!string.IsNullOrEmpty(defaultSet.a_reply_value) && defaultSet.a_reply_value.IndexOf("#") != -1)
                    {
                        int biz_id = 0;
                        //图文或文本类型
                        string a_type = defaultSet.a_reply_value.Split('#')[1];
                        //图文或文本ID
                        string a_biz_id = defaultSet.a_reply_value.Split('#')[0];

                        int.TryParse(a_biz_id, out biz_id);

                        if (KeyWordBizType.TextReply.Equals(a_type))
                        {
                            TextReply textReply = s.Get<TextReply>(biz_id);
                            if (textReply == null)
                            {
                                defaultSet.a_reply_value = null;
                            }
                        }
                        else if (KeyWordBizType.ImgTextReply.Equals(a_type))
                        {
                            ImgTextReply imgTextReply = s.Get<ImgTextReply>(biz_id);
                            if (imgTextReply == null)
                            {
                                defaultSet.a_reply_value = null;
                            }
                        }
                        else
                        {
                            defaultSet.a_reply_value = null;
                        }
                    }
                    else
                    {
                        defaultSet.a_reply_value = null;
                    }

                    //默认无匹配关键词ID
                    if (!string.IsNullOrEmpty(defaultSet.d_reply_value) && defaultSet.d_reply_value.IndexOf("#") != -1)
                    {
                        int biz_id = 0;
                        //图文或文本类型
                        string d_type = defaultSet.d_reply_value.Split('#')[1];
                        //图文或文本ID
                        string d_biz_id = defaultSet.d_reply_value.Split('#')[0];

                        int.TryParse(d_biz_id, out biz_id);

                        if (KeyWordBizType.TextReply.Equals(d_type))
                        {
                            TextReply textReply = s.Get<TextReply>(biz_id);
                            if (textReply == null)
                            {
                                defaultSet.d_reply_value = null;
                            }
                        }
                        else if (KeyWordBizType.ImgTextReply.Equals(d_type))
                        {
                            ImgTextReply imgTextReply = s.Get<ImgTextReply>(biz_id);
                            if (imgTextReply == null)
                            {
                                defaultSet.d_reply_value = null;
                            }
                        }
                        else
                        {
                            defaultSet.d_reply_value = null;
                        }
                    }
                    else
                    {
                        defaultSet.d_reply_value = null;
                    }

                    DefaultSet dSet = s.Get<DefaultSet>("where id != @0 ", 0);
                    if (dSet == null)
                    {
                        s.Insert<DefaultSet>(defaultSet);
                    }
                    else
                    {
                        defaultSet.id = dSet.id;
                        s.Update<DefaultSet>(defaultSet);
                    }
                }
                catch
                {
                    s.RollBack();
                    return StateCode.State_500;
                }
                s.Commit();
                return StateCode.State_200;
            }
        }


    }
}
