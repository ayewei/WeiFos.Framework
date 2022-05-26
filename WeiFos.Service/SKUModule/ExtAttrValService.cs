using WeiFos.ORM.Data;
using System;
using System.Collections.Generic;
using WeiFos.Entity.Common;
using WeiFos.Entity.SKUModule;
using WeiFos.Entity.Enums;

namespace WeiFos.Service.SKUModule
{
    /// <summary>
    /// 商品类型扩展属性值
    /// @author yewei
    /// add by  @date 2015-02-27
    /// </summary>
    public class ExtAttrValService : BaseService<ExtAttrVal>
    {

        /// <summary>
        /// 根据扩展名称ID获取
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ExtAttrVal> Gets(long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<ExtAttrVal>("where ext_attr_name_id = @0", id);
            }
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public StateCode DeleteExtAttrVal(long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    //删除该属性对应产品引用的数据
                    s.ExcuteUpdate("delete tb_sku_ext_attrval where id = @0 ", id);

                    //删除该属性对应产品引用的数据
                    s.ExcuteUpdate("delete tb_pdt_extattrval where extattrval_id = @0 ", id);

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
        /// 保存扩展属性
        /// </summary>
        /// <param name="extAttrVal"></param>
        /// <returns></returns>
        public StateCode SaveExtAttrVal(ExtAttrVal extAttrVal)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    if (extAttrVal.id == 0)
                    {
                        s.Insert<ExtAttrVal>(extAttrVal);
                    }
                    else
                    {
                        s.Update<ExtAttrVal>(extAttrVal);
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



    }
}