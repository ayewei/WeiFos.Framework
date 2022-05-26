using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Entity;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.DistributionModule
{
    /// <summary>
    /// 配送方式实体对象
    /// @author yewei 
    /// @date 2015-10-28
    /// </summary>
    [Serializable]
    [Table(Name = "tb_dist_deliverymode")]
    public class DeliveryMode : BaseClass
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
        /// 物流公司ID
        /// </summary>
        public string logistics_company_ids { get; set; }

        /// <summary>
        /// 运费模板ID
        /// </summary>
        public long freight_template_id { get; set; }

        /// <summary>
        /// 是否默认选择配送方式
        /// </summary>
        public bool is_default { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int order_index { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string remarks { get; set; }

    }
}
