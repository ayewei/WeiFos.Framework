using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Entity.Enums;
using WeiFos.Entity.WeChatModule;
using WeiFos.ORM.Data;
using WeiFos.Service;

namespace WeiFos.Service.WeChatModule
{
    /// <summary>
    /// 微信用户service 
    /// @author yewei 
    /// @date 2015-05-17
    /// </summary>
    public class WeChatUserMsgService : BaseService<WeChatUserMsg>
    {


        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="user_msg"></param>
        /// <returns></returns>
        public StateCode Save(WeChatUserMsg user_msg)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    long id = 0;
                    object data = s.ExecuteScalar("select id from tb_wx_user_msg where openid = @0", user_msg.openid);
                    long.TryParse(data == null ? "0" : data.ToString(), out id);

                    if (id == 0)
                    {
                        s.Insert<WeChatUserMsg>(user_msg); 
                    }
                    else
                    {
                        user_msg.id = id;
                        s.Update<WeChatUserMsg>(user_msg);
                    }
                    return StateCode.State_200;
                }
                catch
                {
                    return StateCode.State_500;
                }
            }
        }


        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public WeChatUserMsg Get(string openid)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<WeChatUserMsg>("where openid = @0", openid); 
            }
        }




    }
}
