using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.WeChatModule.WXOpen
{

    /// <summary>
    /// 微信开放平台授权公众号信息
    /// @author yewei 
    /// add by @date 2018-04-13
    /// </summary>
    [Serializable]
    [Table(Name = "tb_wx_open_account")]
    public class WXOpenAccount : BaseClass
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { set; get; }


        /// <summary>
        ///门店ID
        /// </summary>
        public long store_id { get; set; }


        /// <summary>
        ///公众号昵称
        /// </summary>
        public string nick_name { get; set; }


        /// <summary>
        /// 公众号图片
        /// </summary>
        public string head_img { get; set; }


        /// <summary>
        ///公众微信号
        /// </summary>
        public string wechat_no { get; set; }


        /// <summary>
        /// 微信公众号类型 1订阅号,2认证订阅号,3服务号,4认证服务号
        /// </summary>
        public int type { get; set; }


        /// <summary>
        /// 授权方appid
        /// </summary>
        public string auth_appid { get; set; }


        /// <summary>
        /// 原始ID
        /// </summary>
        public string original_id { get; set; }


        /// <summary>
        /// 公众号二维码
        /// </summary>
        public string qrcode_url { get; set; }


        /// <summary>
        /// 公众号主体名称
        /// </summary>
        public string principal_name { get; set; }

    }
}
