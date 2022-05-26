using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiFos.Entity.ResourceModule
{
    /// <summary>
    /// 资源基本信息 实体类
    /// @author yewei 
    /// add by @date 2015-08-09
    /// </summary>
    public abstract class BaseRes
    {

        /// <summary>
        /// 业务ID
        /// </summary>
        public long biz_id { get; set; }

        /// <summary>
        /// 访问路径
        /// </summary>
        public string visit_path { get; set; }

        /// <summary>
        /// 存储路径
        /// </summary>
        public string file_path { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string file_name { get; set; }

        /// <summary>
        /// 原文件名称
        /// </summary>
        public string original { get; set; }

        /// <summary>
        /// 扩展名
        /// </summary>
        public string extend_name { get; set; }

        /// <summary>
        /// 域名
        /// </summary>
        public string domain_name { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public string file_size { get; set; }

        /// <summary>
        /// 排列序号
        /// </summary>
        public int order_index { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime upload_time { get; set; }

        /// <summary>
        /// 获取图片路径
        /// </summary>
        /// <returns></returns>
        public string getVisitUrl()
        {
            return domain_name + visit_path + file_name + extend_name;
        }

        /// <summary>
        /// 获取图片服务器物理路径
        /// </summary>
        /// <returns></returns>
        public string getPath()
        {
            string path = file_path + visit_path.Replace("/", "\\") + file_name + extend_name;
            return path;
        }



    }
}

