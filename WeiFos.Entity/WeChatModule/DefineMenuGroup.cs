using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.Entity;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.WeChatModule
{
    /// <summary>
    /// 自定义菜单
    /// </summary>
    [Serializable]
    [Table(Name = "tb_wx_menu_group")]
    public class DefineMenuGroup : BaseClass
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { set; get; }

        /// <summary>
        /// 按钮名称
        /// </summary>
        public string name { set; get; }

        /// <summary>
        /// 是否可以
        /// </summary>
        public bool is_enable { set; get; }

        /// <summary>
        /// 菜单button集合
        /// </summary>
        [UnMapped]
        public List<DefineMenu> buttons { get; set; }

    }
}
