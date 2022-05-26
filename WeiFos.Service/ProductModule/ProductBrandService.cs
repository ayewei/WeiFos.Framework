using WeiFos.ORM.Data;
using WeiFos.ORM.Data.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.Entity.ProductModule;
using WeiFos.Entity.BizTypeModule;
using WeiFos.Core;
using WeiFos.Entity.ResourceModule;
using WeiFos.Entity.Enums;
using WeiFos.Entity.SystemModule;

namespace WeiFos.Service.ProductModule
{

    public class ProductBrandService : BaseService<ProductBrand>
    {


        /// <summary>
        /// 检查供应商-品牌名称是否存在
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="id"></param>
        /// <param name="brandName"></param>
        /// <returns></returns>
        public int ExistBrandName(string brandName, long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                return s.Exist<ProductBrand>("where name = @0 and id != @1", brandName, id);
            }
        }



        /// <summary>
        /// 保存供应商品牌
        /// </summary>
        /// <param name="sys_user_id"></param>
        /// <param name="entity"></param>
        /// <param name="imgmsg"></param>
        /// <returns></returns>
        public StateCode Save(long sys_user_id, ProductBrand entity, string imgmsg)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();
                    if (entity.id != 0)
                    {
                        entity.updated_date = DateTime.Now;
                        entity.updated_user_id = sys_user_id;
                        s.Update(entity);
                    }
                    else
                    {
                        entity.created_date = DateTime.Now;
                        entity.created_user_id = sys_user_id;
                        s.Insert(entity);
                    }

                    //判断是否存在图片信息
                    if (!string.IsNullOrEmpty(imgmsg))
                    {
                        //去除重复图片
                        s.ExcuteUpdate("update tb_img set biz_id = 0 where biz_type = @0 and biz_id = @1 ", ImgType.Product_Brand, entity.id);
                        //去除重复图片
                        s.ExcuteUpdate("update tb_img set biz_id = @0 where biz_type = @1 and file_name = @2  ", entity.id, ImgType.Product_Brand, imgmsg);
                    }
                    s.Commit();
                    return StateCode.State_200;
                }
                catch
                {
                    s.RollBack();
                    return StateCode.State_500;
                }
            }
        }



        /// <summary>
        /// 删除品牌
        /// </summary>
        /// <param name="sysUser"></param>
        /// <param name="brand"></param>
        /// <param name="imgmsg"></param>
        /// <returns></returns>
        public StateCode DeleteBrand(long id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                try
                {
                    s.StartTransaction();

                    //是否商品引用
                    if (s.Exist<Product>("where brand_id = @0 ", id) > 0)
                    {
                        s.RollBack();
                        return StateCode.State_503;
                    }

                    //删除品牌数据
                    s.ExcuteUpdate("delete tb_pdt_brand where id = @0 ", id);

                    //删除品牌图片数据
                    s.ExcuteUpdate("delete tb_img where biz_type = @0 and biz_id = @1 ", ImgType.Product_Brand, id);

                    s.Commit();
                    return StateCode.State_200;
                }
                catch
                {
                    s.RollBack();
                    return StateCode.State_500;
                }
            }
        }



        /// <summary>
        /// 根据类别ID获取
        /// </summary>
        /// <param name="ctgy_id"></param>
        /// <returns></returns>
        public dynamic GetByCtgyId(long ctgy_id)
        {
            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                //返回结果集合
                List<dynamic> result = new List<dynamic>();

                //获取当前品牌集合
                List<ProductBrand> brands = s.List<ProductBrand>("where cgty_id = @0", ctgy_id);
                foreach (ProductBrand b in brands)
                {
                    Img img = s.Get<Img>("where biz_type = @0 and biz_id = @1", ImgType.Product_Brand, b.id);
                    var data = new
                    {
                        id = b.id,
                        name = b.name,
                        src = img == null ? "" : img.getThmImgUrl()
                    };

                    result.Add(data);
                }
                return result;
            }
        }



        /// <summary>
        /// 根据品牌
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<dynamic> GetBrandByIds(string ids)
        {
            //品牌信息
            List<dynamic> brands = new List<dynamic>();

            using (ISession s = SessionFactory.Instance.CreateSession())
            {
                long[] arr = StringHelper.StringToLongArray(ids);

                foreach (long id in arr)
                {
                    if (id > 0)
                    {
                        //品牌信息
                        ProductBrand brand = s.Get<ProductBrand>(id);
                        if (brand != null)
                        {
                            string imgurl = "";
                            //获取当前对应图片
                            Img img = s.Get<Img>("where biz_type = @0 and biz_id = @1", ImgType.Product_Brand, id);
                            imgurl = img == null ? "" : img.getImgUrl();
                            var b = new
                            {
                                id = brand.id,
                                name = brand.name,
                                imgurl = imgurl
                            };

                            brands.Add(b);
                        }
                    }
                }
            }

            return brands;
        }





    }
}
