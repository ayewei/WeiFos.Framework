using System;
using System.Collections.Generic;
using System.Text;

namespace WeiFos.GaoDeng.APIModule
{
    /// <summary>
    /// 高灯开票结果
    /// </summary>
    public class InvoiceResult
    {

        /// <summary>
        /// 开票记录ID
        /// </summary>
        public long record_id { get; set; }

 
        /// <summary>
        /// 通知类型
        /// </summary>
        public string notify_type { get; set; }

        /// <summary>
        /// 通知时间
        /// </summary>
        public DateTime? notify_time { get; set; }

        /// <summary>
        /// 商户交易流水号
        /// </summary>
        public string order_id { get; set; }

        /// <summary>
        /// 平台交易流水号
        /// </summary>
        public string g_unique_id { get; set; }

        /// <summary>
        /// 发票号码
        /// </summary>
        public string ticket_sn { get; set; }

        /// <summary>
        /// 发票代码
        /// </summary>
        public string ticket_code { get; set; }

        /// <summary>
        /// 开票时间
        /// </summary>
        public DateTime? ticket_date { get; set; }

        /// <summary>
        /// 发票状态
        /// </summary>
        public int ticket_status { get; set; }

        /// <summary>
        /// 发票含税总价 开票失败时值为空
        /// </summary>
        public string ticket_total_amount_has_tax { get; set; }

        /// <summary>
        /// 发票去税总价 开票失败时值为空
        /// </summary>
        public string ticket_total_amount_no_tax { get; set; }

        /// <summary>
        /// 发票税额 开票失败时值为空
        /// </summary>
        public string ticket_tax_amount { get; set; }

        /// <summary>
        /// 发票二维码
        /// </summary>
        public string qrcode { get; set; }

        /// <summary>
        /// 发票校验码
        /// </summary>
        public string check_code { get; set; }

        /// <summary>
        /// 发票url地址
        /// </summary>
        public string pdf_url { get; set; }

        /// <summary>
        /// 创建时间（该时间为开票接口成功时间，而非异步通知时间） 
        /// </summary>
        public DateTime? create_time { get; set; }

    }
}
