using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using WeiFos.Core;
using WeiFos.Core.JsonHelper;
using WeiFos.Core.Signature;
using WeiFos.SDK.Model;

namespace WeiFos.SDK.Sign
{
    /// <summary>
    /// AlionSign 签名实体
    /// @author yewei 
    /// add by @date 2016-11-08
    /// </summary> 
    public class WeiFosSign
    { 

        /// <summary>
        /// 获取32字节码 sha1加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Get32BitSHA1(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            return StringHelper.ConvertTo32BitSHA1(str);
        }



        /// <summary>
        /// 获取验签字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataPackage"></param>
        /// <returns></returns>
        public static string GetSignStr(string key, string dataPackage)
        {
            GlobalPackage globalPackage = JsonConvert.DeserializeObject<GlobalPackage>(dataPackage);
             
            globalPackage.Global.Sign = "";
            string str = JsonConvert.SerializeObject(globalPackage);
            JObject obj = null;
            if (!string.IsNullOrEmpty(str))
            {
                obj = JObject.Parse(str);
            }

            //重新签名
            return SignHelper.SignMD5(key, JsonSort.SortJson(obj, null));
        }


        /// <summary>
        /// 获取请求提交数据签名包
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dyn_data"></param>
        /// <returns></returns>
        public static string GetSignPackage(string key, dynamic dyn_data)
        {
            //序列化默认配置
            JsonConvert.DefaultSettings = () =>
            {
                return new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver(),
                    DateFormatString = "yyyy-MM-dd HH:mm:ss.fff",
                    Formatting = Formatting.None,
                };
            };
            var jsonSetting = JsonConvert.DefaultSettings();

            string data = JsonConvert.SerializeObject(dyn_data, jsonSetting);
            return GetSignPackage(key, null, data);
        }


        /// <summary>
        /// 获取请求提交数据签名包
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetSignPackage(string key, string data)
        {
            return GetSignPackage(key, null, data);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="signPackage"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetSignPackage(string key, SignPackage signPackage, string data)
        {
            //全局数据包
            GlobalPackage globalPackage = new GlobalPackage();

            if (signPackage == null) signPackage = new SignPackage();

            //签名包
            globalPackage.Global = signPackage;
            //数据包
            globalPackage.Data = JsonConvert.DeserializeObject<dynamic>(data);

            string str = JsonConvert.SerializeObject(globalPackage);
            JObject obj = null;
            if (!string.IsNullOrEmpty(str))
            {
                obj = JObject.Parse(str);
            }

            //将数据包签名
            globalPackage.Global.Sign = SignHelper.SignMD5(key, JsonSort.SortJson(obj, null));
            return JsonConvert.SerializeObject(globalPackage);
        }



        /// <summary>
        /// 验签
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool SignAuth(string key, string data)
        {
            GlobalPackage globalPackage = JsonConvert.DeserializeObject<GlobalPackage>(data);

            string sign = globalPackage.Global.Sign;
            globalPackage.Global.Sign = "";

            string str = JsonConvert.SerializeObject(globalPackage);
            JObject obj = null;
            if (!string.IsNullOrEmpty(str))
            {
                obj = JObject.Parse(str);
            }

            //重新签名
            string new_sign = SignHelper.SignMD5(key, JsonSort.SortJson(obj, null));

            //签名是否一致
            return new_sign.ToLower().Equals(sign.ToLower());
        }



    }
}
