using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WeiFos.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using WeiFos.Admin.Code.Authorization;
using WeiFos.Admin.Models;
using WeiFos.Core.XmlHelper;
using WeiFos.Entity.SystemModule;
using WeiFos.Service;
using WeiFos.Service.ResourceModule;
using WeiFos.Entity.ResourceModule;
using WeiFos.Admin.Code;
using WeiFos.Entity.BizTypeModule;
using System.Data;
using Newtonsoft.Json;
using WeiFos.Entity.Enums;
using WeiFos.Service.OrderModule;

namespace WeiFos.Admin.Controllers
{

    /// <summary>
    /// Copyright (c) 2013-2018 深圳微狐信息科技有限公司
    /// 描 述：Home控制器 
    /// 创建人：叶委
    /// 日 期：2019.01.10
    /// </summary>
    [LoginAuth]
    public class HomeController : BaseController
    {


        /// <summary>
        /// 登录成功
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            //缺省图片路径
            ViewBag.defimgurl = string.Empty;

            //用户图片路径
            ViewBag.imgurl = string.Empty;

            //缺省图片路劲
            ViewBag.defimgurl = ResXmlConfig.Instance.DefaultImgSrc(ViewBag.Res, ImgType.SysUser);
            ViewBag.imgurl = ViewBag.defimgurl;

            Img img = ServiceIoc.Get<ImgService>().GetImg(ImgType.SysUser, LoginUser.Instance.User.id);
            if (img != null)
            {
                ViewBag.imgurl = string.IsNullOrEmpty(img.getImgUrl()) ? ViewBag.defimgurl : img.getImgUrl();
            }

            List<SysModelMenu> modelMenus = AppGlobal.Instance.Menus;
            if (modelMenus != null)
            {
                ViewBag.p_menus = modelMenus.Where(m => m.parent_id == 0 && m.is_enable == true).OrderByDescending(m => m.order_index).ToList();
                ViewBag.childrens = modelMenus.Where(m => m.parent_id != 0 && m.is_enable == true).OrderByDescending(m => m.order_index).ToList();
            }
            else
            {
                //父级菜单
                ViewBag.p_menus = new List<SysModelMenu>();
                //子菜单
                ViewBag.childrens = new List<SysModelMenu>();
            }

            return View();
        }


        /// <summary>
        /// 登录缺省页
        /// </summary>
        /// <returns></returns>
        public IActionResult Default()
        {
            return View();
        }


        /// <summary>
        /// 获取统计数据
        /// </summary>
        /// <returns></returns>
        public ContentResult GetDefaultStatis()
        {
            try
            {
                //统计数据
                DataTable dt = ServiceIoc.Get<ProductOrderService>().GetDefaultStatis();
                //统计报表
                DataTable report_dt = ServiceIoc.Get<ProductOrderService>().GetDefaultStatisReport();

                return Content(JsonConvert.SerializeObject(GetResult(StateCode.State_200, new { statis = dt, report = report_dt })));
            }
            catch (Exception ex)
            {
                return Content(JsonConvert.SerializeObject(GetResult(StateCode.State_500)));
            }
        }


        /// <summary>
        /// 错误页
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
