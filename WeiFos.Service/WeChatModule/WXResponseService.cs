using System;
using WeiFos.Core;
using WeiFos.Entity.ReplyModule;
using WeiFos.Entity.WeChatModule;
using WeiFos.Service.ReplyModule; 
using WeiFos.Service.WeChatModule;
using WeiFos.WeChat.WXRequest;
using WeiFos.WeChat.WXBase;
using WeiFos.Service.SystemModule;
using WeiFos.Service.LogsModule;

namespace WeiFos.Service.WeChatModule
{
    /// <summary>
    /// 处理微信响应服务
    /// @author yewei 
    /// @date 2013-11-06
    /// </summary>
    public class WXResponseService
    {

        /// <summary>
        /// 处理文本消息请求 响应
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string Response(WXReqBaseMsg wxRequest, WeChatAccount account)
        {
            //响应信息
            string responseMsg = string.Empty;
            KeyWord keyWord;

            if (wxRequest != null)
            {
                //先判断是否已开启微客服,如开启则接入微客服
                //responseMsg = ServiceIoc.Get<CustomerService>().GetWXResponseMsg(wxRequest);
                //if (!string.IsNullOrEmpty(responseMsg))
                //{
                //    return responseMsg;
                //}

                //更新联系时间
                ServiceIoc.Get<WeChatUserService>().UpdateLastContactTime(wxRequest.FromUserName);

                switch (wxRequest.MsgType)
                {
                    //文本消息
                    case WXReqMsgType.text:
                        //转换成为文本请求
                        WXReqTextMsg reqTextMsg = (WXReqTextMsg)wxRequest;

                        //统计关键词
                        //StatsKeyWords statsKeyWords = new StatsKeyWords();
                        //statsKeyWords.keyword = reqTextMsg.Content;
                        //statsKeyWords.req_datetime = DateTime.Now;
                        keyWord = ServiceIoc.Get<KeyWordService>().GetKeyWordMsg(reqTextMsg.Content);
                        //if (keyWord != null)
                        //    statsKeyWords.is_hit = true;

                        //ServiceIoc.Get<StatsKeyWordsService>().Save(statsKeyWords);
                        responseMsg = GetKeyWordBizTypeMsg(wxRequest, account, keyWord);
                        break;
                    //图片消息
                    case WXReqMsgType.image:
                        break;

                    //语音消息
                    case WXReqMsgType.voice:
                        break;

                    //视频消息
                    case WXReqMsgType.video:
                        break;

                    //地理位置消息
                    case WXReqMsgType.location:
                        //responseMsg = ServiceIoc.Get<LbsReplyService>().GetWXResponseMsg((WXReqLocationMsg)wxRequest);
                        break;

                    //链接消息
                    case WXReqMsgType.url:
                        break;

                    //事件推送
                    case WXReqMsgType.wxevent:
                        Type t = wxRequest.GetType();
                        string event_name = t.GetProperty("Event").GetValue(wxRequest, null).ToString();

                        ServiceIoc.Get<APILogsService>().Save("WXReqMsgType.wxevent:" + event_name.ToLower());
                        switch (event_name.ToLower())
                        {
                            //订阅
                            case WXEventType.subscribe:
                                WXReqSubscribe subscribe = (WXReqSubscribe)wxRequest;
                                if (!string.IsNullOrEmpty(subscribe.EventKey) && subscribe.EventKey.ToString().IndexOf("qrscene_") != -1)
                                {
                                    //获取扫码参数
                                    string val = subscribe.EventKey.ToString().Replace("qrscene_", "");
                                    ServiceIoc.Get<WeChatUserService>().Subscribe(account, wxRequest.FromUserName, long.Parse(val));
                                }
                                else
                                {
                                    ServiceIoc.Get<WeChatUserService>().Subscribe(account, wxRequest.FromUserName, 0);
                                }
                                responseMsg = ServiceIoc.Get<DefaultSetService>().GetWXResponseAttentionMsg(wxRequest, account);
                                break;

                            //取消订阅
                            case WXEventType.unsubscribe:
                                WXReqSubscribe unSubscribe = (WXReqSubscribe)wxRequest;
                                break;

                            //菜单点击
                            case WXEventType.click:
                                WXReqSubscribe click = (WXReqSubscribe)wxRequest;
                                keyWord = ServiceIoc.Get<KeyWordService>().GetKeyWordMsg(click.EventKey);
                                responseMsg = GetKeyWordBizTypeMsg(wxRequest, account, keyWord);
                                break;

                            //用户扫描二维码
                            case WXEventType.scan:
                                break;

                            //群发任务提回调推送事件
                            case WXEventType.masssendjobfinish:
                                WXReqMassSendJobFinish massSendJobFinish = (WXReqMassSendJobFinish)wxRequest;
                                ServiceIoc.Get<WXSendRecordService>().SetSendStatus(massSendJobFinish);
                                break;

                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }

                //默认无匹配回复
                return string.IsNullOrEmpty(responseMsg) ? ServiceIoc.Get<DefaultSetService>().GetWXResponseDefaultMsg(wxRequest, account) : responseMsg;
            }
            return string.Empty;
        }


        /// <summary>
        /// 根据关键词类型获取响应消息
        /// </summary>
        /// <param name="wxRequest"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        protected string GetKeyWordBizTypeMsg(WXReqBaseMsg wxRequest, WeChatAccount account, KeyWord keyWord)
        {
            if (keyWord == null)
                return string.Empty;

            switch (keyWord.biz_type)
            {
                //活动关键词 数据拼装
                //case KeyWordBizType.Activity:
                //    return ServiceIoc.Get<ActivityBaseService>().GetWXResponseMsg(wxRequest, account, keyWord.biz_id, UserSourceType.WeiXin);

                //图文关键词 数据拼装
                case KeyWordBizType.ImgTextReply:
                    return ServiceIoc.Get<ImgTextReplyService>().GetWXResponseMsg(wxRequest, account, keyWord.biz_id);

                //文本关键词 数据拼装
                case KeyWordBizType.TextReply:
                    return ServiceIoc.Get<TextReplyService>().GetWXResponseMsg(wxRequest, account, keyWord.biz_id);

            }

            return string.Empty;
        }


    }
}
