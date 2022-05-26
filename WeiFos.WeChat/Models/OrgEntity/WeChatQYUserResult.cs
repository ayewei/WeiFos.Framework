using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.WeChat.Models.OrgEntity
{
    /// <summary>
    /// 获取 获取访问用户身份
    /// @author yewei 
    /// @date 2019-3-18
    /// </summary>
    [Serializable]
    public class WeChatQYUserResult : WXCodeError
    {

        /// <summary>
        /// 企业员工用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户openid非企业员工
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 是否是企业员工
        /// 该字段用于程序处理
        /// </summary>
        public bool IsEmployee { get; set; }


    }
}
