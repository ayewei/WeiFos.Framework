using System; 
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.ProductModule
{
    /// <summary>
    /// 商品分类实体类
    /// </summary>
    [Serializable]
    [Table(Name = "tb_pdt_category")]
    public class ProductCgty : BaseClass
    {

        /// <summary>
        /// 
        /// </summary>
        [ID]
        public long id { set; get; }

        /// <summary>
        /// 上级ID
        /// </summary>
        public long parent_id { set; get; }

        /// <summary>
        /// 类别名称
        /// </summary>
        public string name { set; get; }

        /// <summary>
        /// 类别编号
        /// </summary>
        public string serial_no { set; get; }

        /// <summary>
        /// 内容简介
        /// </summary>
        public string introduction { set; get; }

        /// <summary>
        /// 排列序号
        /// </summary>
        public int order_index { set; get; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool is_show { set; get; }

    }
}
