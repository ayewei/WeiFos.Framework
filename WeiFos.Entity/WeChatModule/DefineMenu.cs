using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.WeChatModule
{
    /// <summary>
    /// 自定义菜单
    /// </summary>
    [Serializable]
    [Table(Name = "tb_wx_menu")]
    public class DefineMenu
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { set; get; }

        /// <summary>
        /// 分组ID
        /// </summary>
        public long group_id { set; get; }

        /// <summary>
        /// 父类ID
        /// </summary>
        public long parent_id { set; get; }

        /// <summary>
        /// 临时父级ID
        /// </summary>
        [UnMapped]
        public long tmp_parent_id { set; get; }

        /// <summary>
        /// 临时ID
        /// </summary>
        [UnMapped]
        public long tmp_id { set; get; }

        /// <summary>
        /// 图文回复属性
        /// </summary>
        [UnMapped]
        public string imgurl { set; get; }

        /// <summary>
        /// 菜单类型
        /// </summary>
        [UnMapped]
        public int biz_type { set; get; }

        /// <summary>
        /// 用户后台初始化
        /// </summary>
        [UnMapped]
        public DefineMenuBizContent biz_content { set; get; }

        /// <summary>
        /// 按钮名称
        /// </summary>
        public string name { set; get; }

        /// <summary>
        /// 触发关键词或链接地址 1 关键词 2 链接地址
        /// </summary>
        public string key_val { set; get; }

        /// <summary>
        /// 触发类型
        /// </summary>
        public string type { set; get; }

        /// <summary>
        /// 排序
        /// </summary>
        public int sort { set; get; }

    }




    public class DefineMenuBizContent
    {

        /// <summary>
        /// 对应ID
        /// </summary>
        public long id { set; get; }
         
        /// <summary>
        /// 内容值
        /// </summary>
        public string key_val { set; get; }

        /// <summary>
        /// 图文回复封面地址
        /// </summary>
        public string imgurl { set; get; }

        /// <summary>
        /// 图文回复文本值
        /// </summary>
        public string text { set; get; }
    }



}
