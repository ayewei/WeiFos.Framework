using System.Collections.Generic;
using WeiFos.Entity.ReplyModule;
using WeiFos.Entity.BizTypeModule;
using WeiFos.Entity.WeChatModule;
using WeiFos.ORM.Data;
using WeiFos.Service.ResourceModule;
using WeiFos.WeChat.Helper;
using WeiFos.WeChat.WXBase;
using WeiFos.WeChat.TickeModule;
using Newtonsoft.Json;
using WeiFos.Entity.ResourceModule;

namespace WeiFos.Service.WeChatModule
{
    /// <summary>
    /// 高级群发 service
    /// @author yewei 
    /// @date 2014-12-27
    /// </summary>
    public class WXSendService
    {



        /// <summary>
        /// 根据分组群发
        /// 发送图文消息
        /// </summary>
        /// <param name="gruop_id"></param>
        /// <param name="account"></param>
        /// <param name="WeChatAccount"></param>
        /// <param name="imgTextReplys"></param>
        /// <returns></returns>
        public string SendMpNewsByGroup(int gruop_id, WeChatAccount account, List<ImgTextReply> imgTextReplys)
        {
            //微信群发图文集合
            List<WXArticles> wxArticles = new List<WXArticles>();
             
            //获取token
            AccessToken token = WeChatBaseHelper.GetAccessToken(account.appid, account.app_secret);

            //群发推送的图文消息
            foreach (ImgTextReply i in imgTextReplys)
            {
                //获取当前图文消息
                Img img = ServiceIoc.Get<ImgService>().GetImg(ImgType.ImgTextReply_Title, i.id);

                //图片物理路径
                string path = img.getPath();

                //path = @"D:\szbook\SZBook\SZBook.Resource\DefaultRes\Image\default-activity-scratch-card-start.jpg";
                
                //将图片上传到微信服务器
                string backmsg = WeChatBaseHelper.UploadMultimedia(path, token.access_token, WXReqMsgType.image);
                WXUploadMedia wxUploadMedia = JsonConvert.DeserializeObject<WXUploadMedia>(backmsg);

                WXArticles wxArticle = new WXArticles();

                wxArticle.title = i.title;
                //wxArticle.content_source_url = MsgContentType.GetLinkByType(account, sid, i.content_type, "", i.content_value);
                wxArticle.content = i.details;
                wxArticle.digest = i.introduction;
                wxArticle.show_cover_pic = 1;
                wxArticle.thumb_media_id = wxUploadMedia.media_id;
                wxArticle.author = "";

                wxArticles.Add(wxArticle);
            }
            
            //序列化发送图文消息
            string json_articles = JsonConvert.SerializeObject(wxArticles);

            //拼装发送数据
            string articles_data = "{" + "\"articles\":" + json_articles + "}";

            //发送图文消息
            string json_media = WeChatBaseHelper.UploadNews(token.access_token, articles_data);

            //回传消息
            WXUploadMedia _wxUploadMedia = JsonConvert.DeserializeObject<WXUploadMedia>(json_media);

            //准备群发数据
            string send_msg = "{ \"filter\": { \"is_to_all\":false, \"group_id\" : " + gruop_id + " }, \"mpnews\" :{ \"media_id \": \"" + _wxUploadMedia.media_id + "\" } }";

            //群发接口
            string back_msg = WeChatBaseHelper.SendByGroup(token.access_token, send_msg);

            return "";
        }


        /// <summary>
        /// 根据ID粉丝openId 发送
        /// </summary>
        /// <param name="open_ids"></param>
        /// <param name="account"></param>
        /// <param name="WeChatAccount"></param>
        /// <param name="imgTextReplys"></param>
        /// <param name="tableConfig"></param>
        /// <returns></returns>
        public WXSendRecord SendMpNewsByOpenIds(string[] open_ids, WeChatAccount account, WeChatAccount WeChatAccount, List<ImgTextReply> imgTextReplys)
        {
            //微信群发图文集合
            List<WXArticles> wxArticles = new List<WXArticles>();
             
            //获取token
            AccessToken token = WeChatBaseHelper.GetAccessToken(WeChatAccount.appid, WeChatAccount.app_secret);

            //微信用户OpenId集合
            string openids = JsonConvert.SerializeObject(open_ids);

            //群发推送的图文消息
            foreach (ImgTextReply i in imgTextReplys)
            {
                //获取当前图文消息
                Img img = ServiceIoc.Get<ImgService>().GetImg(ImgType.ImgTextReply_Title, i.id);

                //当天图文缩略图物理路径
                string path = img.getPath();

                //将图片上传到微信服务器
                string backmsg = WeChatBaseHelper.UploadMultimedia(path, token.access_token, WXReqMsgType.image);
                WXUploadMedia wxUploadMedia = JsonConvert.DeserializeObject<WXUploadMedia>(backmsg);

                WXArticles wxArticle = new WXArticles();

                wxArticle.title = i.title;
                //wxArticle.content_source_url = MsgContentType.GetLinkByType(account, sid, i.content_type, "", i.content_value);
                wxArticle.content = i.details;
                wxArticle.digest = i.introduction;
                wxArticle.show_cover_pic = 1;
                wxArticle.thumb_media_id = wxUploadMedia.media_id;
                wxArticle.author = account.name;

                wxArticles.Add(wxArticle);
            }

            //序列化发送图文消息
            string json_articles = JsonConvert.SerializeObject(wxArticles);

            //拼装发送数据
            string articles_data = "{" + "\"articles\":" + json_articles + "}";

            //发送图文消息
            string json_media = WeChatBaseHelper.UploadNews(token.access_token, articles_data);

            //回传消息
            WXUploadMedia _wxUploadMedia = JsonConvert.DeserializeObject<WXUploadMedia>(json_media);

            //准备群发数据
            string send_msg = "{ \"touser\" : " + openids + " , \"msgtype\" : \"mpnews\", \"mpnews\" : { \"media_id\": \"" + _wxUploadMedia.media_id + "\" } }";

            //发送消息结果
            string msg = WeChatBaseHelper.SendByOpenIds(token.access_token, send_msg);

            //群发回调信息
            WXSendRecord wxSendBack = JsonConvert.DeserializeObject<WXSendRecord>(msg);

            return wxSendBack;
        }


        /// <summary>
        /// 根据分组群发
        /// 发送文本消息
        /// </summary>
        /// <param name="gruop_id"></param>
        /// <param name="account"></param>
        /// <param name="WeChatAccount"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public string SendTextByGroup(int gruop_id, WeChatAccount account, WeChatAccount WeChatAccount, string content)
        {
            return "";
        }


        /// <summary>
        /// 根据分组群发
        /// 发送图片消息
        /// </summary>
        /// <param name="gruop_id"></param>
        /// <param name="account"></param>
        /// <param name="WeChatAccount"></param>
        /// <param name="path"></param>
        /// <param name="tableConfig"></param>
        /// <returns></returns>
        public string SendImageByGroup(int gruop_id, WeChatAccount account, WeChatAccount WeChatAccount, string path)
        {
            return "";
        }


        /// <summary>
        /// 根据openId集合群发
        /// 发送图片消息
        /// </summary>
        /// <param name="open_ids"></param>
        /// <param name="account"></param>
        /// <param name="WeChatAccount"></param>
        /// <param name="imgTextReplys"></param>
        /// <param name="tableConfig"></param>
        /// <returns></returns>
        public string SendMpNewsByOpenIds(int[] open_ids, WeChatAccount account, WeChatAccount WeChatAccount, List<ImgTextReply> imgTextReplys)
        {
            return "";
        }

        /// <summary>
        /// 发送文本消息
        /// 根据openId集合群发
        /// </summary>
        /// <param name="open_ids"></param>
        /// <param name="WeChatAccount"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public WXSendRecord SendTextByOpenIds(string[] open_ids, WeChatAccount WeChatAccount, string content)
        { 
            //获取token
            AccessToken token = WeChatBaseHelper.GetAccessToken(WeChatAccount.appid, WeChatAccount.app_secret);

            //微信用户OpenId集合
            string openids = JsonConvert.SerializeObject(open_ids);

            //准备群发数据
            string send_msg = "{ \"touser\" : " + openids + " , \"msgtype\" : \"text\", \"text\" : { \"content\": \"" + content + "\" } }";

            //发送消息结果
            string msg = WeChatBaseHelper.SendByOpenIds(token.access_token, send_msg);

            WXSendRecord wxSendBack = JsonConvert.DeserializeObject<WXSendRecord>(msg);

            return wxSendBack;
        }

        /// <summary>
        /// 根据openId集合群发
        /// 发送图片消息
        /// </summary>
        /// <param name="open_ids"></param>
        /// <param name="account"></param>
        /// <param name="WeChatAccount"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public WXSendRecord SendImageByOpenIds(string[] open_ids, WeChatAccount WeChatAccount, string path)
        { 
            //获取token
            AccessToken token = WeChatBaseHelper.GetAccessToken(WeChatAccount.appid, WeChatAccount.app_secret);

            //微信用户OpenId集合
            string openids = JsonConvert.SerializeObject(open_ids);

            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //判断是否存在图片信息
                if (!string.IsNullOrEmpty(path))
                {

                    //将图片上传到微信服务器
                    string backmsg = WeChatBaseHelper.UploadMultimedia(path, token.access_token, WXReqMsgType.image);
                    WXUploadMedia wxUploadMedia = JsonConvert.DeserializeObject<WXUploadMedia>(backmsg);

                    //准备群发数据
                    string send_msg = "{ \"touser\" : " + openids + " , \"msgtype\" : \"image\", \"image\" : { \"media_id\": \"" + wxUploadMedia.media_id + "\" } }";

                    //发送消息结果
                    string msg = WeChatBaseHelper.SendByOpenIds(token.access_token, send_msg);

                    WXSendRecord wxSendBack = JsonConvert.DeserializeObject<WXSendRecord>(msg);

                    return wxSendBack;
                }
            }
            return null;
        }



    }

}
