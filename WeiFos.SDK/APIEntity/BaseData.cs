using System;
using System.Collections.Generic;
using System.Text;

namespace WeiFos.SDK.APIEntity
{
    /// <summary>
    /// 版 本 WeiFos-Framework 微狐敏捷开发框架
    /// Copyright (c) 深圳微狐信息技术有限公司
    /// 创 建：yewei
    /// 日 期：2022/05/19
    /// 描 述：接口交互基础数据类型
    /// </summary>
    [Serializable]
    public class BaseData<T> where T : struct, IConvertible
    {
        public BaseData()
        {
            this.Msg = "";
            this.Sign = "";
        }
         

        /// <summary>
        /// 状态码
        /// </summary>
        public T Code { get; set; }


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
