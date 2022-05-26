using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.DistributionModule
{
    /// <summary>
    /// 运费模板详细区域 实体对象
    /// @author yewei 
    /// @date 2015-10-28
    /// </summary>
    [Serializable]
    [Table(Name = "tb_dist_freight_region")]
    public class FreightRegion
    {

        /// <summary>
        /// ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 运费模ID
        /// </summary>
        public long freight_template_id { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        public string region_name { get; set; }

        /// <summary>
        /// 首重重量
        /// </summary>
        public int first_weight { get; set; }

        /// <summary>
        /// 续重重量
        /// </summary>
        public int add_weight { get; set; }

        /// <summary>
        /// 首重价格
        /// </summary>
        public decimal first_price { get; set; }

        /// <summary>
        /// 续重价格
        /// </summary>
        public decimal add_price { get; set; }

    }
}
