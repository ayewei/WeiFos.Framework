using Newtonsoft.Json;
using WeiFos.Core.SettingModule;

namespace WeiFos.Core.Signature
{
    /// <summary>
    /// 签名帮助类
    /// @Author yewei 
    /// @date 2015-10-08
    /// </summary>
    public class SignHelper
    {

        /// <summary>
        /// MD5签名(默认key)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SignMD5(string str)
        {
            return SignMD5(str, ConfigManage.AppSettings<string>("AppSettings:EncryptKey"));
        }

        /// <summary>
        /// 默认key
        /// MD5签名(自定义key)
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string SignMD5(object t)
        {
            //序列化
            string t_json = JsonConvert.SerializeObject(t);

            //字符串加入key
            return SignMD5(string.Format("{0}{1}", ConfigManage.AppSettings<string>("AppSettings:EncryptKey"), t_json));
        }


        /// <summary>
        /// 自定义key 签名
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string SignMD5(string str, string key)
        {
            //字符串加入key，减1解码 (偏移量)
            //string tmpstr = StringHelper.DecryptStr(string.Format("{0}{1}", str, key));
            return StringHelper.MD5(string.Format("{0}{1}", key, str));
        }

        /// <summary>
        /// 自定义key 签名
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string SignMD5(object t, string key)
        {
            //序列化
            string t_json = JsonConvert.SerializeObject(t);

            //字符串加入key，减1解码 (偏移量)
            return SignMD5(string.Format("{0}{1}", t_json, key));
        }


        /// <summary>
        /// SHA-1签名(默认key)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SignSHA1(string str)
        {
            return SignSHA1(str, ConfigManage.AppSettings<string>("AppSettings:EncryptKey"));
        }


        /// <summary>
        /// SHA-1签名(自定义key)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string SignSHA1(string str, string key)
        {
            //字符串加入key，减1解码 (偏移量)
            string tmpstr = StringHelper.DecryptStr(string.Format("{0}{1}", str, key));
            return StringHelper.SHA1(tmpstr);
        }


    }
}