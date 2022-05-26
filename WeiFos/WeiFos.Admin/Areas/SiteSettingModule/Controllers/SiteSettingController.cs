using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WeiFos.Admin.Code.Authorization;
using WeiFos.Admin.Controllers;
using WeiFos.Core;
using WeiFos.NetCore.Extensions;
using WeiFos.Core.XmlHelper;
using WeiFos.Entity.BizTypeModule;
using WeiFos.Entity.Enums;
using WeiFos.Entity.ProductModule;
using WeiFos.Entity.ResourceModule;
using WeiFos.Entity.SiteSettingModule;
using WeiFos.Entity.SystemModule;
using WeiFos.ORM.Data;
using WeiFos.ORM.Data.Const;
using WeiFos.ORM.Data.Restrictions;
using WeiFos.Service;
using WeiFos.Service.ProductModule;
using WeiFos.Service.ResourceModule;
using WeiFos.Service.SiteSettingModule;

namespace WeiFos.Admin.Areas.SiteSettingModule.Controllers
{
    /// <summary>
    /// 站点信息 控制器
    /// @author yewei 
    /// add by @date 2015-08-29
    /// </summary>
    [LoginAuth]
    [Area(AreaNames.SiteSettingModule)]
    public class SiteSettingController : BaseController
    {


        #region 资讯模块


        /// <summary>
        /// 资讯分类
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IActionResult InformtCgtyManage()
        {
            return View();
        }


        /// <summary>
        /// 获取资讯类别分页
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="name"></param>
        /// <param name="createdDate"></param>
        /// <returns></returns>
        public JsonResult GetInformtCgtys(int pageSize, int currentPage, string name)
        {
            //创建查询对象
            Criteria ct = new Criteria();
            ct.SetPageSize(pageSize)
            .SetStartPage(currentPage)
            .SetFields(new string[] { "*" })
            .AddOrderBy(new OrderBy("id", "desc"));

            //查询表达式
            MutilExpression me = new MutilExpression();

            if (!string.IsNullOrEmpty(name))
            {
                me.Add(new SingleExpression("name", LogicOper.LIKE, name));
            }

            if (me.Expressions.Count > 0)
            {
                //设置查询条件
                ct.SetWhereExpression(me);
            }

            List<InformtCgty> data = ServiceIoc.Get<InformtCgtyService>().GetList(ct);

            return PageResult(StateCode.State_200, ct.TotalRow, data);
        }


        /// <summary>
        /// 资讯分类页
        /// </summary>
        /// <returns></returns>
        public IActionResult InformtCgtyForm()
        {
            //缺省图片路劲
            ViewBag.defurl = ResXmlConfig.Instance.DefaultImgSrc(AppGlobal.Res, ImgType.InformtCgty);
            ViewBag.imgurl = ViewBag.defurl;

            InformtCgty infoCgty = ServiceIoc.Get<InformtCgtyService>().GetById(bid);
            if (infoCgty != null)
            {
                //正面图
                Img img = ServiceIoc.Get<ImgService>().GetImg(ImgType.InformtCgty, infoCgty.id);
                if (img != null)
                {
                    ViewBag.imgurl = string.IsNullOrEmpty(img.getImgUrl()) ? ViewBag.imgurl : img.getImgUrl();
                }

                ViewBag.informtCgty = JsonConvert.SerializeObject(infoCgty);
            }
            return View();
        }


        /// <summary>
        /// 保存资讯分类
        /// </summary>
        /// <param name="user"></param>
        /// <param name="infoCgty"></param>
        /// <returns></returns>
        public JsonResult SaveInformtCgty(SysUser user, InformtCgty informtCgty, string imgmsg)
        {
            StateCode state = ServiceIoc.Get<InformtCgtyService>().Save(user.id, informtCgty, imgmsg);
            return Json(GetResult(state));
        }


        /// <summary>
        /// 资讯列表
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IActionResult InformtManage(SysUser user)
        {
            List<InformtCgty> Cgtys = new List<InformtCgty>();
            Cgtys.Add(new InformtCgty() { id = 0, name = "——请选择——" });

            List<InformtCgty> ListInformtCgtys = ServiceIoc.Get<InformtCgtyService>().GetAll();
            if (ListInformtCgtys != null)
            {
                Cgtys.AddRange(ListInformtCgtys);
            }

            ViewBag.informtCgtys = Cgtys;

            //资讯集合
            //List<Informt> Informts = ServiceIoc.Get<InformtService>().Where(i => i.id > 1 && i.cgty_id == 2 || i.title.Contains("t") || i.context.Contains("t")).ToList();

            return View();
        }


        /// <summary>
        /// 资讯翻页
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="title"></param>
        /// <param name="cgty_id"></param>
        /// <param name="createdDate"></param>
        /// <returns></returns>
        public ContentResult GetInformts(int pageSize, int currentPage, string title, long cgty_id, string createdDate)
        {
            //创建查询对象
            Criteria ct = new Criteria();
            ct.SetFromTables("v_info_informt")
            .SetPageSize(pageSize)
            .SetStartPage(currentPage)
            .SetFields(new string[] { "*" })
            .AddOrderBy(new OrderBy("order_index", "desc"));

            //查询表达式
            MutilExpression me = new MutilExpression();

            if (!string.IsNullOrEmpty(title))
            {
                me.Add(new SingleExpression("title", LogicOper.LIKE, title));
            }

            if (cgty_id != 0)
            {
                me.Add(new SingleExpression("cgty_id", LogicOper.EQ, cgty_id));
            }

            //日期
            if (!string.IsNullOrEmpty(createdDate))
            {
                DateTime startDate = Convert.ToDateTime(createdDate.Split('-')[0]);
                DateTime endDate = Convert.ToDateTime(createdDate.Split('-')[1]);

                if (startDate.CompareTo(endDate) == 0)
                {
                    me.Add(new SingleExpression("created_date", LogicOper.BETWEEN, new[] { startDate.ToString("yyyy/MM/dd"), endDate.AddDays(1).ToString("yyyy/MM/dd") }));
                }
                else
                {
                    me.Add(new SingleExpression("created_date", LogicOper.BETWEEN, new[] { startDate.ToString("yyyy/MM/dd"), endDate.AddDays(1).ToString("yyyy/MM/dd") }));
                }
            }

            if (me.Expressions.Count > 0)
            {
                //设置查询条件
                ct.SetWhereExpression(me);
            }

            DataTable dt = ServiceIoc.Get<InformtService>().Fill(ct);

            return PageResult(StateCode.State_200, ct.TotalRow, dt);
        }


        /// <summary>
        /// 删除资讯
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public JsonResult DeleteInformt(SysUser user, long[] ids)
        {
            try
            {
                ServiceIoc.Get<InformtService>().Deletes(ids);
                return Json(GetResult(StateCode.State_200));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }


        /// <summary>
        /// 资讯页面
        /// </summary>
        /// <returns></returns>
        public IActionResult InformtForm()
        {
            //缺省图片路劲
            ViewBag.defurl = ResXmlConfig.Instance.DefaultImgSrc(ViewBag.Res, ImgType.Informt);
            ViewBag.imgurl = ViewBag.defurl;

            List<InformtCgty> cgtys = ServiceIoc.Get<InformtCgtyService>().GetListByParentId(0);
            cgtys.Insert(0, new InformtCgty() { name = "根目录", id = 0 });
            ViewBag.Parents = cgtys;

            ViewBag.Ticket = StringHelper.GetEncryption(ImgType.Informt + "#" + bid);
            ViewBag.DetailsTicket = StringHelper.GetEncryption(ImgType.InformtDetails + "#" + bid);

            Informt informt = ServiceIoc.Get<InformtService>().GetById(bid);
            if (informt != null)
            {
                //正面图
                Img img = ServiceIoc.Get<ImgService>().GetImg(ImgType.Informt, informt.id);
                if (img != null)
                {
                    ViewBag.imgurl = string.IsNullOrEmpty(img.getImgUrl()) ? ViewBag.imgurl : img.getImgUrl();
                }
                ViewBag.informt = JsonConvert.SerializeObject(informt);
            }
            return View();
        }


        /// <summary>
        /// 保存资讯
        /// </summary>
        /// <param name="user"></param>
        /// <param name="informt"></param>
        /// <param name="imgmsg"></param>
        /// <returns></returns>
        public JsonResult SaveInformt(SysUser user, Informt informt, string imgmsg)
        {
            StateCode state = ServiceIoc.Get<InformtService>().Save(user.id, informt, imgmsg);
            return Json(GetResult(state));
        }

        #endregion


        #region 企业信息模块

        /// <summary>
        /// 企业信息管理
        /// </summary>
        /// <returns></returns>
        public IActionResult MsgManage()
        {
            return View();
        }

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public string GetMsgs()
        {
            //创建查询对象
            Criteria ct = new Criteria();
            ct.SetFields(new string[] { "id", "type" })
            .AddOrderBy(new OrderBy("id", "desc"));

            //查询表达式
            MutilExpression me = new MutilExpression();

            if (me.Expressions.Count > 0)
            {
                //设置查询条件
                ct.SetWhereExpression(me);
            }

            List<WebIntroduction> introductions = ServiceIoc.Get<WebIntroductionService>().GetList(ct);
            return JsonConvert.SerializeObject(introductions);
        }


        /// <summary>
        /// 企业信息页面
        /// </summary>
        /// <returns></returns>
        public IActionResult MsgForm()
        {
            WebIntroduction init = ServiceIoc.Get<WebIntroductionService>().GetById(bid);
            if (init != null)
            {
                ViewBag.introduction = init;
                ViewBag.entity = JsonConvert.SerializeObject(init);
            }
            else
            {
                ViewBag.MsgType = WebIntroductionType.intList;
            }
            return View();
        }


        /// <summary>
        /// 保存企业信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="webIntroduction"></param>
        /// <param name="imgmsg"></param>
        /// <returns></returns>
        public JsonResult SaveWebIntroduction(SysUser user, WebIntroduction webIntroduction, string imgmsg)
        {
            StateCode state = ServiceIoc.Get<WebIntroductionService>().Save(user.id, webIntroduction);
            return Json(GetResult(state));
        }

        #endregion


        #region Banner 模块


        /// <summary>
        /// 广告图管理
        /// </summary>
        /// <returns></returns>
        public IActionResult BannerManage()
        {
            return View();
        }



        /// <summary>
        /// 保存广告图
        /// </summary>
        /// <param name="user"></param>
        /// <param name="adimg"></param>
        /// <param name="imgmsg"></param>
        /// <returns></returns>
        public IActionResult BannerForm(SysUser user, Banner entity, string imgmsg)
        {
            if (NHttpContext.Current.Request.IsAjaxRequest())
            {
                StateCode code = ServiceIoc.Get<BannerService>().Save(user.id, entity, imgmsg);
                return Json(GetResult(code));
            }
            else
            {
                //所属分类
                List<ProductCatg> productCgtys = ServiceIoc.Get<ProductCatgService>().GetTrees("", HttpUtility.HtmlDecode("&nbsp;&nbsp;"));
                productCgtys.Insert(0, new ProductCatg() { name = "——商品分类——", id = 0 });
                ViewBag.productCgtys = productCgtys;

                //上传票据
                ViewBag.Ticket = StringHelper.GetEncryption(ImgType.Banner + "#" + bid);
                //缺省图片路
                ViewBag.defurl = ResXmlConfig.Instance.DefaultImgSrc(AppGlobal.Res, ImgType.Banner);
                ViewBag.imgurl = ViewBag.defurl;

                entity = ServiceIoc.Get<BannerService>().GetById(bid);
                if (entity != null)
                {
                    ViewBag.entity = JsonConvert.SerializeObject(entity);

                    Img img = ServiceIoc.Get<ImgService>().GetImg(ImgType.Banner, entity.id);
                    if (img != null)
                    {
                        ViewBag.imgurl = string.IsNullOrEmpty(img.getImgUrl()) ? ViewBag.defimgurl : img.getImgUrl();
                    }

                    //商品详情
                    if (entity.content_type == (int)BannerLink.ProductDetails && !string.IsNullOrEmpty(entity.content_value))
                    {
                        long pid = 0;
                        long.TryParse(entity.content_value, out pid);
                        Product product = ServiceIoc.Get<ProductService>().GetById(pid);
                        if (product != null)
                        {
                            ViewBag.bizEntity = JsonConvert.SerializeObject(product);
                        }
                    }//商品列表
                    else if (entity.content_type == (int)BannerLink.ProductList && !string.IsNullOrEmpty(entity.content_value))
                    {
                        GuideProductCatg guideProductCgty = ServiceIoc.Get<GuideProductCatgService>().GetById(long.Parse(entity.content_value));
                        if (guideProductCgty != null)
                        {
                            ViewBag.bizEntity = JsonConvert.SerializeObject(guideProductCgty);
                        }
                    }

                }
            }

            return View();
        }


        /// <summary>
        /// 广告图分页
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ContentResult GetBanners(int pageSize, int pageIndex, string keyword)
        {
            //查询对象
            Criteria ct = new Criteria();

            //查询表达式
            MutilExpression me = new MutilExpression();

            ct.SetFromTables("v_fnt_banner")
            .SetPageSize(pageSize)
            .SetStartPage(pageIndex)
            .SetFields(new string[] { "*" })
            .AddOrderBy(new OrderBy("order_index", "desc"));

            if (!string.IsNullOrEmpty(keyword))
            {
                me.Add(new SingleExpression("name", LogicOper.LIKE, keyword));
            }

            if (me.Expressions.Count > 0)
            {
                ct.SetWhereExpression(me);
            }

            DataTable dt = ServiceIoc.Get<BannerService>().Fill(ct);

            return PageResult(StateCode.State_200, ct.TotalRow, dt);
        }



        /// <summary>
        /// 删除广告图
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public JsonResult DeleteBanner(SysUser user, long[] ids)
        {
            try
            {
                ServiceIoc.Get<BannerService>().Deletes(ids);
                return Json(GetResult(StateCode.State_200));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }

        #endregion


        #region 案例分类


        /// <summary>
        /// 案例分类
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IActionResult CaseCgtyManage(SysUser user)
        {
            return View();
        }



        /// <summary>
        /// 案例详情
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IActionResult CaseCgtyForm(SysUser user)
        {
            //缺省图片路劲
            ViewBag.defurl = ResXmlConfig.Instance.DefaultImgSrc(ViewBag.Res, ImgType.Case_Cover);
            ViewBag.imgurl = ViewBag.defurl;
            ViewBag.Ticket = StringHelper.GetEncryption(ImgType.Case_Cover + "#" + bid);

            SuccessfulCaseCgty cgty = ServiceIoc.Get<SuccessfulCaseCgtyService>().GetById(bid);
            if (cgty != null)
            {
                //正面图
                Img img = ServiceIoc.Get<ImgService>().GetImg(ImgType.Case_Cover, cgty.id);
                if (img != null)
                {
                    ViewBag.imgurl = string.IsNullOrEmpty(img.getImgUrl()) ? ViewBag.imgurl : img.getImgUrl();
                }

                ViewBag.entity = JsonConvert.SerializeObject(cgty);
            }

            return View();
        }



        /// <summary>
        /// 获取案例类别分页
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonResult GetCaseCgtys(int pageSize, int currentPage, string name)
        {
            //创建查询对象
            Criteria ct = new Criteria();
            ct.SetPageSize(pageSize)
            .SetStartPage(currentPage)
            .SetFields(new string[] { "*" })
            .AddOrderBy(new OrderBy("order_index", "desc"));

            //查询表达式
            MutilExpression me = new MutilExpression();

            if (!string.IsNullOrEmpty(name))
            {
                me.Add(new SingleExpression("title", LogicOper.LIKE, name));
            }

            //设置查询条件
            if (me.Expressions.Count > 0)
            {
                ct.SetWhereExpression(me);
            }

            List<SuccessfulCaseCgty> datas = ServiceIoc.Get<SuccessfulCaseCgtyService>().GetList(ct);

            return PageResult(StateCode.State_200, ct.TotalRow, datas);
        }



        /// <summary>
        /// 冻结、解冻 用户
        /// </summary>
        /// <param name="sysUser"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public JsonResult SetCaseCgtyEnable(bool enable)
        {
            //获取状态
            StateCode state = ServiceIoc.Get<SuccessfulCaseCgtyService>().SetEnable(bid, enable);
            return Json(GetResult(state));
        }




        /// <summary>
        /// 保存案例分类
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public JsonResult SaveCaseCgty(SysUser user, SuccessfulCaseCgty entity)
        {
            var state = ServiceIoc.Get<SuccessfulCaseCgtyService>().Save(user.id, entity);
            return Json(GetResult(state));
        }



        #endregion



    }
}