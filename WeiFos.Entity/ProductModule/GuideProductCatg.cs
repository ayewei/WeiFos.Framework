using WeiFos.ORM.Data.Attributes;
using System; 


namespace WeiFos.Entity.ProductModule
{
    /// <summary>
    /// 商品分类实体类
    /// @author yewei
    /// add by @date 2015-02-13
    /// </summary>
    [Serializable]
    [Table(Name = "tb_pdt_guidecatg")]
    public class GuideProductCatg : BaseClass
    {

        /// <summary>
        /// 主键自动增长
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        public long parent_id { get; set; }

        /// <summary>
        /// 上级路径
        /// </summary>
        public string parent_path { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 类别编号
        /// </summary>
        public string serial_no { get; set; }

        /// <summary>
        /// 内容简介
        /// </summary>
        public string introduction { get; set; }

        /// <summary>
        /// 排列序号
        /// </summary>
        public int order_index { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool is_show { get; set; }

        /// <summary>
        /// 是否首页显示
        /// </summary>
        public bool is_index { get; set; }
 
    }
}
