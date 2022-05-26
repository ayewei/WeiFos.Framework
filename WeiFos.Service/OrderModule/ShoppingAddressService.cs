using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data;
using WeiFos.Entity.Common;
using WeiFos.Entity.OrderModule;
using WeiFos.Entity.Enums;

namespace WeiFos.Service.OrderModule
{

    /// <summary>
    /// 订单收货地址信息Service
    /// @author yewei 
    /// @date 2014-04-17
    /// </summary>
    public class ShoppingAddressService : BaseService<ShoppingAddress>
    {


        /// <summary>
        /// 根据user_id获取收货地址
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public List<ShoppingAddress> GetList(long user_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<ShoppingAddress>(" where user_id = @0 ", user_id);
            }
        }



        /// <summary>
        /// 获取收货地址
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public ShoppingAddress Get(long id, long user_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<ShoppingAddress>(" where id = @0 and user_id = @1 ", id, user_id);
            }
        }



        /// <summary>
        /// 新增收货地址
        /// </summary>
        /// <param name="shippingAddress"></param>
        public StateCode Save(ShoppingAddress shippingAddress)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    if (shippingAddress.id == 0)
                    {
                        if (shippingAddress.is_default)
                        {
                            s.ExcuteUpdate("update tb_user_shipping_address set is_default = 0 where user_id = @0 ", shippingAddress.user_id);
                            shippingAddress.is_default = true;
                        }
                        s.Insert(shippingAddress);
                    }
                    else
                    {
                        s.Update(shippingAddress);
                    }
                    return StateCode.State_200;
                }
                catch
                {
                    return StateCode.State_500;
                }
            }
        }


        /// <summary>
        /// 获得默认地址
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public ShoppingAddress GetDefault(long user_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<ShoppingAddress>("where user_id = @0 and is_default = @1", user_id, true);
            }
        }


        /// <summary>
        /// 设置默认地址
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user_id"></param>
        public StateCode SetDefault(long id, long user_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    s.ExcuteUpdate("update tb_user_shipping_address set is_default = @0 where user_id = @1 ", false, user_id);
                    s.ExcuteUpdate("update tb_user_shipping_address set is_default = @0 where user_id = @1 and id = @2", true, user_id, id);
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


        /// <summary>
        /// 删除地址
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user_id"></param>
        public StateCode Del(long id, long user_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.ExcuteUpdate("delete tb_user_shipping_address where id = @0 and user_id = @1", id, user_id);
                    return StateCode.State_200;
                }
                catch
                {
                    return StateCode.State_500;
                }
            }
        }

    }
}