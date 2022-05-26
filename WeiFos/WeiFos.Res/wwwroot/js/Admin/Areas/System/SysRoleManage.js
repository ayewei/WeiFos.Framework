
function getSearchData() {
    return { name: $("#name").val(), status: $("#status").val() };
}

$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

    //表格初始化
    datagrid = $("[name=datagrid]").datagrid({
        url: "/SystemModule/System/GetSysRoles",
        data: getSearchData(),
        pager: {
            index: 0,
            pageSize: [10, 20, 50]
        },
        template_id: "template",
        column: [
            {
                text: "角色名称",
                style: "width:200px;"
            },
            {
                text: "是否可用",
                style: "width:80px;"
            },
            {
                text: "创建日期",
                style: "width:154px;"
            }
        ],
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
        window.location.href = '/SystemModule/System/SysRoleForm';
    });

    //修改
    $("[name=edit_btn]").click(function () {
        var tr = datagrid.getSeleteTr();
        if (tr.length == 0) {
            layer.msg('请选择操作的行', { icon: 2 });
            return false;
        }
        window.location.href = '/SystemModule/System/SysRoleForm?bid=' + datagrid.getSeleteTr().attr("data-id");
    });

    //启用
    $("[name=enable_btn]").digbox({
        Selector: "[name=enable_btn]",
        Title: "提示信息",
        Context: "div.dataTables_filter",
        Content: "确定启用该角色吗？",
        Before: function () {
            var tr = datagrid.getSeleteTr();
            if (tr.length == 0) {
                layer.msg('请选择操作的行', { icon: 2 });
                return false;
            }
            return true;
        },
        CallBack: function (s, c, p) {

            $post("/SystemModule/System/SetEnableSysRole?bid=" + datagrid.getSeleteTr().attr("data-id") + "&status=true", "",

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

    //禁用
    $("[name=disable_btn]").digbox({
        Selector: "[name=disable_btn]",
        Context: "div.dataTables_filter",
        Title: "提示信息",
        Content: "确定禁用该角色吗？",
        Before: function () {
            var tr = datagrid.getSeleteTr();
            if (tr.length == 0) {
                layer.msg('请选择操作的行', { icon: 2 });
                return false;
            }
            return true;
        },
        CallBack: function (s, c, p) {
            $post("/SystemModule/System/SetEnableSysRole?bid=" + datagrid.getSeleteTr().attr("data-id") + "&status=false", "",
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