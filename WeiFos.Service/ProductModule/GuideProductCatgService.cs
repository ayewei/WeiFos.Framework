using WeiFos.ORM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Entity.Common; 
using WeiFos.Entity.ProductModule;
using WeiFos.Entity.Enums;
using WeiFos.Entity.ResourceModule;
using WeiFos.ORM.Data.Restrictions;
using WeiFos.ORM.Data.Const;
using System.Text.RegularExpressions;

namespace WeiFos.Service.ProductModule
{

    /// <summary>
    /// 前端商品类别 Service
    /// @author yewei 
    /// @date 2015-03-19
    /// </summary>
    public class GuideProductCatgService : BaseService<GuideProductCatg>
    {


        /// <summary>
        /// 保存导购分类
        /// </summary>
        /// <param name="sys_user_id"></param>
        /// <param name="entity"></param>
        /// <param name="imgmsg"></param>
        /// <returns></returns>
        public StateCode Save(long sys_user_id, GuideProductCatg entity, string imgmsg)
        {
            string p_path = ServiceIoc.Get<GuideProductCatgService>().GetParentPath(entity.parent_id);
            entity.parent_path = p_path;
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                s.StartTransaction();
                try
                {
                    if (entity.id == 0)
                    {
                        //创建用户ID
                        entity.created_user_id = sys_user_id;
                        //创建时间
                        entity.created_date = DateTime.Now;
                        s.Insert(entity);
                    }
                    else
                    {
                        //修改用户ID
                        entity.updated_user_id = sys_user_id;
                        //修改时间
                        entity.updated_date = DateTime.Now;
                        s.Update(entity);
                    }

                    //判断是否存在图片信息
                    if (!string.IsNullOrEmpty(imgmsg) && imgmsg.IndexOf("#") != -1)
                    {
                        //图片名称
                        string filename = imgmsg.Split('#')[0];
                        //图片类型
                        string biztype = imgmsg.Split('#')[1];
                        //去除重复图片
                        s.ExcuteUpdate("update tb_img set biz_id = 0 where biz_type = @0 and biz_id = @1  ", biztype, entity.id);
                        Img img = s.Get<Img>(" where file_name = @0 and biz_type = @1 ", filename, biztype);
                        if (img != null)
                        {
                            img.biz_id = entity.id;
                            s.Update<Img>(img);
                        }
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
        /// 保存排序
        /// </summary>
        /// <param name="parmas"></param>
        public void SaveOrderIndex(string[] parmas)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    foreach (string str in parmas)
                    {
                        string data_id = str.Split('_')[0];
                        string index = str.Split('_')[1];

                        int id = 0;

                        string pattern = @"^\d*$";
                        int.TryParse(data_id, out id);

                        if (id != 0 && Regex.IsMatch(index, pattern))
                        {
                            s.ExcuteUpdate("update tb_pdt_guidecatg set order_index = @0 where id = @1 ", index, id);
                        }
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
        /// 获取首页导购分类
        /// </summary>
        /// <returns></returns>
        public List<GuideProductCatg> GetIndex(out List<Product> productlist)
        {
            productlist = new List<Product>();
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                List<GuideProductCatg> list = s.GetTop<GuideProductCatg>(2, "where is_index = @0 and parent_id = 0 order by order_index desc", true);
                if (list.Count > 1)
                {
                    List<GuideProductCatg> childs = s.GetTop<GuideProductCatg>(2, "where parent_id = @0 order by order_index desc", list[1].id);
                    foreach (GuideProductCatg cgty in childs)
                    {
                        if (cgty != null)
                        {
                            list.Add(cgty);
                        }
                    }
                }

                foreach (GuideProductCatg c in list)
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
                return s.Exist<GuideProductCatg>("where id != @0 and name = @1 ", id, categoryName);
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
                return s.Exist<GuideProductCatg>("where id != @0 and serial_no = @1 ", id, serialNo);
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
        /// 根据父类ID获取
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public List<GuideProductCatg> GetListByParentId(long parent_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<GuideProductCatg>("where parent_id = @0 order by order_index desc ", parent_id);
            }
        }


        /// <summary>
        /// 获取数据树
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tagstr"></param>
        /// <returns></returns>
        public List<GuideProductCatg> GetTrees(string name, string tagstr)
        {
            //查询对象
            Criteria ct = new Criteria();

            //查询表达式
            MutilExpression me = new MutilExpression();

            ct.SetFields(new string[] { "*" })
            .AddOrderBy(new OrderBy("order_index", "desc"));

            //登录名称
            if (!string.IsNullOrEmpty(name))
            {
                me.Add(new SingleExpression("name", LogicOper.LIKE, name));
            }

            //设置查询条件
            if (me.Expressions.Count > 0)
            {
                ct.SetWhereExpression(me);
            }

            //结果集合
            List<GuideProductCatg> result = new List<GuideProductCatg>();

            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                int index = 0;
                List<GuideProductCatg> menus = s.List<GuideProductCatg>(ct);
                foreach (GuideProductCatg menu in menus.Where(m => m.parent_id == 0))
                {
                    //设置标签
                    menu.name = "|--" + menu.name;
                    result.Add(menu);
                    //获取子节点
                    result.AddRange(GetItemChilds(menu.id, menus.Where(m => m.parent_id != 0).ToList(), index, tagstr));
                }

                return result;
            }
        }


        /// <summary>
        /// 获取指定集合子类
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cgtys"></param>
        /// <param name="index"></param>
        /// <param name="tagstr"></param>
        /// <returns></returns>
        public List<GuideProductCatg> GetItemChilds(long id, List<GuideProductCatg> cgtys, int index, string tagstr)
        {
            //结果集合
            List<GuideProductCatg> result = new List<GuideProductCatg>();

            if (cgtys == null || cgtys.Count == 0) return result;

            index++;
            string tag = "|--";
            for (int i = 0; i < index; i++)
            {
                tag = tagstr + tag;
            }

            //获取子集合
            List<GuideProductCatg> children = cgtys.Where(m => m.parent_id == id).ToList();
            if (children != null && children.Count > 0)
            {
                foreach (GuideProductCatg c in children)
                {
                    c.name = tag + c.name;
                    result.Add(c);
                    result.AddRange(GetItemChilds(c.id, cgtys, index, tagstr));
                }
            }

            return result;
        }


        /// <summary>
        /// 获取父类集合
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public List<GuideProductCatg> GetParents(long parent_id)
        {
            List<GuideProductCatg> webCategory = new List<GuideProductCatg>();

            GuideProductCatg cgtyShow = ServiceIoc.Get<GuideProductCatgService>().GetById(parent_id);
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
                return !(s.Exist<GuideProductCatg>("where parent_id == @0 ", id) > 0);
            }
        }


        /// <summary>
        /// 获取当前分类父类路径
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public string GetParentPath(long id)
        {
            List<GuideProductCatg> webCategory = GetParents(id);
            string str = string.Join(",", webCategory.OrderBy(p => p.id).Select(p => p.id).ToArray());
            return str + ",";
        }

        /// <summary>
        /// 获取当前分类父类路径
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public string GetParentPathName(long id)
        {
            List<GuideProductCatg> webCategory = GetParents(id);
            string str = string.Join("-->", webCategory.OrderBy(p => p.id).Select(p => p.name).ToArray());
            return str;
        }




    }
}
