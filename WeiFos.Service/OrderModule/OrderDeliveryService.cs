using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data;
using WeiFos.Entity.OrderModule;

namespace WeiFos.Service.MessageModule
{
    /// <summary>
    /// 商品订单发货信息Service 
    /// @author yewei 
    /// @date 2014-04-29
    /// </summary>
    public class OrderDeliveryService : BaseService<OrderDelivery>
    {

        /// <summary>
        /// 根据订单ID获取
        /// </summary>
        /// <returns></returns>
        public OrderDelivery GetByOrderId(long order_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<OrderDelivery>("where order_id = @0 ", order_id);
            }
        }



    }
}
