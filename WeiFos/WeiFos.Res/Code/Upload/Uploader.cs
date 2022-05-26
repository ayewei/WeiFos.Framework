using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using WeiFos.Admin;
using WeiFos.Core;
using WeiFos.Core.SettingModule;
using WeiFos.Entity.BizTypeModule;
using WeiFos.Entity.Enums;
using WeiFos.Entity.ResourceModule;
using WeiFos.Service;
using WeiFos.Service.LogsModule;
using WeiFos.Service.ResourceModule;


namespace WeiFos.Res.Code.Upload
{

    /// <summary>
    /// 文件写入实体类
    /// @author yewei 
    /// add by @date 2015-01-16
    /// </summary>
    public class Uploader
    {

        #region 上传模块

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static dynamic UploadFile(HttpContext HttpContext)
        {
            //创建缩略图类型0:不创建，1:创建小图，2:创建中图，3:创建中图和小图
            int moreSize = 0;
            int.TryParse(HttpContext.Request.Query.ContainsKey("createThmImg") ? HttpContext.Request.Query["createThmImg"] : HttpContext.Request.Form["createThmImg"], out moreSize);

            //业务ID
            long bizId = 0;
            long.TryParse(HttpContext.Request.Query.ContainsKey("bizId") ? HttpContext.Request.Query["bizId"] : HttpContext.Request.Form["bizId"], out bizId);

            //业务类型
            string bizType = HttpContext.Request.Query.ContainsKey("bizType") ? HttpContext.Request.Query["bizType"] : HttpContext.Request.Form["bizType"];

            //批量写入文件
            return UploadFolder(HttpContext, moreSize, bizType, bizId);
        }


        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="Files"></param>
        /// <param name="moreSize"></param>
        /// <param name="biz_type"></param>
        /// <param name="biz_id"></param>
        /// <returns></returns>
        public static async Task<dynamic> UploadFolderToAsync(HttpContext HttpContext)
        {
            //创建缩略图类型0:不创建，1:创建小图，2:创建中图，3:创建中图和小图
            int moreSize = 0;
            int.TryParse(HttpContext.Request.Query.ContainsKey("createThmImg") ? HttpContext.Request.Query["createThmImg"] : HttpContext.Request.Form["createThmImg"], out moreSize);

            //业务ID
            long bizId = 0;
            long.TryParse(HttpContext.Request.Query.ContainsKey("bizId") ? HttpContext.Request.Query["bizId"] : HttpContext.Request.Form["bizId"], out bizId);

            //业务类型
            string bizType = HttpContext.Request.Query.ContainsKey("bizType") ? HttpContext.Request.Query["bizType"] : HttpContext.Request.Form["bizType"];

            //批量写入文件
            return await UploadFolder(HttpContext, moreSize, bizType, bizId, true);
        }


        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <param name="moreSize"></param>
        /// <param name="biz_type"></param>
        /// <param name="biz_id"></param>
        /// <param name="is_async">是否是异步线程写入</param>
        /// <returns></returns>
        private static async Task<dynamic> UploadFolder(HttpContext HttpContext, int moreSize, string biz_type, long biz_id, bool is_async = false)
        {
            //错误消息
            string error = string.Empty, url = string.Empty, original = string.Empty, data = string.Empty;

            //状态
            StateCode state = StateCode.State_200;

            //结果集合
            List<dynamic> result = new List<dynamic>();

            //当前资源站点域名 
            string domain = AppGlobal.Res;

            if (HttpContext.Request.Form.Files.Count() > 0)
            {
                foreach (var file in HttpContext.Request.Form.Files)
                {
                    //原文件名
                    original = file.FileName;

                    //访问路径 文件名
                    string[] filedata = { };
                    try
                    {
                        filedata = Uploader.GetUploadPath(biz_type, biz_id, original);
                        //资源根目录
                        string localPath = Directory.CreateDirectory("wwwroot/" + filedata[0] + "/").FullName;

                        //获取图片基本信息
                        BaseRes res = GetBaseRes(biz_type, localPath, filedata[0], filedata[1].Split('.')[0], original, file.Length, domain);
                        Img img = (Img)res;
                        img.biz_type = biz_type;
                        if (ImgType.User.Equals(biz_type))
                        {
                            img.biz_id = biz_id;
                            SaveAutoComplete(img);
                        }
                        else
                        {
                            //保存图片
                            SaveImgMsg(img);
                        }

                        //文件字节
                        byte[] fileBytes = new byte[file.Length];

                        //文件扩展名
                        string f_name = img.extend_name.ToLower();
                        var is_img = ((".gif".Equals(f_name) || ".jpg".Equals(f_name) || ".jpeg".Equals(f_name) || ".bmp".Equals(f_name) || ".png".Equals(f_name)));

                        //如果是图片类型
                        if (is_img)
                        {
                            if (file.Length < int.Parse(ConfigManage.AppSettings<string>("UploadSettings:imageMaxSize")))
                            {
                                //写入图片
                                if (await Uploader.WriteFile(file, localPath, filedata[1], is_async) && is_img)
                                {
                                    switch (moreSize)
                                    {
                                        //创建小图
                                        case 1:
                                            CreateThumbnailPicture(localPath + filedata[1], GetThmUrl(localPath), filedata[1]);
                                            break;
                                        //创建中图
                                        case 2:
                                            CreateThumbnailPicture(localPath + filedata[1], GetMedUrl(localPath), filedata[1], true);
                                            break;
                                        //创建小图和中图
                                        case 3:
                                            CreateThumbnailPicture(localPath + filedata[1], GetThmUrl(localPath), filedata[1]);
                                            CreateThumbnailPicture(localPath + filedata[1], GetMedUrl(localPath), filedata[1], true);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            await Uploader.WriteFile(file, localPath, filedata[1], is_async);
                        }

                        url = domain + filedata[0] + filedata[1];
                        data = filedata[1].Split('.')[0];
                    }
                    catch (Exception e)
                    {
                        state = StateCode.State_500;
                        error = e.Message;
                    }

                    //图片列表
                    result.Add(new
                    {
                        key = data,
                        val = url,
                        original,
                        state
                    });
                }
            }


            return new
            {
                state,
                url,
                original,
                data,
                result,
                error
            };
        }



        #endregion

        /// <summary>
        /// 创建缩略图
        /// </summary>
        /// <param name="srcPath">源图片</param>
        /// <param name="destPath">目标图片</param>
        /// <param name="imgname"></param>
        public static void CreateThumbnailPicture(string srcPath, string destPath, string imgname)
        {
            CreateThumbnailPicture(srcPath, destPath, imgname, false);
        }


        /// <summary>
        /// 创建缩略图
        /// </summary>
        /// <param name="srcPath">源图片</param>
        /// <param name="destPath">目标图片</param>
        /// <param name="imgname">目标图片</param>
        /// <param name="width">宽度</param>
        public static void CreateThumbnailPicture(string srcPath, string destPath, string imgname, bool create_mimg)
        {
            //根据图片的磁盘绝对路径获取 源图片 的Image对象
            Image img = Image.FromFile(srcPath);

            //源图片宽和高
            int imgWidth = img.Width, imgHeight = img.Height;

            //缩略图
            int width, height;

            //创建中图
            if (create_mimg)
            {
                width = 400;
                height = 400;
            }
            else
            {
                width = 150;
                height = 150;
            }

            //宽、高按比例设定
            if (imgWidth > width || imgHeight > height)
            {
                if (imgWidth > imgHeight)
                {
                    height = Convert.ToInt32(width / float.Parse(imgWidth.ToString()) * imgHeight);
                }
                else
                {
                    width = Convert.ToInt32(height / float.Parse(imgHeight.ToString()) * imgWidth);
                }
            }
            else
            {
                width = img.Width;
                height = imgHeight;
            }

            //bmp： 最终要建立的 微缩图 位图对象,宽高按比例缩放
            Bitmap bmp = new Bitmap(width, height);

            //g: 绘制 bmp Graphics 对象
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Transparent);
            //为Graphics g 对象 初始化必要参数，很容易理解。
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            //绘制微缩图
            g.DrawImage(img, new System.Drawing.Rectangle(0, 0, width, height), new System.Drawing.Rectangle(0, 0, imgWidth, imgHeight), GraphicsUnit.Pixel);

            ImageFormat format = img.RawFormat;
            ImageCodecInfo info = ImageCodecInfo.GetImageEncoders().SingleOrDefault(i => i.FormatID == format.Guid);
            EncoderParameter param = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
            EncoderParameters parameters = new EncoderParameters(1);
            parameters.Param[0] = param;
            img.Dispose();

            //如果不存在，则创建
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            //保存已生成微缩图，这里将GIF格式转化成png格式。
            if (format == ImageFormat.Gif)
            {
                destPath = destPath.ToLower().Replace(".gif", ".png");
                bmp.Save(destPath + imgname, ImageFormat.Png);
            }
            else
            {
                if (info != null)
                {
                    bmp.Save(destPath + imgname, info, parameters);
                }
                else
                {
                    bmp.Save(destPath + imgname, format);
                }
            }

            img.Dispose();
            g.Dispose();
            bmp.Dispose();
        }


        /// <summary>
        /// 暂不删除文件
        /// 只删除数据记录
        /// </summary>
        /// <returns></returns>
        public static StateCode DeleteFile(HttpContext context)
        {
            try
            {
                //图片业务类型
                string biztype = context.Request.Query["bizType"];
                string imgmsg = context.Request.Query["imgmsg"];

                switch (biztype)
                {
                    //case ImgType.IDCard:
                    //    ServiceIoc.Get<IDCardImgService>().Delete(imgmsg);
                    //    break;
                    default:
                        ServiceIoc.Get<ImgService>().Delete(biztype, imgmsg);
                        break;
                }
                return StateCode.State_200;
            }
            catch (Exception ex)
            {
                ServiceIoc.Get<SystemLogsService>().Save(1, ex.ToString());
                return StateCode.State_500;
            }
        }



        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="file"></param>
        /// <param name="localPath"></param>
        /// <param name="filename"></param>
        /// <param name="uploadFileBytes"></param>
        /// <returns></returns>
        public static async Task<bool> WriteFile(IFormFile file, string localPath, string filename, bool is_async = false)
        {
            try
            {
                using (var stream = new FileStream(localPath + filename, FileMode.Create))
                {
                    if (is_async)
                    {
                        await file.CopyToAsync(stream);
                    }
                    else
                    {
                        file.CopyTo(stream);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }



        /// <summary>
        /// 获取文件访问路径
        /// 和文件名
        /// </summary>
        /// <param name="bizType"></param>
        /// <param name="bizId"></param>
        /// <param name="up_filename"></param>
        /// <returns></returns>
        public static string[] GetUploadPath(string bizType, long bizId, string up_filename)
        {
            //图片访问路径
            StringBuilder visitPath = new StringBuilder();

            //日期文件夹
            string data_path = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2");

            //文件扩展名
            string extension = Path.GetExtension(up_filename);

            //业务类型文件夹
            if (!ImgType.exist(bizType))
            {
                bizType = ImgType.Default;
            }

            visitPath.Append("/Upload/").Append("Image/").Append(bizType).Append("/").Append(data_path).Append("/").Append(bizId).Append("/");
            return new string[] { visitPath.ToString(), System.Guid.NewGuid().ToString("N") + extension };
        }


        /// <summary>
        /// 获取合并图路径（默认jpg格式）
        /// </summary>
        /// <param name="bizType"></param>
        /// <returns></returns>
        public static string[] GetUploadPath(string bizType, string bizId)
        {
            //图片访问路径
            StringBuilder visitPath = new StringBuilder();

            //日期文件夹
            string data_path = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2");

            //业务类型文件夹
            if (!ImgType.exist(bizType))
            {
                bizType = ImgType.Default;
            }

            visitPath.Append("/Content/Upload/Image/").Append(bizType).Append("/").Append(bizId).Append("/").Append(data_path).Append("/");

            return new string[] { visitPath.ToString(), System.Guid.NewGuid().ToString("N") + ".jpg" };
        }



        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="img"></param>
        /// <param name="tb_name"></param>
        /// <returns></returns>
        public static string[] SaveImgMsg(Img img)
        {
            return ServiceIoc.Get<ImgService>().Save(img);
        }



        /// <summary>
        /// 保存图片自动完成
        /// </summary>
        /// <param name="imgbase"></param>
        public static string[] SaveAutoComplete(Img img)
        {
            return ServiceIoc.Get<ImgService>().SaveAutoComplete(img);
        }



        /// <summary>
        /// 获取图片信息
        /// </summary>
        /// <param name="bizType"></param>
        /// <param name="filePath"></param>
        /// <param name="visitPath"></param>
        /// <param name="fileName"></param>
        /// <param name="originName"></param>
        /// <param name="filelength"></param>
        /// <returns></returns>
        public static BaseRes GetBaseRes(string bizType, string filePath, string visitPath, string fileName, string originName, long filelength, string domain)
        {
            //图片基本信息
            BaseRes img;
            switch (bizType)
            {
                case ImgType.SysUser:
                    img = new Img();
                    break;
                default:
                    img = new Img();
                    break;
            }
            //业务ID
            img.biz_id = 0;
            //资源服务器域名
            img.domain_name = domain;
            //扩展名称
            img.extend_name = Path.GetExtension(originName);
            //文件名称
            img.file_name = fileName.Split('.')[0];
            //原文件名
            img.original = originName;
            //文件访问物理路径
            img.file_path = filePath;
            //服务器路径
            img.visit_path = visitPath;
            //文件大小
            img.file_size = ((float)filelength / 1000) + "kb";
            //上传时间
            img.upload_time = DateTime.Now;

            return img;
        }


        /// <summary>
        /// 获取缩略图(小图)路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetThmUrl(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;
            else
            {
                path = path.Replace(@"\Image\", @"\Thm_Image\");
            }
            return path.ToString();
        }


        /// <summary>
        /// 获取缩略图(中图)路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetMedUrl(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;
            else
            {
                path = path.Replace(@"\Image\", @"\Med_Image\");
            }
            return path.ToString();
        }


        /// <summary>
        /// 校验上传
        /// </summary>
        /// <returns></returns>
        public static bool CheckUploadFile(HttpContext HttpContext)
        {
            //加密字符串参数
            string ticket = HttpContext.Request.Query.ContainsKey("ticket") ? HttpContext.Request.Query["ticket"] : HttpContext.Request.Form["ticket"];
            if (string.IsNullOrEmpty(ticket)) return false;

            //解密字符串
            ticket = StringHelper.GetDecryption(ticket);
            if (ticket.IndexOf("#") != -1)
            {
                string s1 = ticket.Split('#')[0];
                string s2 = ticket.Split('#')[1];
                //业务类型
                string bizType = HttpContext.Request.Query.ContainsKey("bizType") ? HttpContext.Request.Query["bizType"] : HttpContext.Request.Form["bizType"];
                //业务ID
                string bizId = HttpContext.Request.Query.ContainsKey("bizId") ? HttpContext.Request.Query["bizId"] : HttpContext.Request.Form["bizId"];

                if (!s1.Equals(bizType) || !s2.Equals(bizId))
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }


    }
}