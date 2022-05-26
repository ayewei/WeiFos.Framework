using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.WeChatModule
{
    /// <summary>
    /// 微信会员用户实体类
    /// @author yewei 
    /// @date 2015-05-17
    /// </summary>
    [Serializable]
    [Table(Name = "tb_wx_user_msg")]
    public class WeChatUserMsg
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// openid
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; } 

        /// <summary>
        /// 手机号码
        /// </summary>
        public string mobile_phone { get; set; }

        /// <summary>
        /// 店铺名称
        /// </summary>
        public string store_name { get; set; }

        /// <summary>
        /// 店铺地址
        /// </summary>
        public string store_address { get; set; }

        /// <summary>
        /// 职务
        /// </summary>
        public string duties { get; set; }

        /// <summary>
        /// 行业
        /// </summary>
        public string industry { get; set; }
         
    }
}
