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
    [Table(Name = "tb_wx_ent_empl_dept")]
    public class WeChatEmplDept
    {

        /// <summary>
        /// 用户ID
        /// </summary>
        public string user_id { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public long dept_id { get; set; }

        /// <summary>
        /// 是否是部门领导
        /// </summary>
        public bool is_leader_in_dept { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int order_index { get; set; }

    }
}
