using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.Entity.SystemModule;
using WeiFos.ORM.Data;

namespace WeiFos.Service.SystemModule
{
    /// <summary>
    /// 系统用户公司信息Service
    /// @author yewei 
    /// @date 2014-01-24
    /// </summary>
    public class SysUserCompanyMsgService : BaseService<SysUserCompanyMsg>
    {


        /// <summary>
        /// 根据用户ID获取公司信息
        /// </summary>
        /// <param name="sysuserId"></param>
        /// <returns></returns>
        public SysUserCompanyMsg GetBySysUserId(int sysuser_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<SysUserCompanyMsg>("where sysuser_id = @0", sysuser_id);
            }
        }

    }

}
