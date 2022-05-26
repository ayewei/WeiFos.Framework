using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.WeChatModule
{
    /// <summary>
    /// 粉丝标签实体类
    /// @author yewei 
    /// @date 2015-01-04
    /// </summary>
    [Serializable]
    [Table(Name = "tb_wx_usertag")]
    public class WeChatUserTag
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public int id { get; set; } 

        /// 标签名称
        /// </summary>
        public string tag_name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? created_date { get; set; }
                

    }
}
