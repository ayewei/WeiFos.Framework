using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeiFos.API.Code;
using WeiFos.API.Code.Filters;
using WeiFos.Entity.Enums;
using WeiFos.Entity.UserModule;
using WeiFos.Service;
using WeiFos.Service.LogsModule;

namespace WeiFos.API.Controllers
{

    public class UserController : BaseController
    {

        /// <summary>
        /// 用户接口入口
        /// </summary>
        /// <param name="dy"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [LoginAttribute]
        public async Task<IActionResult> Index(User user, [FromBody] dynamic dy, int id)
        {
            switch (id)
            {
                case 300://
                    return await Func300(user, dy);

                //默认返回失败
                default: return Ok(APIResponse.GetResult(StateCode.State_6));
            }
        }



        #region 300 立即收藏


        /// <summary>
        /// 收藏商品
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Func300(User user, dynamic dy)
        {
            return await Task.Run(() =>
            {
                try
                {
                    //店铺id
                    int store_id = int.Parse(dy.Data.StoreID.ToString());

                    return APIResponse.GetResult(StateCode.State_200);
                }
                catch (Exception ex)
                {
                    ServiceIoc.Get<APILogsService>().SaveError("立即收藏[Func300]==>" + ex.ToString());
                    return APIResponse.GetResult(StateCode.State_500);
                }
            });
        }


        #endregion




    }
}