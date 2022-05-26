using WeiFos.ORM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Entity.ProductModule;


namespace WeiFos.Service.ProductModule
{
    /// <summary>
    /// 平台 商品类型 扩展属性值
    /// @author yewei
    /// add by  @date 2015-03-03
    /// </summary>
    public class PdtExtAttrValService : BaseService<PdtExtAttrVal>
    {

        /// <summary>
        /// 获取扩展属性值
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public List<PdtExtAttrVal> Gets(long productID)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<PdtExtAttrVal>("where product_id = @0", productID);
            }
        }

    }
}
