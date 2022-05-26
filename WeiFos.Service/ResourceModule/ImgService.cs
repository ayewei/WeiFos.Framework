using WeiFos.ORM.Data;
using System.Collections.Generic;
using WeiFos.Entity.ResourceModule;

namespace WeiFos.Service.ResourceModule
{
    /// <summary>
    /// 微狐平台图片 Service
    /// @author yewei 
    /// @date 2013-12-11
    /// </summary>
    public class ImgService : BaseService<Img>
    {

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="imgbase"></param>
        public string[] Save(Img img)
        {
            base.Insert(img);
            return new string[] { img.file_name, img.biz_type };
        }

        /// <summary>
        /// 保存图片自动完成
        /// </summary>
        /// <param name="imgbase"></param>
        public string[] SaveAutoComplete(Img img)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                s.StartTransaction();

                base.Insert(img);

                //去除重复图片
                s.ExcuteUpdate("update tb_img set biz_id = 0 where biz_type = @0 and biz_id = @1 ", img.biz_type, img.biz_id);

                //去除重复图片
                s.ExcuteUpdate("update tb_img set biz_id = @0 where biz_type = @1 and file_name = @2  ", img.biz_id, img.biz_type, img.file_name);

                s.Commit();
            }

            return new string[] { img.file_name, img.biz_type };
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public Img GetImg(string filename)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<Img>(" where file_name = @0 ", filename);
            }
        }

        /// <summary>
        /// 根据图片名称和类型获取
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="imgtype"></param>
        /// <returns></returns>
        public Img GetImg(string filename, int biztype)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<Img>(" where file_name = @0 and biz_type = @1", filename, biztype);
            }
        }


        /// <summary>
        /// 根据业务Id获取
        /// </summary>
        /// <param name="biztype"></param>
        /// <param name="bizId"></param>
        /// <returns></returns>
        public Img GetImg(string biztype, long bizId)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Get<Img>("where biz_type = @0 and biz_id = @1", biztype, bizId);
            }
        }


        /// <summary>
        /// 获取图片集合
        /// </summary>
        /// <param name="biztype"></param>
        /// <param name="bizId"></param>
        /// <returns></returns>
        public List<Img> GetImgs(string biztype, long bizId)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.List<Img>("where biz_type = @0 and biz_id = @1", biztype, bizId);
            }
        }

        /// <summary>
        /// 获取图片集合
        /// </summary>
        /// <param name="biztypes"></param>
        /// <param name="bizId"></param>
        /// <returns></returns>
        public List<Img> GetImgs(string[] biztypes, long bizId)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //暂时未处理除图片以外的文件
                List<Img> imgs = new List<Img>();

                foreach (string biztype in biztypes)
                {
                    imgs.AddRange(s.List<Img>("where biz_type = @0 and biz_id = @1", biztype, bizId));
                }
                return imgs;
            }
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="biztype"></param>
        /// <param name="filename"></param>
        public void Delete(string biztype, string filename)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                s.ExcuteUpdate("delete tb_img where file_name = @0 and biz_type = @1", filename, biztype);
            }
        }


        /// <summary>
        /// 获取图片物理路径
        /// </summary>
        /// <param name="biztype"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string GetImgPath(string biztype, string filename)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                Img img = s.Get<Img>(" where file_name = @0 and biz_type = @1", filename, biztype);
                if (img != null)
                {
                    if (img.is_webimg)
                        return string.Empty;

                    return img.file_path + img.file_name + img.extend_name;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取图片web路径
        /// </summary>
        /// <param name="biztype"></param>
        /// <param name="bizid"></param>
        /// <returns></returns>
        public string GetImgUrl(string biztype, long bizid)
        {
            return GetImgUrl(biztype, bizid, false, -1, "");
        }

        /// <summary>
        /// 获取图片web路径，不存在路径返回默认传入的默认路径
        /// </summary>
        /// <param name="biztype"></param>
        /// <param name="bizid"></param>
        /// <param name="defimgsrc"></param>
        /// <returns></returns>
        public string GetImgUrl(string biztype, long bizid, string defimgsrc)
        {
            return GetImgUrl(biztype, bizid, false, -1, defimgsrc);
        }


        /// <summary>
        /// 获取图片地址
        /// </summary>
        /// <param name="biztype"></param>
        /// <param name="bizid"></param>
        /// <param name="is_main"></param>
        /// <param name="size_type"></param>
        /// <returns></returns>
        public string GetImgUrl(string biztype, long bizid, bool is_main, int size_type)
        {
            return GetImgUrl(biztype, bizid, is_main, size_type, "");
        }


        /// <summary>
        /// 获取图片地址
        /// </summary>
        /// <param name="biztype"></param>
        /// <param name="bizid"></param>
        /// <param name="is_main"></param>
        /// <param name="size_type"></param>
        /// <param name="defimgsrc"></param>
        /// <returns></returns>
        public string GetImgUrl(string biztype, long bizid, bool is_main, int size_type, string defimgsrc)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                Img img;
                if (is_main)
                {
                    img = s.Get<Img>(" where biz_type = @0 and biz_id = @1 and is_main = @2", biztype, bizid, is_main);
                }
                else
                {
                    img = s.Get<Img>(" where biz_type = @0 and biz_id = @1", biztype, bizid);
                }

                if (img != null)
                {
                    if (img.is_webimg) return img.webimg_url;
                    switch (size_type)
                    {
                        //小图
                        case 1:
                            if (!string.IsNullOrEmpty(img.visit_path)) img.visit_path = img.visit_path.Replace("/Image/", "/Thm_Image/");
                            break;
                        //中图
                        case 2:
                            if (!string.IsNullOrEmpty(img.visit_path)) img.visit_path = img.visit_path.Replace("/Image/", "/Med_Image/");
                            break;
                        default:
                            break;
                    }
                    return (img.domain_name.EndsWith("/") && img.visit_path.StartsWith("/")) ? (img.domain_name + img.visit_path.Substring(1) + img.file_name + img.extend_name) : (img.domain_name + img.visit_path + img.file_name + img.extend_name);
                    //return img.domain_name + img.visit_path + img.file_name + img.extend_name;
                }
                else
                {
                    return defimgsrc;
                }
            }
        }



    }

}
