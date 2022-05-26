using WeiFos.ORM.Data.Attributes;
using System;
using WeiFos.Entity;

namespace WeiFos.Entity.ShopSettingModule
{

    /// <summary>
    /// 广告图
    /// @author yewei
    /// add by @date 2015-05-17
    /// </summary>
    [Serializable]
    [Table(Name = "tb_fnt_adimg")]
    public class Advertise : BaseClass
    {
        [ID]
        public long id { get; set; }
         
        /// <summary>
        /// 广告图名称
        /// </summary>
        public string adimg_name { get; set; }

        /// <summary>
        /// 内容类型
        /// </summary>
        public int content_type { get; set; }

        /// <summary>
        /// 内容类型值
        /// </summary>
        public string content_value { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool is_show { get; set; }

        /// <summary>
        /// 1，切换图，0：平铺页面图
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int order_index { get; set; }


    }
}
