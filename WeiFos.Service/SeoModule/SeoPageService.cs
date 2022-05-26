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
    /// @date 2013-09-10
    /// </summary>
    public class SeoPageService : BaseService<SeoPage>
    {


        /// <summary>
        /// 根据地址获取页面seo信息
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public SeoPage GetByUrl(string url)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<SeoPage>("where page_url = @0 ", url);
            }
        }


        /// <summary>
        /// 获取默认确实seo信息
        /// </summary>
        /// <returns></returns>
        public SeoPage GetDefault()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<SeoPage>("where is_default = @0 ", true);
            }
        }



        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="seoPage"></param>
        /// <returns></returns>
        public StateCode Save(long user_id, SeoPage seoPage)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    if (seoPage.is_default) s.ExcuteUpdate("update tb_seo_page set is_default = @0", false);

                    if (seoPage.id > 0)
                    {
                        seoPage.updated_date = DateTime.Now;
                        seoPage.updated_user_id = user_id;
                        s.Update(seoPage);
                    }
                    else
                    {
                        seoPage.created_date = DateTime.Now;
                        seoPage.created_user_id = user_id;
                        s.Insert(seoPage);
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





    }
}

