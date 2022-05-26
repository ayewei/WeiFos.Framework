using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.Entity.Enums;
using WeiFos.Entity.SeoModule;
using WeiFos.ORM.Data;

namespace WeiFos.Service.SeoModule
{
    /// <summary>
    /// 网站SEO关键字Service
    /// @author yewei
    /// @date 2013-09-22
    /// </summary>
    public class SeoKeyWordService : BaseService<SeoKeyWord>
    {

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public StateCode Save(long userId, SeoKeyWord entity)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    entity.remarks = entity.remarks ?? "";
                    if (entity.id == 0)
                    {
                        entity.created_user_id = userId;
                        entity.created_date = DateTime.Now;
                        s.Insert(entity);
                    }
                    else
                    {
                        entity.updated_user_id = userId;
                        entity.updated_date = DateTime.Now;
                        s.Update(entity);
                    }
                }
                catch
                {
                    return StateCode.State_500;
                }
                return StateCode.State_200;
            }
        }


        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="is_enable"></param>
        public StateCode SetEnable(long id, bool is_enable)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.ExcuteUpdate("update tb_seo_keyword set is_enable = @0 where id = @1", is_enable, id);
                    return StateCode.State_200;
                }
                catch
                {
                    return StateCode.State_500;
                }
            }
        }




    }
}
