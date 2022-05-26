using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Entity.ProductModule;

namespace WeiFos.Entity.OrderModule
{
    /// <summary>
    /// 购物车临时接收数据对象
    /// @author yewei 
    /// @date 2016-05-14
    /// </summary>
    [Serializable]
    public class TmpShoppingCart
    {
        /// <summary>
        /// 购物车ID
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 商品规格信息
        /// </summary>
        public string spec_msg { get; set; }

        /// <summary>
        /// 商品规格ID
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
        /// 商品租金比例ID
        /// </summary>
        public long rid { get; set; }
        
        /// <summary>
        /// 租金比例
        /// </summary>
        public int ratio { get; set; }

        /// <summary>
        /// 租期
        /// </summary>
        public int lease { get; set; }

        /// <summary>
        /// 商品租金比例
        /// </summary>
        public List<RentRatio> ratios { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal product_price { get; set; }

    }
}
