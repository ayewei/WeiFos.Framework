using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.SDK.Model
{
    /// <summary>
    /// 全局数据包
    /// </summary>
    [Serializable]
    public class GlobalPackage
    {

        /// <summary>
        /// 签名包
        /// </summary>
        public SignPackage Global { get; set; }

        /// <summary>
        /// 数据包
        /// </summary>
        public dynamic Data { get; set; }


    }
}
