using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;
using WeiFos.WeChat.Models;
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
    public class WeChatQYEmployeeResult 
    {

        /// <summary>
        /// 微信员工信息
        /// </summary>
        /// <returns></returns>
        public List<WeChatQYEmployee> userlist { get; set; }


    }
}
