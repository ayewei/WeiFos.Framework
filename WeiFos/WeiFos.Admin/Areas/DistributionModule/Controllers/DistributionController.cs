using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WeiFos.Admin.Code.Authorization;
using WeiFos.Admin.Controllers;
using WeiFos.Entity.DistributionModule;
using WeiFos.Entity.Enums;
using WeiFos.Entity.SystemModule;
using WeiFos.ORM.Data;
using WeiFos.ORM.Data.Const;
using WeiFos.ORM.Data.Restrictions;
using WeiFos.Service;
using WeiFos.Service.DistributionModule;

namespace WeiFos.Admin.Areas.DistributionModule.Controllers
{
    [LoginAuth]
    [Area(AreaNames.DistributionModule)]
    public class DistributionController : BaseController
    {


        #region 物流公司管理



        /// <summary>
        /// 物流公司管理
        /// </summary>
        /// <returns></returns>
        public IActionResult LogisticsCompanyManage()
        {
            return View();
        }



        /// <summary>
        /// 物流公司信息页面
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public IActionResult LogisticsCompanyForm(SysUser user)
        {
            LogisticsCompany entity = ServiceIoc.Get<LogisticsCompanyService>().GetById(bid);
            if (entity != null)
            {
                ViewBag.entity = JsonConvert.SerializeObject(entity);
            }

            return View();
        }


        /// <summary>
        /// 保存物流公司
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public JsonResult DoLogisticsCompanyForm(SysUser user, [FromBody] LogisticsCompany entity)
        {
            StateCode state = ServiceIoc.Get<LogisticsCompanyService>().Save(user.id, entity);
            return Json(GetResult(state));
        }



        /// <summary>
        /// 物流公司 翻页/查询
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public JsonResult GetLogisticsCompanys(int pageSize, int pageIndex, string keyword)
        {
            //查询对象
            Criteria ct = new Criteria();

            //查询表达式
            MutilExpression me = new MutilExpression();

            ct.SetPageSize(pageSize)
            .SetStartPage(pageIndex)
            .SetFields(new string[] { "*" })
            .AddOrderBy(new OrderBy("id", "desc"));

            //物流公司名称
            if (!string.IsNullOrEmpty(keyword))
            {
                me.Add(new SingleExpression("Name", LogicOper.LIKE, keyword));
            }

            if (me.Expressions.Count > 0)
            {
                ct.SetWhereExpression(me);
            }

            List<LogisticsCompany> companys = ServiceIoc.Get<LogisticsCompanyService>().GetList(ct);

            return PageResult(StateCode.State_200, ct.TotalRow, companys);
        }


        /// <summary>
        /// 物流公司名称是否存在
        /// </summary>
        /// <param name="user"></param>
        /// <param name="BrandName"></param>
        /// <returns></returns>
        public int ExistLogisticsCompanyName(string name)
        {
            return ServiceIoc.Get<LogisticsCompanyService>().ExistName(name, bid);
        }



        /// <summary>
        /// 删除物流公司
        /// </summary>
        /// <param name="bid"></param>
        /// <returns></returns>
        public JsonResult DeleteLogisticsCompany()
        {
            try
            {
                ServiceIoc.Get<LogisticsCompanyService>().Delete(bid);
                return Json(GetResult(StateCode.State_200));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }




        #endregion


        #region 运费模板管理


        /// <summary>
        /// 运费模板管理
        /// </summary>
        /// <returns></returns>
        public IActionResult FreightTemplateManage()
        {
            return View();
        }



        /// <summary>
        /// 运费模板管理
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IActionResult FreightTemplateForm(SysUser user)
        {
            FreightTemplate entity = ServiceIoc.Get<FreightTemplateService>().GetById(bid);
            if (entity != null)
            {
                //运费模板对象
                ViewBag.entity = JsonConvert.SerializeObject(entity);
                ViewBag.FRegions = ServiceIoc.Get<FreightRegionService>().GetList("where freight_template_id = @0", bid);
            }
            return View();
        }



        /// <summary>
        /// 运费模板管理
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entity"></param>
        /// <param name="FRegions"></param>
        /// <returns></returns>
        public JsonResult DoFreightTemplateForm(SysUser user, [FromBody] dynamic entity)
        {
            FreightTemplate fTemplate = JsonConvert.DeserializeObject<FreightTemplate>(entity.entity.ToString());
            List<FreightRegion> FRegions = JsonConvert.DeserializeObject<List<FreightRegion>>(entity.FRegions.ToString());
            StateCode code = ServiceIoc.Get<FreightTemplateService>().Save(user.id, fTemplate, FRegions);
            return Json(GetResult(code));
        }



        /// <summary>
        /// 运费模板 翻页/查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonResult GetFreightTemplates(int pageSize, int pageIndex, string name)
        {
            //查询对象
            Criteria ct = new Criteria();

            //查询表达式
            MutilExpression me = new MutilExpression();

            ct.SetPageSize(pageSize)
            .SetStartPage(pageIndex)
            .SetFields(new string[] { "*" })
            .AddOrderBy(new OrderBy("id", "desc"));

            //物流公司名称
            if (!string.IsNullOrEmpty(name))
            {
                me.Add(new SingleExpression("name", LogicOper.LIKE, name));
            }

            if (me.Expressions.Count > 0)
            {
                ct.SetWhereExpression(me);
            }

            List<FreightTemplate> data = ServiceIoc.Get<FreightTemplateService>().GetList(ct);

            return PageResult(StateCode.State_200, ct.TotalRow, data);

        }


        /// <summary>
        /// 保存运费模板
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entity"></param>
        /// <param name="FTRegions"></param>
        /// <param name="FRegions"></param>
        /// <returns></returns>
        public JsonResult SaveFreightTemplate(SysUser user, FreightTemplate entity, List<FreightRegion> FRegions)
        {
            StateCode code = ServiceIoc.Get<FreightTemplateService>().Save(user.id, entity, FRegions);
            return Json(GetResult(code));
        }



        /// <summary>
        /// 根据运费模板ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetRegionByTemRegionID(SysUser user)
        {
            List<FreightRegion> freightRegions = ServiceIoc.Get<FreightRegionService>().GetByTemplateRegionID(bid);
            return Json(freightRegions);
        }



        /// <summary>
        /// 删除运费模板
        /// </summary>
        /// <param name="bid"></param>
        /// <returns></returns>
        public JsonResult DeleteFreightTemplate()
        {
            StateCode state = ServiceIoc.Get<FreightTemplateService>().DeleteFreightTemplate(bid);
            return Json(GetResult(state));
        }



        #endregion


        #region 配送方式管理


        /// <summary>
        /// 配送方式管理
        /// </summary>
        /// <returns></returns>
        public IActionResult DeliveryModeManage()
        {
            return View();
        }


        /// <summary>
        /// 运费模板管理
        /// </summary>
        /// <returns></returns>
        public IActionResult DeliveryModeForm(SysUser user)
        {
            //物流公司
            List<LogisticsCompany> logisticsCompanys = ServiceIoc.Get<LogisticsCompanyService>().GetAll();
            //运费模板
            List<FreightTemplate> freightTemplates = ServiceIoc.Get<FreightTemplateService>().GetAll();
            freightTemplates.Insert(0, new FreightTemplate() { id = 0, name = "——请选择——" });

            ViewBag.LogisticsCompanys = logisticsCompanys;
            ViewBag.FreightTemplates = freightTemplates;

            DeliveryMode entity = ServiceIoc.Get<DeliveryModeService>().GetById(bid);
            if (entity != null)
            {
                ViewBag.entity = JsonConvert.SerializeObject(entity);
            }

            return View();
        }


        /// <summary>
        /// 保存运费模板
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public JsonResult DoDeliveryModeForm(SysUser user, [FromBody] DeliveryMode entity)
        {
            StateCode code = ServiceIoc.Get<DeliveryModeService>().Save(user.id, entity);
            return Json(GetResult(code));
        }





        /// <summary>
        /// 配送方式 翻页/查询
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ContentResult GetDeliveryModes(int pageSize, int pageIndex, string keyword)
        {
            //查询对象
            Criteria ct = new Criteria();

            //查询表达式
            MutilExpression me = new MutilExpression();

            ct.SetFromTables("v_dist_deliverymode")
            .SetPageSize(pageSize)
            .SetStartPage(pageIndex)
            .SetFields(new string[] { "*" })
            .AddOrderBy(new OrderBy("id", "desc"));

            //物流公司名称
            if (!string.IsNullOrEmpty(keyword))
            {
                me.Add(new SingleExpression("name", LogicOper.LIKE, keyword));
            }

            if (me.Expressions.Count > 0)
            {
                ct.SetWhereExpression(me);
            }

            DataTable dt = ServiceIoc.Get<DeliveryModeService>().Fill(ct);

            return PageResult(StateCode.State_200, ct.TotalRow, dt);
        }



        /// <summary>
        /// 删除配送方式
        /// </summary>
        /// <returns></returns>
        public JsonResult DeleteDeliveryMode()
        {
            try
            {
                ServiceIoc.Get<DeliveryModeService>().Delete(bid);
                return Json(GetResult(StateCode.State_200));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }


        /// <summary>
        /// 物流公司名称是否存在
        /// </summary>
        /// <param name="user"></param>
        /// <param name="BrandName"></param>
        /// <returns></returns>
        public int ExistDeliveryModeName(string name)
        {
            return ServiceIoc.Get<DeliveryModeService>().ExistName(name, bid);
        }




        #endregion


    }
}