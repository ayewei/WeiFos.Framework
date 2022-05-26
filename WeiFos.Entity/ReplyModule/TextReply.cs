using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.ReplyModule
{
    /// <summary>
    /// 文本回复实体类
    /// @author yewei 
    /// @date 2013-10-22
    /// </summary>
    [Serializable]
    [Table(Name = "tb_rpy_textreply")]
    public class TextReply : BaseClass
    {
        #region Model

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }
         
        /// <summary>
        /// 回复类容
        /// </summary>
        public string reply_contents { get; set; }

        #endregion Model
    }
}
