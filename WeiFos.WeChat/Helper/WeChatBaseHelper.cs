using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using WeiFos.WeChat.Models;
using WeiFos.WeChat.Models.OrgEntity;
using WeiFos.WeChat.TickeModule;

namespace WeiFos.WeChat.Helper
{
    /// <summary>
    /// 微信基础接口帮助类
    /// </summary>
    public class WeChatBaseHelper
    {


        #region 微信公众号相关接口


        /// <summary>
        /// 获取公众号的全局唯一票据AccessToken
        /// </summary>
        /// access_token是公众号的全局唯一票据，公众号调用各接口时都需使用access_token。
        /// 开发者需要进行妥善保存。access_token的存储至少要保留512个字符空间。
        /// access_token的有效期目前为2个小时，需定时刷新，重复获取将导致上次获取的access_token失效
        /// <param name="appid"></param>
        /// <param name="appsecret"></param>
        /// <returns></returns>
        public static AccessToken GetAccessToken(string appid, string appsecret)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
            url = string.Format(url, appid, appsecret);
            string returnData = HttpClientHelper.HttpGetAsyn(url);

            return JsonConvert.DeserializeObject<AccessToken>(returnData);
        }

        /// <summary>
        /// 删除微信自定义菜单
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static string DeleteWXMenu(string accessToken)
        {
            string posturl = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}";
            posturl = string.Format(posturl, accessToken);
            return PostURL(posturl);
        }


        /// <summary>
        /// 获取临时场景二维码票据
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="scene_id"></param>
        /// <returns></returns>
        public static string GetSceneTicket(string accessToken, long scene_id, long seconds)
        {
            return GetSceneTicket(accessToken, "QR_SCENE", scene_id.ToString(), seconds);
        }


        /// <summary>
        /// 获取临时场景二维码票据
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="scene_str"></param>
        /// <returns></returns>
        public static string GetSceneTicket(string accessToken, string scene_str, long seconds)
        {
            return GetSceneTicket(accessToken, "QR_STR_SCENE", scene_str, seconds);
        }


        /// <summary>
        /// 获取永久场景二维码票据
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="scene_id"></param>
        /// <returns></returns>
        public static string GetLimitSceneTicket(string accessToken, int scene_id)
        {
            return GetLimitSceneTicket(accessToken, "QR_LIMIT_SCENE", scene_id.ToString());
        }



        /// <summary>
        /// 获取永久场景二维码票据
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="scene_str"></param>
        /// <returns></returns>
        public static string GetLimitSceneTicket(string accessToken, string scene_str)
        {
            return GetLimitSceneTicket(accessToken, "QR_LIMIT_STR_SCENE", scene_str);
        }



        /// <summary>
        /// 获取微信二维码ticket
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="sceneType"></param>
        /// <param name="scene_id"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static string GetSceneTicket(string accessToken, string sceneType, string scene_val, long seconds)
        {
            string postData;
            if ("QR_SCENE".Equals(sceneType))
            {
                postData = "{\"action_name\": \"" + sceneType + "\", \"expire_seconds\": " + seconds + ", \"action_info\": {\"scene\": {\"scene_id\": " + long.Parse(scene_val) + "}}}";
            }
            else
            {
                postData = "{\"action_name\": \"" + sceneType + "\", \"expire_seconds\": " + seconds + ", \"action_info\": {\"scene\": {\"scene_str\": \"" + scene_val + "\"}}}";
            }

            return CreateSceneTicket(accessToken, postData);
        }



        /// <summary>
        /// 获取永久二维码
        /// scene_id   场景值ID，临时二维码时为32位非0整型，永久二维码时最大值为100000（目前参数只支持1--100000）
        /// scene_str  场景值ID（字符串形式的ID），字符串类型，长度限制为1到64
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="sceneType"></param>
        /// <param name="scene_val"></param>
        /// <returns></returns>
        public static string GetLimitSceneTicket(string accessToken, string sceneType, string scene_val)
        {
            string postData;
            if ("QR_LIMIT_SCENE".Equals(sceneType))
            {
                postData = "{\"action_name\": \"" + sceneType + "\", \"action_info\": {\"scene\": {\"scene_id\": " + long.Parse(scene_val) + "}}}";
            }
            else
            {
                postData = "{\"action_name\": \"" + sceneType + "\", \"action_info\": {\"scene\": {\"scene_str\": \"" + scene_val + "\"}}}";
            }
            return CreateSceneTicket(accessToken, postData);
        }






        /// <summary>
        /// 创建二维码
        /// </summary>
        /// <param name="accessToken"></param> 
        /// <param name="postData"></param>
        /// <returns></returns>
        private static string CreateSceneTicket(string accessToken, string postData)
        {
            string posturl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
            posturl = string.Format(posturl, accessToken);
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.UTF8;
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);

                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码
                return sr.ReadToEnd();
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                outstream.Close();
                outstream = null;
                instream.Close();
                instream = null;
                sr.Close();
                response.Close();
            }
        }


        /// <summary>
        /// 创建微信自定义菜单
        /// </summary>
        /// <param name="posturl"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string CreateWXMenu(string accessToken, string postData)
        {
            string posturl = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";
            posturl = string.Format(posturl, accessToken);
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.UTF8;
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);

                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码
                return sr.ReadToEnd();
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                outstream.Close();
                outstream = null;
                instream.Close();
                instream = null;
                sr.Close();
                response.Close();
            }
        }


        /// <summary>
        /// 上传图文消息素材
        /// </summary>
        /// <param name="path"></param>
        /// <param name="accessToken"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string UploadNews(string accessToken, string data)
        {
            string posturl = "https://api.weixin.qq.com/cgi-bin/media/uploadnews?access_token={0}";
            posturl = string.Format(posturl, accessToken);
            return HttpClientHelper.ClientRequest(posturl, data, "POST", null);
        }


        /// <SUMMARY> 
        /// 上传多媒体文件,返回 MediaId 
        /// </SUMMARY> 
        /// <PARAM name="ACCESS_TOKEN"></PARAM> 
        /// <PARAM name="Type"></PARAM> 
        /// <RETURNS></RETURNS> 
        public static string UploadMultimedia(string path, string accessToken, string type)
        {
            string result = string.Empty;
            string posturl = "http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}";
            posturl = string.Format(posturl, accessToken, type);

            string filepath = path;

            WebClient myWebClient = new WebClient();
            myWebClient.Credentials = CredentialCache.DefaultCredentials;

            try
            {
                byte[] responseArray = myWebClient.UploadFile(posturl, "POST", filepath);
                result = System.Text.Encoding.Default.GetString(responseArray, 0, responseArray.Length);
            }
            catch (Exception ex)
            {
                result = "Error:" + ex.Message;
            }

            return result;
        }


        public static string PostURL(string url)
        {
            try
            {

                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                return reader.ReadToEnd();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 根据分组进行群发
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string SendByGroup(string accessToken, string data)
        {
            string posturl = "https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token={0}";
            posturl = string.Format(posturl, accessToken);
            return HttpClientHelper.ClientRequest(posturl, data, "POST", null);
        }

        /// <summary>
        /// 根据OpenID列表群发
        /// 订阅号不可用，服务号认证后可用
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string SendByOpenIds(string accessToken, string data)
        {
            string posturl = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token={0}";
            posturl = string.Format(posturl, accessToken);
            return HttpClientHelper.ClientRequest(posturl, data, "POST", null);
        }

        #endregion


    }
}
