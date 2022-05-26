using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Text;

namespace WeiFos.NetCore.Extensions
{
    /// <summary>
    /// 版 本 Weifos-Framework 微狐敏捷开发框架
    /// Copyright (c) 2013-2022 深圳微狐信息科技有限公司
    /// 创建人：叶委
    /// 日 期：2017.03.04
    /// 描 述：扩展 .net core HttpRequest
    /// </summary>
    public static class HttpRequestExtension
    {
         

        /// <summary>
        /// 获取当前请求路径
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetAbsoluteUri(this HttpRequest request)
        {
            return new StringBuilder()
                .Append(request.Scheme)
                .Append("://")
                .Append(request.Host)
                .Append(request.PathBase)
                .Append(request.Path)
                .Append(request.QueryString)
                .ToString();
        }


        /// <summary>
        /// 获取客户Ip
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetClientIp(this HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }


        /// <summary>
        /// 是否是ajax请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null) throw new ArgumentNullException("request");
            return (request.Headers["X-Requested-With"] == "XMLHttpRequest") || ((request.Headers != null) && (request.Headers["X-Requested-With"] == "XMLHttpRequest"));
        }


    }
}
