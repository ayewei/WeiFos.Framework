using WeiFos.Entity.SystemModule;
using WeiFos.ORM.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WeiFos.Entity.Enums;
using WeiFos.Entity.LogsModule;
using WeiFos.ORM.Data.Restrictions;
using WeiFos.ORM.Data.Const;

namespace WeiFos.Service.SystemModule
{
    /// <summary>
    /// 系统菜单Service
    /// @author yewei  
    /// @date 2013-05-05
    /// </summary>
    public class SysModelMenuService : BaseService<SysModelMenu>
    {


        /// <summary>
        /// 保存系统菜单
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public StateCode Save(long user_id, SysModelMenu entity)
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
        /// 根据父类Id获取分类信息
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public List<SysModelMenu> GetListByParentId(long parent_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<SysModelMenu>("where parent_id = @0 order by order_index desc", parent_id);
            }
        }



        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isenable"></param>
        public StateCode SetEnable(long id, bool isenable)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    int exist = s.Exist<SysModelMenu>("where id = @0", id);
                    if (exist > 0)
                    {
                        s.ExcuteUpdate("update tb_sys_model_menu set is_enable = @0 where id = @1", isenable, id);

                        //将自己的子菜单设置不可用
                        List<SysModelMenu> menus = s.List<SysModelMenu>("where parent_id = @0", id);
                        if (menus != null && menus.Count() > 0)
                        {
                            foreach (SysModelMenu m in menus)
                            {
                                s.ExcuteUpdate("update tb_sys_model_menu set is_enable = @0 where id = @1", isenable, m.id);
                            }
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
        /// 递归获取
        /// </summary>
        /// <param name="parent_id"></param>
        /// <param name="index"></param>
        /// <param name="tagstr"></param>
        /// <returns></returns>
        public List<SysModelMenu> GetChildrenTag(long parent_id, int index, string tagstr)
        {
            List<SysModelMenu> webCategory = new List<SysModelMenu>();

            List<SysModelMenu> children = GetListByParentId(parent_id);

            if (children != null && children.Count > 0)
            {
                index++;

                string tag = "|--";
                for (int i = 0; i < index; i++)
                {
                    tag = tagstr + tag;
                }

                foreach (SysModelMenu c in children)
                {
                    c.name = tag + c.name;
                    webCategory.Add(c);
                    webCategory.AddRange(GetChildrenTag(c.id, index, tagstr));
                }
            }
            return webCategory;
        }


        /// <summary>
        /// 获取菜单
        /// 递归排序
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<SysModelMenu> GetMenus(string name)
        {
            //查询对象
            Criteria ct = new Criteria();

            //查询表达式
            MutilExpression me = new MutilExpression();

            ct.SetFromTables("tb_sys_model_menu")
            .SetFields(new string[] { "*" })
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
            List<SysModelMenu> result = new List<SysModelMenu>();

            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //查询数据库所有
                List<SysModelMenu> menus = s.List<SysModelMenu>(ct);
                //初始化父级菜单
                List<SysModelMenu> parents = menus.Where(m => m.parent_id == 0).ToList();
                //初始化子级菜单
                List<SysModelMenu> childs = menus.Where(m => m.parent_id != 0).ToList();
                if (parents.Count > 0)
                {
                    foreach (SysModelMenu menu in parents)
                    {
                        result.Add(menu);
                        List<SysModelMenu> tmps = GetItemChilds(menu.id, menus);
                        if (tmps != null) result.AddRange(GetItemChilds(menu.id, menus));
                    }
                }
                else
                {
                    foreach (SysModelMenu menu in childs)
                    {
                        result.Add(menu);
                        List<SysModelMenu> tmps = GetItemChilds(menu.id, menus);
                        if (tmps != null) result.AddRange(GetItemChilds(menu.id, menus));
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
        public List<SysModelMenu> GetItemChilds(long id, List<SysModelMenu> menus)
        {
            if (menus == null || menus.Count == 0) return null;

            //结果集合
            List<SysModelMenu> result = new List<SysModelMenu>();

            //获取子集合
            List<SysModelMenu> children = menus.Where(m => m.parent_id == id).ToList();
            if (children != null && children.Count > 0)
            {
                foreach (SysModelMenu c in children)
                {
                    result.Add(c);
                    result.AddRange(GetItemChilds(c.id, menus));
                }
            }

            return result;
        }





    }
}