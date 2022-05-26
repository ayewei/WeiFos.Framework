using System;
using System.Collections.Generic;
using System.Linq;
using WeiFos.Entity.OrgModule;
using WeiFos.Entity.SystemModule;
using WeiFos.Core.Extensions;
using WeiFos.Entity.Enums;
using WeiFos.Cache.Base; 
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using WeiFos.Service;
using WeiFos.Service.SystemModule;
using WeiFos.Entity.BizTypeModule;
using WeiFos.Core.XmlHelper;
using WeiFos.Service.OrgModule;
using WeiFos.Service.ResourceModule;
using WeiFos.NetCore.Extensions; 

namespace WeiFos.Admin.Code
{

    /// <summary>
    /// Copyright (c) 2013-2018 深圳微狐信息科技有限公司
    /// 描 述：用户登录信息
    /// 创建人：叶委
    /// 日 期：2019.01.10
    /// </summary>
    public class LoginUser
    {

        #region 单列模式  

        /*私有构造器，不能该类外部new对象*/
        private LoginUser()
        { }

        private static LoginUser instance = null;
        public static LoginUser Instance
        {
            get { return instance = instance ?? new LoginUser(); }
        }

        #endregion


        #region 缓存key模块

        /// <summary>
        /// 登录用户token(session_id)
        /// </summary>
        private const string sys_user_token = "weifos_token";

        /// <summary>
        /// 功能用户对于员工信息key
        /// </summary>
        private const string sys_emp_key = "sys_emp_key";

        /// <summary>
        /// 用户权限key
        /// </summary>
        private static string sys_user_func_key = "weifos_sys_user_func";

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
        /// 登录写入readis缓存信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public StateCode LoginIn(SysUser user, HttpContext httpContext)
        {
            try
            {
                //当前用户角色集合
                List<UserRole> user_reles = ServiceIoc.Get<UserRoleService>().GetByUserId(user.id);
                //用户图像
                string head_img = ResXmlConfig.Instance.DefaultImgSrc(AppGlobal.Res, ImgType.SysUser);
                //用户信息
                Employee employee = ServiceIoc.Get<EmployeeService>().GetByUserId(user.id);
                if (employee != null)
                {
                    string img = ServiceIoc.Get<ImgService>().GetImgUrl(ImgType.SysUser, employee.id);
                    if (!string.IsNullOrEmpty(img)) head_img = img;
                }
                //用户图像
                user.head_img = head_img;

                //是否开启用户权限
                if (AppGlobal.Instance.EnableRedis)
                {
                    string session_id = NHttpContext.Current.Session.Id;
                    //登录用户信息
                    AppGlobal.Instance.CacheRedis.Write(sys_user_token + "_" + session_id, user, CacheId.login_info);
                    //登录用户对应员工信息
                    AppGlobal.Instance.CacheRedis.Write(sys_emp_key + "_" + session_id, employee, CacheId.login_info);
                    //当前用户登录角色
                    AppGlobal.Instance.CacheRedis.Write(sys_user_role_key + "_" + session_id, user_reles, CacheId.login_info);
                    //当前用户登录权限集合 
                    AppGlobal.Instance.CacheRedis.Write(sys_user_func_key + "_" + session_id, GetLoginUserPermissions(user.id), CacheId.login_info);
                }
                else
                {
                    //登录用户信息
                    httpContext.Session.Set(sys_user_token, user);
                    //登录用户信息
                    httpContext.Session.Set(sys_emp_key, employee);
                    //当前用户登录角色
                    httpContext.Session.Set(sys_user_role_key, user_reles);
                    //当前用户登录权限集合 
                    httpContext.Session.Set(sys_user_func_key, GetLoginUserPermissions(user.id));
                }

                #region Claim身份认证

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim("UserId", user.id.ToString()));
                claims.Add(new Claim("AccountName", user.login_name));
                claims.Add(new Claim("LoginIP", httpContext.GetClientIp()));
                //init the identity instances 
                var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
                httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(60),
                    IsPersistent = false,
                    AllowRefresh = false
                });

                #endregion

                return StateCode.State_200;
            }
            catch (Exception ex)
            {
                return StateCode.State_500;
            }
        }


        /// <summary>
        /// 当前用户所有权限
        /// </summary>
        public SysUser User
        {
            get
            {
                if (AppGlobal.Instance.EnableRedis)
                {
                    string session_id = NHttpContext.Current.Session.Id;
                    return AppGlobal.Instance.CacheRedis.Read<SysUser>(sys_user_token + "_" + session_id, CacheId.module);
                }
                else
                {
                    //登录用户信息
                    return NHttpContext.Current.Session.Get<SysUser>(sys_user_token);
                }
            }
        }

 
        /// <summary>
        /// 当前登录用户信息
        /// </summary> 
        public Employee Employee
        {
            get
            {
                if (AppGlobal.Instance.EnableRedis)
                {
                    string session_id = NHttpContext.Current.Session.Id;
                    return AppGlobal.Instance.CacheRedis.Read<Employee>(sys_emp_key + "_" + session_id, CacheId.module);
                }
                else
                {
                    //登录用户信息
                    return NHttpContext.Current.Session.Get<Employee>(sys_emp_key);
                }
            }
        }


        /// <summary>
        /// 当前用户所有权限
        /// </summary>
        public List<SysPermission> Permissions
        {
            get
            {
                if (AppGlobal.Instance.EnableRedis)
                {
                    string session_id = NHttpContext.Current.Session.Id;
                    return AppGlobal.Instance.CacheRedis.Read<List<SysPermission>>(sys_user_func_key + "_" + session_id, CacheId.login_info);
                }
                else
                {
                    //登录用户信息
                    return NHttpContext.Current.Session.Get<List<SysPermission>>(sys_user_func_key);
                }
            }
        }


        /// <summary>
        /// 验证账号权限
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool VerifyPermission(string code)
        {
            //根据权限编号 获取权限信息
            return VerifyPermission(code, true);
        }


        /// <summary>
        /// 验证账号权限
        /// </summary>
        /// <param name="p_code"></param>
        /// <param name="is_verify"></param>
        /// <returns></returns>
        public bool VerifyPermission(string p_code, bool is_verify)
        {
            //如果不校验
            if (!is_verify) return true;

            //超级管理员
            if ((bool)Instance.User.is_manager) return true;

            //权限验证
            if ((Permissions.Count == 0 || Permissions.Count == 0)) return false;

            //根据权限编号 获取权限信息 
            return Permissions.Exists(p => p.code == p_code);
        }


        /// <summary>
        /// 根据当前请求获取权限编号集合
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetArrayCodeByUrl(string url)
        {
            List<SysPermission> current_ps;
            if ((bool)Instance.User.is_manager)
            {
                current_ps = AppGlobal.Instance.Permissions.Where(p => url.Equals(p.action_url)).ToList();
            }
            else
            {
                current_ps = Instance.Permissions.Where(p => url.Equals(p.action_url)).ToList();
            }

            //权限集合
            List<SysPermission> tmp_ps = new List<SysPermission>();

            //存在权限集合
            if (current_ps != null)
            {
                foreach (SysPermission sp in current_ps)
                {
                    //List<SysPermission> tmp_p = GetChildrenPermissions(sp);
                    //if (tmp_p != null)
                    //{
                    //    tmp_ps.AddRange(tmp_p);
                    //}
                }
            }

            //获取当前权限
            return string.Join(",", tmp_ps.Select(p => p.code).ToArray());
        }


        /// <summary>
        /// 是否登录
        /// </summary>
        public bool IsLogin(HttpContext HttpContext)
        {
            return HttpContext.User.Identity.IsAuthenticated && Instance.User != null;
        }


        /// <summary>
        /// 注销用户
        /// </summary>
        public void LoginOut(HttpContext HttpContext)
        {
            //注销登录
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //清空Session信息
            HttpContext.Session.Clear();

            if (AppGlobal.Instance.EnableRedis)
            {
                try
                {
                    string session_id = NHttpContext.Current.Session.Id;

                    //登录用户信息
                    AppGlobal.Instance.CacheRedis.Remove(sys_user_token + "_" + session_id, CacheId.login_info);

                    //登录用户对应员工信息
                    AppGlobal.Instance.CacheRedis.Remove(sys_emp_key + "_" + session_id, CacheId.login_info);

                    //登录用户角色信息
                    AppGlobal.Instance.CacheRedis.Remove(sys_user_role_key + "_" + session_id, CacheId.login_info);

                    //登录用户权限信息
                    AppGlobal.Instance.CacheRedis.Remove(sys_user_func_key + "_" + session_id, CacheId.login_info);
                }
                catch (Exception) { }
            }
        }


        /// <summary>
        /// 系统所有用户角色
        /// </summary> 
        public List<UserRole> UserRoles
        {
            get
            {
                if (AppGlobal.Instance.EnableRedis)
                {
                    return AppGlobal.Instance.CacheRedis.Read<List<UserRole>>(sys_user_role_key, CacheId.module);
                }
                else
                { 
                    return NHttpContext.Current.Session.Get<List<UserRole>>(sys_user_role_key) ?? new List<UserRole>();
                }
            }
        }


        /// <summary>
        /// 注册验证码
        /// </summary>
        public string VerificationCode
        {
            get
            {
                return "";
            }
            set
            {
                string str = value;
            }
        }


        /// 根据权限ID集合获取权限集合
        /// </summary>
        /// <param name="ids"></param>
        private List<SysPermission> GetPermissions(long[] ids)
        {
            List<SysPermission> userPermissions = new List<SysPermission>();
            for (int i = 0; i < ids.Count(); i++)
            {
                //获取当前权限
                foreach (SysPermission p in Permissions)
                {
                    if (ids[i] == p.id)
                    {
                        userPermissions.Add(p);
                    }
                }
            }
            return userPermissions;
        }


        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public List<SysPermission> GetLoginUserPermissions(long user_id)
        {
            //当前用户权限
            List<SysUserPermission> uplist = ServiceIoc.Get<SysUserPermissionService>().GetByUserId(user_id);

            //当前用户角色
            List<UserRole> userRoles = UserRoles.Where(r => r.sysuser_id == user_id).ToList();

            var result = from rp in AppGlobal.Instance.RolePermissions
                         join ur in userRoles on rp.role_id equals ur.role_id into plist
                         from a in plist
                         select new
                         {
                             rp.permission_id
                         };

            //合并两个数据集合
            var result1 = result.Select(p => p.permission_id).Union(uplist.Select(up => up.permission_id)).Distinct().ToArray();

            return GetPermissions(result1);
        }




    }
}