using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.OrderModule
{
    /// <summary>
    /// 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
    /// Copyright (c) 2013-2018 深圳微狐信息技术有限公司
    /// 创 建：
    /// 日 期：2018-11-27 14:56
    /// 描 述：订单支付流程
    /// </summary>
    [Serializable]
    [Table(Name = "tb_ord_pay_flow")]
    public class OrderPayFlow
    {

        #region 实体成员

        /// <summary>
        /// 主键ID
        /// </summary>
        /// <returns></returns>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        /// <returns></returns>
        public long order_id { get; set; }

        /// <summary>
        ///  支付方式
        /// </summary>
        /// <returns></returns>
        public int pay_method { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        /// <returns></returns>
        public decimal amount { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        /// <returns></returns>
        public string serial_no { get; set; }

        /// <summary>
        /// 交易标识
        /// </summary>
        /// <returns></returns>
        public string transaction_id { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        /// <returns></returns>
        public DateTime created_date { get; set; }

        #endregion

    }
}