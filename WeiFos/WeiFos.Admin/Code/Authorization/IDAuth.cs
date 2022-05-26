using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeiFos.Admin.Code.Authorization
{
    public class IDAuth : Attribute, IActionFilter
    {

        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }


        public void OnActionExecuting(ActionExecutingContext context)
        {
            //if (context.HttpContext.Request.IsAjaxRequest())
            //{
            //    string url = filterContext.RequestContext.HttpContext.Request.Url.ToString();
            //    string codes = UserInfo.GetArrayCodeByUrl(url);
            //    bool auth = false;
            //    foreach (var code in StringHelper.StringToArray(codes))
            //    {
            //        if (UserInfo.VerifyPermission(codes))
            //        {
            //            auth = true;
            //            break;
            //        }
            //    }

            //    if (!auth) filterContext.Result = new ContentResult() { Content = "XMLHttpRequest.PermissionDenied" };
            //}
        }



    }
}
