using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.Core;
using WeiFos.Entity.UserModule;
using WeiFos.Entity.WeChatModule;

namespace WeiFos.Entity.BizTypeModule
{

    /// <summary>
    /// 图文或栏目 链接类型 实体类
    /// @author yewei 
    /// @date 2013-10-03
    /// </summary>
    public class MsgContentType
    {
        /// <summary>
        /// 外部链接
        /// </summary>
        public const int OutLink = 0;

        /// <summary>
        /// 图文
        /// </summary>
        public const int ImgTextDetails = 1;

        /// <summary>
        /// 一键拨号
        /// </summary>
        public const int OneTouchDial = 3;

        /// <summary>
        /// 导航信息
        /// </summary>
        public const int Navigation = 5;

        /// <summary>
        /// 微活动
        /// </summary>
        public const int WeiActivity = 10;

        /// <summary>
        /// 微相册(相册空间)
        /// </summary>
        public const int WeiAlbumZone = 11;

        /// <summary>
        /// 微相册（相册）
        /// </summary>
        public const int WeiAlbum = 12;

        /// <summary>
        /// 微留言
        /// </summary>
        public const int WeiMessage = 13;

        /// <summary>
        /// 名片分享
        /// </summary>
        public const int VisitingCard = 15;

        /// <summary>
        /// 注册
        /// </summary>
        public const int Register = 20;

        /// <summary>
        /// 微商城
        /// </summary>
        public const int WeiMall = 35;

        /// <summary>
        /// 微预约
        /// </summary>
        public const int WeiResv = 36;

        /// <summary>
        /// 微信会员卡
        /// </summary>
        public const int MemberCard = 45;


        public static Dictionary<int, string> typeList = new Dictionary<int, string>()
        {
            {MsgContentType.OutLink,"外部链接"},
            {MsgContentType.Navigation,"导航信息"},
            {MsgContentType.WeiActivity,"微活动"},
            {MsgContentType.WeiMall,"微商城"},
            //{MsgContentType.VisitingCard,"名片分享"},
            //{MsgContentType.Register,"用户注册"},
            //{MsgContentType.WeiEstate,"微房产"} ,
            //{MsgContentType.WeiCatering,"微餐饮"} ,
            //{MsgContentType.WeiMall,"微商城"} ,
            //{MsgContentType.WeiLife,"微生活"} 
        };

        /// <summary>
        /// 根据key获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValueByKey(int key)
        {
            foreach (var item in typeList)
            {
                if (key.Equals(item.Key))
                {
                    return item.Value;
                }
            }
            return "暂无";
        }

        public static string GetLinkByType(WeChatAccount account, int content_type, string usercode, string content_value)
        {
            return GetLinkByType(account, 0, content_type, usercode, content_value);
        }

        public static string GetLinkByType(WeChatAccount account, int sid, int content_type, string content_value)
        {
            return GetLinkByType(account, sid, content_type, "", content_value);
        }

        /// <summary>
        /// 根据信息类型获取对应链接
        /// </summary>
        /// <param name="content_type"></param>
        /// <param name="content_value"></param>
        /// <returns></returns>
        public static string GetLinkByType(WeChatAccount account, int sid, int content_type, string usercode, string content_value)
        {
            StringBuilder sb = new StringBuilder();

            switch (content_type)
            {
                //一键拨号
                case MsgContentType.OneTouchDial:
                    return sb.Append("tel:").Append(content_value).ToString();
                //外链
                case MsgContentType.OutLink:
                    return content_value;
                //图文
                case MsgContentType.ImgTextDetails:
                    sb.Append("WeiSiteDomain");
                    sb.Append("Home/InfoDetails/").Append(content_value).Append(".html");
                    return sb.ToString();
                //导航
                case MsgContentType.Navigation:
                    sb.Append("http://api.map.baidu.com/marker?location=");
                    if (!string.IsNullOrEmpty(content_value) && content_value.IndexOf(",") != -1 && content_value.IndexOf("#") != -1)
                    {
                        int index = content_value.IndexOf("#");
                        string location = content_value.Substring(0, index);
                        string address = content_value.Substring(index + 1, content_value.Length - index - 1);
                        sb.Append(location);
                        sb.Append("&title=").Append(account.name);
                        sb.Append("&content=").Append(address);
                        sb.Append("&output=html");
                    }
                    return sb.ToString();
                //微活动
                case MsgContentType.WeiActivity:
                    sb.Append("WeiSiteDomain");
                    sb.Append("weiactivity.aspx?");
                    sb.Append("aid=" + account.id + "&");
                    sb.Append("bid=" + content_value.Split('#')[0].ToString() + "&");
                    sb.Append("code=" + content_value.Split('#')[1].ToString() + "&");
                    sb.Append("usercode=" + usercode);
                    return sb.ToString();
                //微相册
                case MsgContentType.WeiAlbum:
                    sb.Append("WeiSiteDomain");
                    sb.Append("Album/Album.aspx?");
                    sb.Append("bid=" + content_value);
                    return sb.ToString();
                //微留言
                case MsgContentType.WeiMessage:
                    sb.Append("WeiSiteDomain");
                    sb.Append("weimessage.aspx?");
                    sb.Append("code=" + content_value + "&");
                    sb.Append("usercode=" + usercode);
                    return sb.ToString();
                //名片
                case MsgContentType.VisitingCard:
                    sb.Append("WeiSiteDomain");
                    sb.Append("Home/MyQRCode");
                    return sb.ToString();
                //完成注册
                case MsgContentType.Register:
                    sb.Append("WeiSiteDomain");
                    sb.Append("Home/Register");
                    return sb.ToString();
                //微商城
                case MsgContentType.WeiMall:
                    sb.Append("WeiSiteDomain");
                    sb.Append("index");
                    return sb.ToString();
                //微预约
                case MsgContentType.WeiResv:
                    sb.Append("WeiSiteDomain");
                    sb.Append("WeiReservation/ResvOrderSubmit.aspx?");
                    sb.Append("aid=" + account.id + "&");
                    sb.Append("bid=" + content_value);
                    return sb.ToString();

                //微信会员卡
                case MsgContentType.MemberCard:
                    sb.Append("WeiSiteDomain");

                    //存在会员卡号
                    if (content_value.IndexOf("#") != -1)
                    {
                        sb.Append("MemberCard/Index.aspx?");
                        sb.Append("aid=" + account.id + "&");
                        sb.Append("bid=" + content_value.Split('#')[0] + "&");
                        sb.Append(usercode);
                    }//不存在会员卡号
                    else
                    {
                        sb.Append("MemberCard/ResvOrderSubmit.aspx?");
                        sb.Append("aid=" + account.id + "&");
                        sb.Append("bid=" + content_value);
                    }

                    return sb.ToString();

                default:
                    return "";
            }
        }


    }
}