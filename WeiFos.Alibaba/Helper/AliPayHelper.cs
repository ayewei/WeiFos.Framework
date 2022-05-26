using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WeiFos.Core;

namespace WeiFos.Alibaba.Helper
{
    /// <summary>
    /// 阿里支付接口帮助类
    /// @Author yewei 
    /// @Date 2016-11-14
    /// </summary>
    public class AliPayHelper
    {

        /// <summary>
        /// 用于防钓鱼，调用接口query_timestamp来获取时间戳的处理函数
        /// 注意：远程解析XML出错，与IIS服务器配置有关
        /// </summary>
        /// <returns>时间戳字符串</returns>
        public static string Query_timestamp(string seller_id, string charset)
        {
            string url = "https://mapi.alipay.com/gateway.do?service=query_timestamp&partner=" + seller_id + "&_input_charset=" + charset;
            string encrypt_key = "";

            XmlTextReader Reader = new XmlTextReader(url);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Reader);

            encrypt_key = xmlDoc.SelectSingleNode("/alipay/response/timestamp/encrypt_key").InnerText;

            return encrypt_key;
        }



        /// <summary>
        /// 生成提交表单
        /// </summary>
        /// <param name="dicPara"></param>
        /// <param name="strMethod"></param>
        /// <param name="url"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string BuildRequest(Dictionary<string, string> dicPara, string strMethod, string url, string charset)
        {
            StringBuilder sbHtml = new StringBuilder();
            sbHtml.Append("<form id='alipaysubmit' name='alipaysubmit' action='" + url + "_input_charset=" + charset + "' method='" + strMethod.ToLower().Trim() + "'>");
            foreach (KeyValuePair<string, string> temp in dicPara)
            {
                sbHtml.Append("<input type='hidden' name='" + temp.Key + "' value='" + temp.Value + "'/>");
            }
            sbHtml.Append("<input type='submit' style='display:none;'></form>");
            sbHtml.Append("<script>document.forms['alipaysubmit'].submit();</script>");
            return sbHtml.ToString();
        }



        /// <summary>
        /// 获取是否是支付宝服务器发来的请求的验证结果
        /// </summary>
        /// <param name="notify_id"></param>
        /// <param name="_partner"></param>
        /// <returns></returns>
        public static string GetResponseTxt(string notify_id, string _partner)
        {
            string veryfy_url = "https://mapi.alipay.com/gateway.do?service=notify_verify&partner=" + _partner + "&notify_id=" + notify_id;
            return HttpHelper.Get(veryfy_url);
        }


        /// <summary>
        /// 应用授权URL拼装
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="redirect_uri"></param>
        /// <returns></returns>
        public static string GetOuthUrl(string appid, string redirect_uri)
        {
            string oauthURL = "https://openauth.alipay.com/oauth2/appToAppAuth.htm?app_id={0}&app_auth_code={1}";
            return string.Format(oauthURL, appid, System.Web.HttpUtility.UrlEncode(redirect_uri).Replace("+", "%20"));
        }


    }
}
