using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.Entity.BizTypeModule
{
    /// <summary>
    /// 广告图类型 实体类
    /// @author yewei 
    /// @date 2015-03-16
    /// </summary>
    public class AdvContentType
    {

        /// <summary>
        /// 1：自定义链接
        /// </summary>
        public const int Custom = 1;


        /// <summary>
        /// 单个商品
        /// </summary>
        public static Dictionary<int, string> AdvContentTypeList = new Dictionary<int, string>() {
            { AdvContentType.Custom,"自定义链接"}
        };

    }
}
