using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using weifos.Service.OrgModule;
using Weifos.Service.OrgModule;
using WeiFos.Admin.Code.Authorization;
using WeiFos.Admin.Controllers;
using WeiFos.Core;
using WeiFos.Core.Extensions;
using WeiFos.Core.XmlHelper;
using WeiFos.NetCore.Extensions;
using WeiFos.Entity.Enums;
using WeiFos.Entity.OrgModule;
using WeiFos.Entity.SystemModule;
using WeiFos.ORM.Data;
using WeiFos.ORM.Data.Const;
using WeiFos.ORM.Data.Restrictions;
using WeiFos.Service;
using WeiFos.Service.OrgModule;

namespace WeiFos.Admin.Areas.OrgModule.Controllers
{

    /// <summary>
    /// 组织管理 控制器
    /// @author yewei 
    /// add by @date 2015-01-11
    /// </summary>
    [LoginAuth]
    [Area(AreaNames.OrgModule)]
    public class OrgController : BaseController
    {


        #region 组织机构——公司管理

        /// <summary>
        /// 系统菜单管理
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IActionResult CompanyManage(SysUser user)
        {
            return View();
        }


        /// <summary>
        /// 公司查询分页
        /// </summary>
        /// <returns></returns>
        public ContentResult GetCompanys(int pageSize, int pageIndex, string keyword)
        {
            try
            {
                //查询对象
                Criteria ct = new Criteria();

                //查询表达式
                MutilExpression me = new MutilExpression();

                ct.SetFromTables("tb_org_company")
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

                DataTable dt = ServiceIoc.Get<CompanyService>().Fill(ct);

                return PageResult(StateCode.State_200, ct.TotalRow, dt);
            }
            catch
            {
                return PageResult(StateCode.State_500, 0, null);
            }
        }


        /// <summary>
        /// 初始化菜单页面
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IActionResult CompanyForm(SysUser user, Company? entity = null)
        {
            if (Request.IsAjaxRequest())
            {
                //保存菜单
                StateCode code = ServiceIoc.Get<CompanyService>().Save(user.id, entity);

                //初始化
                AppGlobal.Instance.Initial();

                //返回数据
                return Json(GetResult(code));
            }
            else
            {
                List<Company> menuList = ServiceIoc.Get<CompanyService>().GetTrees("", HttpUtility.HtmlDecode("&nbsp;&nbsp;"));
                menuList.Insert(0, new Company() { name = "根目录", id = 0 });
                ViewBag.Parents = menuList;

                entity = ServiceIoc.Get<CompanyService>().GetById(bid);
                if (entity != null)
                {
                    ViewBag.entity = JsonConvert.SerializeObject(entity);
                }
            }

            return View();
        }


        /// <summary>
        /// 删除公司信息
        /// </summary>
        /// <returns></returns>
        public IActionResult DelCompany()
        {
            try
            {
                ServiceIoc.Get<CompanyService>().Delete(bid);
                return Json(GetResult(StateCode.State_200));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }


        #endregion


        #region 组织机构——部门管理


        /// <summary>
        /// 获取公司列表
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public JsonResult GetCompanysTree(string keyword)
        {
            List<Company> dt = ServiceIoc.Get<CompanyService>().GetCompanyTrees(keyword);
            return Json(GetResult(StateCode.State_200, dt));
        }


        /// <summary>
        /// 部门管理页
        /// </summary>
        public IActionResult DepartmentManage()
        {
            return View();
        }


        /// <summary>
        /// 部门表单页
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public IActionResult DepartmentForm(SysUser user, Department entity)
        {
            if (Request.IsAjaxRequest())
            {
                StateCode code = ServiceIoc.Get<DepartmentService>().Save(user.id, entity);
                return Json(GetResult(code));
            }
            else
            {
                List<Company> menuList = ServiceIoc.Get<CompanyService>().GetTrees("", HttpUtility.HtmlDecode("&nbsp;&nbsp;"));
                menuList.Insert(0, new Company() { name = "根目录", id = 0 });
                ViewBag.Parents = menuList;

                List<Department> departments = ServiceIoc.Get<DepartmentService>().GetTrees("", HttpUtility.HtmlDecode("&nbsp;&nbsp;"));
                departments.Insert(0, new Department() { name = "根目录", id = 0 });
                ViewBag.Departments = departments;

                entity = ServiceIoc.Get<DepartmentService>().GetById(bid);
                if (entity != null)
                {
                    ViewBag.entity = JsonConvert.SerializeObject(entity);
                }
            }

            return View();
        }


        /// <summary>
        /// 部门查询
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="company_id"></param>
        /// <param name="keyword"></param>
        /// </summary>
        public ContentResult GetDepartments(int pageSize, int pageIndex, long company_id, string keyword)
        {
            try
            {
                //查询对象
                Criteria ct = new Criteria();

                //查询表达式
                MutilExpression me = new MutilExpression();

                ct.SetFromTables("v_org_department")
                .SetPageSize(pageSize)
                .SetStartPage(pageIndex)
                .SetFields(new string[] { "*" })
                .AddOrderBy(new OrderBy("id", "desc"));

                //所属公司
                if (company_id != 0)
                {
                    me.Add(new SingleExpression("company_id", LogicOper.EQ, company_id));
                }

                //查询关键词
                if (!string.IsNullOrEmpty(keyword))
                {
                    me.Add(new SingleExpression("name", LogicOper.LIKE, keyword));
                }

                //设置查询条件
                if (me.Expressions.Count > 0)
                {
                    ct.SetWhereExpression(me);
                }

                DataTable data = ServiceIoc.Get<DepartmentService>().Fill(ct);
                return PageResult(StateCode.State_200, ct.TotalRow, data);
            }
            catch (Exception ex)
            {
                return PageResult(StateCode.State_500, 0, null);
            }
        }


        /// <summary>
        /// 删除部门信息
        /// </summary>
        /// <returns></returns>
        public IActionResult DelDepartment()
        {
            try
            {
                ServiceIoc.Get<DepartmentService>().Delete(bid);
                return Json(GetResult(StateCode.State_200));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }


        #endregion


        #region 组织机构——岗位管理



        /// <summary>
        /// 部门管理页
        /// </summary>
        public IActionResult PostManage()
        {
            return View();
        }



        /// <summary>
        /// 部门表单页
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public IActionResult PostForm(SysUser user, OrgPost entity)
        {
            if (Request.IsAjaxRequest())
            {
                StateCode code = ServiceIoc.Get<OrgPostService>().Save(user.id, entity);
                return Json(GetResult(code));
            }
            else
            {
                //所属公司
                List<Company> menuList = ServiceIoc.Get<CompanyService>().GetTrees("", HttpUtility.HtmlDecode("&nbsp;&nbsp;"));
                menuList.Insert(0, new Company() { name = "根目录", id = 0 });
                ViewBag.Parents = menuList;

                //所属部门
                List<Department> departments = ServiceIoc.Get<DepartmentService>().GetTrees("", HttpUtility.HtmlDecode("&nbsp;&nbsp;"));
                departments.Insert(0, new Department() { name = "根目录", id = 0 });
                ViewBag.Departments = departments;

                //所属岗位
                List<OrgPost> posts = ServiceIoc.Get<OrgPostService>().GetTrees("", HttpUtility.HtmlDecode("&nbsp;&nbsp;"));
                posts.Insert(0, new OrgPost() { name = "根目录", id = 0 });
                ViewBag.Posts = posts;

                entity = ServiceIoc.Get<OrgPostService>().GetById(bid);
                if (entity != null)
                {
                    ViewBag.entity = JsonConvert.SerializeObject(entity);
                }
            }

            return View();
        }



        /// <summary>
        /// 部门查询
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="company_id"></param>
        /// <param name="keyword"></param>
        /// </summary>
        public ContentResult GetPosts(int pageSize, int pageIndex, long company_id, string keyword)
        {
            try
            {

                //查询对象
                Criteria ct = new Criteria();

                //查询表达式
                MutilExpression me = new MutilExpression();

                ct.SetFromTables("v_org_post")
                .SetPageSize(pageSize)
                .SetStartPage(pageIndex)
                .SetFields(new string[] { "*" })
                .AddOrderBy(new OrderBy("id", "desc"));

                //所属公司
                if (company_id != 0)
                {
                    me.Add(new SingleExpression("company_id", LogicOper.EQ, company_id));
                }

                //查询关键词
                if (!string.IsNullOrEmpty(keyword))
                {
                    me.Add(new SingleExpression("name", LogicOper.LIKE, keyword));
                }

                //设置查询条件
                if (me.Expressions.Count > 0)
                {
                    ct.SetWhereExpression(me);
                }

                DataTable data = ServiceIoc.Get<OrgPostService>().Fill(ct);
                return PageResult(StateCode.State_200, ct.TotalRow, data);
            }
            catch (Exception ex)
            {
                return PageResult(StateCode.State_500, 0, null);
            }
        }



        /// <summary>
        /// 删除岗位
        /// </summary>
        /// <returns></returns>
        public IActionResult DelPost()
        {
            try
            {
                ServiceIoc.Get<OrgPostService>().Delete(bid);
                return Json(GetResult(StateCode.State_200));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }

        #endregion


        #region 组织机构——员工管理

        /// <summary>
        /// 部门管理页
        /// </summary>
        public IActionResult EmployeeManage()
        {
            return View();
        }


        /// <summary>
        /// 部门表单页
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public IActionResult EmployeeForm(SysUser user, Employee entity)
        {
            if (Request.IsAjaxRequest())
            {
                StateCode code = ServiceIoc.Get<EmployeeService>().Save(user.id, entity);
                return Json(GetResult(code));
            }
            else
            {
                //所属公司
                List<Company> companyList = ServiceIoc.Get<CompanyService>().GetTrees("", HttpUtility.HtmlDecode("&nbsp;&nbsp;"));
                companyList.Insert(0, new Company() { name = "根目录", id = 0 });
                ViewBag.Parents = companyList;

                //所属部门
                List<Department> departments = ServiceIoc.Get<DepartmentService>().GetTrees("", HttpUtility.HtmlDecode("&nbsp;&nbsp;"));
                departments.Insert(0, new Department() { name = "根目录", id = 0 });
                ViewBag.Departments = departments;

                //所属岗位
                List<OrgPost> orgPosts = ServiceIoc.Get<OrgPostService>().GetTrees("", HttpUtility.HtmlDecode("&nbsp;&nbsp;"));
                orgPosts.Insert(0, new OrgPost() { name = "根目录", id = 0 });
                ViewBag.Posts = orgPosts;

                entity = ServiceIoc.Get<EmployeeService>().GetById(bid);
                if (entity != null)
                {
                    ViewBag.entity = JsonConvert.SerializeObject(entity);
                }
            }

            return View();
        }


        /// <summary>
        /// 部门查询
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="company_id"></param>
        /// <param name="keyword"></param>
        /// </summary>
        public ContentResult GetEmployees(int pageSize, int pageIndex, long company_id, string keyword)
        {
            try
            {
                //查询对象
                Criteria ct = new Criteria();

                //查询表达式
                MutilExpression me = new MutilExpression();

                ct.SetFromTables("v_org_employee")
                .SetPageSize(pageSize)
                .SetStartPage(pageIndex)
                .SetFields(new string[] { "*" })
                .AddOrderBy(new OrderBy("id", "desc"));

                //所属公司
                if (company_id != 0)
                {
                    me.Add(new SingleExpression("company_id", LogicOper.EQ, company_id));
                }

                //查询关键词
                if (!string.IsNullOrEmpty(keyword))
                {
                    me.Add(new SingleExpression("name", LogicOper.LIKE, keyword));
                }

                //设置查询条件
                if (me.Expressions.Count > 0)
                {
                    ct.SetWhereExpression(me);
                }

                DataTable data = ServiceIoc.Get<DepartmentService>().Fill(ct);
                return PageResult(StateCode.State_200, ct.TotalRow, data);
            }
            catch (Exception ex)
            {
                return PageResult(StateCode.State_500, 0, null);
            }
        }


        /// <summary>
        /// 删除员工信息
        /// </summary>
        /// <returns></returns>
        public IActionResult DelEmployee()
        {
            try
            {
                ServiceIoc.Get<EmployeeService>().Delete(bid);
                return Json(GetResult(StateCode.State_200));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }



        /// <summary>
        /// 验证用户编号是否存在
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public int EmployeeNo(string no)
        {
            return ServiceIoc.Get<EmployeeService>().ExistUserNo(no, bid);
        }



        /// <summary>
        /// 实时搜索
        /// </summary>
        /// <param name="user"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public JsonResult RTSearch(SysUser user, string k)
        {
            var ret = ServiceIoc.Get<EmployeeService>().TopSearchNo(k);
            return Json(GetResult(StateCode.State_200, ret));
        }


        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="order_no"></param>
        /// <returns></returns>
        public ContentResult GetSearchEmployee(SysUser user, string k)
        {
            //订单详情
            Employee entity = ServiceIoc.Get<EmployeeService>().TopSearchNo(k, 1).FirstOrDefault();

            //返回数据
            return Content(JsonConvert.SerializeObject(GetResult(StateCode.State_200, new { entity })));
        }

        #endregion


    }
}