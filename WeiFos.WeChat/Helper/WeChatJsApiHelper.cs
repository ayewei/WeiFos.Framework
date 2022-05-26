﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WeiFos.Core;
using WeiFos.Core.Extensions;
using WeiFos.WeChat.EnumModule;
using WeiFos.WeChat.TickeModule;
using WeiFos.WeChat.WXAlgorithm;

namespace WeiFos.WeChat.Helper
{

    public class WeChatJsApiHelper
    {

        private static string GetConfigSign(string noncestr, string jsapi_ticket, string timestamp, string url = "")
        {
            Dictionary<string, string> paraMap = new Dictionary<string, string>();
            paraMap.Add("noncestr", noncestr);
            paraMap.Add("jsapi_ticket", jsapi_ticket);
            paraMap.Add("timestamp", timestamp);
            //url = string.IsNullOrEmpty(url) ? NHttpContext.Current.Request.GetAbsoluteUri().ToString().Split('#')[0] : url;
            paraMap.Add("url", url);
            string str = WXAlgorithmHelper.FormatBizQueryParaMapForUnifiedPay(paraMap);
            string result = StringHelper.SHA1(str); //MD5Helper.GetMD5(str);
            return result;
        }


        public static JsApiTicket GetJsApiTicket(string access_token)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi";
            url = string.Format(url, access_token);
            string returnData = HttpClientHelper.HttpGetAsyn(url);
            return JsonConvert.DeserializeObject<JsApiTicket>(returnData);
        }


        /// <summary>
        /// 微信js sdk 配置
        /// 返回
        /// wx.config()
        /// </summary>
        /// <param name="debug"></param>
        /// <param name="appid"></param>
        /// <param name="jsapi_ticket"></param>
        /// <param name="jsApiList"></param>
        /// <returns></returns>
        public static string GetConfig(bool debug, string appid, string jsapi_ticket, List<JsApiName> jsApiList)
        {
            return GetConfig(debug, appid, jsapi_ticket, jsApiList, false, "");
        }


        /// <summary>
        /// 微信js sdk 配置
        ///  url（当前网页的URL，不包含#及其后面部分） 
        /// 签名用的url必须是调用JS接口页面的完整URL。
        /// </summary>
        /// <param name="debug"></param>
        /// <param name="appid"></param>
        /// <param name="jsapi_ticket"></param>
        /// <param name="jsApiList"></param>
        /// <param name="is_config"></param>
        /// <returns></returns>
        public static string GetConfig(bool debug, string appid, string jsapi_ticket, List<JsApiName> jsApiList, bool is_config, string url = "")
        {
            string timestamp = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
            string noncestr = StringHelper.CreateNoncestr(16);
            string api = "[" + string.Join(",", jsApiList.Select(s => string.Format("'{0}'", s)).ToArray()) + "]";

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("debug", debug.ToString().ToLower());
            dictionary.Add("appId", "'" + appid + "'");
            dictionary.Add("timestamp", timestamp);
            dictionary.Add("nonceStr", "'" + noncestr + "'");
            dictionary.Add("signature", "'" + GetConfigSign(noncestr, jsapi_ticket, timestamp, url) + "'");
            dictionary.Add("jsApiList", api);
            var entries = dictionary.Select(d => string.Format("{0}: {1}", d.Key, d.Value));
            if (is_config)
            {
                return "{" + string.Join(",", entries.ToArray()) + "}";
            }
            return "wx.config({" + string.Join(",", entries.ToArray()) + "})";
        }



    }
}
