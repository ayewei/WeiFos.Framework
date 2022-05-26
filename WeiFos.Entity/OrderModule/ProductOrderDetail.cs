using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.OrderModule
{
    /// <summary>
    /// 商品订单详细实体类
    /// @author yewei 
    /// @date 2014-04-16
    /// </summary>
    [Serializable]
    [Table(Name = "tb_odr_order_detail")]
    public class ProductOrderDetail 
    {
        #region Model

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
        /// 商品ID
        /// </summary>
        public long product_id { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string product_name { get; set; }

        /// <summary>
        /// 商品英文名称
        /// </summary>
        public string product_en_name { get; set; }

        /// <summary>
        /// 商品主图URL
        /// </summary>
        public string product_img_url { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public string product_serial_number { get; set; }

        /// <summary>
        /// 规格信息
        /// </summary>
        public string spec_msg { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal? unit_price { get; set; }

        /// <summary>
        /// 所属规格详情ID
        /// </summary>
        public string specset { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// 总重量
        /// </summary>
        public int total_weight { get; set; }
        
        /// <summary>
        /// 成本价
        /// </summary>
        public decimal? cost_price { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public decimal? total_amount { get; set; }

        /// <summary>
        /// 实付金额
        /// </summary>
        public decimal? actual_amount { get; set; }

 
        /// <summary>
        /// 显示序号
        /// </summary>
        public int order_index { get; set; }


        #endregion Model
    }

}
