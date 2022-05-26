using System;
using System.Collections.Generic;
using System.Text;

namespace WeiFos.SDK.APIEntity
{
    // <summary>
    /// API 接口数据返回实体
    /// @author yewei 
    /// @date 2016-09-29
    /// </summary>
    [Serializable]
    public class APIResult<T> where T : struct, IConvertible
    {

        /// <summary>
        /// 基础消息
        /// </summary>
        public BaseData<T> Basis { get; set; }

        /// <summary>
        /// 返回结果集
        /// </summary>
        public dynamic Result { get; set; }

    }
}
