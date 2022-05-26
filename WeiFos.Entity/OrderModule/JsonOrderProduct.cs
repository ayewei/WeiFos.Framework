using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.Entity.OrderModule
{
    /// <summary>
    /// json 订单商品对象
    /// @author yewei 
    /// @date 2014-04-19
    /// </summary>
    [Serializable]
    public class JsonOrderProduct
    {
        /// <summary>
        /// 所属规格详情ID
        /// </summary>
        public string specset { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public long product_id { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string product_name { get; set; }

        /// <summary>
        /// 规格信息
        /// </summary>
        public string spec_msg { get; set; }

        /// <summary>
        /// 商品重量
        /// </summary>
        public int weight { get; set; }

        /// <summary>
        /// 商品主图URL
        /// </summary>
        public string product_img_url { get; set; }

        /// <summary>
        /// 商品单价
        /// </summary>
        public decimal? product_price { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        public int qty { get; set; }

        /// <summary>
        /// 商品租金比例
        /// </summary>
        public long rid { get; set; }

        /// <summary>
        /// 购物车ID
        /// </summary>
        public long cid { get; set; }

        /// <summary>
        /// 租聘年限
        /// </summary>
        public int rent_year { get; set; }

        /// <summary>
        /// 租金比例
        /// </summary>
        public int rent_ratio { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool is_enable { get; set; }

    }

}
