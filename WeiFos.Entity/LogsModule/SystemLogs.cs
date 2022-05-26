using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.LogsModule
{
    /// <summary>
    /// 接口日志 实体类
    /// @author yewei 
    /// @date 2015-11-15
    /// </summary>
    [Serializable]
    [Table(Name = "tb_logs_sys_user_op")]
    public class SystemLogs
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 系统操作用户ID
        /// </summary>
        public long sys_user_id { get; set; }


        /// <summary>
        /// 操作内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 类型 0：操作日志，1：异常日志
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime created_date { get; set; }

    }
}
