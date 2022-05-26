using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using WeiFos.Core.ExcelModule;

namespace WeiFos.NetCore.ExcelModule
{
    public static class ExcelHelper
    {



        /// <summary>
        /// 输出文件到浏览器
        /// </summary>
        /// <param name="ms">Excel文档流</param>
        /// <param name="context">HTTP上下文</param>
        /// <param name="fileName">文件名</param>
        private static void RenderToBrowser(MemoryStream ms, HttpContext context, string fileName)
        {
            //浏览器信息
            string userAgent = context.Request.Headers["User-Agent"];
            //IE浏览器
            string regexStr = @"msie (?<ver>[\d.]+)";
            //regexStr = @"firefox\/([\d.]+)";
            //regexStr = @"opera\/([\d.]+)";
            //regexStr = @"version\/([\d.]+)"; //Safari
            //regexStr = @"chrome\/([\d.]+)";
            Regex r = new Regex(regexStr, RegexOptions.IgnoreCase);
            Match m = r.Match(userAgent);
            if (m.Success)
            {
                fileName = HttpUtility.UrlEncode(fileName);
            }

            if (context.Request.Headers["User-Agent"] == "IE")
                context.Response.Headers.Add("Content-Disposition", "attachment;fileName=" + fileName);
            //context.Response.WriteAsync(ms.ToArray());
        }


        /// <summary>
        /// DataReader转换成Excel文档流，并输出到客户端
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="context">HTTP上下文</param>
        /// <param name="fileName">输出的文件名</param>
        public static void RenderToExcel(IDataReader reader, HttpContext context, string fileName)
        {
            using (MemoryStream ms = ExcelRender.RenderToExcel(reader))
            {
                RenderToBrowser(ms, context, fileName);
            }
        }

        /// <summary>
        /// DataTable转换成Excel文档流，并输出到客户端
        /// </summary>
        /// <param name="table"></param>
        /// <param name="response"></param>
        /// <param name="fileName">输出的文件名</param>
        public static void RenderToExcel(DataTable table, HttpContext context, string fileName)
        {
            using (MemoryStream ms = ExcelRender.RenderToExcel(table))
            {
                RenderToBrowser(ms, context, fileName);
            }
        }




    }
}
