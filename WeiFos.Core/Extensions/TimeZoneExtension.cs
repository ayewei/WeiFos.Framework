using System;
using System.Collections.Generic;
using System.Text;

namespace WeiFos.Core.Extensions
{
    /// <summary>
    /// 版 本 Weifos-Framework 微狐敏捷开发框架
    /// Copyright (c) 2013-2022 深圳微狐信息科技有限公司
    /// 创建人：叶委
    /// 日 期：2017.03.04
    /// 描 述：在.NET Core使用TimeZone
    /// </summary>
    public static class TimeZoneExtension
    {

        /**/
        /// <summary>
        /// 取得某月的最后一天
        /// </summary>
        /// <param name="datetime">要取得月份最后一天的时间</param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(1).AddDays(-1);
        }

        /// <summary>  
        /// 将Unix时间戳格式转换为c# DateTime时间格式  
        /// </summary>  
        /// <param name="timeStamp">时间戳</param>  
        /// <returns>DateTime </returns>  
        public static DateTime ToServerLocalTime(this DateTime clientTime)
        {
            return TimeZoneInfo.ConvertTime(clientTime, TimeZoneInfo.Local);//等价的建议写法
        }


        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>double</returns>  
        public static int ConvertDateTimeInt(DateTime time)
        {
            DateTime startTime = TimeZoneInfo.ConvertTime(new System.DateTime(1970, 1, 1), TimeZoneInfo.Local);
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 获得和java中System.currentTimeMillis()相同的值
        /// </summary>
        /// <returns>long</returns>
        public static long CurrentTimeMillis()
        {
            DateTime jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)((DateTime.UtcNow - jan1st1970).TotalMilliseconds);

        }


        /// <summary>
        /// 将服务器时间转换为国际标准时间utc
        ///  @author yewei add by 2013-08-15
        /// </summary>
        /// <param name="dateTime">服务器时间如：DateTime.Now</param>
        /// <returns>国际标准时间UTC</returns>
        public static DateTime ConvertTimeToUtc(this DateTime clientTime)
        {
            return TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
        }


        /// <summary>
        /// 将国际标准时间utc转换为对应的其他时区时间
        ///  @author yewei add by 2013-08-15
        /// </summary>
        /// <param name="dateTime">utc时间</param>
        /// <param name="currentCompanyTimeZone">当前公司所在时区</param>
        /// <returns></returns>
        public static DateTime ConvertTimeFromUtc(DateTime dateTime, String currentCompanyTimeZone)
        {
            if (string.IsNullOrEmpty(currentCompanyTimeZone))
            {
                currentCompanyTimeZone = "Greenwich Standard Time";
            }
            TimeZoneInfo timezoneTo = TimeZoneInfo.FindSystemTimeZoneById(currentCompanyTimeZone);
            DateTime output = TimeZoneInfo.ConvertTimeFromUtc(dateTime, timezoneTo);
            return output;
        }

 


    }
}
