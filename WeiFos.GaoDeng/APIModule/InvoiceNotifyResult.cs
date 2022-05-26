using System;
using System.Collections.Generic;
using System.Text;

namespace WeiFos.GaoDeng.APIModule
{
    /// <summary>
    /// 开票通知结果 实体对象
    /// @date 2019-04-09
    /// </summary>
    public class InvoiceNotifyResult
    {

        /// <summary>
        /// 通知类型
        /// </summary>
        public int code { get; set; }


        /// <summary>
        /// 通知类型
        /// </summary>
        public string msg { get; set; }


        /// <summary>
        /// 通知类型
        /// </summary>
        public InvoiceResult data { get; set; }

    }

}