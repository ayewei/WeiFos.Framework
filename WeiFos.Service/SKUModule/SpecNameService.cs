using WeiFos.ORM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Entity.Common;
using WeiFos.Entity.ProductModule;
using WeiFos.Entity.SKUModule;
using WeiFos.Entity.Enums;
using WeiFos.ORM.Data.Restrictions;
using WeiFos.ORM.Data.Const;

namespace WeiFos.Service.SKUModule
{

    /// <summary>
    /// 供应商规格名称 Service
    /// @author   
    /// add by @date 2015-02-28 
    /// </summary>
    public class SpecNameService : BaseService<SpecName>
    {

        /// <summary>
        /// 获取当前供应商规格
        /// </summary>
        /// <param name="type_id"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<SpecName> Gets(long type_id, string keyword)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {

                //查询对象
                Criteria ct = new Criteria();

                //查询表达式
                MutilExpression me = new MutilExpression();

                ct.SetFields(new string[] { "*" })
                .AddOrderBy(new OrderBy("order_index", "desc"));

                //名称
                if (!string.IsNullOrEmpty(keyword))
                {
                    me.Add(new SingleExpression("name", LogicOper.LIKE, keyword));
                }

                //所属商品类型
                me.Add(new SingleExpression("product_type_id", LogicOper.EQ, type_id));

                //设置查询条件
                ct.SetWhereExpression(me);

                return s.List<SpecName>(ct);
            }
        }


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="specName"></param>
        /// <param name="specValues"></param>
        public StateCode Save(SpecName specName, string[] specValues)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    if (specName.id == 0)
                    {
                        s.Insert(specName);

                        foreach (string str in specValues)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                string val = str;
                                if (str.Length > 15) val = str.Substring(0, 14).ToString();

                                SpecValue ssv = new SpecValue();

                                //所属属性
                                ssv.specname_id = specName.id;
                                ssv.val = val;
                                ssv.product_type_id = specName.product_type_id;
                                ssv.created_user_id = specName.created_user_id;
                                ssv.created_date = DateTime.Now;
                                s.Insert(ssv);
                            }
                        }
                    }
                    else
                    {
                        s.Update(specName);
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
        /// 删除规格
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public StateCode DeleteSpecName(long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    int exist = s.Exist<SpecSet>("where specname_id = @0 ", id);
                    if (exist > 0)
                    {
                        s.Commit();
                        return StateCode.State_501;
                    }

                    //删除规格信息
                    s.ExcuteUpdate("delete tb_sku_specname where id = @0 ", id);

                    //删除规格对应的值
                    s.ExcuteUpdate("delete tb_sku_specvalue where specname_id = @0 ", id);

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
        /// 根据商品ID、商品规格名称ID、商品规格名称值ID，获取自定义信息
        /// </summary>
        /// <returns></returns>
        public SpecCustom GetSpecCustomMsg(long productID, long skuNameID, long skuValueID)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<SpecCustom>(" where product_id = @0 and specname_id=@1 AND specvalue_id = @2", productID, skuNameID, skuValueID);
            }
        }

    }
}
