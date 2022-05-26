using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.SiteSettingModule
{
    /// <summary>
    /// 资讯类别 实体类
    /// @author yewei 
    /// add by @date 2015-08-18
    /// </summary>
    [Serializable]
    [Table(Name = "tb_info_category")]
    public class InformtCgty : BaseClass
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 类别前缀
        /// </summary>
        public string prefix_no { get; set; }

        /// <summary>
        /// 类别简介
        /// </summary>
        public string introduction { get; set; }

        /// <summary>
        /// 排序索引
        /// </summary>
        public int order_index { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        public long parent_id { get; set; }

        /// <summary>
        /// SeoTitle
        /// </summary>
        public string seo_title { get; set; }

        /// <summary>
        /// SeoKeyword
        /// </summary>
        public string seo_keyword { get; set; }

        /// <summary>
        /// SeoDescription
        /// </summary>
        public string seo_description { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool is_enable { get; set; }

    }
}
