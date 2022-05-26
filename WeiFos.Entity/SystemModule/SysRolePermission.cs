using WeiFos.ORM.Data.Attributes;
using WeiFos.ORM.Data.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.Entity.SystemModule
{
    /// <summary>
    /// 角色权限实体类
    /// @author yewei 
    /// @date 2013-05-17
    /// </summary>
    [Serializable]
    [Table(Name = "tb_sys_role_permission")]
    public class SysRolePermission 
    {
        /// <summary>
        /// 权限ID（联合主键）
        /// </summary>
        [ID(Generator = KeyGenerator.Manual)]
        public long permission_id { get; set; }

        /// <summary>
        /// 角色ID（联合主键）
        /// </summary>
        [ID(Generator = KeyGenerator.Manual)]
        public long role_id { get; set; }
         

    }
}
