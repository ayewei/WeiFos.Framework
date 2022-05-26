using WeiFos.ORM.Data;
using System.Data;
using WeiFos.Entity.ProductModule;


namespace WeiFos.Service.ProductModule
{

    public class SpecSetService : BaseService<SpecSet>
    {
        /// <summary>
        /// 获取供应商品规格详情
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public DataTable Gets(int sysUserID, int productID, int type)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {

                DataTable dt = null;
                if (type == 0)
                {
                    dt = s.Fill("select * from V_Sto_CommoditySpec where SysUserID=@0 and ProductID = @1", sysUserID, productID);
                }
                else
                {
                    dt = s.Fill("select * from V_Sto_Spec where SysUserID=@0 and ProductID = @1", sysUserID, productID);

                }
                return dt;
            }
        }

        /// <summary>
        /// 根据商品的SKUID 查找sku的值
        /// </summary>
        /// <param name="storePdtSKUID"></param>
        /// <returns></returns>
        //public string SpecValueIDS(int sku_id)
        //{
        //    List<SpecSet> skuValueIDS = new List<SpecSet>();
        //    string values = string.Empty;
        //    using (ISession s = SessionFactory.Instance.CreateSession())
        //    {
        //        skuValueIDS = s.List<SpecSet>(" where SKUID=@0", sku_id);

        //        foreach (SpecSet val in skuValueIDS)
        //        {
        //            values += s.Get<SpecValue>(" where id=@0", val.sku_id).Val + "\0";
        //        }
        //    }
        //    return values;
        //}



    }
}
