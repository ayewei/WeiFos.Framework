using WeiFos.Entity.SystemModule;
using WeiFos.ORM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.Service.SystemModule
{
    /// <summary>
    /// 用户权限Service
    /// @author yewei 
    /// @date 2013-05-17
    /// </summary>
    public class SysUserPermissionService : BaseService<SysUserPermission>
    {

        /// <summary>
        /// 根据用户ID获取权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<SysUserPermission> GetByUserId(long userId)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<SysUserPermission>("where sysuser_id = @0", userId);
            }
        }




    }
}
