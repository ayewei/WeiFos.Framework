using System;
using System.Collections.Generic;
using System.Text;
using WeiFos.ORM.Data;
using WeiFos.Entity.ProductModule;
using WeiFos.Entity.BizTypeModule;
using WeiFos.Core;
using WeiFos.Entity.SKUModule;
using WeiFos.Entity.OrderModule;
using WeiFos.Entity.LogsModule;
using WeiFos.Entity.Enums;
using WeiFos.Entity.ResourceModule;

namespace WeiFos.Service.OrderModule
{
    /// <summary>
    /// 购物车 Service 
    /// @author yewei 
    /// @date 2014-04-29
    /// </summary>
    public class ShoppingCartService : BaseService<ShoppingCart>
    {


        /// <summary>
        /// 加入购物车
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="product_id"></param>
        /// <param name="specset"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        public StateCode Join(long user_id, long product_id, string specset, int qty)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {

                    //商品SKU
                    ProductSku sku = s.Get<ProductSku>("where product_id = @0 and dbo.fn_check_specset(specset,@1) = 1 ", product_id, specset ?? "");

                    //商品已删除
                    Product product = s.Get<Product>("where id = @0 ", sku.product_id);
                    if (product == null) return StateCode.State_501;

                    //商品已下架
                    if (!((bool)product.is_shelves && !(bool)product.is_delete && (DateTime.Now > product.shelves_sdate && DateTime.Now < product.shelves_edate))) return StateCode.State_505;

                    s.StartTransaction();

                    //购买数量大于0
                    if (qty > 0 || qty == -1)
                    {
                        //规格详细信息
                        StringBuilder sb = new StringBuilder();

                        //是否开启规格
                        if (product.is_open_spec)
                        {
                            string[] arr = StringHelper.StringToArray(sku.specset);
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
                                    {
                                        if(sb.Length == 0)
                                        {
                                            sb.Append(string.Format("{0}：{1}", specname.name, specvalue.val));
                                        }
                                        else
                                        {
                                            sb.Append(string.Format("，{0}：{1}", specname.name, specvalue.val));
                                        }
                                    }
                                    else
                                        throw new Exception("商品规格信息异常");
                                }
                            }
                        }

                        string img_url = "";
                        Img img = s.Get<Img>(" where biz_type = @0 and biz_id = @1 and is_main = @2", ImgType.Product_Cover, sku.product_id, true);
                        if (img != null)
                        {
                            if (img.is_webimg)
                            {
                                img_url = img.webimg_url;
                            }
                            else
                            {
                                img.visit_path = img.visit_path.Replace("/Image/", "/Thm_Image/");
                                img_url = img.domain_name + img.visit_path + img.file_name + img.extend_name;
                            }
                        }

                        //当前购物车信息
                        ShoppingCart shoppingcart = s.Get<ShoppingCart>("where user_id = @0 and dbo.fn_check_specset(specset,@1) = 1 and product_id = @2", user_id, sku.specset, product_id);

                        if (shoppingcart != null)
                        {
                            shoppingcart.product_img_url = img_url;
                            shoppingcart.count += qty;
                            shoppingcart.market_price = product.market_price;
                            shoppingcart.product_price = sku.sale_price;
                            shoppingcart.product_name = product.name;
                            shoppingcart.weight = sku.weight;
                            shoppingcart.updated_date = DateTime.Now;
                            s.Update<ShoppingCart>(shoppingcart);
                        }
                        else
                        {
                            shoppingcart = new ShoppingCart()
                            {
                                user_id = user_id,
                                product_id = sku.product_id,
                                market_price = product.market_price,
                                product_price = sku.sale_price,
                                product_name = product.name,
                                product_en_name = product.en_name,
                                weight = sku.weight,
                                specset = sku.specset,
                                spec_msg = sb.ToString(),
                                product_img_url = img_url,
                                count = qty,
                                created_date = DateTime.Now,
                                updated_date = DateTime.Now
                            };
                            s.Insert<ShoppingCart>(shoppingcart);
                        }
                    }

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
        /// 更新购物车
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="count"></param>
        /// <param name="product_id"></param>
        /// <param name="specset"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public StateCode Update(long user_id, int count, long product_id, string specset, long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    string sql = "update tb_ord_shoppingcart set count = @0 where user_id = @1 and product_id = @2 and dbo.fn_check_specset(specset,@3) = 1 and id = @4";
                    s.ExcuteUpdate(sql, count, user_id, product_id, specset, id);
                    return StateCode.State_200;
                }
                catch (Exception ex)
                {
                    s.Insert(new APILogs()
                    {
                        type = 1,
                        content = ex.ToString(),
                        created_date = DateTime.Now
                    });
                    return StateCode.State_500;
                }
            }
        }


        /// <summary>
        /// 根据用户ID获取购物车
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public List<ShoppingCart> Gets(long user_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<ShoppingCart>("where user_id = @0", user_id);
            }
        }


          
        /// <summary>
        /// 删除购物车商品
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="id">购物车ID</param>
        public StateCode Delete(long user_id, long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.ExcuteUpdate("delete tb_ord_shoppingcart where user_id = @0 and id = @1 ", user_id, id);
                    return StateCode.State_200;
                }
                catch(Exception ex)
                {
                    s.Insert(new APILogs()
                    {
                        type = 1,
                        content = ex.ToString(),
                        created_date = DateTime.Now
                    });
                    return StateCode.State_500;
                }
            }
        }



        /// <summary>
        /// 根据购物车ID集合删除
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public StateCode DeleteByIds(long user_id, long[] ids)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    foreach (long id in ids)
                    {
                        s.ExcuteUpdate("delete tb_ord_shoppingcart where user_id = @0 and id = @1", user_id, id);
                    }

                    s.Commit();
                }
                catch (Exception ex)
                {
                    s.RollBack();
                    return StateCode.State_500;
                }
                return StateCode.State_200;
            }
        }



        /// <summary>
        /// 根据购物车ID集合删除
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public int SumByUserId(long user_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return int.Parse(s.ExecuteScalar("select COALESCE(sum(tb_ord_shoppingcart.count),0) as num from [dbo].[tb_ord_shoppingcart] where user_id = @0", user_id).ToString());
            }
        }




    }
}
