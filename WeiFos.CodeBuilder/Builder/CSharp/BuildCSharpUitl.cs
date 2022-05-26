using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.CodeBuilder.Entity;
using WeiFos.ORM.Data.DBEntityModule;

namespace WeiFos.CodeBuilder.Builder.CSharp
{
    /// <summary>
    /// 通用代码生成类
    /// @author yewei 
    /// @date 2018-11-17
    /// </summary>
    public class BuildCSharpUitl
    {


        /// <summary>
        /// 注释头 三斜杠方式
        /// </summary>
        /// <param name="author"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static string NotesCreate1(string author, string desc)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("    /// <summary>\r\n");
            sb.Append("    /// 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架\r\n");
            sb.Append("    /// Copyright (c) 2013-2018 深圳微狐信息技术有限公司\r\n");
            sb.Append("    /// 创 建：" + author + "\r\n");
            sb.Append("    /// 日 期：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n");
            sb.Append("    /// 描 述：" + desc + "\r\n");
            sb.Append("    /// </summary>\r\n");
            return sb.ToString();
        }


        /// <summary>
        /// 注释头 *号方式
        /// </summary>
        /// <param name="author"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static string NotesCreate2(string author, string desc)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("/*!\r\n");
            sb.Append(" * 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架\r\n");
            sb.Append(" * Copyright (c) 2013-2018 深圳微狐信息技术有限公司\r\n");
            sb.Append(" * 创 建：" + author + "\r\n");
            sb.Append(" * 日 期：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n");
            sb.Append(" * 描 述：" + desc + "\r\n");
            sb.Append(" */ \r\n");
            return sb.ToString();
        }




        #region 操作扩展


       
        /// <summary>
        /// 自动生成对象名
        /// 例 tb_seo_page => SeoPage
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetClassName(string name)
        {
            //返回空字符串
            if (string.IsNullOrEmpty(name)) return string.Empty;

            //表名称结果集
            StringBuilder tb_name = new StringBuilder();

            name = name.Replace("tb_", "");
            if (name.IndexOf('_') == -1)
            {
                if (name.Length == 1)
                {
                    tb_name.Append(name.ToUpper());
                }
                else
                {
                    string first = name.Substring(0, 1).ToUpper();
                    string surplus = name.Substring(1, name.Length - 1).ToUpper();
                    tb_name.Append(first + surplus);
                }
            }
            else
            {
                //获取截取的字符串
                string[] arr = name.Split('_');

                string str = arr[1];
                string first = str.Substring(0, 1).ToUpper();
                string surplus = str.Substring(1, str.Length - 1);
                tb_name.Append(first + surplus);

                //首字母小写转大写
                //for (int i = 0; i < arr.Length; i++)
                //{
                //    string str = arr[i];
                //    if (str.Length == 1)
                //    {
                //        tb_name.Append(str.ToUpper());
                //    }
                //    else
                //    {
                //        string first = str.Substring(0, 1).ToUpper();
                //        string surplus = str.Substring(1, str.Length - 1);
                //        tb_name.Append(first + surplus);
                //    }
                //}
            }

            return tb_name.ToString();
        }



        /// <summary>
        /// 是否具备基础类
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static bool HasBaseClass(List<FieldDetail> fields)
        {
            int i = 0;
            foreach (var field in fields)
            {
                if ("created_user_id".Equals(field.name)) i++;
                if ("created_date".Equals(field.name)) i++;
                if ("updated_user_id".Equals(field.name)) i++;
                if ("updated_date".Equals(field.name)) i++;
            }

            return i == 4;
        }



        /// <summary>
        /// 是否是基类字段
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static bool IsBaseFiled(string field)
        {
            if ("created_user_id".Equals(field)) return true;
            if ("created_date".Equals(field)) return true;
            if ("updated_user_id".Equals(field)) return true;
            if ("updated_date".Equals(field)) return true;
            return false;
        }



        /// <summary>
        /// 根据命名空间获取模块名称
        /// </summary>
        /// <param name="name_space"></param>
        /// <returns></returns>
        public static string GetModuleName(string name_space)
        {
            //命名空间
            if (string.IsNullOrEmpty(name_space)) return string.Empty;
            if (name_space.IndexOf(".") != -1)
            {
                int last_index = name_space.LastIndexOf(".");
                return name_space.Substring(last_index + 1, name_space.Length - last_index - 1);
            }
            return name_space;
        }

        #endregion



    }
}
