using WeiFos.ORM.Data;
using WeiFos.Entity.ProductModule;



namespace WeiFos.Service.ProductModule
{

    /// <summary>
    /// 商品自定义规格 Service
    /// @author yewei
    /// add by  @date 2015-04-09
    /// </summary>
    public class SpecCustomService : BaseService<SpecCustom>
    {
        /// <summary>
        /// 获取商品自定义规格根据 商品ID和规格值ID
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="specValueID"></param>
        /// <returns></returns>
        public SpecCustom GetSpecCustomBySpecValue(int productID,int specValueID)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<SpecCustom>("where product_id = @0 and  specvalue_id=@1", productID, specValueID);
            }
        }




    }
}
