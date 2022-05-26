﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using WeiFos.WeChat.WXBase;

namespace WeiFos.WeChat.WXRequest
{
    /// <summary>
    /// 微信订阅事件消息 实体对象
    /// @author yewei 
    /// @date 2014-07-21
    /// </summary>
    [Serializable]
    [XmlRoot("xml")]
    public class WXReqSubscribe : WXReqEventMsg
    { 

    }

}
