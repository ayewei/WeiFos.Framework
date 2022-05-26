using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Entity.Enums;
using WeiFos.Entity.SiteSettingModule;
using WeiFos.ORM.Data;

namespace WeiFos.Service.SiteSettingModule
{
    /// <summary>
    /// 广告图 Service
    /// @author yewei 
    /// @date 2015-02-15
    /// </summary>
    public class SuccessfulCaseCgtyService : BaseService<SuccessfulCaseCgty>
    {



        /// <summary>
        /// 保存资讯类别
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cgty"></param>
        /// <returns></returns>
        public StateCode Save(long userId, SuccessfulCaseCgty cgty)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    if (cgty.id == 0)
                    {
                        //创建用户ID
                        cgty.created_user_id = userId;
                        //创建时间
                        cgty.created_date = DateTime.Now;
                        s.Insert(cgty);
                    }
                    else
                    {
                        //修改用户ID
                        cgty.updated_user_id = userId;
                        //修改时间
                        cgty.updated_date = DateTime.Now;
                        s.Update(cgty);
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
        /// <param name="isenable"></param>
        public StateCode SetEnable(long id, bool isenable)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.ExcuteUpdate("update tb_cases_cgty set is_enable = @0 where id = @1", isenable, id);
                    return StateCode.State_200;
                }
                catch
                {
                    return StateCode.State_500;
                }
            }
        }




        /// <summary>
        /// 获取案例分类列表
        /// </summary>
        /// <returns></returns>
        public List<SuccessfulCaseCgty> GetList()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<SuccessfulCaseCgty>("where is_enable = @0 order by order_index desc", true);
            }
        }

         


    }
}
