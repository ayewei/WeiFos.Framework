using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Core.XmlHelper;
using WeiFos.WeChat.WXBase;

namespace WeiFos.WeChat.WXRequest
{

    /// <summary>
    /// 接收事件推送与消息排重
    /// 单列模式
    /// @author yewei
    /// @date 2018-04-14
    /// </summary>
    public class WXReqDistinctMsg
    {


        #region 单列模式  

        private static WXReqDistinctMsg instance = null;
        private WXReqDistinctMsg() {/*私有构造器，不能该类外部new对象*/}
        public static WXReqDistinctMsg Instance
        {
            get { return instance = instance ?? new WXReqDistinctMsg(); }
        }

        #endregion



        public List<BaseMsg> MsgList;


        /// <summary>
        /// 查询消息状态
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public bool QueryMessage(string xml)
        {
            bool result = true;
            if (Instance.MsgList == null) Instance.MsgList = new List<BaseMsg>();

            //当前请求消息
            WXReqBaseMsg req_base_msg = XmlConvertHelper.DeserializeObject<WXReqBaseMsg>(xml);

            //如果是事件消息类型，标识符号为时间节点
            if (req_base_msg.MsgType == WXReqMsgType.wxevent)
            {
                if (!Instance.MsgList.Exists(m => m.MsgFlag.Equals(req_base_msg.CreateTime.ToString())))
                {
                    Instance.MsgList.Add(new BaseMsg
                    {
                        CreateTime = DateTime.Now,
                        FromUser = req_base_msg.FromUserName,
                        MsgFlag = req_base_msg.CreateTime.ToString()
                    });
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                if (!Instance.MsgList.Exists(m => m.MsgFlag.Equals(req_base_msg.MsgId.ToString())))
                {
                    Instance.MsgList.Add(new BaseMsg
                    {
                        CreateTime = DateTime.Now,
                        FromUser = req_base_msg.FromUserName,
                        MsgFlag = req_base_msg.MsgId.ToString()
                    });
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

    }


    public class BaseMsg
    {
        /// <summary>
        /// 发送者标识
        /// </summary>
        public string FromUser { get; set; }

        /// <summary>
        /// 消息表示。普通消息时，为msgid，事件消息时，为事件的创建时间
        /// </summary>
        public string MsgFlag { get; set; }

        /// <summary>
        /// 添加到队列的时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }


}