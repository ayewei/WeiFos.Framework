using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WeiFos.Core;
using WeiFos.Entity.Enums;

namespace WeiFos.Res.Code.Attributes
{
    /// <summary>
    /// CheckUpload 控制器
    /// @author yewei 
    /// add by @date 2015-01-09
    /// </summary>
    public class CheckUpload : ActionFilterAttribute
    {
        //是否登录
        public bool IsLogin { get; set; }


        /// <summary>
        /// 签名验证
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            //加密字符串参数
            string ticket = actionContext.HttpContext.Request.Query["ticket"];
            if (string.IsNullOrEmpty(ticket)) {
                //actionContext.Response = APIResponse.toJson(StateCode.State_9000);
                return;
            };

            //解密字符串
            ticket = StringHelper.GetDecryption(ticket);
            if (ticket.IndexOf("#") != -1)
            {
                string s1 = ticket.Split('#')[0];
                string s2 = ticket.Split('#')[1];
                //业务类型
                string bizType = actionContext.HttpContext.Request.Query["bizType"];
                //业务ID
                string bizId = actionContext.HttpContext.Request.Query["bizId"];

                if (!s1.Equals(bizType) || !s2.Equals(bizId))
                {
                    //actionContext.Response = APIResponse.toJson(StateCode.State_9000);
                    return;
                }
            }
            else
            {
                //actionContext.Response = APIResponse.toJson(StateCode.State_9000);
                return;
            } 
        }



    }
}
