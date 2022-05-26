using WeiFos.ORM.Data;
using System;
using System.Collections.Generic;
using WeiFos.Entity.ProductModule;
using WeiFos.Entity.SKUModule;
using WeiFos.Entity.Enums; 


namespace WeiFos.Service.SKUModule
{

    /// <summary>
    /// 商品类型 规格值
    /// @author yewei
    /// add by  @date 2015-03-03
    /// </summary>
    public class SpecValueService : BaseService<SpecValue>
    {

        /// <summary>
        /// 获取规格值
        /// </summary>
        /// <param name="specname_id"></param>
        /// <returns></returns>
        public List<SpecValue> Gets(long specname_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<SpecValue>("where specname_id = @0", specname_id);
            }
        }


        /// <summary>
        /// 保存规格值
        /// </summary>
        /// <param name="specValue"></param>
        /// <returns></returns>
        public StateCode SaveSpecValue(SpecValue specValue)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    if (specValue.id == 0)
                    {
                        s.Insert<SpecValue>(specValue);
                    }
                    else
                    {
                        s.Update<SpecValue>(specValue);
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
        /// 删除规格值
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public StateCode DeleteSpecValue(long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    SpecValue sv = s.Get<SpecValue>(id);

                    if (sv != null)
                    {
                        //平台商品引用
                        int exist_p = s.Exist<SpecSet>("where specvalue_id = @0 ", id);
                        if (exist_p > 0)
                        {
                            s.Commit();
                            return StateCode.State_503;
                        }
                    }

                    //删除商品类型规格值
                    s.ExcuteUpdate("delete tb_sku_specvalue where id = @0 ", id);

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




    }
}
