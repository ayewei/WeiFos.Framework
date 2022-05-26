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
    /// add by @date 2015-08-31
    /// </summary>
    [Serializable]
    [Table(Name = "tb_site_introduction")]
    public class WebIntroduction : BaseClass
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }


        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string wechat_context { get; set; }

        /// <summary>
        /// 详情内容
        /// </summary>
        public string context { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string type { get; set; }

    }
}
