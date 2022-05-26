using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data;
using WeiFos.Entity.DistributionModule;
using WeiFos.Entity.Enums;

namespace WeiFos.Service.DistributionModule
{

    /// <summary>
    /// 物流公司 Service
    /// @author yewei
    /// add by  @date 2015-10-28
    /// </summary>
    public class LogisticsCompanyService : BaseService<LogisticsCompany>
    {


        /// <summary>
        /// 保存导购分类
        /// </summary>
        /// <param name="sys_user_id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public StateCode Save(long sys_user_id, LogisticsCompany entity)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    if (entity.id == 0)
                    {
                        //创建用户ID
                        entity.created_user_id = sys_user_id;
                        //创建时间
                        entity.created_date = DateTime.Now;
                        s.Insert(entity);
                    }
                    else
                    {
                        //修改用户ID
                        entity.updated_user_id = sys_user_id;
                        //修改时间
                        entity.updated_date = DateTime.Now;
                        s.Update(entity);
                    }
                    return StateCode.State_200;
                }
                catch
                {
                    return StateCode.State_500;
                }
            }
        }



        /// <summary>
        /// 检查品牌名称是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public int ExistName(string name, long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //品牌是否存在该名词
                return s.Exist<LogisticsCompany>("where name = @0 and id != @1", name, id);
            }
        }



    }
}