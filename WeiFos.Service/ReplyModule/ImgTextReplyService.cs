using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.Core;
using WeiFos.Entity.Common;
using WeiFos.Entity.ResourceModule;
using WeiFos.Entity.ReplyModule;
using WeiFos.ORM.Data;
using WeiFos.WeChat.WXResponse;
using WeiFos.WeChat.WXRequest;
using WeiFos.Service.ResourceModule;
using WeiFos.Core.XmlHelper;
using WeiFos.Entity.BizTypeModule;
using WeiFos.Entity.Enums;
using WeiFos.WeChat.WXBase;
using WeiFos.Entity.WeChatModule;
using WeiFos.Entity.SystemModule;

namespace WeiFos.Service.ReplyModule
{
    /// <summary>
    /// 图文回复Service
    /// @author yewei 
    /// @date 2013-10-22
    /// </summary>
    public class ImgTextReplyService : BaseService<ImgTextReply>
    {


        /// <summary>
        /// 保存图文回复
        /// 自定义 图文回复表 图片表
        /// </summary>
        /// <param name="imgTextReply"></param>
        /// <param name="keywords"></param>
        /// <param name="imgmsg"></param>
        /// <param name="imgtext_table"></param>
        /// <param name="img_table"></param>
        public StateCode Save(SysUser user, ImgTextReply imgTextReply, string[] keywords, string imgmsg)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    if (keywords != null)
                    {

                        #region 多图文引用

                        int[] moreIds = StringHelper.StringToIntArray(imgTextReply.quote_detailsIds);
                        List<int> mIds = new List<int>();
                        for (int i = 0; i < moreIds.Length; i++)
                        {
                            //该产品类型下面是否存在商品
                            object _count = s.ExecuteScalar("if exists (select id from tb_rpy_imgtextreply where id = @0 ) select '1' else select '0'", moreIds[i]);
                            int count = int.Parse(_count.ToString());
                            if (count > 0)
                            {
                                mIds.Add(moreIds[i]);
                            }
                        }
                        imgTextReply.quote_detailsIds = string.Join(",", mIds.Select(a => a.ToString()).ToArray());

                        #endregion

                        #region 推荐阅读图文关联

                        int[] recIds = StringHelper.StringToIntArray(imgTextReply.rec_detailsIds);
                        List<int> rIds = new List<int>();
                        for (int i = 0; i < recIds.Length; i++)
                        {
                            //该产品类型下面是否存在商品
                            object _count = s.ExecuteScalar("if exists (select id from tb_rpy_imgtextreply where id = @0) select '1' else select '0'", recIds[i]);
                            int count = int.Parse(_count.ToString());
                            if (count > 0)
                            {
                                rIds.Add(recIds[i]);
                            }
                        }
                        imgTextReply.rec_detailsIds = string.Join(",", rIds.Select(a => a.ToString()).ToArray());

                        #endregion

                        #region 图文回复

                        //新增
                        if (imgTextReply.id == 0)
                        {
                            imgTextReply.created_user_id = user.id;
                            imgTextReply.created_date = DateTime.Now;
                            s.Insert<ImgTextReply>(imgTextReply);
                        }
                        else
                        {
                            imgTextReply.updated_user_id = user.id;
                            imgTextReply.updated_date = DateTime.Now;
                            s.Update<ImgTextReply>(imgTextReply);
                        }

                        //如果是图文回复类型
                        if (imgTextReply.content_type == MsgContentType.ImgTextDetails)
                        {
                            s.ExcuteUpdate("update tb_rpy_imgtextreply set content_value = @0 where id = @1", imgTextReply.id, imgTextReply.id);
                        }

                        #endregion

                        #region 图文封面图片信息

                        //判断是否存在图片信息
                        if (!string.IsNullOrEmpty(imgmsg))
                        {
                            //去除重复图片
                            s.ExcuteUpdate("update tb_img set biz_id = 0 where biz_type = @0 and biz_id = @1 ", ImgType.ImgTextReply_Title, imgTextReply.id);
                            //去除重复图片
                            s.ExcuteUpdate("update tb_img set biz_id = @0 where biz_type = @1 and file_name = @2  ", imgTextReply.id, ImgType.ImgTextReply_Title, imgmsg);
                        }

                        #endregion

                        #region 关键词

                        s.ExcuteUpdate("delete tb_rpy_keywords where biz_id=@0 and biz_type=@1 ", imgTextReply.id, KeyWordBizType.ImgTextReply);
                        foreach (string k in keywords)
                        {
                            if (k != string.Empty)
                            {
                                KeyWord kw = s.Get<KeyWord>("where keyword = @0 ", k);
                                if (kw == null)
                                {
                                    KeyWord _keyword = new KeyWord()
                                    {
                                        keyword = k,
                                        biz_id = imgTextReply.id,
                                        biz_type = KeyWordBizType.ImgTextReply,

                                        updated_date = DateTime.Now,
                                        created_date = DateTime.Now
                                    };
                                    s.Insert<KeyWord>(_keyword);
                                }
                            }
                        }

                        #endregion

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
                        s.ExcuteUpdate("delete tb_img where biz_id = @0 and biz_type = @1 ", Ids[i], ImgType.ImgTextReply_Title);
                        s.ExcuteUpdate("delete tb_rpy_imgtextreply where id =@0 ", Ids[i]);
                        s.ExcuteUpdate("delete tb_rpy_keywords where biz_id=@0 and biz_type=@1 ", Ids[i], KeyWordBizType.ImgTextReply);
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
        /// 根据ID 获取多图文
        /// </summary>
        /// <param name="imgTextReplyIds"></param>
        /// <returns></returns>
        public List<ImgTextReply> GetTextReplysByIds(long[] imgTextReplyIds)
        {
            List<ImgTextReply> imgTextReplys = new List<ImgTextReply>();
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                for (int i = 0; i < imgTextReplyIds.Length; i++)
                {
                    ImgTextReply imgTextReply = s.Get<ImgTextReply>(imgTextReplyIds[i]);
                    if (imgTextReply != null)
                    {
                        imgTextReplys.Add(imgTextReply);
                    }
                }
            }
            return imgTextReplys;
        }

        /// <summary>
        /// 根据微官网栏目获取多图文
        /// </summary>
        /// <param name="cgtyId"></param>
        /// <param name="tableConfig"></param>
        /// <returns></returns>
        public List<ImgTextReply> GetByCgtyId(long cgtyId)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<ImgTextReply>(" where category_id = @1", cgtyId);
            }
        }

        /// <summary>
        /// 查询是否存在多个图文信息
        /// </summary>
        /// <param name="cgtyId"></param>
        /// <param name="tableConfig"></param>
        /// <returns></returns>
        public int GetCountByCgtyId(long cgtyId)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return (int)s.ExecuteScalar("select count(id) from tb_rpy_imgtextreply  where category_id = @0", cgtyId);
            }
        }

        /// <summary>
        /// 获取已经定义图文回复
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return (int)s.ExecuteScalar("select count(id) from tb_rpy_imgtextreply ");
            }
        }

        /// <summary>
        /// 获取图文回复
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ImgTextReply GetImgTextReply(long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<ImgTextReply>(" where id = @0 ", id);
            }
        }


        /// <summary>
        /// 获取多图文响应信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <param name="tableConfig"></param>
        /// <returns></returns>
        public string GetWXResponseMsg(WXReqBaseMsg request, WeChatAccount account, long id)
        {
            //当前图文信息
            ImgTextReply imgTextReply = base.GetById(id);

            //图文回复集合
            List<ImgTextReply> imgTextReplys = new List<ImgTextReply>();

            //图文回复微信数据交互中间对象
            List<WXRepImgTextReply> wXRepImgTextReply = new List<WXRepImgTextReply>();

            if (imgTextReply != null)
            {
                //将当前图文设置在第一个位置
                imgTextReplys.Add(imgTextReply);

                //获取多图文id
                long[] ids = StringHelper.StringToLongArray(imgTextReply.quote_detailsIds);

                //获取相关多图文信息
                imgTextReplys.AddRange(GetTextReplysByIds(ids));

                foreach (ImgTextReply it in imgTextReplys)
                {
                    //微信多图文
                    WXRepImgTextReply wxRepImgText = new WXRepImgTextReply();

                    //标题
                    wxRepImgText.Title = it.title;

                    //图文消息描述
                    wxRepImgText.Description = it.introduction;

                    //图片链接
                    wxRepImgText.PicUrl = ServiceIoc.Get<ImgService>().GetImgUrl(ImgType.ImgTextReply_Title, it.id);

                    //如果是图文详情
                    if (it.content_type == MsgContentType.ImgTextDetails)
                    {
                        it.content_value = it.id.ToString();
                    }

                    string url = MsgContentType.GetLinkByType(account, it.content_type, request.FromUserName, it.content_value);
                    //if (it.content_type == MsgContentType.ImgTextDetails)
                    //{
                    //    url = MsgContentType.GetLinkByType(account, webSite.id, it.content_type, request.FromUserName, it.content_value) + "&title=" + it.title;
                    //}
                    //else
                    //{
                    //    url = MsgContentType.GetLinkByType(account, webSite.id, it.content_type, request.FromUserName, it.content_value);
                    //}

                    //链接地址
                    wxRepImgText.Url = url.Trim();

                    wXRepImgTextReply.Add(wxRepImgText);

                    //保存图文统计信息
                    //StatsImgTexts statsImgTexts = new StatsImgTexts();
                    //statsImgTexts.req_datetime = DateTime.Now;
                    //statsImgTexts.title = it.title;
                    //statsImgTexts.sta_state = 1;
                    //ServiceIoc.Get<StatsImgTextsService>().Save(statsImgTexts);
                }

                return GetWXResponseMsg(request, wXRepImgTextReply);

            }
            return string.Empty;
        }


        /// <summary>
        /// 微信多图文回复序列化
        /// </summary>
        /// <param name="request"></param>
        /// <param name="wXRepImgTextReply"></param>
        /// <returns></returns>
        public string GetWXResponseMsg(WXReqBaseMsg request, List<WXRepImgTextReply> wXRepImgTextReply)
        {
            //多图文信息
            if (wXRepImgTextReply != null && wXRepImgTextReply.Count > 0)
            {
                WXRepNews wXRepBaseMsg = new WXRepNews();
                //接收人
                wXRepBaseMsg.ToUserName = request.FromUserName;
                //发送人
                wXRepBaseMsg.FromUserName = request.ToUserName;
                //创建时间
                wXRepBaseMsg.CreateTime = DateTime.Now.Ticks;
                //消息类型
                wXRepBaseMsg.MsgType = WXRepMsgType.news;
                //多图文数量
                wXRepBaseMsg.imgTextReplys = wXRepImgTextReply;
                //多图文数量
                wXRepBaseMsg.ArticleCount = wXRepImgTextReply.Count;

                return XmlConvertHelper.SerializeObject<WXRepNews>(wXRepBaseMsg);
            }
            return string.Empty;
        }




    }
}
