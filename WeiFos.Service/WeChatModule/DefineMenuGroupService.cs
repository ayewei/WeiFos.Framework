using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Entity.BizTypeModule;
using WeiFos.Entity.Enums;
using WeiFos.Entity.ReplyModule;
using WeiFos.Entity.ResourceModule;
using WeiFos.Entity.WeChatModule;
using WeiFos.ORM.Data;

namespace WeiFos.Service.WeChatModule
{
    /// <summary>
    /// 微信菜单分组 Service
    /// @author yewei 
    /// @date 2018-03-21
    /// </summary>
    public class DefineMenuGroupService : BaseService<DefineMenuGroup>
    {



        /// <summary>
        /// 保存自定义菜单
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="groupMenu"></param>
        /// <returns></returns>
        public StateCode SaveBuild(long user_id, DefineMenuGroup groupMenu)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {

                    s.StartTransaction();

                    //现有的菜单
                    List<DefineMenu> old_menus = s.List<DefineMenu>("where group_id = @0", groupMenu.id);
                    //临时菜单数据
                    List<DefineMenu> tmp_menus = new List<DefineMenu>();

                    if (groupMenu.id == 0)
                    {
                        groupMenu.created_date = DateTime.Now;
                        groupMenu.created_user_id = user_id;
                        s.Insert(groupMenu);
                    }
                    else
                    {
                        groupMenu.updated_date = DateTime.Now;
                        groupMenu.updated_user_id = user_id;
                        s.Update(groupMenu);
                    }

                    //获取主菜单
                    List<DefineMenu> p_menus = groupMenu.buttons.Where(m => m.tmp_parent_id == 0).ToList();
                    List<DefineMenu> c_menus = groupMenu.buttons.Where(m => m.tmp_parent_id != 0).ToList();
                    foreach (DefineMenu menu in p_menus)
                    {
                        //是否在原有的集合存在
                        DefineMenu omenu = old_menus.Where(m => m.id == menu.id).SingleOrDefault();
                        menu.parent_id = 0;
                        menu.group_id = groupMenu.id;
                        if (omenu == null)
                        {
                            s.Insert(menu);
                        }
                        else
                        {
                            tmp_menus.Add(menu);
                            s.Update(menu);
                        }

                        //子菜单
                        foreach (DefineMenu cmenu in c_menus)
                        {
                            //是否在原有的集合存在
                            DefineMenu cm = old_menus.Where(m => m.id == cmenu.id).SingleOrDefault();
                            //父级ID
                            cmenu.parent_id = menu.id;
                            //组ID
                            cmenu.group_id = groupMenu.id;

                            if (cmenu.tmp_parent_id == menu.tmp_id)
                            {
                                if (cm == null)
                                    s.Insert(cmenu);
                                else
                                {
                                    tmp_menus.Add(cmenu);
                                    s.Update(cmenu);
                                }
                            }
                        }
                    }


                    #region 删除页面已删除的菜单

                    //筛选出删除的商品规格
                    foreach (DefineMenu omenu in old_menus)
                    {
                        bool is_delete = true;
                        foreach (DefineMenu tmenu in tmp_menus)
                        {
                            if (tmenu.id == omenu.id)
                            {
                                is_delete = false;
                                break;
                            }
                        }
                        if (is_delete)
                        {
                            s.ExcuteUpdate("delete tb_wx_menu where id = @0 ", omenu.id);
                        }
                    }

                    #endregion

                    s.Commit();
                }
                catch 
                {
                    s.RollBack();
                    return StateCode.State_500;
                }
                return StateCode.State_200;
            }
        }




        /// <summary>
        /// 菜单初始化
        /// </summary>
        /// <param name="nemu_id"></param>
        /// <returns></returns>
        public DefineMenuGroup Init(long menu_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //获取当前
                DefineMenuGroup menu_group = s.Get<DefineMenuGroup>(menu_id);
                //当前菜单是否存在
                if (menu_group != null)
                {
                    List<DefineMenu> buttons = s.List<DefineMenu>("where group_id = @0 order by sort asc", menu_group.id);
                    foreach (DefineMenu menu in buttons)
                    {
                        //内容
                        DefineMenuBizContent content = new DefineMenuBizContent();
                        //如果是主菜单且存在子菜单
                        if (menu.parent_id == 0 && buttons.Exists(b => b.parent_id == menu.id)) continue;

                        if ("click".Equals(menu.type))
                        {
                            //对应关键词
                            KeyWord keyWord = s.Get<KeyWord>("where keyword = @0", menu.key_val);
                            if (keyWord == null) continue;

                            if ("ImgTextReply".Equals(keyWord.biz_type))
                            {
                                menu.biz_type = 1;
                                ImgTextReply imgTextReply = s.Get<ImgTextReply>(keyWord.biz_id);
                                if (imgTextReply != null)
                                {
                                    Img img = s.Get<Img>(" where biz_type = @0 and biz_id = @1", ImgType.ImgTextReply_Title, imgTextReply.id);
                                    content.id = imgTextReply.id;
                                    content.imgurl = img == null ? "" : img.getImgUrl();
                                    content.key_val = keyWord.keyword;
                                }
                            }
                            else
                            {
                                menu.biz_type = 2;
                                TextReply textReply = s.Get<TextReply>(keyWord.biz_id);
                                content.id = textReply.id;
                                content.key_val = keyWord.keyword;
                                content.text = keyWord.keyword;
                            }
                        }
                        else
                        {
                            content.key_val = menu.key_val;
                        }

                        menu.biz_content = content;
                    }

                    menu_group.buttons = buttons;
                }
                return menu_group;
            }
        }




        /// <summary>
        /// 设置菜单是否可用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="is_enable"></param>
        /// <returns></returns>
        public StateCode SetEnable(long id, bool is_enable)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    if (is_enable) s.ExecuteScalar("update tb_wx_menu_group set is_enable = @0 ", false);
                    s.ExecuteScalar("update tb_wx_menu_group set is_enable = @0 where id = @1 ", is_enable, id);

                    return StateCode.State_200;
                }
                catch 
                {
                    return StateCode.State_500;
                }
            }
        }







    }
}