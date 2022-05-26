using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Web;
using WeiFos.Core;
using WeiFos.Core.Extensions;
using WeiFos.Core.SettingModule;
using WeiFos.Entity.BizTypeModule;
using WeiFos.NetCore.Extensions;
using WeiFos.Entity.Enums;
using WeiFos.Entity.ResourceModule;
using WeiFos.Service;
using WeiFos.Service.ResourceModule;

namespace WeiFos.Res.Code.Upload
{

    /// <summary>
    /// OSS图片上传
    /// @author yewei 
    /// add by @date 2015-01-16
    /// </summary>
    public class OSSUpload
    {

        public static dynamic UploadFile(HttpContext context)
        {
            //状态位
            StateCode state = StateCode.State_200;
            //错误消息
            string error = string.Empty;
            //原文件名
            string originName = string.Empty;
            //上传文件地址
            string fileurl = string.Empty;
            //文件名
            string newfilename = string.Empty;

            //OSS AccessId
            string accessId = ConfigManage.AppSettings<string>("AppSettings:AccessId");
            //OSS AccessKey
            string accessKey = ConfigManage.AppSettings<string>("AppSettings:AccessKey");
            //OSS endpoint
            string endpoint = ConfigManage.AppSettings<string>("AppSettings:endpoint");
            //OSS 图片Bucket
            string PicBucket = ConfigManage.AppSettings<string>("AppSettings:PicBucket");
            //OSS 图片域名
            string PicDomain = ConfigManage.AppSettings<string>("AppSettings:PicDomain");

            try
            {
                //业务类型
                string bizType = NHttpContext.Current.Request.Query["bizType"];

                //业务IDTicketID
                string bizId = NHttpContext.Current.Request.Query["bizId"];
                bizId = string.IsNullOrEmpty(bizId) ? "0" : StringHelper.GetDecryption(bizId);

                //上传file 名称
                //string filedName = Config.GetString("imageFieldName");

                ////获取file 数据
                //var file = HttpContext.Current.Request.Files[filedName];

                ////原文件名
                //originName = file.FileName;

                ////OSS 上传
                //string firstName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(100000, 999999);
                //string lastName = Path.GetExtension(file.FileName);
                //string fullName = firstName + lastName;

                ////写入数据库信息
                //if (file.ContentLength < int.Parse(Config.GetString("imageMaxSize")))
                //{
                //    string[] imgmsg = OSSUpload.SaveImgOss(bizType, PicDomain, firstName, originName, file.ContentLength);
                //}

                //ObjectMetadata metadata = new ObjectMetadata();
                //metadata.ContentType = file.ContentType;

                //OssClient ossClient = new OssClient(endpoint, accessId, accessKey);

                //using (var fs = file.InputStream)
                //{
                //    var ret = ossClient.PutObject(PicBucket, fullName, fs, metadata);
                //}
                ////图片全路径
                //fileurl = PicDomain + fullName;
                ////原始图片名
                //originName = file.FileName;
                ////新图片名
                //newfilename = firstName;
            }
            catch (Exception e)
            {
                state = StateCode.State_500;
                error = e.Message;
            }

            var backdata = new
            {
                state = (int)state,
                url = fileurl,
                original = originName,
                data = newfilename,
                error = error
            };

            return backdata;

        }

        /// <summary>
        /// 保存OSS图片信息方法
        /// </summary>
        /// <param name="bizType"></param>
        /// <param name="ossDomain"></param>
        /// <param name="fileName"></param>
        /// <param name="originName"></param>
        /// <param name="filelength"></param>
        /// <returns></returns>
        public static string[] SaveImgOss(string bizType, string ossDomain, string fileName, string originName, int filelength)
        {
            Img img = new Img();
            //业务类型
            img.biz_type = bizType;
            //业务ID
            img.biz_id = 0;
            //资源服务器域名
            img.domain_name = ossDomain;
            //扩展名称
            img.extend_name = Path.GetExtension(originName);
            //文件名称
            img.file_name = fileName.Split('.')[0];
            //原文件名
            img.original = originName;
            //文件访问物理路径
            img.file_path = "";
            //服务器路径
            img.visit_path = "";
            //文件大小
            img.file_size = ((float)filelength / 1000) + "kb";
            //上传时间
            img.upload_time = DateTime.Now;

            return ServiceIoc.Get<ImgService>().Save(img);
        }




    }
}