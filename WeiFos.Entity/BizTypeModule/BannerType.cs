using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.Entity.BizTypeModule
{

    /// <summary>
    /// 广告图片类型
    /// @author yewei 
    /// @date 2013-07-24
    /// </summary>
    public class BannerType
    { 

        /// <summary>
        /// 移动端轮播广告图
        /// </summary>
        public const int AppBanner = 200; 

        /// <summary>
        /// 移动端首页底部栏目1
        /// </summary>
        public const int AppIndexColumn1 = 201;

        /// <summary>
        /// 移动端首页栏目Banner
        /// </summary>
        public const int AppIndexColumn2 = 202;

        /// <summary>
        /// 移动端首页栏目Banner
        /// </summary>
        public const int AppIndexColumn3 = 203;

        /// <summary>
        /// 移动端首页底部栏目Banner
        /// </summary>
        public const int AppIndexBotton1 = 211;
        

        public static Dictionary<int, string> BannerList = new Dictionary<int, string>()
        {
            {BannerType.AppBanner,"移动端—轮播广告图"},
            {BannerType.AppIndexColumn1,"移动端—首页栏目1"},
            {BannerType.AppIndexColumn2,"移动端—首页栏目2"},
            {BannerType.AppIndexColumn3,"移动端—首页栏目3"},
            {BannerType.AppIndexBotton1,"移动端—首页底部栏目"}
        };


        public static string GetValueBykey(int key)
        {
            foreach (var item in BannerList)
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

