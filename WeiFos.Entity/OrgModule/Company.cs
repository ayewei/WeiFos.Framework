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
    [Table(Name = "tb_org_company")]
    public class Company : BaseClass
    {

        #region 实体成员

        /// <summary>
        /// 主键ID（自增）
        /// </summary>
        /// <returns></returns>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        /// <returns></returns>
        public long parent_id { get; set; }

        /// <summary>
        /// 公司全称
        /// </summary>
        /// <returns></returns>
        public string name { get; set; }

        /// <summary>
        /// 部门简称
        /// </summary>
        /// <returns></returns>
        public string for_short { get; set; }

        /// <summary>
        /// 英文名称
        /// </summary>
        /// <returns></returns>
        public string en_name { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        /// <returns></returns>
        public string manager { get; set; }

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
        /// 邮编
        /// </summary>
        /// <returns></returns>
        public string postal_code { get; set; }

        /// <summary>
        /// 经营范围
        /// </summary>
        /// <returns></returns>
        public string biz_scope { get; set; }

        /// <summary>
        /// 官网
        /// </summary>
        /// <returns></returns>
        public string site_url { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        /// <returns></returns>
        public string remarks { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        /// <returns></returns>
        public string province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        /// <returns></returns>
        public string city { get; set; }

        /// <summary>
        /// 区县
        /// </summary>
        /// <returns></returns>
        public string county { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        /// <returns></returns>
        public string address { get; set; }

        /// <summary>
        /// 成立时间
        /// </summary>
        /// <returns></returns>
        public DateTime f_time { get; set; }

        /// <summary>
        /// 公司性质
        /// 1:国家机关
        /// 2:房地产
        /// 3:建筑业
        /// 4:社会服务业
        /// 5:IT/互联网
        /// 6:制造业
        /// 7:金融业
        /// 8:其他业
        /// </summary>
        /// <returns></returns>
        public int nature { get; set; }

        /// <summary>
        /// 显示排序
        /// </summary>
        /// <returns></returns>
        public int order_index { get; set; }


        /// <summary>
        /// 子节点
        /// </summary>
        /// <returns></returns>
        [UnMapped]
        public List<Company> childs { get; set; }

        #endregion

    }
}