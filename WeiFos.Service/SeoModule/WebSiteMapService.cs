using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.Entity.SeoModule;
using WeiFos.ORM.Data;

namespace WeiFos.Service.SeoModule
{
    /// <summary>
    /// 站点地图Service
    /// @author yewei 
    /// @date 2013-09-11
    /// </summary>
    public class WebSiteMapService : BaseService<WebSiteMap>
    {

        /// <summary>
        /// 根据上级ID获取
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<WebSiteMap> GetListByParentId(int parentId)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<WebSiteMap>("where parent_id = @0", parentId);
            }
        }


    }

}
