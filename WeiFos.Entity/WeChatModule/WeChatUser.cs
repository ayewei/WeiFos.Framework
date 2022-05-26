using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.WeChatModule
{
    /// <summary>
    /// 微信会员用户实体类
    /// @author yewei 
    /// @date 2015-05-17
    /// </summary>
    [Serializable]
    [Table(Name = "tb_wx_user")]
    public class WeChatUser
    {   
        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// openid
        /// </summary>
        public string openid { get; set; } 

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime created_date { get; set; }

        /// <summary>
        /// 用户是否订阅
        /// </summary>
        public int subscribe { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string nickname { get; set; }

        /// <summary>
        /// 用户所在国家
        /// </summary>
        public string country { get; set; }

        /// <summary>
        /// 用户所在省份
        /// </summary>
        public string province { get; set; }

        /// <summary>
        /// 用户所在城市
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// 用户的性别
        /// </summary>
        public int sex { get; set; }

        /// <summary>
        /// 用户语言
        /// </summary>
        public string language { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string headimgurl { get; set; }

        /// <summary>
        /// 关注时间
        /// </summary>
        public string subscribe_time { get; set; }

        /// <summary>
        /// 上次联系时间
        /// </summary>
        public DateTime? last_contact_time { get; set; }

        /// <summary>
        /// UnionID机制
        /// 同一用户，对同一个微信开放平台下的不同应用，unionid是相同的
        /// </summary>
        public string unionid { get; set; }


        #region  自定义用户数据
         
        /// <summary>
        /// 用户标签
        /// </summary>
        public string tag { get; set; }
 
        #endregion

    }
}