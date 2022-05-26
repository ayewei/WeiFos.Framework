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
    [Table(Name = "tb_code_server_link")]
    public class ServerLink : BaseClass
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [ID]
        public int id { get; set; }

        /// <summary>
        /// 数据库类型
        /// 服务器类型（1：mysql，5：sql server，10：oracle）
        /// </summary>
        public int db_type { get; set; }

        /// <summary>
        /// 链接名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string db_name { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string ip { get; set; }

        /// <summary>
        /// 链接名称
        /// </summary>
        public string login_name { get; set; }

        /// <summary>
        /// 链接名称
        /// </summary>
        public string psw { get; set; }

        /// <summary>
        /// 数据库链接
        /// </summary>
        public string connection_str { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string remarks { get; set; }

        /// <summary>
        /// 项目集合
        /// </summary>
        [UnMapped]
        public List<ProjectSetting> projects { get; set; }


    }
}
