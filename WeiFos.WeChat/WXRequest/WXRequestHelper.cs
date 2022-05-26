using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using WeiFos.Core.XmlHelper;
using WeiFos.WeChat.WXBase;

namespace WeiFos.WeChat.WXRequest
{
    /// <summary>
    /// 处理微信请求服务
    /// @author yewei 
    /// @date 2013-11-06
    /// </summary>
    public class WXRequestHelper
    {

        /// <summary>
        /// 处理微信请求
        /// </summary>
        /// <param name="postStr"></param>
        /// <returns></returns>
        public static WXReqBaseMsg GetRequest(string postStr)
        {

            //XML文档处理对象
            XmlDocument postObj = new XmlDocument();

            //加载xml格式请求数据
            postObj.LoadXml(postStr);

            //获取xml结构根目录
            XmlElement postElement = postObj.DocumentElement;

            //获取请求消息类型
            string msgtype = postElement.SelectSingleNode("MsgType").InnerText;

            WXReqBaseMsg entitybase = null;

            //判断接收消息类型
            switch (msgtype)
            {
                //文本消息
                case WXReqMsgType.text:
                    entitybase = XmlConvertHelper.DeserializeObject<WXReqTextMsg>(postStr);
                    break;
                //图片消息
                case WXReqMsgType.image:
                    entitybase = XmlConvertHelper.DeserializeObject<WXReqImageMsg>(postStr);
                    break;
                //语音消息
                case WXReqMsgType.voice:
                    entitybase = XmlConvertHelper.DeserializeObject<WXReqVoiceMsg>(postStr);
                    break;
                //视频消息
                case WXReqMsgType.video:
                    entitybase = XmlConvertHelper.DeserializeObject<WXReqVideoMsg>(postStr);
                    break;
                //地理位置消息
                case WXReqMsgType.location:
                    entitybase = XmlConvertHelper.DeserializeObject<WXReqLocationMsg>(postStr);
                    break;
                //链接消息
                case WXReqMsgType.url:
                    entitybase = XmlConvertHelper.DeserializeObject<WXReqLinkMsg>(postStr);
                    break;
                //事件消息
                case WXReqMsgType.wxevent:
                    WXReqEventMsg wxReqEventMsg = XmlConvertHelper.DeserializeObject<WXReqEventMsg>(postStr);
                    switch (wxReqEventMsg.Event.ToString().ToLower())
                    {
                        //订阅
                        case WXEventType.subscribe:
                            entitybase = XmlConvertHelper.DeserializeObject<WXReqSubscribe>(postStr);
                            break;

                        //取消订阅
                        case WXEventType.unsubscribe:
                            entitybase = XmlConvertHelper.DeserializeObject<WXReqSubscribe>(postStr);
                            break;

                        //菜单点击
                        case WXEventType.click:
                            entitybase = XmlConvertHelper.DeserializeObject<WXReqSubscribe>(postStr);
                            break;

                        //用户扫描二维码
                        case WXEventType.scan:
                            entitybase = XmlConvertHelper.DeserializeObject<WXReqEventScanMsg>(postStr);
                            break;

                        //群发任务提回调推送事件
                        case WXEventType.masssendjobfinish:
                            entitybase = XmlConvertHelper.DeserializeObject<WXReqMassSendJobFinish>(postStr);
                            break;

                        default:
                            break;
                    }
                    break;
            }
            return entitybase;
        }


    }
}
