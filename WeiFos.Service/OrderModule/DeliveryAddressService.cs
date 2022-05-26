using WeiFos.ORM.Data;
using WeiFos.Entity.OrderModule;

namespace WeiFos.Service.MessageModule
{
    /// <summary>
    /// 订单收货地址信息Service
    /// @author yewei 
    /// @date 2014-04-17
    /// </summary>
    public class DeliveryAddressService : BaseService<DeliveryAddress> 
    {

        /// <summary>
        /// 根据订单ID获取收货信息表
        /// </summary>
        /// <param name="order_id"></param>
        /// <returns></returns>
        public DeliveryAddress GetByOrderId(long order_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<DeliveryAddress>("where order_id = @0 ", order_id);
            }
        }


    }
}
