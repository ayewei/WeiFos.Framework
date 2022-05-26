using WeiFos.ORM.Data.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.Entity.SystemModule
{
    /// <summary>
    /// 配置参数实体类
    /// @author yewei 
    /// @date 2013-04-27
    /// </summary>
    [Serializable]
    [Table(Name = "tb_sys_config_param")]
    public class ConfigParam : BaseClass
    {
  

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }
        
        /// <summary>
        /// 参数key
        /// </summary>
        public string config_key { get; set; }

        /// <summary>
        /// 参数value
        /// </summary>
        public string config_value { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool is_enable { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remarks { get; set; }

 

    }
}
