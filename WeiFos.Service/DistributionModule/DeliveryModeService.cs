using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Core;
using WeiFos.ORM.Data; 
using WeiFos.Entity.DistributionModule;
using WeiFos.Entity.ProductModule;
using WeiFos.Entity.Enums;
using WeiFos.Entity.OrderModule;

namespace WeiFos.Service.DistributionModule
{
    // <summary>
    /// 配送方式实 Service
    /// @author yewei
    /// add by  @date 2015-10-28
    /// </summary>
    public class DeliveryModeService : BaseService<DeliveryMode>
    {


        /// <summary>
        /// 保存配送方式
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public StateCode Save(long userID, DeliveryMode entity)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    //是否是默认配送方式
                    if (entity.is_default) s.ExcuteUpdate("update tb_dist_deliverymode set is_default = @0", false);

                    if (entity.id == 0)
                    {
                        entity.created_user_id = userID;
                        entity.created_date = DateTime.Now;
                        s.Insert(entity);
                    }
                    else
                    {
                        entity.updated_date = DateTime.Now;
                        entity.updated_user_id = userID;
                        s.Update(entity);
                    }
                    return StateCode.State_200;
                }
                catch (Exception ex)
                {
                    return StateCode.State_500;
                }
            }
        }



        /// <summary>
        /// 根据配送方式ID获取物流公司
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<LogisticsCompany> GetLogisticsCompanys(long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                List<LogisticsCompany> lsCompanys = new List<LogisticsCompany>();

                //获取当前配送方式
                DeliveryMode deliveryMode = s.Get<DeliveryMode>(id);
                if (deliveryMode == null) throw new Exception("不存在该配送方式");

                //当前物流公司
                long[] ids = StringHelper.StringToLongArray(deliveryMode.logistics_company_ids);

                foreach (long company_id in ids)
                {
                    LogisticsCompany company = s.Get<LogisticsCompany>(company_id);
                    if (company != null)
                    {
                        lsCompanys.Add(company);
                    }
                }
                return lsCompanys;
            }
        }



        /// <summary>
        /// 根据重量生成配送方式
        /// 默认配送方式
        /// </summary>
        /// <param name="orderProducts"></param>
        /// <param name="shippingAddressID"></param>
        /// <returns></returns>
        public StateCode GetDefaultFreight(List<JsonOrderProduct> orderProducts, long shippingAddressID, out string result)
        {
            result = "";
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //用户收货地址
                ShoppingAddress shippingAddress = s.Get<ShoppingAddress>(shippingAddressID);
                if (shippingAddress == null) return StateCode.State_252;

                //配送方式实体类
                DeliveryMode mode = s.Get<DeliveryMode>("where is_default = @0", true);
                if (mode == null) return StateCode.State_251;

                decimal price = 0, first_price = 0, add_price = 0;
                long weight = 0;

                foreach (JsonOrderProduct jop in orderProducts)
                {
                    Product pdt = s.Get<Product>(new string[] { "id", "is_postage" }, "where id = @0", jop.product_id);
                    //排除包邮商品
                    if (pdt != null && !pdt.is_postage)
                    {
                        ProductSku sku = s.Get<ProductSku>("where dbo.fn_check_specset(specset,@0) = 1 and product_id = @1", jop.specset ?? "", jop.product_id);
                        weight += sku.weight * jop.qty;
                    }
                }

                //获取当前配送方式运费模板
                FreightTemplate fTemplate = s.Get<FreightTemplate>(mode.freight_template_id);

                if (fTemplate != null && weight > 0)
                {
                    //默认首重价格
                    first_price = fTemplate.default_first_price;
                    //默认续重价格
                    add_price = fTemplate.default_add_price;

                    //特殊地区按特殊地区价格算
                    FreightRegion fRegion = s.Get<FreightRegion>("where region_name like '%'+@0+'%' and freight_template_id = @1 ", shippingAddress.province, fTemplate.id);
                    if (fRegion != null)
                    {
                        if (weight < fRegion.first_weight)
                        {
                            price = fRegion.first_price;
                        }
                        else
                        {
                            decimal t = (((weight - fRegion.first_weight) / fRegion.add_weight) + (((weight - fRegion.first_weight) % fRegion.add_weight) == 0 ? 0 : 1)) * fRegion.add_price;
                            price = weight <= fRegion.first_weight ? fRegion.first_price : t + fRegion.first_price;
                        }
                    }
                    else
                    {
                        if (weight < fTemplate.first_weight)
                        {
                            price = fTemplate.default_first_price;
                        }
                        else
                        {
                            decimal tmp = (((weight - fTemplate.first_weight) / fTemplate.add_weight) + (((weight - fTemplate.first_weight) % fTemplate.add_weight) == 0 ? 0 : 1)) * add_price;
                            price = weight <= fTemplate.first_weight ? first_price : Math.Ceiling((decimal)(weight - fTemplate.first_weight) / (decimal)fTemplate.add_weight) * add_price + first_price;
                        }
                    }
                }

                result = mode.id + "#" + price;

                return StateCode.State_200;
            }
        }



        /// <summary>
        /// 根据重量生成配送方式（最小运费）
        /// </summary>
        /// <param name="orderProducts"></param>
        /// <param name="shippingAddressID"></param>
        /// <returns></returns>
        public string GetDeliveryModeFreight(List<JsonOrderProduct> orderProducts, long shippingAddressID)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //用户收货地址
                ShoppingAddress shippingAddress = s.Get<ShoppingAddress>(shippingAddressID);
                if (shippingAddress == null) throw new Exception("不存在该用户收货地址");

                decimal price = 0, first_price = 0, add_price = 0;
                decimal pricetmp = 0;
                long defaultModeID = 0, modeID = 0, weight = 0;

                foreach (JsonOrderProduct jop in orderProducts)
                {
                    Product pdt = s.Get<Product>(jop.product_id);
                    //排除包邮商品
                    if (pdt != null && !pdt.is_postage)
                    {
                        ProductSku sku = s.Get<ProductSku>("where dbo.fn_check_specset(specset,@0) = 1 and product_id = @1", jop.specset ?? "", jop.product_id);
                        weight += sku.weight * jop.qty;
                    }
                }

                //配送方式实体类
                List<DeliveryMode> modes = s.List<DeliveryMode>("", "");

                foreach (DeliveryMode m in modes)
                {
                    //记录当前配送方式ID
                    modeID = m.id;

                    //获取当前配送方式运费模板
                    FreightTemplate fTemplate = s.Get<FreightTemplate>(m.freight_template_id);

                    if (fTemplate != null)
                    {
                        //默认首重价格
                        first_price = fTemplate.default_first_price;
                        //默认续重价格
                        add_price = fTemplate.default_add_price;

                        //获取特殊区域 
                        FreightRegion fRegion = s.Get<FreightRegion>("where region_name like '%'+@0+'%' and freight_template_id = @1 ", shippingAddress.province, fTemplate.id);
                        if (fRegion != null)
                        {
                            if (weight < fRegion.first_weight)
                            {
                                pricetmp = fRegion.first_price;
                            }
                            else
                            {
                                decimal t = (((weight - fRegion.first_weight) / fRegion.add_weight) + ((weight - fRegion.first_weight) % fRegion.add_weight) == 0 ? 0 : 1) * fRegion.add_price;
                                pricetmp = weight <= fRegion.first_weight ? fRegion.first_price : t + fRegion.first_price;
                            }
                        }

                        if (weight < fTemplate.first_weight)
                        {
                            price = fTemplate.default_first_price;
                        }
                        else
                        {
                            decimal tmp = (((weight - fTemplate.first_weight) / fTemplate.add_weight) + ((weight - fTemplate.first_weight) % fTemplate.add_weight) == 0 ? 0 : 1) * add_price;
                            price = weight <= fTemplate.first_weight ? first_price : Math.Ceiling((decimal)(weight - fTemplate.first_weight) / (decimal)fTemplate.add_weight) * add_price + first_price;
                        }

                        //最小价格
                        if (price < pricetmp)
                        {
                            pricetmp = price;
                            defaultModeID = m.id;
                        }
                    }
                }

                defaultModeID = defaultModeID == 0 ? modeID : defaultModeID;
                price = price < pricetmp ? price : pricetmp == 0 ? price : pricetmp;

                return defaultModeID + "#" + (int)price;
            }
        }



        /// <summary>
        /// 根据重量生成配送方式(集合)
        /// </summary>
        /// <param name="orderProducts"></param>
        /// <param name="ShippingAddressID"></param>
        /// <returns></returns>
        public Dictionary<long, string> GenerateDeliveryModes(List<JsonOrderProduct> orderProducts, long ShippingAddressID)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //用户收货地址
                ShoppingAddress shippingAddress = s.Get<ShoppingAddress>(ShippingAddressID);
                if (shippingAddress == null) throw new Exception("不存在该用户收货地址");

                int weight = 0;

                foreach (JsonOrderProduct jop in orderProducts)
                {
                    ProductSku sku = s.Get<ProductSku>("where dbo.fn_check_specset(specset,@0) = 1 and product_id = @1", (jop.specset ?? ""), jop.product_id);
                    weight += sku.weight * jop.qty;
                }

                //配送方式实体类
                List<DeliveryMode> modes = s.List<DeliveryMode>("", "");

                //配送方式以及运费集合
                Dictionary<long, string> deliveryMode = new Dictionary<long, string>();
                decimal price = 0, first_price = 0, add_price = 0;

                foreach (DeliveryMode m in modes)
                {
                    //获取当前配送方式运费模板
                    FreightTemplate fTemplate = s.Get<FreightTemplate>(m.freight_template_id);

                    if (fTemplate != null)
                    {
                        //默认首重价格
                        first_price = fTemplate.default_first_price;
                        //默认续重价格
                        add_price = fTemplate.default_add_price;

                        //特殊地区按特殊地区价格算
                        FreightRegion fRegion = s.Get<FreightRegion>("where region_name like '%'+@0+'%' and freight_template_id = @1 ", shippingAddress.province, fTemplate.id);
                        if (fRegion != null)
                        {
                            if (weight < fRegion.first_weight)
                            {
                                price = fRegion.first_price;
                            }
                            else
                            {
                                decimal t = (((weight - fRegion.first_weight) / fRegion.add_weight) + ((weight - fRegion.first_weight) % fRegion.add_weight) == 0 ? 0 : 1) * fRegion.add_price;
                                price = weight <= fRegion.first_weight ? fRegion.first_price : t + fRegion.first_price;
                            }
                        }
                        else
                        {
                            if (weight < fTemplate.first_weight)
                            {
                                price = fTemplate.default_first_price;
                            }
                            else
                            {
                                decimal tmp = (((weight - fTemplate.first_weight) / fTemplate.add_weight) + ((weight - fTemplate.first_weight) % fTemplate.add_weight) == 0 ? 0 : 1) * add_price;
                                price = weight <= fTemplate.first_weight ? first_price : Math.Ceiling((decimal)(weight - fTemplate.first_weight) / (decimal)fTemplate.add_weight) * add_price + first_price;
                            }
                        }

                        deliveryMode.Add(m.id, string.Format("{0}： {1}", m.name, price.ToString("f2")));
                    }
                }
                return deliveryMode;
            }
        }



        /// <summary>
        /// 设置默认
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StateCode SetDefault(long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    //将其他的全部设置成false
                    s.ExcuteUpdate("update tb_dist_deliverymode set is_default = @0", false);

                    //将当前设置成true
                    s.ExcuteUpdate("update tb_dist_deliverymode set is_default = @0 where id = @1", true, id);

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
        /// 检查品牌名称是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public int ExistName(string name, long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Exist<DeliveryMode>("where name = @0 and id != @1", name, id);
            }
        }



    }
}
