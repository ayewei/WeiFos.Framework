using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.UserModule
{
    /// <summary>
    /// 用户详细实体类
    /// @author yewei 
    /// @date 2016-03-20
    /// </summary>
    [Serializable]
    [Table(Name = "tb_user_details")]
    public class UserDetail
    {

        /// <summary>
        /// 用户ID 自增
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string nickname { get; set; }

        /// <summary>
        /// 微信号
        /// </summary>
        public string we_chat_no { get; set; }

        /// <summary>
        /// QQ号码
        /// </summary>
        public string qq { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int sex { get; set; }

        /// <summary>
        /// 出生年月
        /// </summary>
        public DateTime? birth { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string province { get; set; }

        /// <summary>
        /// 县市
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        public string area { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string address { get; set; }

    }
}