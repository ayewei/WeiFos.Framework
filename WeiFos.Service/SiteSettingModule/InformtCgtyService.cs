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
using WeiFos.Entity.SystemModule;

namespace WeiFos.Service.SiteSettingModule
{
    /// <summary>
    /// 资讯分类 Service
    /// @author yewei 
    /// @date 2015-01-09
    /// </summary>
    public class InformtCgtyService : BaseService<InformtCgty>
    {


        /// <summary>
        /// 保存资讯类别
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="infoCgty"></param>
        /// <param name="imgmsg"></param>
        /// <returns></returns>
        public StateCode Save(long userId, InformtCgty infoCgty, string imgmsg)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    if (infoCgty.id == 0)
                    {
                        //创建用户ID
                        infoCgty.created_user_id = userId;
                        //创建时间
                        infoCgty.created_date = DateTime.Now;
                        s.Insert<InformtCgty>(infoCgty);
                    }
                    else
                    {
                        //修改用户ID
                        infoCgty.updated_user_id = userId;
                        //修改时间
                        infoCgty.updated_date = DateTime.Now;
                        s.Update<InformtCgty>(infoCgty);
                    }

                    //判断是否存在图片信息
                    if (!string.IsNullOrEmpty(imgmsg))
                    {
                        //去除重复图片
                        s.ExcuteUpdate("update tb_img set biz_id = 0 where biz_type = @0 and biz_id = @1", ImgType.InformtCgty, infoCgty.id);
                        //修改图片
                        s.ExcuteUpdate("update tb_img set biz_id = @0 where biz_type = @1 and file_name = @2", infoCgty.id, ImgType.InformtCgty, imgmsg);
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
        /// 根据上级ID获取
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<InformtCgty> GetListByParentId(int parentId)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<InformtCgty>("where parent_id = @0", parentId);
            }
        }



        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isenable"></param>
        public void SetEnable(int id, bool isenable)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    int exist = s.Exist<InformtCgty>("where id = @0", id);
                    if (exist > 0)
                    {
                        s.ExcuteUpdate("update tb_info_category set is_enable = @0 where id = @1", isenable, id);

                        //将自己的子菜单设置不可用
                        List<InformtCgty> cgtys = s.List<InformtCgty>("where ParentID = @0", id);
                        if (cgtys != null && cgtys.Count() > 0)
                        {
                            foreach (InformtCgty m in cgtys)
                            {
                                s.ExcuteUpdate("update tb_info_category set is_enable = @0 where id = @1", isenable, m.id);
                            }
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
        /// 根据上级ID获取
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<InformtCgty> GetIndexCtgy(ConfigParam config)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                if (config != null)
                {
                    //资讯类别
                    List<InformtCgty> cgtys = new List<InformtCgty>();
                    foreach (string id in config.config_value.Split(','))
                    {
                        InformtCgty cgty = s.Get<InformtCgty>(long.Parse(id));
                        cgtys.Add(cgty);
                    }
                    return cgtys;
                }
                return null;
            }
        }






    }
}