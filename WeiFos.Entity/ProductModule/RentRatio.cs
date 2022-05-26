using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.ProductModule
{
    /// <summary>
    /// 商品返佣比例
    /// </summary>
    [Serializable]
    [Table(Name = "tb_pdt_ratio")]
    public class RentRatio
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { set; get; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public long product_id { set; get; }

        /// <summary>
        /// 比例
        /// </summary>
        public int number { set; get; }

    }
}
