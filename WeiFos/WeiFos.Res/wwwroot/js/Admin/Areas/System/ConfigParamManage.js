
function getSearchData() {
    return { keyword: $("#keyword").val(), status: $("#status").val() };
}

$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

    //表格初始化
    datagrid = $("[name=datagrid]").datagrid({
        url: "/SystemModule/System/GetConfigParams",
        data: getSearchData(),
        pager: {
            index: 0,
            pageSize: [10, 20, 50]
        },
        template_id: "template",
        column: [
            {
                text: "key",
                style: "width:100px;"
            },
            {
                text: "value",
                style: "width:200px;"
            },
            {
                text: "是否生效",
                style: "width:50px;"
            },
            {
                text: "备注",
                style: "width:200px;"
            }
        ],
        dblclick: function (tr) {
            window.location.href = '/SystemModule/System/ConfigParamForm?bid=' + tr.attr("data-id");
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
        window.location.href = '/SystemModule/System/ConfigParamForm';
    });

    //修改
    $("[name=edit_btn]").click(function () {
        var tr = datagrid.getSeleteTr();
        if (tr.length == 0) {
            layer.msg('请选择操作的行', { icon: 2 });
            return false;
        }
        window.location.href = '/SystemModule/System/ConfigParamForm?bid=' + datagrid.getSeleteTr().attr("data-id");
    });

    //启用该参数
    $("div.dataTables_filter").digbox({
        Selector: "[name=enable_btn]",
        Title: "提示信息",
        Content: "确定启用该菜单吗？",
        Before: function () {
            var tr = datagrid.getSeleteTr();
            if (tr.length == 0) {
                layer.msg('请选择操作的行', { icon: 2 });
                return false;
            }
            return true;
        },
        CallBack: function (s, c, p) {

            $post("/SystemModule/System/SetConfigParamEnable?bid=" + datagrid.getSeleteTr().attr("data-id") + "&isenable=true", "",

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

    //禁用该参数
    $("div.dataTables_filter").digbox({
        Selector: "[name=disable_btn]",
        Title: "提示信息",
        Content: "确定禁用该菜单吗？",
        Before: function () {
            var tr = datagrid.getSeleteTr();
            if (tr.length == 0) {
                layer.msg('请选择操作的行', { icon: 2 });
                return false;
            }
            return true;
        },
        CallBack: function (s, c, p) {
            $post("/SystemModule/System/SetConfigParamEnable?bid=" + datagrid.getSeleteTr().attr("data-id") + "&isenable=false", "",
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


 