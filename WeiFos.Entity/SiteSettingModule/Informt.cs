using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.SiteSettingModule
{
    /// <summary>
    /// 资讯表 实体类
    /// @author yewei 
    /// add by @date 2015-08-18
    /// </summary>
    [Serializable]
    [Table(Name = "tb_info_informt")]
    public class Informt : BaseClass
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 类别ID
        /// </summary>
        public long cgty_id { get; set; }

        /// <summary>
        /// 资讯标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 资讯内容
        /// </summary>
        public string context { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime release_date { get; set; }

        /// <summary>
        /// 是否首页显示
        /// </summary>
        public bool is_index { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool is_enable { get; set; }

        /// <summary>
        /// 是否转载
        /// </summary>
        public bool is_reprinted { get; set; }

        /// <summary>
        /// 转载地址
        /// </summary>
        public string reprinted_url { get; set; }

        /// <summary>
        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool recommend { get; set; }

        /// <summary>
        /// 浏览次数
        /// </summary>
        public int view_count { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string source { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string author { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string introduction { get; set; }

        /// <summary>
        /// 索引
        /// </summary>
        public int order_index { get; set; }

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

    }
}
