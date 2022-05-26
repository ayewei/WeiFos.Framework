using WeiFos.Entity.SystemModule;
using WeiFos.ORM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.Entity.OrgModule;

namespace WeiFos.Service.SystemModule
{
    /// <summary>
    /// 角色权限Service
    /// @author yewei 
    /// @date 2013-05-17
    /// </summary>
    public class SysRolePermissionService : BaseService<SysRolePermission>
    {

        /// <summary>
        /// 根据角色ID获取权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<SysRolePermission> GetPermissionsByRoleId(long roleId)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                SysRole sr = s.Get<SysRole>(roleId);
                if (sr.is_enable)
                {
                    return s.List<SysRolePermission>("where role_Id=@0", roleId);
                }
                return new List<SysRolePermission>();
            }
        }


    }

}
