using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data;
using WeiFos.Entity.BizTypeModule;
using WeiFos.Entity.Common;
using WeiFos.Entity.SiteSettingModule;
using WeiFos.Entity.Enums;
using System.Data;

namespace WeiFos.Service.SiteSettingModule
{

    /// <summary>
    /// 资讯 Service
    /// @author yewei 
    /// @date 2015-08-29
    /// </summary>
    public class PartnerService : BaseService<Partner>
    {

        /// <summary>
        /// 保存资讯类别
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="informt"></param>
        /// <param name="imgmsg"></param>
        /// <returns></returns>
        public StateCode Save(long userId, Partner partner, string imgmsg)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    if (partner.id == 0)
                    {
                        //创建用户ID
                        partner.created_user_id = userId;
                        //创建时间
                        partner.created_date = DateTime.Now;
                        s.Insert<Partner>(partner);
                    }
                    else
                    {
                        //修改用户ID
                        partner.updated_user_id = userId;
                        //修改时间
                        partner.updated_date = DateTime.Now;
                        s.Update<Partner>(partner);
                    }

                    //判断是否存在图片信息
                    if (!string.IsNullOrEmpty(imgmsg))
                    {
                        //去除重复图片
                        s.ExcuteUpdate("update tb_img set biz_id = 0 where biz_type = @0 and biz_id = @1", ImgType.Partner, partner.id);
                        //修改图片
                        s.ExcuteUpdate("update tb_img set biz_id = @0 where biz_type = @1 and file_name = @2", partner.id, ImgType.Partner, imgmsg);
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
                        s.ExcuteUpdate("delete tb_img where biz_type = @0 and biz_id = @1 ", ImgType.Partner, ids[i]);
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
        /// 获取首页资讯信息
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public DataTable GetIndex()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                string sql = "select * from v_site_partner order by order_index desc";
                return s.Fill(sql, "");
            }
        }



    }
}
