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
using WeiFos.Entity.ResourceModule;


namespace WeiFos.Service.SiteSettingModule
{
    /// <summary>
    /// 广告图 Service
    /// @author yewei 
    /// @date 2015-02-15
    /// </summary>
    public class BannerService : BaseService<Banner>
    { 


        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="ImgBizId"></param>
        /// <returns></returns>
        public Banner Get(int type)
        { 
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<Banner>("where type = @0", type);
            }
        }



        /// <summary>
        /// 获取首页广告图
        /// </summary>
        /// <returns></returns>
        public List<Banner> GetWhatIndexAdImg(int size)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.GetTop<Banner>(size, "where type = @0 order by order_index desc", BannerType.AppBanner);
            }
        }




        /// <summary>
        /// 保存广告图
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="webimg"></param>
        /// <param name="imgmsg"></param>
        public StateCode Save(long userId, Banner webimg, string imgmsg)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    if (webimg.id == 0)
                    {
                        webimg.created_user_id = userId;
                        webimg.created_date = DateTime.Now;
                        s.Insert<Banner>(webimg);
                    }
                    else
                    {
                        webimg.updated_user_id = userId;
                        webimg.updated_date = DateTime.Now;
                        s.Update<Banner>(webimg);
                    }

                    //是否是主图
                    if (webimg.is_main)
                    {
                        s.ExcuteUpdate("update tb_fnt_banner set is_main = @0 where type = @1 ", false, webimg.type);
                        s.ExcuteUpdate("update tb_fnt_banner set is_main = @0 where id = @1 ", true, webimg.id);
                    }

                    //判断是否存在图片信息
                    if (!string.IsNullOrEmpty(imgmsg))
                    {
                        //去除重复图片
                        s.ExcuteUpdate("update tb_img set biz_id = 0 where biz_type = @0 and biz_id = @1 ", ImgType.Banner, webimg.id);
                        //修改图片
                        s.ExcuteUpdate("update tb_img set biz_id = @0 where biz_type = @1 and file_name = @2", webimg.id, ImgType.Banner, imgmsg);
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
        /// 删除
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
                        s.Delete<Banner>(ids[i]);
                        //删除商品图片数据
                        s.ExcuteUpdate("delete tb_img where biz_type = @0 and biz_id = @1 ", ImgType.Banner, ids[i]);
                    }
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