using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.WeChat.WXBase.WXOpen
{
    /// <summary>
    /// 微信开放平台 
    /// 授权信息
    /// @author yewei
    /// @date 2018-04-11
    /// </summary>
    public class WXOpenAuthInfo : WXErrCode
    {


        /// <summary>
        /// 授权方appid（官网上文档的字段）
        /// </summary>
        public string authorization_appid { get; set; }


        /// <summary>
        /// 实际appid 字段
        /// </summary>
        public string authorizer_appid { get; set; }


        /// <summary>
        /// 实际接口返回刷新token字段
        /// </summary>
        public string authorizer_refresh_token { get; set; }


        /// <summary>
        /// 公众号授权给开发者的权限集列表，ID为1到15时分别代表：
        /// 1.消息管理权限 2.用户管理权限 3.帐号服务权限 4.网页服务权限 5.微信小店权限
        /// 6.微信多客服权限 7.群发与通知权限 8.微信卡券权限 9.微信扫一扫权限 
        /// 10.微信连WIFI权限 11.素材管理权限 12.微信摇周边权限 13.微信门店权限 14.微信支付权限 
        /// 15.自定义菜单权限 请注意： 1）该字段的返回不会考虑公众号是否具备该权限集的权限（因为可能部分具备），
        /// 请根据公众号的帐号类型和认证情况，来判断公众号的接口权限。
        /// </summary>
        public List<FuncInfoItem> func_info { get; set; }
    }



    public class FuncInfoItem
    {

        /// <summary>
        /// 
        /// </summary>
        public string appid { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public FuncScopeCatg funcscope_category { get; set; }
    }



    public class FuncScopeCatg
    {
        public int id { get; set; }
    }



}
