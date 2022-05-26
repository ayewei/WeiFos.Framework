using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.WeChatModule
{
    /// <summary>
    /// 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
    /// Copyright (c) 2013-2018 深圳微狐信息技术有限公司
    /// 创 建：叶委
    /// 日 期：2019-03-15 14:38:04
    /// 描 述：公众号接口授权配置明细表
    /// </summary>
    [Serializable]
    [Table(Name = "tb_wx_api_auth")]
    public class WeChatAuth
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { set; get; }

        /// <summary>
        /// 门店ID
        /// </summary>
        public long store_id { get; set; }

        /// <summary>
        /// 1：普通公众号内部接入API模式
        /// 2：开发平台授权模式
        /// 3：企业号内部接入api模式
        /// 4：企业号外部授权模式
        /// </summary>
        public int auth_type { get; set; }

        /// <summary>
        /// 配置授权所需参数key
        /// 例如 component_access_token ,ticket 等等
        /// 全部json 格式化数据存放
        /// </summary>
        public string auth_key { get; set; }

        /// <summary>
        /// 配置value
        /// </summary>
        public string val { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime update_time { get; set; }

        /// <summary>
        /// 凭证的有效时间 
        /// </summary>
        public int expires_in { get; set; }

        /// <summary>
        /// 提前更新时间 
        /// </summary>
        public int expires_lead { get; set; }

    }
}
