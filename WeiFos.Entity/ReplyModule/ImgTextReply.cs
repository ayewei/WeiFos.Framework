using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.ReplyModule
{
    /// <summary>
    /// 网站页面详情实体类
    /// @author yewei 
    /// @date 2013-09-21
    /// </summary>
    [Serializable]
    [Table(Name = "tb_rpy_imgtextreply")]
    public class ImgTextReply : BaseClass
    {
        #region Model
        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 页面标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 页面详情
        /// </summary>
        public string details { get; set; }

        /// <summary>
        /// 内容简介
        /// </summary>
        public string introduction { get; set; }

        /// <summary>
        /// 内容类型
        /// </summary>
        public int content_type { get; set; }

        /// <summary>
        /// 内容类型值
        /// </summary>
        public string content_value { get; set; }

        /// <summary>
        /// 多图文引用
        /// </summary>
        public string quote_detailsIds { get; set; }

        /// <summary>
        /// 所属栏目
        /// </summary>
        public int category_id { get; set; }

        /// <summary>
        /// 所属账号
        /// </summary>
        //public int account_id { get; set; }

        /// <summary>
        /// 显示标题图片
        /// </summary>
        public bool show_titleimg { get; set; }

        /// <summary>
        /// 推荐消息
        /// </summary>
        public string rec_detailsIds { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int order_index { get; set; }

        #endregion Model
    }
}
