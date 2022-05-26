using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.UserModule
{

    /// <summary>
    /// 会员等级记录 实体对象
    /// @date 2018-03-07
    /// </summary>

    [Serializable]
    [Table(Name = "tb_mbr_level_setting")]
    public class MemberLevelSetting : BaseClass
    {

        /// <summary>
        /// id
        /// </summary>
        [ID]
        public long id { get; set; }
         
        /// <summary>
        /// 级别名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public int? level { get; set; }
         
        /// <summary>
        /// 折扣
        /// </summary>
        public int discount { get; set; }

        /// <summary>
        /// 累计消费金额
        /// </summary>
        public decimal total_amount { get; set; }
         

    }
}