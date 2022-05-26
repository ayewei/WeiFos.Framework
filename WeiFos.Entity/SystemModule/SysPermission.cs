using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.SystemModule
{
    /// <summary>
    /// 系统权限实体类
    /// @author yewei 
    /// @date 2013-05-07
    /// </summary>
    [Serializable]
    [Table(Name = "tb_sys_permission")]
    public class SysPermission : BaseClass
    {
        #region Model

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 权限编号
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 权限对应页面
        /// </summary>
        public string action_url { get; set; }
        
        /// <summary>
        /// 权限说明
        /// </summary>
        public string remarks { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        public long parent_id { get; set; }
        
        /// <summary>
        /// 排序索引
        /// </summary>
        public int order_index { get; set; }

        #endregion Model
    }
}
