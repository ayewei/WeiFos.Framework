using WeiFos.Core;
using WeiFos.ORM.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WeiFos.Entity.ProductModule;
using WeiFos.Entity.Common;
using WeiFos.Entity.BizTypeModule;
using Newtonsoft.Json;
using WeiFos.Entity.Enums;
using WeiFos.Entity.ResourceModule;
using WeiFos.Entity.SKUModule;

namespace WeiFos.Service.ProductModule
{

    /// <summary>
    /// 商品 Service
    /// @author yewei
    /// add by  @date 2015-03-03
    /// </summary>
    public class ProductService : BaseService<Product>
    {


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="product"></param>
        /// <param name="pdtAttrVals"></param>
        /// <param name="pdtExtAttrVals"></param>
        /// <param name="skus"></param>
        /// <param name="specCustoms"></param>
        /// <param name="imglist"></param>
        /// <param name="ratios"></param>
        /// <param name="mainimg"></param>
        /// <returns></returns>
        public StateCode Save(long userID, Product product, List<PdtAttrVal> pdtAttrVals, List<PdtExtAttrVal> pdtExtAttrVals, List<ProductSku> skus, List<SpecCustom> specCustoms,
            List<Img> imglist, string mainimg)
        {
            product.catg_path = ServiceIoc.Get<ProductCatgService>().GetParentPath(product.catg_id);
            product.catg_pathname = ServiceIoc.Get<ProductCatgService>().GetParentPathName(product.catg_id);

            product.gcatg_path = ServiceIoc.Get<GuideProductCatgService>().GetParentPath(product.gcatg_id);
            product.gcatg_pathname = ServiceIoc.Get<GuideProductCatgService>().GetParentPathName(product.gcatg_id);

            //分类路径
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    //是否是最后分类
                    if (product.gcatg_id != 0 && s.Exist<GuideProductCatg>("where parent_id = @0 ", product.gcatg_id) > 0) return StateCode.State_201;

                    #region 商品基本信息
                    if (product.id != 0)
                    {
                        //product.is_shelves = new_product.is_shelves;
                        product.updated_user_id = userID;
                        product.updated_date = DateTime.Now;
                        s.Update<Product>(product);
                    }
                    else
                    {
                        product.created_user_id = userID;
                        product.created_date = DateTime.Now;
                        s.Insert<Product>(product);
                    }
                    #endregion

                    #region 处理商品图片 

                    if (imglist != null && imglist.Count > 0)
                    {
                        //去除重复图片
                        s.ExcuteUpdate("update tb_img set biz_id = 0 where biz_type = @0 and biz_id = @1 and is_webimg = @2", ImgType.Product_Cover, product.id, false);
                        foreach (Img image in imglist)
                        {
                            if (!string.IsNullOrEmpty(image.file_name))
                            {
                                Img img = s.Get<Img>("where file_name = @0 and biz_type = @1 ", image.file_name, ImgType.Product_Cover);
                                if (img != null)
                                {
                                    //显示顺序
                                    img.order_index = image.order_index;
                                    //业务ID
                                    img.biz_id = product.id;
                                    s.Update<Img>(img);
                                }
                            }
                        }

                        //重置主图
                        s.ExcuteUpdate("update tb_img set is_main = @0 where biz_id = @1 and biz_type = @2 ", false, product.id, ImgType.Product_Cover);

                        //如果没设置主图，则设置第一个为默认主图 
                        if (string.IsNullOrEmpty(mainimg)) mainimg = imglist[0].file_name;

                        //filename情况
                        if (mainimg.Length < 30)
                            s.ExcuteUpdate("update tb_img set is_main = @0 where id = @1 and biz_type = @2", true, long.Parse(mainimg), ImgType.Product_Cover);
                        else
                            s.ExcuteUpdate("update tb_img set is_main = @0 where file_name = @1 and biz_type = @2 ", true, mainimg, ImgType.Product_Cover);
                    }

                    #endregion

                    #region 商品基本属性值
                    if (pdtAttrVals == null)
                    {
                        //删除商品基本属性值
                        s.ExcuteUpdate("delete tb_pdt_attrval where product_id = @0 ", product.id);
                    }
                    else
                    {
                        List<PdtAttrVal> tmp_baseAttrValues = new List<PdtAttrVal>();

                        //数据库原有数据
                        List<PdtAttrVal> old_baseAttrValues = s.List<PdtAttrVal>("where product_id = @0", product.id);

                        foreach (PdtAttrVal ptbav in pdtAttrVals)
                        {
                            ptbav.product_id = product.id;
                            int exist_AttrName = s.Exist<AttrName>("where id = @0", ptbav.attrname_id);

                            //是否存商品类型基础扩展属性名
                            if (exist_AttrName > 0)
                            {
                                if (ptbav.id != 0)
                                {
                                    ptbav.val = ptbav.val ?? "";
                                    s.Update<PdtAttrVal>(ptbav);
                                    tmp_baseAttrValues.Add(ptbav);
                                }
                                else
                                {
                                    //确保 pdt_attributename_id 数据唯一
                                    int exist_pdtval = s.Exist<PdtAttrVal>("where product_id = @0 and attrname_id = @1 ", product.id, ptbav.attrname_id);
                                    if (exist_pdtval == 0)
                                    {
                                        s.Insert<PdtAttrVal>(ptbav);
                                    }
                                }
                            }
                        }

                        //筛选出删除的基础属性值
                        foreach (PdtAttrVal p in old_baseAttrValues)
                        {
                            bool exists = true;
                            foreach (PdtAttrVal o_p in tmp_baseAttrValues)
                            {
                                if (p.id == o_p.id)
                                {
                                    exists = false;
                                    break;
                                }
                            }
                            if (exists)
                            {
                                //删除商品基本属性值
                                s.ExcuteUpdate("delete tb_pdt_attrval where id = @0 ", p.id);
                            }
                        }
                    }
                    #endregion

                    #region 商品扩展属性值

                    //清空扩展属性值
                    s.ExcuteUpdate("delete tb_pdt_extattrval where product_id = @0 ", product.id);
                    if (pdtExtAttrVals != null)
                    {
                        foreach (PdtExtAttrVal pta in pdtExtAttrVals)
                        {
                            pta.product_id = product.id;
                            s.Insert<PdtExtAttrVal>(pta);
                        }
                    }

                    #endregion

                    #region 商品自定义规格信息

                    List<SpecCustom> old_ssc = s.List<SpecCustom>("where product_id = @0 ", product.id);
                    s.ExcuteUpdate("delete tb_pdt_spec_custom where product_id = @0 ", product.id);

                    if (specCustoms != null)
                    {
                        foreach (SpecCustom ssc in specCustoms)
                        {
                            ssc.custom_value = ssc.custom_value == null ? "" : ssc.custom_value;
                            ssc.product_id = product.id;
                            s.Insert<SpecCustom>(ssc);
                        }
                    }

                    #endregion

                    #region 商品SKU

                    //当前商品规格详细
                    List<ProductSku> oldSkus = s.List<ProductSku>("where product_id = @0 ", product.id);

                    //开启规格
                    if (product.is_open_spec)
                    {
                        //当前合法的规格详细信息
                        List<ProductSku> currentSkus = new List<ProductSku>();

                        //当前商品规格名称集合
                        List<SpecName> specNames = s.List<SpecName>("where product_type_id = @0", product.product_type_id);

                        //当前商品规格名称集合
                        List<SpecValue> specValues = s.List<SpecValue>("where product_type_id = @0", product.product_type_id);

                        //商品规格信息
                        if (skus == null || skus.Count == 0) throw new Exception("商品规格信息异常");

                        //遍历sku集合
                        foreach (ProductSku s_sku in skus)
                        {
                            //标示是否是合法sku数据
                            bool ispass = true;

                            //检测规格数据是否存在
                            string[] arr = StringHelper.StringToArray(s_sku.specset);
                            foreach (string i in arr)
                            {
                                int specname_id = 0;
                                int specvalue_id = 0;
                                if (i.IndexOf("_") != -1)
                                {
                                    int.TryParse(i.Split('_')[0], out specname_id);
                                    int.TryParse(i.Split('_')[1], out specvalue_id);
                                    //验证规格名称和规格值
                                    if (!specNames.Exists(sn => sn.id == specname_id) || !specValues.Exists(sv => sv.id == specvalue_id))
                                    {
                                        ispass = false;
                                        break;
                                    }
                                }
                            }

                            if (ispass)
                            {
                                //是否在原有的集合存在
                                ProductSku ssku = oldSkus.Where(osku => osku.specset.Equals(s_sku.specset)).SingleOrDefault();
                                if (ssku != null)
                                {
                                    s_sku.id = ssku.id;
                                    s_sku.product_id = product.id;
                                    s.Update<ProductSku>(s_sku);
                                    currentSkus.Add(s_sku);
                                }
                                else
                                {
                                    s_sku.product_id = product.id;
                                    s.Insert<ProductSku>(s_sku);
                                }

                                //商品规格集合数据
                                foreach (string i in arr)
                                {
                                    int specname_id = 0;
                                    int specvalue_id = 0;
                                    if (i.IndexOf("_") != -1)
                                    {
                                        int.TryParse(i.Split('_')[0], out specname_id);
                                        int.TryParse(i.Split('_')[1], out specvalue_id);

                                        SpecSet specset = new SpecSet();
                                        specset.specname_id = specname_id;
                                        specset.specvalue_id = specvalue_id;
                                        specset.product_id = product.id;
                                        specset.pdt_sku_id = s_sku.id;
                                        s.Insert<SpecSet>(specset);
                                    }
                                }
                            }
                        }

                        //筛选出删除的商品规格
                        foreach (ProductSku o_sku in oldSkus)
                        {
                            bool exists = true;
                            foreach (ProductSku cpsd in currentSkus)
                            {
                                if (cpsd.id == o_sku.id)
                                {
                                    exists = false;
                                    break;
                                }
                            }
                            if (exists)
                            {
                                //删除sku
                                s.ExcuteUpdate("delete tb_pdt_sku where id = @0 and product_id = @1", o_sku.id, product.id);
                                //删除商品规格集合
                                s.ExcuteUpdate("delete tb_pdt_specset where pdt_sku_id = @0 and product_id = @1", o_sku.id, product.id);
                            }
                        }

                    }
                    else
                    {
                        //SKUs
                        if (skus == null || skus.Count != 1) throw new Exception("商品扩展属性数据异常");

                        //判断开启规格之前的信息
                        if (oldSkus.Count > 1)
                        {
                            foreach (ProductSku sku in oldSkus)
                            {
                                //删除商品 自定义规格值
                                s.ExcuteUpdate("delete tb_pdt_specset where pdt_sku_id = @0 and product_id = @1 ", sku.id, product.id);
                            }
                            //删除商品 SKU 集合
                            s.ExcuteUpdate("delete tb_pdt_sku where and product_id = @0", product.id);
                        }

                        ProductSku c_sku = s.Get<ProductSku>(" where product_id = @0", product.id); 
                        skus[0].specset = "";
                        skus[0].product_id = product.id;
                        if (c_sku == null)
                        {
                            s.Insert(skus[0]);
                        }
                        else
                        {
                            skus[0].id = c_sku.id;
                            s.Update(skus[0]);
                        }
                    }

                    #endregion

                    //修改SKU商品名称
                    s.ExcuteUpdate("update tb_pdt_sku set product_name = @0 where product_id = @1", product.name, product.id);

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
        /// 官网列表商品
        /// </summary>
        /// <returns></returns>
        public List<Product> ComList()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                string sql = "where is_delete = @0 and is_hot = @1 and is_shelves = @2 and (GETDATE() BETWEEN shelves_sdate AND shelves_edate) order by order_index desc";
                return s.List<Product>(sql, false, true, true);
            }
        }

        /// <summary>
        /// 获取列表页默认显示商品
        /// </summary>
        /// <param name="guide_product_id"></param>
        /// <returns></returns>
        public List<Product> GetDefaultList(long guide_product_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                string sql = "where is_delete = @0 and guide_category_id = @1 and is_shelves = @2 and (GETDATE() BETWEEN shelves_sdate AND shelves_edate) order by order_index desc";
                return s.List<Product>(sql, false, guide_product_id, true);
            }
        }


        /// <summary>
        /// 购物车猜你喜欢商品
        /// </summary>
        /// <returns></returns>
        public List<Product> LikeProducts()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                string sql = "where is_delete = @0 and is_shelves = @1 and is_hot = @2 and (GETDATE() BETWEEN shelves_sdate AND shelves_edate) order by order_index desc";
                return s.List<Product>(sql, false, true, true);
            }
        }


        /// <summary>
        /// 上架商品
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        public StateCode ShelvesProducts(long[] productIds)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    for (int i = 0; i < productIds.Count(); i++)
                    {
                        s.StartTransaction();
                        s.ExcuteUpdate("update tb_pdt_product set is_shelves = @0 where id = @1", true, productIds[i]);
                        s.Commit();
                    }
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
        /// 商品删除或还原
        /// </summary>
        /// <param name="productIds"></param>
        /// <param name="createdUserID"></param>
        /// <param name="state"></param>
        public void DeleteOrRestore(long[] productIds, bool isDelete)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    for (int i = 0; i < productIds.Count(); i++)
                    {
                        s.ExcuteUpdate("update tb_pdt_product set is_delete = @0  where id = @1 ", isDelete, productIds[i]);
                    }
                    s.Commit();
                }
                catch
                {
                    s.RollBack();
                }
            }
        }


        /// <summary>
        /// 删除选中商品
        /// </summary>
        /// <param name="productIds"></param>
        public StateCode Deletes(long[] productIds)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    for (int i = 0; i < productIds.Count(); i++)
                    {
                        //删除商品 
                        s.ExcuteUpdate("delete tb_pdt_product where id = @0 ", productIds[i]);

                        //删除商品 基本属性值
                        s.ExcuteUpdate("delete tb_pdt_attrval where product_id = @0 ", productIds[i]);

                        //删除商品 扩展属性值
                        s.ExcuteUpdate("delete tb_pdt_extattrval where product_id = @0 ", productIds[i]);

                        //获取当前商品对应的SKU 信息
                        s.ExcuteUpdate("delete tb_pdt_sku where product_id = @0", productIds[i]);

                        //获取当前商品对应的规格集合
                        s.ExcuteUpdate("delete tb_pdt_specset where product_id = @0", productIds[i]);

                        //删除商品图片数据
                        s.ExcuteUpdate("delete tb_img where biz_type = @0 and biz_id = @1 ", ImgType.Product_Cover, productIds[i]);

                        //删除商品图片数据
                        s.ExcuteUpdate("delete tb_img where biz_type = @0 and biz_id = @1 ", ImgType.Product_Details, productIds[i]);

                        //删除商品自定义规格名称 
                        s.ExcuteUpdate("delete tb_pdt_spec_custom where product_id = @0 ", productIds[i]);
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
        /// 保存排期
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public void SaveShelvesDate(long[] ids, DateTime startDate, DateTime endDate)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    s.ExcuteUpdate("update tb_pdt_product set shelves_sdate = @0 , shelves_edate = @1 where id = @2", startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), ids[i]);
                }
            }
        }


        /// <summary>
        /// 选择上架或下架
        /// </summary>
        /// <param name="productIds"></param>
        /// <param name="IsShelves"></param>
        public void SelectShelves(long[] productIds, bool IsShelves)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    for (int i = 0; i < productIds.Count(); i++)
                    {
                        //设置上架或下架
                        s.ExcuteUpdate("update tb_pdt_product set is_shelves = @0 where id = @1 ", IsShelves, productIds[i]);
                    }
                    s.Commit();
                }
                catch
                {
                    s.RollBack();
                }
            }
        }


        /// <summary>
        /// 获取商品是否可用
        /// </summary>
        /// <param name="product_id"></param>
        /// <returns></returns>
        public bool GetProuctEnable(long product_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Exist<Product>("where (GETDATE() BETWEEN shelves_sdate AND shelves_edate) and is_shelves = @0 and is_delete = @1 and id = @2", true, false, product_id) > 0;
            }
        }



        /// <summary>
        /// 初始化商品信息
        /// </summary>
        /// <param name="bid"></param>
        /// <param name="tid"></param>
        /// <returns></returns>
        public dynamic InitProduct(long bid, long tid)
        {
            try
            {
                using (ISession s = SessionFactory.Instance.CreateSession())
                {
                    //图片集合
                    List<Img> imgs = s.List<Img>("where biz_type = @0 and biz_id = @1 order by order_index asc", ImgType.Product_Cover, bid);

                    //基础属性值
                    List<PdtAttrVal> baseAttrPdtVals = s.List<PdtAttrVal>("where product_id = @0", bid);

                    //扩展属性值
                    List<PdtExtAttrVal> pdtExtAttrVals = s.List<PdtExtAttrVal>("where product_id = @0", bid);

                    //基础属性名
                    List<AttrName> baseAttrNames = s.List<AttrName>("where product_type_id = @0 order by order_index DESC", tid);

                    //扩展属性名
                    List<ExtAttrName> attrNames = s.List<ExtAttrName>("where product_type_id = @0 order by order_index DESC", tid);

                    //扩展属性值
                    List<ExtAttrVal> attrValues = s.List<ExtAttrVal>("where product_type_id = @0 order by order_index DESC", tid);

                    //规格名称
                    List<SpecName> specNames = s.List<SpecName>("where product_type_id = @0", tid);

                    //规格值
                    List<SpecValue> specValues = s.List<SpecValue>("where product_type_id = @0", tid);

                    //自定义规格名称
                    List<SpecCustom> specCustoms = s.List<SpecCustom>("where product_id = @0", bid);

                    //商品对应规格详情
                    List<ProductSku> skus = s.List<ProductSku>("where product_id = @0 ", bid);


                    var data = new
                    {
                        //图片集合
                        imgs = imgs,
                        //基础属性值
                        baseAttrPdtVals = baseAttrPdtVals,
                        //扩展属性值
                        pdtExtAttrVals = pdtExtAttrVals,
                        //基础属性名
                        baseAttrNames = baseAttrNames,
                        //扩展属性名
                        extAttrNames = attrNames,
                        //扩展属性值
                        extAttrVals = attrValues,
                        //规格名称
                        specNames = specNames,
                        //规格值
                        specValues = specValues,
                        //自定义规格头
                        specCustoms = specCustoms,
                        //规格值
                        skus = skus,
                        //状态
                        State = StateCode.State_200
                    };
                    return data;
                }
            }
            catch
            {
                return new { State = 500 };
            }
        }



        /// <summary>
        /// 获取材料采购信息
        /// </summary>
        /// <param name="bid"></param>
        /// <param name="tid"></param>
        /// <returns></returns>
        public dynamic GetPurProduct(long bid, long tid)
        {
            try
            {
                using (ISession s = SessionFactory.Instance.CreateSession())
                {
                    //获取当前材料信息
                    //Product product = s.Get<Product>(new string[] { "unit", "is_open_spec" }, "where id = @0", bid);

                    //图片集合
                    List<Img> imgs = s.List<Img>("where biz_type = @0 and biz_id = @1 order by order_index asc", ImgType.Product_Cover, bid);

                    //规格名称
                    List<SpecName> specNames = s.List<SpecName>("where product_type_id = @0", tid);

                    //规格值
                    List<SpecValue> specValues = s.List<SpecValue>("where product_type_id = @0", tid);

                    //自定义规格名称
                    List<SpecCustom> specCustoms = s.List<SpecCustom>("where product_id = @0", bid);

                    //商品对应规格详情
                    List<ProductSku> skus = s.List<ProductSku>("where product_id = @0 ", bid);

                    var data = new
                    {
                        //图片集合
                        imgs = imgs,
                        //规格名称
                        specNames = specNames,
                        //规格值
                        specValues = specValues,
                        //自定义规格头
                        specCustoms = specCustoms,
                        //规格值
                        skus = skus,
                        //状态
                        State = StateCode.State_200
                    };
                    return data;
                }
            }
            catch
            {
                return new { State = 500 };
            }
        }






        /// <summary>
        /// 获取最大ID
        /// </summary>
        /// <returns></returns>
        public int GetMaxID()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                string count = s.ExecuteScalar("select MAX(id) from  dbo.tb_pdt_product").ToString();
                return (string.IsNullOrEmpty(count) ? 0 : int.Parse(count));
            }
        }



        #region 微信端展示调用方法

        /// <summary>
        /// 获取首页商品
        /// </summary>
        /// <returns></returns>
        public List<Product> GetIndex(long cgty_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                string sql = "where is_delete = @0 and is_index = @1 and is_shelves = @2 and guide_category_id = @3 (GETDATE() BETWEEN shelves_sdate AND shelves_edate) order by order_index desc";
                return s.List<Product>(sql, false, true, true, cgty_id);
            }
        }


        /// <summary>
        /// 加载商品信息 
        /// </summary>
        /// <param name="bid"></param>
        /// <returns></returns>
        public dynamic LoadProduct(long bid)
        {
            try
            {
                using (ISession s = SessionFactory.Instance.CreateSession())
                {
                    Product product = s.Get<Product>(bid);

                    if (product == null) return new { State = StateCode.State_501 };

                    //图片集合
                    List<Img> imgs = s.List<Img>("where biz_type = @0 and biz_id = @1 order by order_index asc", ImgType.Product_Cover, product.id);

                    //基础属性值
                    List<PdtAttrVal> baseAttrPdtVals = s.List<PdtAttrVal>("where product_id = @0", product.id);

                    //扩展属性值
                    List<PdtExtAttrVal> pdtExtAttrVals = s.List<PdtExtAttrVal>("where product_id = @0", product.id);

                    //基础属性名
                    List<AttrName> baseAttrNames = s.List<AttrName>("where product_type_id = @0 order by order_index DESC", product.product_type_id);

                    //扩展属性名
                    List<ExtAttrName> attrNames = s.List<ExtAttrName>("where product_type_id = @0 order by order_index DESC", product.product_type_id);

                    //扩展属性值
                    List<ExtAttrVal> attrValues = s.List<ExtAttrVal>("where product_type_id = @0 order by order_index DESC", product.product_type_id);

                    //规格名称
                    List<SpecName> specNames = s.List<SpecName>("where product_type_id = @0", product.product_type_id);

                    //规格值
                    List<SpecValue> specValues = s.List<SpecValue>("where product_type_id = @0", product.product_type_id);

                    //自定义规格名称
                    List<SpecCustom> specCustoms = s.List<SpecCustom>("where product_id = @0", product.id);

                    //商品对应规格详情
                    List<ProductSku> skus = s.List<ProductSku>("where product_id = @0 and is_enable = @1 and stock > 0", product.id, true);

                    //商品租金比例设置
                    List<RentRatio> ratios = s.List<RentRatio>("where product_id = @0 ", bid);

                    var data = new
                    {
                        product = product,
                        //图片集合
                        imgs = imgs.Select(m => m.getImgUrl()),
                        //基础属性值
                        baseAttrPdtVals = baseAttrPdtVals,
                        //扩展属性值
                        pdtExtAttrVals = pdtExtAttrVals,
                        //基础属性名
                        baseAttrNames = baseAttrNames,
                        //扩展属性名
                        extAttrNames = attrNames,
                        //扩展属性值
                        extAttrVals = attrValues,
                        //规格名称
                        specNames = specNames,
                        //规格值
                        specValues = specValues,
                        //自定义规格头
                        specCustoms = specCustoms,
                        //规格值
                        skus = skus,
                        //商品租金比例设置
                        ratios = ratios,
                        //状态
                        State = StateCode.State_200
                    };

                    return data;
                }
            }
            catch
            {
                return new { State = 500 };
            }
        }



        #endregion


    }
}
