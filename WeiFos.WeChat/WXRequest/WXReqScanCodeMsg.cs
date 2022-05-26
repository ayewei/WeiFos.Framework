using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using WeiFos.WeChat.WXBase;

namespace WeiFos.WeChat.WXRequest
{
    public class WXReqScanCodeMsg : WXReqEventMsg
    { 
        /// <summary>
        /// 扫描信息
        /// </summary>
        public WXReqScanCodeInfo ScanCodeInfo { get; set; }
    }


    public class WXReqScanCodeInfo
    {
        [XmlElement("ScanType", typeof(CDATA))]
        public CDATA ScanType { get; set; }

        [XmlElement("ScanResult", typeof(CDATA))]
        public CDATA ScanResult { get; set; }
    }
}
