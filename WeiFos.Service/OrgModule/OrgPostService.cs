using System;
using System.Linq;
using WeiFos.ORM.Data;
using WeiFos.Entity.LogsModule;
using WeiFos.Entity.Common;
using WeiFos.Entity.OrgModule;
using WeiFos.Service;
using WeiFos.Entity.Enums;
using System.Collections.Generic;
using WeiFos.ORM.Data.Restrictions;
using WeiFos.ORM.Data.Const;

namespace weifos.Service.OrgModule
{

    /// <summary>
    /// 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
    /// Copyright (c) 2013-2018 深圳微狐信息技术有限公司
    /// 创 建：
    /// 日 期：2019-02-03 14:06:46
    /// 描 述：岗位业务逻辑
    /// </summary>
    public class OrgPostService : BaseService<OrgPost>
    {


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public StateCode Save(long user_id, OrgPost entity)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    if (entity.id == 0)
                    {
                        entity.created_date = DateTime.Now;
                        entity.created_user_id = user_id;
                        s.Insert(entity);
                    }
                    else
                    {
                        entity.updated_date = DateTime.Now;
                        entity.updated_user_id = user_id;
                        s.Update(entity);
                    }
                    return StateCode.State_200;
                }
                catch (Exception ex)
                {
                    s.Insert(new SystemLogs() { content = ex.ToString(), created_date = DateTime.Now, type = 1 });
                    return StateCode.State_500;
                }
            }
        }




        /// <summary>
        /// 树形结构
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tagstr"></param>
        /// <returns></returns>
        public List<OrgPost> GetTrees(string name, string tagstr)
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
            List<OrgPost> result = new List<OrgPost>();

            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                int index = 0;
                List<OrgPost> posts = s.List<OrgPost>(ct);
                foreach (OrgPost post in posts.Where(m => m.parent_id == 0))
                {
                    //设置标签 
                    result.Add(post);
                    //获取子节点
                    result.AddRange(GetItemChilds(post.id, posts.Where(m => m.parent_id != 0).ToList(), index, tagstr));
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
        public List<OrgPost> GetItemChilds(long id, List<OrgPost> cgtys, int index, string tagstr)
        {
            //结果集合
            List<OrgPost> result = new List<OrgPost>();

            if (cgtys == null || cgtys.Count == 0) return result;

            index++;
            string tag = "|--";
            for (int i = 0; i < index; i++)
            {
                tag = tagstr + tag;
            }

            //获取子集合
            List<OrgPost> children = cgtys.Where(m => m.parent_id == id).ToList();
            if (children != null && children.Count > 0)
            {
                foreach (OrgPost c in children)
                {
                    c.name = tag + c.name;
                    result.Add(c);
                    result.AddRange(GetItemChilds(c.id, cgtys, index, tagstr));
                }
            }

            return result;
        }






    }
}
