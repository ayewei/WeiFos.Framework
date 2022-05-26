using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.DistributionModule
{
    /// <summary>
    /// 物流公司实体对象
    /// @author yewei 
    /// @date 2015-10-28
    /// </summary>
    [Serializable]
    [Table(Name = "tb_dist_logistics_company")]
    public class LogisticsCompany : BaseClass
    {

        /// <summary>
        /// ID
        /// </summary>
        [ID]
        public long id { get; set; }


        /// <summary>
        /// 公司名称
        /// </summary>
        public string name { get; set; }


        /// <summary>
        /// 快递100Code
        /// </summary>
        public string code_hundred { get; set; }


        /// <summary>
        /// 淘宝Code
        /// </summary>
        public string code_taobao { get; set; }


        /// <summary>
        /// 公司网址
        /// </summary>
        public string site_url { get; set; }


        /// <summary>
        /// 显示顺序
        /// </summary>
        public int order_index { get; set; }

    }
}
