using System;
using System.Collections.Generic;
using System.Linq;
using WeiFos.NetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using WeiFos.Admin.Code;
using WeiFos.Admin.Controllers;
using WeiFos.Entity.Enums;
using WeiFos.Entity.SystemModule;
using WeiFos.Service;
using WeiFos.Service.SystemModule;
using WeiFos.Core.Extensions;
using Newtonsoft.Json;
using WeiFos.Entity.OrgModule;
using WeiFos.Service.OrgModule;
using WeiFos.ORM.Data.Restrictions;
using WeiFos.ORM.Data.Const;
using WeiFos.ORM.Data;
using WeiFos.Service.LogsModule;
using System.Data;
using WeiFos.Entity.LogsModule;
using WeiFos.Core;
using WeiFos.Service.ResourceModule;
using WeiFos.Entity.BizTypeModule;
using WeiFos.Entity.ResourceModule;
using WeiFos.Core.XmlHelper;
using WeiFos.Admin.Code.Authorization;
using WeiFos.Core.SettingModule;

namespace WeiFos.Admin.Areas.SystemModule.Controllers
{
    /// <summary>
    /// System 控制器
    /// @author yewei 
    /// add by @date 2015-01-11
    /// </summary>
    [Area(AreaNames.SystemModule)]
    [LoginAuth]
    public class SystemController : BaseController
    {

        /// <summary>
        /// 获取当前用户权限集合
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public JsonResult GetMyPCode(string url)
        {
            return Json(LoginUser.Instance.GetArrayCodeByUrl(url));
        }


        /// <summary>
        /// 系统Icon图标管理
        /// </summary>
        /// <returns></returns>
        public ActionResult IconsManage()
        {
            return View();
        }


        #region 系统管理——系统菜单模块

        /// <summary>
        /// 系统菜单管理
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ActionResult SysMenuManage(SysUser user)
        {
            ViewBag.Title = "系统菜单管理";
            return View();
        }


        /// <summary>
        /// 菜单查询分页
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSysMenus(string name)
        {
            List<SysModelMenu> menus = ServiceIoc.Get<SysModelMenuService>().GetMenus(name);
            return Json(GetResult(StateCode.State_200, menus));
        }


        /// <summary>
        /// 初始化菜单页面
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ActionResult SysMenuForm(SysUser user, SysModelMenu entity = null)
        {
            if (Request.IsAjaxRequest())
            {
                //保存菜单
                StateCode code = ServiceIoc.Get<SysModelMenuService>().Save(user.id, entity);

                //初始化
                AppGlobal.Instance.Initial();

                //返回数据
                return Json(GetResult(code));
            }
            else
            {
                List<SysModelMenu> menuList = ServiceIoc.Get<SysModelMenuService>().GetChildrenTag(0, 0, System.Web.HttpUtility.HtmlDecode("&nbsp;&nbsp;"));
                menuList.Insert(0, new SysModelMenu() { name = "根目录", id = 0 });
                ViewBag.Parents = menuList;

                entity = ServiceIoc.Get<SysModelMenuService>().GetById(bid);
                if (entity != null)
                {
                    ViewBag.entity = JsonConvert.SerializeObject(entity);
                }
            }

            return View();
        }


        /// <summary>
        /// 设置系统菜单是否可用
        /// </summary>
        /// <param name="user"></param>
        /// <param name="menu"></param>
        /// <returns></returns>
        public JsonResult SetSysMenuEnable(bool isenable)
        {
            StateCode state = ServiceIoc.Get<SysModelMenuService>().SetEnable(bid, isenable);
            if (StateCode.State_200 == state)
            {
                AppGlobal.Instance.Initial();
            }

            return Json(GetResult(state));
        }


        #endregion


        #region 系统管理——用户模块

        /// <summary>
        /// 系统用户管理
        /// </summary>
        /// <returns></returns>
        public ActionResult SysUserManage()
        {
            return View();
        }



        /// <summary>
        /// 系统用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entity"></param>
        /// <param name="employee"></param>
        /// <param name="rIds"></param>
        /// <param name="pIds"></param>
        /// <returns></returns>
        public ActionResult SysUserForm(SysUser user, SysUser entity, Employee employee = null, string rIds = "", string pIds = "")
        {
            if (Request.IsAjaxRequest())
            {
                //保存菜单
                StateCode code = ServiceIoc.Get<SysUserService>().SaveUser(user.id, entity, employee, rIds, pIds);
                //返回数据
                return Json(GetResult(code));
            }
            else
            {
                //当前用户信息
                entity = ServiceIoc.Get<SysUserService>().GetById(bid);

                //具有的角色
                List<SysRole> hasRoles = new List<SysRole>();

                //待分配的角色
                List<SysRole> tobeRoles = new List<SysRole>();

                //所有角色
                List<SysRole> roles = ServiceIoc.Get<SysRoleService>().GetAll();

                if (entity != null)
                {
                    ViewBag.entity = JsonConvert.SerializeObject(entity);

                    //获取用户对应员工信息
                    employee = ServiceIoc.Get<EmployeeService>().GetByUserId(bid);
                    if (employee != null)
                    {
                        //员工信息
                        ViewBag.employee = JsonConvert.SerializeObject(employee);
                    }

                    //该用户对应的角色
                    List<UserRole> userRoles = ServiceIoc.Get<UserRoleService>().GetByUserId(entity.id);

                    foreach (SysRole r in roles)
                    {
                        if (userRoles.Count != 0)
                        {
                            foreach (UserRole ur in userRoles)
                            {
                                if (r.id == ur.role_id && !hasRoles.Contains(r))
                                {
                                    hasRoles.Add(r);
                                }

                                if (r.id != ur.role_id && !tobeRoles.Contains(r))
                                {
                                    tobeRoles.Add(r);
                                }
                            }
                        }
                        else
                        {
                            tobeRoles = roles;
                        }
                    }

                    List<SysUserPermission> userPermission = ServiceIoc.Get<SysUserPermissionService>().GetByUserId(bid);
                    ViewBag.pids = string.Join(",", userPermission.Select(p => p.permission_id.ToString()).ToArray());
                }
                else
                {
                    tobeRoles = roles;
                }

                ViewBag.hasRoles = hasRoles;
                ViewBag.tobeRoles = tobeRoles;
            }

            //系统所有权限
            List<SysPermission> permissions = ServiceIoc.Get<SysPermissionService>().GetList("order by order_index desc", "");
            ViewBag.permissions = permissions.Where(p => p.parent_id == 0).ToList();
            ViewBag.pchildrens = permissions.Where(p => p.parent_id != 0).ToList();

            return View();
        }



        /// <summary>
        /// 翻页查询
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="keyword"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public ContentResult GetSysUsers(int pageSize, int pageIndex, string keyword, int status)
        {
            try
            {
                //查询对象
                Criteria ct = new Criteria();

                //查询表达式
                MutilExpression me = new MutilExpression();

                ct.SetFromTables("v_sys_user")
                .SetPageSize(pageSize)
                .SetStartPage(pageIndex)
                .SetFields(new string[] { "*" })
                .AddOrderBy(new OrderBy("created_date", "desc"));

                //登录名称
                if (!string.IsNullOrEmpty(keyword))
                {
                    me.Add(new SingleExpression("", LogicOper.CUSTOM, "("));
                    me.Add(new SingleExpression("login_name", LogicOper.LIKE, " or ", keyword));
                    me.Add(new SingleExpression("name", LogicOper.LIKE, " or ", keyword));
                    me.Add(new SingleExpression("", LogicOper.CUSTOM, "", ")"));
                }

                if (status != -1)
                {
                    me.Add(new SingleExpression("status", LogicOper.EQ, status));
                }

                //设置查询条件
                if (me.Expressions.Count > 0)
                {
                    ct.SetWhereExpression(me);
                }

                DataTable dt = ServiceIoc.Get<SysUserService>().Fill(ct);

                return PageResult(StateCode.State_200, ct.TotalRow, dt);
            }
            catch (Exception ex)
            {
                return PageResult(StateCode.State_500, 0, null);
            }
        }



        /// <summary>
        /// 验证登录名称是否存在
        /// </summary>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        public int ExistLoginName(string login_name)
        {
            return ServiceIoc.Get<SysUserService>().ExistLoginName(login_name);
        }


        /// <summary>
        /// 验证邮箱是否存在
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public int ExistEmail(string email)
        {
            return ServiceIoc.Get<SysUserService>().ExistEmail(email, bid);
        }



        /// <summary>
        /// 冻结、解冻 用户
        /// </summary>
        /// <param name="sysUser"></param>
        /// <param name="userMsg"></param>
        /// <returns></returns>
        public JsonResult SetEnable(int status)
        {
            //获取状态
            StateCode state = ServiceIoc.Get<SysUserService>().SetEnable(bid, status);
            return Json(GetResult(state));
        }



        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public JsonResult ResetPsw(SysUser user)
        {
            string psw = ConfigManage.AppSettings<string>("AppSettings:DefaultPassWord");
            StateCode state = ServiceIoc.Get<SysUserService>().ResetPsw(bid, psw);
            return Json(GetResult(StateCode.State_200));
        }



        /// <summary>
        /// 保存密码
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public JsonResult SavePsw(SysUser user, string pass_word, string new_psw, string rnew_psw)
        {
            try
            {
                string psw = ServiceIoc.Get<SysUserService>().GetPassWordByID(user.id);
                //原始密码不正确
                if (!psw.Equals(StringHelper.ConvertTo32BitSHA1(pass_word)))
                {
                    return Json(GetResult(StateCode.State_4));
                }
                ServiceIoc.Get<SysUserService>().ResetPsw(user.id, StringHelper.ConvertTo32BitSHA1(new_psw));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
            return Json(GetResult(StateCode.State_200));
        }




        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entity"></param>
        /// <param name="imgmsg"></param>
        /// <returns></returns>
        public ActionResult UpdateUserForm(SysUser user, Employee entity = null, string imgmsg = null)
        {
            if (Request.IsAjaxRequest())
            {
                //保存菜单
                StateCode code = ServiceIoc.Get<EmployeeService>().Save(user.id, entity, imgmsg);
                //返回数据
                return Json(GetResult(code));
            }
            else
            {
                //缺省图片路径
                ViewBag.defurl = ResXmlConfig.Instance.DefaultImgSrc(AppGlobal.Res, ImgType.SysUser);
                //用户图片路径
                ViewBag.imgurl = ViewBag.defurl;
                //当前用户加密ID
                ViewBag.Ticket = StringHelper.GetEncryption(ImgType.SysUser + "#" + bid);

                //当前用户信息
                entity = ServiceIoc.Get<EmployeeService>().GetByUserId(LoginUser.Instance.User.id);
                if (entity != null)
                {
                    //当前用户实体
                    ViewBag.entity = JsonConvert.SerializeObject(entity);
                    Img img = ServiceIoc.Get<ImgService>().GetImg(ImgType.SysUser, entity.id);
                    if (img != null)
                    {
                        ViewBag.imgurl = string.IsNullOrEmpty(img.getImgUrl()) ? ViewBag.defurl : img.getImgUrl();
                    }
                }
            }

            return View();
        }



        #endregion


        #region 系统管理——角色模块

        /// <summary>
        /// 角色管理
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ActionResult SysRoleManage(SysUser user)
        {
            return View();
        }



        /// <summary>
        /// 角色页面
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entity"></param>
        /// <param name="pIds"></param>
        /// <returns></returns>
        public ActionResult SysRoleForm(SysUser user, SysRole entity = null, string pIds = "")
        {

            if (Request.IsAjaxRequest())
            {
                StateCode state = ServiceIoc.Get<SysRoleService>().Save(user.id, entity, pIds);
                //返回数据
                return Json(GetResult(state));
            }
            else
            {
                entity = ServiceIoc.Get<SysRoleService>().GetById(bid);
                if (entity != null)
                {
                    ViewBag.entity = JsonConvert.SerializeObject(entity);
                    List<SysRolePermission> rolePermissions = ServiceIoc.Get<SysRolePermissionService>().GetPermissionsByRoleId(bid);
                    ViewBag.pids = string.Join(",", rolePermissions.Select(p => p.permission_id.ToString()).ToArray());
                }
            }

            //系统所有权限
            List<SysPermission> permissions = ServiceIoc.Get<SysPermissionService>().GetList("order by order_index desc", "");

            ViewBag.permissions = permissions.Where(p => p.parent_id == 0).ToList();

            ViewBag.childrens = permissions.Where(p => p.parent_id != 0).ToList();

            return View();
        }



        /// <summary>
        /// 角色名称是否存在
        /// </summary>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        public int ExistRoleName(string name)
        {
            return ServiceIoc.Get<SysRoleService>().ExistRoleName(bid, name);
        }




        /// <summary>
        /// 获取角色翻页
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public ContentResult GetSysRoles(int pageSize, int pageIndex, string name, int status)
        {
            try
            {
                //查询对象
                Criteria ct = new Criteria();

                //查询表达式
                MutilExpression me = new MutilExpression();

                ct.SetFromTables("tb_sys_role ")
                .SetPageSize(pageSize)
                .SetStartPage(pageIndex)
                .SetFields(new string[] { "*" })
                .AddOrderBy(new OrderBy("id", "desc"));

                //登录名称
                if (!string.IsNullOrEmpty(name))
                {
                    me.Add(new SingleExpression("name", LogicOper.LIKE, name));
                }

                if (status != -1)
                {
                    me.Add(new SingleExpression("status", LogicOper.EQ, status));
                }

                //设置查询条件
                if (me.Expressions.Count > 0)
                {
                    ct.SetWhereExpression(me);
                }

                DataTable dt = ServiceIoc.Get<SysUserService>().Fill(ct);

                return PageResult(StateCode.State_200, ct.TotalRow, dt);
            }
            catch (Exception ex)
            {
                return PageResult(StateCode.State_500, 0, null);
            }
        }



        /// <summary>
        /// 设置角色是否可用
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public JsonResult SetEnableSysRole(bool status)
        {
            //获取状态
            StateCode state = ServiceIoc.Get<SysRoleService>().SetEnable(bid, status);
            return Json(GetResult(state));
        }

        #endregion


        #region 系统管理——权限模块


        /// <summary>
        /// 权限管理页面
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ActionResult SysPermissionManage(SysUser user)
        {
            return View();
        }



        /// <summary>
        /// 获取权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSysPermissions()
        {
            try
            {
                List<SysPermission> permissions = ServiceIoc.Get<SysPermissionService>().GetList("order by order_index desc", "");

                List<SysPermission> parents = permissions.Where(p => p.parent_id == 0).ToList();

                List<SysPermission> childrens = permissions.Where(p => p.parent_id != 0).ToList();

                var data = new { parents, childrens };

                return Json(GetResult(StateCode.State_200, data));
            }
            catch (Exception ex)
            {
                return Json(GetResult(StateCode.State_500));
            }
        }




        /// <summary>
        /// 权限页面
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entity"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public ActionResult SysPermissionForm(SysUser user, SysPermission entity = null)
        {
            //所属分类数据
            List<SysPermission> modulePermissions = new List<SysPermission>();
            modulePermissions.Add(new SysPermission() { name = "根", id = 0 });
            modulePermissions.AddRange(GetModuleSysPermission());
            ViewBag.module = modulePermissions;

            if (Request.IsAjaxRequest())
            {
                StateCode state = ServiceIoc.Get<SysPermissionService>().Save(user.id, entity);
                AppGlobal.Instance.Initial();
                return Json(GetResult(state));
            }
            else
            {
                entity = ServiceIoc.Get<SysPermissionService>().GetById(bid);
                if (entity != null)
                {
                    ViewBag.entity = JsonConvert.SerializeObject(entity);
                }
            }

            return View();
        }




        /// <summary>
        /// 验证用户编号是否存在
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public int ExistPermissionCode(string code)
        {
            return ServiceIoc.Get<SysPermissionService>().ExistByCode(bid, code);
        }



        /// <summary>
        /// 删除权限
        /// </summary>
        /// <returns></returns>
        public JsonResult DeletePermission()
        {
            try
            {
                ServiceIoc.Get<SysPermissionService>().Delete(bid);
                return Json(GetResult(StateCode.State_200));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }


        #endregion


        #region 系统管理——数据字典模块

        /// <summary>
        /// 权限管理页面
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ActionResult ConfigParamManage(SysUser user)
        {
            return View();
        }



        /// <summary>
        /// 获取系统参数配置
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="status"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public JsonResult GetConfigParams(int pageSize, int pageIndex, int status, string keyword)
        {
            //查询对象
            Criteria ct = new Criteria();

            //查询表达式
            MutilExpression me = new MutilExpression();

            ct.SetPageSize(pageSize)
            .SetStartPage(pageIndex)
            .SetFields(new string[] { "*" })
            .AddOrderBy(new OrderBy("id", "desc"));

            //查询关键词
            if (!string.IsNullOrEmpty(keyword))
            {
                me.Add(new SingleExpression("key", LogicOper.LIKE, keyword));
            }

            //栏目类型
            if (status != -1)
            {
                me.Add(new SingleExpression("is_enable", LogicOper.EQ, status == 1));
            }

            //条件
            if (me.Expressions.Count > 0)
            {
                ct.SetWhereExpression(me);
            }

            List<ConfigParam> list = ServiceIoc.Get<ConfigParamService>().GetList(ct);

            return PageResult(StateCode.State_200, ct.TotalRow, list);
        }


        /// <summary>
        /// 参数配置key是否存在
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int ExistConfigParamKey(SysUser user, string config_key)
        {
            return ServiceIoc.Get<ConfigParamService>().ExistConfigParamKey(config_key, bid);
        }


        /// <summary>
        /// 初始化参数配置
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ActionResult ConfigParamForm(SysUser user, ConfigParam entity = null)
        {
            if (Request.IsAjaxRequest())
            {
                //保存菜单
                StateCode code = ServiceIoc.Get<ConfigParamService>().Save(user.id, entity);

                //初始化
                AppGlobal.Instance.Initial();

                //返回数据
                return Json(GetResult(code));
            }
            else
            {
                entity = ServiceIoc.Get<ConfigParamService>().GetById(bid);
                if (entity != null)
                {
                    ViewBag.key = entity.config_key;
                    ViewBag.entity = JsonConvert.SerializeObject(entity);
                }
            }
            return View();
        }


        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="user"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public JsonResult SaveConfigParamForm(SysUser user, ConfigParam param)
        {
            try
            {
                if (param.id == 0)
                {
                    param.created_user_id = user.id;
                    param.created_date = DateTime.Now;
                    ServiceIoc.Get<ConfigParamService>().Insert(param);
                }
                else
                {
                    param.updated_user_id = user.id;
                    param.updated_date = DateTime.Now;
                    ServiceIoc.Get<ConfigParamService>().Update(param);
                }
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
            return Json(GetResult(StateCode.State_200));
        }


        /// <summary>
        /// 设置参数是否可用
        /// </summary>
        /// <param name="user"></param>
        /// <param name="isenable"></param>
        /// <returns></returns>
        public JsonResult SetConfigParamEnable(SysUser user, bool isenable)
        {
            StateCode state = ServiceIoc.Get<ConfigParamService>().SetEnable(bid, isenable);
            return Json(GetResult(state));
        }


        #endregion


        #region 系统管理——日志模块


        /// <summary>
        /// 后台系统日志 
        /// </summary>
        /// <returns></returns>
        public ActionResult LogSystemManage()
        {
            return View();
        }


        /// <summary>
        /// 接口日志 
        /// </summary>
        /// <returns></returns>
        public ActionResult LogInterfaceManage()
        {
            return View();
        }


        /// <summary>
        /// 登录日志 
        /// </summary>
        /// <returns></returns>
        public ActionResult LogLoginManage()
        {
            return View();
        }



        /// <summary>
        /// 接口日志
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="type"></param>
        /// <param name="createDate"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public JsonResult GetInterfaceLogs(int pageSize, int pageIndex, int type, string createDate, string keyword)
        {
            //查询对象
            Criteria ct = new Criteria();

            //查询表达式
            MutilExpression me = new MutilExpression();

            ct.SetPageSize(pageSize)
            .SetStartPage(pageIndex)
            .SetFields(new string[] { "*" })
            .AddOrderBy(new OrderBy("id", "desc"));

            //发布日期
            if (!string.IsNullOrEmpty(createDate))
            {
                DateTime startDate = Convert.ToDateTime(createDate.Split('-')[0]);
                DateTime endDate = Convert.ToDateTime(createDate.Split('-')[1]);

                if (startDate.CompareTo(endDate) == 0)
                {
                    me.Add(new SingleExpression("created_date", LogicOper.BETWEEN, new[] { startDate.ToString(), endDate.AddDays(1).ToString() }));
                }
                else
                {
                    me.Add(new SingleExpression("created_date", LogicOper.BETWEEN, new[] { startDate.ToString(), endDate.AddDays(1).ToString() }));
                }
            }

            //查询关键词
            if (!string.IsNullOrEmpty(keyword))
            {
                me.Add(new SingleExpression("content", LogicOper.LIKE, keyword));
            }

            //栏目类型
            if (type != -1)
            {
                me.Add(new SingleExpression("type", LogicOper.EQ, type));
            }

            //条件
            if (me.Expressions.Count > 0)
            {
                ct.SetWhereExpression(me);
            }

            List<APILogs> logs = ServiceIoc.Get<APILogsService>().GetList(ct);

            return PageResult(StateCode.State_200, ct.TotalRow, logs);
        }




        /// <summary>
        /// 操作日志接口日志
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="type"></param>
        /// <param name="createDate"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ContentResult GetSystemLogs(int pageSize, int pageIndex, int type, string createDate, string keyword)
        {
            //查询对象
            Criteria ct = new Criteria();

            //查询表达式
            MutilExpression me = new MutilExpression();

            ct.SetPageSize(pageSize)
             .SetFromTables("v_logs_sys_user_op")
            .SetStartPage(pageIndex)
            .SetFields(new string[] { "*" })
            .AddOrderBy(new OrderBy("id", "desc"));

            //发布日期
            if (!string.IsNullOrEmpty(createDate))
            {
                DateTime startDate = Convert.ToDateTime(createDate.Split('-')[0]);
                DateTime endDate = Convert.ToDateTime(createDate.Split('-')[1]);

                if (startDate.CompareTo(endDate) == 0)
                {
                    me.Add(new SingleExpression("created_date", LogicOper.BETWEEN, new[] { startDate.ToString(), endDate.AddDays(1).ToString() }));
                }
                else
                {
                    me.Add(new SingleExpression("created_date", LogicOper.BETWEEN, new[] { startDate.ToString(), endDate.AddDays(1).ToString() }));
                }
            }

            //查询关键词
            if (!string.IsNullOrEmpty(keyword))
            {
                me.Add(new SingleExpression("content", LogicOper.LIKE, keyword));
            }

            //栏目类型
            if (type != -1)
            {
                me.Add(new SingleExpression("type", LogicOper.EQ, type));
            }

            //条件
            if (me.Expressions.Count > 0)
            {
                ct.SetWhereExpression(me);
            }

            DataTable dt = ServiceIoc.Get<SystemLogsService>().Fill(ct);
            return PageResult(StateCode.State_200, ct.TotalRow, dt);
        }



        /// <summary>
        /// 操作日志接口日志
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="type"></param>
        /// <param name="createDate"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ContentResult GetLoginLogs(int pageSize, int pageIndex, string createDate, string keyword)
        {
            //查询对象
            Criteria ct = new Criteria();

            //查询表达式
            MutilExpression me = new MutilExpression();

            ct.SetPageSize(pageSize)
             .SetFromTables("v_logs_sys_login")
            .SetStartPage(pageIndex)
            .SetFields(new string[] { "*" })
            .AddOrderBy(new OrderBy("id", "desc"));

            //发布日期
            if (!string.IsNullOrEmpty(createDate))
            {
                DateTime startDate = Convert.ToDateTime(createDate.Split('-')[0]);
                DateTime endDate = Convert.ToDateTime(createDate.Split('-')[1]);

                if (startDate.CompareTo(endDate) == 0)
                {
                    me.Add(new SingleExpression("login_time", LogicOper.BETWEEN, new[] { startDate.ToString(), endDate.AddDays(1).ToString() }));
                }
                else
                {
                    me.Add(new SingleExpression("login_time", LogicOper.BETWEEN, new[] { startDate.ToString(), endDate.AddDays(1).ToString() }));
                }
            }

            //查询关键词
            if (!string.IsNullOrEmpty(keyword))
            {
                me.Add(new SingleExpression("login_name", LogicOper.LIKE, keyword));
            }

            //条件
            if (me.Expressions.Count > 0)
            {
                ct.SetWhereExpression(me);
            }

            DataTable dt = ServiceIoc.Get<SystemLogsService>().Fill(ct);
            return PageResult(StateCode.State_200, ct.TotalRow, dt);
        }



        /// <summary>
        /// 清空接口日志
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public JsonResult ClearInterfaceLogs(SysUser user)
        {
            StateCode code = ServiceIoc.Get<APILogsService>().Clear();
            return Json(GetResult(code));
        }



        /// <summary>
        /// 清空接口日志
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public JsonResult ClearSystemLogs(SysUser user)
        {
            StateCode code = ServiceIoc.Get<SystemLogsService>().Clear();
            return Json(GetResult(code));
        }



        #endregion


        /// <summary>
        /// 获取权限模块
        /// </summary>
        /// <param name="parent_id"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private List<SysPermission> GetModuleSysPermission()
        {
            List<SysPermission> sysPermissions = new List<SysPermission>();

            List<SysPermission> modules = ServiceIoc.Get<SysPermissionService>().GetChildren(0);

            string tag = System.Web.HttpUtility.HtmlDecode("&nbsp;&nbsp;") + "|--";

            foreach (SysPermission m in modules)
            {
                m.name = tag + m.name;
                sysPermissions.Add(m);

                List<SysPermission> functions = ServiceIoc.Get<SysPermissionService>().GetChildren(m.id);
                foreach (SysPermission f in functions)
                {
                    f.name = System.Web.HttpUtility.HtmlDecode("&nbsp;&nbsp;") + tag + f.name;
                    sysPermissions.Add(f);
                }
            }

            return sysPermissions;
        }





    }
}