using WeiFos.ORM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Entity.Common;
using WeiFos.Entity.ShopSettingModule;
using WeiFos.Service;
using WeiFos.Entity.ProductModule;
using WeiFos.Entity.Enums;

namespace WeiFos.Service.ShopSettingModule
{
    /// <summary>
    /// 前端商品类别 Service
    /// @author yewei 
    /// @date 2015-03-19
    /// </summary>
    public class GuideProductCgtyService : BaseService<GuideProductCgty>
    {


        /// <summary>
        /// 获取首页导购分类
        /// </summary>
        /// <returns></returns>
        public List<GuideProductCgty> GetIndex(out List<Product> productlist)
        {
            productlist = new List<Product>();
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                List<GuideProductCgty> list = s.GetTop<GuideProductCgty>(2, "where is_index = @0 and parent_id = 0 order by order_index desc", true);
                if (list.Count > 1)
                {
                    List<GuideProductCgty> childs = s.GetTop<GuideProductCgty>(2, "where parent_id = @0 order by order_index desc", list[1].id);
                    foreach (GuideProductCgty cgty in childs)
                    {
                        if (cgty != null)
                        {
                            list.Add(cgty);
                        }
                    }
                }

                foreach (GuideProductCgty c in list)
                {
                    string sql = "where is_delete = @0 and is_index = @1 and is_shelves = @2 and guide_category_id = @3 and (GETDATE() BETWEEN shelves_startdate AND shelves_enddate) order by order_index desc";
                    productlist.AddRange(s.List<Product>(sql, false, true, true, c.id));
                }
                return list;
            }
        }



        /// <summary>
        /// 查看分类名称是否存在
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public int ExistName(long id, string categoryName)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Exist<GuideProductCgty>("where id != @0 and name = @1 ", id, categoryName);
            }
        }


        /// <summary>
        /// 查看分类编号是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public int ExistSerialNo(long id, string serialNo)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Exist<GuideProductCgty>("where id != @0 and serial_no = @1 ", id, serialNo);
            }
        }


        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isenable"></param>
        public StateCode SetEnable(long id, bool isshow)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.ExcuteUpdate("update tb_fnt_guide_productcgty set is_show = @0 where id = @1", isshow, id);
                    return StateCode.State_200;
                }
                catch
                {
                    return StateCode.State_500;
                }
            }
        }


        /// <summary>
        /// 保存绑定导购商品
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cgtyid"></param>
        /// <returns></returns>
        public StateCode SaveSelect(long[] ids, long cgtyid)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    for (int i = 0; i < ids.Count(); i++)
                    {
                        //设置上架或下架
                        s.ExcuteUpdate("update tb_pdt_product set guide_category_id = @0 where id = @1 ", cgtyid, ids[i]);
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
        /// 根据父类ID获取
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public List<GuideProductCgty> GetListByParentId(long parent_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<GuideProductCgty>("where parent_id = @0 order by order_index desc ", parent_id);
            }
        }


        /// <summary>
        /// 获取子类递归集合
        /// </summary>
        /// <param name="parent_id"></param>
        /// <param name="index"></param>
        /// <param name="tagstr"></param>
        /// <returns></returns>
        public List<GuideProductCgty> GetChildrenTag(long parent_id, int index, string tagstr)
        {
            List<GuideProductCgty> webCategory = new List<GuideProductCgty>();

            List<GuideProductCgty> children = GetListByParentId(parent_id);

            if (children != null && children.Count > 0)
            {
                index++;

                string tag = "|--";
                for (int i = 0; i < index; i++)
                {
                    tag = tagstr + tag;
                }

                foreach (GuideProductCgty c in children)
                {
                    c.name = tag + c.name;
                    webCategory.Add(c);
                    webCategory.AddRange(GetChildrenTag(c.id, index, tagstr));
                }
            }
            return webCategory;
        }


        /// <summary>
        /// 递归排序
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public List<GuideProductCgty> GetChildren(long parent_id)
        {
            List<GuideProductCgty> webCategory = new List<GuideProductCgty>();

            List<GuideProductCgty> children = GetListByParentId(parent_id);

            if (children != null && children.Count > 0)
            {
                foreach (GuideProductCgty c in children)
                {
                    webCategory.Add(c);
                    webCategory.AddRange(GetChildren(c.id));
                }
            }
            return webCategory;
        }


        /// <summary>
        /// 获取父类集合
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public List<GuideProductCgty> GetParents(long parent_id)
        {
            List<GuideProductCgty> webCategory = new List<GuideProductCgty>();

            GuideProductCgty cgtyShow = ServiceIoc.Get<GuideProductCgtyService>().GetById(parent_id);
            if (cgtyShow != null)
            {
                webCategory.Add(cgtyShow);
                webCategory.AddRange(GetParents(cgtyShow.parent_id));
            }

            return webCategory;
        }


        /// <summary>
        /// 是否是最后子分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsLastChildren(long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return !(s.Exist<GuideProductCgty>("where parent_id == @0 ", id) > 0);
            }
        }


        /// <summary>
        /// 获取当前分类父类路径
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public string GetParentPath(long id)
        {
            List<GuideProductCgty> webCategory = GetParents(id);
            string str = string.Join(",", webCategory.OrderBy(p => p.id).Select(p => p.id).ToArray());
            return str + ",";
        }



    }
}
