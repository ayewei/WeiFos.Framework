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
        url: "/SKUModule/SKU/GetProductTypes",
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
                text: "备注信息",
                style: "width: 140px; "
            }
        ],
        dblclick: function (tr) {
            window.location.href = "/SKUModule/SKU/ProductTypeForm?bid=" + tr.attr("data-id");
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
        window.location.href = "/SKUModule/SKU/ProductTypeForm";
    });

    //修改
    $("[name=edit_btn]").click(function () {
        var tr = datagrid.getSeleteTr();
        if (tr.length == 0) {
            layer.msg('请选择操作的行', { icon: 2 });
            return false;
        }
        window.location.href = '/SKUModule/SKU/ProductTypeForm?bid=' + datagrid.getSeleteTr().attr("data-id");
    });

    //删除
    $("div.dataTables_filter").digbox({
        Selector: "[name=delete_btn]",
        Title: "提示信息",
        Context: "确定删除该数据吗？",
        Before: function () {
            var tr = datagrid.getSeleteTr();
            if (tr.length == 0) {
                layer.msg('请选择操作的行', { icon: 2 });
                return false;
            }
            return true;
        },
        CallBack: function (s, c, p) {
            $post("/SKUModule/SKU/DeleteTypeName?bid=" + datagrid.getSeleteTr().attr("data-id"), "",
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
 



 