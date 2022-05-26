using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiZhong.Domain.ReplyModule
{
    /// <summary>
    /// 图文链接类型实体类
    /// @author yewei 
    /// @date 2013-10-03
    /// </summary>
    public class ImgTextContentType
    {
        /// <summary>
        /// 外部链接
        /// </summary>
        public const string OutLink = "OutLink";

        /// <summary>
        /// 导航信息
        /// </summary>
        public const string Navigation = "Navigation";

        /// <summary>
        /// 微活动
        /// </summary>
        public const string WeiActivity = "WeiActivity";

        /// <summary>
        /// 业务类型
        /// </summary>
        public const string BizType = "BizType";

        /// <summary>
        /// 微汽车
        /// </summary>
        public const string WeiCar = "WeiCar";

        /// <summary>
        /// 微房产
        /// </summary>
        public const string WeiEstate = "WeiEstate";


        public static Dictionary<string, string> typeList = new Dictionary<string, string>()
        {
            {ImgTextContentType.OutLink,"外部链接"},
            {ImgTextContentType.Navigation,"导航信息"},
            {ImgTextContentType.WeiActivity,"微活动"},
            {ImgTextContentType.BizType,"业务类型"},
            {ImgTextContentType.WeiCar,"微汽车"},
            {ImgTextContentType.WeiEstate,"微房产"} 
        };

        /// <summary>
        /// 根据key获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValueByKey(int key)
        {
            foreach (var item in typeList)
            {
                if (key.Equals(item.Key))
                {
                    return item.Value;
                }
            }
            return "暂无";
        }


    }

}
