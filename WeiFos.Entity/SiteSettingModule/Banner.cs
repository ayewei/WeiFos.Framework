using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.SiteSettingModule
{
    /// <summary>
    /// 系统用户实体类
    /// @author yewei 
    /// @date 2015-01-09
    /// </summary>
    [Serializable]
    [Table(Name = "tb_fnt_banner")]
    public class Banner : BaseClass
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 图片类型
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// Banner名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Banner 简介
        /// </summary>
        public string intro { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool is_show { get; set; }

        /// <summary>
        /// 是否主图
        /// </summary>
        public bool is_main { get; set; }

        /// <summary>
        /// 内容类型根据BannerLink设置
        /// </summary>
        public int content_type { get; set; }

        /// <summary>
        /// 内容值
        /// </summary>
        public string content_value { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public int order_index { get; set; }

    }
}
