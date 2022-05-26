using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WeiFos.Core.SettingModule;
using Yahoo.Yui.Compressor;

namespace WeiFos.Core.ScriptCompress
{
    /// <summary>
    /// 版 本 Weifos-Framework 微狐敏捷开发框架
    /// Copyright (c) 2013-2017 深圳微狐信息科技有限公司
    /// 创建人：叶委-叶委
    /// 日 期：2018.03.07
    /// 描 述：js,css,文件压缩和下载
    /// </summary>
    public class JsCssHelper
    {
        private static JavaScriptCompressor javaScriptCompressor = new JavaScriptCompressor();
        private static CssCompressor cssCompressor = new CssCompressor();


        #region Js 文件操作
        /// <summary>
        /// 读取js文件内容并压缩
        /// </summary>
        /// <param name="filePathlist"></param>
        /// <returns></returns>
        public static string ReadJSFile(string[] filePathlist, string rootPath = null)
        {
            StringBuilder jsStr = new StringBuilder();
            try
            {
                //string rootPath = Assembly.GetExecutingAssembly().CodeBase.Replace("/bin/Learun.Util.DLL", "").Replace("file://", "/");
                //存在虚拟机获取到的是相对路径，转换绝对路径
                //assembleFileName = Path.GetFullPath((new Uri(rootPath)).LocalPath);

                //DirectoryInfo Dir = Directory.GetParent(AppContext.BaseDirectory);
                //Dir.Parent.Parent.FullName

                if (string.IsNullOrEmpty(rootPath)) rootPath = AppContext.BaseDirectory;
                foreach (var filePath in filePathlist)
                {
                    string path = rootPath + filePath;
                    if (File.Exists(path))
                    {
                        string content = File.ReadAllText(path, Encoding.UTF8);
                        if (ConfigManage.AppSettings<bool>("AppSettings:JsCompressor"))
                        {
                            content = javaScriptCompressor.Compress(content);
                        }
                        jsStr.Append(content);
                    }
                }
                return jsStr.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }
        #endregion

        #region Css 文件操作
        /// <summary>
        /// 读取css 文件内容并压缩
        /// </summary>
        /// <param name="filePathlist"></param>
        /// <returns></returns>
        public static string ReadCssFile(string[] filePathlist)
        {
            StringBuilder cssStr = new StringBuilder();
            try
            { 
                string rootPath = AppContext.BaseDirectory;
                foreach (var filePath in filePathlist)
                {
                    string path = rootPath + filePath;
                    if (File.Exists(path))
                    {
                        string content = File.ReadAllText(path, Encoding.UTF8);
                        content = cssCompressor.Compress(content);
                        cssStr.Append(content);
                    }
                }
                return cssStr.ToString();
            }
            catch (Exception)
            {
                return cssStr.ToString();
            }
        }
        #endregion
    }
}
