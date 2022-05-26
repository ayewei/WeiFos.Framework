using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.CodeBuilder.Entity
{

    /// <summary>
    /// C# MVC代码生成实体
    /// @author yewei 
    /// @date 2018-11-17
    /// </summary
    public class CSharpMVCCode
    {

        /// <summary>
        /// 实体
        /// </summary>
        public string entity { get; set; }

        /// <summary>
        /// 业务逻辑
        /// </summary>
        public string service { get; set; }

        /// <summary>
        /// 表单页
        /// </summary>
        public string view_form { get; set; }

        /// <summary>
        /// 管理页
        /// </summary>
        public string view_manage { get; set; }

        /// <summary>
        /// 表单脚本页
        /// </summary>
        public string js_form { get; set; }

        /// <summary>
        /// 管理页脚本
        /// </summary>
        public string js_manage { get; set; }

        /// <summary>
        /// 控制器Action
        /// </summary>
        public string action { get; set; }

    }
}
