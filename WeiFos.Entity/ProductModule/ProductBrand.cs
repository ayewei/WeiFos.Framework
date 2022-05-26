using System;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.ProductModule
{
    /// <summary>
    /// 品牌实体类
    /// </summary>
    [Serializable]
    [Table(Name = "tb_pdt_brand")]
    public class ProductBrand : BaseClass
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 分类ID
        /// </summary>
        public long cgty_id { get; set; }

        /// <summary>
        /// 品牌名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 英文名称
        /// </summary>
        public string name_en { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int order_index { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remarks { get; set; }

    }
}
