using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiFos.CodeBuilder.Entity;
using WeiFos.ORM.Data.DBEntityModule;

namespace WeiFos.CodeBuilder.Builder.CSharp
{
    /// <summary>
    /// C# 代码生成
    /// @author yewei 
    /// @date 2018-11-17
    /// </summary>
    public static class BuildCSharpMVC
    {


        #region 代码生成模块


        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <param name="pro_module"></param>
        /// <param name="author"></param>
        /// <returns></returns>
        public static CSharpMVCCode CreateCode(TableInfo tableInfo, string[] pro_module, string author)
        {
            CSharpMVCCode code = new CSharpMVCCode();

            //生成实体
            code.entity = CreateEntity(tableInfo, pro_module, author);
            //生成业务逻辑
            code.service = CreateService(tableInfo, pro_module, author);
            //生成表单脚本
            code.js_form = CreateFormJS(tableInfo, pro_module, author);
            //生成管理页脚本
            code.js_manage = CreateManageJS(tableInfo, pro_module, author);
            //生成表单页
            code.view_form = CreateFormView(tableInfo, pro_module, author);
            //生成表单管理页
            code.view_manage = CreateManageView(tableInfo, pro_module, author);
            //生成表单管理页
            code.action = CreateController(tableInfo, pro_module, author);

            return code;
        }


        #endregion


        #region 实体生成模块


        /// <summary>
        /// 实体类创建
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <param name="name_space"></param>
        /// <param name="author"></param>
        /// <returns></returns>
        public static string CreateEntity(TableInfo tableInfo, string[] pro_module, string author)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("using System;\r\n");
                sb.Append("using WeiFos.ORM.Data.Attributes;\r\n\r\n");

                //该类是否具备基类
                bool has_base = BuildCSharpUitl.HasBaseClass(tableInfo.fields);
                //获取基类
                string base_class = has_base ? " : BaseClass" : "";

                sb.Append("namespace ").Append(pro_module[0]).Append(".Domain.").Append(pro_module[1]).Append("Module\r\n");
                sb.Append("{\r\n\r\n");
                sb.Append(BuildCSharpUitl.NotesCreate1(author, tableInfo.remark));
                sb.Append("    [Serializable]\r\n");
                sb.Append("    [Table(Name = \"" + tableInfo.name + "\")]\r\n");
                sb.Append("    public class " + BuildCSharpUitl.GetClassName(tableInfo.name) + base_class + " \r\n");
                sb.Append("    {\r\n\r\n");
                sb.Append("        #region 实体成员\r\n\r\n");

                #region 设置字段根据数据库字段

                foreach (var field in tableInfo.fields)
                {
                    string datatype = GetDataType(field.typename);

                    //如果不是基础字段
                    if (!(has_base && BuildCSharpUitl.IsBaseFiled(field.name)))
                    {
                        sb.Append("        /// <summary>\r\n");
                        sb.Append("        /// " + GetRemark(field.remark) + "\r\n");
                        sb.Append("        /// </summary>\r\n");
                        sb.Append("        /// <returns></returns>\r\n");
                        //主键
                        if (field.is_primary) sb.Append("        [ID]\r\n");
                        sb.Append("        public " + datatype + " " + field.name + " { get; set; }\r\n\r\n");
                    }
                }

                #endregion

                sb.Append("        #endregion\r\n\r\n");

                sb.Append("    }\r\n");
                sb.Append("}\r\n\r\n");

                return sb.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }



        #endregion


        #region 业务逻辑生成模块



        /// <summary>
        /// 业务逻辑生成
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <param name="pro_module"></param>
        /// <param name="author"></param>
        /// <returns></returns>
        public static string CreateService(TableInfo tableInfo, string[] pro_module, string author)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                //实体名称
                string entity_name = BuildCSharpUitl.GetClassName(tableInfo.name); 

                sb.Append("using System;\r\n");
                sb.Append("using WeiFos.ORM.Data;\r\n");
                sb.Append("using WeiFos.Entity.LogsModule;\r\n");
                sb.Append("using ").Append(pro_module[0]).Append(".Domain.Common").Append(";\r\n");
                sb.Append("using ").Append(pro_module[0]).Append(".Domain.").Append(pro_module[1]).Append("Module").Append(";\r\n");
                sb.Append("using ").Append(pro_module[0]).Append(".Service.Common").Append(";\r\n\r\n"); 

                sb.Append("namespace ").Append(pro_module[0]).Append(".Service.").Append(pro_module[1]).Append("Module").Append("\r\n");
                sb.Append("{\r\n\r\n");
                sb.Append(BuildCSharpUitl.NotesCreate1(author, tableInfo.remark.Replace("表", "").Replace("\r\n", "\r\n*") + "业务逻辑"));
                 
                sb.Append("    public class ").Append(entity_name).Append("Service : BaseService<").Append(entity_name).Append(">\r\n");
                sb.Append("    {\r\n\r\n");

                sb.Append("        /// <summary>\r\n");
                sb.Append("        /// 保存\r\n");
                sb.Append("        /// </summary>\r\n");
                sb.Append("        /// <param name=\"user_id\"></param>\r\n");
                sb.Append("        /// <param name=\"entity\"></param>\r\n");
                sb.Append("        /// <returns></returns>\r\n"); 
                sb.Append("        public StateCode Save(long user_id, " + entity_name + " entity)\r\n");
                sb.Append("        {\r\n");
                sb.Append("            using (ISession s = SessionFactory.Instance.CreateSession())\r\n");
                sb.Append("            {\r\n");
                sb.Append("               try\r\n");
                sb.Append("               {\r\n");
                sb.Append("                  if (entity.id == 0)\r\n");
                sb.Append("                  {\r\n");
                sb.Append("                     entity.created_date = DateTime.Now;\r\n");
                sb.Append("                     entity.created_user_id = user_id;\r\n");
                sb.Append("                     s.Insert(entity);\r\n");
                sb.Append("                  }\r\n");
                sb.Append("                  else\r\n");
                sb.Append("                  {\r\n");
                sb.Append("                     entity.updated_date = DateTime.Now;\r\n");
                sb.Append("                     entity.updated_user_id = user_id;\r\n");
                sb.Append("                     s.Update(entity);\r\n");
                sb.Append("                   }\r\n");
                sb.Append("                   return StateCode.State_200;\r\n");
                sb.Append("                }\r\n");
                sb.Append("                catch (Exception ex)\r\n");
                sb.Append("                {\r\n");
                sb.Append("                   s.Insert(new SystemLogs() { content = ex.ToString(), created_date = DateTime.Now, type = 1 });\r\n");
                sb.Append("                   return StateCode.State_500;\r\n");
                sb.Append("                }\r\n");
                sb.Append("            }\r\n");
                sb.Append("        }\r\n\r\n");

                sb.Append("    }\r\n");

                sb.Append("}\r\n\r\n");


                return sb.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion


        #region View页面生成



        /// <summary>
        /// 表单页面脚本生成
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <param name="pro_module"></param>
        /// <param name="author"></param>
        /// <returns></returns>
        public static string CreateFormView(TableInfo tableInfo, string[] pro_module, string author)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                 
                sb.Append("@{\r\n");
                sb.Append("    Layout = \"~/Views/Shared/_Layout.cshtml\";\r\n");
                sb.Append("}\r\n");
                sb.Append("@section Js{\r\n"); 
                sb.Append("    <script src=\"@(AppGlobal.Res)Content/Resources/Script/plugins/yw-jq-plugin/yw.config.js?@AppGlobal.VNo\"></script>\r\n");
                sb.Append("    <script src=\"@(AppGlobal.Res)Content/Resources/Script/plugins/yw-jq-plugin/validate/validates-2.1.3.js?@AppGlobal.VNo\"></script>\r\n");
                sb.Append("    <script src=\"@(AppGlobal.Res)Content/Resources/Script/Admin/Areas/").Append(pro_module[1]).Append("/").Append(BuildCSharpUitl.GetClassName(tableInfo.name)).Append("Form.js?@AppGlobal.VNo\"></script>\r\n");
                sb.Append("}\r\n\r\n");

                string remark = tableInfo.remark.Replace("表", "");
                sb.Append("<div class=\"content pd5\">\r\n");
                sb.Append("    <section class=\"content-header\">\r\n");
                sb.Append("        <div class=\"container-fluid\">\r\n");
                sb.Append("            <div class=\"row mb-2\">\r\n");
                sb.Append("                <div class=\"col-sm-6\">\r\n");
                sb.Append("                    <ol class=\"breadcrumb\">\r\n");
                sb.Append("                       <li class=\"breadcrumb-item\"><a href=\"@AppGlobal.Admin\">首页</a></li>\r\n");
                sb.Append("                       <li class=\"breadcrumb-item\"><a href=\"@(AppGlobal.Admin)SystemModule/System/").Append(BuildCSharpUitl.GetClassName(tableInfo.name)).Append("Manage\">").Append(remark).Append("管理").Append("</a></li>\r\n");
                sb.Append("                       <li class=\"breadcrumb-item active\">@(ViewBag.entity == null ? \"添加\" : \"修改\")").Append(remark).Append("</li>\r\n");
                sb.Append("                    </ol>\r\n");
                sb.Append("                </div>\r\n");
                sb.Append("            </div>\r\n");
                sb.Append("        </div>\r\n"); 
                sb.Append("    </section>\r\n");

                sb.Append("    <div class=\"controls box\">\r\n");
                sb.Append("        <table class=\"table_s1 mt30\" >\r\n");
                bool has_base = BuildCSharpUitl.HasBaseClass(tableInfo.fields);
                foreach (var field in tableInfo.fields)
                {
                    //如果不是基础字段
                    if (!BuildCSharpUitl.IsBaseFiled(field.name) && !field.is_primary)
                    {
                        string file_name = field.remark.Replace("\r\n", "");
                        sb.Append("        <tr>\r\n");
                        sb.Append("            <th scope=\"row\">").Append(file_name).Append("：</th>\r\n");
                        sb.Append("            <td>\r\n");
                        sb.Append("                <input type=\"text\" maxlength=\"50\" data-val=\"").Append(field.name).Append("\" class=\"form-control form-control-sm\" />\r\n");
                        sb.Append("            </td>\r\n");
                        sb.Append("        </tr>\r\n");
                    }
                }
                sb.Append("        </table>\r\n");

                sb.Append("        <div class=\"form-actions\" >\r\n");
                sb.Append("            <input type=\"hidden\" id=\"entity\" value=\"@ViewBag.entity\" />\r\n");
                sb.Append("            <input type=\"button\" class=\"btn btn-info\" value=\"重新加载\" onclick=\"javascript: window.location.reload();\" />\r\n");
                sb.Append("            <input type=\"button\" class=\"btn btn-cancel\" value=\"返 回\" id=\"BackBtn\" onclick=\"javascript: history.go(-1);\" />\r\n");
                sb.Append("            <input type=\"button\" class=\"btn btn-small btn-primary btn-save\" value=\"保 存\" id=\"SaveBtn\" />\r\n");
                sb.Append("        </div>\r\n");
                sb.Append("    </div>\r\n");
                sb.Append("</div>\r\n");

                return sb.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// 管理页面脚本生成
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <param name="pro_module"></param>
        /// <param name="author"></param>
        /// <returns></returns>
        public static string CreateManageView(TableInfo tableInfo, string[] pro_module, string author)
        {
            try
            {
                StringBuilder sb = new StringBuilder(); 

                sb.Append("@{\r\n");
                sb.Append("    Layout = \"~/Views/Shared/_Layout.cshtml\";\r\n");
                sb.Append("}\r\n");
                sb.Append("@section Js{\r\n");
                sb.Append("    <script src=\"@(AppGlobal.Res)Content/Resources/Script/plugins/template/template-web.js?@AppGlobal.VNo\"></script>\r\n");
                sb.Append("    <script src=\"@(AppGlobal.Res)Content/Resources/Script/plugins/template/template.helper.js?@AppGlobal.VNo\"></script>\r\n");
                sb.Append("    <script src=\"@(AppGlobal.Res)Content/Resources/Script/plugins/yw-jq-plugin/datagrid/datagrid-1.0.js?@AppGlobal.VNo\"></script>\r\n");
                sb.Append("    <script src=\"@(AppGlobal.Res)Content/Resources/Script/Admin/Areas/").Append(pro_module[1]).Append("/").Append(BuildCSharpUitl.GetClassName(tableInfo.name)).Append("Manage.js?@AppGlobal.VNo\"></script>\r\n");
                sb.Append("}\r\n\r\n");

                string remark = tableInfo.remark.Replace("表", "");
                sb.Append("<div class=\"content pd5\">\r\n");
                sb.Append("    <div class=\"row\">\r\n");
                sb.Append("         <div class=\"col-12\">\r\n");
                sb.Append("            <div class=\"card-body\">\r\n");
                sb.Append("                <div class=\"dataTables_wrapper container-fluid dt-bootstrap4\">\r\n");
                sb.Append("                    <div class=\"row\">\r\n");

                sb.Append("                       <div class=\"col-md-6\">\r\n");
                sb.Append("                           <div class=\"dataTables_length\">\r\n");
                sb.Append("                               <div class=\"input-group input-group-sm\">\r\n");
                sb.Append("                                   <input type=\"text\" id=\"keyword\" class=\"form-control float-right form-control-sm\" placeholder=\"Search\" />\r\n");
                sb.Append("                                   <div class=\"input-group-append\">\r\n");
                sb.Append("                                       <button type=\"button\" class=\"btn btn-default\" name=\"search_btn\"><i class=\"fa fa-search\"></i></button>\r\n");
                sb.Append("                                   </div>\r\n");
                sb.Append("                               </div>\r\n");
                sb.Append("                           </div>\r\n");
                sb.Append("                       </div>\r\n");

                sb.Append("                       <div class=\"col-md-6\">\r\n");
                sb.Append("                           <div class=\"dataTables_filter\">\r\n");
                sb.Append("                               <div class=\"btn-group btn-group-sm\">\r\n");
                sb.Append("                                   <a name=\"refresh_btn\" class=\"btn btn-default\"><i class=\"fa fa-refresh\"></i></a>\r\n"); 
                sb.Append("                               </div>\r\n");
                sb.Append("                               <div class=\"btn-group btn-group-sm\">\r\n");
                sb.Append("                                   <a name=\"add_btn\" class=\"btn btn-default\"><i class=\"fa fa-plus\"></i>&nbsp;新增</a>\r\n"); 
                sb.Append("                                   <a name=\"edit_btn\" class=\"btn btn-default\"><i class=\"fa fa-pencil-square-o\"></i>&nbsp;编辑</a>\r\n"); 
                sb.Append("                                   <a name=\"delete_btn\" class=\"btn btn-default\"><i class=\"fa fa-trash-o\"></i>&nbsp;删除</a>\r\n");
                sb.Append("                               </div>\r\n");
                sb.Append("                           </div>\r\n");
                sb.Append("                       </div>\r\n");

                sb.Append("                    </div>\r\n");
                sb.Append("                    <div class=\"row\">\r\n");
                sb.Append("                       <div class=\"col-sm-12 mt6\">\r\n");
                sb.Append("                           <div name=\"datagrid\"></div>\r\n");
                sb.Append("                       </div>\r\n");
                sb.Append("                    </div>\r\n");
                sb.Append("                </div>\r\n");
                sb.Append("            </div>\r\n");
                sb.Append("        </div>\r\n");
                sb.Append("    </div>\r\n");

                sb.Append("     <script type=\"text/html\" id=\"template\">\r\n");
                sb.Append("         {{each data as obj i}}\r\n");
                sb.Append("         <tr data-id=\"{{obj.id}}\">\r\n");
                bool has_base = BuildCSharpUitl.HasBaseClass(tableInfo.fields);
                foreach (var field in tableInfo.fields)
                {
                    //如果不是基础字段
                    if (!(has_base && BuildCSharpUitl.IsBaseFiled(field.name) && !field.is_primary))
                    {
                        string file_name = field.remark.Replace("\r\n", "");
                        sb.Append("             <td>{{").Append("obj.").Append(field.name).Append("}}").Append("</td>\r\n");
                    }
                }
                sb.Append("         </tr>\r\n");
                sb.Append("         {{/each}}\r\n");
                sb.Append("     </script>\r\n");

                sb.Append("</div>\r\n");

                return sb.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }



        #endregion


        #region JavaScript脚本生成



        /// <summary>
        /// 表单页面脚本生成
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <param name="pro_module"></param>
        /// <param name="author"></param>
        /// <returns></returns>
        public static string CreateFormJS(TableInfo tableInfo, string[] pro_module, string author)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                //注释头
                sb.Append(BuildCSharpUitl.NotesCreate2(author, tableInfo.remark.Replace("表", "").Replace("\r\n", "\r\n*") + "表单脚本"));

                sb.Append("var entity = {};\r\n\r\n");
                sb.Append("$(function() {\r\n\r\n");
                sb.Append("    //初始化权限编号\r\n");
                sb.Append("    App_G.Util.getPCodes();\r\n\r\n");
                sb.Append("    //映射页面数据\r\n");
                sb.Append("    entity = App_G.Mapping.Load(\"#entity\");\r\n\r\n");
                sb.Append("    //初始化验证插件\r\n");
                sb.Append("    yw.valid.config({\r\n");
                sb.Append("        submiteles: \"#SaveBtn\",\r\n");
                sb.Append("        data: [\r\n");
                sb.Append("            {\r\n");
                sb.Append("                attr: \"data-val\",\r\n");
                sb.Append("                data: entity\r\n");
                sb.Append("            }\r\n");
                sb.Append("        ],\r\n");
                sb.Append("        vsuccess: function () {\r\n\r\n");
                sb.Append("            $(\"#SaveBtn\").setDisable();\r\n");
                sb.Append("            var data = {\r\n");
                sb.Append("                entity: App_G.Mapping.Get(\"data-val\", { id: App_G.Util.getRequestId('bid') })\r\n");
                sb.Append("            };\r\n\r\n");
                sb.Append("            $post(\"/" + pro_module[1] + "Module/" + pro_module[1] + "/").Append(BuildCSharpUitl.GetClassName(tableInfo.name)).Append( "Form\", JSON.stringify(data), function (result) {\r\n");
                sb.Append("                if (result.Code == App_G.Code.Code_200){\r\n"); 
                sb.Append("                    layer.msg(result.Message, { icon: 1 });\r\n");
                sb.Append("                    var url = App_G.Util.getDomain() + \"/" + pro_module[1] + "Module/" + pro_module[1] + "/" + pro_module[1] + "Manage\";\r\n");
                sb.Append("                    setTimeout(\"window.location.href= '\" + url + \"'\", 1000);\r\n");
                sb.Append("                }\r\n");
                sb.Append("                else\r\n");
                sb.Append("                {\r\n");
                sb.Append("                     layer.msg(result.Message, { icon: 2 });\r\n");
                sb.Append("                }\r\n");
                sb.Append("            });\r\n");
                sb.Append("        }\r\n");
                sb.Append("    });\r\n\r\n");


                #region 表单验证模块

                sb.Append("    var v = yw.valid.getValidate({});\r\n\r\n");

                bool has_base = BuildCSharpUitl.HasBaseClass(tableInfo.fields);

                foreach (var field in tableInfo.fields)
                {
                    //如果不是基础字段
                    if (!(has_base && BuildCSharpUitl.IsBaseFiled(field.name)) && !field.is_primary)
                    {
                        string remark = field.remark.Replace("\r\n", "");
                        sb.Append("    //").Append(remark).Append("\r\n");
                        sb.Append("    v.valid(\"[data-val=" + field.name + "]\", {\r\n");
                        sb.Append("        vtype: verifyType.anyCharacter,\r\n");
                        sb.Append("        focus: { msg: \"请输入" + remark + "\" },\r\n");
                        sb.Append("        blur: { msg: \"" + remark + "格式不正确\" }\r\n");
                        sb.Append("    });\r\n\r\n");
                    }
                }

                #endregion

                sb.Append("});");

                return sb.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// 管理页面脚本生成
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <param name="pro_module"></param>
        /// <param name="author"></param>
        /// <returns></returns>
        public static string CreateManageJS(TableInfo tableInfo, string[] pro_module, string author)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                 
                //对象名称
                string tb_name = BuildCSharpUitl.GetClassName(tableInfo.name);
                //对应表单地址
                string form_url = "/" + pro_module[1] + "Module/" + pro_module[1] + "/" + tb_name + "Form";

                //注释头
                sb.Append(BuildCSharpUitl.NotesCreate2(author, tableInfo.remark.Replace("表", "") + "管理页脚本"));

                sb.Append("var datagrid;\r\n\r\n");
                sb.Append("$(function () {\r\n\r\n");

                sb.Append("    App_G.Util.getPCodes();\r\n\r\n");
                sb.Append("    //表格初始化\r\n");
                sb.Append("    datagrid = $(\"[name=datagrid]\").datagrid({\r\n");
                sb.Append("        url: \"/").Append(pro_module[1]).Append("Module/").Append(pro_module[1]).Append("/Get").Append(tb_name).Append("s\",\r\n");
                sb.Append("        data: getSearchData(),\r\n");
                sb.Append("        pager: {\r\n");
                sb.Append("            index: 0,\r\n");
                sb.Append("            pageSize: [10, 20, 50]\r\n");
                sb.Append("        },\r\n");
                sb.Append("        template_id: \"template\",\r\n");
                sb.Append("        column: [\r\n");

                #region 列表表头
                 
                bool has_base = BuildCSharpUitl.HasBaseClass(tableInfo.fields);

                foreach (var field in tableInfo.fields)
                {
                    //如果不是基础字段
                    if (!(has_base && BuildCSharpUitl.IsBaseFiled(field.name)) && !field.is_primary)
                    {
                        string remark = field.remark.Replace("\r\n", "");
                        sb.Append("            {\r\n");
                        sb.Append("                text: \"").Append(remark).Append("\",\r\n");
                        sb.Append("                style: \"width: 120px; \"\r\n");
                        sb.Append("            },\r\n");
                    }
                }

                #endregion

                sb.Append("        ],\r\n");
                sb.Append("        dblclick: function (tr) {\r\n");
                sb.Append("            window.location.href = \"").Append(form_url).Append("?bid=\" + tr.attr(\"data - id\");\r\n");
                sb.Append("        }\r\n");
                sb.Append("    });\r\n\r\n");

                sb.Append("    //查询\r\n");
                sb.Append("    $(\"[name=search_btn]\").click(function () {\r\n");
                sb.Append("        datagrid.execute(getSearchData());\r\n");
                sb.Append("    });\r\n\r\n");

                sb.Append("    //刷新\r\n");
                sb.Append("    $(\"[name=refresh_btn]\").click(function () {\r\n");
                sb.Append("        window.location.reload();\r\n");
                sb.Append("    });\r\n\r\n");

                sb.Append("    //新增\r\n");
                sb.Append("    $(\"[name=add_btn]\").click(function () {\r\n");
                sb.Append("         window.location.href = \"").Append(form_url).Append("\";\r\n");
                sb.Append("    });\r\n\r\n");

                sb.Append("    //删除\r\n");
                sb.Append("    $(\"div.dataTables_filter\").digbox({\r\n");
                sb.Append("        Selector: \"[name=delete_btn]\",\r\n");
                sb.Append("        Title: \"提示信息\",\r\n");
                sb.Append("        Context: \"确定删除该数据吗？\",\r\n");
                sb.Append("        Before: function () {\r\n");
                sb.Append("            var tr = datagrid.getSeleteTr();\r\n");
                sb.Append("            if (tr.length == 0) {\r\n");
                sb.Append("                layer.msg('请选择操作的行', { icon: 2 });\r\n");
                sb.Append("                return false;\r\n");
                sb.Append("            }\r\n");
                sb.Append("            return true;\r\n");
                sb.Append("        },\r\n");
                sb.Append("        CallBack: function (s, c, p) {\r\n");
                sb.Append("            $post(\"/").Append(pro_module[1]).Append("Module/").Append(pro_module[1]).Append("/Del").Append(tb_name).Append("?bid=\" + datagrid.getSeleteTr().attr(\"data-id\"),\"\",\r\n");
                sb.Append("                function (result) {\r\n");
                sb.Append("                    if (result.Code == App_G.Code.Code_200) {\r\n");
                sb.Append("                        layer.msg(result.Message, { icon: 1 });\r\n");
                sb.Append("                        datagrid.execute(getSearchData());\r\n");
                sb.Append("                    } else {\r\n");
                sb.Append("                        layer.msg(result.Message, { icon: 2 });\r\n");
                sb.Append("                    }\r\n");
                sb.Append("            });\r\n");
                sb.Append("        }\r\n");
                sb.Append("    });\r\n\r\n");

                sb.Append("});\r\n\r\n");


                sb.Append("function getSearchData() {\r\n");
                sb.Append("    return { keyword: $(\"#keyword\").val() };\r\n");
                sb.Append("}\r\n\r\n");

                return sb.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }



        #endregion


        #region 控制器生成模块



        /// <summary>
        /// 实体类创建
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <param name="pro_module"></param>
        /// <param name="author"></param>
        /// <returns></returns>
        public static string CreateController(TableInfo tableInfo, string[] pro_module, string author)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                //实体名称
                string class_name = BuildCSharpUitl.GetClassName(tableInfo.name); 
                //模块名称
                string remark = tableInfo.remark.Replace("表", "");

                sb.Append("using System;\r\n");
                sb.Append("using System.Web;\r\n");
                sb.Append("using System.Linq;\r\n");
                sb.Append("using System.Web.Mvc;\r\n");
                sb.Append("using Newtonsoft.Json;\r\n");
                sb.Append("using System.Collections.Generic;\r\n");
                sb.Append("using WeiFos.ORM.Data;\r\n");
                sb.Append("using WeiFos.ORM.Data.Const;\r\n");
                sb.Append("using WeiFos.ORM.Data.Restrictions;\r\n");
                sb.Append("using ").Append(pro_module[0]).Append(".Domain").Append(".Common;\r\n");
                sb.Append("using ").Append(pro_module[0]).Append(".Domain").Append(".SystemModule;\r\n");
                sb.Append("using ").Append(pro_module[0]).Append(".Domain").Append(pro_module[1]).Append(".Module;\r\n");
                sb.Append("using ").Append(pro_module[0]).Append(".Service").Append(pro_module[1]).Append(".Module;\r\n");
                sb.Append("using ").Append(pro_module[0]).Append(".Service;\r\n\r\n");

                //SouTang.Admin.Controllers
                sb.Append("namespace ").Append(pro_module[0]).Append(".Admin.Controllers\r\n");
                sb.Append("{\r\n\r\n");
                sb.Append(BuildCSharpUitl.NotesCreate1(author, remark + "控制器"));
                sb.Append("    public class " + pro_module[1] + "Controller : BaseController\r\n");
                sb.Append("    {\r\n\r\n");

                sb.Append("        /// <summary>\r\n");
                sb.Append("        /// ").Append(remark).Append("管理页").Append("\r\n");
                sb.Append("        /// </summary>\r\n");
                sb.Append("        public ActionResult ").Append(class_name).Append("Manage()\r\n");
                sb.Append("        {\r\n");
                sb.Append("            return View();\r\n");
                sb.Append("        }\r\n\r\n");

                sb.Append("        /// <summary>\r\n");
                sb.Append("        /// ").Append(remark).Append("表单页").Append("\r\n");
                sb.Append("        /// </summary>\r\n");
                sb.Append("        /// <param name=\"user\"></param>\r\n");
                sb.Append("        /// <param name=\"entity\"></param>\r\n");
                sb.Append("        /// <returns></returns>\r\n");
                sb.Append("        public ActionResult ").Append(class_name).Append("Form(SysUser user,").Append(class_name).Append(" entity)\r\n");
                sb.Append("        {\r\n");
                sb.Append("            if (Request.IsAjaxRequest())\r\n");
                sb.Append("            {\r\n");
                sb.Append("                StateCode code = ServiceIoc.Get<").Append(class_name).Append("Service>().Save(user.id, entity);\r\n");
                sb.Append("                return Json(GetResult(code), JsonRequestBehavior.AllowGet);\r\n");
                sb.Append("            }\r\n");
                sb.Append("            else\r\n");
                sb.Append("            {\r\n");
                sb.Append("                entity = ServiceIoc.Get<").Append(class_name).Append("Service>().GetById(bid);\r\n");
                sb.Append("                if(entity != null)\r\n");
                sb.Append("                {\r\n");
                sb.Append("                    ViewBag.entity = JsonConvert.SerializeObject(entity);\r\n");
                sb.Append("                }\r\n");
                sb.Append("            }\r\n\r\n");
                sb.Append("            return View();\r\n");
                sb.Append("        }\r\n\r\n");


                sb.Append("        /// <summary>\r\n");
                sb.Append("        /// ").Append(remark).Append("查询").Append("\r\n");
                sb.Append("        /// <param name=\"pageSize\"></param>\r\n");
                sb.Append("        /// <param name=\"pageIndex\"></param>\r\n");
                sb.Append("        /// <param name=\"keyword\"></param>\r\n");
                sb.Append("        /// </summary>\r\n");
                sb.Append("        public ContentResult Get").Append(class_name).Append("s(int pageSize, int pageIndex, string keyword)\r\n");
                sb.Append("        {\r\n");
                sb.Append("            try\r\n");
                sb.Append("            {\r\n");
                sb.Append("                //查询对象\r\n");
                sb.Append("                Criteria ct = new Criteria();\r\n\r\n");
                sb.Append("                //查询表达式\r\n");
                sb.Append("                MutilExpression me = new MutilExpression();\r\n\r\n");
                sb.Append("                ct.SetFromTables(\"").Append(tableInfo.name).Append("\")\r\n");
                sb.Append("                .SetPageSize(pageSize)\r\n");
                sb.Append("                .SetStartPage(pageIndex)\r\n");
                sb.Append("                .SetFields(new string[] { \"*\" })\r\n");
                sb.Append("                .AddOrderBy(new OrderBy(\"id\", \"desc\"));\r\n\r\n");
                sb.Append("                //查询关键词\r\n");
                sb.Append("                if (!string.IsNullOrEmpty(keyword))\r\n");
                sb.Append("                {\r\n");
                sb.Append("                    me.Add(new SingleExpression(\"keyword\", LogicOper.LIKE, keyword));\r\n");
                sb.Append("                }\r\n\r\n");
                sb.Append("                //设置查询条件\r\n");
                sb.Append("                if (me.Expressions.Count > 0)\r\n");
                sb.Append("                {\r\n");
                sb.Append("                    ct.SetWhereExpression(me);\r\n");
                sb.Append("                }\r\n\r\n");
                sb.Append("                DataTable ").Append("data = ServiceIoc.Get<").Append(class_name).Append("Service>().Fill(ct);\r\n");
                //sb.Append("                List<").Append(class_name).Append("> data = ServiceIoc.Get<").Append(class_name).Append("Service>().GetList(ct);\r\n");
                sb.Append("                return PageResult(StateCode.State_200, ct.TotalRow, data);\r\n");
                sb.Append("            }\r\n");
                sb.Append("            catch (Exception ex)\r\n");
                sb.Append("            {\r\n");
                sb.Append("                return PageResult(StateCode.State_500, 0, null);\r\n");
                sb.Append("            }\r\n");
                sb.Append("        }\r\n\r\n");



                sb.Append("    }\r\n");
                sb.Append("}\r\n\r\n");

                return sb.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion


        #region 操作扩展


        /// <summary>
        /// 获取字段类型
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string GetDataType(string name)
        {
            string ctype = "";
            switch (name.ToLower())
            {
                case "int":
                    ctype = "int";
                    break;
                case "bigint":
                    ctype = "long";
                    break;

                case "text":
                case "varchar":
                case "char":
                case "nvarchar":
                case "nchar":
                    ctype = "string";
                    break;
                case "decimal":
                    ctype = "decimal";
                    break;
                case "datetime":
                    ctype = "DateTime";
                    break;
                case "float":
                    ctype = "double";
                    break;
                case "bit":
                    ctype = "bool";
                    break;
            }

            return ctype;
        }


        /// <summary>
        /// 处理备注包含回车换行
        /// </summary>
        /// <param name="remark"></param>
        /// <returns></returns>
        private static string GetRemark(string remark)
        {
            //返回空字符串
            if (string.IsNullOrEmpty(remark)) return string.Empty;
            return remark.Replace("\r\n", "\r\n&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;///").ToString();
        }



        #endregion


    }
}