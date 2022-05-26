using System;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.OrgModule
{
    /// <summary>
    /// 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
    /// Copyright (c) 2013-2018 深圳微狐信息技术有限公司
    /// 创 建：叶委
    /// 日 期：2019-02-03 14:06:16
    /// 描 述：岗位表
    /// </summary>
    [Serializable]
    [Table(Name = "tb_org_post")]
    public class OrgPost : BaseClass
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
        public long company_id { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        /// <returns></returns>
        public long department_id { get; set; }

        /// <summary>
        /// 岗位编号
        /// </summary>
        /// <returns></returns>
        public string no { get; set; }

        /// <summary>
        /// 岗位名称
        /// </summary>
        /// <returns></returns>
        public string name { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        /// <returns></returns>
        public string remarks { get; set; }

        /// <summary>
        /// 上级岗位
        /// </summary>
        /// <returns></returns>
        public long parent_id { get; set; }

        /// <summary>
        /// 显示排序
        /// </summary>
        /// <returns></returns>
        public int order_index { get; set; }

        #endregion

    }
}
