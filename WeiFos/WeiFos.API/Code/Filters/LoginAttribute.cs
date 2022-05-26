using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeiFos.API.Controllers;
using WeiFos.Core.SettingModule;
using WeiFos.Entity.Enums;
using WeiFos.Entity.UserModule;
using WeiFos.SDK.Model;
using WeiFos.Service;
using WeiFos.Service.LogsModule;
using WeiFos.Service.UserModule;

namespace WeiFos.API.Code.Filters
{

    /// <summary>
    /// 验签登录过滤器
    /// @author yewei 
    /// @date 2019-06-10
    /// </summary>
    /// <summary>
    /// 验签登录过滤器
    /// @author yewei 
    /// @date 2019-06-10
    /// </summary>
    public class LoginAttribute : ActionFilterAttribute
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
                //post提交方式
                if ("post".Equals(context.HttpContext.Request.Method.ToLower()))
                {
                    //是否需要登录
                    var Dynamic = JsonConvert.DeserializeObject<dynamic>(dyStr);
                    //序列化数据包
                    SignPackage Sign = JsonConvert.DeserializeObject<SignPackage>(Dynamic.Global.ToString());

                    if (string.IsNullOrEmpty(Sign.Token))
                    {
                        context.Result = APIResponse.GetResult(StateCode.State_211);
                        return;
                    }

                    UserToken token = ServiceIoc.Get<UserTokenService>().Get(Sign.Token);
                    if (token == null)
                    {
                        context.Result = APIResponse.GetResult(StateCode.State_205);
                        return;
                    }

                    if (!token.is_enable)
                    {
                        context.Result = APIResponse.GetResult(StateCode.State_205);
                        return;
                    }

                    //获取当前用户
                    User user = ServiceIoc.Get<UserService>().GetById(token.user_id);
                    if (user == null)
                    {
                        context.Result = APIResponse.GetResult(StateCode.State_223);
                        return;
                    }

                    if (context.ActionArguments.ContainsKey("user"))
                    {
                        context.ActionArguments["user"] = user;
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
