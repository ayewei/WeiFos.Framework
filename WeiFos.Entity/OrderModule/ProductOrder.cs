using System;
using System.Collections.Generic;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.OrderModule
{
    /// <summary>
    /// 商品订单实体类
    /// @author yewei 
    /// @date 2014-04-16
    /// </summary>
    [Serializable]
    [Table(Name = "tb_odr_order")]
    public class ProductOrder : BaseClass
    {
        #region Model
        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 订单类型
        /// 0 商品订单，1课程订单
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 付款模式
        /// 0：全款
        /// 1：分期
        /// </summary>
        public int pay_model { get; set; }

        /// <summary>
        /// 用户优惠卷ID
        /// </summary>
        public long user_coupon_id { get; set; }

        /// <summary>
        /// 优惠卷金额
        /// </summary>
        public decimal coupon_amount { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string serial_no { get; set; }

        /// <summary>
        /// 第三方 交易标识
        /// </summary>
        public string transaction_id { get; set; }

        /// <summary>
        /// 状态查看 OrderStatus 状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 推荐人
        /// </summary>
        public string referrer { get; set; }

        /// <summary>
        /// 0,未删除，1回收站，2已删除 状态
        /// </summary>
        public int delete_status { get; set; }

        /// <summary>
        /// 0未发起退款，
        /// 1已发起退款，
        /// 2退款未通过，
        /// 5退款已通过 
        /// </summary>
        public int refund_status { get; set; }

        /// <summary>
        /// 是否支付
        /// </summary>
        public bool is_pay { get; set; }

        /// <summary>
        /// 是否货到付款
        /// </summary>
        public bool is_cod { get; set; }

        /// <summary>
        /// 成本价
        /// </summary>
        public decimal cost_price { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public decimal total_amount { get; set; }

        /// <summary>
        /// 商品总重量
        /// </summary>
        public int total_weight { get; set; }

        /// <summary>
        /// 实付金额
        /// </summary>
        public decimal actual_amount { get; set; }

        /// <summary>
        /// 配送方式
        /// </summary>
        public long delivery_mode_id { get; set; }

        /// <summary>
        /// 提货方式 0:物流、1:自提
        /// </summary>
        public int logistic_method { get; set; }

        /// <summary>
        /// 配送费用
        /// </summary>
        public decimal freight { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public int pay_method { get; set; }

        /// <summary>
        /// 支付手续费
        /// </summary>
        public decimal handling_fee { get; set; }

        /// <summary>
        /// 订单备注
        /// </summary>
        public string remarks { get; set; }

        /// <summary>
        /// 发票抬头
        /// </summary>
        public string invoice { get; set; }

        /// <summary>
        /// 会员优惠折扣
        /// </summary>
        public decimal discount_amount { get; set; }

        /// <summary>
        /// 订单详图片集合
        /// </summary>
        [UnMapped]
        public List<string> detail_imgs { get; set; }

        /// <summary>
        /// 订单数量集合
        /// </summary>
        [UnMapped]
        public int details_count { get; set; }

        /// <summary>
        /// 订单明细集合
        /// </summary>
        [UnMapped]
        public List<ProductOrderDetail> details { get; set; }

        /// <summary>
        /// 订单收件人信息
        /// </summary>
        [UnMapped]
        public OrderDelivery delivery { get; set; }



        #endregion Model

    }
}
