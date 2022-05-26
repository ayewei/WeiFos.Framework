using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeiFos.Core.EnumHelper;
using WeiFos.Entity.Enums;

namespace WeiFos.API.Code
{
    /// <summary>
    /// API数据响应
    /// @author yewei 
    /// @date 2019-06-10
    /// </summary>
    public class APIResponse
    {


        /// <summary>
        /// 根据状态码返回
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static OkObjectResult GetResult(StateCode state)
        {
            APIResult result = new APIResult();
            result.Basis = new BaseData();
            result.Basis.State = state;
            return GetResult(result);
        }


        /// <summary>
        /// 状态码 加自定义数据
        /// </summary>
        /// <param name="state"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static OkObjectResult GetResult(StateCode state, dynamic data)
        {
            APIResult result = new APIResult();
            result.Basis = new BaseData();
            result.Basis.State = state;

            result.Result = data;
            return GetResult(result);
        }


        /// <summary>
        /// 根据状态码返回
        /// </summary>
        /// <param name="state"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static OkObjectResult GetResult(APIResult result)
        {
            if (result == null || result.Basis == null || result.Basis.State == 0) throw new Exception("返回数据异常");
            result.Basis.Msg = EnumHelper.GetEnumDescByValue(typeof(StateCode), (int)result.Basis.State);
            if (result.Result == null)
            {
                result.Result = new { };
            }
            return new OkObjectResult(result);
        }


     



    }
}
