using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.WeChat.Models.OrgEntity.SendMsg
{

    /// <summary>
    /// 企业微信 推送文本消息
    /// @author yewei 
    /// @date 2019-03-31
    /// </summary>
    [Serializable]
    public class TextMsg
    {

        /// <summary>
        /// 成员ID列表（消息接收者，多个接收者用‘|’分隔，最多支持1000个）。
        /// 特殊情况：指定为@all，则向该企业应用的全部成员发送
        /// </summary>
        public string touser { get; set; }

        /// <summary>
        /// 部门ID列表，多个接收者用‘|’分隔，最多支持100个。
        /// 当touser为@all时忽略本参数
        /// </summary>
        public string toparty { get; set; }

        /// <summary>
        /// 标签ID列表，多个接收者用‘|’分隔，最多支持100个。
        /// 当touser为@all时忽略本参数
        /// </summary>
        public string totag { get; set; }

        /// <summary>
        /// 消息类型，此时固定为：text
        /// </summary>
        public string msgtype { get; set; }

        /// <summary>
        /// 企业应用的id，整型。企业内部开发，可在应用的设置页面查看；
        /// 第三方服务商，可通过接口 获取企业授权信息 获取该参数值
        /// </summary>
        public string agentid { get; set; }

        /// <summary>
        /// 消息内容，最长不超过2048个字节，超过将截断
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// 表示是否是保密消息，0表示否，1表示是，默认0
        /// </summary>
        public string safe { get; set; }

    }





}
