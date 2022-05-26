using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.DistributionModule
{
    /// <summary>
    /// 配送方式实体对象
    /// @author yewei 
    /// @date 2015-10-28
    /// </summary>
    [Serializable]
    [Table(Name = "tb_dist_freight_template")]
    public class FreightTemplate : BaseClass
    {
        /// <summary>
        /// ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 首重
        /// </summary>
        public int first_weight { get; set; }

        /// <summary>
        /// 续重
        /// </summary>
        public int add_weight { get; set; }

        /// <summary>
        /// 默认首重价格
        /// </summary>
        public decimal default_first_price { get; set; }

        /// <summary>
        /// 默认续重价格
        /// </summary>
        public decimal default_add_price { get; set; }

    }
}
