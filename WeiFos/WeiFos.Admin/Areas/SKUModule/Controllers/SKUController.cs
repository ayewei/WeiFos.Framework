using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiFos.Core;
using WeiFos.ORM.Data;
using WeiFos.ORM.Data.Restrictions;
using WeiFos.ORM.Data.Const;
using WeiFos.Entity.SystemModule;
using WeiFos.Entity.SKUModule;
using WeiFos.Service;
using WeiFos.Service.SKUModule;
using WeiFos.Entity.Common;
using Newtonsoft.Json;
using WeiFos.Entity.Enums;
using WeiFos.Entity.OrgModule;
using WeiFos.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;
using WeiFos.Admin.Code.Authorization;
using WeiFos.NetCore.Extensions;


namespace WeiFos.Admin.Areas.SKUModule.Controllers
{
    /// <summary>
    /// 平台商品SKU 控制器
    /// @author yewei 
    /// add by @date 2015-03-10
    /// </summary>
    [LoginAuth]
    [Area(AreaNames.SKUModule)]
    public class SKUController : BaseController
    {

        #region 商品类型——SKU管理——商品类型


        /// <summary>
        /// 商品类型管理页面 
        /// </summary>
        /// <returns></returns>
        public IActionResult ProductTypeManage()
        {
            return View();
        }


        /// <summary>
        /// 商品类型保存页面
        /// </summary>
        /// <returns></returns>
        public IActionResult ProductTypeForm(SysUser user, ProductType entity)
        {
            if (NHttpContext.Current.Request.IsAjaxRequest())
            {
                StateCode code = ServiceIoc.Get<ProductTypeService>().Save(user.id, entity);
                return Json(GetResult(code));
            }
            else
            {
                entity = ServiceIoc.Get<ProductTypeService>().GetById(bid);
                if (entity != null)
                {
                    ViewBag.bid = bid;
                    ViewBag.entity = JsonConvert.SerializeObject(entity);
                }
            }

            return View();
        }



        /// <summary>
        /// 商品类型列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public JsonResult GetProductTypes(int pageSize, int pageIndex, string keyword)
        {
            //查询对象
            Criteria ct = new Criteria();

            //查询表达式
            MutilExpression me = new MutilExpression();

            ct.SetPageSize(pageSize)
            .SetStartPage(pageIndex)
            .SetFields(new string[] { "*" })
            .AddOrderBy(new OrderBy("id", "desc"));

            //名称
            if (!string.IsNullOrEmpty(keyword))
            {
                me.Add(new SingleExpression("name", LogicOper.LIKE, keyword));
            }

            //设置查询条件
            if (me.Expressions.Count > 0)
            {
                ct.SetWhereExpression(me);
            }

            List<ProductType> data = ServiceIoc.Get<ProductTypeService>().GetList(ct);

            return PageResult(StateCode.State_200, ct.TotalRow, data);
        }


        /// <summary>
        /// 删除供应商商品类型
        /// </summary>
        /// <param name="ProductTypeName"></param>
        /// <returns></returns>
        public JsonResult DeleteTypeName(SysUser user)
        {
            StateCode state = ServiceIoc.Get<ProductTypeService>().DeleteType(bid);
            return Json(GetResult(state));
        }

        /// <summary>
        /// 供应商商品类型 是否存在
        /// </summary>
        /// <param name="ProductTypeName"></param>
        /// <returns></returns>
        public int ExistTypeName(SysUser user, string name)
        {
            return ServiceIoc.Get<ProductTypeService>().ExistTypeName(bid, name);
        }



        #endregion


        #region 商品类型——SKU模块——商品类型基础属性

        /// <summary>
        /// 商品类型 基础属性
        /// </summary>
        /// <returns></returns>
        public IActionResult AttrName(SysUser user)
        {
            ProductType entity = ServiceIoc.Get<ProductTypeService>().Get(bid);
            if (entity == null) return RedirectToAction("ProductTypeManage");

            ViewBag.bid = bid;
            ViewBag.entity = entity;
            return View();
        }


        /// <summary>
        /// 获取供应商商品类型基础属性
        /// </summary>
        /// <param name="user"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public JsonResult GetAttrNames(SysUser user, string keyword)
        {
            List<AttrName> data = ServiceIoc.Get<AttrNameService>().Gets(bid, keyword);
            return Json(GetResult(StateCode.State_200, data));
        }


        /// <summary>
        /// 保存供应商商品类型基础属性
        /// </summary>
        /// <param name="user"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public JsonResult SaveAttrName(SysUser user, AttrName attrName)
        {
            StateCode code = ServiceIoc.Get<AttrNameService>().Save(user.id, attrName);
            return Json(GetResult(code));
        }



        /// <summary>
        /// 删除商品类型基础属性
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public JsonResult DeleteAttrName()
        {
            StateCode state = ServiceIoc.Get<AttrNameService>().DeleteAttrName(bid);
            return Json(GetResult(state));
        }


        #endregion


        #region 商品类型——SKU模块——商品类型扩展属性


        /// <summary>
        /// 商品类型 扩展属性
        /// </summary>
        /// <returns></returns>
        public IActionResult ExtAttrName(SysUser user)
        {
            int exist = ServiceIoc.Get<ProductTypeService>().ExistById(bid);
            if (exist == 0) return RedirectToAction("ProductTypeManage");

            //商品类型ID
            ViewBag.bid = bid;
            //当前实体
            ViewBag.entity = exist;

            return View();
        }


        /// <summary>
        /// 获取扩展属性
        /// </summary>
        /// <param name="user"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public JsonResult GetExtAttrNames(SysUser user, string keyword)
        {
            //获取扩展名称
            List<ExtAttrName> resultlist = ServiceIoc.Get<ExtAttrNameService>().Gets(bid, keyword);

            //扩展属性值
            List<ExtAttrVal> extAttrVals = new List<ExtAttrVal>();

            foreach (ExtAttrName s in resultlist)
            {
                List<ExtAttrVal> extAttrValTmp = ServiceIoc.Get<ExtAttrValService>().Gets(s.id);
                if (extAttrValTmp.Count > 0)
                {
                    extAttrVals.AddRange(extAttrValTmp);
                }
            }

            return Json(GetResult(StateCode.State_200, new { result = resultlist, vals = extAttrVals }));
        }


        /// <summary>
        /// 保存扩展属性
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public JsonResult SaveExtAttrName(SysUser user, ExtAttrName extAttrName, string attrvals)
        {
            //属性值集合
            string[] arr_val = null;
            if (attrvals != null && attrvals.IndexOf("，") != -1) attrvals = attrvals.Replace("，", ",");

            arr_val = StringHelper.StringToArray(attrvals);

            if (extAttrName.id == 0)
            {
                extAttrName.created_user_id = user.id;
                extAttrName.created_date = DateTime.Now;
            }
            else
            {
                extAttrName.updated_user_id = user.id;
                extAttrName.updated_date = DateTime.Now;
            }

            //保存
            StateCode state = ServiceIoc.Get<ExtAttrNameService>().Save(extAttrName, arr_val);

            return Json(GetResult(state));
        }

        /// <summary>
        /// 删除扩展属性名称
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public JsonResult DeleteExtAttrName(SysUser user)
        {
            StateCode state = ServiceIoc.Get<ExtAttrNameService>().DeleteExtAttrName(bid);
            return Json(GetResult(state));
        }

        /// <summary>
        /// 删除扩展属性值
        /// </summary>
        /// <returns></returns>
        public JsonResult DeleteExtAttrVal()
        {
            StateCode state = ServiceIoc.Get<ExtAttrValService>().DeleteExtAttrVal(bid);
            return Json(GetResult(state));
        }

        /// <summary>
        /// 保存扩展属性值
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public JsonResult SaveExtAttrVal(SysUser user, ExtAttrVal extAttrVal)
        {

            if (extAttrVal.id != 0)
            {
                extAttrVal.updated_user_id = user.id;
                extAttrVal.updated_date = DateTime.Now;
            }
            else
            {
                extAttrVal.created_user_id = user.id;
                extAttrVal.created_date = DateTime.Now;
            }

            StateCode state = ServiceIoc.Get<ExtAttrValService>().SaveExtAttrVal(extAttrVal);
            return Json(GetResult(state));
        }


        #endregion


        #region 商品类型——SKU模块——规格模块


        /// <summary>
        /// 规格
        /// </summary>
        /// <returns></returns>
        public IActionResult SpecName()
        {
            ProductType productType = ServiceIoc.Get<ProductTypeService>().Get(bid);
            if (productType == null) return RedirectToAction("ProductTypeManage");

            //商品类型ID
            ViewBag.bid = bid;
            return View();
        }


        /// <summary>
        /// 商品类型获取商品规格
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public JsonResult GetSpecNames(string keyword)
        {
            List<SpecName> resultlist = ServiceIoc.Get<SpecNameService>().Gets(bid, keyword);

            //扩展属性值
            List<SpecValue> specValues = new List<SpecValue>();

            foreach (SpecName s in resultlist)
            {
                List<SpecValue> extAttrValTmp = ServiceIoc.Get<SpecValueService>().Gets(s.id);
                if (extAttrValTmp.Count > 0)
                {
                    specValues.AddRange(extAttrValTmp);
                }
            }

            return Json(GetResult(StateCode.State_200, new { result = resultlist, vals = specValues }));
        }


        /// <summary>
        /// 保存商品规格
        /// </summary>
        /// <param name="user"></param>
        /// <param name="specName"></param>
        /// <param name="specValues"></param>
        /// <returns></returns>
        public JsonResult SaveSpecName(SysUser user, SpecName specName, string specValues)
        {
            //属性值集合
            string[] arr_val = null;
            if (specValues != null && specValues.IndexOf("，") != -1) specValues = specValues.Replace("，", ",");
            arr_val = StringHelper.StringToArray(specValues);

            if (specName.id == 0)
            {
                specName.created_user_id = user.id;
                specName.created_date = DateTime.Now;
            }
            else
            {
                specName.updated_user_id = user.id;
                specName.updated_date = DateTime.Now;
            }

            //保存
            StateCode state = ServiceIoc.Get<SpecNameService>().Save(specName, arr_val);
            return Json(GetResult(state));
        }

        /// <summary>
        /// 保存扩展属性值
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public JsonResult SaveSpecValue(SysUser user, SpecValue specValue)
        {
            if (specValue.id != 0)
            {
                specValue.updated_user_id = user.id;
                specValue.updated_date = DateTime.Now;
            }
            else
            {
                specValue.created_user_id = user.id;
                specValue.created_date = DateTime.Now;
            }

            StateCode state = ServiceIoc.Get<SpecValueService>().SaveSpecValue(specValue);
            return Json(GetResult(state));
        }

        /// <summary>
        /// 删除规格
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public JsonResult DeleteSpecName(SysUser user)
        {
            StateCode state = ServiceIoc.Get<SpecNameService>().DeleteSpecName(bid);
            return Json(GetResult(state));
        }

        /// <summary>
        /// 删除规格值
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public JsonResult DeleteSpecValue(SysUser user)
        {
            StateCode state = ServiceIoc.Get<SpecValueService>().DeleteSpecValue(bid);
            return Json(GetResult(state));
        }

        #endregion



        /// <summary>
        /// 创建SKU信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public JsonResult CreateSKU(SysUser user)
        {
            try
            {
                var data = ServiceIoc.Get<ProductTypeService>().CreateSKU(bid);
                return Json(GetResult(StateCode.State_200, data));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }




    }
}