using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.SKUModule
{
    /// <summary>
    /// 商品类型 扩展属性值 实体类
    /// @author  
    /// add by @date 2015-02-13
    /// </summary>
    [Serializable]
    [Table(Name = "tb_sku_ext_attrval")]
    public class ExtAttrVal : BaseClass
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
        /// 所属属性名称ID
        /// </summary>
        public long ext_attr_name_id { get; set; }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string val { get; set; }

    }
}
