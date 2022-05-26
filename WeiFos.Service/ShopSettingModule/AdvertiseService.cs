using System;
using System.Collections.Generic;
using WeiFos.ORM.Data;
using WeiFos.Entity.BizTypeModule;
using WeiFos.Entity.Common;
using WeiFos.Entity.ShopSettingModule;
using WeiFos.Entity.Enums;
using WeiFos.Entity.ResourceModule;

namespace WeiFos.Service.ShopSettingModule
{
    /// <summary>
    /// 广告图 Service
    /// @author yewei 
    /// @date 2013-12-11
    /// </summary>
    public class AdvertiseService : BaseService<Advertise>
    {


        /// <summary>
        /// 保存导购图
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="adv"></param>
        /// <param name="imgmsg"></param>
        /// <returns></returns>
        public StateCode Save(long userID, Advertise adv, string imgmsg)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
       
                    //判断是否存在图片信息
                    if (!string.IsNullOrEmpty(imgmsg))
                    {
                        //去除重复图片
                        s.ExcuteUpdate("update tb_img set biz_id = 0 where biz_type = @0 and biz_id = @1 ", ImgType.Advertise, adv.id);
                        //去除重复图片
                        s.ExcuteUpdate("update tb_img set biz_id = @0 where biz_type = @1 and file_name = @2  ", adv.id, ImgType.Advertise, imgmsg);
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
        /// 设置状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isenable"></param>
        public StateCode SetEnable(int id, bool isshow)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.ExcuteUpdate("update tb_fnt_banner set is_show = @0 where id = @1", isshow, id);
                    return StateCode.State_200;
                }
                catch
                {
                    return StateCode.State_500;
                }
            }
        }



        #region 商城前端方法



        /// <summary>
        /// 根据商品分类ID获取
        /// </summary>
        /// <param name="ctgyId"></param>
        /// <returns></returns>
        public List<Advertise> GetIndex()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<Advertise>("where is_show = @0 order by order_index desc", true);
            }
        }


        #endregion



    }
}