using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.SKUModule
{
    /// <summary>
    /// 平台商品类型 实体类
    /// @author  
    /// add by @date 2015-02-13
    /// </summary>
    [Serializable]
    [Table(Name = "tb_sku_product_type")]
    public class ProductType : BaseClass
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string remarks { get; set; }
    }
}
