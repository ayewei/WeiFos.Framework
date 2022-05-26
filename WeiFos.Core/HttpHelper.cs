using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using WeiFos.Core.Extensions;
using System.Text; 

namespace WeiFos.Core
{
    /// <summary>
    /// Http帮助类
    /// </summary>
    public class HttpHelper
    {
        #region  Get请求
        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <returns></returns>
        public static string Get(string url)
        {
            return Send("GET", url, "");
        }
        #endregion

        #region Post请求
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">url请求</param>
        /// <param name="postData">参数</param>
        /// <returns></returns>
        public static string Post(string url, string postData)
        {
            return Send("POST", url, postData);
        }
        #endregion

        #region 发送请求
        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="verb">请求方式</param>
        /// <param name="url">url连接</param>
        /// <param name="postData">参数</param>
        /// <returns></returns>
        public static string Send(string verb, string url, string postData)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Timeout = 60 * 1000;
            req.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.110 Safari/537.36";
            req.Method = verb;
            try
            {
                if (verb == "POST")
                {
                    byte[] data = Encoding.UTF8.GetBytes(postData);
                    req.ContentType = "application/text; charset=utf-8";
                    req.ContentLength = data.Length;
                    Stream requestStream = req.GetRequestStream();
                    requestStream.Write(data, 0, data.Length);
                    requestStream.Close();
                }
                using (var res = req.GetResponse())
                {
                    using (var stream = res.GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                        string response = sr.ReadToEnd();
                        sr.Close();
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Post提交参数
        /// <summary>
        /// Post提交参数 1
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="postData">提交参数</param>
        /// <returns></returns>
        public static string RequestPost(string url, string postData)
        {
            string result = string.Empty;
            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                byte[] bs = Encoding.ASCII.GetBytes(postData);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = bs.Length;
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                    reqStream.Close();
                }
                using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        result = reader.ReadToEnd().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Post提交参数 2
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="postData">提交参数</param>
        /// <returns></returns>
        public static string PostUTF8(string url, string postData)
        {
            string result = string.Empty;
            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                byte[] bs = Encoding.UTF8.GetBytes(postData);
                req.Timeout = 60 * 1000;
                req.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.110 Safari/537.36";
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                req.ContentLength = bs.Length;
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                    reqStream.Close();
                }
                using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        result = reader.ReadToEnd().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Post提交参数 3
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="postData">提交参数</param>
        /// <returns></returns>
        public static string PostGBK(string url, string postData)
        {
            string result = string.Empty;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Timeout = 60 * 1000;
            req.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.110 Safari/537.36";
            req.Method = "POST";
            try
            {
                byte[] data = Encoding.GetEncoding("GBK").GetBytes(postData);
                req.ContentType = "application/text; charset=GBK";
                req.ContentLength = data.Length;
                Stream requestStream = req.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
                using (var res = req.GetResponse())
                {
                    using (var stream = res.GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                        result = sr.ReadToEnd();
                        sr.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// GZIPPost提交参数 4
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="postData">提交参数</param>
        /// <returns></returns>
        public static string RequestPostGZIP(string url, string postData)
        {
            string result = string.Empty;
            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                byte[] bs = Encoding.ASCII.GetBytes(postData);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = bs.Length;
                req.Headers["Accept-Encoding"] = "GZip";
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                    reqStream.Close();
                }
                using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        result = reader.ReadToEnd().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }
        #endregion

        #region 发送Https请求
        /// <summary>
        /// 发送Https请求
        /// </summary>
        /// <param name="verb">请求方式</param>
        /// <param name="url">url连接</param>
        /// <param name="postData">参数</param>
        /// <returns></returns>
        public static string GetHttps(string verb, string url, string postData)
        {
            //CertificatePolicy
            //ServicePointManager.CertificatePolicy = new MyPolicy();
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Timeout = 60 * 1000;
            req.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.110 Safari/537.36";
            req.Method = verb;
            try
            {
                if (verb == "POST")
                {
                    byte[] data = Encoding.UTF8.GetBytes(postData);
                    req.ContentType = "application/text; charset=utf-8";
                    req.ContentLength = data.Length;
                    Stream requestStream = req.GetRequestStream();
                    requestStream.Write(data, 0, data.Length);
                    requestStream.Close();
                }
                using (var res = req.GetResponse())
                {
                    using (var stream = res.GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                        string response = sr.ReadToEnd();
                        sr.Close();
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 模拟请求
        /// <summary>
        /// 模拟请求
        /// </summary>
        /// <param name="url">请求链接</param>
        /// <returns></returns>
        public static string GetRequest(string url)
        {
            string result = string.Empty;
            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Timeout = 2000;
                httpRequest.Method = "GET";
                HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                StreamReader sr = new StreamReader(httpResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                result = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }
        #endregion


        public static HttpResponseMessage ObjectToJson(Object obj)
        {
            String str;
            if (obj is String || obj is Char)
            {
                str = obj.ToString();
            }
            else
            {
                str = JsonConvert.SerializeObject(obj);
            }
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str, System.Text.Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }

        CookieContainer cookieContainer = new CookieContainer();

        public HttpHelper()
        {

        }

        public void AddCookie(Cookie cookie)
        {
            cookieContainer.Add(cookie);
        }

        public byte[] POST(Uri uri, byte[] postData = null)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                req.CookieContainer = cookieContainer;
                req.Timeout = 60 * 1000;
                req.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.110 Safari/537.36";
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = 0;
                if (postData != null && postData.Length > 0)
                {
                    req.ContentLength = postData.Length;
                    using (Stream reqStream = req.GetRequestStream())
                    {
                        reqStream.Write(postData, 0, postData.Length);
                        reqStream.Close();
                    }
                }
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();

                var stream = response.GetResponseStream();

                List<byte> list = new List<byte>();

                int len = 1024, read = 0;

                byte[] buffer = new byte[len];

                while ((read = stream.Read(buffer, 0, len)) > 0)
                {
                    list.AddRange(buffer.Take(read));
                }
                return list.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// POST
        /// <para>multipart/form-data</para>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">key:name,value:byte[]</param>
        /// <returns></returns>
        public byte[] PostwithMultipartData(Uri uri, Dictionary<string, string> fieldData = null)
        {
            try
            {
                string boundary = "----CustomBoundary" + DateTime.Now.Ticks.ToString();

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                req.CookieContainer = cookieContainer;
                req.Timeout = 60 * 1000;
                req.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.110 Safari/537.36";
                req.Method = "POST";
                req.ContentType = "multipart/form-data; boundary=" + boundary;
                req.ContentLength = 0;

                List<byte> list = new List<byte>();

                if (fieldData != null && fieldData.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    //发送字段数据
                    foreach (var item in fieldData)
                    {
                        sb = sb.Append("--" + boundary);
                        sb = sb.AppendLine();
                        sb = sb.AppendFormat("Content-Disposition: form-data; name=\"{0}\"", item.Key);
                        sb = sb.AppendLine();
                        sb = sb.AppendLine();
                        sb = sb.Append(item.Value);
                        sb = sb.AppendLine();
                    }
                    //数据结尾
                    sb.AppendLine("--" + boundary + "--");
                    string dataValue = sb.ToString();
                    byte[] fieldBuffer = Encoding.UTF8.GetBytes(dataValue);
                    list.AddRange(fieldBuffer);
                }
                if (list.Count > 0)
                {
                    req.ContentLength = list.Count;

                    using (Stream reqStream = req.GetRequestStream())
                    {
                        reqStream.Write(list.ToArray(), 0, list.Count);
                        reqStream.Close();
                    }
                }
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();

                var stream = response.GetResponseStream();

                List<byte> backData = new List<byte>();

                int len = 1024, read = 0;

                byte[] buffer = new byte[len];

                while ((read = stream.Read(buffer, 0, len)) > 0)
                {
                    backData.AddRange(buffer.Take(read));
                }
                return backData.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public byte[] GET(Uri uri)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                req.CookieContainer = cookieContainer;
                req.Timeout = 60 * 1000;
                req.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.110 Safari/537.36";
                req.Method = "GET";
                req.ContentType = "application/x-www-form-urlencoded";
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                var stream = response.GetResponseStream();

                List<byte> list = new List<byte>();

                int len = 1024, read = 0;

                byte[] buffer = new byte[len];

                while ((read = stream.Read(buffer, 0, len)) > 0)
                {
                    list.AddRange(buffer.Take(read));
                }
                return list.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

 

}
