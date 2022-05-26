using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WeiFos.Entity.Enums;
using WeiFos.Entity.SeoModule;
using WeiFos.ORM.Data;

namespace WeiFos.Service.SeoModule
{

    /// <summary>
    /// 网站SEO关键字类别Service
    /// @author yewei
    /// @date 2013-09-22
    /// </summary>
    public class SeoKeyWordCgtyService : BaseService<SeoKeyWordCgty>
    {



        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public StateCode Save(long userId, SeoKeyWordCgty entity)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    if (entity.id == 0)
                    {
                        //创建用户ID
                        entity.created_user_id = userId;
                        //创建时间
                        entity.created_date = DateTime.Now;
                        s.Insert(entity);
                    }
                    else
                    {
                        //修改用户ID
                        entity.updated_user_id = userId;
                        //修改时间
                        entity.updated_date = DateTime.Now;
                        s.Update(entity);
                    }
                }
                catch
                {
                    return StateCode.State_500;
                }
                return StateCode.State_200;
            }
        }


        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isenable"></param>
        public void SetEnable(long id, bool isenable)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                SeoKeyWordCgty cgty = s.Get<SeoKeyWordCgty>(id);
                if (cgty != null)
                {
                    cgty.is_enable = isenable;
                    s.Update<SeoKeyWordCgty>(cgty);
                }
            }
        }


        /// <summary>
        /// 转移关键词信息
        /// </summary>
        /// <param name="currer_cgty_id"></param>
        /// <param name="change_cgty_id"></param>
        public void ChangeKeyWord(int currer_cgty_id, int change_cgty_id, int type)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                s.ExcuteUpdate("update tb_seo_keyword set category_id = @0 where category_id = @1 and type = @2 ", change_cgty_id, currer_cgty_id, type);
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
                s.StartTransaction();
                foreach (string str in parmas)
                {
                    string _id = str.Split('_')[0];
                    string index = str.Split('_')[1];

                    int id = 0;

                    string pattern = @"^\d*$";
                    int.TryParse(_id, out id);

                    if (id != 0 && Regex.IsMatch(index, pattern))
                    {
                        s.ExcuteUpdate("update tb_seo_keywordcgty set order_index = @0 where id = @1  ", index, id);
                    }
                }
                s.Commit();
            }
        }


        /// <summary>
        /// 获取子类递归集合
        /// </summary>
        /// <param name="parent_id"></param>
        /// <param name="index"></param>
        /// <param name="tagstr"></param>
        /// <returns></returns>
        public List<SeoKeyWordCgty> GetChildrenTag(int type, long parent_id, int index, string tagstr)
        {
            List<SeoKeyWordCgty> wCategory = new List<SeoKeyWordCgty>();

            List<SeoKeyWordCgty> children = GetListByParentId(parent_id, type);

            if (children != null && children.Count > 0)
            {
                index++;

                string tag = "|--";
                for (int i = 0; i < index; i++)
                {
                    tag = tagstr + tag;
                }

                foreach (SeoKeyWordCgty c in children)
                {
                    c.name = tag + c.name;
                    wCategory.Add(c);
                    wCategory.AddRange(GetChildrenTag(type, c.id, index, tagstr));
                }
            }
            return wCategory;
        }



        /// <summary>
        /// 根据父类ID获取
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public List<SeoKeyWordCgty> GetListByParentId(long parent_id, int type = 1)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<SeoKeyWordCgty>("where parent_id = @0 and type = @1 order by order_index desc ", parent_id, type);
            }
        }


        /// <summary>
        /// 递归排序
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public List<SeoKeyWordCgty> GetChildren(long parent_id, int type)
        {
            List<SeoKeyWordCgty> wCategory = new List<SeoKeyWordCgty>();

            List<SeoKeyWordCgty> children = GetListByParentId(parent_id, type);

            if (children != null && children.Count > 0)
            {
                foreach (SeoKeyWordCgty c in children)
                {
                    wCategory.Add(c);
                    wCategory.AddRange(GetChildren(c.id, type));
                }
            }
            return wCategory;
        }


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        public void Deletes(long id, int type)
        {
            List<SeoKeyWordCgty> categorys = new List<SeoKeyWordCgty>();

            SeoKeyWordCgty category = GetById(id);

            categorys.Add(category);
            categorys.AddRange(GetChildren(id, type));

            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                s.StartTransaction();
                foreach (SeoKeyWordCgty pc in categorys)
                {
                    s.ExcuteUpdate("delete tb_seo_keywordcgty where id = @0 and type = @1", pc.id, type);
                }
                s.Commit();
            }
        }



    }
}



