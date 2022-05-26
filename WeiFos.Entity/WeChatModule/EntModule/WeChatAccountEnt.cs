using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.WeChatModule.EntModule
{
    /// <summary>
    /// 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
    /// Copyright (c) 2013-2018 深圳微狐信息技术有限公司
    /// 创 建：叶委
    /// 日 期：2019-03-15 14:32:37
    /// 描 述：企业号信息
    /// </summary>
    [Serializable]
    [Table(Name = "tb_wx_account_ent")]
    public class WeChatAccountEnt : BaseClass
    {

        #region 实体成员

        /// <summary>
        /// 主键ID（自增）
        /// </summary>
        /// <returns></returns>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 企业微信的corpid
        /// </summary>
        /// <returns></returns>
        public string corpid { get; set; }

        /// <summary>
        /// 企业微信的应用id
        /// </summary>
        /// <returns></returns>
        public string agentid { get; set; }

        /// <summary>
        /// 企业微信的应用秘钥
        /// </summary>
        /// <returns></returns>
        public string corpsecret { get; set; }

        #endregion

    }
}