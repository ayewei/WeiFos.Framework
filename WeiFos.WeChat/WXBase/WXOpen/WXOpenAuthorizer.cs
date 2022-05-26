using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.WeChat.WXBase.WXOpen
{
    /// <summary>
    /// 微信开放平台
    /// 公众号授权信息实体
    /// @author yewei
    /// @date 2018-04-11
    /// </summary>
    public class WXOpenAuthorizer : WXErrCode
    {

        /// <summary>
        /// authorizer_info
        /// </summary>
        public WXOpenWeChatAccount authorizer_info { get; set; }


        /// <summary>
        /// 授权方appid
        /// </summary>
        public WXOpenAuthInfo authorization_info { get; set; }


    }
}
