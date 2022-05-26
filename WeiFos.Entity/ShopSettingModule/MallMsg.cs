using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.ShopSettingModule
{
    /// <summary>
    /// 商城信息设置
    /// @author yewei 
    /// @date 2013-11-22
    /// </summary>
    [Serializable]
    [Table(Name = "tb_fnt_mallmsg")]
    public class MallMsg
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public int id { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string mall_name { get; set; }

        /// <summary>
        /// 分享标题
        /// </summary>
        public string share_title { get; set; }

        /// <summary>
        /// 分享内容
        /// </summary>
        public string share_content { get; set; }

        /// <summary>
        /// 服务电话
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        public string zip_code { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        public string fax { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// 版权
        /// </summary>
        public string copyright { get; set; }


    }

}