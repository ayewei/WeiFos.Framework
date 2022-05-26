using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WeiFos.Admin.Code.Authorization;
using WeiFos.Admin.Controllers;
using WeiFos.CodeBuilder.Entity;
using WeiFos.NetCore.Extensions;
using WeiFos.Entity.CodeBuildModule;
using WeiFos.Entity.Enums;
using WeiFos.Entity.SystemModule;
using WeiFos.ORM.Data;
using WeiFos.ORM.Data.Const;
using WeiFos.ORM.Data.Restrictions;
using WeiFos.Service;
using WeiFos.Service.CodeBuildModule;

namespace WeiFos.Admin.Areas.CodeBuildModule.Controllers
{
    /// <summary>
    /// System 控制器
    /// @author yewei 
    /// add by @date 2018-01-11
    /// </summary>
    [LoginAuth]
    [Area(AreaNames.CodeBuildModule)]
    public class CodeBuildController : BaseController
    {



        #region 数据库连接模块



        /// <summary>
        /// 服务器连接管理
        /// </summary>
        /// <returns></returns>
        public IActionResult ServerLinkManage()
        {
            return View();
        }



        /// <summary>
        /// 服务器连接管理表单页面
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public IActionResult ServerLinkForm(SysUser user, ServerLink entity)
        {
            //当前用户信息
            entity = ServiceIoc.Get<ServerLinkService>().GetById(bid);
            if (entity != null)
            {
                ViewBag.entity = JsonConvert.SerializeObject(entity);
            }

            return View();
        }


        /// <summary>
        /// 保存数据库连接
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public JsonResult SaveServerLink(SysUser user, [FromBody]ServerLink entity)
        {
            StateCode code = ServiceIoc.Get<ServerLinkService>().Save(user.id, entity);
            return Json(GetResult(code));
        }



        /// <summary>
        /// 服务器链接翻页
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public JsonResult GetServerLinks(int pageSize, int pageIndex, string keyword)
        {
            try
            {
                //查询对象
                Criteria ct = new Criteria();

                //查询表达式
                MutilExpression me = new MutilExpression();

                ct.SetFromTables("tb_code_server_link")
                .SetPageSize(pageSize)
                .SetStartPage(pageIndex)
                .SetFields(new string[] { "*" })
                .AddOrderBy(new OrderBy("id", "desc"));

                //查询关键词
                if (!string.IsNullOrEmpty(keyword))
                {
                    me.Add(new SingleExpression("", LogicOper.CUSTOM, "("));
                    me.Add(new SingleExpression("name", LogicOper.LIKE, "", keyword));
                    me.Add(new SingleExpression("ip", LogicOper.LIKE, " or ", keyword));
                    me.Add(new SingleExpression("", LogicOper.CUSTOM, "", ")"));
                }

                //设置查询条件
                if (me.Expressions.Count > 0)
                {
                    ct.SetWhereExpression(me);
                }

                //查询
                List<ServerLink> list = ServiceIoc.Get<ServerLinkService>().GetList(ct);

                return PageResult(StateCode.State_200, ct.TotalRow, list);
            }
            catch (Exception ex)
            {
                return PageResult<SysUser>(StateCode.State_500, 0, null);
            }
        }



        /// <summary>
        /// 删除数据库连接
        /// </summary>
        /// <returns></returns>
        public JsonResult DelServerLink()
        {
            try
            {
                ServiceIoc.Get<ServerLinkService>().Delete(bid);
                return Json(GetResult(StateCode.State_200));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }


        #endregion



        #region 项目配置模块


        /// <summary>
        /// 项目配置列表
        /// </summary>
        /// <returns></returns>
        public IActionResult ProjectSettingManage()
        {

            return View();
        }



        /// <summary>
        /// 项目配置页
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IActionResult ProjectSettingForm(SysUser user)
        {
            List<ServerLink> links = ServiceIoc.Get<ServerLinkService>().GetAll();
            ViewBag.links = links;

            ProjectSetting entity = ServiceIoc.Get<ProjectSettingService>().GetById(bid);
            if (entity != null)
            {
                ViewBag.entity = JsonConvert.SerializeObject(entity);
            }

            return View();
        }


        /// <summary>
        /// 提交项目配置页
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entity"></param>
        /// <returns></returns> 
        public JsonResult DoProjectSettingForm(SysUser user, [FromBody] ProjectSetting entity)
        {
            //ProjectSetting projectSetting1 = JsonConvert.DeserializeObject<ProjectSetting>(entity.entity1.ToString());
            StateCode code = ServiceIoc.Get<ProjectSettingService>().Save(user.id, entity);
            //返回数据
            return Json(GetResult(code));
        }



        /// <summary>
        /// 服务器链接翻页
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ContentResult GetProjectSettings(int pageSize, int pageIndex, string keyword)
        {
            try
            {
                //查询对象
                Criteria ct = new Criteria();

                //查询表达式
                MutilExpression me = new MutilExpression();

                ct.SetFromTables("v_code_project_setting")
                .SetPageSize(pageSize)
                .SetStartPage(pageIndex)
                .SetFields(new string[] { "*" })
                .AddOrderBy(new OrderBy("id", "desc"));

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

                //查询
                DataTable list = ServiceIoc.Get<ProjectSettingService>().Fill(ct);

                return PageResult(StateCode.State_200, ct.TotalRow, list);
            }
            catch (Exception ex)
            {
                return PageResult(StateCode.State_500, 0, null);
            }
        }



        /// <summary>
        /// 删除数据库连接
        /// </summary>
        /// <returns></returns>
        public JsonResult DelProjectSetting(int id)
        {
            try
            {
                ServiceIoc.Get<ProjectSettingService>().Del(id);
                return Json(GetResult(StateCode.State_200));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }



        /// <summary>
        /// 项目配置模块
        /// </summary>
        /// <param name="id"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public JsonResult GetProjectModules(int id, string keyword)
        {
            try
            {
                //查询对象
                Criteria ct = new Criteria();

                //查询表达式
                MutilExpression me = new MutilExpression();

                ct.SetFromTables("tb_code_project_module")
                .SetFields(new string[] { "*" })
                .AddOrderBy(new OrderBy("id", "desc"));

                //查询关键词
                if (!string.IsNullOrEmpty(keyword))
                {
                    me.Add(new SingleExpression("name", LogicOper.LIKE, keyword));
                }

                //所属项目ID
                me.Add(new SingleExpression("project_setting_id", LogicOper.EQ, id));

                //设置查询条件
                if (me.Expressions.Count > 0)
                {
                    ct.SetWhereExpression(me);
                }

                //查询
                List<ProjectModule> list = ServiceIoc.Get<ProjectModuleService>().GetList(ct);

                return Json(GetResult(StateCode.State_200, list));
            }
            catch (Exception ex)
            {
                return Json(GetResult(StateCode.State_500));
            }
        }



        /// <summary>
        /// 保存项目配置模块
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public JsonResult SaveProjectModule(SysUser user, [FromBody]ProjectModule entity)
        {
            StateCode code = ServiceIoc.Get<ProjectModuleService>().Save(user.id, entity);
            return Json(GetResult(code));
        }



        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DelProjectModule(int id)
        {
            try
            {
                ServiceIoc.Get<ProjectModuleService>().Delete(id);
                return Json(GetResult(StateCode.State_200));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }



        /// <summary>
        /// 获取模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetProjectModule(int id)
        {
            try
            {
                var module = ServiceIoc.Get<ProjectModuleService>().GetById(id);
                return Json(GetResult(StateCode.State_200, module));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }



        #endregion



        #region 代码生成——操作


        /// <summary>
        /// 代码生成页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }



        /// <summary>
        /// 获取服务器数据库集合
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDataTables()
        {
            try
            {
                var list = ServiceIoc.Get<ServerLinkService>().GetTables(bid);
                return Json(GetResult(StateCode.State_200, list));
            }
            catch (Exception ex)
            {
                return Json(GetResult(StateCode.State_500));
            }
        }



        /// <summary>
        /// 获取表格
        /// </summary>
        /// <param name="link_id"></param>
        /// <param name="tb_name"></param>
        /// <returns></returns>
        public JsonResult GetTableInfo(int link_id, int pid, string tb_name)
        {
            try
            {
                //表详情
                var tableInfo = ServiceIoc.Get<ServerLinkService>().GetDataTableInfo(link_id, tb_name);

                //项目模块
                List<ProjectModule> modules = ServiceIoc.Get<ProjectModuleService>().GetListByPId(pid);

                return Json(GetResult(StateCode.State_200, new { tableInfo, modules }));
            }
            catch (Exception ex)
            {
                return Json(GetResult(StateCode.State_500));
            }
        }




        /// <summary>
        /// 加载数据库连接
        /// </summary>
        /// <returns></returns>
        public JsonResult LoadServer()
        {
            try
            {
                var data = ServiceIoc.Get<ServerLinkService>().LoadServer();
                return Json(GetResult(StateCode.State_200, data));
            }
            catch (Exception ex)
            {
                return Json(GetResult(StateCode.State_500));
            }
        }



        #endregion



        #region 代码生成——生成


        /// <summary>
        /// 代码生成预览页面
        /// </summary>
        /// <returns></returns>
        public IActionResult CodePreview()
        {
            return View();
        }



        /// <summary>
        /// 预览加载代码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="link_id"></param>
        /// <param name="name_space"></param>
        /// <param name="tb_name"></param>
        /// <returns></returns>
        public JsonResult LoadCode(SysUser user, int link_id, int mid, string tb_name)
        {
            try
            {
                //获取命名空间
                string[] arr = ServiceIoc.Get<ProjectModuleService>().GetProjectModule(mid);

                CSharpMVCCode code = ServiceIoc.Get<ServerLinkService>().LoadCode(user, link_id, arr, tb_name);

                return Json(GetResult(StateCode.State_200, code));
            }
            catch
            {
                return Json(GetResult(StateCode.State_500));
            }
        }



        /// <summary>
        /// 立即生成代码
        /// </summary>
        /// <returns></returns>
        public JsonResult CodeBuild()
        {
            try
            {
                var list = ServiceIoc.Get<ServerLinkService>().GetTables(bid);
                return Json(GetResult(StateCode.State_200, list));
            }
            catch (Exception ex)
            {
                return Json(GetResult(StateCode.State_500));
            }
        }




        #endregion





    }
}