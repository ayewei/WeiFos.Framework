using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.SiteSettingModule
{
    /// <summary>
    /// 合作伙伴 实体类
    /// @author yewei 
    /// @date 2015-01-09
    /// </summary>
    [Serializable]
    [Table(Name = "tb_site_partner")]
    public class Partner : BaseClass
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 合作伙伴名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string link_url { get; set; }

        /// <summary>
        /// 排序索引
        /// </summary>
        public int order_index { get; set; }
        

    }
}
