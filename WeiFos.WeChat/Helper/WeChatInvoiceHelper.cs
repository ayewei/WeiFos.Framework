using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.WeChat.Models.Invoice;
using WeiFos.WeChat.TickeModule;

namespace WeiFos.WeChat.Helper
{
    /// <summary>
    /// 微信电子发票帮助类
    /// @author yewei 
    /// @date 2019-05-15 
    /// </summary>
    public class WeChatInvoiceHelper
    {

        /// <summary>
        /// 获取授权页ticket
        /// 商户在调用授权页前需要先获取一个7200s过期的授权页ticket，
        /// 在获取授权页接口中，该ticket作为参数传入，加强安全性。
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static JsApiTicket GetTicket(string accessToken)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=wx_card", accessToken);
            string returnData = HttpClientHelper.HttpGetAsyn(url);
            return JsonConvert.DeserializeObject<JsApiTicket>(returnData);
        }



        /// <summary>
        /// 获取授权页链接
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string GetAuthUrl(string accessToken, InvoiceAuthUrl entity)
        {
            string posturl = string.Format("https://api.weixin.qq.com/card/invoice/getauthurl?access_token={0}", accessToken);
            return HttpClientHelper.ClientRequest(posturl, JsonConvert.SerializeObject(entity), "POST", null);
        }


    }
}
