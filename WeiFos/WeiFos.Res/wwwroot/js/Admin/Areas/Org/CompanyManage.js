/*!
 * 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
 * Copyright (c) 2013-2018 深圳微狐信息技术有限公司
 * 创 建：
 * 日 期：2018-12-05 11:53:51
 * 描 述：公司管理页脚本
 */
var datagrid;

$(function () {

    App_G.Auth.InitID();

    //表格初始化
    datagrid = $("[name=datagrid]").datagrid({
        url: "/OrgModule/Org/GetCompanys",
        data: getSearchData(),
        pager: {
            index: 0,
            pageSize: [10, 20, 50]
        },
        template_id: "template",
        column: [ 
            {
                text: "显示排序",
                style: "width: 40px; "
            },
            {
                text: "公司全称",
                style: "width: 140px; "
            },
            {
                text: "部门简称",
                style: "width: 80px; "
            },
            {
                text: "英文名称",
                style: "width: 160px; "
            },
            {
                text: "负责人",
                style: "width: 80px; "
            },
            {
                text: "电话",
                style: "width: 100px; "
            },
            {
                text: "成立时间",
                style: "width: 120px; "
            }
        ],
        dblclick: function (tr) {
            window.location.href = "/OrgModule/Org/CompanyForm?bid=" + tr.attr("data-id");
        },
        callback: function (result) {
            if (result.Code == App_G.Code.Code_200) {
                trlist = result.Data.pageData;
            } else {
                layer.msg(result.Message, { icon: 2 });
            }
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
        window.location.href = "/OrgModule/Org/CompanyForm";
    });

    //修改
    $("[name=edit_btn]").click(function () {
        var tr = datagrid.getSeleteTr();
        if (tr.length == 0) {
            layer.msg('请选择操作的行', { icon: 2 });
            return false;
        }
        window.location.href = '/OrgModule/Org/CompanyForm?bid=' + datagrid.getSeleteTr().attr("data-id");
    });

    //删除
    $("[name=delete_btn]").digbox({
        Title: "提示信息",
        Content: "确定删除该数据吗？",
        Before: function () {
            var tr = datagrid.getSeleteTr();
            if (tr.length == 0) {
                layer.msg('请选择操作的行', { icon: 2 });
                return false;
            }
            return true;
        },
        CallBack: function (s, c, p) {
            $post("/OrgModule/Org/DelCompany?bid=" + datagrid.getSeleteTr().attr("data-id"), "",
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

//查询数据
function getSearchData() {
    return { keyword: $("#keyword").val() };
}

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