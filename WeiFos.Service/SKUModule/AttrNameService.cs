using WeiFos.ORM.Data;
using WeiFos.ORM.Data.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Entity.ProductModule;
using WeiFos.Entity.SKUModule;
using WeiFos.Entity.Enums;
using WeiFos.ORM.Data.Restrictions;
using WeiFos.ORM.Data.Const;

namespace WeiFos.Service.SKUModule
{
    /// <summary>
    /// 商品类型 基本属性名称
    /// @author yewei
    /// add by  @date 2015-02-27
    /// </summary>
    public class AttrNameService : BaseService<AttrName>
    {

        /// <summary>
        /// 保存基本属性名称
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public StateCode Save(long user_id, AttrName entity)
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
        /// 删除商品类型 基础属性名称
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public StateCode DeleteAttrName(long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    //存在商品数据引用该数据 
                    int exist_s = s.Exist<PdtAttrVal>("where attrname_id = @0 ", id);
                    if (exist_s > 0)
                    {
                        s.Commit();
                        return StateCode.State_1;
                    }

                    s.ExcuteUpdate("delete tb_sku_attrname where id = @0 ", id);
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
        /// 基础属性集合
        /// </summary>
        /// <param name="id"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<AttrName> Gets(long id, string keyword)
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
                me.Add(new SingleExpression("product_type_id", LogicOper.EQ, id));

                //设置查询条件
                ct.SetWhereExpression(me);

                return s.List<AttrName>(ct);
            }
        }





    }
}
