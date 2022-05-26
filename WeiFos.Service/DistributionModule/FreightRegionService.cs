using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data;
using WeiFos.Entity.DistributionModule;
 

namespace WeiFos.Service.DistributionModule
{
    /// <summary>
    /// 运费模板详细区域 Service
    /// @author yewei
    /// add by  @date 2015-10-28
    /// </summary>
    public class FreightRegionService : BaseService<FreightRegion>
    {


        /// <summary>
        /// 根据运费模板ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<FreightRegion> GetByTemplateRegionID(long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //品牌是否存在该名词
                return s.List<FreightRegion>("where TemplateRegionID = @0", id);
            }
        }




    }
}
