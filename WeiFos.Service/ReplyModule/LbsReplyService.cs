using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.Core;
using WeiFos.Entity.Common;
using WeiFos.Entity.Enums;
using WeiFos.Entity.ReplyModule;
using WeiFos.Entity.ResourceModule;
using WeiFos.WeChat.WXRequest;
using WeiFos.WeChat.WXResponse;
using WeiFos.ORM.Data;
using WeiFos.Service.ResourceModule;
using WeiFos.Entity.BizTypeModule;
using WeiFos.Entity.SystemModule;

namespace WeiFos.Service.ReplyModule
{
    /// <summary>
    /// Lbs Service
    /// @author yewei 
    /// @date 2013-04-27
    /// </summary>
    public class LbsReplyService : BaseService<LbsReply>
    {


        public string GetWXResponseMsg(WXReqLocationMsg request)
        {
            DefaultSet defaultSet = ServiceIoc.Get<DefaultSetService>().GetDefaultSet();
            List<LbsReply> lbsReplys = GetLbsReplys(request.Location_Y, request.Location_X, defaultSet.lbs_distance);
            List<WXRepImgTextReply> wxRepImgTextReply = new List<WXRepImgTextReply>();
            if (lbsReplys.Count > 1)
            {
                wxRepImgTextReply.Add(new WXRepImgTextReply()
                {
                    Title = "查到2条记录,点击查看全部",
                    Description = "查到2条记录,点击查看全部",
                    PicUrl = "http://api.map.baidu.com/staticimage?width=400&height=300&zoom=11&markers=" + request.Location_Y + "," + request.Location_X,
                    Url = "" + "lbs/lbslist.aspx?lat=" + request.Location_X + "&lng=" + request.Location_Y
                });
            }
            for (int i = 0; i < lbsReplys.Count; i++)
            {
                wxRepImgTextReply.Add(new WXRepImgTextReply()
                {
                    Title = lbsReplys[i].name,
                    Description = lbsReplys[i].intro,
                    PicUrl = ServiceIoc.Get<ImgService>().GetImgUrl(ImgType.Activity_Start, lbsReplys[i].id),
                    Url = "" + "lbs/lbsDetail.aspx?bid=" + lbsReplys[i].id + "&lat=" + request.Location_X + "&lng=" + request.Location_Y
                });
            }
            if (wxRepImgTextReply.Count > 0)
                return ServiceIoc.Get<ImgTextReplyService>().GetWXResponseMsg(request, wxRepImgTextReply);
            return string.Empty;
        }



        /// <summary>
        /// 获取已经定义LBS回复
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return (int)s.ExecuteScalar("select count(id) from tb_rpy_lbs ");
            }
        }


        /// <summary>
        /// 获取LBS回复
        /// </summary>
        /// <param name="bid"></param>
        /// <returns></returns>
        public LbsReply Get(long bid)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<LbsReply>(" where id = @0 ", bid);
            }
        }



        public List<LbsReply> GetLbsReplys(double lng, double lat, double distance)
        {
            List<LbsReply> lbsReplys = new List<LbsReply>();
            double _distance = 0;
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                List<LbsReply> _lbsReplys = s.List<LbsReply>("where IsEnable = @0", true);
                foreach (LbsReply lbsreply in _lbsReplys)
                {
                    if (distance == 0)
                    {
                        lbsReplys.Add(lbsreply);
                    }
                    else
                    {
                        _distance = AlgorithmHelper.GetShortDistance(lng, lat, lbsreply.lng, lbsreply.lat);
                        if (_distance <= distance)
                            lbsReplys.Add(lbsreply);
                    }
                }
            }
            return lbsReplys;
        }


        /// <summary>
        /// 保存图文回复
        /// </summary>
        /// <param name="user"></param>
        /// <param name="lbsReply"></param>
        /// <param name="imgmsg"></param>
        public StateCode Save(SysUser user, LbsReply lbsReply, string imgmsg)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    if (lbsReply.id != 0)
                    {
                        lbsReply.updated_date = DateTime.Now;
                        lbsReply.updated_user_id = user.id;
                        s.Update<LbsReply>(lbsReply);
                    }
                    else
                    {
                        lbsReply.created_date = DateTime.Now;
                        lbsReply.created_user_id = user.id;
                        s.Insert<LbsReply>(lbsReply);
                    }

                    if (!string.IsNullOrEmpty(imgmsg) && imgmsg.IndexOf("#") != -1)
                    {
                        var filename = imgmsg.Split('#')[0];
                        var biztype = imgmsg.Split('#')[1];
                        Img img = s.Get<Img>("where file_name = @0 and biz_type = @1", filename, biztype);
                        if (img != null)
                        {
                            img.biz_id = lbsReply.id;
                            s.Update<Img>(img);
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
        /// 修改
        /// </summary>
        /// <param name="lbsReply"></param>
        /// <param name="titleImgMsg"></param>
        /// <param name="user"></param>
        public void Update(LbsReply lbsReply, string titleImgMsg, SysUser user)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    //保存LBS
                    lbsReply.created_date = DateTime.Now;
                    lbsReply.created_user_id = user.id;
                    s.Update<LbsReply>(lbsReply);

                    //保存图片
                    Img img;
                    if (!string.IsNullOrEmpty(titleImgMsg) && titleImgMsg.IndexOf("#") != -1)
                    {
                        var filename = titleImgMsg.Split('#')[0];
                        var biztype = titleImgMsg.Split('#')[1];
                        s.ExcuteUpdate("update T_Img set biz_id = 0  where biz_type = @0 and biz_id = @1", biztype, lbsReply.id);
                        img = s.Get<Img>("@T_Img where file_name = @0 and biz_type = @1", filename, biztype);
                        if (img != null)
                        {
                            img.biz_id = lbsReply.id;
                            s.Update<Img>(img);
                        }
                    }
                    s.Commit();
                }
                catch
                {
                    s.RollBack();
                }
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="bid"></param>
        public void Delete(int bid)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    //删除LBS
                    s.ExcuteUpdate("delete tb_rpy_lbs where id = @1", bid);
                    //删除LBS封面图片
                    s.ExcuteUpdate("delete T_Img where biz_id = @0 and  biz_type = @1", bid, ImgType.LbsReply_Title);
                    s.Commit();
                }
                catch
                {
                    s.RollBack();
                }
            }
        }




    }
}
