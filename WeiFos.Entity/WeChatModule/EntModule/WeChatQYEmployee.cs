using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;
using WeiFos.WeChat.Models.OrgEntity;

namespace WeiFos.Entity.WeChatModule.EntModule
{
    /// <summary>
    /// 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
    /// Copyright (c) 2013-2018 深圳微狐信息技术有限公司
    /// 创 建：叶委
    /// 日 期：2019-03-15 14:32:37
    /// 描 述：企业号员工信息
    /// </summary>
    [Serializable]
    [Table(Name = "tb_wx_ent_empl")]
    public class WeChatQYEmployee
    {

        /// <summary>
        /// 主键ID（自增）
        /// </summary>
        /// <returns></returns>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 对应用户ID
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int order_index { get; set; }

        /// <summary>
        /// 成员头像的mediaid，通过素材管理接口上传图片获得的mediaid
        /// </summary>
        [UnMapped]
        public string avatar_mediaid { get; set; }


        #region 对应微信字段

        /// <summary>
        /// 成员UserID。对应管理端的帐号，企业内必须唯一。
        /// 不区分大小写，长度为1~64个字节
        /// </summary>
        public string userid { get; set; }

        /// <summary>
        /// 成员名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string mobile { get; set; }
         

        /// <summary>
        /// 职务信息；第三方仅通讯录应用可获取
        /// </summary>
        public string position { get; set; }

        /// <summary>
        /// 性别。0表示未定义，1表示男性，2表示女性
        /// </summary>
        public int gender { get; set; }

        /// <summary>
        /// email
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 头像url。注：如果要获取小图将url最后的”/0”改成”/100”即可。第三方仅通讯录应用可获取
        /// </summary>
        public string avatar { get; set; }

        /// <summary>
        /// 座机。第三方仅通讯录应用可获取
        /// </summary>
        public string telephone { get; set; }

        /// <summary>
        /// 成员启用状态。1表示启用的成员，0表示被禁用。注意，服务商调用接口不会返回此字段
        /// </summary>
        public int enable { get; set; }

        /// <summary>
        /// 别名；第三方仅通讯录应用可获取
        /// </summary>
        public string alias { get; set; }

        /// <summary>
        ///  激活状态: 1=已激活，2=已禁用，4=未激活。
        ///  已激活代表已激活企业微信或已关注微工作台（原企业号）。
        ///  未激活代表既未激活企业微信又未关注微工作台（原企业号）
        /// </summary>
        public int status { get; set; }

        /// <summary>
        ///  员工个人二维码，扫描可添加为外部联系人；第三方仅通讯录应用可获取
        /// </summary>
        public string qr_code { get; set; }

        /// <summary>
        /// external_position
        /// </summary>
        public string external_position { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }


        #endregion


        /// <summary>
        /// 创建用户
        /// </summary>
        public long? created_user_id { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? created_date { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime? updated_date { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public long? updated_user_id { get; set; }

    }
}
