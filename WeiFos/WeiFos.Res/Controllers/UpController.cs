using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeiFos.Core.SettingModule;
using WeiFos.Entity.Enums;
using WeiFos.Res.Code;
using WeiFos.Res.Code.Upload;

namespace WeiFos.Res.Controllers
{
    /// <summary>
    /// 图片上传控制器
    /// @author yewei 
    /// add by @date 2015-02-15
    /// </summary>
    public class UpController : Controller
    {



        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [CheckUp]
        [HttpPost]
        public async Task<IActionResult> UploadFile(List<IFormFile> files)
        {
            if (ConfigManage.AppSettings<bool>("WeChatSettings:IsOpenOss"))
            {
                //弱类型在这里不能初始化值，所以必须重新定义
                var result = Uploader.UploadFile(HttpContext);
                return Ok(result);
            }
            else
            {  
                var result = await Uploader.UploadFolderToAsync(HttpContext);
                return Ok(result);
            }
        }



        /// <summary>
        /// 删除文件操作
        /// </summary>
        /// <returns></returns>
        [CheckUp]
        [HttpGet]
        public IActionResult DeleteFile()
        {
            StateCode code = Uploader.DeleteFile(HttpContext);
            return Ok(code);
        }



        //[AppCheckUp]
        //[HttpPost]
        //public Task<IActionResult> AppUpload()
        //{
        //    if (Settings.IsOpenOss)
        //    {
        //        //弱类型在这里不能初始化值，所以必须重新定义
        //        var result = Uploader.UploadFile(HttpContext.Current);
        //        return APIResponse.toJson(result);
        //    }
        //    else
        //    {
        //        var result = Uploader.UploadFile(HttpContext.Current);
        //        return APIResponse.toJson(result);
        //    }
        //}


    }
}