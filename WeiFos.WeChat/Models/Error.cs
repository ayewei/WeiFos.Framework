using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.WeChat.Models
{
    public class WXCodeError
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public int errcode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string errmsg { get; set; }
    }
}
