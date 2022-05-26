
var entity, v1 = null, v2 = null;

$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

    //页面数据绑定
    entity = App_G.Mapping.Load("#entity");

    //初始化验证插件
    yw.valid.config({
        submiteles: "#SaveBtn",
        //初始化
        data: [
            {
                attr: "data-val",
                data: entity
            }
        ],//验证成功
        vsuccess: function () {

            $("#SaveBtn").setDisable({ text: "保 存 中 ..." });
            var data = App_G.Mapping.Get("data-val", { id: App_G.Util.getRequestId('bid') });

            //新增情况下
            if (App_G.Util.getRequestId('bid') == 0) {
                data.modules = getModules();
            }

            $post("/CodeBuildModule/CodeBuild/DoProjectSettingForm", JSON.stringify(data), function (result) {
                if (result.Code == App_G.Code.Code_200) {
                    layer.msg(result.Message, { icon: 1 });
                    var url = App_G.Util.getDomain() + "/CodeBuildModule/CodeBuild/ProjectSettingManage";
                    setTimeout("window.location.href ='" + url + "'", 1000);
                } else {
                    layer.msg(result.Message, { icon: 2 });
                }
            });

        }
    });


    var v = yw.valid.getValidate({});

    //项目名称
    v.valid("[data-val=name]", {
        vtype: verifyType.isNumberlatterCcter,
        focus: { msg: "请输入项目名称，只能包含汉字、字母、数字" },
        blur: { msg: "请输入项目名称，只能包含汉字、字母、数字" }
    });

    //项目英文名称
    v.valid("[data-val=en_name]", {
        vtype: verifyType.isNumberlatterCcter,
        focus: { msg: "项目英文名称,将作为命名空间第一级节点名称" },
        blur: { msg: "项目英文名称,将作为命名空间第一级节点名称" }
    });

    //页面数据绑定
    if ($.trim($("#entity").val()).length > 0) {
        var menu = $.parseJSON($("#entity").val());
        App_G.Util.bindJson("data-val", menu);
    }

    //表格初始化
    var url = "/CodeBuildModule/CodeBuild/GetProjectModules?id=" + App_G.Util.getRequestId('bid');
    var datagrid = $("[name=datagrid]").datagrid({
        url: url,
        data: getSearchData(),
        template_id: "template",
        column: [
            {
                text: "模块名称",
                style: "width:150px;"
            },
            {
                text: "英文名称(对应命名空间)",
                style: "width:300px;"
            },
            {
                text: "创建时间",
                style: "width:150px;"
            }
        ],
        //行双击事件
        dblclick: function (tr) {



        },
        callback: function (result) {
            if (result.Code == App_G.Code.Code_200) {
                trlist = result.Data;
            } else {
                layer.msg(result.Message, { icon: 2 });
            }
        }
    });

    //新增
    $("div.dataTables_filter").digbox({
        Selector: "[name=add_btn]",
        Title: "提示信息",
        Content: template("form_template", { data: [{}] }),
        Show: function (s, c, p) {

            p.find("button.btn.btn-primary").attr("id", "add_module_digbox_submit");

            if (v1 == null) {

                //console.log($(window.parent.document).attr("id"))
                v1 = yw.valid.getValidate({
                    selector: window.parent.parent.document,
                    submiteles: "#add_module_digbox_submit", vsuccess: function () {

                        try {

                            var entity = App_G.Mapping.Get("data-val-c", { project_setting_id: App_G.Util.getRequestId('bid') }, parent.document);
                            if (App_G.Util.getRequestId('bid') > 0) {

                                $post("/CodeBuildModule/CodeBuild/SaveProjectModule", JSON.stringify(entity),
                                    function (result) {
                                        if (result.Code == App_G.Code.Code_200) {
                                            layer.msg(result.Message, { icon: 1 });
                                            datagrid.execute(getSearchData());
                                        } else {
                                            layer.msg(result.Message, { icon: 2 });
                                        }
                                    });
                            } else {
                                data.entity.id = new Date().getTime();
                                data.entity.created_date = "";
                                $("div[name=datagrid] tbody").append(template("template", { data: [data.entity] }));
                            }

                            //是否在框架里面打开
                            if (self.frameElement && self.frameElement.tagName == "IFRAME") {
                                try { $("div.modal.fade.in", parent.document).modal('hide'); } catch{ }
                            } else {
                                $("div.modal.fade.in").modal('hide');
                            }
                        } catch{

                        }
                    }
                });

                //模块名称
                v1.valid("#module_name", {
                    selector: parent.document,
                    vtype: verifyType.isNumberlatterCcter,
                    focus: { msg: "只能包含汉字、字母、数字" },
                    blur: { msg: "只能包含汉字、字母、数字" }
                });

                //模块英文名称
                v1.valid("#module_en_name", {
                    selector: parent.document,
                    vtype: verifyType.isEnglishName,
                    focus: { msg: "作为命名空间第二级节点名称" },
                    blur: { msg: "作为命名空间第二级节点名称" }
                });

            }
        },
        CallBack: function (s, c, p) {
            return false;
        }
    });

    //修改
    $("[name=edit_btn]").click(function () {
        var tr = datagrid.getSeleteTr();
        if (tr.length == 0) {
            layer.msg('请选择操作的行', { icon: 2 });
            return false;
        }

        $("").digbox({
            IsAuto: true,
            Title: "提示信息",
            Content: template("form_template", { data: [{}] }),
            Show: function (s, c, p) {

                p.find("button.btn.btn-primary").attr("id", "edit_module_digbox_submit");

                //如果新增状态下
                if (App_G.Util.getRequestId('bid') == 0) {

                    App_G.Util.bindJson("data-val-c", {
                        name: $.trim($(o).find("td:eq(0)").text()),
                        en_name: $.trim($(o).find("td:eq(1)").text())
                    });

                } else {
                    $get("/CodeBuildModule/CodeBuild/GetProjectModule?id=" + tr.attr('data-id'), "",
                        function (result) {
                            if (result.Code == App_G.Code.Code_200) {
                                App_G.Mapping.Bind("data-val-c", result.Data, parent.document);
                            } else {
                                App_G.MsgBox.error_digbox("操作异常");
                            }
                        });
                }

                if (v2 == null) {

                    v2 = yw.valid.getValidate({
                        selector: window.parent.parent.document,
                        submiteles: "#edit_module_digbox_submit", vsuccess: function () {

                            try {
                                var entity = App_G.Mapping.Get("data-val-c", {
                                    id: tr.attr("data-id"),
                                    project_setting_id: App_G.Util.getRequestId('bid')
                                }, parent.document);
                            } catch{ }
                            if (App_G.Util.getRequestId('bid') > 0) {

                                $post("/CodeBuildModule/CodeBuild/SaveProjectModule", JSON.stringify(entity),
                                    function (result) {
                                        if (result.Code == App_G.Code.Code_200) {
                                            layer.msg(result.Message, { icon: 1 });
                                            datagrid.execute(getSearchData());
                                        } else {
                                            layer.msg(result.Message, { icon: 2 });
                                        }
                                    });

                            } else {
                                data.entity.id = new Date().getTime();
                                data.entity.created_date = "";
                                $("div[name=datagrid] tbody").append(template("template", { data: [data.entity] }));
                            }

                            //是否在框架里面打开
                            if (self.frameElement && self.frameElement.tagName == "IFRAME") {
                                try { $("div.modal.fade.in", parent.document).modal('hide'); } catch{ }
                            } else {
                                $("div.modal.fade.in").modal('hide');
                            }

                        }
                    });

                    //模块名称
                    v2.valid("#module_name", {
                        selector: parent.document,
                        vtype: verifyType.isNumberlatterCcter,
                        focus: { msg: "只能包含汉字、字母、数字" },
                        blur: { msg: "只能包含汉字、字母、数字" }
                    });

                    //模块英文名称
                    v2.valid("#module_en_name", {
                        selector: parent.document,
                        vtype: verifyType.isEnglishName,
                        focus: { msg: "作为命名空间第二级节点名称" },
                        blur: { msg: "作为命名空间第二级节点名称" }
                    });
                }

            },
            CallBack: function (s, c, p) {
                return true;
            }

        });
    });

    //删除
    $("div.dataTables_filter").digbox({
        Selector: "[name=delete_btn]",
        Title: "提示信息",
        Context: "确定删除该模块？",
        Before: function () {
            var tr = datagrid.getSeleteTr();
            if (tr.length == 0) {
                layer.msg('请选择操作的行', { icon: 2 });
                return false;
            }
            return true;
        },
        CallBack: function (s, c, p) {
            $get("/CodeBuildModule/CodeBuild/DelProjectModule?id=" + datagrid.getSeleteTr().attr("data-id"), "",
                function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        layer.msg(result.Message, { icon: 1 });
                        datagrid.execute(getSearchData());
                    } else {
                        layer.msg(result.Message, { icon: 2 });
                    }
                });
        }
    });

});


function getSearchData() {
    return { keyword: $("#keyword").val(), id: App_G.Util.getRequestId('bid') };
}

//获取模块信息
function getModules() {
    var data = [];
    $.each($("div[name=datagrid] > table > tbody > tr"), function (i, o) {
        var entity = {
            project_setting_id: App_G.Util.getRequestId('bid'),
            name: $.trim($(o).find("td:eq(0)").text()),
            en_name: $.trim($(o).find("td:eq(1)").text())
        }
        data.push(entity);
    });

    return data;
}