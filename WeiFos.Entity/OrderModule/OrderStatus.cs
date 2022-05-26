using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.Entity.OrderModule
{
    /// <summary>
    /// 商城订单状态 
    /// @author yewei 
    /// @date 2014-03-25
    /// </summary>
    public class OrderStatus
    {
        /// <summary>
        /// 等待买家付款
        /// </summary>
        public const int WaitingPayment = 1;

        /// <summary>
        /// 部分付款
        /// </summary>
        public const int PaymentPart = 2;

        /// <summary>
        /// 买家已付款
        /// </summary>
        public const int PaymentsMade = 3;

        /// <summary>
        /// 买家退款
        /// </summary>
        public const int Refund = 4;

        /// <summary>
        /// 卖家已发货
        /// </summary>
        public const int Sent = 10;

        /// <summary>
        /// 买家退货
        /// </summary>
        public const int Return = 11;

        /// <summary>
        /// 交易成功
        /// </summary>
        public const int Success = 18;

        /// <summary>
        /// 交易关闭
        /// </summary>
        public const int Close = -1;


        public static Dictionary<int, string> statusList = new Dictionary<int, string>()
        {
            {OrderStatus.PaymentPart,"部分付款"},
            {OrderStatus.WaitingPayment,"等待买家付款"},
            {OrderStatus.Refund,"买家退款"},
            {OrderStatus.PaymentsMade,"买家已付款"},
            {OrderStatus.Sent,"卖家已发货"},
            {OrderStatus.Return,"退货"},
            {OrderStatus.Success,"交易成功"},
            {OrderStatus.Close,"交易关闭"}
        };

        /// <summary>
        /// 根据key获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValueByKey(int key)
        {
            foreach (var item in statusList)
            {
                if (key.Equals(item.Key))
                {
                    return item.Value;
                }
            }
            return "";
        }

    }
}
