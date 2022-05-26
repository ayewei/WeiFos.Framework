using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.Entity.WeiXin
{
    /// <summary>
    /// 微信调用接口凭证
    /// @author yewei 
    /// @date 2014-07-17
    /// </summary>
    public class WXQRCodeTicket
    {

        /// <summary>
        /// 获取的二维码ticket，凭借此ticket可以在有效时间内换取二维码。
        /// </summary>
        public string ticket { get; set; }

        /// <summary>
        /// 二维码的有效时间，以秒为单位。最大不超过1800
        /// </summary>
        public int expire_seconds { get; set; }

    }


}
