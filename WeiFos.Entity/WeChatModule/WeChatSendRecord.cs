using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;
using WeiFos.WeChat.WXBase;

namespace WeiFos.Entity.WeChatModule
{
    /// <summary>
    /// 微信推送实体类
    /// @author yewei 
    /// @date 2013-04-27
    /// </summary>
    [Serializable]
    [Table(Name = "tb_wx_sendrecord")]
    public class WeChatSendRecord : WXSendRecord
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public int id { get; set; }

    }
}
