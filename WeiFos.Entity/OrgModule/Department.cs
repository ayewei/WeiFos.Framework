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
    /// 创 建：yewei
    /// 日 期：2019-01-03 12:54:19
    /// 描 述：部门表
    /// </summary>
    [Serializable]
    [Table(Name = "tb_org_department")]
    public class Department : BaseClass
    {

        #region 实体成员

        /// <summary>
        /// id
        /// </summary>
        /// <returns></returns>
        [ID]
        public long id { get; set; }


        /// <summary>
        /// 微信部门ID
        /// </summary>
        public long wx_dept_id { get; set; }


        /// <summary>
        /// 微信部门上级ID
        /// </summary>
        public long wx_dept_parent_id { get; set; }


        /// <summary>
        /// 公司ID
        /// </summary>
        /// <returns></returns>
        public long company_id { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        /// <returns></returns>
        public string name { get; set; }

        /// <summary>
        /// 部门编号
        /// </summary>
        /// <returns></returns>
        public string no { get; set; }

        /// <summary>
        /// 部门简称
        /// </summary>
        /// <returns></returns>
        public string for_short { get; set; }

        /// <summary>
        /// 1 综合性 
        /// 2 生产性
        /// 3 咨询性
        /// 4 协调性
        /// 0 其他性
        /// </summary>
        /// <returns></returns>
        public int nature { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        /// <returns></returns>
        public string tel { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        /// <returns></returns>
        public string fax { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        /// <returns></returns>
        public string email { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        /// <returns></returns>
        public string remarks { get; set; }

        /// <summary>
        /// 所属上级
        /// </summary>
        /// <returns></returns>
        public long parent_id { get; set; }

        /// <summary>
        /// 显示排序
        /// </summary>
        /// <returns></returns>
        public int order_index { get; set; }

        /// <summary>
        /// 子部门集合
        /// </summary>
        [UnMapped]
        public List<Department> childs { get; set; }


        #endregion

    }
}