using WeiFos.Entity.Common;
using WeiFos.Entity.LogsModule;
using WeiFos.Entity.WeChatModule;
using WeiFos.Entity.WeChatModule.EntModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data;
using WeiFos.WeChat.TickeModule;
using WeiFos.Service;
using WeiFos.Entity.Enums;

namespace Solution.Service.WeChatModule.EntModule
{
    /// <summary>
    /// 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
    /// Copyright (c) 2013-2018 深圳微狐信息技术有限公司
    /// 创 建：叶委
    /// 日 期：2019-03-15 14:38:04
    /// 描 述：企业号信息业务逻辑
    /// </summary>
    public class WeChatAccountEntService: BaseService<WeChatAccountEnt>
    {

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public StateCode Save(long user_id, WeChatAccountEnt entity)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    if (entity.id == 0)
                    {
                        entity.created_date = DateTime.Now;
                        entity.created_user_id = user_id;
                        s.Insert(entity);
                    }
                    else
                    {
                        entity.updated_date = DateTime.Now;
                        entity.updated_user_id = user_id;
                        s.Update(entity);
                    }
                    return StateCode.State_200;
                }
                catch (Exception ex)
                {
                    s.Insert(new SystemLogs() { content = ex.ToString(), created_date = DateTime.Now, type = 1 });
                    return StateCode.State_500;
                }
            }
        }


        /// <summary>
        /// 获取商户信息
        /// </summary>
        /// <returns></returns>
        public WeChatAccountEnt Get()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<WeChatAccountEnt>("", "");
            }
        }


         


    }
}
