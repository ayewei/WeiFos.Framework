using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.SeoModule
{
    /// <summary>
    /// 设置seo页面实体类
    /// @author yewei 
    /// @date 2013-09-22
    /// </summary>
    [Serializable]
    [Table(Name = "tb_seo_page")]
    public class SeoPage : BaseClass
    {
        #region Model

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 页面名称
        /// </summary>
        public string page_name { get; set; }

        /// <summary>
        /// 页面路径
        /// </summary>
        public string page_url { get; set; }

        /// <summary>
        /// seo标题
        /// </summary>
        public string seo_title { get; set; }

        /// <summary>
        /// seo关键字
        /// </summary>
        public string seo_keyword { get; set; }

        /// <summary>
        /// seo描述
        /// </summary>
        public string seo_description { get; set; }

        /// <summary>
        /// 设置整站默认SEO 
        /// </summary>
        public bool is_default { get; set; }
         
        /// <summary>
        /// 备注
        /// </summary>
        public string remarks { get; set; }

        /// <summary>
        /// 排序索引
        /// </summary>
        public int order_index { get; set; }


        #endregion Model
    }
}
