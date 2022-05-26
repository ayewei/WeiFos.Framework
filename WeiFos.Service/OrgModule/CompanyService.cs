using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Entity.Enums;
using WeiFos.Entity.LogsModule;
using WeiFos.Entity.OrgModule;
using WeiFos.ORM.Data;
using WeiFos.ORM.Data.Const;
using WeiFos.ORM.Data.Restrictions;

namespace WeiFos.Service.OrgModule
{

    /// <summary>
    /// 公司信息业务逻辑
    /// @author yewei
    /// add by  @date 2015-03-03
    /// </summary>
    public class CompanyService : BaseService<Company>
    {



        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public StateCode Save(long user_id, Company entity)
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
        /// 获取菜单
        /// 递归排序
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Company> GetCompanyTrees(string name)
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
            List<Company> result = new List<Company>();

            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //查询数据库所有
                List<Company> menus = s.List<Company>(ct);
                //初始化父级菜单
                List<Company> parents = menus.Where(m => m.parent_id == 0).ToList();
                //初始化子级菜单
                List<Company> childs = menus.Where(m => m.parent_id != 0).ToList();
                if (parents.Count > 0)
                {
                    foreach (Company menu in parents)
                    {
                        //获取子节点
                        menu.childs = GetItemChilds(menu.id, menus);
                        result.Add(menu); 
                    }
                }
                else
                {
                    foreach (Company menu in childs)
                    {
                        //获取子节点
                        menu.childs = GetItemChilds(menu.id, menus);
                        result.Add(menu); 
                    }
                }

                return result;
            }
        }



        /// <summary>
        /// 获取指定集合子类
        /// </summary>
        /// <param name="id"></param>
        /// <param name="menus"></param>
        /// <returns></returns>
        public List<Company> GetItemChilds(long id, List<Company> menus)
        {
            if (menus == null || menus.Count == 0) return null;

            //结果集合
            List<Company> result = new List<Company>();

            //获取子集合
            List<Company> children = menus.Where(m => m.parent_id == id).ToList();
            if (children != null && children.Count > 0)
            {
                foreach (Company c in children)
                {
                    result.Add(c);
                    result.AddRange(GetItemChilds(c.id, menus));
                }
            }

            return result;
        }



        /// <summary>
        /// 树形结构
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tagstr"></param>
        /// <returns></returns>
        public List<Company> GetTrees(string name, string tagstr)
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
            List<Company> result = new List<Company>();

            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                int index = 0;
                List<Company> menus = s.List<Company>(ct);
                foreach (Company menu in menus.Where(m => m.parent_id == 0))
                {
                    //设置标签
                    menu.name = "|--[" + menu.en_name + "]" + menu.name;
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
        public List<Company> GetItemChilds(long id, List<Company> cgtys, int index, string tagstr)
        {
            //结果集合
            List<Company> result = new List<Company>();

            if (cgtys == null || cgtys.Count == 0) return result;

            index++;
            string tag = "|--";
            for (int i = 0; i < index; i++)
            {
                tag = tagstr + tag;
            }

            //获取子集合
            List<Company> children = cgtys.Where(m => m.parent_id == id).ToList();
            if (children != null && children.Count > 0)
            {
                foreach (Company c in children)
                {
                    c.name = tag + "[" + c.en_name + "]" + c.name;
                    result.Add(c);
                    result.AddRange(GetItemChilds(c.id, cgtys, index, tagstr));
                }
            }

            return result;
        }






    }
}
