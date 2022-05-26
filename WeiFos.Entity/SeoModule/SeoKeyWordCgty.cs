using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.SeoModule
{
    /// <summary>
    /// 产品关键字实体类
    /// @author yewei 
    /// @date 2013-09-22
    /// </summary>
    [Serializable]
    [Table(Name = "tb_seo_keywordcgty")]
    public class SeoKeyWordCgty : BaseClass
    {
 

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        ///  关键字名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 0代表为标签
        /// 1代表为关键字
        /// </summary>
        public int? type { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        public long parent_id { get; set; }

        /// <summary>
        /// 排序索引
        /// </summary>
        public int order_index { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool is_enable { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remarks { get; set; }

   
    }
}
