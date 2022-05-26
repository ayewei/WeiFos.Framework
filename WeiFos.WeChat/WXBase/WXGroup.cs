using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.WeChat.WXBase
{
    /// <summary>
    /// 微信 用户集合
    /// 通过接口获取用户数据临时对象
    /// </summary>
    [Serializable]
    public class WXGroup
    {

        /// <summary>
        /// 分组id，由微信分配 
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 分组名字，UTF8编码 
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 分组内用户数量 
        /// </summary>
        public int count { get; set; }

    }
}
