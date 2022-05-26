using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.SiteSettingModule
{
    /// <summary>
    /// 成功案例
    /// @author yewei 
    /// @date 2015-01-09
    /// </summary>
    [Serializable]
    [Table(Name = "tb_cases")]
    public class SuccessfulCase : BaseClass
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 所属类别
        /// </summary>
        public long cgty_id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 英文标题
        /// </summary>
        public string english_title { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string introduction { get; set; }


        /// <summary>
        /// 内容
        /// </summary>
        public string context { get; set; }


        /// <summary>
        /// 微信内容
        /// </summary>
        public string wechat_context { get; set; }
 

        /// <summary>
        /// 排序
        /// </summary>
        public int order_index { get; set; }

        /// <summary>
        /// SeoTitle
        /// </summary>
        public string seo_title { get; set; }

        /// <summary>
        /// seo_keyword
        /// </summary>
        public string seo_keyword { get; set; }

        /// <summary>
        /// seo_description
        /// </summary>
        public string seo_description { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool is_enable { get; set; }


    }
}
