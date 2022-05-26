using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.WeChatModule
{
    /// <summary>
    /// 自定义粉丝组 实体类
    /// @author yewei 
    /// @date 2014-01-21
    /// </summary>
    [Serializable]
    [Table(Name = "")]
    public class FansGroup
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public int id { get; set; }

        /// <summary>
        /// 公众号ID
        /// </summary>
        public int account_id { set; get; }

        /// <summary>
        /// 用户code
        /// </summary>
        public string group_name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string group_color { get; set; }
        

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime created_date { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public int update_mode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime subscribe_time { get; set; }

        /// <summary>
        /// 上次联系时间
        /// </summary>
        public DateTime contact_time { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime end_user_tag { get; set; }

        public string area { get; set; }

        public bool sex { get; set; }
        
        
    }

}
