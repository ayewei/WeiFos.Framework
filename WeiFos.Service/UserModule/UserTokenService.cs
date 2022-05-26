using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Entity.Enums;
using WeiFos.Entity.LogsModule;
using WeiFos.Entity.UserModule;
using WeiFos.ORM.Data;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Service.UserModule
{
    /// <summary>
    /// 用户 Service
    /// @date 2015-09-25
    /// </summary>
    public class UserTokenService : BaseService<UserToken>
    {


        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public UserToken Get(string t)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<UserToken>("where token = @0", t);
            }
        }



        /// <summary>
        /// 更新令牌最后活动时间
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public StateCode TokenUpdate(string token)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    var ret = s.ExecuteScalar(string.Format("exec p_user_token_update '{0}'", token));
                    return StateCode.State_200;
                }
                catch (Exception ex)
                {
                    APILogs log = new APILogs();
                    log.content = "更新令牌最后活动时间异常==》" + ex.ToString();
                    log.created_date = DateTime.Now;
                    log.type = 1;
                    s.Insert(log);
                    return StateCode.State_500;
                }
            }
        }




    }
}
