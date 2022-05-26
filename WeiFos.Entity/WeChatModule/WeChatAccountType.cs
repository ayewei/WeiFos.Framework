using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.Entity.WeChat
{
    /// <summary>
    /// 微信公众号类型
    /// @author arvin 
    /// @date 2014-02-11
    /// </summary>
    public static class WeChatAccountType
    {
        /// <summary>
        /// 订阅号
        /// </summary>
        public const int Subscriber = 1;

        /// <summary>
        /// 认证订阅号
        /// </summary>
        public const int AuthSubscriber = 2;

        /// <summary>
        /// 服务号
        /// </summary>
        public const int Service = 3;

        /// <summary>
        /// 认证服务号
        /// </summary>
        public const int AuthService = 4; 


        public static Dictionary<int, string> accountTypeList = new Dictionary<int, string>()
        {
            {WeChatAccountType.Subscriber,"订阅号"},
            {WeChatAccountType.AuthSubscriber,"认证订阅号"},
            {WeChatAccountType.Service,"服务号"},
            {WeChatAccountType.AuthService,"认证服务号"}
        };

        /// <summary>
        /// 根据key获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValueByKey(int key)
        {
            foreach (var item in accountTypeList)
            {
                if (key.Equals(item.Key))
                {
                    return item.Value;
                }
            }
            return "暂无";
        }


    }
}
