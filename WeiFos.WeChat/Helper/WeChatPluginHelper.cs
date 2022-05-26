using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.WeChat.Helper
{
    /// <summary>
    /// 微信支付接口帮助类
    /// @Author yewei 
    /// @Date 2015-12-17
    /// </summary>
    public class WeChatPluginHelper
    {

        /// <summary>
        /// 获取模板ID
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="template_id_short"></param>
        /// <returns></returns>
        public static string GetTemplateID(string accessToken, string template_id_short)
        {
            string data = "{ \"template_id_short\":\"" + template_id_short + "\"}";
            string posturl = "https://api.weixin.qq.com/cgi-bin/template/api_add_template?access_token={0}";
            posturl = string.Format(posturl, accessToken);
            return HttpClientHelper.PostURL(posturl, data);
        }


        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string SendTmpMsg(string accessToken, string data)
        {
            string posturl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
            posturl = string.Format(posturl, accessToken);
            return HttpClientHelper.PostURL(posturl, data);
        }



    }
}
