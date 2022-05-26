using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using WeiFos.Core.Extensions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeiFos.NetCore.Extensions;

namespace WeiFos.Admin.Code.Authorization
{

    /// <summary>
    /// Copyright (c) 2013-2018 深圳微狐信息科技有限公司
    /// 描 述：登录验证
    /// 创建人：叶委
    /// 日 期：2019.01.10
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class LoginAuth : Attribute, IAuthorizationFilter, IActionFilter
    {


        /// <summary>
        /// 身份认证
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext.Result != null) return;

            //如果未登录
            if (LoginUser.Instance.IsLogin(filterContext.HttpContext)) return;

            //如果不是异步请求
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                string url = AppGlobal.Admin;
                url = string.Concat(url, "?returnUrl=", filterContext.HttpContext.Request.Path);

                RedirectResult redirectResult = new RedirectResult(url);
                filterContext.Result = redirectResult;
                return;
            }
            else
            {
                filterContext.Result = new ContentResult() { Content = "XMLHttpRequest.LoginOut" };
            }
        }



        /// <summary>
        /// Action方法注入参数
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.ContainsKey("user"))
            {
                context.ActionArguments["user"] = LoginUser.Instance.User;
            }
        }



        public void OnActionExecuted(ActionExecutedContext context)
        {
           
        }




    }
}
