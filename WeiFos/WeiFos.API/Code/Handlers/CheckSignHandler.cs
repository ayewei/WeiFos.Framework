using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure; 
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks; 

namespace WeiFos.API.Code.Handlers
{

    /// <summary>
    /// ASP.NET Core 认证与授权
    /// @author yewei 
    /// @date 2019-06-13
    /// https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-2.2#authorization-handlers
    /// https://www.cnblogs.com/RainingNight/p/authorization-in-asp-net-core.html#iauthorizedata
    /// </summary>
    public class CheckSignRequirement : AuthorizationHandler<NameAuthorizationRequirement>, IAuthorizationRequirement
    {
        public CheckSignRequirement()
        { 
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, NameAuthorizationRequirement requirement)
        {
            return Task.CompletedTask;
        }
    }


    public class CheckSignHandler : AuthorizationHandler<CheckSignRequirement>
    {
        /// <summary>
        /// 验证方案提供对象
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }


        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="schemes"></param>
        public CheckSignHandler(IAuthenticationSchemeProvider schemes)
        {
            Schemes = schemes;
        }


        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CheckSignRequirement requirement)
        {
            //从AuthorizationHandlerContext转成HttpContext，以便取出表求信息
            var httpContext = (context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext).HttpContext;
            //请求Url
            var questUrl = httpContext.Request.Path.Value.ToLower();
            //判断请求是否停止
            var handlers = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            foreach (var scheme in await Schemes.GetRequestHandlerSchemesAsync())
            {
                var handler = await handlers.GetHandlerAsync(httpContext, scheme.Name) as IAuthenticationRequestHandler;
                if (handler != null && await handler.HandleRequestAsync())
                {
                    context.Fail();
                    return;
                }
            }

            //判断请求是否拥有凭据，即有没有登录
            var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenticate != null)
            {
                var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                //result?.Principal不为空即登录成功
                if (result?.Principal != null)
                {
                    httpContext.User = result.Principal;

                    //TO DO Permissions 
                    //----------------//

                    //判断过期时间
                    if (DateTime.Parse(httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration).Value) >= DateTime.Now)
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                    }
                    return;
                }
            } 

            context.Succeed(requirement);
        }
 

      
    }
}
