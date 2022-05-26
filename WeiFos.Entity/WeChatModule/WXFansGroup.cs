using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.WeChatModule
{
    /// <summary>
    /// 粉丝分组实体类
    /// @author yewei 
    /// @date 2014-01-21
    /// </summary>
    [Serializable]
    [Table(Name = "tb_wx_usergroup")]
    public class WXFansGroup
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public int id { get; set; }       
        
        /// <summary>
        /// 用户code
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 公众号ID
        /// </summary>
        public int count { set; get; }

    }

}
