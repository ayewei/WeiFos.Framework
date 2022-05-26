using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WeiFos.WeChat.MessageModule
{
    /// <summary>
    /// 消息基类
    /// </summary>
    public class ReturnMessage
    {
        /// <summary>
        /// 返回状态码
        /// String(16)
        /// SUCCESS/FAIL,此字段是通信标识，非交易标识，交易是否成功需要查
        /// </summary>
        public string return_code { get; set; }

        /// <summary>
        /// 返回信息
        /// String(128)
        /// 如非空，为错误原因,签名失败参数格式校验错误
        /// </summary>
        public string return_msg { get; set; }

        public string ToXmlString()
        {
            return string.Format(@"<xml><return_code><![CDATA[{0}]]></return_code>
                    <return_msg><![CDATA[{1}]]></return_msg></xml>", return_code, return_msg);
        }
    }
}
