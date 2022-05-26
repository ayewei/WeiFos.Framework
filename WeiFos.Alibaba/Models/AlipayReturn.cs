using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.Alibaba.Models
{

    /// <summary>
    /// 阿里支付回调实体
    /// </summary>
    public class AlipayReturn
    {

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 支付宝交易号
        /// </summary>
        public string trade_no { get; set; }

        /// <summary>
        /// 交易状态
        /// </summary>
        public string trade_status { get; set; }

        /// <summary>
        /// 通知消息
        /// </summary>
        public string message { get; set; }

    }
}
