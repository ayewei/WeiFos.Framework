using WeiFos.ORM.Data.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.Entity.SystemModule
{
    /// <summary>
    /// 系统角色实体类
    /// @author yewei 
    /// @date 2013-04-18
    /// </summary>
    [Serializable]
    [Table(Name = "tb_sys_role ")]
    public class SysRole : BaseClass
    {
 
        /// <summary>
		/// 主键ID
		/// </summary>
        [ID]
        public long id { get; set; }

		/// <summary>
		/// 角色名称
		/// </summary>
        public string name { get; set; } 

        /// <summary>
        /// 备注
        /// </summary>
        public string remarks { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool is_enable { get; set; }

    }
}
