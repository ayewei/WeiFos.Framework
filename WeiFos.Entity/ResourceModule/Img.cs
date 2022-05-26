using System;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.ResourceModule
{
    /// <summary>
    /// 官网图片实体类
    /// @author yewei 
    /// @date 2013-09-21
    /// </summary>
    [Serializable]
    [Table(Name = "tb_img")]
    public class Img : BaseRes
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 业务类型（title 页面封面图、bg 官网背景图片、ad 官网广告图、logo 官网logo 图片）
        /// </summary>
        public string biz_type { get; set; }

        /// <summary>
        /// 默认图片
        /// </summary>
        public bool default_picture { get; set; }

        /// <summary>
        /// 是否是主图
        /// </summary>
        public bool is_main { get; set; }

        /// <summary>
        /// 是否是网络图
        /// </summary>
        public bool is_webimg { get; set; }

        /// <summary>
        /// 网络图地址
        /// </summary>
        public string webimg_url { get; set; }


        /// <summary>
        /// 获取图片路径
        /// </summary>
        /// <returns></returns>
        public string getImgUrl()
        {
            return domain_name + visit_path + file_name + extend_name;
        }

        /// <summary>
        /// 获取缩略图
        /// </summary>
        /// <returns></returns>
        public string getThmImgUrl()
        {
            string m_visit_path = visit_path.Replace("/Image/", "/Thm_Image/");
            return domain_name + m_visit_path + file_name + extend_name;
        }

        /// <summary>
        /// 获取中图
        /// </summary>
        /// <returns></returns>
        public string getMedImgUrl()
        {
            string m_visit_path = visit_path.Replace("/Image/", "/Med_Image/");
            return domain_name + m_visit_path + file_name + extend_name;
        }

    }
}
