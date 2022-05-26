 

$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

    //表格初始化
    datagrid = $("[name=datagrid]").datagrid({
        url: "/SystemModule/System/GetSysPermissions",
        data: {},
        template_id: "template",
        //行双击事件
        dblclick: function (tr) {
            window.location.href = '/SystemModule/System/SysRoleForm?bid=' + tr.attr("data-id");
        }
    });

    //查询
    $("[name=search_btn]").click(function () {
        datagrid.execute(getSearchData());
    });

    //刷新
    $("[name=refresh_btn]").click(function () {
        window.location.reload();
    });

    //新增
    $("[name=add_btn]").click(function () {
        window.location.href = '/SystemModule/System/SysPermissionForm';
    });

    //删除权限
    $("a[name=delete_btn]").digbox({
        Selector: "a[name=delete_btn]",
        Context: "div[name=datagrid]",
        Title: "提示信息",
        Content: "删除该权限，对应的子权限也将删除，确认删除吗？",
        CallBack: function (b, c, p) {
            $get("/SystemModule/System/DeletePermission?bid=" + c.parent().attr("data-id"), "",
                function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        layer.msg(result.Message, { icon: 1 });
                        datagrid.execute({});
                    } else {
                        layer.msg(result.Message, { icon: 2 });
                    }
                });
        }
    });

});





 