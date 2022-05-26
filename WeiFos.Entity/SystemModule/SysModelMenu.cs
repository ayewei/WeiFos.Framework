using WeiFos.ORM.Data.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.Entity.SystemModule
{
    /// <summary>
    /// 系统菜单实体类
    /// @author yewei 
    /// @date 2013-05-07
    /// </summary>
    [Serializable]
    [Table(Name = "tb_sys_model_menu")]
    public class SysModelMenu : BaseClass
    {
        #region Model
        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 所属模块
        /// </summary>
        public int model { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 菜单编号
        /// </summary>
        public string serial_no { get; set; }

        /// <summary>
        /// 菜单链接
        /// </summary>
        public string action_url { get; set; }

        /// <summary>
        /// 父类Id
        /// </summary>
        public long parent_id { get; set; }

        /// <summary>
        /// 排序索引
        /// </summary>
        public int order_index { get; set; }

        /// <summary>
        /// 菜单样式
        /// </summary>
        public string menu_class { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool is_enable { get; set; }


        #endregion Model
    }
}
