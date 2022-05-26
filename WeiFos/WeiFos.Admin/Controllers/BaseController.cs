using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using WeiFos.Core.EnumHelper;
using WeiFos.Entity.Common;
using WeiFos.Entity.Enums;

namespace WeiFos.Admin.Controllers
{


    public class BaseController : Controller
    {
        //业务ID
        public long bid = 0;


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.Query.ContainsKey("bid"))
            {
                long.TryParse(filterContext.HttpContext.Request.Query["bid"], out bid);
            }
            base.OnActionExecuting(filterContext);
        }


        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        /// <summary>
        /// 返回分页数据
        /// 针对普通数据集合
        /// </summary>
        /// <param name="state"></param>
        /// <param name="totalRow"></param>
        /// <param name="pageData"></param>
        /// <returns></returns> 
        protected JsonResult PageResult<T>(StateCode code, int totalRow, List<T> pageData)
        {
            return Json(GetResult(code, new { totalRow, pageData }));
        }



        /// <summary>
        /// 返回分页数据
        /// 针对DataTable
        /// </summary>
        /// <param name="code"></param>
        /// <param name="totalRow"></param>
        /// <param name="pageData"></param>
        /// <returns></returns> 
        protected ContentResult PageResult(StateCode code, int totalRow, DataTable pageData)
        {
            return Content(JsonConvert.SerializeObject(GetResult(code, new { totalRow, pageData })));
        }



        /// <summary>
        /// 返回Josn数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns> 
        protected ContentResult ContentResult(StateCode code)
        {
            return Content(JsonConvert.SerializeObject(GetResult(code, null)));
        }



        /// <summary>
        /// 返回数据
        /// 针对DataTable
        /// </summary>
        /// <param name="code"></param>
        /// <param name="Data"></param>
        /// <returns></returns> 
        protected ContentResult ContentResult(StateCode code, DataTable Data)
        {
            return Content(JsonConvert.SerializeObject(GetResult(code, Data)));
        }


        /// <summary>
        /// 返回数据
        /// 针对dynamic
        /// </summary>
        /// <param name="code"></param>
        /// <param name="Data"></param>
        /// <returns></returns> 
        protected ContentResult ContentResult(StateCode code, dynamic Data)
        {
            return Content(JsonConvert.SerializeObject(GetResult(code, Data)));
        }


        /// <summary>
        /// 获取返回信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected StateMessage GetResult(StateCode state)
        {
            return GetResult(state, "");
        }



        /// <summary>
        /// 获取返回信息
        /// </summary>
        /// <param name="state"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected StateMessage GetResult(StateCode state, object data)
        {
            //获取状态码信息
            string description = EnumHelper.GetEnumDescByValue(typeof(StateCode), state);

            //全局返回信息
            StateMessage stateMessage = new StateMessage();
            stateMessage.Code = state;
            stateMessage.Message = description;
            stateMessage.Data = data;
            return stateMessage;
        }





    }
}