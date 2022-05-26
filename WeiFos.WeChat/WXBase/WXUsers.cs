using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.WeChat.WXBase
{
    /// <summary>
    /// 微信 用户集合
    /// 通过接口获取用户数据临时对象
    /// @author yewei 
    /// @date 2014-12-27
    /// </summary>
    [Serializable]
    public class WXUsers
    {

        /// <summary>
        /// 关注该公众账号的总用户数 
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 拉取的OPENID个数，最大值为10000 
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// 列表数据，OPENID的列表 
        /// </summary>
        public open_id data { get; set; }

        /// <summary>
        /// 拉取列表的后一个用户的OPENID 
        /// </summary>
        public string next_openid { get; set; }

    }

    public class open_id
    {
        public string[] openid;
    }


}
