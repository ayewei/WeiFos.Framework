using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.ReplyModule
{
    /// <summary>
    /// 默认回复设置 实体类
    /// @author yewei 
    /// @date 2013-11-16
    /// </summary>
    [Serializable]
    [Table(Name = "tb_rpy_defaultset")]
    public class DefaultSet
    {
        /// <summary>
		/// 主键ID 
		/// </summary>
        [ID]
        public int id { get; set; }

        /// <summary>
        /// 账号ID
        /// </summary>
        public int account_id { get; set; }
        
        /// <summary>
        /// LBS查询范围
        /// </summary>
        public double lbs_distance { get; set; }

        /// <summary>
        /// LBS是否开启
        /// </summary>
        public bool lbs_isopen { get; set; }

        /// <summary>
        /// 默认回复设置 
        /// </summary>
        public string a_reply_value { get; set; }

        /// <summary>
        /// 关注默认设置 
        /// </summary>
        public string d_reply_value { get; set; }

        /// <summary>
        /// 是否开启默认回复设置
        /// </summary>
        public bool d_reply_isopen { get; set; }

        /// <summary>
        /// 是否开启关注默认设置
        /// </summary>
        public bool a_reply_isopen { get; set; }

    }

}
