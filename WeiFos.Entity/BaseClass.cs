using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.Entity
{
    /// <summary>
    /// 基本字段实体类
    /// @author yewei 
    /// @date 2013-09-21
    /// </summary>
    [Serializable]
    public class BaseClass
    {
        /// <summary>
        /// 创建用户
        /// </summary>
        public long? created_user_id { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? created_date { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public long? updated_user_id { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime? updated_date { get; set; }

    }
}
