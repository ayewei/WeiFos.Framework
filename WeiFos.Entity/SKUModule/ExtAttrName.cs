using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.SKUModule
{
    /// <summary>
    /// 商品类型 扩展名实体类
    /// @author  
    /// add by @date 2015-02-13
    /// </summary>
    [Serializable]
    [Table(Name = "tb_sku_ext_attrname")]
    public class ExtAttrName : BaseClass
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 商品类型ID
        /// </summary>
        public long product_type_id { get; set; }

        /// <summary>
        /// 扩展属性名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 是否多选
        /// </summary>
        public bool is_mchoice { get; set; }

        /// <summary>
        /// 排列序号
        /// </summary>
        public int order_index { get; set; }

    }

}
