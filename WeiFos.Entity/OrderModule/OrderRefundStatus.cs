using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.Entity.OrderModule
{
    /// <summary>
    /// 定单退货退款状态
    /// </summary>
    public class OrderRefundStatus
    {

        /// <summary>
        /// 1已发起退款
        /// </summary>
        public const int Apply = 1;

        /// <summary>
        /// 2已拒绝
        /// </summary>
        public const int NoPass = 2;

        /// <summary>
        /// 5退款已通过
        /// </summary>
        public const int Pass = 5;


        public static Dictionary<int, string> statusList = new Dictionary<int, string>()
        {
            {OrderRefundStatus.Apply,"待处理"},
            {OrderRefundStatus.Pass,"已通过"},
            {OrderRefundStatus.NoPass,"已拒绝"}
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
