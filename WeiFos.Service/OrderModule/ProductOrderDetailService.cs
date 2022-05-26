using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.Entity.Common;
using WeiFos.ORM.Data;
using WeiFos.Entity.OrderModule;
using WeiFos.Entity.Enums;

namespace WeiFos.Service.MessageModule
{


    /// <summary>
    /// 商品订单详细Service 
    /// @author yewei 
    /// @date 2014-04-16
    /// </summary>
    public class ProductOrderDetailService : BaseService<ProductOrderDetail>
    {

        /// <summary>
        /// 商品订单详细列表
        /// </summary>
        /// <param name="order_id"></param>
        /// <param name="account_id"></param>
        /// <param name="tableConfig"></param>
        /// <returns></returns>
        public List<ProductOrderDetail> GetListByOrderId(long order_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<ProductOrderDetail>(" where order_id = @0  order by id desc", order_id);
            }
        }

        /// <summary>
        /// 根据订单ID获取详情数量
        /// </summary>
        /// <param name="order_id"></param>
        /// <param name="tableConfig"></param>
        /// <returns></returns>
        public string GetCountByOrderId(int order_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.ExecuteScalar("select count(id) from tb_odr_orderdetail where order_id = @0 ", order_id).ToString();
            }
        }


        /// <summary>
        /// 删除订单明细
        /// </summary>
        /// <param name="order_id"></param>
        /// <param name="bid"></param> 
        /// <returns></returns>
        public StateCode Delete(int order_id, int bid)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    ProductOrder order = s.Get<ProductOrder>(" where id = @0 ", order_id);

                    //验证该属性值
                    if (order != null)
                    {
                        object d_count = s.ExecuteScalar("select count(id) from tb_odr_orderdetail where order_id = @0", order_id);
                        if (int.Parse(d_count.ToString()) > 1)
                        {
                            //定单详细
                            ProductOrderDetail detail = s.Get<ProductOrderDetail>(@" where id = @0 and order_id = @1 ", bid, order_id);
                            if (detail != null)
                            {
                                decimal actual_amount = (decimal)(order.actual_amount - detail.actual_amount);
                                decimal total_amount = (decimal)(order.total_amount - detail.total_amount);
                                int total_weight = (order.total_weight - detail.total_weight) < 0 ? 0 : order.total_weight - detail.total_weight;

                                //修改订单金额
                                s.ExcuteUpdate("update tb_odr_order set actual_amount = @0,total_amount=@1,total_weight=@2 where id = @3 ", actual_amount, total_amount, total_weight, order_id);

                                //删除当前订单明细
                                s.ExcuteUpdate("delete tb_odr_orderdetail where id = @0 and order_id = @1", bid, order_id);
                            }
                            s.Commit();
                            return StateCode.State_200;
                        }
                        else
                        {
                            s.RollBack();
                            return StateCode.State_1;
                        }
                    }
                    else
                    {
                        s.RollBack();
                        return StateCode.State_201;
                    }
                }
                catch
                {
                    s.RollBack();
                    return StateCode.State_500;
                }
            }
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="order_id"></param>
        ///// <param name="tableConfig"></param>
        ///// <returns></returns>
        //public List<JsonProductSpec> GetProductDetailsSpec(int order_id, int account_id)
        //{
        //    //商品规格中间对象
        //    List<JsonProductSpec> jps = new List<JsonProductSpec>();

        //    using (ISession s = SessionFactory.Instance.CreateSession())
        //    {
        //        List<ProductOrderDetail> productOrderDetails = s.List<ProductOrderDetail>("@ " + tableConfig.tb_odr_orderdetail + " where order_id = @0 and account_id = @1  order by id desc", order_id, account_id);

        //        foreach (ProductOrderDetail pod in productOrderDetails)
        //        {
        //            ProductSpecDetails psd = s.Get<ProductSpecDetails>("@" + tableConfig.tb_pdt_specdetails + " where id = @0 and product_id = @1 and account_id = @2 ", pod.specdetails_id, pod.product_id, pod.account_id);
        //            if (psd != null)
        //            {
        //                var js = new JsonProductSpec();
        //                //所属商品规格信息ID
        //                js.specdetails_id = psd.id;
        //                //所属账号ID
        //                js.account_id = psd.account_id;
        //                //商品ID
        //                js.product_id = psd.product_id;
        //                //商品成本价
        //                js.cost_price = psd.cost_price;
        //                //商品价格
        //                js.product_price = psd.product_price;
        //                //商品库存
        //                js.stock_number = psd.stock_number;
        //                //商品重量
        //                js.weight = psd.weight;

        //                List<ProductSpec> productSpecs = s.List<ProductSpec>("@" + tableConfig.tb_pdt_spec + " where product_id = @0 and account_id = @1 and specdetails_id = @2", psd.product_id, psd.account_id, psd.id);
        //                if (productSpecs.Count > 0)
        //                {
        //                    js.specvalue_ids = string.Join(",", productSpecs.Select(ps => ps.specname_id.ToString() + "_" + ps.pdt_specvalue_id.ToString()).ToArray());
        //                }
        //                jps.Add(js);
        //            }
        //        }
        //    }
        //    return jps;
        //}


        /// <summary>
        /// 获取商品购买总数
        /// </summary>
        /// <param name="product_id"></param>
        /// <param name="account_id"></param>
        /// <param name="tableConfig"></param>
        /// <returns></returns>
        public int GetProductByCount(int product_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                object count = s.ExecuteScalar("select count(id) from tb_odr_orderdetail where product_id = @0 ", product_id);

                return int.Parse(count.ToString());
            }
        }

    }
}
