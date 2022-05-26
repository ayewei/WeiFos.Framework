using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.ProductModule
{
    /// <summary>
    /// 中心门店_商品SKU信息
    /// </summary>
    [Serializable]
    [Table(Name = "tb_pdt_sku")]
    public class ProductSku
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { set; get; }

        /// <summary>
        /// 平台商品ID
        /// </summary>
        public long product_id { set; get; }

        /// <summary>
        /// 规格集合
        /// </summary>
        public string specset { set; get; }

        /// <summary>
        /// 规格集合
        /// </summary>
        public string product_name { set; get; }

        /// <summary>
        /// SKU名称
        /// </summary>
        public string sku_name { set; get; }

        /// <summary>
        /// 重量
        /// </summary>
        public int weight { set; get; }

        /// <summary>
        /// 货号
        /// </summary>
        public string serial_no { set; get; }

        /// <summary>
        /// 成本价
        /// </summary>
        public decimal cost_price { set; get; }

        /// <summary>
        /// 市场价
        /// </summary>
        public decimal market_price { set; get; }

        /// <summary>
        /// 销售价
        /// </summary>
        public decimal sale_price { set; get; } 

        /// <summary>
        /// 库存
        /// </summary>
        public int stock { set; get; }

        /// <summary>
        /// 预警库存
        /// </summary>
        public int warning_stock { set; get; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool is_enable { set; get; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool is_delete { set; get; }

    }
}
