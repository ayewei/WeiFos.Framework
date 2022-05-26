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
    /// 订单发货人信息Service
    /// @author yewei
    /// @date 2014-04-17
    /// </summary>
    public class OrderConsignorService : BaseService<OrderConsignor> 
    {

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="orderConsignor"></param>
        public void Save(OrderConsignor orderConsignor)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    if (orderConsignor.is_default)
                    {
                        s.ExcuteUpdate("update tb_cfg_malltbconfig set is_default = @0", true);
                    }
                    s.Insert<OrderConsignor>(orderConsignor);
                    s.Commit();
                }
                catch
                {
                    s.RollBack();
                }
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="orderConsignor"></param>
        /// <param name="tableConfig"></param>
        public void _Update(OrderConsignor orderConsignor)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    if (orderConsignor.is_default)
                    {
                        //s.ExcuteUpdate("update tb_cfg_malltbconfig set is_default = @0 account_id = @1 ", true, orderConsignor.account_id);
                    }
                    s.Update<OrderConsignor>(orderConsignor);
                    s.Commit();
                }
                catch
                {
                    s.RollBack();
                }
            }
        }

    }
}
