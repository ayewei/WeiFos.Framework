using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WeiFos.Cache;
using WeiFos.Cache.Base;
using WeiFos.NetCore.Extensions;
using WeiFos.Cache.Session;
using WeiFos.Core.Extensions;
using WeiFos.Core.SettingModule;
using WeiFos.Entity.SystemModule;
using WeiFos.Service;
using WeiFos.Service.SystemModule;

namespace WeiFos.Admin
{
    /// <summary>
    /// Copyright (c) 2013-2018 深圳微狐信息科技有限公司
    /// 描 述：系统全局配置类型 
    /// 创建人：叶委
    /// 日 期：2019.01.10
    /// </summary>
    public class AppGlobal
    {

        #region 单列模式  

        /*私有构造器，不能该类外部new对象*/
        private AppGlobal() { Initial(); }

        private static AppGlobal instance = null;
        public static AppGlobal Instance
        {
            get { return instance = instance ?? new AppGlobal(); }
        }

        #endregion

        #region 缓存操作

        /// <summary>
        /// 是否开启缓存
        /// </summary>
        private bool enableRedis;
        public bool EnableRedis { get { return enableRedis; } }

        /// <summary>
        /// 缓存操作
        /// </summary>
        private ICacheSession cacheRedis = null;
        public ICacheSession CacheRedis
        {
            get
            {
                return cacheRedis;
            }
        }

        #endregion

        #region 基础参数配置

        /// <summary>
        /// 资源域名
        /// </summary>
        public static string Res = string.Empty;

        /// <summary>
        /// 系统后台域名
        /// </summary>
        public static string Admin = string.Empty;

        /// <summary>
        /// 系统名称
        /// </summary>
        public static string SysName = string.Empty;

        /// <summary>
        /// 脚本版本版本号
        /// </summary>
        public static string VNo = string.Empty;

        #endregion

        #region 缓存key模块

        /// <summary>
        /// 登录用户token(session_id)
        /// </summary>
        private readonly string sys_user_token = "weifos_token";

        /// <summary>
        /// 功能用户对于员工信息key
        /// </summary>
        private static string sys_emp_key = "sys_emp_key";

        /// <summary>
        /// 功能菜单key
        /// </summary>
        private static string sys_menu_key = "weifos_sys_menu";

        /// <summary>
        /// 功能权限key
        /// </summary>
        private static string sys_func_key = "weifos_sys_func";

        /// <summary>
        /// 系统角色key
        /// </summary>
        private static string sys_role_key = "weifos_sys_role";


        /// <summary>
        /// 用户权限key
        /// </summary>
        public static string sys_user_func_key = "weifos_sys_user_func";

        /// <summary>
        /// 用户角色权限key
        /// </summary>
        private static string sys_role_func_key = "weifos_sys_role_func";

        /// <summary>
        /// 用户角色key
        /// </summary>
        private static string sys_user_role_key = "weifos_sys_user_role";

        /// <summary>
        /// 图形验证码
        /// </summary>
        private const string sys_v_code_key = "sys_v_code";

        /// <summary>
        /// 短信丢失时间
        /// </summary>
        private const string sms_lose_time_key = "sms_lose_time";



        #endregion


        /// <summary>
        /// 初始化数据
        /// </summary>
        public void Initial()
        {
            //读取
            enableRedis = ConfigManage.AppSettings<bool>("RedisConfig:EnableRedis");
            if (enableRedis) cacheRedis = CacheSessionFactory.Instance.CreateCache();

            //版本号
            VNo = ConfigManage.AppSettings<string>("AppSettings:VNo");
            //系统名称
            SysName = ConfigManage.AppSettings<string>("AppSettings:SysName");
            //资源域名
            Res = ConfigManage.AppSettings<string>("AppSettings:DomainRes");
            //后台域名
            Admin = ConfigManage.AppSettings<string>("AppSettings:DomainAdmin");

            //获取所有菜单
            List<SysModelMenu> db_menus = ServiceIoc.Get<SysModelMenuService>().Where(m => m.is_enable == true).OrderByDescending(a => a.order_index).ToList();
            //获取所有权限
            List<SysPermission> db_permissions = ServiceIoc.Get<SysPermissionService>().GetAll().OrderByDescending(a => a.order_index).ToList();
            //角色集合
            List<SysRole> db_reles = ServiceIoc.Get<SysRoleService>().Where(r => r.is_enable == true).ToList();
            //用户角色权限
            List<UserRole> db_user_reles = ServiceIoc.Get<UserRoleService>().GetAll();
            //所有用户权限
            //List<SysUserPermission> db_user_permissions = ServiceIoc.Get<SysUserPermissionService>().GetAll();
            //用户角色权限
            List<SysRolePermission> db_role_permissions = ServiceIoc.Get<SysRolePermissionService>().GetAll();

            //是否开启Redis缓存
            if (EnableRedis)
            {
                //系统菜单
                cacheRedis.Write(sys_menu_key, db_menus, CacheId.module);
                //系统权限
                cacheRedis.Write(sys_func_key, db_permissions, CacheId.module);
                //系统角色
                cacheRedis.Write(sys_role_key, db_reles, CacheId.module);
                //系统角色权限
                cacheRedis.Write(sys_role_func_key, db_role_permissions, CacheId.module);
            }
            else
            {
                //系统菜单
                menus = db_menus;
                //系统权限
                permissions = db_permissions;
                //系统角色
                roles = db_reles;
                //系统角色权限
                rolePermissions = db_role_permissions;
            }
        }


        /// <summary>
        /// 系统菜单
        /// </summary> 
        private List<SysModelMenu> menus;
        public List<SysModelMenu> Menus
        {
            get
            {
                if (EnableRedis)
                {
                    return cacheRedis.Read<List<SysModelMenu>>(sys_menu_key, CacheId.module);
                }
                else
                {
                    return menus;
                }
            }
        }


        /// <summary>
        /// 系统所有角色
        /// </summary> 
        private List<SysRole> roles;
        public List<SysRole> Roles
        {
            get
            {
                if (EnableRedis)
                {
                    return cacheRedis.Read<List<SysRole>>(sys_role_key, CacheId.module);
                }
                else
                {
                    return roles;
                }
            }
        }


        /// <summary>
        /// 系统所有角色权限
        /// </summary> 
        private List<SysRolePermission> rolePermissions;
        public List<SysRolePermission> RolePermissions
        {
            get
            {
                if (EnableRedis)
                {
                    return cacheRedis.Read<List<SysRolePermission>>(sys_role_func_key, CacheId.module);
                }
                else
                {
                    return NHttpContext.Current.Session.Get<List<SysRolePermission>>(sys_role_func_key) ?? new List<SysRolePermission>();
                }
            }
        }


        /// <summary>
        /// 系统所有权限
        /// </summary> 
        private List<SysPermission> permissions;
        public List<SysPermission> Permissions
        {
            get
            {
                if (EnableRedis)
                {
                    return cacheRedis.Read<List<SysPermission>>(sys_func_key, CacheId.module);
                }
                else
                {
                    return permissions;
                }
            }
        }



        /// <summary>
        /// 是否存在子菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ExistChildrenMenus(long id)
        {
            return Menus.Exists(m => m.parent_id == id);
        }


    }
}