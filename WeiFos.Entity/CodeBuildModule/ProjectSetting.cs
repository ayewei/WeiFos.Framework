using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.Attributes;

namespace WeiFos.Entity.CodeBuildModule
{
    /// <summary>
    /// 项目配置 实体对象
    /// @author yewei 
    /// @date 2018-11-13
    /// </summary>
    [Serializable]
    [Table(Name = "tb_code_project_setting")]
    public class ProjectSetting : BaseClass
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public int id { get; set; }


        /// <summary>
        /// 连接ID
        /// </summary>
        public int link_id { get; set; }
        

        /// <summary>
        /// 项目名称
        /// </summary>
        public string name { get; set; }


        /// <summary>
        /// 项目命名空间
        /// 模块英文名称（将作为命名空间第一级节点名称）
        /// </summary>
        public string en_name { get; set; }

        /// <summary>
        /// 项目模块集合
        /// </summary>
        [UnMapped]
        public List<ProjectModule> modules { get; set; }


    }
}
