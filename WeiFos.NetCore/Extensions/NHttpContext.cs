using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeiFos.NetCore.Extensions
{
    /// <summary>
    /// 版 本 Weifos-Framework 微狐敏捷开发框架
    /// Copyright (c) 2013-2022 深圳微狐信息科技有限公司
    /// 创建人：叶委
    /// 日 期：2019.01.18
    /// 描 述：请求上下文扩展
    /// </summary>
    public static class NHttpContext
    {

        /// <summary>
        /// 上下文访问器接口
        /// </summary>
        private static IHttpContextAccessor _accessor;

        //当前请求访问的上下文
        public static HttpContext Current => _accessor.HttpContext;


        internal static void Configure(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
    }


    /// <summary>
    /// 版 本 Weifos-Framework 微狐敏捷开发框架
    /// Copyright (c) 2013-2022 深圳微狐信息科技有限公司
    /// 创建人：叶委
    /// 日 期：2019.01.18
    /// 描 述：静态请求上下文
    /// </summary>
    public static class StaticHttpContextExtensions
    {

        /// <summary>
        /// 添加访问上下文扩展方法
        /// </summary>
        /// <param name="services"></param>
        public static void AddHttpContextAccessorExt(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        /// <summary>
        /// 放回应用构造器扩展方法
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder app)
        {
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            NHttpContext.Configure(httpContextAccessor);
            return app;
        }

    }



}
