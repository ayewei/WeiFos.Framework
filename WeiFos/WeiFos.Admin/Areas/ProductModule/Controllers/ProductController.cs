using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using WeiFos.Admin.Controllers;
using WeiFos.Core;
using WeiFos.Core.XmlHelper;
using WeiFos.Entity.BizTypeModule;
using WeiFos.Entity.Enums;
using WeiFos.Entity.SystemModule;
using WeiFos.Entity.ProductModule;
using WeiFos.Entity.ResourceModule;
using WeiFos.Entity.SKUModule;
using WeiFos.ORM.Data;
using WeiFos.ORM.Data.Const;
using WeiFos.ORM.Data.Restrictions;
using WeiFos.Service;
using WeiFos.Service.ProductModule;
using WeiFos.Service.ResourceModule;
using WeiFos.Service.SKUModule;
using WeiFos.Admin.Code.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeiFos.NetCore.Extensions;

namespace WeiFos.Admin.Areas.ProductModule.Controllers
{

    /// <summary>
    /// 商品控制器
    /// @author yewei 
    /// add by @date 2015-03-10
    /// </summary>
    [LoginAuth]
    [Area(AreaNames.ProductModule)]
    public class ProductController : BaseController
    {

        #region 商品管理——商品分类

        /// <summary>
        /// 加载类别数据
        /// </summary>
        /// <returns></returns>
        public JsonResult LoadCgtyChildren(SysUser user)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder cgtys = new StringBuilder();

            //商品类别集合
            List<ProductCatg> productCgtys = new List<ProductCatg>();

            //父类集合      
            List<ProductCatg> parentCgtys = new List<ProductCatg>();

            //当前分类所有的父类
            parentCgtys = ServiceIoc.Get<ProductCatgService>().GetParents(bid);

            try
            {
                if (parentCgtys.Count != 0)
                {
                    //商品分类
                    foreach (ProductCatg pc in parentCgtys)
                    {
                        string datas = "";
                        if (pc.parent_id == 0)
                        {
                            DataTable dt = ServiceIoc.Get<ProductCatgService>().Fill("select * from v_pdt_productcatg where parent_id = @0 ", 0);
                            datas = JsonConvert.SerializeObject(dt);
                            datas = datas.Substring(1, datas.Length - 2);

                            if (cgtys.Length > 0) cgtys.Append(",").Append(datas);
                            else cgtys.Append(datas);
                        }
                        else
                        {
                            DataTable dt = ServiceIoc.Get<ProductCatgService>().Fill("select * from v_pdt_productcatg where parent_id = @0", pc.parent_id);
                            datas = JsonConvert.SerializeObject(dt);
                            datas = datas.Substring(1, datas.Length - 2);

                            if (cgtys.Length > 0) cgtys.Append(",").Append(datas);
                            else cgtys.Append(datas);
                        }
                    }
                }
                else
                {
                    DataTable dt = ServiceIoc.Get<ProductCatgService>().Fill("select * from v_pdt_productcatg where parent_id = @0 ", 0);
                    cgtys.Append(JsonConvert.SerializeObject(dt));

                    string datas = JsonConvert.SerializeObject(dt);
                    datas = datas.Substring(1, datas.Length - 2);

                    if (cgtys.Length > 0) cgtys.Append(",").Append(datas);
                    else cgtys.Append(datas);

                    parentCgtys.Add(new ProductCatg());
                }

                sb.Append("{");
                sb.Append("\"parents\":").Append(JsonConvert.SerializeObject(parentCgtys)).Append(",");
                sb.Append("\"cgtys\":").Append("[" + cgtys.ToString() + "]");
                sb.Append("}");

                return Json(GetResult(StateCode.State_200, sb.ToString()));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }


        /// <summary>
        /// 加载类别数据
        /// </summary>
        /// <returns></returns>
        public ContentResult GetCgtyChildrens()
        {
            //获取当前子类别
            DataTable dt = ServiceIoc.Get<ProductCatgService>().Fill("select * from v_pdt_productcatg where parent_id = @0", bid);
            return ContentResult(StateCode.State_200, dt);
        }


        /// <summary>
        /// 商品分类列表
        /// </summary>
        /// <param name="brandID"></param>
        /// <returns></returns>
        public IActionResult CategoryManage()
        {
            return View();
        }


        /// <summary>
        /// 商品分类分页
        /// </summary>
        /// <returns></returns>
        public ContentResult GetProductCatgs(int pageSize, int pageIndex, string keyword)
        {
            try
            {
                //查询对象
                Criteria ct = new Criteria();

                //查询表达式
                MutilExpression me = new MutilExpression();

                ct.SetFromTables("tb_pdt_category")
                .SetPageSize(pageSize)
                .SetStartPage(pageIndex)
                .SetFields(new string[] { "*" })
                .AddOrderBy(new OrderBy("order_index", "desc"));

                //登录名称
                if (!string.IsNullOrEmpty(keyword))
                {
                    me.Add(new SingleExpression("name", LogicOper.LIKE, keyword));
                }

                //设置查询条件
                if (me.Expressions.Count > 0)
                {
                    ct.SetWhereExpression(me);
                }

                DataTable dt = ServiceIoc.Get<ProductCatgService>().Fill(ct);

                return PageResult(StateCode.State_200, ct.TotalRow, dt);
            }
            catch (Exception ex)
            {
                return PageResult(StateCode.State_500, 0, null);
            }
        }


        /// <summary>
        /// 保存商品分类
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entity"></param>
        /// <param name="imgmsg"></param>
        /// <returns></returns>
        public IActionResult CategoryForm(SysUser user, ProductCatg? entity = null, string? imgmsg = null)
        {
            if (NHttpContext.Current.Request.IsAjaxRequest())
            {
                //获取状态
                StateCode state = ServiceIoc.Get<ProductCatgService>().Save(user, entity, imgmsg);
                return Json(GetResult(state));
            }
            else
            {
                //所属分类
                List<ProductCatg> Parents = ServiceIoc.Get<ProductCatgService>().GetTrees("", HttpUtility.HtmlDecode("&nbsp;&nbsp;"));
                Parents.Insert(0, new ProductCatg() { name = "根", id = 0 });
                ViewBag.Parents = Parents;
                //当前商品分类
                entity = ServiceIoc.Get<ProductCatgService>().GetById(bid);
                if (entity != null)
                {
                    ViewBag.entity = JsonConvert.SerializeObject(entity);
                }
            }
            return View();
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public JsonResult DeleteProductCatg()
        {
            try
            {
                ServiceIoc.Get<ProductCatgService>().Delete(bid);
                return Json(GetResult(StateCode.State_200));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }


        /// <summary>
        /// 保存商品分类排序索引
        /// </summary>
        /// <returns></returns>
        public JsonResult SaveOrderIndex(string data)
        {
            try
            {
                string[] arr = StringHelper.StringToArray(data);
                ServiceIoc.Get<ProductCatgService>().SaveOrderIndex(arr);
                return Json(GetResult(StateCode.State_200));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }


        /// <summary>
        /// 查看商品分类名称是否存在
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int ExistCategoryName(string name)
        {
            return ServiceIoc.Get<ProductCatgService>().ExistName(bid, name);
        }


        /// <summary>
        /// 查看商品分类编号是否存在
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int ExistSerialNo(string serial_no)
        {
            return ServiceIoc.Get<ProductCatgService>().ExistSerialNo(bid, serial_no);
        }


        /// <summary>
        /// 保存商品类别
        /// </summary>
        /// <returns></returns>
        public JsonResult SaveProductCgty(SysUser user, ProductCatg productCgty, string imgMsg)
        {
            //获取状态
            StateCode state = ServiceIoc.Get<ProductCatgService>().Save(user, productCgty, imgMsg);
            return Json(GetResult(state));
        }


        #endregion


        #region 商品管理——导购管理 

        /// <summary>
        /// 前端导购分类
        /// </summary>
        /// <returns></returns>
        public IActionResult GuidePdtCatgManage()
        {
            return View();
        }


        /// <summary>
        /// 前端导购分类
        /// </summary>
        /// <returns></returns> 
        public IActionResult GuidePdtCatgForm(SysUser user, GuideProductCatg? entity = null, string? imgmsg = null)
        {
            if (NHttpContext.Current.Request.IsAjaxRequest())
            {
                //获取状态
                StateCode state = ServiceIoc.Get<GuideProductCatgService>().Save(user.id, entity, imgmsg);
                return Json(GetResult(state));
            }
            else
            {
                //所属分类
                List<GuideProductCatg> Parents = ServiceIoc.Get<GuideProductCatgService>().GetTrees("", HttpUtility.HtmlDecode("&nbsp;&nbsp;"));
                Parents.Insert(0, new GuideProductCatg() { name = "根", id = 0 });
                ViewBag.Parents = Parents;
                //当前商品分类
                entity = ServiceIoc.Get<GuideProductCatgService>().GetById(bid);
                if (entity != null)
                {
                    ViewBag.entity = JsonConvert.SerializeObject(entity);
                }
            }
            return View();
        }


        /// <summary>
        /// 获取导购分类
        /// </summary>
        /// <returns></returns> 
        public ContentResult GetGuideProductCgtys(int pageSize, int pageIndex, string keyword)
        {
            try
            {
                //查询对象
                Criteria ct = new Criteria();

                //查询表达式
                MutilExpression me = new MutilExpression();

                ct.SetFromTables("tb_pdt_guidecatg")
                .SetPageSize(pageSize)
                .SetStartPage(pageIndex)
                .SetFields(new string[] { "*" })
                .AddOrderBy(new OrderBy("order_index", "desc"));

                //登录名称
                if (!string.IsNullOrEmpty(keyword))
                {
                    me.Add(new SingleExpression("name", LogicOper.LIKE, keyword));
                }

                //设置查询条件
                if (me.Expressions.Count > 0)
                {
                    ct.SetWhereExpression(me);
                }

                DataTable dt = ServiceIoc.Get<GuideProductCatgService>().Fill(ct);

                return PageResult(StateCode.State_200, ct.TotalRow, dt);
            }
            catch (Exception ex)
            {
                return PageResult(StateCode.State_500, 0, null);
            }
        }


        /// <summary>
        /// 导购商品分类名称是否存在
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int ExistGuideCategoryName(string name)
        {
            return ServiceIoc.Get<GuideProductCatgService>().ExistName(bid, name);
        }


        /// <summary>
        /// 查看商品分类编号是否存在
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int ExistGuideNo(string serial_no)
        {
            return ServiceIoc.Get<GuideProductCatgService>().ExistSerialNo(bid, serial_no);
        }


        /// <summary>
        /// 显示、隐藏 导购分类
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isshow"></param>
        /// <returns></returns>
        public JsonResult SetEnableGuideProductCgty(int id, bool isshow)
        {
            //获取状态
            StateCode state = ServiceIoc.Get<GuideProductCatgService>().SetEnable(id, isshow);
            return Json(GetResult(state));
        }


        /// <summary>
        /// 删除导购分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DeletePdtGuideCatg()
        {
            //获取状态
            StateCode state = ServiceIoc.Get<GuideProductCatgService>().Delete(bid);
            return Json(GetResult(state));
        }


        /// <summary>
        /// 保存排序
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public JsonResult SaveGuideCatgOrderIndex(string data)
        {
            try
            {
                string[] arr = StringHelper.StringToArray(data);
                ServiceIoc.Get<GuideProductCatgService>().SaveOrderIndex(arr);
                return Json(GetResult(StateCode.State_200));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }

        #endregion


        #region 商品管理——品牌管理

        /// <summary>
        /// 品牌管理页面
        /// </summary>
        /// <returns></returns>
        public IActionResult BrandManage()
        {
            return View();
        }

        /// <summary>
        /// 品牌 翻页/查询
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ContentResult GetBrands(int pageSize, int pageIndex, string keyword)
        {
            //查询对象
            Criteria ct = new Criteria();

            //查询表达式
            MutilExpression me = new MutilExpression();

            ct.SetFromTables("v_pdt_brand")
            .SetPageSize(pageSize)
            .SetStartPage(pageIndex)
            .SetFields(new string[] { "*" })
            .AddOrderBy(new OrderBy("id", "desc"));

            //关键词名称
            if (!string.IsNullOrEmpty(keyword))
            {
                me.Add(new SingleExpression("name", LogicOper.LIKE, keyword));
            }

            //设置查询条件
            if (me.Expressions.Count > 0)
            {
                ct.SetWhereExpression(me);
            }

            DataTable dt = ServiceIoc.Get<ProductBrandService>().Fill(ct);

            return PageResult(StateCode.State_200, ct.TotalRow, dt);
        }


        /// <summary>
        /// 品牌信息页面
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entity"></param>
        /// <param name="imgmsg"></param>
        /// <returns></returns>
        public IActionResult BrandForm(SysUser user, ProductBrand entity, string imgmsg)
        {
            if (NHttpContext.Current.Request.IsAjaxRequest())
            {
                //获取状态
                StateCode state = ServiceIoc.Get<ProductBrandService>().Save(user.id, entity, imgmsg);
                return Json(GetResult(state));
            }
            else
            {
                ViewBag.Ticket = StringHelper.GetEncryption(ImgType.Product_Brand + "#" + bid);
                ViewBag.defurl = ResXmlConfig.Instance.DefaultImgSrc(AppGlobal.Res, ImgType.Product_Brand);
                ViewBag.imgurl = ViewBag.imgurl;

                entity = ServiceIoc.Get<ProductBrandService>().GetById(bid);
                if (entity != null)
                {
                    Img img = ServiceIoc.Get<ImgService>().GetImg(ImgType.Product_Brand, entity.id);
                    if (img != null)
                    {
                        ViewBag.imgurl = string.IsNullOrEmpty(img.getImgUrl()) ? ViewBag.defimgurl : img.getImgUrl();
                    }
                    ViewBag.entity = JsonConvert.SerializeObject(entity);
                }
            }

            return View();
        }


        /// <summary>
        ///  商品牌名称是否存在
        /// </summary>
        /// <param name="user"></param>
        /// <param name="BrandName"></param>
        /// <returns></returns>
        public int ExistBrandName(SysUser user, string name)
        {
            return ServiceIoc.Get<ProductBrandService>().ExistBrandName(name, bid);
        }

        /// <summary>
        /// 删除供应商品牌
        /// </summary>
        /// <param name="bid"></param>
        /// <returns></returns>
        public JsonResult DeleteBrand()
        {
            StateCode state = ServiceIoc.Get<ProductBrandService>().DeleteBrand(bid);
            return Json(GetResult(state));
        }



        #endregion


        #region 商品管理——修改、删除、保存



        /// <summary>
        /// 获取具备上架条件的商品
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="keyword"></param>
        /// <param name="catg_id"></param>
        /// <param name="brand_id"></param>
        /// <param name="date"></param>
        /// <param name="guideCtgyID"></param>
        /// <param name="is_shelves"></param>
        /// <returns></returns>
        public ContentResult GetShelvesProducts(int pageSize, int pageIndex, string keyword, int catg_id, int brand_id, string date, int guideCtgyID, int is_shelves)
        {
            //查询对象
            Criteria ct = new Criteria();

            //查询表达式
            MutilExpression me = new MutilExpression();

            ct.SetFromTables("v_pdt_product")
            .SetPageSize(pageSize)
            .SetStartPage(pageIndex)
            .SetFields(new string[] { "*" })
            .AddOrderBy(new OrderBy("id", "desc"));

            //类别ID
            if (catg_id != 0)
            {
                me.Add(new SingleExpression("','+ catg_path +cast(catg_id as varchar(10))+','", LogicOper.LIKE, "," + catg_id + ","));
            }

            //品牌ID
            if (brand_id != 0)
            {
                me.Add(new SingleExpression("brand_id", LogicOper.EQ, brand_id));
            }

            //商品名称
            if (!string.IsNullOrEmpty(keyword))
            {
                me.Add(new SingleExpression("product_name", LogicOper.LIKE, keyword));
            }

            //发布日期
            if (!string.IsNullOrEmpty(date))
            {
                DateTime startDate = Convert.ToDateTime(date.Split('-')[0]);
                DateTime endDate = Convert.ToDateTime(date.Split('-')[1]);

                if (startDate.CompareTo(endDate) == 0)
                {
                    me.Add(new SingleExpression("created_date", LogicOper.BETWEEN, new[] { startDate.ToString(), endDate.AddDays(1).ToString() }));
                }
                else
                {
                    me.Add(new SingleExpression("created_date", LogicOper.BETWEEN, new[] { startDate.ToString(), endDate.AddDays(1).ToString() }));
                }
            }

            if (guideCtgyID != 0)
            {
                me.Add(new SingleExpression("guide_category_id", LogicOper.EQ, guideCtgyID));
            }

            //是否上架
            if (is_shelves != -1)
            {
                me.Add(new SingleExpression("is_shelves", LogicOper.EQ, is_shelves == 1));
            }

            me.Add(new SingleExpression("is_delete", LogicOper.EQ, false));
            ct.SetWhereExpression(me);

            DataTable dt = ServiceIoc.Get<ProductService>().Fill(ct);

            return PageResult(StateCode.State_200, ct.TotalRow, dt);
        }



        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="user"></param>
        /// <param name="tid">商品类型ID</param>
        /// <returns></returns>
        public JsonResult InitProduct(SysUser user, long tid)
        {
            try
            {
                return Json(GetResult(StateCode.State_200, ServiceIoc.Get<ProductService>().InitProduct(bid, tid)));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }



        /// <summary>
        /// 获取商品SKU信息，生成采购
        /// </summary>
        /// <param name="user"></param>
        /// <param name="tid"></param>
        /// <returns></returns>
        public JsonResult GetPurProduct(SysUser user, long tid)
        {
            try
            {
                return Json(GetResult(StateCode.State_200, ServiceIoc.Get<ProductService>().GetPurProduct(bid, tid)));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }



        /// <summary>
        /// 保存商品
        /// </summary>
        /// <param name="user"></param>
        /// <param name="product"></param>
        /// <param name="pdtAttrVals"></param>
        /// <param name="pdtExtAttrVals"></param>
        /// <param name="skus"></param>
        /// <param name="specCustoms"></param>
        /// <param name="imgs"></param>
        /// <param name="mainimg"></param>
        /// <returns></returns>
        public JsonResult SaveProduct(SysUser user, Product product, List<PdtAttrVal> pdtAttrVals, List<PdtExtAttrVal> pdtExtAttrVals, List<ProductSku> skus, List<SpecCustom> specCustoms, List<Img> imgs, string mainimg)
        {
            try
            {
                StateCode state = ServiceIoc.Get<ProductService>().Save(user.id, product, pdtAttrVals, pdtExtAttrVals, skus, specCustoms, imgs, mainimg);
                return Json(GetResult(state));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }



        /// <summary>
        /// 商品管理
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IActionResult ProductManage(SysUser user)
        {
            //所属分类
            List<ProductCatg> productCatgs = ServiceIoc.Get<ProductCatgService>().GetTrees("", HttpUtility.HtmlDecode("&nbsp;&nbsp;"));
            productCatgs.Insert(0, new ProductCatg() { name = "——商品分类——", id = 0 });
            ViewBag.productCatgs = productCatgs;

            //导购分类
            List<GuideProductCatg> guideProductCatgs = ServiceIoc.Get<GuideProductCatgService>().GetTrees("", HttpUtility.HtmlDecode("&nbsp;&nbsp;"));
            guideProductCatgs.Insert(0, new GuideProductCatg() { name = "——导购分类——", id = 0 });
            ViewBag.guideProductCatgs = guideProductCatgs;

            //品牌
            List<ProductBrand> brands = ServiceIoc.Get<ProductBrandService>().GetAll();
            brands.Insert(0, new ProductBrand() { name = "——所属品牌——", id = 0 });
            ViewBag.Brands = brands;

            return View();
        }



        /// <summary>
        /// 编辑商品基本信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cid"></param>
        /// <returns></returns>
        public IActionResult ProductForm(SysUser user, long cid)
        {
            //导购分类
            List<GuideProductCatg> guideProductCatgs = ServiceIoc.Get<GuideProductCatgService>().GetTrees("", HttpUtility.HtmlDecode("&nbsp;&nbsp;"));
            guideProductCatgs.Insert(0, new GuideProductCatg() { name = "——导购分类——", id = 0 });
            ViewBag.GuideProductCatgs = guideProductCatgs;

            //商品类型集合
            List<ProductType> productTypes = ServiceIoc.Get<ProductTypeService>().GetAll();
            productTypes.Insert(0, new ProductType() { name = "——商品类型——", id = 0 });
            ViewBag.ProductTypes = productTypes;

            //品牌
            List<ProductBrand> brands = ServiceIoc.Get<ProductBrandService>().GetAll();
            brands.Insert(0, new ProductBrand() { name = "——商品品牌——", id = 0 });
            ViewBag.Brands = brands;

            //加密ID
            ViewBag.CoverTicket = StringHelper.GetEncryption(ImgType.Product_Cover + "#" + bid);
            ViewBag.DetailsTicket = StringHelper.GetEncryption(ImgType.Product_Details + "#" + bid);

            //商品信息
            Product entity = ServiceIoc.Get<ProductService>().GetById(bid);
            if (entity != null)
            {
                //当前商品信息
                ViewBag.entity = JsonConvert.SerializeObject(entity);
                //商品对应分类
                ViewBag.ProductCgty = ServiceIoc.Get<ProductCatgService>().GetById(entity.catg_id);
            }

            if (cid != 0)
            {
                //不存在商品类别
                ProductCatg productCgty = ServiceIoc.Get<ProductCatgService>().GetById(cid);
                if (productCgty != null)
                {
                    ViewBag.ProductCgty = productCgty;
                    ViewBag.No = ViewBag.ProductCgty.serial_no + StringHelper.GetRandomCode(4) + ServiceIoc.Get<ProductService>().GetMaxID();
                }
                else
                {
                    ViewBag.No = ServiceIoc.Get<ProductService>().GetMaxID();
                }
            }

            return View();
        }



        /// <summary>
        /// 商品列表翻页查询
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="brand_id"></param>
        /// <param name="catg_id"></param>
        /// <param name="gcatg_id"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ContentResult GetProducts(int pageSize, int pageIndex, int brand_id, int catg_id, int gcatg_id, string keyword, string date, int is_shelves)
        {
            //查询对象
            Criteria ct = new Criteria();

            //查询表达式
            MutilExpression me = new MutilExpression();

            ct.SetFromTables("v_pdt_product")
            .SetPageSize(pageSize)
            .SetStartPage(pageIndex)
            .SetFields(new string[] { "*" })
            .AddOrderBy(new OrderBy("id", "desc"));

            //类别ID
            if (catg_id != 0)
            {
                me.Add(new SingleExpression("','+ catg_path +cast(catg_id as varchar(10))+','", LogicOper.LIKE, "," + catg_id + ","));
            }

            //发布日期
            if (!string.IsNullOrEmpty(date))
            {
                DateTime startDate = Convert.ToDateTime(date.Split('-')[0]);
                DateTime endDate = Convert.ToDateTime(date.Split('-')[1]);

                if (startDate.CompareTo(endDate) == 0)
                {
                    me.Add(new SingleExpression("created_date", LogicOper.BETWEEN, new[] { startDate.ToString(), endDate.AddDays(1).ToString() }));
                }
                else
                {
                    me.Add(new SingleExpression("created_date", LogicOper.BETWEEN, new[] { startDate.ToString(), endDate.AddDays(1).ToString() }));
                }
            }

            //品牌ID
            if (brand_id != 0)
            {
                me.Add(new SingleExpression("brand_id", LogicOper.EQ, brand_id));
            }

            //商品名称
            if (!string.IsNullOrEmpty(keyword))
            {
                me.Add(new SingleExpression("name", LogicOper.LIKE, keyword));
            }

            //是否上架
            if (is_shelves != -1)
            {
                me.Add(new SingleExpression("is_shelves", LogicOper.EQ, is_shelves == 1));
            }

            me.Add(new SingleExpression("is_delete", LogicOper.EQ, false));
            ct.SetWhereExpression(me);

            DataTable dt = ServiceIoc.Get<ProductService>().Fill(ct);

            return PageResult(StateCode.State_200, ct.TotalRow, dt);
        }



        /// <summary>
        /// 是否删除
        /// </summary>
        /// <param name="user"></param>
        /// <param name="isDelete"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public JsonResult DeleteOrRestore(SysUser user, bool isDelete, long[] ids)
        {
            try
            {
                ServiceIoc.Get<ProductService>().DeleteOrRestore(ids, isDelete);
                return Json(GetResult(StateCode.State_200));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }



        /// <summary>
        /// 删除选中（彻底删除）
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public JsonResult DeleteSelect(SysUser user, long[] ids)
        {
            try
            {
                StateCode state = ServiceIoc.Get<ProductService>().Deletes(ids);
                return Json(GetResult((state)));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }

        #endregion


        #region 商品管理——排期与上下架

        /// <summary>
        /// 上架选择商品类型 
        /// </summary>
        /// <returns></returns>
        public IActionResult ShelvesProduct(SysUser user)
        {
            return View();
        }

        /// <summary>
        /// 排期与上下架页面
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IActionResult SchedulShelveManage(SysUser user)
        {
            //所属分类
            List<ProductCatg> productCatgs = ServiceIoc.Get<ProductCatgService>().GetTrees("", HttpUtility.HtmlDecode("&nbsp;&nbsp;"));
            productCatgs.Insert(0, new ProductCatg() { name = "——商品分类——", id = 0 });
            ViewBag.productCatgs = productCatgs;

            //导购分类
            List<GuideProductCatg> guideProductCatgs = ServiceIoc.Get<GuideProductCatgService>().GetTrees("", HttpUtility.HtmlDecode("&nbsp;&nbsp;"));
            guideProductCatgs.Insert(0, new GuideProductCatg() { name = "——导购分类——", id = 0 });
            ViewBag.GuideProductCatgs = guideProductCatgs;

            //商品类型集合
            List<ProductType> productTypes = ServiceIoc.Get<ProductTypeService>().GetAll();
            productTypes.Insert(0, new ProductType() { name = "——商品类型——", id = 0 });
            ViewBag.ProductTypes = productTypes;

            //品牌
            List<ProductBrand> brands = ServiceIoc.Get<ProductBrandService>().GetAll();
            brands.Insert(0, new ProductBrand() { name = "——商品品牌——", id = 0 });
            ViewBag.Brands = brands;

            return View();
        }


        /// <summary>
        /// 保存排期
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public JsonResult SaveShelvesDate(SysUser user, long[] ids, string date)
        {
            try
            {
                //发布日期
                DateTime startDate = Convert.ToDateTime(date.Split('-')[0]);
                DateTime endDate = Convert.ToDateTime(date.Split('-')[1]);

                ServiceIoc.Get<ProductService>().SaveShelvesDate(ids, startDate, endDate);
                return Json(GetResult(StateCode.State_200));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }

        /// <summary>
        /// 选中上架或下架
        /// </summary>
        /// <param name="user"></param>
        /// <param name="idstr"></param>
        /// <returns></returns>
        public JsonResult SelectShelves(SysUser user, long[] ids, bool isShelves)
        {
            try
            {
                ServiceIoc.Get<ProductService>().SelectShelves(ids, isShelves);
                return Json(GetResult(StateCode.State_200));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }

        #endregion


        #region 商品管理——商品回收站


        /// <summary>
        /// 初始化商品回收站
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IActionResult ProductRecycle(SysUser user)
        {
            //所属分类
            List<ProductCatg> productCatgs = ServiceIoc.Get<ProductCatgService>().GetTrees("", HttpUtility.HtmlDecode("&nbsp;&nbsp;"));
            productCatgs.Insert(0, new ProductCatg() { name = "——商品分类——", id = 0 });
            ViewBag.productCatgs = productCatgs;

            //导购分类
            List<GuideProductCatg> guideProductCatgs = ServiceIoc.Get<GuideProductCatgService>().GetTrees("", HttpUtility.HtmlDecode("&nbsp;&nbsp;"));
            guideProductCatgs.Insert(0, new GuideProductCatg() { name = "——导购分类——", id = 0 });
            ViewBag.guideProductCatgs = guideProductCatgs;

            //品牌
            List<ProductBrand> brands = ServiceIoc.Get<ProductBrandService>().GetAll();
            brands.Insert(0, new ProductBrand() { name = "——所属品牌——", id = 0 });
            ViewBag.Brands = brands;

            return View();
        }


        /// <summary>
        /// 商品回收站
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="catg_id"></param>
        /// <param name="brand_id"></param>
        /// <param name="keyword"></param> 
        /// <returns></returns>
        public ContentResult GetProductRecycles(int pageSize, int pageIndex, int catg_id, int brand_id, string keyword)
        {
            //查询对象
            Criteria ct = new Criteria();

            //查询表达式
            MutilExpression me = new MutilExpression();

            ct.SetFromTables("v_pdt_product")
            .SetPageSize(pageSize)
            .SetStartPage(pageIndex)
            .SetFields(new string[] { "*" })
            .AddOrderBy(new OrderBy("id", "desc"));


            //类别ID
            if (catg_id != 0)
            {
                me.Add(new SingleExpression("','+ catg_path +cast(catg_id as varchar(10))+','", LogicOper.LIKE, "," + catg_id + ","));
            }

            //品牌ID
            if (brand_id != 0)
            {
                me.Add(new SingleExpression("brand_id", LogicOper.EQ, brand_id));
            }

            //商品名称
            if (!string.IsNullOrEmpty(keyword))
            {
                me.Add(new SingleExpression("name", LogicOper.LIKE, keyword));
            }

            me.Add(new SingleExpression("is_delete", LogicOper.EQ, true));
            ct.SetWhereExpression(me);

            DataTable dt = ServiceIoc.Get<ProductService>().Fill(ct);

            return PageResult(StateCode.State_200, ct.TotalRow, dt);
        }


        #endregion

    }
}