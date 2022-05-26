using WeiFos.Entity.BizTypeModule;
using WeiFos.Entity.Common;
using WeiFos.Entity.SiteSettingModule;
using WeiFos.Entity.SystemModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data;
using WeiFos.Entity.Enums;
using WeiFos.Entity.OrgModule;

namespace WeiFos.Service.SiteSettingModule
{


    /// <summary>
    /// 成功案例 Service
    /// @author yewei 
    /// @date 2015-02-15
    /// </summary>
    public class SuccessfulCaseService : BaseService<SuccessfulCase>
    {



        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sysUser"></param>
        /// <param name="entity"></param>
        /// <param name="imgmsg"></param>
        /// <returns></returns>
        public StateCode Save(SysUser sysUser, SuccessfulCase entity, string imgmsg)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    if (entity.id != 0)
                    {
                        entity.updated_date = DateTime.Now;
                        entity.updated_user_id = sysUser.id;
                        s.Update(entity);
                    }
                    else
                    {
                        entity.created_date = DateTime.Now;
                        entity.created_user_id = sysUser.id;
                        s.Insert(entity);
                    }

                    //判断是否存在图片信息
                    if (!string.IsNullOrEmpty(imgmsg))
                    {
                        //去除重复图片
                        s.ExcuteUpdate("update tb_img set biz_id = 0 where biz_type = @0 and biz_id = @1 ", ImgType.Case_Cover, entity.id);
                        //去除重复图片
                        s.ExcuteUpdate("update tb_img set biz_id = @0 where biz_type = @1 and file_name = @2  ", entity.id, ImgType.Case_Cover, imgmsg);
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
        /// 获取当前案例上一案例
        /// </summary>
        /// <param name="bId"></param>
        /// <returns>Article</returns>
        public SuccessfulCase GetLast(long bId)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                SuccessfulCase entity = s.Get<SuccessfulCase>(bId);
                if (entity != null)
                {
                    List<SuccessfulCase> entitylist = s.GetTop<SuccessfulCase>(1, " WHERE order_index < @0 and is_enable = @1 order by order_index desc ", entity.order_index, true);
                    if (entitylist != null && entitylist.Count > 0)
                    {
                        return entitylist[0];
                    }
                }
                return null;
            }
        }




        /// <summary>
        /// 篇获取当前案例下一案例
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        public SuccessfulCase GetNext(long bId)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                SuccessfulCase entity = s.Get<SuccessfulCase>(bId);
                if (entity != null)
                {
                    List<SuccessfulCase> entitylist = s.GetTop<SuccessfulCase>(1, " WHERE order_index > @0 and cgty_id = @1 order by order_index asc ", entity.order_index, true);
                    if (entitylist != null && entitylist.Count > 0)
                    {
                        return entitylist[0];
                    }
                }
                return null;
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
                        s.Delete<SuccessfulCase>(ids[i]);
                        //删除商品图片数据
                        s.ExcuteUpdate("delete tb_img where biz_type = @0 and biz_id = @1 ", ImgType.Case_Cover, ids[i]);
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
