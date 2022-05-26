using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.SKUModule
{
    /// <summary>
    /// 商品规格值
    /// @author yewei
    /// add by @date 2015-02-28 
    /// </summary>
    [Serializable]
    [Table(Name = "tb_sku_specvalue")]
    public class SpecValue : BaseClass
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 产品类型ID
        /// </summary>
        public long product_type_id { get; set; }

        /// <summary>
        /// 所属规格名称ID
        /// </summary>
        public long specname_id { get; set; }

        /// <summary>
        /// 规格值
        /// </summary>
        public string val { get; set; }

        /// <summary>
        /// 排列序号
        /// </summary>
        public int order_index { get; set; }

    }
}
