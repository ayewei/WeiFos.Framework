using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.ORM.Data.Const
{
    /// <summary>
    /// 签名包 实体类
    /// @author yewei 
    /// add by @date 2015-10-09
    /// </summary>
    [Serializable]
    public class ORMSignPackage
    {
        /// <summary>
        /// IP地址
        /// </summary>
        public string IP { get; set; } 

        /// <summary>
        /// 平台操作系统
        /// </summary>
        public int System { get; set; } 

        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 签名字符串
        /// </summary>
        public string Sign { get; set; }

    }
}
