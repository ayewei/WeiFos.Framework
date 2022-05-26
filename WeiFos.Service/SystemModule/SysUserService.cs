using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Core;
using WeiFos.Core.EnumHelper;
using WeiFos.Core.SettingModule;
using WeiFos.Entity.Common;
using WeiFos.Entity.Enums;
using WeiFos.Entity.OrgModule;
using WeiFos.Entity.ResourceModule;
using WeiFos.Entity.SystemModule;
using WeiFos.ORM.Data;

namespace WeiFos.Service.SystemModule
{

    /// <summary>
    /// 系统用户Service
    /// @author yewei 
    /// @date 2015-01-09
    /// </summary>
    public class SysUserService : BaseService<SysUser>
    {


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="login_name"></param>
        /// <param name="psw"></param>
        /// <returns></returns>
        public SysUser Login(string login_name, string psw, string ip)
        {
            //登录用户信息
            SysUser user = new SysUser();
            user.login_code = StateCode.State_500;

            //登录日志
            LogSysLogin login_log = new LogSysLogin();
            login_log.login_time = DateTime.Now;
            login_log.login_ip = ip;

            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {

                    #region 用户或密码错误

                    if (string.IsNullOrEmpty(login_name) || string.IsNullOrEmpty(psw))
                    {
                        user.login_code = StateCode.State_201;
                        login_log.is_success = false;
                        login_log.result = EnumHelper.GetEnumDescByValue(typeof(StateCode), user.login_code);
                        //写入登录日志
                        s.Insert(login_log);
                        //返回登录信息
                        return user;
                    }

                    #endregion

                    s.StartTransaction();

                    //查询用户
                    SysUser sys_user = s.Get<SysUser>("where login_name = @0", login_name);
                    if (sys_user == null)
                    {
                        user.login_code = StateCode.State_201;
                        login_log.is_success = false;
                        login_log.result = EnumHelper.GetEnumDescByValue(typeof(StateCode), user.login_code);
                        //写入登录日志
                        s.Insert(login_log);
                        s.Commit();
                        //返回登录信息
                        return user;
                    }

                    //用户或密码错误
                    if (!StringHelper.ConvertTo32BitSHA1(psw).Equals(sys_user.pass_word))
                    {
                        user.login_code = StateCode.State_201;
                        login_log.is_success = false;
                        login_log.result = EnumHelper.GetEnumDescByValue(typeof(StateCode), user.login_code);
                        //写入登录日志
                        s.Insert(login_log);
                        s.Commit();
                        //返回登录信息
                        return user;
                    }

                    //用户已被冻结
                    if (sys_user.status == 0)
                    {
                        user.login_code = StateCode.State_209;
                        login_log.is_success = false;
                        login_log.result = EnumHelper.GetEnumDescByValue(typeof(StateCode), user.login_code);
                        //写入登录日志
                        s.Insert(login_log);
                        s.Commit();
                        //返回登录信息
                        return user;
                    }

                    sys_user.login_code = StateCode.State_200;
                    login_log.is_success = false;
                    login_log.result = EnumHelper.GetEnumDescByValue(typeof(StateCode), sys_user.login_code);
                    //写入登录日志
                    s.Insert(login_log);

                    //更新登录状态
                    string sql = "update tb_sys_user set login_time = @0,login_ip = @1, login_count += 1 where id = @2";
                    s.ExcuteUpdate(sql, DateTime.Now, ip, sys_user.id);
                    sys_user.login_code = StateCode.State_200;
                    sys_user.pass_word = null;

                    s.Commit();

                    sys_user.pass_word = string.Empty;
                    return sys_user;
                }
                catch 
                {
                    s.RollBack();
                    return user;
                }
            }
        }



        /// <summary>
        /// 根据密码
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string GetPassWordByID(long userID)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.ExecuteScalar("select pass_word from tb_sys_user where id = @0", userID).ToString();
            }
        }


        /// <summary>
        /// 根据登录名称查找是否存在数据
        /// </summary>
        /// <param name="loginname"></param>
        /// <returns></returns>
        public int ExistLoginName(string loginname)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Exist<SysUser>("where login_name = @0", loginname);
            }
        }

        public async Task<int> ExistLoginNameAsync(string loginname)
        {
            return await Task.Run(() =>
            {
                using (ISession s = SessionFactory.Instance.CreateSession())
                {
                    return s.Exist<SysUser>("where login_name = @0", loginname);
                }
            });
        }

        /// <summary>
        /// 根据邮箱查找是否存在数据
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public int ExistEmail(string email, long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Exist<SysUser>("where email = @0 and id != @1", email, id);
            }
        }


        /// <summary>
        /// 根据邮箱查找是否存在数据
        /// </summary>
        /// <param name="user_no"></param>
        /// <returns></returns>
        public int ExistUserNo(string user_no, long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Exist<SysUser>("where user_no = @0 and id != @1", user_no, id);
            }
        }



        /// <summary>
        /// 根据登录用户名获取
        /// </summary>
        /// <param name="loginname"></param>
        /// <returns></returns>
        public SysUser GetByLoginName(string loginname)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<SysUser>("where login_name = @0", loginname);
            }
        }


        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isenable"></param>
        public StateCode SetEnable(long id, int status)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.ExcuteUpdate("update tb_sys_user set status = @0 where id = @1", status, id);
                    return StateCode.State_200;
                }
                catch
                {
                    return StateCode.State_500;
                }
            }
        }


        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="psw"></param>
        public StateCode ResetPsw(long id, string psw)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.ExcuteUpdate("update tb_sys_user set pass_word = @0 where id = @1", psw, id);
                    return StateCode.State_200;
                }
                catch 
                {
                    return StateCode.State_200;
                }
            }
        }



        /// <summary>
        /// 保存系统用户
        /// </summary>
        /// <param name="sysUser"></param>
        /// <param name="created_user_id"></param>
        /// <param name="rIds"></param>
        /// <param name="pIds"></param>
        /// <returns></returns>
        public StateCode SaveUser(SysUser sysUser, long created_user_id, string rIds, string pIds)
        {
            //角色ID
            long[] roleIds = StringHelper.StringToLongArray(rIds);
            //权限ID
            long[] permissionIds = StringHelper.StringToLongArray(pIds);

            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                s.StartTransaction();
                try
                {
                    if (sysUser.id == 0)
                    {
                        //是否是管理员
                        sysUser.is_manager = false;
                        //登录次数
                        sysUser.login_count = 0;
                        //创建用户ID
                        sysUser.created_user_id = created_user_id;
                        //创建时间
                        sysUser.created_date = DateTime.Now;
                        //密码
                        sysUser.pass_word = StringHelper.ConvertTo32BitSHA1(ConfigManage.AppSettings<string>("AppSettings:DefaultPassWord"));
                        s.Insert(sysUser);
                    }
                    else
                    {
                        //修改用户ID
                        sysUser.updated_user_id = created_user_id;
                        //修改时间
                        sysUser.updated_date = DateTime.Now;
                        s.Update(sysUser);
                    }

                    //用户角色处理
                    s.ExcuteUpdate("delete tb_sys_user_role where sysuser_id = @0", sysUser.id);
                    if (roleIds != null && roleIds.Count() > 0)
                    {
                        for (int i = 0; i < roleIds.Length; i++)
                        {
                            UserRole ur = new UserRole();
                            ur.sysuser_id = sysUser.id;
                            ur.role_id = roleIds[i];
                            s.Insert<UserRole>(ur);
                        }
                    }
                    //用户权限处理
                    s.ExcuteUpdate("delete tb_sys_user_permission where sysuser_id=@0", sysUser.id);
                    if (permissionIds != null && permissionIds.Count() > 0)
                    {
                        for (int i = 0; i < permissionIds.Length; i++)
                        {
                            SysUserPermission up = new SysUserPermission();
                            up.sysuser_id = sysUser.id;
                            up.permission_id = permissionIds[i];
                            s.Insert(up);
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
        /// 用户更新个人信息
        /// </summary>
        /// <param name="sysUser"></param>
        /// <param name="userMsg"></param>
        /// <returns></returns>
        public StateCode UpdateMyMsg(SysUser sysUser, string imgmsg)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    //获取用户最新信息
                    SysUser current = s.Get<SysUser>(sysUser.id);
                    sysUser.is_manager = false;

                    //修改用户基本信息
                    s.Update<SysUser>(sysUser);

                    //判断是否存在图片信息
                    if (!string.IsNullOrEmpty(imgmsg) && imgmsg.IndexOf("#") != -1)
                    {
                        //图片名称
                        string filename = imgmsg.Split('#')[0];
                        //图片类型
                        string biztype = imgmsg.Split('#')[1];
                        //去除重复图片
                        s.ExcuteUpdate("update tb_img set biz_id = 0 where biz_type = @0 and biz_id = @1  ", biztype, sysUser.id);
                        Img img = s.Get<Img>(" where file_name = @0 and biz_type = @1 ", filename, biztype);
                        if (img != null)
                        {
                            img.biz_id = sysUser.id;
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
        /// 保存系统用户
        /// </summary>
        /// <param name="created_user_id"></param>
        /// <param name="entity"></param>
        /// <param name="employee"></param>
        /// <param name="rIds"></param>
        /// <param name="pIds"></param>
        /// <returns></returns>
        public StateCode SaveUser(long created_user_id, SysUser entity, Employee employee, string rIds, string pIds)
        {
            //角色ID
            long[] roleIds = StringHelper.StringToLongArray(rIds);
            //权限ID
            long[] permissionIds = StringHelper.StringToLongArray(pIds);

            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                s.StartTransaction();
                try
                {
                    if (entity.id == 0)
                    {
                        //是否是管理员
                        entity.is_manager = false;
                        //登录次数
                        entity.login_count = 0;
                        //创建用户ID
                        entity.created_user_id = created_user_id;
                        //创建时间
                        entity.created_date = DateTime.Now;
                        //密码
                        entity.pass_word = StringHelper.ConvertTo32BitSHA1(ConfigManage.AppSettings<string>("AppSettings:DefaultPassWord"));
                        //插入用户数据
                        s.Insert(entity);

                        //对应系统用户
                        employee.sys_user_id = entity.id;
                        //插入用户对应员工数据
                        s.Insert(employee);
                    }
                    else
                    {
                        //修改用户实体
                        s.Update(entity);
                        int exist = s.Exist<Employee>("where sys_user_id = @0", entity.id);
                        if (exist == 0) s.Insert(employee);
                        else s.Update(employee);
                    }

                    //用户角色处理
                    s.ExcuteUpdate("delete tb_sys_user_role where sysuser_id = @0", entity.id);
                    if (roleIds != null && roleIds.Count() > 0)
                    {
                        for (int i = 0; i < roleIds.Length; i++)
                        {
                            UserRole ur = new UserRole();
                            ur.sysuser_id = entity.id;
                            ur.role_id = roleIds[i];
                            s.Insert<UserRole>(ur);
                        }
                    }

                    //用户权限处理
                    s.ExcuteUpdate("delete tb_sys_user_permission where sysuser_id=@0", entity.id);
                    if (permissionIds != null && permissionIds.Count() > 0)
                    {
                        for (int i = 0; i < permissionIds.Length; i++)
                        {
                            SysUserPermission up = new SysUserPermission();
                            up.sysuser_id = entity.id;
                            up.permission_id = permissionIds[i];
                            s.Insert(up);
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







    }
}