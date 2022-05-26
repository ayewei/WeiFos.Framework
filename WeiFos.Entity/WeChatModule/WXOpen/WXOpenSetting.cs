using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.WeChatModule.WXOpen
{
    /// <summary>
    /// 开放平台配置表
    /// @author yewei 
    /// add by @date 2018-04-13
    /// </summary>
    [Serializable]
    [Table(Name = "tb_wx_open_setting")]
    public class WXOpenSetting
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { set; get; }


        /// <summary>
        /// 开放平台应用ID
        /// </summary>
        public string component_appid { set; get; }

        /// <summary>
        /// 开放平台应用秘钥
        /// </summary>
        public string component_appsecret { set; get; }

        /// <summary>
        /// token
        /// </summary>
        public string token { set; get; }


        /// <summary>
        /// 消息加密密钥由43位字符组成，可随机修改，字符范围为A-Z，a-z，0-9。
        /// </summary>
        public string aes_key { set; get; }

    }
}
