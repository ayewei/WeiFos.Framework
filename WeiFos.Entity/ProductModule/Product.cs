using System;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.ProductModule
{
    /// <summary>
    /// 商品实体类
    /// @author yewei
    /// add by @date 2015-04-15
    /// </summary>
    [Serializable]
    [Table(Name = "tb_pdt_product")]
    public class Product : BaseClass
    {

        /// <summary>
        /// ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public long supplier_id { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string en_name { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public string no { get; set; }

        /// <summary>
        /// 所属分类
        /// </summary>
        public long catg_id { get; set; }

        /// <summary>
        /// 分类路径
        /// </summary>
        public string catg_path { get; set; }

        /// <summary>
        /// 分类路径名称
        /// </summary>
        public string catg_pathname { get; set; }

        /// <summary>
        ///导购分类 
        /// </summary>
        public long gcatg_id { get; set; }

        /// <summary>
        /// 导购分类路径
        /// </summary>
        public string gcatg_path { get; set; }

        /// <summary>
        /// 导购分类路径名称
        /// </summary>
        public string gcatg_pathname { get; set; }

        /// <summary>
        /// 所属品牌
        /// </summary>
        public long brand_id { get; set; }

        /// <summary>
        /// 商品类型
        /// </summary>
        public long product_type_id { get; set; }

        /// <summary>
        /// 销售价
        /// </summary>
        public decimal sale_price { get; set; }

        /// <summary>
        /// 市场价
        /// </summary>
        public decimal market_price { get; set; }

        /// <summary>
        /// 是否上架
        /// </summary>
        public bool is_shelves { get; set; }

        /// <summary>
        /// 上架开始日期
        /// </summary>
        public DateTime? shelves_sdate { get; set; }

        /// <summary>
        /// 下架开始日期
        /// </summary>
        public DateTime? shelves_edate { get; set; }

        /// <summary>
        /// 排列序号
        /// </summary>
        public int order_index { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool? is_delete { get; set; }

        /// <summary>
        /// 是否开启规格
        /// </summary>
        public bool is_open_spec { get; set; }

        /// <summary>
        /// 是否首页显示
        /// </summary>
        public bool is_index { get; set; }

        /// <summary>
        /// 商品计量单位
        /// </summary>
        public string unit { get; set; }

        /// <summary>
        /// 商品简介
        /// </summary>
        public string introduction { get; set; }

        /// <summary>
        /// 商品详情
        /// </summary>
        public string details { get; set; }

        /// <summary>
        /// 商品标签
        ///  ,1,1,1,1,1,
        ///  ,0,0,0,0,0,
        ///  ,新品, 热门, 推荐,  , ,
        /// </summary>
        public string tag { get; set; }

        /// <summary>
        /// 是否包邮
        /// </summary>
        public bool is_postage { get; set; }

        /// <summary>
        /// 销量
        /// </summary>
        public int sales { get; set; }

    }
}
