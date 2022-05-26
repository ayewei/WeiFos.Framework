/*!
 * 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
 * Copyright (c) 2013-2018 深圳微狐信息技术有限公司
 * 创 建：叶委
 * 日 期：2018-12-05 11:53:51
 * 描 述：退款单管理页脚本
 */
var datagrid, datagrid1, status = 0, v = null;

$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

    //日历控件
    $("[name=date]").daterangepicker({
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

    //报表订单
    datagrid = $("[name=datagrid]").datagrid({
        url: "/OrderModule/Order/GetProductOrderReports",
        data: getSearchData(),
        pager: {
            index: 0,
            pageSize: [20, 50]
        },
        column: [
            {
                text: "订单号",
                style: "width:100px;"
            },
            {
                text: "总金额",
                style: "width:100px;"
            },
            {
                text: "总成本(元)",
                style: "width:100px;"
            },
            {
                text: "总折扣",
                style: "width:100px;"
            },
            {
                text: "优惠卷金额",
                style: "width:100px;"
            },
            {
                text: "运费",
                style: "width:100px;"
            },
            {
                text: "实付金额",
                style: "width:100px;"
            },
            {
                text: "利润",
                style: "width:100px;"
            },
            {
                text: "提交时间",
                style: "width:100px;"
            }
        ],
        dblclick: function (tr) {
            window.location.href = '/OrderModule/Order/ProductOrderForm?bid=' + tr.attr("data-id");
        }
    });


    //表格初始化
    datagrid1 = $("[name=datagrid1]").datagrid({
        template_id: "details_template",
        url: "/OrderModule/Order/GetSaleDetailReports",
        data: getSearchData(),
        pager: {
            index: 0,
            pageSize: [10, 20, 50]
        },
        column: [
            {
                text: "订单号",
                style: "width:100px;"
            },
            {
                text: "商品名称",
                style: "width:100px;"
            },
            {
                text: "单价(元)",
                style: "width:8px;"
            },
            {
                text: "商品数量",
                style: "width:50px;"
            },
            {
                text: "小计金额",
                style: "width:50px;"
            },
            {
                text: "成本价",
                style: "width:50px;"
            },
            {
                text: "利润",
                style: "width:50px;"
            },
            {
                text: "创建时间",
                style: "width:50px;"
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

    //订单导出
    $("#export_index_btn").click(function () {
        var layer_box = layer.load(1, {
            shade: [0.3, '#000']
        });
        var form = yw.dynamicform.createForm({
            data: GetSearchData(),
            url: '/Order/ProductOrdersExportExcel',
            type: "post"
        })
        $(form).appendTo("body");
        $("#" + form.attr("id")).submit();

        setTimeout(function () {
            layer.close(layer_box);
        }, 2000)
    });

    //明细导出pagination
    $("#export_index_btn1").click(function () {
        var layer_box = layer.load(1, {
            shade: [0.3, '#000']
        });
        var form = yw.dynamicform.createForm({
            data: GetSearchData1(),
            url: '/Order/SaleDetailExportExcel',
            type: "post"
        })
        $(form).appendTo("body");
        $("#" + form.attr("id")).submit();

        setTimeout(function () {
            layer.close(layer_box);
        }, 2000)
    });


    totalOrder();
    //查询商品订单
    $("#search_btn").click(function () {
        pager.execute(GetSearchData());
        totalOrder();
    });



    totalOrder1();
    $("#search_btn1").click(function () {
        pager1.execute(GetSearchData1());
        totalOrder1();
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


//订单统计
function totalOrder() {
    //查询订单
    $post("/Order/GetTotalProductOrder?recm=" + Math.random(), JSON.stringify(GetSearchData()),
        function (result) {
            if (result.Code == App_G.Code.Code_200) {
                var entity = $.parseJSON(result.Data)[0];
                //页面绑定用户对象
                App_G.Mapping.Bind("data-val", entity);
            } else {
                layer.msg(result.Message, { icon: 2 });
            }
        });
}


//明细统计
function totalOrder1() {
    //查询订单
    $post("/Order/GetTotalSaleDetail?recm=" + Math.random(), JSON.stringify(GetSearchData1()),
        function (result) {
            if (result.Code == App_G.Code.Code_200) {
                var entity = $.parseJSON(result.Data)[0];
                //页面绑定用户对象
                App_G.Mapping.Bind("data-val1", entity);

            } else {
                layer.msg(result.Message, { icon: 2 });
            }
        });
}



//获取查询数据
function GetSearchData() {
    return { keyword: $("#keyword").val(), date: $("#Date").val() };
}


//获取查询数据
function GetSearchData1() {
    return { keyword: $("#keyword1").val(), date: $("#Date1").val() };
}

