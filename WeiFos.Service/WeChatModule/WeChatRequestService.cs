using System.Xml;
using WeiFos.Core.XmlHelper;
using WeiFos.WeChat.WXBase;
using WeiFos.WeChat.WXRequest;


namespace WeiFos.Service.WeChat
{
    /// <summary>
    /// 处理微信请求服务
    /// @author yewei 
    /// @date 2013-11-06
    /// </summary>
    public class WeChatRequestService
    {


        public WXReqBaseMsg GetRequest(string postStr)
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
                    entitybase = XmlConvertHelper.DeserializeObject<WXReqEventMsg>(postStr);
                    break;
            }
            return entitybase;
        }


    }
}
