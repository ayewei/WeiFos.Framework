using WeiFos.ORM.Data.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.Entity.SystemModule
{
    /// <summary>
    /// 用户角色实体类
    /// @author yewei 
    /// @date 2013-05-06
    /// </summary>
    [Serializable]
    [Table(Name = "tb_sys_user_role")]
    public class UserRole
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long sysuser_id { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public long role_id { get; set; }
    }

}
