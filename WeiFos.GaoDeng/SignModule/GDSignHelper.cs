using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using WeiFos.Core;
using WeiFos.Core.Extensions;

namespace WeiFos.GaoDeng.SignModule
{

    /// <summary>
    /// 签名帮助类
    /// </summary>
    public static class GDSignHelper
    {

        /// <summary>
        /// 时间戳
        /// </summary>
        public static string GetTimeStamp
        {
            get { return ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString(); }
        }



        /// <summary>
        /// 签名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="timestamp"></param>
        /// <param name="key"></param>
        /// <param name="appsecret"></param>
        /// <returns></returns>
        public static string GetSign<T>(T t, string timestamp, string key, string appsecret)
        {
            //string str = JsonConvert.SerializeObject(t);
            //JObject obj = null;
            //if (!string.IsNullOrEmpty(str))
            //{
            //    obj = JObject.Parse(str);
            //}
            //return SignMD5(key, appsecret, JsonSort.SortJson(obj, null));

            Dictionary<string, string> bizObj = new Dictionary<string, string>();
            PropertyInfo[] propertys = t.GetType().GetProperties();

            foreach (PropertyInfo p in propertys)
            {
                string fieldName = p.Name;
                object val = p.GetValue(t, null);
                //if (val != null && val.ToString() != "")
                if (val != null)
                {
                    bizObj.Add(p.Name, val.ToString());
                }
            }

            //获取排序内容
            string content = DictionaryByAsc(bizObj);
            return SignMD5(timestamp, key, appsecret, content);
        }




        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="key"></param>
        /// <param name="appsecret"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string SignMD5(string timestamp, string key, string appsecret, string content)
        {
            if (string.IsNullOrEmpty(key)) throw new Exception("签名key不能为空");
            if (string.IsNullOrEmpty(content)) throw new Exception("签名内容不能为空");

            return MD5Helper.MD5Encrypt(key + timestamp + RFC3986Encoder.UrlEncode(content).Replace("+", "%20") + appsecret).ToUpper();
        }



        /// <summary>
        /// 对参数进行排序，按照key升序排列
        /// </summary>
        /// <param name="paraMap"></param>
        /// <returns></returns>
        public static string DictionaryByAsc(Dictionary<string, string> paraMap)
        {
            var buff = new List<string>();
            var result = from pair in paraMap orderby pair.Key select pair;
            foreach (var o in result)
            {
                if (string.IsNullOrEmpty(o.Key.Trim())) continue;
                buff.Add(string.Format("{0}={1}", o.Key.Trim(), o.Value.Trim()));
            }
            return string.Join("&", buff.ToArray());
        }



    }
}
