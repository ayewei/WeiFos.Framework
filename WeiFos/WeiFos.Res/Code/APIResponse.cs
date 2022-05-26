using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Core.EnumHelper;
using WeiFos.Entity.Common;
using WeiFos.Entity.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WeiFos.Res.Code
{
    /// <summary>
    /// API 自定义响应
    /// @author yewei 
    /// @date 2015-10-10
    /// </summary>
    public class APIResponse
    {

        #region 单列模式  

        /*私有构造器，不能该类外部new对象*/
        private APIResponse()  { }

        private static APIResponse instance = null;
        public static APIResponse Instance
        {
            get { return instance = instance ?? new APIResponse(); }
        }

        #endregion


        /// <summary>
        /// 返回数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns> 
        public ContentResult ContentResult(StateCode code)
        {
            return new ContentResult() { Content = JsonConvert.SerializeObject(GetResult(code)), ContentType = "application/json" };
        }



        /// <summary>
        /// 返回数据
        /// 针对dynamic
        /// </summary>
        /// <param name="code"></param>
        /// <param name="Data"></param>
        /// <returns></returns> 
        public ContentResult ContentResult(StateCode code, dynamic Data)
        {
            return new ContentResult() { Content = JsonConvert.SerializeObject(GetResult(code, Data)), ContentType = "application/json" };
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