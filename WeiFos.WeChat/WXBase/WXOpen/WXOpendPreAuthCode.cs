using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.WeChat.WXBase.WXOpen
{

    /// <summary>
    /// 获取预授权码
    /// @author yewei
    /// @date 2018-04-12
    /// </summary>
    public class WXOpendPreAuthCode : WXErrCode
    {

        /// <summary>
        /// 预授权码
        /// </summary>
        public string pre_auth_code { get; set; }


        /// <summary>
        /// 有效期，为10分钟
        /// </summary>
        public long expires_in { get; set; }

    }

}
