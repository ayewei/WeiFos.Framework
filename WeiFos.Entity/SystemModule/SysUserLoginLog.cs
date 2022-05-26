using WeiFos.ORM.Data.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.Entity.Enums;

namespace WeiFos.Entity.SystemModule
{
    /// <summary>
    /// 系统用户登录日志实体
    /// @author yewei 
    /// @date 2018-10-20
    /// </summary>
    [Serializable]
    [Table(Name = "tb_logs_sys_login")]
    public class LogSysLogin 
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long sys_user_id { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        public DateTime login_time { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string login_ip { get; set; }

        /// <summary>
        /// 登录是否成功
        /// </summary>
        public bool is_success { get; set; }

        /// <summary>
        /// 登录状态
        /// </summary>
        public int device_type { get; set; }

        /// <summary>
        /// 登录执行结果
        /// </summary>
        public string result { get; set; }

    }
}