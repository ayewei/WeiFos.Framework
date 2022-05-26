using System;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.ProductModule
{
    /// <summary>
    /// 扩展属性值表实体类
    /// @author yewei
    /// add by @date 2015-04-15
    /// </summary>
    [Serializable]
    [Table(Name = "tb_pdt_extattrval")]
    public class PdtExtAttrVal
    {
     
        [ID]
        public long id { set; get; }

		/// <summary>
		/// 供应商商品ID
		/// </summary>
        public long product_id { set; get; }

		/// <summary>
		/// 产品类型扩展属性ID
		/// </summary>
        public long extattrval_id { set; get; }
		 
    }
}
