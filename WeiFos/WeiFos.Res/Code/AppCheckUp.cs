using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WeiFos.Core;
using WeiFos.Core.SettingModule;
using WeiFos.Entity.Enums;
using WeiFos.Service;
using WeiFos.Service.LogsModule;

namespace WeiFos.Res.Code
{
    /// <summary>
    /// App上传票据校验
    /// @author yewei 
    /// @date 2016-11-08
    /// </summary>
    public class AppCheckUp : ActionFilterAttribute
    {


        /// <summary>
        /// 签名验证
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            try
            {
                base.OnActionExecuting(actionContext);
                //加密字符串参数   
                string ticket = actionContext.HttpContext.Request.Query["ticket"];
                //业务类型
                string bizType = actionContext.HttpContext.Request.Query["bizType"];
                //业务ID
                string bizId = actionContext.HttpContext.Request.Query["bizId"];

                if (!string.IsNullOrEmpty(ticket) && !string.IsNullOrEmpty(bizType) && !string.IsNullOrEmpty(bizId))
                {
                    string val = bizType + "#" + bizId + ConfigManage.AppSettings<string>("DataBase:EncryptKey");
                    if (!ticket.ToUpper().Equals(StringHelper.ConvertTo32BitSHA1(val).ToUpper()))
                    {
                        //actionContext.Response = APIResponse.toJson(StateCode.State_9000);
                        return;
                    }
                }
                else
                {
                    //actionContext.Response = APIResponse.toJson(StateCode.State_9001);
                    return;
                }
            }
            catch (Exception ex)
            {
                ServiceIoc.Get<APILogsService>().Save("AppCheckUp==>" + ex.ToString());
            }

            return;
        }



    }
}