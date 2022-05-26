using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using WeiFos.Core.SettingModule;
using WeiFos.SDK.Sign;

namespace WeiFos.SDK
{
    /// <summary>
    /// 签名
    /// </summary>
    public class SignClient
    {
        //默认30秒
        private static int TimeOut = 30000;

        /// <summary>
        /// Http异步GET
        /// </summary>
        /// <param name="url">请求URL地址</param>
        /// <returns></returns>
        public static string HttpGetAsyn(string url)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = httpClient.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            return null;
        }


        /// <summary>
        /// Http异步POST
        /// </summary>
        /// <param name="url">请求URL地址</param>
        /// <param name="postData">POST数据</param>
        /// <returns></returns>
        public static string PostAsynSign(string url, string key, string postData)
        {
            //获取签名数据
            string data = WeiFosSign.GetSignPackage(key, postData);
            //实例化请求对象
            return HttpPostAsyn(url, data);
        }


        /// <summary>
        /// Http异步POST
        /// </summary>
        /// <param name="url">请求URL地址</param>
        /// <param name="dyn">POST数据</param>
        /// <returns></returns>
        public static string PostAsynSign(string url, string key, dynamic dyn)
        {
            //获取签名数据
            string data = WeiFosSign.GetSignPackage(key, dyn);
            //实例化请求对象
            return HttpPostAsyn(url, data);
        }

        /// <summary>
        /// Http同步POST
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <param name="dyn"></param>
        /// <returns></returns>
        public static string PostSign(string url, string key, dynamic dyn)
        {
            //获取签名数据
            string data = WeiFosSign.GetSignPackage(key, dyn);
            //实例化请求对象
            return PostURL(url, data);
        }


        /// <summary>
        /// Http同步POST
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <param name="dyn"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static string PostSign(string url, string key, dynamic dyn, int timeOut)
        {
            //获取签名数据
            string data = WeiFosSign.GetSignPackage(key, dyn);
            //实例化请求对象
            return PostURL(url, data, timeOut);
        }


        /// <summary>
        /// Http异步POST返回字节流
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <param name="dyn"></param>
        /// <returns></returns>
        public static Stream PostAsynSignStream(string url, string key, dynamic dyn)
        {
            //获取签名数据
            string data = WeiFosSign.GetSignPackage(key, dyn);
            //实例化请求对象
            return HttpPostAsynStream(url, data);
        }

        /// <summary>
        /// Http同步POST返回字节流
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <param name="dyn"></param>
        /// <returns></returns>
        public static Stream PostSignStream(string url, string key, dynamic dyn)
        {
            //获取签名数据
            string data = WeiFosSign.GetSignPackage(key, dyn);
            //实例化请求对象
            return HttpPostStream(url, data, "POST", null);
        }


        /// <summary>
        /// 异步请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string HttpPostAsyn(string url, string postData)
        {
            try
            {
                //实例化请求对象
                HttpContent httpContent = new StringContent(postData, Encoding.UTF8);

                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {
                WeiFos.Core.Logs.Write("ex:" + ex.Message.ToString(), "MsmkLogger");
                return null;
            }
        }

        /// <summary>
        ///  Http异步POST返回字节流
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static Stream HttpPostAsynStream(string url, string postData)
        {
            try
            {
                //实例化请求对象
                HttpContent httpContent = new StringContent(postData, Encoding.UTF8);

                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStreamAsync().Result;
                }
                return null;
            }
            catch (Exception ex)
            {
                WeiFos.Core.Logs.Write("ex:" + ex.Message.ToString(), "MsmkLogger");
                return null;
            }
        }

        /// <summary>
        /// Http同步POST返回字节流
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="method"></param>
        /// <param name="cer"></param>
        /// <returns></returns>
        public static Stream HttpPostStream(string url, string data, string method, X509Certificate2 cer)
        {
            try
            {
                //请求对象
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method;
                //request.ContentType = "application/x-www-form-urlencoded";
                request.ContentType = "application/octet-stream";

                //SSL证书
                if (url.StartsWith("https") && cer != null)
                {
                    //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    request.ClientCertificates.Add(cer);
                }

                //如果存在数据
                if (!string.IsNullOrEmpty(data))
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(data);
                    request.ContentLength = buffer.Length;
                    Stream postData = request.GetRequestStream();
                    postData.Write(buffer, 0, buffer.Length);
                    postData.Close();
                }

                //响应对象
                var encoding = System.Text.Encoding.UTF8;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return response.GetResponseStream();
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// POST并接收返回数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string PostURL(string url)
        {
            return ClientRequest(url, string.Empty, "POST", TimeOut, null);
        }

        /// <summary>
        /// POST带数据并接收返回数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string PostURL(string url, string data)
        {
            return ClientRequest(url, data, "POST", TimeOut, null);
        }

        /// <summary>
        /// POST带数据并接收返回数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string PostURL(string url, string data, int timeOut)
        {
            return ClientRequest(url, data, "POST", timeOut, null);
        }

        /// <summary>
        /// POST带数据并接收返回数据
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="data">数据</param>
        /// <param name="cert_path">证书本地路径</param>
        /// <param name="cert_password">证书密码</param>
        /// <returns></returns>
        public static string PostURL(string url, string data, string certPath, string certPwd)
        {
            //X509Certificate2 cer = new X509Certificate2(certPath, certPwd, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
            X509Certificate2 cer = new X509Certificate2(certPath, certPwd, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
            return ClientRequest(url, data, "POST", TimeOut, cer);
        }


        /// <summary>
        /// Get接收返回数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetURL(string url)
        {
            return ClientRequest(url, string.Empty, "GET", TimeOut, null);
        }

        /// <summary>
        /// Get带数据并接收返回数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetURL(string url, string data)
        {
            return ClientRequest(url, data, "GET", TimeOut, null);
        }


        /// <summary>
        /// 默认为Get 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ClientRequest(string url, string data)
        {
            return ClientRequest(url, data, "GET", TimeOut, null);
        }


        /// <summary>
        /// 默认为Get 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static string ClientRequest(string url, string data, int timeOut)
        {
            return ClientRequest(url, data, "GET", timeOut, null);
        }


        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static string ClientRequest(string url, string data, string method, int timeOut, X509Certificate2 cer)
        {
            //请求对象
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            //request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = "application/json;charset=UTF-8";
            request.Timeout = timeOut;

            try
            {
                //SSL证书
                if (url.StartsWith("https") && cer != null)
                {
                    //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    request.ClientCertificates.Add(cer);
                }

                //如果存在数据
                if (!string.IsNullOrEmpty(data))
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(data);
                    request.ContentLength = buffer.Length;
                    Stream postData = request.GetRequestStream();
                    postData.Write(buffer, 0, buffer.Length);
                    postData.Close();
                }

                #region 处理超时

                //超时处理
                //https://docs.microsoft.com/zh-cn/dotnet/api/system.net.httpwebrequest.begingetresponse?view=net-6.0
                //IAsyncResult result = (IAsyncResult)request.BeginGetResponse(new AsyncCallback(RespCallback), myRequestState); 
                //ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), request, DefaultTimeout, true);

                //HttpWebRequest request = state as HttpWebRequest;
                //if (request != null)
                //{
                //    request.Abort();
                //}

                #endregion

                //响应对象
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8))
                    {
                        string str = sr.ReadToEnd().Trim();
                        sr.Close();
                        sr.Dispose();
                        return str;
                    }
                }
            }
            catch (WebException ex)
            {
                //取消对Internet资源的请求
                request.Abort();
                var rsp = request.GetResponse();
                if (rsp != null) rsp.Close();

                //WebExceptionStatus.NameResolutionFailure=>断网或者无法找到域名情况
                //WebExceptionStatus.Timeout=>请求超时的情况
                if (ConfigManage.AppSettings<bool>("AppSettings:SignClientErrorLog"))
                    WeiFos.Core.Logs.Write("SignClient响应状态码：" + ex.Status.ToString() + "，错误内容：" + ex.ToString());

                return ex.Status.ToString().ToLower();
            }
        }



        public static string HttpGet(string url)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            wc.Encoding = Encoding.UTF8;
            string returnText = wc.DownloadString(url);
            return returnText;
        }

        // 获取POST返回来的数据
        public static string PostInput(Stream stream)
        {
            try
            {
                System.IO.Stream s = stream;
                int count = 0;
                byte[] buffer = new byte[1024];
                StringBuilder builder = new StringBuilder();
                while ((count = s.Read(buffer, 0, 1024)) > 0)
                {
                    builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
                }
                s.Flush();
                s.Close();
                s.Dispose();
                return builder.ToString();
            }
            catch (Exception ex)
            { throw ex; }
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }


    }

}