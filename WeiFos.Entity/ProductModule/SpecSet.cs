using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.ProductModule
{
    /// <summary>
    /// 商品规格集合
    /// </summary>
    [Serializable]
    [Table(Name = "tb_pdt_specset")]
    public class SpecSet 
    {
		/// <summary>
		/// 
		/// </summary>
		[ID]
		public long id { set; get; }

		/// <summary>
        /// 中心门店商品ID
		/// </summary>
        public long product_id{ set; get; }

        /// <summary>
		/// 规格名称ID
		/// </summary>
        public long specname_id { set; get; }

		/// <summary>
        /// 规格值ID
		/// </summary>
        public long specvalue_id { set; get; }

		/// <summary>
        /// SKUID
		/// </summary>
        public long pdt_sku_id { set; get; }
		 
    }
}
