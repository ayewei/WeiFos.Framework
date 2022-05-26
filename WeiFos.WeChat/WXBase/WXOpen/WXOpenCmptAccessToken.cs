using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.WeChat.WXBase.WXOpen
{


    /// <summary>
    /// @author yewei 
    /// @date 2018-04-12
    /// 第一步：获取compoment_access_token
    /// 在第三方应用通过审核后，微信服务器每隔10分钟向第三方的接收地址推送一次component_verify_ticket，
    /// 用于第三方接口 调用凭据
    /// </summary>
    public class WXOpenCmptAccessToken : WXErrCode
    {

        /// <summary>
        /// token令牌
        /// </summary>
        public string component_access_token { get; set; }

        /// <summary>
        /// 过期时间（秒）
        /// </summary>
        public long expires_in { get; set; }

    }
}
