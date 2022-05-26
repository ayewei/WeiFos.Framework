using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WeiFos.Alibaba.Common;
using WeiFos.Core;
using WeiFos.Core.JsonHelper;

namespace WeiFos.Alibaba.Algorithm
{
    /// <summary>
    /// 阿里支付算法类
    /// @Author yewei 
    /// @Date 2016-11-14
    /// </summary>
    public class AliAlgorithmHelper
    {


        #region 签名处理模块



        /// <summary>
        ///  最后对请求字符串的所有一级value（biz_content作为一个value）进行encode，
        ///  编码格式按请求串中的charset为准，没传charset按UTF-8处理，获得最终的请求字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string GetRSASignUrlEncode<T>(T t, string charset)
        {
            Dictionary<string, string> paraMap = GetDictionaryByType(t);
            var buff = new List<string>();
            foreach (var o in paraMap)
            {
                if (string.IsNullOrEmpty(o.Key.Trim())) continue;
                if (string.IsNullOrEmpty(charset))
                {
                    buff.Add(string.Format("{0}={1}", o.Key.Trim(), o.Value.Trim()));
                }
                else
                {
                    //及时支付情况下没有处理 特殊符号，例如转义后的加号，APP支付情况下有处理特殊符号
                    string url_val = HttpUtility.UrlEncode(o.Value.Trim(), Encoding.GetEncoding(charset));
                    string val = StringHelper.UrlEncodeSymbolsReplace(url_val);
                    buff.Add(string.Format("{0}={1}", o.Key.Trim(), val));
                }
            }

            return string.Join("&", buff.ToArray());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string GetMD5SignUrlEncode<T>(T t, string charset)
        {
            Dictionary<string, string> paraMap = GetDictionaryByType(t);
            var buff = new List<string>();
            foreach (var o in paraMap)
            {
                if (string.IsNullOrEmpty(o.Key.Trim())) continue;
                if (string.IsNullOrEmpty(charset))
                {
                    buff.Add(string.Format("{0}={1}", o.Key.Trim(), o.Value.Trim()));
                }
                else
                {
                    buff.Add(string.Format("{0}={1}", o.Key.Trim(), HttpUtility.UrlEncode(o.Value.Trim(), Encoding.GetEncoding(charset))));
                }
            }

            return string.Join("&", buff.ToArray());
        }


        /// <summary>
        /// 获取RSA签名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetRSASign<T>(T t, string key)
        {
            return GetRSASign<T>(t, key, null);
        }


        /// <summary>
        /// 获取RSA签名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="key"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string GetRSASign<T>(T t, string key, string charset)
        {
            if (string.IsNullOrEmpty(key)) throw new SDKRuntimeException("签名key不能为空");

            //获取字典排序字符串
            string unSignParaString = FormatBizQueryParaMapForUnifiedPay(GetDictionaryByType(t));

            //获取签名
            return RSAFromPkcs8.sign(unSignParaString, key, charset);
        }



        /// <summary>
        /// 获取签名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetMD5Sign<T>(T t, string key)
        {
            return GetMD5Sign<T>(t, key, null);
        }




        #endregion



        #region 验签处理模块



        /// <summary>
        /// 根据表单数据返回数据集合
        /// </summary>
        /// <param name="request"></param>
        /// <param name="is_get"></param>
        /// <returns></returns>
        //public static SortedDictionary<string, string> GetRequestPost(HttpWebRequest request, bool is_get)
        //{
        //    SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
        //    NameValueCollection coll = is_get ? request.QueryString : request.Form;
        //    String[] requestItem = coll.AllKeys;
        //    for (int i = 0; i < requestItem.Length; i++)
        //    {
        //        sArray.Add(requestItem[i], is_get ? request.QueryString[requestItem[i]] : request.Form[requestItem[i]]);
        //    }

        //    return sArray;
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static T DeserializeDictionary<T>(IDictionary<string, string> datas) where T : new()
        {
            if (datas == null ) throw new SDKRuntimeException("序列化数据不能为空");

            T obj = new T();
            PropertyInfo[] propertys = obj.GetType().GetProperties();

            foreach (PropertyInfo p in propertys)
            {
                foreach (var v in datas)
                {
                    if (p.Name.Equals(v.Key))
                    { 
                        if (!string.IsNullOrEmpty(v.Value))
                        {
                            //通过反射 给 对象 obj 的相对应属性 赋值 p 代表类的某个属性， obj是类的实例，value 赋给属性的值
                            p.SetValue(obj, v.Value, null);
                        }
                    }
                }
            }

            return obj;
        }


        #endregion




        /// <summary>
        /// 获取签名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="key"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string GetMD5Sign<T>(T t, string key, string charset)
        {
            if (string.IsNullOrEmpty(key)) throw new SDKRuntimeException("支付密钥不能为空");

            Dictionary<string, string> bizObj = new Dictionary<string, string>();
            PropertyInfo[] propertys = t.GetType().GetProperties();

            foreach (PropertyInfo p in propertys)
            {
                string fieldName = p.Name;
                object val = p.GetValue(t, null);
                if (val != null && val.ToString() != "")
                {
                    bizObj.Add(p.Name, val.ToString());
                }
            }

            string unSignParaString = string.Empty;
            unSignParaString = FormatBizQueryParaMapForUnifiedPay(bizObj);

            return SignMD5(key, unSignParaString);
        }


        /// <summary>
        /// 获取阿里待签名字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string GetToSignStr<T>(T t, string charset)
        {
            //获取字典排序字符串
            return FormatBizQueryParaMapForUnifiedPay(GetDictionaryByType(t));
        }



        /// <summary>
        /// 参数名ASCII码从小到大排序（字典序）；
        /// 如果参数的值为空不参与签名；
        /// 参数名区分大小写；
        /// 验证调用返回或微信主动通知签名时，传送的sign参数不参与签名，将生成的签名与该sign值作校验。
        /// 微信接口可能增加字段，验证签名时必须支持增加的扩展字段
        /// </summary>
        /// <param name="paraMap"></param>
        /// <returns></returns>
        private static string FormatBizQueryParaMapForUnifiedPay(Dictionary<string, string> paraMap)
        {
            try
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
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }




        /// <summary>
        /// MD5字符串加Key加密
        /// </summary>
        /// <param name="key"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private static string SignMD5(string key, string content)
        {
            return SignMD5(key, content, null);
        }



        /// <summary>
        ///  MD5字符串加Key加密
        /// </summary>
        /// <param name="key"></param>
        /// <param name="content"></param>
        /// <param name="_input_charset"></param>
        /// <returns></returns>
        private static string SignMD5(string key, string content, string _input_charset)
        {
            if (string.IsNullOrEmpty(key)) throw new SDKRuntimeException("签名key不能为空");
            if (string.IsNullOrEmpty(content)) throw new SDKRuntimeException("签名内容不能为空");
            return MD5Helper.GetMD5(content + key, _input_charset).ToUpper();
        }



        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="paraMap"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSign(Dictionary<string, string> paraMap, string key)
        {
            string unSignParaString = FormatBizQueryParaMapForUnifiedPay(paraMap);
            return SignMD5(key, unSignParaString);
        }



        /// <summary>
        /// 根据类型获取对应字典集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDictionaryByType<T>(T t)
        {
            //JsonSerializerSettings jsetting = new JsonSerializerSettings();
            //jsetting.NullValueHandling = NullValueHandling.Ignore;
            ////反序列化数据包
            //string json_t = JsonConvert.SerializeObject(t, jsetting);
            //string new_sign = JsonSort.SortJson(JObject.Parse(json_t), null);

            Dictionary<string, string> bizObj = new Dictionary<string, string>();
            PropertyInfo[] propertys = t.GetType().GetProperties();
            foreach (PropertyInfo p in propertys)
            {
                string fieldName = p.Name;
                object val = p.GetValue(t, null);

                string[] types = { "Boolean", "SByte", "Byte", "UInt16", "Int16", "UInt32", "Int32", "UInt64", "Int64", "Char", "Single", "String", "Double", "Nullable`1" };
                if (!types.Contains(p.PropertyType.Name))
                {
                    JsonSerializerSettings jsetting = new JsonSerializerSettings();
                    jsetting.NullValueHandling = NullValueHandling.Ignore;
                    jsetting.DateFormatString = "yyyy-MM-dd hh:mm:ss";
                    bizObj.Add(fieldName, JsonConvert.SerializeObject(val, jsetting));
                }
                else
                {
                    if (val != null && val.ToString() != "")
                    {
                        bizObj.Add(fieldName, val.ToString());
                    }
                }
            }

            return bizObj;
        }





    }
}
