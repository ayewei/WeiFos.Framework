using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiZoon.Domain.ReplyModule
{
    /// <summary>
    /// 关键词操作类型 实体类
    /// @author yewei 
    /// @date 2013-10-16
    /// </summary>
    public static class KeyWordOpType
    {
        /// <summary>
        /// 普通关键字
        /// </summary>
        public const int Biz = 0;

        /// <summary>
        /// 关注时回复关键字
        /// </summary>
        public const int Attention = 1;

        /// <summary>
        /// 无匹配关键字
        /// </summary>
        public const int NoMacth = 2;
    }
}
