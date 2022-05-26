using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.WeChat.WXBase.WXOpen
{

    /// <summary>
    /// 微信开放平台
    /// 接受微信授权公众账号实体
    /// </summary>
    public class WXOpenWeChatAccount : WXErrCode
    {

        /// <summary>
        /// 微信SDK Demo Special
        /// </summary>
        public string nick_name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string head_img { get; set; }

        /// <summary>
        /// 授权方公众号类型，0代表订阅号，1代表由历史老帐号升级后的订阅号，2代表服务号
        /// </summary>
        public ServiceTypeInfo service_type_info { get; set; }

        /// <summary>
        /// 授权方认证类型，-1代表未认证，0代表微信认证，1代表新浪微博认证，2代表腾讯微博认证，
        /// 3代表已资质认证通过但还未通过名称认证，4代表已资质认证通过、还未通过名称认证，
        /// 但通过了新浪微博认证，5代表已资质认证通过、还未通过名称认证，但通过了腾讯微博认证
        /// </summary>
        public VerifyTypeInfo verify_type_info { get; set; }

        /// <summary>
        /// 授权方公众号的原始ID
        /// </summary>
        public string user_name { get; set; }

        /// <summary>
        /// 公众号的主体名称
        /// </summary>
        public string principal_name { get; set; }

        /// <summary>
        /// 用以了解以下功能的开通状况（0代表未开通，1代表已开通）： open_store:是否开通微信门店功能
        /// open_scan:是否开通微信扫商品功能 open_pay:是否开通微信支付功能 open_card:是否开通微信卡券功能
        /// open_shake:是否开通微信摇一摇功能
        /// </summary>
        public BusinessInfo business_info { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string alias { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string qrcode_url { get; set; }

    }


    /// <summary>
    /// 授权方公众号类型
    /// </summary>
    public class ServiceTypeInfo
    {
        public int id { get; set; }
    }

    /// <summary>
    /// 授权方认证类型
    /// </summary>
    public class VerifyTypeInfo
    {
        public int id { get; set; }
    }


    /// <summary>
    /// 用以了解功能的开通状况
    /// </summary>
    public class BusinessInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public int open_store { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int open_scan { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int open_pay { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int open_card { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int open_shake { get; set; }
    }

}
