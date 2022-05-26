using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.Entity.BizTypeModule
{
    /// <summary>
    /// Banner链接 实体类
    /// @author yewei 
    /// @date 2015-03-16
    /// </summary>
    public class BannerLink
    {

        /// <summary>
        /// 1：自定义链接
        /// </summary>
        public const int Custom = 1;

        /// <summary>
        /// 2：商品列表
        /// </summary>
        public const int ProductList = 2;

        /// <summary>
        /// 5：商品详情
        /// </summary>
        public const int ProductDetails = 5;


        /// <summary>
        /// 15：领劵中心
        /// </summary>
        public const int Coupons = 15;

        /// <summary>
        /// 集合
        /// </summary>
        public static Dictionary<int, string> List = new Dictionary<int, string>() {
            { BannerLink.Custom,"自定义链接"},
            { BannerLink.ProductList,"商品分类"},
            { BannerLink.ProductDetails,"商品详情"}, 
            { BannerLink.Coupons,"领劵中心"}
        };


    }
}
