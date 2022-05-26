using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.WeChatModule
{
    /// <summary>
    /// 粉丝标签中间表 实体类
    /// @author yewei 
    /// @date 2015-01-05
    /// </summary>
    [Serializable]
    [Table(Name = "tb_wx_usertag_u")]
    public class UserTagUnite
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public int id { get; set; }

        /// <summary>
        /// 微信用户ID
        /// </summary>
        public string open_id { get; set; }

        /// <summary>
        /// 标签ID
        /// </summary>
        public int user_tag_id { get; set; }
            
    }
}
