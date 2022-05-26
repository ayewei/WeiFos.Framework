using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.SDK.Model
{
    /// <summary>
    /// 签名包 实体类
    /// @author yewei 
    /// add by @date 2015-10-09
    /// </summary>
    [Serializable]
    public class SignPackage
    {
        public SignPackage()
        {
            IP = "";
            IMEI = "";
            IMSI = "";
            OS = 0;
            Token = "";
            Sign = "";
        }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 手机IMEI号
        /// </summary>
        public string IMEI { get; set; }

        /// <summary>
        /// 手机IMSI号
        /// </summary>
        public string IMSI { get; set; }

        /// <summary>
        /// 2:android，4:IOS, 6：ERP
        /// </summary>
        public int OS { get; set; }

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
