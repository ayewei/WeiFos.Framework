using WeiFos.ORM.Data.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.Entity.Enums;

namespace WeiFos.Entity.SystemModule
{

    /// <summary>
    /// 系统用户实体类
    /// @author yewei 
    /// @date 2013-12-03
    /// </summary>
    [Serializable]
    [Table(Name = "tb_sys_user")]
    public class SysUser : BaseClass
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; } 

        /// <summary>
        /// 登录账号
        /// </summary>
        public string login_name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string pass_word { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 是否是管理员
        /// </summary>
        public bool? is_manager { get; set; }
          
        /// <summary>
        /// 登录IP
        /// </summary>
        public string login_ip { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        public int? login_count { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime? login_time { get; set; }

        /// <summary>
        /// 用户图像
        /// </summary>
        [UnMapped]
        public string head_img { get; set; }

        /// <summary>
        /// 登录状态码
        /// </summary>
        [UnMapped]
        public StateCode login_code { get; set; }


    }
}