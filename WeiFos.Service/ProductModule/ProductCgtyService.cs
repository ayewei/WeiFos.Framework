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
using WeiFos.Entity.OrgModule;

namespace WeiFos.Service.ProductModule
{
    /// <summary>
    /// 商品类别 Service
    /// @author yewei 
    /// @date 2015-02-25
    /// </summary>
    public class ProductCgtyService : BaseService<ProductCgty>
    {

        /// <summary>
        /// 根据父类ID获取
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public List<ProductCgty> GetListByParentId(long parent_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<ProductCgty>("where parent_id = @0 order by order_index desc ", parent_id);
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
                return s.Exist<ProductCgty>("where id != @0 and name = @1 ", id, categoryName);
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
                return s.Exist<ProductCgty>("where id != @0 and serial_no = @1 ", id, serialNo);
            }
        }


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="user"></param>
        /// <param name="productCgty"></param>
        /// <returns></returns>
        public StateCode Save(SysUser user, ProductCgty productCgty, string imgmsg)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                s.StartTransaction();
                try
                {
                    if (productCgty.id == 0)
                    {
                        //创建用户ID
                        productCgty.created_user_id = user.id;
                        //创建时间
                        productCgty.created_date = DateTime.Now;
                        s.Insert<ProductCgty>(productCgty);
                    }
                    else
                    {
                        //修改用户ID
                        productCgty.updated_user_id = user.id;
                        //修改时间
                        productCgty.updated_date = DateTime.Now;
                        s.Update<ProductCgty>(productCgty);
                    }

                    //判断是否存在图片信息
                    if (!string.IsNullOrEmpty(imgmsg) && imgmsg.IndexOf("#") != -1)
                    {
                        //图片名称
                        string filename = imgmsg.Split('#')[0];
                        //图片类型
                        string biztype = imgmsg.Split('#')[1];
                        //去除重复图片
                        s.ExcuteUpdate("update tb_img set biz_id = 0 where biz_type = @0 and biz_id = @1  ", biztype, productCgty.id);
                        Img img = s.Get<Img>(" where file_name = @0 and biz_type = @1 ", filename, biztype);
                        if (img != null)
                        {
                            img.biz_id = productCgty.id;
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
        /// 树形节点分类
        /// </summary>
        /// <param name="parent_id"></param>
        /// <param name="index"></param>
        /// <param name="tagstr"></param>
        /// <returns></returns>
        public List<ProductCgty> GetChildrenTag(long parent_id, int index, string tagstr)
        {
            List<ProductCgty> webCategory = new List<ProductCgty>();

            List<ProductCgty> children = GetListByParentId(parent_id);

            if (children != null && children.Count > 0)
            {
                index++;

                string tag = "|--";
                for (int i = 0; i < index; i++)
                {
                    tag = tagstr + tag;
                }

                foreach (ProductCgty c in children)
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
        public List<ProductCgty> GetChildren(long parent_id)
        {
            List<ProductCgty> webCategory = new List<ProductCgty>();

            List<ProductCgty> children = GetListByParentId(parent_id);

            if (children != null && children.Count > 0)
            {
                foreach (ProductCgty c in children)
                {
                    webCategory.Add(c);
                    webCategory.AddRange(GetChildren(c.id));
                }
            }
            return webCategory;
        }

        /// <summary>
        /// 是否存在子对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool HasChildren(long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Exist<ProductCgty>("where parent_id = @0", id) > 0;
            }
        }


        /// <summary>
        /// 获取父类集合
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ProductCgty> GetParents(long id)
        {
            //当前产品类别对象集合
            List<ProductCgty> pcs = new List<ProductCgty>();

            //当前对象
            ProductCgty cgty = base.GetById(id);

            if (cgty != null)
            {
                pcs.Add(cgty);

                //父级对象
                ProductCgty pcgty = base.GetById(cgty.parent_id);
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
            List<ProductCgty> webCategory = GetParents(id);
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
            List<ProductCgty> webCategory = GetParents(id);
            string str = string.Join("-->", webCategory.OrderBy(p => p.id).Select(p => p.name).ToArray());
            return str;
        }



        /// <summary>
        /// 获取所有子节点
        /// </summary>
        /// <returns></returns>
        public List<ProductCgty> GetLastList()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<ProductCgty>("where [is_last] = @0", true);
            }
        }




    }
}
