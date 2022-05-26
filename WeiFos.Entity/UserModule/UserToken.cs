using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.UserModule
{

    /// <summary>
    /// APP用户Token令牌 实体类
    /// @author yewei 
    /// add by @date 2015-01-09
    /// </summary>
    [Serializable]
    [Table(Name = "tb_user_token")]
    public class UserToken
    {

        /// <summary>
        /// 主键ID（自增）
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// 令牌
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// 手机IMEI号
        /// </summary>
        public string imei { get; set; }

        /// <summary>
        /// 手机IMSI号
        /// </summary>
        public string imsi { get; set; }

        /// <summary>
        /// 平台 4:IOS,2:android
        /// </summary>
        public int os { get; set; }

        /// <summary>
        /// 上次活动时间
        /// </summary>
        public DateTime? last_time { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime created_date { get; set; }

        /// <summary>
        /// 状态0:注销，1:登录
        /// </summary>
        public bool is_enable { get; set; }


    }
}