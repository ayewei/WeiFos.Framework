using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.SystemModule
{
    /// <summary>
    /// 短信消息 实体类
    /// @author yewei 
    /// add by @date 2015-10-09
    /// </summary>
    [Serializable]
    [Table(Name = "tb_sms")]
    public class SmsMessage
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 类型
        /// 1注册，5 绑定新手机
        /// 10忘记密码，15申请换房
        /// 20取消换房,100邮箱
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 短信内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime created_time { get; set; }

    }
}