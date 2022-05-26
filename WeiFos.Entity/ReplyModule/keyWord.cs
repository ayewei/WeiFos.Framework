using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.ReplyModule
{
    /// <summary>
    /// 关键字实体类
    /// @author yewei 
    /// @date 2013-09-21
    /// </summary>
    [Serializable]
    [Table(Name = "tb_rpy_keywords")]
    public class KeyWord : BaseClass
    {
 
		/// <summary>
		/// 主键ID
		/// </summary>
        [ID]
        public long id { get; set; }

		/// <summary>
		/// 回复关键词
		/// </summary>
        public string keyword { get; set; }

        /// <summary>
        /// 所属模块业务  
        /// </summary>
        public string biz_type { get; set; }

        /// <summary>
        /// 所属模块业务ID
        /// </summary>
        public long biz_id { get; set; }
         
    }
}
