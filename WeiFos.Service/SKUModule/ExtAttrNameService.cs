using WeiFos.ORM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Entity.SKUModule;
using WeiFos.Entity.Enums;
using WeiFos.ORM.Data.Restrictions;
using WeiFos.ORM.Data.Const;

namespace WeiFos.Service.SKUModule
{

    /// <summary>
    /// 商品类型  扩展属性名称
    /// @author yewei
    /// add by  @date 2015-02-27
    /// </summary>
    public class ExtAttrNameService : BaseService<ExtAttrName>
    {
        /// <summary>
        /// 保存扩展属性
        /// </summary>
        /// <param name="extAttrName"></param>
        /// <param name="productTypeAttrValues"></param>
        /// <param name="user_id"></param>
        public StateCode Save(ExtAttrName extAttrName, string[] productTypeAttrValues)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    if (extAttrName.id == 0)
                    {
                        s.Insert<ExtAttrName>(extAttrName);

                        foreach (string str in productTypeAttrValues)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                string val = str;
                                if (str.Length > 15) val = str.Substring(0, 14).ToString();

                                ExtAttrVal ptv = new ExtAttrVal();

                                //所属属性
                                ptv.ext_attr_name_id = extAttrName.id;
                                ptv.val = val;
                                ptv.product_type_id = extAttrName.product_type_id;
                                ptv.created_date = DateTime.Now;
                                ptv.created_user_id = extAttrName.created_user_id;
                                s.Insert<ExtAttrVal>(ptv);
                            }
                        }
                    }
                    else
                    {
                        s.Update<ExtAttrName>(extAttrName);
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
        /// 删除
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public StateCode DeleteExtAttrName(long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    //删除该属性
                    s.ExcuteUpdate("delete tb_sku_ext_attrname where id = @0 ", id);

                    //删除该属性对应所有值 所有商品扩展属性
                    List<ExtAttrVal> ExtAttrVals = s.List<ExtAttrVal>("where ext_attr_name_id = @0 ", id);
                    foreach (ExtAttrVal ptv in ExtAttrVals)
                    {
                        //删除该属性对应产品引用的数据
                        s.ExcuteUpdate("delete tb_sku_ext_attrval where id = @0 ", ptv.id);

                        //删除该属性对应产品引用的数据
                        s.ExcuteUpdate("delete tb_pdt_extattrval where extattrval_id = @0 ", ptv.id);

                        //删除该属性对应产品引用的数据
                        //s.ExcuteUpdate("delete tb_sup_pdt_extattrval where extattrval_id = @0 ", ptv.id);
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
        /// 获取商品类扩展名
        /// </summary>
        /// <param name="type_id"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<ExtAttrName> Gets(long type_id, string keyword)
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

                return s.List<ExtAttrName>(ct);
            }
        }







    }

}
