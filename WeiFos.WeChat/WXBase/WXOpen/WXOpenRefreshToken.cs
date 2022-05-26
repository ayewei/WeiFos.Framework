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
    /// 刷新令牌结果参数
    /// </summary>
    public class WXOpenRefreshToken : WXErrCode
    {

        /// <summary>
        /// 授权方令牌
        /// </summary>
        public string authorizer_access_token { get; set; }


        /// <summary>
        /// 有效期，为2小时
        /// </summary>
        public long expires_in { get; set; }


        /// <summary>
        /// 刷新令牌
        /// </summary>
        public string authorizer_refresh_token { get; set; }

    }
}
