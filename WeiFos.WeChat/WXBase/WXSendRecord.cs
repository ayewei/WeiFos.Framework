using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.WeChat.WXBase
{
    /// <summary>
    /// 微信群发回传json对象
    /// @author yewei 
    /// @date 2015-01-11
    /// </summary>
    [Serializable]
    public class WXSendRecord : WXErrCode
    {

        /// <summary>
        /// 发送对象
        /// </summary>
        public string send_obj { get; set; }

        /// <summary>
        /// 发送对象值
        /// </summary>
        public string send_obj_value { get; set; }
        

        /// <summary>
        /// 发送对象 所显示文本
        /// </summary>
        public string send_obj_text { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public string send_msg_type { get; set; }

        /// <summary>
        /// 发送值
        /// </summary>
        public string send_content { get; set; }

        /// <summary>
        /// 微信群发回传状态
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 发送状态
        /// </summary>
        public string send_status { get; set; }

        /// <summary>
        /// 群发的消息ID
        /// </summary>
        public long msg_id { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime send_time { get; set; }
        
    }

}
