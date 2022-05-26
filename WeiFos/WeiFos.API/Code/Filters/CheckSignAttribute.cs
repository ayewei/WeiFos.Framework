using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.API.Controllers;
using WeiFos.Core.SettingModule;
using WeiFos.Entity.Enums; 
using WeiFos.SDK.Model;
using WeiFos.SDK.Sign;
using WeiFos.Service;
using WeiFos.Service.LogsModule; 

namespace WeiFos.API.Code.Filters
{
    /// <summary>
    /// 验签登录过滤器
    /// @author yewei 
    /// @date 2019-06-10
    /// 只能实现一个过滤器接口，要么是同步版本的，要么是异步版本的。如果你需要在接口中执行异步工作，那么就去实现异步接口。
    /// 否则应该实现同步版本的接口。框架会首先检查是不是实现了异步接口，如果实现了异步接口，那么将调用它。不然则调用同步接口的方法。
    /// 如果一个类中实现了两个接口，那么只有异步方法会被调用。最后，不管 action 是同步的还是异步的，过滤器的同步或是异步是独立于 action 的
    /// </summary>
    public class CheckSignAttribute : ActionFilterAttribute
    {

        /// <summary>
        /// 签名密钥
        /// </summary>
        private static string sign_secret = ConfigManage.AppSettings<string>("Jwt:Key");



        /// <summary>
        /// OnActionExecuting 的异步执行方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                //数据包
                string dyStr = context.ActionArguments["dy"].ToString();
                //动态运行时对象
                var dy = JsonConvert.DeserializeObject<dynamic>(dyStr);
                //调试模式
                if (ConfigManage.AppSettings<bool>("AppSettings:IsDebugModel"))
                {
                    ServiceIoc.Get<APILogsService>().Save(dyStr);
                }

                //post提交方式
                if ("post".Equals(context.HttpContext.Request.Method.ToLower()))
                {
                    if (context.HttpContext.User.Identity.IsAuthenticated)
                    {
                        string jwtToken = context.HttpContext.Request.Headers["Authorization"];
                        if (jwtToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                        {
                            jwtToken = jwtToken.Substring("Bearer ".Length).Trim();
                        }
                    }

                    //数据包
                    SignPackage Sign = JsonConvert.DeserializeObject<SignPackage>(dy.Global.ToString());

                    //签名校验
                    if (!WeiFosSign.SignAuth(sign_secret, dyStr))
                    {
                        if (!ConfigManage.AppSettings<bool>("AppSettings:IsDebugModel"))
                        {
                            context.Result = APIResponse.GetResult(StateCode.State_5);
                        }
                        else
                        {
                            context.Result = APIResponse.GetResult(StateCode.State_5, WeiFosSign.GetSignStr(sign_secret, dyStr));
                        }
                        return;
                    }
                }

                //继续执行
                await base.OnActionExecutionAsync(context, next);
            }
            catch (Exception ex)
            {
                ServiceIoc.Get<APILogsService>().Save("CheckSign==>" + ex.ToString());
                context.Result = APIResponse.GetResult(StateCode.State_500);
            }
        }




    }
}

