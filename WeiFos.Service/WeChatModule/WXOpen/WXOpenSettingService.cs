using WeiFos.Entity.WeChatModule.WXOpen; 
using System.Collections.Generic;
using WeiFos.ORM.Data;

namespace WeiFos.Service.WeChatModule.WXOpen
{
    /// <summary>
    /// 开放平台配置表
    /// @author yewei 
    /// @date 2018-04-13
    /// </summary>
    public class WXOpenSettingService : BaseService<WXOpenSetting>
    {


        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public WXOpenSetting Get()
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<WXOpenSetting>("where id != @0", 0);
            }
        }

         

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public void UpdateTicket(long id, string ticket)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                s.ExcuteUpdate("update tb_wx_open_setting set component_verify_ticket = @0 where id = @1", ticket, id);
            }
        }



    }
}
