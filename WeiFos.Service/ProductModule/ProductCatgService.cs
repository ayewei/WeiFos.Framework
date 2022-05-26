using WeiFos.ORM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WeiFos.Entity.ProductModule;
using WeiFos.Entity.SystemModule;
using WeiFos.Entity.BizTypeModule;
using WeiFos.Entity.Common;
using WeiFos.Entity.Enums;
using WeiFos.Entity.ResourceModule;
using WeiFos.ORM.Data.Restrictions;
using WeiFos.ORM.Data.Const;

namespace WeiFos.Service.ProductModule
{
    /// <summary>
    /// 商品类别 Service
    /// @author yewei 
    /// @date 2015-02-25
    /// </summary>
    public class ProductCatgService : BaseService<ProductCatg>
    {

        /// <summary>
        /// 根据父类ID获取
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public List<ProductCatg> GetListByParentId(long parent_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<ProductCatg>("where parent_id = @0 order by order_index desc ", parent_id);
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
                            s.ExcuteUpdate("update tb_pdt_category set order_index = @0 where id = @1 ", index, id);
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
        /// 查看分类名称是否存在
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public int ExistName(long id, string categoryName)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Exist<ProductCatg>("where id != @0 and name = @1 ", id, categoryName);
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
                return s.Exist<ProductCatg>("where id != @0 and serial_no = @1 ", id, serialNo);
            }
        }


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public StateCode Save(SysUser user, ProductCatg entity, string imgmsg)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                s.StartTransaction();
                try
                {
                    if (entity.id == 0)
                    {
                        //创建用户ID
                        entity.created_user_id = user.id;
                        //创建时间
                        entity.created_date = DateTime.Now;
                        s.Insert<ProductCatg>(entity);
                    }
                    else
                    {
                        //修改用户ID
                        entity.updated_user_id = user.id;
                        //修改时间
                        entity.updated_date = DateTime.Now;
                        s.Update<ProductCatg>(entity);
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
        /// 获取数据树
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tagstr"></param>
        /// <returns></returns>
        public List<ProductCatg> GetTrees(string name, string tagstr)
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
            List<ProductCatg> result = new List<ProductCatg>();

            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                int index = 0;
                List<ProductCatg> menus = s.List<ProductCatg>(ct);
                foreach (ProductCatg menu in menus.Where(m => m.parent_id == 0))
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
        public List<ProductCatg> GetItemChilds(long id, List<ProductCatg> cgtys, int index, string tagstr)
        {
            //结果集合
            List<ProductCatg> result = new List<ProductCatg>();

            if (cgtys == null || cgtys.Count == 0) return result;

            index++;
            string tag = "|--";
            for (int i = 0; i < index; i++)
            {
                tag = tagstr + tag;
            }

            //获取子集合
            List<ProductCatg> children = cgtys.Where(m => m.parent_id == id).ToList();
            if (children != null && children.Count > 0)
            {
                foreach (ProductCatg c in children)
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
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ProductCatg> GetParents(long id)
        {
            //当前产品类别对象集合
            List<ProductCatg> pcs = new List<ProductCatg>();

            //当前对象
            ProductCatg cgty = base.GetById(id);

            if (cgty != null)
            {
                pcs.Add(cgty);

                //父级对象
                ProductCatg pcgty = base.GetById(cgty.parent_id);
                if (pcgty != null)
                {
                    pcs.Insert(0, pcgty);
                    pcs.InsertRange(0, GetParents(pcgty.parent_id));
                }
            }
            return pcs;
        }


        /// <summary>
        /// 获取当前分类父类路径
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public string GetParentPath(long id)
        {
            List<ProductCatg> webCategory = GetParents(id);
            string str = string.Join(",", webCategory.OrderBy(p => p.id).Select(p => p.id).ToArray());
            return "," + str + ",";
        }


        /// <summary>
        /// 获取当前分类父类路径
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public string GetParentPathName(long id)
        {
            List<ProductCatg> webCategory = GetParents(id);
            string str = string.Join("-->", webCategory.OrderBy(p => p.id).Select(p => p.name).ToArray());
            return str;
        }



        /// <summary>
        /// 获取所有子节点
        /// </summary>
        /// <returns></returns>
        public List<ProductCatg> GetLastList()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<ProductCatg>("where [is_last] = @0", true);
            }
        }




    }
}
