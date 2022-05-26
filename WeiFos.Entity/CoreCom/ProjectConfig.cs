using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.CoreCom
{
    /// <summary>
    /// 项目
    /// </summary>
    [Table(Name = "tb_prj_config")]
    [Serializable]
    public class ProjectConfig
    {

        /// <summary>
        /// 主键
        /// </summary>
        [ID]
        public long id { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 项目秘钥
        /// </summary>
        public string pkey { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        public string pvalue { get; set; }

        /// <summary>
        /// 是否校验
        /// </summary>
        public bool is_check { get; set; }

        /// <summary>
        /// 票据时间
        /// </summary>
        public DateTime? expiry_date   {  get; set;  }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string remark { get; set; }


    }
}
