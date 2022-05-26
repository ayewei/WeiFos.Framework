using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using WeiFos.WeChat.WXBase;

namespace WeiFos.WeChat.WXRequest
{
    /// <summary>
    /// 地理位置消息 实体对象
    /// @author yewei 
    /// @date 2013-11-04 WXReqLocationMsg
    /// </summary>
    [Serializable]
    [XmlRoot("xml")]
    public class WXReqLocationMsg : WXReqBaseMsg
    {

        /// <summary>
        /// 地理位置纬度 lat
        /// </summary>
        public double Location_X { get; set; }

        /// <summary>
        /// 地理位置经度 lng
        /// </summary>
        public double Location_Y { get; set; }

        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public string Scale { get; set; }

        /// <summary>
        /// 地理位置消息
        /// </summary>
        public string Label { get; set; }

    }
}
