using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data; 
using WeiFos.Service;
using WeiFos.WeChat.WXBase;
using WeiFos.WeChat.WXRequest;


namespace WeiFos.Service.WeChatModule
{

    /// <summary>
    /// 高级群发消息记录 service
    /// @author yewei 
    /// @date 2014-12-27
    /// </summary>
    public class WXSendRecordService : BaseService<WXSendRecord>
    {

        /// <summary>
        /// 微信群发回调
        /// </summary>
        /// <param name="wxReqMassSendJobFinish"></param>
        public void SetSendStatus(WXReqMassSendJobFinish wxReqMassSendJobFinish)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                WXSendRecord wxSendRecord = s.Get<WXSendRecord>("where msg_id = @1 ", wxReqMassSendJobFinish.MsgId);

                if (wxSendRecord != null)
                {
                    s.ExcuteUpdate("update tb_pub_wxsendrecord set status = @0 ", wxReqMassSendJobFinish.Status.ToString());
                }
            }
        }



    }
}
