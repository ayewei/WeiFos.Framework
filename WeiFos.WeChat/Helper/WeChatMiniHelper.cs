using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WeiFos.WeChat.Models;
using WeiFos.WeChat.TickeModule;

namespace WeiFos.WeChat.Helper
{
    /// <summary>
    /// 微信开放平台接口帮助类
    /// @Author yewei 
    /// @Date 2018-04-13
    /// </summary>
    public class WeChatMiniHelper
    {



        /// <summary>
        /// 获取SessionKey
        /// </summary>
        /// <param name="appid">小程序 appId</param>
        /// <param name="secret">小程序 appSecret</param>
        /// <param name="js_code">登录时获取的 code</param>
        /// <returns></returns>
        public static string GetSessionKey(string appid, string secret, string js_code)
        {
            string posturl = "https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code";
            return HttpClientHelper.GetURL(string.Format(posturl, appid, secret, js_code));
        }




        /// <summary>
        /// 获取小程序二维码，适用于需要的码数量较少的业务场景。
        /// 通过该接口生成的小程序码，永久有效，有数量限制
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="path"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static byte[] CreateQRCode(string access_token, string path, int width)
        {
            string posturl = $"https://api.weixin.qq.com/cgi-bin/wxaapp/createwxaqrcode?access_token={access_token}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(posturl);
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";
            byte[] payload = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { path, width = 300 }));
            request.ContentLength = payload.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(payload, 0, payload.Length);
            writer.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //返回图片数据流
            Stream s = response.GetResponseStream();
            List<byte> bytes = new List<byte>();
            int temp = s.ReadByte();
            while (temp != -1)
            {
                bytes.Add((byte)temp);
                temp = s.ReadByte();
            }
            return bytes.ToArray();
        }




        /// <summary>
        /// 获取小程序二维码，适用于需要的码数量较少的业务场景。
        /// 通过该接口生成的小程序码，永久有效，有数量限制
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="path"></param>
        /// <param name="width"></param>
        /// <param name="auto_color">自动配置线条颜色，如果颜色依然是黑色，则说明不建议配置主色调</param>
        /// <param name="line_color">auto_color 为 false 时生效，使用 rgb 设置颜色 例如 {"r":"xxx","g":"xxx","b":"xxx"} 十进制表示</param>
        /// <param name="is_hyaline">是否需要透明底色，为 true 时，生成透明底色的小程序码</param>
        /// <returns></returns>
        public static byte[] GetQRCode(string access_token, string path, int width, bool auto_color, string line_color, bool is_hyaline)
        {
            string posturl = $"https://api.weixin.qq.com/wxa/getwxacode?access_token={access_token}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(posturl);
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";
            int r = int.Parse(line_color.Split('#')[0]);
            int g = int.Parse(line_color.Split('#')[1]);
            int b = int.Parse(line_color.Split('#')[2]);
            byte[] payload = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { path, width = 300, auto_color, line_color = new { r, g, b }, is_hyaline }));
            request.ContentLength = payload.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(payload, 0, payload.Length);
            writer.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //返回图片数据流
            Stream s = response.GetResponseStream();
            List<byte> bytes = new List<byte>();
            int temp = s.ReadByte();
            while (temp != -1)
            {
                bytes.Add((byte)temp);
                temp = s.ReadByte();
            }
            return bytes.ToArray();
        }




        /// <summary>
        /// 获取小程序二维码，适用于需要的码数量较少的业务场景。
        /// 通过该接口生成的小程序码，永久有效，无限制
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="scene">最大32个可见字符，只支持数字，大小写英文以及部分特殊字符：!#$&'()*+,/:;=?@-._~，其它字符请自行编码为合法字符（因不支持%，中文无法使用 urlencode 处理，请使用其他编码方式）</param>
        /// <param name="page">必须是已经发布的小程序存在的页面（否则报错），例如 pages/index/index, 根路径前不要填加 /,不能携带参数（参数请放在scene字段里），如果不填写这个字段，默认跳主页面</param>
        /// <param name="width"></param>
        /// <param name="auto_color">自动配置线条颜色，如果颜色依然是黑色，则说明不建议配置主色调</param>
        /// <param name="line_color">auto_color 为 false 时生效，使用 rgb 设置颜色 例如 {"r":"xxx","g":"xxx","b":"xxx"} 十进制表示</param>
        /// <param name="is_hyaline">是否需要透明底色，为 true 时，生成透明底色的小程序码</param>
        /// <returns></returns>
        public static byte[] GetUnlimited(string access_token, string scene, string page, int width, bool auto_color, string line_color, bool is_hyaline)
        {
            string posturl = $"https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token={access_token}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(posturl);
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";
            int r = int.Parse(line_color.Split('#')[0]);
            int g = int.Parse(line_color.Split('#')[1]);
            int b = int.Parse(line_color.Split('#')[2]);
            byte[] payload = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { scene, page, width = 300, auto_color, line_color = new { r, g, b }, is_hyaline }));
            request.ContentLength = payload.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(payload, 0, payload.Length);
            writer.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //返回图片数据流
            Stream s = response.GetResponseStream();
            List<byte> bytes = new List<byte>();
            int temp = s.ReadByte();
            while (temp != -1)
            {
                bytes.Add((byte)temp);
                temp = s.ReadByte();
            }
            return bytes.ToArray();
        }



        /// <summary>
        /// 获取小程序全局唯一后台接口调用凭据（access_token）
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static AccessToken GetAccessToken(string appid, string secret)
        {
            string posturl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
            string result = HttpClientHelper.GetURL(string.Format(posturl, appid, secret));
            return JsonConvert.DeserializeObject<AccessToken>(result);
        }




        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string SendTmpMsg(string accessToken, string data)
        {
            string posturl = "https://api.weixin.qq.com/cgi-bin/message/subscribe/send?access_token={0}";
            posturl = string.Format(posturl, accessToken);
            return HttpClientHelper.PostURL(posturl, data);
        }




    }
}
