using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.SystemModule
{
    /// <summary>
    /// 操作日志Service
    /// @author yewei 
    /// @date 2014-01-23
    /// </summary>
    [Serializable]
    [Table(Name = "tb_sys_operatinglogs")]
    public class SysOperatingLogs
    {

        #region Model

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public int id { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime operating_time { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        public string operating_content { get; set; }

        /// <summary>
        /// 操作系统用户
        /// </summary>
        public int sysuser_id { get; set; }


        #endregion Model

    }

}
