using System.Collections.Generic;
using WeiFos.Entity.WeChatModule;
using WeiFos.ORM.Data;


namespace WeiFos.Service.WeChatModule
{
    /// <summary>
    /// 自定义菜单service
    /// </summary>
    public class DefineMenuService : BaseService<DefineMenu>
    {
        /// <summary>
        /// 获得菜单项
        /// </summary>
        /// <param name="Id">菜单ID</param>
        /// <returns></returns>
        public DefineMenu Get(long Id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<DefineMenu>(" where id = @0 ", Id);
            }
        }

        /// <summary>
        /// 获得有效菜单列表
        /// </summary>
        /// <returns></returns>
        public List<DefineMenu> GetEnableList()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<DefineMenu>(" where is_show = 1 order by sort asc", "");
            }
        }

        /// <summary>
        /// 删除指定菜单及子菜单
        /// </summary>
        /// <param name="id"></param>
        public void DeleteMenu(DefineMenu menu)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    s.Delete<DefineMenu>(menu.id);
                    if (menu.parent_id == 0)
                        s.ExcuteUpdate("delete tb_wx_menu where parent_id = @0", menu.id);
                    s.Commit();
                }
                catch
                {
                    s.RollBack();
                }
            }
        }
    }
}
