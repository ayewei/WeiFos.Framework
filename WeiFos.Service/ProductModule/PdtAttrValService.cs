using WeiFos.ORM.Data;
using System;
using System.Collections.Generic;
using WeiFos.Entity.ProductModule;

namespace WeiFos.Service.ProductModule
{

    /// <summary>
    /// 平台 商品类型 基本属性值
    /// @author yewei
    /// add by  @date 2015-03-03
    /// </summary>
    public class PdtAttrValService : BaseService<PdtAttrVal>
    {

        /// <summary>
        /// 获取基础属性 商品值
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public List<PdtAttrVal> Gets(long productID)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<PdtAttrVal>("where product_id = @0", productID);
            }
        }


    }
}
