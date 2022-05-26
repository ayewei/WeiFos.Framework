using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.OrderModule
{
    /// <summary>
    /// 订单退订 实体对象
    /// @author yewei 
    /// @date 2014-12-29
    /// </summary>
    [Serializable]
    [Table(Name = "tb_odr_refund")]
    public class OrderRefund : BaseClass
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public long order_id { get; set; }


        /// <summary>
        /// 订单编号
        /// </summary>
        public string order_serial_no { get; set; }

        /// <summary>
        /// 退款单编号
        /// </summary>
        public string refund_serial_no { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 订单实付金额
        /// </summary>
        public decimal? order_actual_amount { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal? refund_total_amount { get; set; }

        /// <summary>
        /// 实退金额
        /// </summary>
        public decimal? refund_actual_amount { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string pay_method { get; set; }

        /// <summary>
        /// 支付手续费
        /// </summary>
        public decimal? handling_fee { get; set; }

        /// <summary>
        /// 原因
        /// </summary>
        public string reason { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remarks { get; set; }

        /// <summary>
        /// 物流单号
        /// </summary>
        public string logistics_no { get; set; }

        /// <summary>
        /// 物流公司
        /// </summary>
        public string logistics_company { get; set; }

        /// <summary>
        /// 物流费用
        /// </summary>
        public decimal? logistics_fee { get; set; }


    }
}
