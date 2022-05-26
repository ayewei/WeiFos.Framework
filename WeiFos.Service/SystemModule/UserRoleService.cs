using WeiFos.Entity.SystemModule;
using WeiFos.ORM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.Service.SystemModule
{
    /// <summary>
    /// 用户角色Service
    /// @author yewei 
    /// @date 2013-05-16
    /// </summary>
    public class UserRoleService : BaseService<UserRole>
    {

        /// <summary>
        /// 通过用户ID获取角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserRole> GetByUserId(long userId)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<UserRole>("where sysuser_id = @0", userId);
            }
        }


    }
}
