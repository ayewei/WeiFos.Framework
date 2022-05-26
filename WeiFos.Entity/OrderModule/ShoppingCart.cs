using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.OrderModule
{
    /// <summary>
    /// 商城购物车 实体类
    /// @author yewei 
    /// @date 2014-03-24
    /// </summary>
    [Serializable]
    [Table(Name = "tb_ord_shoppingcart")]
    public class ShoppingCart
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long user_id { get; set; }
        
        /// <summary>
        /// openid
        /// </summary>
        public string openid { get; set; }
         
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
        /// 商品市场价
        /// </summary>
        public decimal? market_price { get; set; }

        /// <summary>
        /// 商品一口价
        /// </summary>
        public decimal? product_price { get; set; }

        /// <summary>
        /// 规格信息
        /// </summary>
        public string spec_msg { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public int weight { get; set; }

        /// <summary>
        /// 商品SKU组合
        /// </summary>
        public string specset { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// 租金比例ID
        /// </summary>
        public long rid { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime created_date { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime updated_date { get; set; }

    }
}
