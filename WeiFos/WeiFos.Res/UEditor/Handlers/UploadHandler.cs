using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WeiFos.Core.SettingModule;
using WeiFos.Entity.Enums;
using WeiFos.Res.Code.Upload;

namespace WeiFos.Res.UEditor.Handlers
{
    /// <summary>
    /// 文件上传
    /// </summary>
    public class UploadHandler : Handler
    {
        /// <summary>
        /// 上传文件配置
        /// </summary>
        public UploadConfig UploadConfig { get; private set; }

        /// <summary>
        /// 上传响应结果
        /// </summary>
        public UploadResult Result { get; private set; }

        /// <summary>
        /// 文件上传处理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="config"></param>
        public UploadHandler(HttpContext context, UploadConfig config) : base(context)
        {
            this.UploadConfig = config;
            this.Result = new UploadResult() { State = UploadState.Unknown };
        }


        /// <summary>
        /// 处理进程
        /// </summary>
        public async override void Process()
        {
            byte[] uploadFileBytes = null;  //文件内容
            string uploadFileName = null;   //文件名 
            if (UploadConfig.Base64)
            {
                uploadFileName = UploadConfig.Base64Filename;
                uploadFileBytes = Convert.FromBase64String(Request.Form[UploadConfig.UploadFieldName]);
            }
            else { }

            try
            {
                //上传状态
                StateCode state = StateCode.State_500;
                //原文件名，url地址，data:上传后新生成的名
                string original = string.Empty, url = string.Empty, data = string.Empty;
                //是否使用OSS上传功能
                if (ConfigManage.AppSettings<bool>("WeChatSettings:IsOpenOss"))
                {
                    var result = OSSUpload.UploadFile(Context);
                    state = result.state;
                    url = result.url;
                    data = result.data;
                    original = result.original;
                }
                else
                {
                    if (Uploader.CheckUploadFile(Context))
                    {
                        var result = await Uploader.UploadFile(Context); 
                        state = result.state;
                        url = result.url;
                        data = result.data;
                        original = result.original;
                    }
                    else
                    {
                        Result.ErrorMessage = "上传票据校验失败";
                    }
                }

                if (state == StateCode.State_200)
                {
                    Result.State = UploadState.Success;
                }

                //原始路径名
                Result.OriginFileName = original;
                Result.Url = url;
                Result.Data = data;
            }
            catch (Exception e)
            {
                Result.State = UploadState.FileAccessError;
                Result.ErrorMessage = e.Message;
            }
            finally
            {
                WriteResult();
            }
        }


        private void WriteResult()
        {
            this.WriteJson(new
            {
                state = GetStateMessage(Result.State),
                url = Result.Url,
                title = Result.OriginFileName,
                original = Result.OriginFileName,
                data = Result.Data,
                error = Result.ErrorMessage
            });
        }


        private string GetStateMessage(UploadState state)
        {
            switch (state)
            {
                case UploadState.Success:
                    return "SUCCESS";
                case UploadState.FileAccessError:
                    return "文件访问出错，请检查写入权限";
                case UploadState.SizeLimitExceed:
                    return "文件大小超出服务器限制";
                case UploadState.TypeNotAllow:
                    return "不允许的文件格式";
                case UploadState.NetworkError:
                    return "网络错误";
            }
            return "未知错误";
        }

        private bool CheckFileType(string filename)
        {
            var fileExtension = Path.GetExtension(filename).ToLower();
            return UploadConfig.AllowExtensions.Select(x => x.ToLower()).Contains(fileExtension);
        }

        private bool CheckFileSize(int size)
        {
            return size < UploadConfig.SizeLimit;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UploadConfig
    {
        /// <summary>
        /// 文件命名规则
        /// </summary>
        public string PathFormat { get; set; }

        /// <summary>
        /// 文件保存路径：绝对路径
        /// </summary>
        public string SaveAbsolutePath { get; set; }
        /// <summary>
        /// 是否 FTP 上传
        /// </summary>
        public bool FtpUpload { get; set; }

        /// <summary>
        /// FTP 账户
        /// </summary>
        public string FtpAccount { get; set; }

        /// <summary>
        /// FTP 密码
        /// </summary>
        public string FtpPwd { get; set; }

        /// <summary>
        /// IP 地址
        /// </summary>
        public string FtpIp { get; set; }

        /// <summary>
        /// 上传表单域名称
        /// </summary>
        public string UploadFieldName { get; set; }

        /// <summary>
        /// 上传大小限制
        /// </summary>
        public int SizeLimit { get; set; }

        /// <summary>
        /// 上传允许的文件格式
        /// </summary>
        public string[] AllowExtensions { get; set; }

        /// <summary>
        /// 文件是否以 Base64 的形式上传
        /// </summary>
        public bool Base64 { get; set; }

        /// <summary>
        /// Base64 字符串所表示的文件名
        /// </summary>
        public string Base64Filename { get; set; }
    }

    public class UploadResult
    {
        public UploadState State { get; set; }
        public string Url { get; set; }
        public string OriginFileName { get; set; }
        public string Data { get; set; }
        public string ErrorMessage { get; set; }
    }

    public enum UploadState
    {
        Success = 0,
        SizeLimitExceed = -1,
        TypeNotAllow = -2,
        FileAccessError = -3,
        NetworkError = -4,
        Unknown = 1,
    }
}