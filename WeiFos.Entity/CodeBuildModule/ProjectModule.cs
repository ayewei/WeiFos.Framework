using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.CodeBuildModule
{
    /// <summary>
    /// 服务器链接实体对象
    /// @author yewei 
    /// @date 2018-11-13
    /// </summary>
    [Serializable]
    [Table(Name = "tb_code_project_module")]
    public class ProjectModule : BaseClass
    {


        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public int id { get; set; }


        /// <summary>
        /// 项目配置ID
        /// </summary>
        public int project_setting_id { get; set; }


        /// <summary>
        /// 项目名称
        /// </summary>
        public string name { get; set; }


        /// <summary>
        /// 项目英文名称
        /// 模块英文名称（将作为命名空间第二级节点名称）
        /// </summary>
        public string en_name { get; set; } 


    }
}
