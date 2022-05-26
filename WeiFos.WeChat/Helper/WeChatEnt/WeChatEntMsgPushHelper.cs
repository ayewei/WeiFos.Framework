using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.WeChat.Helper.WeChatEnt
{
    /// <summary>
    /// 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
    /// Copyright (c) 2013-2018 深圳微狐信息技术有限公司
    /// 创 建：叶委
    /// 日 期：2019-03-31 12:19:43
    /// 描 述：微信企业号消息推送
    /// </summary>
    public class WeChatEntMsgPushHelper
    {



        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string SendTmpMsg(string accessToken, string data)
        {
            string posturl = "https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}";
            posturl = string.Format(posturl, accessToken);
            return HttpClientHelper.PostURL(posturl, data);
        }





    }
}
