using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.SystemModule
{
    /// <summary>
    /// 系统用户 公司信息实体类
    /// @author yewei 
    /// @date 2013-12-03
    /// </summary>
    [Serializable]
    [Table(Name = "tb_sys_usercompanymsg")]
    public class SysUserCompanyMsg
    {

        #region Model

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public int id { get; set; }

        /// <summary>
        /// 系统用户id
        /// </summary>
        public long sysuser_id { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string company_name { get; set; }

        /// <summary>
        /// 公司简介
        /// </summary>
        public string introduction { get; set; }

        /// <summary>
        /// 公司网址
        /// </summary>
        public string company_site { get; set; }

        /// <summary>
        /// 公司电话
        /// </summary>
        public string tel { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// 区县
        /// </summary>
        public string area { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string address { get; set; }


        #endregion Model

    }

}
