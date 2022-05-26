using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.WeChatModule.WXOpen
{
    /// <summary>
    /// 开放平台凭据缓存表
    /// @author yewei 
    /// add by @date 2018-04-13
    /// </summary>
    [Serializable]
    [Table(Name = "tb_wx_open_auth")]
    public class WXOpenAuth
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


    }
}
