using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.WeiXin
{
    [Serializable]
    [Table(Name = "tb_ur_account_wx")]
    public class WXAccount
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public int id { set; get; }

        /// <summary>
        /// 公众号ID
        /// </summary>
        public int account_id { set; get; }

        /// <summary>
        /// 公众号原始id
        /// </summary>
        public string account_original_id { get; set; }

        /// <summary>
        /// 公众号邮箱
        /// </summary>
        public string account_email { get; set; }

        /// <summary>
        /// 微信号
        /// </summary>
        public string account_tag { get; set; }

        /// <summary>
        /// 粉丝数
        /// </summary>
        public int funs { get; set; }

        /// <summary>
        /// 公众号类型 1订阅号,2认证订阅号,3认证订阅号,4认证订阅号
        /// </summary>
        public int account_type { get; set; }

        /// <summary>
        /// 公众号应用id
        /// </summary>
        public string account_appid { get; set; }

        /// <summary>
        /// 公众号应用密钥
        /// </summary>
        public string account_appsecret { get; set; }
    }
}
