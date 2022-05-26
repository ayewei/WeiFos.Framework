using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using WeiFos.Core;
using WeiFos.WeChat.Common;
using WeiFos.WeChat.Models;

namespace WeiFos.WeChat.WXAlgorithm
{
    /// <summary>
    /// 微信支付算法类
    /// @Author yewei 
    /// @Date 2015-11-02
    /// </summary>
    public class WXAlgorithmHelper
    {

        /// <summary>
        /// 时间戳
        /// </summary>
        public static string GetTimeStamp
        {
            get { return ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString(); }
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
        public static string FormatBizQueryParaMapForUnifiedPay(Dictionary<string, string> paraMap)
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
                throw new SDKRuntimeException(e.Message);
            } 
        }


        /// <summary>
        /// MD5字符串加Key加密
        /// </summary>
        /// <param name="key"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string SignMD5(string key, string content)
        {
            if (string.IsNullOrEmpty(key)) throw new SDKRuntimeException("财付通签名key不能为空");
            if (string.IsNullOrEmpty(content)) throw new SDKRuntimeException("财付通签名内容不能为空");
            //string str = MD5Helper.GetMD5(content + "&key=" + key).ToUpper();
            return MD5Helper.MD5Encrypt(content + "&key=" + key).ToUpper();
        }
  
        
        /// <summary>
        /// 获取签名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="paySecretKey">支付密钥</param>
        /// <returns></returns>
        public static string GetSign<T>(T t, string paySecretKey)
        {
            if (string.IsNullOrEmpty(paySecretKey)) throw new SDKRuntimeException("支付密钥不能为空");

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

            string unSignParaString = FormatBizQueryParaMapForUnifiedPay(bizObj);
            return SignMD5(paySecretKey, unSignParaString);
        }


        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="paraMap"></param>
        /// <param name="paySecretKey"></param>
        /// <returns></returns>
        public static string GetSign(Dictionary<string, string> paraMap, string paySecretKey)
        {
            string unSignParaString = FormatBizQueryParaMapForUnifiedPay(paraMap);
            return SignMD5(paySecretKey, unSignParaString);
        }



    }
}
