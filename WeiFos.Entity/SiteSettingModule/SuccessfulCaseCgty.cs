using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.SiteSettingModule
{
    /// <summary>
    /// 成功案例
    /// @author yewei 
    /// @date 2015-01-09
    /// </summary>
    [Serializable]
    [Table(Name = "tb_cases_cgty")]
    public class SuccessfulCaseCgty : BaseClass
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public long id { get; set; }


        /// <summary>
        /// 标题
        /// </summary>
        public string name { get; set; }


        /// <summary>
        /// 前缀编号
        /// </summary>
        public string prefix_no { get; set; }


        /// <summary>
        /// 类别简介
        /// </summary>
        public string introduction { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        public int order_index { get; set; }
         

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool is_enable { get; set; }


    }
}
