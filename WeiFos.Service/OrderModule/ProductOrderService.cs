using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data;
using WeiFos.Core;
using Newtonsoft.Json;
using WeiFos.Entity.Common;
using WeiFos.Entity.ProductModule;
using WeiFos.Entity.BizTypeModule;
using WeiFos.Entity.SKUModule;
using WeiFos.Entity.OrderModule;
using WeiFos.ORM.Data.Restrictions;
using WeiFos.ORM.Data.Const;
using WeiFos.Service.DistributionModule;
using WeiFos.Entity.LogsModule;
using WeiFos.Entity.UserModule;
using WeiFos.Entity.SystemModule;
using System.Data;
using WeiFos.Service.SystemModule;
using WeiFos.Service.UserModule;
using WeiFos.Entity.Enums;
using WeiFos.Entity.ResourceModule;

namespace WeiFos.Service.OrderModule
{
    /// <summary>
    /// 商品订单 Service 
    /// @author yewei 
    /// @date 2014-04-16
    /// </summary>
    public class ProductOrderService : BaseService<ProductOrder>
    {



        /// <summary>
        /// 获取订单数据
        /// </summary>
        /// <param name="bid"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public ProductOrder Get(int bid, long user_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<ProductOrder>("where id = @0 and created_user_id = @1 ", bid, user_id);
            }
        }



        /// <summary>
        /// 通过定单编号获取定单
        /// </summary>
        /// <param name="serial_no">定单编号</param> 
        /// <returns></returns>
        public ProductOrder Get(string serial_no)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<ProductOrder>("where serial_no = @0 ", serial_no);
            }
        }



        /// <summary>
        /// 获取订单统计
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataTable GetSum(string keyword, string date)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //创建查询对象
                Criteria ct = new Criteria();
                ct.SetFromTables("tb_odr_order")
                .SetFields(new string[] {
                    "sum(actual_amount) actual_amount",
                    "sum(cost_price) cost_price",
                    "sum(freight) freight",
                    "sum(discount_amount) discount_amount",
                    "sum(coupon_amount) coupon_amount",
                    "(sum(actual_amount)  - sum(cost_price) - sum(freight) - sum(discount_amount) - sum(coupon_amount)) profit"
                });

                //查询表达式
                MutilExpression me = new MutilExpression();

                //查询关键词
                if (!string.IsNullOrEmpty(keyword))
                {
                    me.Add(new SingleExpression("serial_no", LogicOper.LIKE, "", keyword));
                }

                //日期
                if (!string.IsNullOrEmpty(date))
                {
                    DateTime startDate = Convert.ToDateTime(date.Split('-')[0]);
                    DateTime endDate = Convert.ToDateTime(date.Split('-')[1]);

                    if (startDate.CompareTo(endDate) == 0)
                    {
                        me.Add(new SingleExpression("created_date", LogicOper.BETWEEN, new[] { startDate.ToString(), endDate.AddDays(1).ToString() }));
                    }
                    else
                    {
                        me.Add(new SingleExpression("created_date", LogicOper.BETWEEN, new[] { startDate.ToString(), endDate.AddDays(1).ToString() }));
                    }
                }

                //类型
                if (me.Expressions.Count > 0)
                {
                    ct.SetWhereExpression(me);
                }

                return s.Fill(ct);
            }
        }



        /// <summary>
        /// 获取销售明细统计
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataTable GetTotalSaleDetails(string keyword, string date)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //创建查询对象
                Criteria ct = new Criteria();
                ct.SetFromTables("v_odr_order_detail")
                .SetFields(new string[] {
                    "sum(actual_amount) sale_total_amount",
                    "sum(cost_price) sale_cost",
                    "sum(count) total_num",
                    "(sum(actual_amount)  - sum(cost_price)) profit"
                });

                //查询表达式
                MutilExpression me = new MutilExpression();

                //查询关键词
                if (!string.IsNullOrEmpty(keyword))
                {
                    me.Add(new SingleExpression("", LogicOper.CUSTOM, "("));
                    me.Add(new SingleExpression("product_name", LogicOper.LIKE, "", keyword));
                    me.Add(new SingleExpression("sup_name", LogicOper.LIKE, " or ", keyword));
                    me.Add(new SingleExpression("", LogicOper.CUSTOM, "", ")"));
                }

                //日期
                if (!string.IsNullOrEmpty(date))
                {
                    DateTime startDate = Convert.ToDateTime(date.Split('-')[0]);
                    DateTime endDate = Convert.ToDateTime(date.Split('-')[1]);

                    if (startDate.CompareTo(endDate) == 0)
                    {
                        me.Add(new SingleExpression("created_date", LogicOper.BETWEEN, new[] { startDate.ToString(), endDate.AddDays(1).ToString() }));
                    }
                    else
                    {
                        me.Add(new SingleExpression("created_date", LogicOper.BETWEEN, new[] { startDate.ToString(), endDate.AddDays(1).ToString() }));
                    }
                }

                if (me.Expressions.Count() > 0)
                {
                    ct.SetWhereExpression(me);
                }

                return s.Fill(ct);
            }
        }




        /// <summary>
        /// 获取订单数据
        /// </summary>
        /// <param name="serial_no"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public ProductOrder Get(string serial_no, long user_id, string[] files = null)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                files = files ?? new string[] { "*" };
                return s.Get<ProductOrder>(files, "where serial_no = @0 and created_user_id = @1 ", serial_no, user_id);
            }
        }


        /// <summary>
        /// 新增定单
        /// </summary>
        /// <param name="orderProduct"></param>
        /// <param name="isShoppingCart"></param>
        /// <param name="productOrder"></param> 
        /// <param name="addressID"></param> 
        /// <returns></returns>
        public StateCode Save(List<JsonOrderProduct> orderProduct, ProductOrder productOrder, long addressID, bool isShoppingCart)
        {
            string freight_str = "";
            decimal freight = 0;
            long deliveryModeID = 0;

            //如果是自提则不计算运费
            if (productOrder.logistic_method == 0)
            {
                StateCode code = ServiceIoc.Get<DeliveryModeService>().GetDefaultFreight(orderProduct, addressID, out freight_str);
                if (code != StateCode.State_200) return code;
                //运费
                freight = decimal.Parse(freight_str.Split('#')[1]);
                //运费模板ID
                deliveryModeID = int.Parse(freight_str.Split('#')[0]);
            }

            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    //订单商品较验
                    foreach (var op in orderProduct)
                    {
                        //较验商品规格是否存在
                        ProductSku sku = s.Get<ProductSku>("where product_id = @0 and dbo.fn_check_specset(specset,@1) = 1 ", op.product_id, op.specset);
                        if (sku == null)
                        {
                            productOrder = null;
                            s.RollBack();
                            return StateCode.State_1;
                        }

                        //较验商品是否存在
                        Product product = s.Get<Product>("where id = @0 ", sku.product_id);
                        if (product == null)
                        {
                            productOrder = null;
                            s.RollBack();
                            return StateCode.State_1;
                        }

                        //较验商品库存
                        if (op.qty > sku.stock)
                        {
                            productOrder = null;
                            s.RollBack();
                            return StateCode.State_502;
                        }

                        //较验商品状态
                        if (!((bool)product.is_shelves && !(bool)product.is_delete && (DateTime.Now > product.shelves_sdate && DateTime.Now < product.shelves_edate)))
                        {
                            productOrder = null;
                            s.RollBack();
                            return StateCode.State_505;
                        }

                        //商品库存处理
                        sku.stock = sku.stock - op.qty;
                        s.Update<ProductSku>(sku);
                    }

                    //创建订单 
                    productOrder.serial_no = AlgorithmHelper.CreateNo();
                    productOrder.status = OrderStatus.WaitingPayment;
                    productOrder.delete_status = 0;
                    productOrder.created_date = DateTime.Now;
                    productOrder.total_amount = 0;
                    productOrder.actual_amount = 0;
                    productOrder.cost_price = 0;
                    productOrder.delivery_mode_id = deliveryModeID;
                    s.Insert<ProductOrder>(productOrder);

                    //总金额,成本价,优惠卷金额
                    decimal totalProdutPrice = 0, costProdutPrice = 0;

                    //商品的总重量,包邮商品的总重量
                    int total_weight = 0, sum_total_weight = 0;

                    int index = 0;

                    foreach (var op in orderProduct)
                    {
                        //商品规格
                        ProductSku sku = s.Get<ProductSku>("where product_id = @0 and dbo.fn_check_specset(specset,@1) = 1", op.product_id, op.specset);
                        //商品
                        Product product = s.Get<Product>(sku.product_id);

                        string[] arr = StringHelper.StringToArray(sku.specset);
                        StringBuilder specinfo = new StringBuilder();
                        foreach (string i in arr)
                        {
                            int specname_id = 0;
                            int specvalue_id = 0;
                            if (i.IndexOf("_") != -1)
                            {
                                int.TryParse(i.Split('_')[0], out specname_id);
                                int.TryParse(i.Split('_')[1], out specvalue_id);

                                SpecName specname = s.Get<SpecName>("where id = @0 ", specname_id);
                                SpecValue specvalue = s.Get<SpecValue>("where id = @0 ", specvalue_id);

                                if (specname != null && specvalue != null)
                                    specinfo.Append(string.Format("{0}：{1}; ", specname.name, specvalue.val));
                                else
                                    throw new Exception("商品规格信息异常");
                            }
                        }

                        //当前当商品成本
                        decimal cost_sum = sku.cost_price * op.qty;

                        //押金
                        decimal sum = sku.sale_price * op.qty;

                        totalProdutPrice += sum;
                        costProdutPrice += cost_sum;

                        //该商品是否包邮
                        if (!product.is_postage)
                        {
                            sum_total_weight += sku.weight * op.qty;
                        }

                        //计算重量
                        total_weight += sku.weight * op.qty;

                        //获取产品主图片
                        string mainPic = "";
                        List<Img> pictures = s.List<Img>("where biz_type = @0 and biz_id = @1", ImgType.Product_Cover, product.id);
                        Img img = pictures.Where(p => p.is_main).SingleOrDefault();
                        if (img != null) mainPic = img.is_webimg ? img.webimg_url : img.getThmImgUrl();

                        //创建订单明细
                        ProductOrderDetail orderDetail = new ProductOrderDetail()
                        {
                            order_id = productOrder.id,
                            product_id = product.id,
                            specset = op.specset,
                            product_name = product.name,
                            product_en_name = product.en_name,
                            product_img_url = mainPic,
                            spec_msg = specinfo.ToString().Trim(),
                            unit_price = sku.sale_price,
                            count = op.qty,
                            total_weight = sku.weight * op.qty,
                            order_index = ++index,
                            cost_price = cost_sum,
                            total_amount = sum,
                            actual_amount = sum
                        };
                        s.Insert(orderDetail);

                        //购物车删除商品
                        if (isShoppingCart)
                        {
                            s.ExcuteUpdate("delete tb_ord_shoppingcart where user_id = @0 and product_id = @1 and dbo.fn_check_specset(specset,@2) = 1", productOrder.created_user_id, orderDetail.product_id, orderDetail.specset ?? "");
                        }
                    }

                    #region 会员折扣模块

                    //会员折扣优惠金额
                    decimal discount = 0;
                    //累计消费金额 
                    object user_total_amount = s.ExecuteScalar("select COALESCE(SUM(total_amount),0) from tb_odr_order where created_user_id = @0 and is_pay = @1", productOrder.created_user_id, true);
                    //累计消费金额
                    decimal amount = decimal.Parse(user_total_amount.ToString());

                    //获取等级列表
                    List<MemberLevelSetting> levels = s.List<MemberLevelSetting>("order by total_amount desc");
                    foreach (var level in levels)
                    {
                        if (amount >= level.total_amount)
                        {
                            discount = totalProdutPrice * (100 - level.discount) / 100;
                            break;
                        }
                    }

                    //会员折扣
                    productOrder.discount_amount = discount;
                    #endregion

                    #region 优惠卷模块

                    if (productOrder.user_coupon_id != 0)
                    {
                        //存在未使用的优惠卷 
                        string coupon_sql = "where id = @0 and is_used = @1 and full_amount <= @2";
                        UserCoupon userCoupon = s.Get<UserCoupon>(coupon_sql, productOrder.user_coupon_id, false, totalProdutPrice);
                        if (userCoupon != null)
                        {
                            //优惠卷金额
                            productOrder.coupon_amount = userCoupon.coupon_amount;
                            //修改会员优惠卷状态
                            s.ExcuteUpdate("update tb_user_aty_coupon set is_used = @0 where id = @1", true, userCoupon.id);
                        }
                    }

                    #endregion

                    #region 发货单

                    ShoppingAddress sa = s.Get<ShoppingAddress>(addressID);
                    if (sa == null) throw new Exception("收货地址不存在");

                    //订单发货信息
                    OrderDelivery orderDelivery = new OrderDelivery();
                    orderDelivery.order_id = productOrder.id;
                    orderDelivery.province = sa.province;
                    orderDelivery.city = sa.city;
                    orderDelivery.area = sa.area;
                    orderDelivery.address = sa.address;
                    orderDelivery.contact = sa.contact;
                    orderDelivery.mobile = sa.mobile;
                    orderDelivery.postal_code = sa.postal_code;
                    orderDelivery.tel = sa.tel;

                    s.Insert<OrderDelivery>(orderDelivery);
                    #endregion

                    //总计金额(商品总金额+物流费用)
                    var totalPrice = totalProdutPrice + freight;
                    //成本价
                    productOrder.cost_price = costProdutPrice;
                    //配送费
                    productOrder.freight = freight;
                    //总金额
                    productOrder.total_amount = totalPrice;
                    //实付金额
                    productOrder.actual_amount = totalPrice - productOrder.coupon_amount - discount;
                    //总重量
                    productOrder.total_weight = total_weight;
                    //更新订单
                    s.Update<ProductOrder>(productOrder);

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
        /// 完成订单
        /// </summary>
        /// <param name="serial_number"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public StateCode FinishOrder(string serial_number, int user_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    ProductOrder productOrder = s.Get<ProductOrder>(@" where serial_no = @0 ", serial_number);
                    if (productOrder == null)
                    {
                        s.RollBack();
                        return StateCode.State_201;
                    }

                    if (productOrder.status == OrderStatus.PaymentsMade)
                    {
                        s.ExcuteUpdate("update tb_odr_order set status = @0  where id = @1", OrderStatus.Success, productOrder.id);
                        s.Commit();
                        return StateCode.State_200;
                    }
                    else
                    {
                        s.RollBack();
                        return StateCode.State_206;
                    }
                }
                catch
                {
                    s.RollBack();
                    return StateCode.State_500;
                }
            }
        }



        /// <summary>
        /// 自动关闭订单
        /// </summary>
        /// <param name="serial_number"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public StateCode AutoCloseOrder()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    //开启事务
                    s.StartTransaction();

                    //获取当前即将关闭的订单
                    List<ProductOrder> orders = s.List<ProductOrder>("where status = @0 and datediff(minute ,created_date,GETDATE()) >= 10", OrderStatus.WaitingPayment);

                    foreach (var o in orders)
                    {
                        //修改订单对应的用户使用的优惠卷
                        s.ExcuteUpdate("update tb_user_aty_coupon set is_used = @0 where id = @1", false, o.user_coupon_id);
                    }

                    //修改订单状态
                    s.ExcuteUpdate("update tb_odr_order set status = @0 where status = @1 and datediff(minute ,created_date,GETDATE()) >= 10 ", OrderStatus.Close, OrderStatus.WaitingPayment);

                    s.Commit();

                    return StateCode.State_200;
                }
                catch (Exception ex)
                {
                    s.RollBack();
                    return StateCode.State_500;
                }
            }
        }



        /// <summary>
        /// 订单退款
        /// </summary>
        /// <param name="pods">退款订单商品ID及数量</param>
        /// <param name="serial_number">订单编号</param>
        /// <param name="orderStatus">订单状态</param>
        /// <param name="account_id"></param> 
        /// <returns></returns>
        public StateCode ApplyRefund(OrderRefund refund, List<OrderRefundDetail> refundDetails, long user_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    ProductOrder order = s.Get<ProductOrder>(" where id = @0 and  created_user_id = @1", refund.order_id, user_id);
                    if (order == null) return StateCode.State_201;

                    //更新订单状态
                    order.status = OrderStatus.Refund;
                    s.Update<ProductOrder>(order);

                    //退款单
                    refund.created_user_id = user_id;
                    refund.order_serial_no = order.serial_no;
                    refund.status = OrderRefundStatus.Apply;
                    refund.order_actual_amount = order.actual_amount;
                    refund.created_date = DateTime.Now;
                    s.Insert<OrderRefund>(refund);

                    //退款总金额
                    decimal amount = 0;

                    //退款明细
                    foreach (OrderRefundDetail refundDetail in refundDetails)
                    {
                        var orderDetail = s.Get<ProductOrderDetail>(@" where product_id = @0 and order_id = @1", refundDetail.product_id, refund.order_id);
                        if (orderDetail != null)
                        {
                            //如果退订的商品数量大于订单商品数量
                            if (refundDetail.count > orderDetail.count)
                            {
                                s.RollBack();
                                return StateCode.State_504;
                            }
                            refundDetail.order_id = refund.order_id;
                            refundDetail.refund_id = refund.id;
                            refundDetail.product_name = orderDetail.product_name;
                            refundDetail.product_en_name = orderDetail.product_en_name;
                            refundDetail.product_serial_number = orderDetail.product_serial_number;
                            refundDetail.product_img_url = orderDetail.product_img_url;
                            refundDetail.spec_msg = orderDetail.spec_msg;
                            refundDetail.unit_price = orderDetail.unit_price;
                            refundDetail.total_amount = (orderDetail.unit_price == null ? 0 : (decimal)orderDetail.unit_price) * refundDetail.count;
                            s.Insert<OrderRefundDetail>(refundDetail);
                            amount += (decimal)refundDetail.total_amount;
                        }
                    }

                    //更新退款单
                    refund.refund_serial_no = WeiFos.Core.AlgorithmHelper.CreateNo() + refund.id;
                    refund.refund_total_amount = amount;
                    refund.refund_actual_amount = amount;
                    s.Update<OrderRefund>(refund);

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
        /// 发货操作
        /// </summary>
        /// <param name="orderDelivery"></param>
        /// <returns></returns>
        public StateCode SaveOrderSend(OrderDelivery orderDelivery)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    //订单详细
                    List<ProductOrderDetail> detailList = s.List<ProductOrderDetail>("where order_id = @0", orderDelivery.order_id);
                    OrderDelivery odr_delivery = s.Get<OrderDelivery>("where order_id = @0 ", orderDelivery.order_id);
                    foreach (var od in detailList)
                    {
                        //较验商品规格是否存在
                        ProductSku sku = s.Get<ProductSku>("where product_id = @0 and dbo.fn_check_specset(specset,@1) = 1 ", od.product_id, od.specset ?? "");
                        if (sku == null)
                        {
                            s.RollBack();
                            return StateCode.State_1;
                        }

                        //较验商品是否存在
                        Product product = s.Get<Product>("where id = @0 ", sku.product_id);
                        if (product == null)
                        {
                            s.RollBack();
                            return StateCode.State_1;
                        }

                        //较验商品库存
                        if (od.count > sku.stock)
                        {
                            s.RollBack();
                            return StateCode.State_202;
                        }

                        //商品库存更新
                        s.ExcuteUpdate("update tb_pdt_sku set stock = stock - " + od.count + " where id = @0", sku.id);

                        //商品销量更新
                        s.ExcuteUpdate("update tb_pdt_product set sales = sales + " + od.count + " where id = @0", sku.product_id);
                    }

                    //修改订单状态
                    s.ExcuteUpdate("update tb_odr_order set status = @0  where id = @1", OrderStatus.Sent, orderDelivery.order_id);

                    odr_delivery.province = orderDelivery.province;
                    odr_delivery.city = orderDelivery.city;
                    odr_delivery.area = orderDelivery.area;
                    odr_delivery.address = orderDelivery.address;
                    odr_delivery.postal_code = orderDelivery.postal_code;
                    odr_delivery.mobile = orderDelivery.mobile;
                    odr_delivery.tel = orderDelivery.tel;
                    odr_delivery.contact = orderDelivery.contact;
                    odr_delivery.tracking_no = orderDelivery.tracking_no;
                    odr_delivery.logistic_company = orderDelivery.logistic_company;
                    //修改发货信息
                    s.Update<OrderDelivery>(odr_delivery);

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
        /// 修改订单金额
        /// </summary>
        /// <param name="sys_user_id"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public StateCode UpdateAmount(long sys_user_id, ProductOrder order)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    ProductOrder tmp_order = s.Get<ProductOrder>("where serial_no = @0", order.serial_no);
                    if (tmp_order == null) return StateCode.State_551;

                    //订单金额为0
                    if (order.actual_amount == 0) return StateCode.State_556;

                    //修改订单金额
                    s.ExcuteUpdate("update tb_odr_order set remarks = @0,actual_amount = @1 where id = @2", order.remarks, order.actual_amount, tmp_order.id);

                    //订单操作记录
                    OrderRecord record = new OrderRecord();

                    record.serial_no = tmp_order.serial_no;
                    record.before_amount = tmp_order.actual_amount;
                    record.after_amount = order.actual_amount;
                    record.content = "修改订单金额";
                    record.created_user_id = sys_user_id;
                    record.created_date = DateTime.Now;
                    s.Insert(record);

                    s.Commit();
                    return StateCode.State_200;
                }
                catch (Exception ex)
                {
                    s.RollBack();
                    return StateCode.State_500;
                }
            }
        }



        /// <summary>
        /// 获取订单翻页
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="pageindex"></param>
        /// <param name="status"></param>
        /// <param name="order_no"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public string GetOrders(int PageSize, int pageindex, int status, string order_no, string date)
        {
            Criteria ct = new Criteria();
            ct.SetPageSize(PageSize)
            .SetStartPage(pageindex)
            .SetFields(new string[] { "id", "serial_no", "status", "created_date", "actual_amount", "remarks" }).AddOrderBy(new OrderBy("id", "desc"));

            MutilExpression me = new MutilExpression();

            //订单状态
            if (status != 0)
            {
                me.Add(new SingleExpression("status", LogicOper.EQ, status));
            }

            if (!string.IsNullOrEmpty(order_no))
            {
                me.Add(new SingleExpression("serial_no", LogicOper.LIKE, order_no.Trim()));
            }

            if (me.Expressions.Count > 0)
            {
                //设置查询条件
                ct.SetWhereExpression(me);
            }

            try
            {
                using (ISession s = SessionFactory.Instance.CreateSession())
                {
                    //订单列表
                    List<ProductOrder> orders = s.List<ProductOrder>(ct);
                    foreach (ProductOrder o in orders)
                    {
                        //订单详情
                        o.details = s.List<ProductOrderDetail>("where order_id = @0", o.id);
                    }

                    return JsonConvert.SerializeObject(new
                    {
                        state = StateCode.State_200,
                        startPage = ct.StartPage,
                        totalRow = ct.TotalRow,
                        Data = new { pageData = orders }
                    });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new
                {
                    state = StateCode.State_500
                });
            }
        }



        /// <summary>
        /// 获取近三天订单
        /// </summary>
        public DataTable GetDefaultOrders()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Fill("select id,serial_no,status,created_date,actual_amount,total_amount from  tb_odr_order where DateDiff(dd,getdate() - 3 ,getdate())<=3");
            }
        }


        /// <summary>
        /// 获取缺省页统计信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDefaultStatis()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Fill("execute [dbo].[default_order_statis]");
            }
        }



        /// <summary>
        /// 获取缺省页统计报表
        /// </summary>
        /// <returns></returns>
        public DataTable GetDefaultStatisReport()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Fill("select * from v_odr_order_statis order by daynum asc");
            }
        }




        #region API端方法


        /// <summary>
        /// 获取订单翻页
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="PageSize"></param>
        /// <param name="pageindex"></param>
        /// <param name="status"></param>
        /// <param name="refund_status"></param>
        /// <returns></returns>
        public dynamic GetOrders(long user_id, int PageSize, int pageindex, int status, int refund_status)
        {
            Criteria ct = new Criteria();
            ct.SetPageSize(PageSize)
            .SetStartPage(pageindex)
            .SetFields(new string[] { "id", "type", "serial_no", "actual_amount", "refund_status", "created_date", "status" }).AddOrderBy(new OrderBy("id", "desc"));

            MutilExpression me = new MutilExpression();
            me.Add(new SingleExpression("created_user_id", LogicOper.EQ, user_id));

            if (refund_status > 0)
            {
                me.Add(new SingleExpression("refund_status", LogicOper.GT, 0));
            }
            else
            {
                if (status != 0)
                {
                    if (status == 10)
                    {
                        me.Add(new SingleExpression("", LogicOper.CUSTOM, "("));
                        me.Add(new SingleExpression("status", LogicOper.EQ, "", 3));
                        me.Add(new SingleExpression("status", LogicOper.EQ, " or ", 10));
                        me.Add(new SingleExpression("", LogicOper.CUSTOM, "", ")"));

                        me.Add(new SingleExpression("refund_status", LogicOper.EQ, 0));
                    }
                    else
                    {
                        me.Add(new SingleExpression("status", LogicOper.EQ, status));
                    }
                }
            }


            //未删除的订单
            me.Add(new SingleExpression("delete_status", LogicOper.EQ, 0));

            if (me.Expressions.Count > 0)
            {
                //设置查询条件
                ct.SetWhereExpression(me);
            }

            try
            {
                using (ISession s = SessionFactory.Instance.CreateSession())
                {
                    //订单列表
                    List<ProductOrder> orders = null;
                    if (pageindex <= (ct.TotalRow / PageSize + ct.TotalRow % PageSize == 0 ? 0 : 1))
                    {
                        orders = s.List<ProductOrder>(ct);
                    }
                    else
                    {
                        orders = new List<ProductOrder>();
                    }

                    foreach (ProductOrder o in orders)
                    {
                        List<string> imgs = new List<string>();
                        //商品订单
                        if (o.type == 0)
                        {
                            List<ProductOrderDetail> details = s.List<ProductOrderDetail>("where order_id = @0", o.id);
                            foreach (ProductOrderDetail d in details)
                            {
                                imgs.Add(d.product_img_url ?? "");
                            }
                            o.details_count = details.Sum(d => d.count);
                            o.detail_imgs = imgs;
                        }
                    }

                    return new
                    {
                        orders,
                        totalRow = ct.TotalRow,
                        State = StateCode.State_200
                    };
                }
            }
            catch (Exception ex)
            {
                var result = new
                {
                    State = StateCode.State_500
                };
                return JsonConvert.SerializeObject(result);
            }
        }


        /// <summary>
        /// 获取付尾款订单翻页
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="PageSize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public dynamic GetOrderFinals(long user_id, int PageSize, int pageindex)
        {
            Criteria ct = new Criteria();
            ct.SetPageSize(PageSize)
            .SetStartPage(pageindex)
            .SetFields(new string[] { "id", "type", "serial_no", "actual_amount", "refund_status", "created_date", "status" }).AddOrderBy(new OrderBy("id", "desc"));

            MutilExpression me = new MutilExpression();
            //所属用户
            me.Add(new SingleExpression("created_user_id", LogicOper.EQ, user_id));
            //部分付款
            me.Add(new SingleExpression("status", LogicOper.EQ, OrderStatus.PaymentPart));
            //未删除的订单
            me.Add(new SingleExpression("delete_status", LogicOper.EQ, 0));

            if (me.Expressions.Count > 0)
            {
                //设置查询条件
                ct.SetWhereExpression(me);
            }

            try
            {
                using (ISession s = SessionFactory.Instance.CreateSession())
                {
                    //订单列表
                    List<ProductOrder> orders = null;
                    if (pageindex <= ct.StartPage)
                    {
                        orders = s.List<ProductOrder>(ct);
                    }
                    else
                    {
                        orders = new List<ProductOrder>();
                    }

                    return new
                    {
                        orders,
                        totalRow = ct.TotalRow,
                        State = StateCode.State_200
                    };
                }
            }
            catch (Exception ex)
            {
                var result = new
                {
                    State = StateCode.State_500
                };
                return JsonConvert.SerializeObject(result);
            }
        }



        /// <summary>
        /// 获取已付订金订单
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public dynamic GetAdvances(long user_id)
        {
            try
            {
                using (ISession s = SessionFactory.Instance.CreateSession())
                {
                    //获取
                    object count = s.ExecuteScalar("select count(id) from tb_odr_order where type = 1 and status = 2 and created_user_id = @0", user_id);
                    return new
                    {
                        num = int.Parse(count.ToString()),
                        State = StateCode.State_200
                    };
                }
            }
            catch (Exception ex)
            {
                var result = new
                {
                    State = StateCode.State_500
                };
                return JsonConvert.SerializeObject(result);
            }
        }


        /// <summary>
        /// 生成预支付订单方法
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public string GetOrder(ProductOrder order)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                List<ProductOrderDetail> orderDetails;
                orderDetails = s.List<ProductOrderDetail>("where order_id = @0", order.id);
                if (orderDetails == null)
                {
                    throw new Exception("不存在订单详细数据");
                }

                //初始化微支付订单数据
                var orders = new
                {
                    no = order.serial_no,
                    total_fee = order.actual_amount,
                    body = orderDetails[0].spec_msg,
                    detail = string.Join(" ", orderDetails.Select(od => od.spec_msg += "×" + od.count)),
                };

                return JsonConvert.SerializeObject(orders);
            }
        }



        /// <summary>
        /// 加载订单明细
        /// </summary>
        /// <param name="serial_no"></param>
        /// <returns></returns>
        public dynamic GetOrder(long user_id, string serial_no)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //查询的字段
                string[] files = new string[] { "id", "type", "is_pay", "serial_no", "pay_model", "total_amount", "actual_amount", "discount_amount", "coupon_amount", "status", "refund_status", "created_date", "freight", "user_coupon_id", "coupon_amount" };
                ProductOrder order = s.Get<ProductOrder>(files, "where serial_no = @0 and created_user_id = @1", serial_no, user_id);

                //获取订单详情
                order.details = s.List<ProductOrderDetail>("where order_id = @0", order.id);
                //订单发货信息
                OrderDelivery delivery = s.Get<OrderDelivery>("where order_id = @0", order.id);

                //返回数据
                return new
                {
                    order,
                    delivery
                };
            }
        }




        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="serial_no"></param>
        /// <returns></returns>
        public StateCode CencelOrder(long user_id, string serial_no)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    //回调修改状态
                    s.ExcuteUpdate("update tb_odr_order set status = @0 where serial_no = @1 and created_user_id = @2", (int)OrderStatus.Close, serial_no, user_id);

                    //获取当前即将关闭的订单
                    ProductOrder order = s.Get<ProductOrder>("where serial_no = @0 and created_user_id = @1", serial_no, user_id);

                    //修改订单对应的用户使用的优惠卷
                    //s.ExcuteUpdate("update tb_user_aty_coupon set is_used = @0 where id = @1", false, order.user_coupon_id);

                    s.Commit();

                    return StateCode.State_200;
                }
                catch (Exception ex)
                {
                    s.RollBack();
                    s.Insert(new APILogs() { type = 1, created_date = DateTime.Now, content = ex.ToString() });
                    return StateCode.State_500;
                }
            }
        }



        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="serial_no"></param>
        /// <returns></returns>
        public StateCode DeleteOrder(long user_id, string serial_no)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    //回调修改状态
                    s.ExcuteUpdate("update tb_odr_order set delete_status = @0 where serial_no = @1 and created_user_id = @2", 1, serial_no, user_id);
                    return StateCode.State_200;
                }
                catch (Exception ex)
                {
                    s.Insert(new APILogs() { type = 1, created_date = DateTime.Now, content = ex.ToString() });
                    return StateCode.State_500;
                }
            }
        }




        /// <summary>
        /// 支付回调处理
        /// </summary>
        /// <param name="serial_no"></param>
        /// <param name="transaction_id"></param>
        /// <param name="pay_method"></param>
        /// <param name="pay_amount"></param>
        /// <returns></returns>
        public StateCode NotifyHandler(string serial_no, string transaction_id, int pay_method)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    //获取订单
                    ProductOrder order = s.Get<ProductOrder>("where serial_no = @0", serial_no);
                    if (order != null)
                    {
                        //回调修改订单状态
                        s.ExcuteUpdate("update tb_odr_order set is_pay = @0,status = @1,transaction_id = @2 where serial_no = @3 ", true, (int)OrderStatus.PaymentsMade, transaction_id, serial_no);
                        //支付方式
                        s.ExcuteUpdate("update tb_odr_order set pay_method = @0 where serial_no = @1 ", pay_method, serial_no);
                        //发布短信
                        //SendPayCourseSms(order);
                    }
                    else
                    {
                        s.Insert(new APILogs() { type = 1, created_date = DateTime.Now, content = "商品订单号：" + serial_no + ",数据丢失" });
                    }

                    s.Commit();
                    return StateCode.State_200;
                }
                catch (Exception ex)
                {
                    s.RollBack();
                    s.Insert(new APILogs() { type = 1, created_date = DateTime.Now, content = ex.ToString() });
                    return StateCode.State_500;
                }
            }
        }



        /// <summary>
        /// 课程订单
        /// </summary>
        /// <param name="serial_no"></param>
        /// <param name="transaction_id"></param>
        /// <param name="pay_method"></param>
        /// <returns></returns>
        public StateCode NotifyHandlerByStages(string serial_no, string transaction_id, int pay_method)
        {
            //是否发送短信
            bool is_send = false;
            //订单想那些
            ProductOrder order = null;

            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    //获取当前课程支付流程
                    OrderPayFlow orderPayFlow = s.Get<OrderPayFlow>("where serial_no = @0", serial_no);
                    if (orderPayFlow != null)
                    {
                        //如果当前订单未支付
                        if (string.IsNullOrEmpty(orderPayFlow.transaction_id))
                        {
                            //当前订单
                            order = s.Get<ProductOrder>(orderPayFlow.order_id);

                            //修改支付流程
                            string sql_flow = "update tb_ord_pay_flow set transaction_id = @0,pay_method = @1 where id = @2";

                            //修改订单主表
                            string sql_order = "update tb_odr_order set is_pay = @0,status = @1,pay_method = @2,transaction_id = @3 where id = @4";

                            //修改课程订单子表
                            string sql_order_course = "update tb_odr_order_course set amount += @0 where order_id = @1";

                            //修改支付流程状态
                            s.ExcuteUpdate(sql_flow, transaction_id, pay_method, orderPayFlow.id);

                            //修改支付流程状态
                            s.ExcuteUpdate(sql_order_course, orderPayFlow.amount, orderPayFlow.order_id);

                            //订单支付流程
                            List<OrderPayFlow> flows = s.Where<OrderPayFlow>(o => o.order_id == orderPayFlow.order_id);
                            //如果全部支付
                            if (flows.Sum(f => f.amount) >= order.actual_amount)
                            {
                                is_send = true;
                                s.ExcuteUpdate(sql_order, true, (int)OrderStatus.PaymentsMade, pay_method, transaction_id, order.id);
                            }
                            else
                                s.ExcuteUpdate(sql_order, false, (int)OrderStatus.PaymentPart, pay_method, transaction_id, order.id);
                        }
                    }
                    else
                    {
                        s.Insert(new APILogs() { type = 1, created_date = DateTime.Now, content = "分期订单号：" + serial_no + ",数据丢失" });
                    }

                    s.Commit();
                }
                catch (Exception ex)
                {
                    s.RollBack();
                    s.Insert(new APILogs() { type = 1, created_date = DateTime.Now, content = ex.ToString() });
                    return StateCode.State_500;
                }
            }

            //是否发送短信
            //if (is_send) SendPayCourseSms(order);

            return StateCode.State_200;
        }



        #endregion



    }
}
