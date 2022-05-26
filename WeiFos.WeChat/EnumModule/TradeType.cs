using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.WeChat.EnumModule
{
    /// <summary>
    /// 微信交易类型
    /// @author yewei 
    /// @date 2015-11-04
    /// </summary>
    public enum TradeType
    {

        /// <summary>
        /// JSAPI支付
        /// </summary>
        JSAPI,

        /// <summary>
        /// Native支付
        /// </summary>
        NATIVE,

        /// <summary>
        /// APP支付
        /// </summary>
        APP,

        /// <summary>
        /// 刷卡支付，刷卡支付有单独的支付接口
        /// 不调用统一下单接口
        /// </summary>
        MICROPAY
        
    }
}
