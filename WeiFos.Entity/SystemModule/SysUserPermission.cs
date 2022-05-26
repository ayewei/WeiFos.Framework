using WeiFos.ORM.Data.Attributes;
using WeiFos.ORM.Data.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.Entity.SystemModule
{
    /// <summary>
    /// 系统权限实体类
    /// @author yewei 
    /// @date 2013-05-17
    /// </summary>
    [Serializable]
    [Table(Name = "tb_sys_user_permission")]
    public class SysUserPermission 
    {
        #region Model

        /// <summary>
        /// 用户ID（联合主键）
        /// </summary>
        [ID(Generator = KeyGenerator.Manual)]
        public long sysuser_id { get; set; }

        /// <summary>
        /// 权限ID（联合主键）
        /// </summary>
        [ID(Generator = KeyGenerator.Manual)]
        public long permission_id { get; set; }
         
        #endregion Model
    }

}
