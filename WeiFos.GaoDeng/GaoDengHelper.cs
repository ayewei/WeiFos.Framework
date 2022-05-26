using Newtonsoft.Json;
using WeiFos.GaoDeng.APIModule;
using System;
using WeiFos.Core;
using WeiFos.Core.SettingModule;
using WeiFos.GaoDeng.SignModule;

namespace WeiFos.GaoDeng
{

    /// <summary>
    /// 高灯区块链电子发票统一服务接口
    /// @author yewei 
    /// @date 2019-04-08
    /// </summary>
    public class GaoDengHelper
    {



        /// <summary>
        /// 绑定商户号
        /// </summary>
        /// <param name="key"></param>
        /// <param name="appsecret"></param>
        /// <param name="content"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static dynamic BindMerchant(string key, string appsecret, string content, JsonSerializerSettings setting)
        {
            //
            GDMerchant gdMerchant = JsonConvert.DeserializeObject<GDMerchant>(content);
            //时间戳
            string timestamp = GDSignHelper.GetTimeStamp;
            //获取签名
            string signature = GDSignHelper.GetSign(gdMerchant, timestamp, key, appsecret);
            //接口地址
            string url = ConfigManage.AppSettings<string>("AppSettings:InvoiceApi") + "/merchant/bind?signature=" + signature + "&timestamp=" + timestamp + "&appkey=" + key + "&appsecret=" + appsecret;

            //请求接口
            //string responseData = HttpHelper.PostURL(url, JsonConvert.SerializeObject(gdMerchant, setting));
            string responseData = HttpHelper.Post(url, JsonConvert.SerializeObject(gdMerchant, setting));

            //返回
            return JsonConvert.DeserializeObject<dynamic>(responseData);
        }



    }

}
