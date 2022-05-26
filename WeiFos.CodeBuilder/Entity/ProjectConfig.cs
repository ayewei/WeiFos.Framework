using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.ORM.Data.DBEntityModule;

namespace WeiFos.CodeBuilder.Entity
{

    /// <summary>
    /// 代码生成 项目配置类
    /// @author yewei
    /// @date 2014-02-14
    /// </summary>
    public class ProjectConfig
    {

        /// <summary>
        /// 命名空间
        /// </summary>
        public string name_space { get; set; }

        /// <summary>
        /// 生成物理路径
        /// </summary>
        public string base_path { get; set; }

        /// <summary>
        /// 生成实体对应命名空间
        /// </summary>
        public Dictionary<string, List<TableInfo>> entity_map { get; set; }

        /// <summary>
        /// 生成业务逻辑对应命名空间
        /// </summary>
        public Dictionary<string, List<TableInfo>> service_map { get; set; }

        /// <summary>
        /// 生成JavaScript对应命名空间
        /// </summary>
        public Dictionary<string, List<TableInfo>> script_map { get; set; }

        /// <summary>
        /// 生成页面对应文件夹
        /// </summary>
        public Dictionary<string, List<TableInfo>> page_map { get; set; }

    }
}
