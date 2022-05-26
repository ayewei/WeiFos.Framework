using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Entity.UserModule;
using WeiFos.ORM.Data;

namespace WeiFos.Service.UserModule
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserDetailService : BaseService<UserDetail>
    {


        /// <summary>
        /// 根据用户ID获取
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public UserDetail GetByUserId(long uid)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<UserDetail>("where user_id = @0 ", uid);
            }
        }




    }
}

