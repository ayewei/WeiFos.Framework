using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.Entity.Enums;

namespace WeiFos.Entity.Common
{
    /// <summary>
    /// WeiFos 全局状态返信息
    /// @author yewei 
    /// @date 2015-02-11
    /// </summary>
    public class StateMessage
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public StateCode Code { get; set; }

        /// <summary>
        /// 状态信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 自定义参数
        /// </summary>
        public object Data { get; set; }

    }

}
