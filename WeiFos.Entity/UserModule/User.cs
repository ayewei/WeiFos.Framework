using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.Entity.Enums;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.UserModule
{
    /// <summary>
    /// 会员实体类
    /// @author yewei 
    /// @date 2013-09-21
    /// </summary>
    [Serializable]
    [Table(Name = "tb_user")]
    public class User
    {

        #region Model

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 是否绑定微信
        /// </summary>
        public bool is_bind_wechat { get; set; }

        /// <summary>
        /// 登陆账号
        /// </summary>
        public string login_name { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        public string user_name { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
        public string psw { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int state { get; set; }

        /// <summary>
        /// 联系号码
        /// </summary>
        public string mobile { get; set; }

		/// <summary>
		/// 登陆次数
		/// </summary>
        public int login_count { get; set; }

		/// <summary>
		/// QQ号码
		/// </summary>
        public string qq { get; set; }

		/// <summary>
		/// 邮箱
		/// </summary>
        public string email { get; set; }

		/// <summary>
		/// 配额
		/// </summary>
        public int quota { get; set; }

		/// <summary>
		/// 上次登陆时间
		/// </summary>
        public DateTime last_logintime { get; set; }

        /// <summary>
        /// 登陆IP地址
        /// </summary>
        public string ip_address { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime created_date { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime? updated_date { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool is_enable { get; set; }

        /// <summary>
        /// 商家ID（系统用户ID）
        /// </summary>
        public int sysuser_id { get; set; }

        /// <summary>
        /// 用户图像
        /// </summary>
        [UnMapped]
        public string head_img { get; set; }

        /// <summary>
        /// 登录状态码
        /// </summary>
        [UnMapped]
        public StateCode login_code { get; set; }

        #endregion Model

    }


}
