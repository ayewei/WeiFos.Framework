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
    /// 描 述：订单修改记录
    /// </summary>
    [Serializable]
    [Table(Name = "tb_odr_order_record")]
    public class OrderRecord
    {

        #region 实体成员

        /// <summary>
        /// 主键ID
        /// </summary>
        /// <returns></returns>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        /// <returns></returns>
        public string serial_no { get; set; }

        /// <summary>
        /// 修改前金额
        /// </summary>
        /// <returns></returns>
        public decimal before_amount { get; set; }

        /// <summary>
        /// 修改后金额
        /// </summary>
        /// <returns></returns>
        public decimal after_amount { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        /// <returns></returns>
        public string content { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        public long created_user_id { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        /// <returns></returns>
        public DateTime created_date { get; set; }

        #endregion

    }
}