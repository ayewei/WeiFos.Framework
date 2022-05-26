/*!
 * 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
 * Copyright (c) 2013-2018 深圳微狐信息技术有限公司
 * 创 建：叶委
 * 日 期：2018-12-05 11:53:51
 * 描 述：退款单管理页脚本
 */
var datagrid, status = 0, v = null;

$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

    //日历控件
    $("#date").daterangepicker({
        language: 'zh-CN',
        showDropdowns: true,
        timePicker: true,
        locale: {
            applyLabel: '确定',
            cancelLabel: '取消',
            fromLabel: '起始时间',
            toLabel: '结束时间',
            customRangeLabel: '自定义',
            daysOfWeek: ['日', '一', '二', '三', '四', '五', '六'],
            monthNames: ['一月', '二月', '三月', '四月', '五月', '六月',
                '七月', '八月', '九月', '十月', '十一月', '十二月'],
            firstDay: 1
        },
        format: 'YYYY/MM/DD'
    });

    //表格初始化
    datagrid = $("[name=datagrid]").datagrid({
        url: "/OrderModule/Order/GetOrderRefunds",
        data: getSearchData(),
        pager: {
            index: 0,
            pageSize: [10, 20, 50]
        },
        column: [
            {
                text: "商品信息",
                style: "width:200px;"
            },
            {
                text: "联系人",
                style: "width:150px;"
            },
            {
                text: "实收款(元)",
                style: "width:150px;"
            },
            {
                text: "订单状态",
                style: "width:100px;"
            }
        ],
        dblclick: function (tr) {
            window.location.href = '/OrderModule/Order/ProductOrderForm?bid=' + tr.attr("data-id");
        }
    });

    //回车查询 
    $("#keyword").keypress(function (e) {
        var e = e || window.event;
        if (e.keyCode == 13) {
            datagrid.execute(getSearchData());
        }
    });

    //清空
    $("#clear_btn").click(function () {
        $("#date").val("");
    });

    //查询
    $("[name=search_btn]").click(function () {
        datagrid.execute(getSearchData());
    });

    //刷新
    $("[name=refresh_btn]").click(function () {
        window.location.reload();
    });

    //查看详情
    $("[name=view_btn]").click(function () {
        var tr = datagrid.getSeleteTr();
        if (tr.length == 0) {
            layer.msg('请选择操作的行', { icon: 2 });
            return false;
        }
        window.location.href = '/ProductModule/Product/BrandForm';
    });

    //确认退款
    $("[name=check_btn]").digbox({
        Title: "提示信息",
        Content: "确定审核通过该订单吗",
        Before: function () {
            var tr = datagrid.getSeleteTr();
            if (tr.length == 0) {
                layer.msg('请选择操作的行', { icon: 2 });
                return false;
            }
            return true;
        },
        CallBack: function (s, c, p) {

            var data = { id: c.attr("data-id") };
            $post("/ProductModule/Product/CloseOrder?bid=" + datagrid.getSeleteTr().attr("data-id"), "",
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
    return { keyword: $("#keyword").val(), status: status, date: $("#date").val() };
}

//绑定翻页查询
function BindSearch() {
    if (App_G.Util.getUrlParam('keyword') != null) {
        $("#keyword").val(App_G.Util.getUrlParam('serial_no'));
    }

    if (App_G.Util.getUrlParam('date') != null) {
        $("#date").val(App_G.Util.getUrlParam('date'));
    }
}

//获取查询数据
function GetSearchData() {
    return { keyword: $("#keyword").val(), status: $("#status").val(), date: $("#Date").val() };
}

//获取订单状态
template.defaults.imports.GetOrderStatus = function (state) {
    switch (state) {
        case 1:
            return "退款申请中";
        case 2:
            return "已拒绝";
        case 5:
            return "已通过";

        default:
            return "";
    }
};