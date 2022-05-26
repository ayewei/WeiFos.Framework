var trlist, datagrid;

function getSearchData() {
    return { name: $("[name=menu_name]").val() };
}

$(function () {

    //表格初始化
    datagrid = $("[name=datagrid]").datagrid({
        url: "/SystemModule/System/GetSysMenus",
        data: getSearchData(),
        template_id: "template",
        column: [
            {
                text: "菜单名称",
                style: "width:180px;"
            },
            {
                text: "菜单编号",
                style: "width:150px;"
            },
            {
                text: "链接地址",
                style: "width:300px;"
            },
            {
                text: "排序索引",
                style: "width:150px;"
            },
            {
                text: "是否启用"
            }
        ],
        //行双击事件
        dblclick: function (tr) {
            window.location.href = '/SystemModule/System/SysMenuForm?bid=' + tr.attr("data-id");
        },
        callback: function (result) {
            if (result.Code == App_G.Code.Code_200) {
                trlist = result.Data;
            } else {
                layer.msg(result.Message, { icon: 2 });
            }
        }
    });

    //查询
    $("[name=search_btn]").click(function () {
        datagrid.execute(getSearchData());
    });

    //绑定事件
    $("#dataTable .fa").click(function () {
        var a = $(this);
        //如果是展开
        if (a.hasClass("fa-minus-square-o")) {
            a.removeClass("fa-minus-square-o");
            a.addClass("fa-plus-square-o");
            hideTr(a.parent().parent().parent());
        } else {
            a.removeClass("fa-plus-square-o");
            a.addClass("fa-minus-square-o");
            showTr(a.parent().parent().parent());
        }
    });

    //刷新
    $("[name=refresh_btn]").click(function () {
        window.location.reload();
    });

    //新增
    $("[name=add_btn]").click(function () {
        window.location.href = '/SystemModule/System/SysMenuForm';
    });

    //修改
    $("[name=edit_btn]").click(function () {
        var tr = datagrid.getSeleteTr();
        if (tr.length == 0) {
            layer.msg('请选择操作的行', { icon: 2 });
            return false;
        }
        window.location.href = '/SystemModule/System/SysMenuForm?bid=' + datagrid.getSeleteTr().attr("data-id");
    });


    //启用
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

            $post("/SystemModule/System/SetSysMenuEnable?bid=" + datagrid.getSeleteTr().attr("data-id") + "&isenable=true", "",

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
            $post("/SystemModule/System/SetSysMenuEnable?bid=" + datagrid.getSeleteTr().attr("data-id") + "&isenable=false", "",
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

    //删除
    $("div.dataTables_filter").digbox({
        Selector: "[name=delete_btn]",
        Title: "提示信息",
        Context: "确定删除该菜单吗？",
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
                        App_G.MsgBox.success_digbox();
                        current.parent().parent().find("td:eq(2)").find("span").attr("class", "label label-success").text("启用");
                        current.parent().parent().find("td:eq(4)").find("a:eq(1)").hide();
                        current.parent().parent().find("td:eq(4)").find("a:eq(2)").show();
                    } else {
                        App_G.MsgBox.error_digbox();
                    }

                });

        }
    });

    //关闭全部
    $("#close_all").click(function () {
        $("tr[pid=tr_0]").find("i.fa").removeClass("fa fa-minus-square-o").addClass("fa fa-plus-square-o");
        $("table.table tbody").find("tr[pid!=tr_0]").hide();
    });

    //展开全部
    $("#open_all").click(function () {
        $("tr[pid=tr_0]").find("i.fa").removeClass("fa fa-plus-square-o").addClass("fa fa-minus-square-o");
        $("table.table tbody").find("tr[pid!=tr_0]").show();
    });


    //行内展开折叠
    $("table.table").find(".fa-minus-square-o.fa").click(function () {
        if ($(this).hasClass("fa-plus-square-o fa")) {
            $(this).attr("class", "fa-minus-square-o fa");
            showTr($(this).parent().parent().parent());
        } else {
            $(this).attr("class", "fa-plus-square-o fa");
            hideTr($(this).parent().parent().parent());
        }
    });

 

});


//显示Tr
function showTr(tr) {
    var trs = $("tr[pid=" + tr.attr("id") + "]");
    trs.show();

    $.each(trs, function (i, o) {
        showTr($(o));
    });
}

//隐藏Tr
function hideTr(tr) {
    var trs = $("tr[pid=" + tr.attr("id") + "]");
    trs.hide();

    $.each(trs, function (i, o) {
        hideTr($(o));
    });
}

//删除Tr
function deleteTr(tr) {
    var trs = $("tr[pid=" + tr.attr("id") + "]");
    trs.remove();

    $.each(trs, function (i, o) {
        deleteTr($(o));
    });
}

var index = 0;
//父类数量
function getParentCount(pid) {
    $.each(trlist, function (i, o) {
        if (pid == o.id) {
            index++;
            getParentCount(o.parent_id);
            return;
        }
    });
    return index;
}

template.defaults.imports.getTrIndex = function (pid) {
    if ("tr_" + pid == "tr_0") {
        return "";
    } else {
        index = 0;
        return getParentCount(pid, index);
    }
}; 