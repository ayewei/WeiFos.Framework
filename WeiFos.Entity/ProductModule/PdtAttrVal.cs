using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.ProductModule
{
    /// <summary>
    /// 商品_基础属性_属性值实体类
    /// @author yewei
    /// add by @date 2015-04-15
    /// </summary>
    [Serializable]
    [Table(Name = "tb_pdt_attrval")]
    public class PdtAttrVal 
    {
        /// <summary>
        /// 主见ID
        /// </summary>
        [ID]
        public long id { set; get; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public long product_id { set; get; }

        /// <summary>
        /// 所属属性名称ID
        /// </summary>
        public long attrname_id { set; get; }

        /// <summary>
        /// 属性值
        /// </summary>
        public string val { set; get; }
    }
}
