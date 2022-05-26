using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeiFos.Entity.Enums;

namespace WeiFos.API.Code
{

    // <summary>
    /// API 接口数据返回实体
    /// @author yewei 
    /// @date 2016-09-29
    /// </summary>
    [Serializable]
    public class APIResult
    {

        /// <summary>
        /// 基础消息
        /// </summary>
        public BaseData Basis { get; set; }

        /// <summary>
        /// 返回结果集
        /// </summary>
        public dynamic Result { get; set; }

    }


    [Serializable]
    public class BaseData
    {
        public BaseData()
        {
            this.Msg = "";
            this.Sign = "";
        }

        /// <summary>
        /// 状态码
        /// </summary>
        public StateCode State { get; set; }


        /// <summary>
        /// 签名字段 MD5（{Content}+Md5Key）
        /// </summary> 
        public string Sign { get; set; }


        /// <summary>
        /// 状态码返回消息说明
        /// </summary>
        public string Msg { get; set; }
    }


}