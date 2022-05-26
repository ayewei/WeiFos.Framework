using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.WeChat.WXBase
{
    /// <summary>
    /// 微信接口响应返回结果
    /// @author yewei
    /// @date 2014-02-11
    /// </summary>
    public class WXErrCode
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public int errcode { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string errmsg { get; set; }
    }
}
