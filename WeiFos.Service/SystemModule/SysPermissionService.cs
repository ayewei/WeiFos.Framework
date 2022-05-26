using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WeiFos.Entity.Enums;
using WeiFos.Entity.SystemModule;
using WeiFos.ORM.Data;
using WeiFos.Service.OrgModule;

namespace WeiFos.Service.SystemModule
{
    /// <summary>
    /// 系统权限Service
    /// @author yewei 
    /// @date 2013-05-05
    /// </summary>
    public class SysPermissionService : BaseService<SysPermission>
    {


        /// <summary>
        /// 根据ID数组获取权限
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public List<SysPermission> GetPermissionsByIds(long[] Ids)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                if (Ids.Count() > 0)
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
                    return s.List<SysPermission>(sb.ToString());
                }
                return new List<SysPermission>();
            }
        }



        /// <summary>
        /// 获取用户所有权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<SysPermission> GetPermissionsByUserId(long userId)
        {
            //权限列表
            List<SysPermission> sysPermission = new List<SysPermission>();

            //获取该用户的角色
            List<UserRole> userRoles = ServiceIoc.Get<UserRoleService>().GetByUserId(userId);

            //获取该用户的权限
            List<SysUserPermission> userPermissions = ServiceIoc.Get<SysUserPermissionService>().GetByUserId(userId);

            //用户权限ID数组
            List<long> permissionId = new List<long>();

            if (userRoles != null && userRoles.Count > 0)
            {
                foreach (UserRole ur in userRoles)
                {
                    //根据角色获取权限
                    List<SysRolePermission> rolePermissions = ServiceIoc.Get<SysRolePermissionService>().GetPermissionsByRoleId(ur.role_id);

                    //通过用户具有的角色获取权限
                    if (rolePermissions != null && rolePermissions.Count > 0)
                    {
                        foreach (SysRolePermission srp in rolePermissions)
                        {
                            //如果不存在重复的权限ID
                            if (!permissionId.Contains(srp.permission_id))
                            {
                                permissionId.Add(srp.permission_id);
                            }
                        }
                    }
                }
            }

            //通过用户权限获取权限
            if (userPermissions != null && userPermissions.Count > 0)
            {
                foreach (SysUserPermission sup in userPermissions)
                {
                    //如果不存在重复的权限ID
                    if (!permissionId.Contains(sup.permission_id))
                    {
                        permissionId.Add(sup.permission_id);
                    }
                }
                sysPermission = GetPermissionsByIds(permissionId.ToArray());
            }

            return sysPermission;
        }



        /// <summary>
        /// 权限
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public long[] CheckPermissionIds(long[] ids)
        {
            List<long> pIds = new List<long>();
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                for (int i = 0; i < ids.Count(); i++)
                {
                    SysPermission p = s.Get<SysPermission>(ids[i]);
                    if (p != null)
                        pIds.Add(p.id);
                }
            }
            return pIds.ToArray();
        }



        /// <summary>
        /// 根据上级ID获取
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<SysPermission> GetChildren(long parentId)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<SysPermission>("where parent_id =@0 order by order_index desc", parentId);
            }
        }



        /// <summary>
        /// 权限编号是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public int ExistByCode(long id, string code)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Exist<SysPermission>("where id != @0 and code = @1 ", id, code);
            }
        }



        /// <summary>
        /// 保存权限
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public StateCode Save(long user_id, SysPermission entity)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    if (entity.id == 0)
                    {
                        //创建用户ID
                        entity.created_user_id = user_id;
                        //创建时间
                        entity.created_date = DateTime.Now;

                        s.Insert(entity);
                    }
                    else
                    {
                        //修改用户ID
                        entity.updated_user_id = user_id;
                        //修改时间
                        entity.updated_date = DateTime.Now;

                        ServiceIoc.Get<SysPermissionService>().Update(entity);
                    }
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
