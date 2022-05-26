using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Entity.BizTypeModule;
using WeiFos.Entity.Common;
using WeiFos.Entity.SiteSettingModule;
using WeiFos.ORM.Data;
using WeiFos.Entity.Enums;
using System.Data;
using WeiFos.Entity.SystemModule;

namespace WeiFos.Service.SiteSettingModule
{
    /// <summary>
    /// 资讯 Service
    /// @author yewei 
    /// @date 2015-08-29
    /// </summary>
    public class InformtService : BaseService<Informt>
    {


        /// <summary>
        /// 获取首页资讯信息
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public DataTable GetIndex(ConfigParam config)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                if (config != null)
                {
                    string sql = "select * from v_info_informt where is_index = @0 and is_enable = @1 and cgty_id in (" + config.config_value + ") order by order_index desc";
                    return s.Fill(sql, true, true);
                }
                return null;
            }
        }

        

        /// <summary>
        /// 保存资讯类别
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="informt"></param>
        /// <param name="imgmsg"></param>
        /// <returns></returns>
        public StateCode Save(long userId, Informt informt, string imgmsg)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    if (informt.id == 0)
                    {
                        //创建用户ID
                        informt.created_user_id = userId;
                        //创建时间
                        informt.created_date = DateTime.Now;
                        s.Insert<Informt>(informt);
                    }
                    else
                    {
                        //修改用户ID
                        informt.updated_user_id = userId;
                        //修改时间
                        informt.updated_date = DateTime.Now;
                        s.Update<Informt>(informt);
                    }

                    //判断是否存在图片信息
                    if (!string.IsNullOrEmpty(imgmsg))
                    {
                        //去除重复图片
                        s.ExcuteUpdate("update tb_img set biz_id = 0 where biz_type = @0 and biz_id = @1", ImgType.Informt, informt.id);
                        //修改图片
                        s.ExcuteUpdate("update tb_img set biz_id = @0 where biz_type = @1 and file_name = @2", informt.id, ImgType.Informt, imgmsg);
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

        /// <summary>
        /// 删除选中
        /// </summary>
        /// <param name="ids"></param>
        public void Deletes(long[] ids)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    for (int i = 0; i < ids.Count(); i++)
                    {
                        //删除资讯
                        s.Delete<Informt>(ids[i]);
                        //删除商品图片数据
                        s.ExcuteUpdate("delete tb_img where biz_type = @0 and biz_id = @1 ", ImgType.Informt, ids[i]);
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
        /// 获取当前文章上一篇
        /// </summary>
        /// <param name="nextArticleId"></param>
        /// <returns>Article</returns>
        public Informt GetLast(long bId, long cgtyId)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                Informt informt = s.Get<Informt>(bId);
                if (informt != null)
                {
                    List<Informt> articlelist = s.GetTop<Informt>(1, " WHERE order_index < @0 and cgty_id = @1 and is_enable = @2 order by order_index desc ", informt.order_index, cgtyId, true);
                    if (articlelist != null && articlelist.Count > 0)
                    {
                        return articlelist[0];
                    }
                }
                return null;
            }
        }


        /// <summary>
        /// 获取当前文章下一篇
        /// </summary>
        /// <param name="nextArticleId"></param>
        /// <returns></returns>
        public Informt GetNext(long bId, long cgtyId)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                Informt informt = s.Get<Informt>(bId);
                if (informt != null)
                {
                    List<Informt> articlelist = s.GetTop<Informt>(1, " WHERE order_index > @0 and cgty_id = @1 order by order_index asc ", informt.order_index, cgtyId, true);
                    if (articlelist != null && articlelist.Count > 0)
                    {
                        return articlelist[0];
                    }
                }
                return null;
            }
        }



        /// <summary>
        /// 获取当前文章下一篇
        /// </summary>
        /// <param name="nextArticleId"></param>
        /// <returns></returns>
        public DataTable GetRecommend(long cgtyId)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            { 
                string sql = "select * from v_info_informt where recommend = @0 and cgty_id = @1 order by order_index asc";
                return s.Fill(sql, true, cgtyId);
            }
        }



        /// <summary>
        /// 增加浏览次数
        /// </summary>
        /// <param name="id"></param>
        public void UpdateViewCount(long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                s.ExcuteUpdate("update tb_info_informt set view_count = view_count+1 where id = @0", id);
            }
        }



    }
}
