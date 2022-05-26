using WeiFos.ORM.Data;
using System;
using System.Collections.Generic;
using System.Text;
using WeiFos.Entity.ProductModule;
using WeiFos.Core;
using WeiFos.Entity.SKUModule; 

namespace WeiFos.Service.ProductModule
{
    /// <summary>
    /// 商品类型 SKU Service
    /// @author yewei
    /// add by  @date 2015-03-03
    /// </summary>
    public class ProductSkuService : BaseService<ProductSku>
    {

        /// <summary>
        /// 根据商品ID获取SKU集合
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public List<ProductSku> Gets(int product_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<ProductSku>("where product_id = @0", product_id);
            }
        }


        /// <summary>
        /// 获取规格详情
        /// </summary>
        /// <param name="specset"></param>
        /// <returns></returns>
        public string GetSkuMsg(string specset)
        {
            StringBuilder sb = new StringBuilder();
            string[] arr = StringHelper.StringToArray(specset);

            using (ISession s = SessionFactory.Instance.CreateSession())
            {
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
                            sb.Append(string.Format("{0}：{1}; ", specname.name, specvalue.val));
                        else
                            throw new Exception("商品规格信息异常");
                    }
                }

                return sb.Length > 0 ? sb.Remove(sb.Length - 1, 1).ToString() : null;
            }
        }


        /// <summary>
        /// 获取当前商品SKU信息
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="specset"></param>
        /// <returns></returns>
        public ProductSku Get(long product_id, string specset)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<ProductSku>(" where product_id = @0 and specset = @1", product_id, specset);
            }
        }

        /// <summary>
        /// 获取当前商品SKU信息
        /// </summary>
        /// <param name="product_id"></param>
        /// <param name="sku_id"></param>
        /// <returns></returns>
        public ProductSku Get(long product_id, int sku_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<ProductSku>(" where product_id = @0 and id = @1", product_id, sku_id);
            }
        }

    }
}
