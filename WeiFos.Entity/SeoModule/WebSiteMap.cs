using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.SeoModule
{
    /// <summary>
    /// 站点地图实体类
    /// @author yewei 
    /// @date 2013-09-04
    /// </summary>
    [Serializable]
    [Table(Name = "tb_seo_sitemap")]
    public class WebSiteMap : BaseClass
    {
        #region Model

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 链接名称
        /// </summary>
        public string map_name { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string map_url { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        public int parent_id { get; set; }

        /// <summary>
        /// 排序索引
        /// </summary>
        public int order_index { get; set; }

        /// <summary>
        /// 页面更新频率，取值为：always, hourly, daily, weekly, monthly, yearly, never。
        /// 这里需要注意：always表示页面在每次被访问到时就更新never表示当前Url的是一个目录
        /// </summary>
        public string changefreq { get; set; }

        /// <summary>
        /// 当前Url的相对优先权，这个优先权是相对于当前Sitemap中其它Url而言的
        /// </summary>
        public string priority { get; set; }
         
        #endregion Model
    }
}
