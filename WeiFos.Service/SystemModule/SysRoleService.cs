using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WeiFos.Entity.SystemModule;
using WeiFos.ORM.Data;
using WeiFos.ORM.Data.Attributes;
using WeiFos.Entity.Common;
using WeiFos.Entity.Enums;
using WeiFos.Core;
using WeiFos.Entity.OrgModule;

namespace WeiFos.Service.SystemModule
{
    /// <summary>
    /// 系统角色 Service
    /// @author yewei 
    /// @date 2015-01-09
    /// </summary>
    public class SysRoleService : BaseService<SysRole>
    {


        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="entity"></param>
        /// <param name="pIds"></param>
        /// <returns></returns>
        public StateCode Save(long user_id, SysRole entity, string pIds)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    long[] permissionIds = StringHelper.StringToLongArray(pIds);

                    s.StartTransaction();

                    if (entity.id == 0)
                    {
                        entity.created_date = DateTime.Now;
                        entity.created_user_id = user_id;
                        s.Insert<SysRole>(entity);
                    }
                    else
                    {
                        entity.updated_date = DateTime.Now;
                        entity.updated_user_id = user_id;
                        s.Update<SysRole>(entity);
                    }

                    //角色权限处理
                    s.ExcuteUpdate("delete tb_sys_role_permission where role_id = @0", entity.id);
                    for (int i = 0; i < permissionIds.Length; i++)
                    {
                        int exist = s.Exist<SysPermission>("where id = @0", permissionIds[i]);
                        if (exist > 0)
                        {
                            SysRolePermission rp = new SysRolePermission();
                            rp.role_id = entity.id;
                            rp.permission_id = permissionIds[i];
                            s.Insert<SysRolePermission>(rp);
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
        /// 角色名是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rolename"></param>
        /// <returns></returns>
        public int ExistRoleName(long id, string rolename)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Exist<SysRole>("where name = @0 and id != @1", rolename, id);
            }
        }


        /// <summary>
        /// 根据ID数组获取角色
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public List<SysRole> GetRolesByIds(long[] Ids)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("where id in (");
                for (int i = 0; i < Ids.Count(); i++)
                {
                    if (i < Ids.Count() - 1)
                    {
                        sb.Append(Ids[i] + ",");
                    }
                    else
                    {
                        sb.Append(Ids[i]);
                    }
                }
                sb.Append(")");
                return s.List<SysRole>(sb.ToString());
            }
        }

        /// <summary>
        /// 设置该角色是否可用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isenable"></param>
        public StateCode SetEnable(long id, bool isenable)
        {
            try
            {
                using (ISession s = SessionFactory.Instance.CreateSession())
                {
                    SysRole sysRole = s.Get<SysRole>(id);
                    if (sysRole != null)
                    {
                        s.ExcuteUpdate("update tb_sys_role  set is_enable = @0 where id = @1", isenable, id);
                    }
                }
                return StateCode.State_200;
            }
            catch
            {
                return StateCode.State_500;
            }
        }

        /// <summary>
        /// 获取用户所有角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<SysRole> GetSysRolesByUserId(int userId)
        {
            //角色列表
            List<SysRole> sysroles = new List<SysRole>();
            //获取该用户的角色
            List<UserRole> userRoles = ServiceIoc.Get<UserRoleService>().GetByUserId(userId);

            //用户权限ID数组
            List<long> roleIds = new List<long>();

            if (userRoles != null && userRoles.Count > 0)
            {
                foreach (UserRole ur in userRoles)
                {
                    //如果不存在重复的角色ID
                    if (!roleIds.Contains(ur.role_id))
                    {
                        roleIds.Add(ur.role_id);
                    }
                }
                sysroles = GetRolesByIds(roleIds.ToArray());
            }
            return sysroles;
        }


        /// <summary>
        /// 校验角色数组
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public long[] CheckRoleIds(long[] ids)
        {
            List<long> roleIds = new List<long>();
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                for (int i = 0; i < ids.Count(); i++)
                {
                    SysRole sysRole = s.Get<SysRole>(ids[0]);
                    if (sysRole != null) roleIds.Add(sysRole.id);
                }
            }
            return roleIds.ToArray();
        }




    }
}
