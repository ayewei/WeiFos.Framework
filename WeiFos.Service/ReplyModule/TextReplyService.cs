using System;
using System.Linq;
using WeiFos.Core.XmlHelper;
using WeiFos.Entity.Enums;
using WeiFos.Entity.ReplyModule;
using WeiFos.WeChat.WXRequest;
using WeiFos.WeChat.WXResponse;
using WeiFos.ORM.Data;
using WeiFos.WeChat.WXBase;
using WeiFos.Entity.WeChatModule;
using WeiFos.Entity.SystemModule;

namespace WeiFos.Service.ReplyModule
{

    /// <summary>
    /// 回复关键字Service
    /// @author yewei 
    /// @date 2013-10-22
    /// </summary>
    public class TextReplyService : BaseService<TextReply>
    {

        /// <summary>
        /// 保存文本回复
        /// </summary>
        /// <param name="aId"></param>
        /// <param name="keywords"></param>
        /// <param name="reply_contents"></param>
        public StateCode Save(SysUser user, TextReply textReply, string[] keywords)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    //关键词不为空
                    if (keywords != null)
                    {
                        if (textReply.id != 0)
                        {
                            //修改回复信息
                            int exist = s.Exist<TextReply>("where id = @0", textReply.id);
                            if (exist > 0)
                            {
                                textReply.updated_user_id = user.id;
                                textReply.updated_date = DateTime.Now;
                                s.Update<TextReply>(textReply);
                            }
                        }
                        else
                        {
                            textReply.created_user_id = user.id;
                            textReply.created_date = DateTime.Now;
                            s.Insert<TextReply>(textReply);
                        }

                        s.ExcuteUpdate("delete tb_rpy_keywords where biz_id=@0 and biz_type=@1 ", textReply.id, KeyWordBizType.TextReply);

                        foreach (string k in keywords)
                        {
                            if (!string.IsNullOrEmpty(k))
                            {
                                int k_exist = s.Exist<KeyWord>("where keyword = @0 ", k);
                                if (k_exist == 0)
                                {
                                    KeyWord key_word = new KeyWord()
                                    {
                                        keyword = k,
                                        biz_id = textReply.id,
                                        biz_type = KeyWordBizType.TextReply,
                                        created_user_id = user.id,
                                        created_date = DateTime.Now
                                    };
                                    s.Insert<KeyWord>(key_word);
                                }
                            }
                        }

                    }
                    s.Commit();
                    return StateCode.State_200;
                }
                catch
                {
                    s.RollBack();
                    return StateCode.State_500;
                }

            }
        }


        /// <summary>
        /// 删除文本回复
        /// </summary>
        /// <param name="Ids"></param>
        public StateCode Deletes(long[] Ids)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    for (int i = 0; i < Ids.Count(); i++)
                    {
                        s.ExcuteUpdate("delete tb_rpy_textreply where id = @0", Ids[i]);
                        s.ExcuteUpdate("delete tb_rpy_keywords where biz_id = @0 and biz_type = @1 ", Ids[i], KeyWordBizType.TextReply);
                    }
                    s.Commit();
                    return StateCode.State_200;
                }
                catch
                {
                    s.RollBack();
                    return StateCode.State_500;
                }
            }
        }

        /// <summary>
        /// 删除文本回复
        /// </summary>
        /// <param name="id"></param>
        /// <param name="account_id"></param>
        public void DeleteTextReply(int id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    s.ExcuteUpdate("delete tb_rpy_textreply where id =@0 ", id);
                    s.ExcuteUpdate("delete tb_rpy_keywords where biz_id=@0 and biz_type=@1 ", id, KeyWordBizType.TextReply);
                    s.Commit();
                }
                catch
                {
                    s.RollBack();
                }
            }
        }

        /// <summary>
        /// 获取已经定义文本回复
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return (int)s.ExecuteScalar("select count(id) from tb_rpy_textreply ");
            }
        }


        /// <summary>
        /// 获取文本响应信息
        /// </summary>
        /// <param name="wxRequest"></param>
        /// <param name="id"></param>
        /// <param name="globalConfig"></param>
        /// <returns></returns>
        public string GetWXResponseMsg(WXReqBaseMsg wxRequest, WeChatAccount account, long id)
        {

            TextReply textReply = base.GetById(id);

            WXRepTextReply wXRepTextReply = new WXRepTextReply();

            wXRepTextReply.CreateTime = DateTime.Now.Ticks;
            wXRepTextReply.FromUserName = wxRequest.ToUserName;
            wXRepTextReply.ToUserName = wxRequest.FromUserName;
            wXRepTextReply.MsgType = WXRepMsgType.text;

            //文本回复内容
            wXRepTextReply.Content = textReply.reply_contents;

            return XmlConvertHelper.SerializeObject<WXRepTextReply>(wXRepTextReply);
        }

        /// <summary>
        /// 获取文本响应信息
        /// </summary>
        /// <param name="wxRequest"></param>
        /// <param name="account"></param>
        /// <param name="content"></param>
        /// <param name="globalConfig"></param>
        /// <returns></returns>
        public string GetWXResponseMsg(WXReqBaseMsg wxRequest, string content)
        {

            if (!string.IsNullOrEmpty(content))
            {
                WXRepTextReply wXRepTextReply = new WXRepTextReply();

                wXRepTextReply.CreateTime = DateTime.Now.Ticks;
                wXRepTextReply.FromUserName = wxRequest.ToUserName;
                wXRepTextReply.ToUserName = wxRequest.FromUserName;
                wXRepTextReply.MsgType = WXRepMsgType.text;

                //文本回复内容
                wXRepTextReply.Content = content;

                return XmlConvertHelper.SerializeObject<WXRepTextReply>(wXRepTextReply);
            }
            return string.Empty;

        }


    }
}
