using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.OrgModule
{
    /// <summary>
    /// 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
    /// Copyright (c) 2013-2018 深圳微狐信息技术有限公司
    /// 创 建：
    /// 日 期：2018-12-05 11:53:51
    /// 描 述：公司表
    /// </summary>
    [Serializable]
    [Table(Name = "tb_org_employee")]
    public class Employee
    {

        #region 实体成员

        /// <summary>
        /// 主键ID（自增）
        /// </summary>
        /// <returns></returns>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 公司ID
        /// </summary>
        /// <returns></returns>
        public long? company_id { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        /// <returns></returns>
        public long? department_id { get; set; }

        /// <summary>
        /// 系统用户ID
        /// </summary>
        /// <returns></returns>
        public long? sys_user_id { get; set; }

        /// <summary>
        /// 员工编号
        /// </summary>
        public string no { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 职位ID
        /// </summary>
        public long? post_id { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 上次登录IP
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// QQ号码
        /// </summary>
        public string qq { get; set; }

        /// <summary>
        /// 微信号
        /// </summary>
        public string wechat_no { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public bool sex { get; set; }

        /// <summary>
        /// 出生年月
        /// </summary>
        public DateTime birthday { get; set; }

        /// <summary>
        /// 登录IP
        /// </summary>
        public string province { get; set; }

        /// <summary>
        /// 登录IP
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// 区县
        /// </summary>
        public string county { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int order_index { get; set; }
        
        #endregion

    }
}