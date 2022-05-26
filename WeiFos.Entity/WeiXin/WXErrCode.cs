using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.Entity.WeiXin
{
    /// <summary>
    /// 微信接口响应返回结果
    /// @author arvin 
    /// @date 2014-02-11
    /// </summary>
    public class WXErrCode
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public int errcode { get; set; }

        /// <summary>
        /// 响应状态
        /// </summary>
        public string errmsg { get; set; }
    }
}
