using WeiFos.ORM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.Entity.ProductModule;
using WeiFos.Entity.SKUModule;
using WeiFos.Entity.Enums;
using Newtonsoft.Json;

namespace WeiFos.Service.SKUModule
{
    public class ProductTypeService : BaseService<ProductType>
    {


        /// <summary>
        /// 保存商品类型
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public StateCode Save(long user_id, ProductType entity)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {

                    if (entity.id == 0)
                    {
                        //创建用户ID
                        entity.created_user_id = user_id;

                        //创建时间
                        entity.created_date = DateTime.Now;

                        s.Insert(entity);
                    }
                    else
                    {
                        //修改用户ID
                        entity.updated_user_id = user_id;

                        //修改时间
                        entity.updated_date = DateTime.Now;

                        s.Update(entity);
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
        /// 获取商品类型
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductType Get(long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //供应商商品类型表是否存在
                return s.Get<ProductType>("where id = @0 ", id);
            }
        }


        /// <summary>
        /// 根据id获取是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int ExistById(long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //供应商商品类型表是否存在
                return s.Exist<ProductType>("where id = @0 ", id);
            }
        }


        /// <summary>
        /// 供应商 商品类型是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type_name"></param>
        /// <returns></returns>
        public int ExistTypeName(long id, string type_name)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //商品类型名称是否存在
                return s.Exist<ProductType>("where name = @0 and id != @1 ", type_name, id);
            }
        }


        /// <summary>
        /// 删除商品类型
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type_name"></param>
        /// <returns></returns>
        public StateCode DeleteType(long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    //子门店中心 是否存在商品使用类型
                    int exist_p = s.Exist<Product>("where product_type_id = @0 ", id);
                    if (exist_p > 0)
                    {
                        s.Commit();
                        return StateCode.State_503;
                    }

                    //当前类型对应规格名称集合
                    List<SpecName> exist_SpecNames = s.List<SpecName>("where product_type_id = @0 ", id);
                    foreach (SpecName ssn in exist_SpecNames)
                    {
                        int exist_spec = s.Exist<SpecSet>("where specname_id = @0", ssn.id);

                        //存在商品SKU引用
                        if (exist_spec > 0)
                        {
                            s.Commit();
                            return StateCode.State_503;
                        }
                    }

                    //删除基础属性模块
                    List<AttrName> attrNames = s.List<AttrName>("where product_type_id = @0 ", id);
                    foreach (AttrName an in attrNames)
                    {
                        s.ExcuteUpdate("delete tb_pdt_attrval where attrname_id = @0 ", an.id);
                    }
                    s.ExcuteUpdate("delete tb_sku_attrname where product_type_id = @0 ", id);
                    //删除基础属性模块

                    //删除扩展属性模块
                    List<ExtAttrName> extAttrNames = s.List<ExtAttrName>("where product_type_id = @0", id);
                    foreach (ExtAttrName ean in extAttrNames)
                    {
                        s.ExcuteUpdate("delete tb_sku_ext_attrval where ext_attr_name_id = @0 ", ean.id);
                    }
                    s.ExcuteUpdate("delete tb_sku_ext_attrname where product_type_id = @0", id);
                    //删除扩展属性模块

                    //删除规格模块
                    List<SpecName> specNames = s.List<SpecName>("where product_type_id = @0 ", id);
                    foreach (SpecName ssn in specNames)
                    {
                        //删除规格名称
                        s.ExcuteUpdate("delete tb_sku_specname where product_type_id = @0", id);

                        //删除基础属性名对应的商品值
                        s.ExcuteUpdate("delete tb_sku_specvalue where specname_id = @0", ssn.id);
                    }
                    //删除规格模块

                    //最后删除 商品类型
                    s.ExcuteUpdate("delete tb_sku_product_type where id = @0 ", id);
                    s.Commit();
                    return StateCode.State_200;
                }
                catch(Exception ex)
                {
                    s.RollBack();
                    return StateCode.State_500;
                }
            }
        }


        /// <summary>
        /// 创建json格式sku 信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public dynamic CreateSKU(long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            { 
                //基础属性名称
                List<AttrName> baseAttrNames = new List<AttrName>();

                //扩展属性名
                List<ExtAttrName> attrNames = new List<ExtAttrName>();
                List<ExtAttrVal> attrValues = new List<ExtAttrVal>();

                //自定义规格
                List<SpecName> specNames = new List<SpecName>();
                List<SpecValue> specValues = new List<SpecValue>();

                //获取当前商品类型
                ProductType productType = s.Get<ProductType>(id);

                if (productType != null)
                {
                    //商品类型基础属性
                    baseAttrNames = s.List<AttrName>("where product_type_id = @0 order by order_index DESC", id);

                    attrNames = s.List<ExtAttrName>("where product_type_id = @0 order by order_index DESC", id);
                    if (attrNames != null && attrNames.Count > 0)
                    {
                        foreach (ExtAttrName atn in attrNames)
                        {
                            List<ExtAttrVal> atnValues = s.List<ExtAttrVal>("where ext_attr_name_id = @0", atn.id);
                            if (atnValues != null && atnValues.Count > 0)
                            {
                                attrValues.AddRange(atnValues);
                            }
                        }
                    }

                    specNames = s.List<SpecName>("where product_type_id = @0 order by order_index DESC", id);
                    if (specNames != null && specNames.Count > 0)
                    {
                        foreach (SpecName tsp in specNames)
                        {
                            List<SpecValue> atvValues = s.List<SpecValue>("where specname_id = @0", tsp.id); 
                            if (atvValues != null && atvValues.Count > 0)
                            {
                                specValues.AddRange(atvValues);
                            }
                        }
                    }
                }

                return new {
                    baseAttrNames = baseAttrNames,
                    extAttrNames = attrNames,
                    extAttrVals = attrValues,
                    specNames = specNames,
                    specValues = specValues
                };
            }
        }



    }
}

