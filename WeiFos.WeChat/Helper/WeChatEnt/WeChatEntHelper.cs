using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.WeChat.Models;
using WeiFos.WeChat.Models.OrgEntity;
using WeiFos.WeChat.TickeModule;

namespace WeiFos.WeChat.Helper.WeChatEnt
{


    /// <summary>
    /// 企业号 帮助类
    /// @Author yewei 
    /// @Date 2019-03-16
    /// </summary>
    public class WeChatEntHelper
    {


        #region  获取企业号微信授权Token


        /// <summary>
        /// 获取企业号微信授权Token
        /// </summary>
        /// <param name="corpid"></param>
        /// <param name="corpsecret"></param>
        /// <returns></returns>
        public static AccessToken GetAccessToken(string corpid, string corpsecret)
        {
            string url = "https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}";
            url = string.Format(url, corpid, corpsecret);
            string returnData = HttpClientHelper.HttpGetAsyn(url);

            return JsonConvert.DeserializeObject<AccessToken>(returnData);
        }


        #endregion


        #region  获取企业微信部门（获取所有）


        /// <summary>
        /// 获取企业微信部门（获取所有）
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static WeChatDeptResult GetDepartment(string token)
        {
            return GetDepartment(token, 0);
        }


        #endregion


        #region 获取企业微信部门（获取单个部门）

        /// <summary>
        /// 获取企业微信部门（获取单个部门）
        /// </summary>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static WeChatDeptResult GetDepartment(string token, int id)
        {
            string url = "https://qyapi.weixin.qq.com/cgi-bin/department/list?access_token={0}&id={1}";
            if (id != 0) url = string.Format(url, token, id);
            else
            {
                url = "https://qyapi.weixin.qq.com/cgi-bin/department/list?access_token={0}";
                url = string.Format(url, token);
            }

            string returnData = HttpClientHelper.HttpGetAsyn(url);
            return JsonConvert.DeserializeObject<WeChatDeptResult>(returnData);
        }

        #endregion


        #region 获取企业微信员工信息（根据user_id获取）


        /// <summary>
        /// 获取企业微信员工信息（根据user_id获取）
        /// </summary>
        /// <param name="token"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public static WeChatEmployee GetEmployeeByUserId(string token, string user_id)
        {
            string url = "https://qyapi.weixin.qq.com/cgi-bin/user/get?access_token={0}&userid={1}";
            string returnData = HttpClientHelper.HttpGetAsyn(string.Format(url, token, user_id));
            return JsonConvert.DeserializeObject<WeChatEmployee>(returnData);
        }


        #endregion


        #region 获取企业微信员工信息（根据user_id获取）


        /// <summary>
        /// 获取企业微信员工信息（根据user_id获取）
        /// </summary>
        /// <param name="token"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public static string GetEmployeeByUserIdJson(string token, string user_id)
        {
            string url = "https://qyapi.weixin.qq.com/cgi-bin/user/get?access_token={0}&userid={1}";
            return HttpClientHelper.HttpGetAsyn(string.Format(url, token, user_id));
        }


        #endregion


        #region 获取企业微信员（根据部门ID获取）


        /// <summary>
        /// 获取企业微信员（根据部门ID获取）
        /// </summary>
        /// <param name="token"></param>
        /// <param name="dept_id">获取的部门id</param>
        /// <param name="dept_idfetch_child">1/0：是否递归获取子部门下面的成员</param>
        /// <returns></returns>
        public static string GetEmployeeByDeptId(string token, long dept_id, int dept_idfetch_child)
        {
            string url = "https://qyapi.weixin.qq.com/cgi-bin/user/list?access_token={0}&department_id={1}&fetch_child={2}";
            return HttpClientHelper.HttpGetAsyn(string.Format(url, token, dept_id, dept_idfetch_child));
        }


        #endregion


        #region 获取微信企业部门（获取部门）


        /// <summary>
        /// 获取微信企业部门（获取部门）
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static WXCodeError UpdateWeChatEmployee(WeChatEmployee employee)
        {
            string url = "https://qyapi.weixin.qq.com/cgi-bin/user/update?access_token={0}";
            string returnData = HttpClientHelper.PostURL(string.Format(url, JsonConvert.SerializeObject(employee)));
            return JsonConvert.DeserializeObject<WXCodeError>(returnData);
        }

        #endregion


        #region 构造网页授权链接

        /// <summary>
        /// 获取授权连接
        /// </summary>
        /// <param name="redirect_uri">重定向地址，需要urlencode，这里填写的应是服务开发方的回调地址</param>
        /// <param name="appid">公众号的appid</param>
        /// <param name="scope">授权作用域，拥有多个作用域用逗号（,）分隔</param>
        /// <param name="state">重定向后会带上state参数，开发者可以填写任意参数值，最多128字节</param>
        /// <param name="responseType">默认为填code</param>
        /// <returns>URL</returns>
        public static string Authorize(string redirect_uri, string appid, string state)
        {
            string oauthURL = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state={2}#wechat_redirect";
            return string.Format(oauthURL, appid, System.Web.HttpUtility.UrlEncode(redirect_uri).Replace("+", "%20"), state);
        }

        #endregion


        #region 该接口用于根据code获取成员信息

        /// <summary>
        /// 该接口用于根据code获取成员信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static WeChatQYUserResult GetUserInfo(string token, string code)
        {
            string url = "https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={0}&code={1}";
            string returnData = HttpClientHelper.PostURL(string.Format(url, token, code));
            return JsonConvert.DeserializeObject<WeChatQYUserResult>(returnData);
        }

        #endregion


    }
}
