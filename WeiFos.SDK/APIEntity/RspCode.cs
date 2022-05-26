using System;
using System.Collections.Generic;
using System.Text;
using WeiFos.SDK.Attributes;

namespace WeiFos.SDK.APIEntity
{
    // <summary>
    /// API相应状态码
    /// @author yewei 
    /// @date 2022-04-29
    /// </summary>
    [Serializable]
    public abstract class RspCode
    {

        [StateCode(Describe = "验证通过")]
        public const int Code_0 = 0;

        [StateCode(Describe = "验证未通过")]
        public const int Code_1 = 1;

        [StateCode(Describe = "数据不存在")]
        public const int Code_2 = 2;

        [StateCode(Describe = "签名失败")]
        public const int Code_3 = 3;

        [StateCode(Describe = "断网")]
        public const int Code_4 = 4;

        [StateCode(Describe = "超时")]
        public const int Code_5 = 5;

        [StateCode(Describe = "未知端口号访问")]
        public const int Code_7 = 7;

        [StateCode(Describe = "部分成功")]
        public const int Code_199 = 199;

        [StateCode(Describe = "操作成功")]
        public const int Code_200 = 200;

        [StateCode(Describe = "操作失败")]
        public const int Code_500 = 500;


    }
}
