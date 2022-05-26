using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.Entity.BizTypeModule
{


    /// <summary>
    /// 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
    /// Copyright (c) 2013-2018 深圳微狐信息技术有限公司
    /// 创 建：叶委
    /// 日 期：2019-08-23 12:22:24
    /// 描 述：支付方式
    /// </summary>
    public class PayMethod
    {


        #region 微信支付

        /// <summary>
        /// 11付款码支付
        /// </summary>
        public const int WeChat_Code = 11;

        /// <summary>
        /// 12 微信JSAPI支付
        /// </summary>
        public const int WeChat_JsApi = 12;

        /// <summary>
        /// 13 微信小程序支付
        /// </summary>
        public const int WeChat_Mini = 13;

        /// <summary>
        /// 14微信扫码支付
        /// </summary>
        public const int WeChat_Native = 14;

        /// <summary>
        /// 15微信APP支付
        /// </summary>
        public const int WeChat_App = 15;

        /// <summary>
        /// 16微信APP支付
        /// </summary>
        public const int WeChat_H5 = 16;

        #endregion


        #region 支付宝支付


        /// <summary>
        /// 21支付宝支付
        /// </summary>
        public const int AliPay = 21;


        #endregion


        /// <summary>
        /// 31电子钱包支付
        /// </summary>
        public const int E_Wallet = 31;

        /// <summary>
        /// 41储值卡支付
        /// </summary>
        public const int SV_Card = 41;

        /// <summary>
        /// 51现金支付
        /// </summary>
        public const int Cash = 51;

        /// <summary>
        /// 61刷卡支付（银行卡或信用卡）
        /// </summary>
        public const int Bank_Card = 61;

        /// <summary>
        /// 100混合支付
        /// </summary>
        public const int Mixed = 100;



        public static Dictionary<int, string> PayMethods = new Dictionary<int, string>()
        {
            { WeChat_Code,"微信—付款码支付"},
            { WeChat_JsApi,"微信—微信JSAPI支付"},
            { WeChat_Mini,"微信—小程序支付"},
            { WeChat_Native,"微信—原生支付"},
            { WeChat_App,"微信—APP支付"},
            { WeChat_H5,"微信—H5支付"},
            { AliPay,"支付宝支付"},
            { E_Wallet,"电子钱包支付"},
            { SV_Card,"储值卡支付"},
            { Cash,"现金支付"},
            { Bank_Card,"刷卡支付（银行卡或信用卡）"},
            { Mixed,"混合支付"}
        };


        public static string Get(int key)
        {
            foreach (var item in PayMethods)
            {
                if (key.Equals(item.Key))
                {
                    return item.Value;
                }
            }
            return "--";
        }


    }
}
