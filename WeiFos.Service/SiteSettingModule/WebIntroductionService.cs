using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data;
using WeiFos.Entity.Common;
using WeiFos.Entity.SiteSettingModule; 
using WeiFos.Entity.Enums;

namespace WeiFos.Service.SiteSettingModule
{
    /// <summary>
    /// 资讯 Service
    /// @author yewei 
    /// @date 2015-08-29
    /// </summary>
    public class WebIntroductionService : BaseService<WebIntroduction>
    {


        /// <summary>
        /// 获取企业介绍
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public WebIntroduction GetByType(string type)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<WebIntroduction>("where type = @0", type);
            }
        }


        /// <summary>
        /// 保存企业信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="webIntroduction"></param>
        /// <returns></returns>
        public StateCode Save(long userId, WebIntroduction webIntroduction)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    webIntroduction.title = "";
                    if (webIntroduction.id == 0)
                    {
                        int exist = s.Exist<WebIntroduction>("where type = @0", webIntroduction.type);
                        if (exist > 0) return StateCode.State_1;

                        webIntroduction.created_date = DateTime.Now;
                        webIntroduction.created_user_id = userId;
                        s.Insert(webIntroduction);
                    }
                    else
                    {
                        webIntroduction.updated_date = DateTime.Now;
                        webIntroduction.updated_user_id = userId;
                        s.Update(webIntroduction);
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




    }
}
