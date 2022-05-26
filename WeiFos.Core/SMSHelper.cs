using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace WeiFos.Core
{
    /// <summary>
    /// @Author yewei 
    /// 字符处理对象
    /// </summary>
    public  class SMSHelper
    {

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="sname">提交账户</param>
        /// <param name="spwd">提交密码</param>
        /// <param name="scorpid">企业代码（扩展号，可空着）</param>
        /// <param name="sprdid">产品编号</param>
        /// <param name="receive_mobile">接收号码，每次只能提交1个号码</param>
        /// <param name="smsg">信息内容,通常为70汉字以内，具体由平台内部决定</param>
        public static void Send(string sname, string spwd, string scorpid, string sprdid, string receive_mobile, string smsg, string posturl)
        {
            string postStrTpl = "sname={0}&spwd={1}&scorpid={2}&sprdid={3}&sdst={4}&smsg={5}";

            UTF8Encoding encoding = new UTF8Encoding();
            byte[] postData = encoding.GetBytes(string.Format(postStrTpl, sname, spwd, scorpid, sprdid, receive_mobile, smsg));

            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(posturl);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = postData.Length;

            Stream newStream = myRequest.GetRequestStream();
            newStream.Write(postData, 0, postData.Length);
            newStream.Flush();
            newStream.Close();

            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            if (myResponse.StatusCode == HttpStatusCode.OK)
            {
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string dd = reader.ReadToEnd();
            }
            else
            {
                //访问失败
            }
        }


        /// <summary>
        /// 短信发送
        /// </summary>
        /// <param name="mob">手机号码</param>
        /// <param name="msg">发送内容</param>
        /// <returns></returns>
        public static string GetHtmlFromUrl(string mob, string msg)
        {
            string strRet = null;
            string strServer = "SMSServer";
            string strUid = "SMSUid";
            string strKey = "SMSKey";
            msg = "SMSSendMsg";
            string url = string.Format("{0}?Uid={1}&Key={2}&smsMob={3}&smsText={4}", strServer, strUid, strKey, mob, msg);
            if (url == null || url.Trim().ToString() == "")
            {
                return strRet;
            }
            string targeturl = url.Trim().ToString();
            try
            {
                HttpWebRequest hr = (HttpWebRequest)WebRequest.Create(targeturl);
                hr.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";
                hr.Method = "GET";
                hr.Timeout = 30 * 60 * 1000;
                WebResponse hs = hr.GetResponse();
                Stream sr = hs.GetResponseStream();
                StreamReader ser = new StreamReader(sr, Encoding.Default);
                strRet = ser.ReadToEnd();
            }
            catch
            {
                strRet = null;
            }
            return strRet;
        }

    }
}
