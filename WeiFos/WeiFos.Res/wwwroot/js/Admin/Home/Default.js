/*!
 * 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
 * Copyright (c) 2013-2018 深圳微狐信息技术有限公司
 * 创 建：叶委
 * 日 期：2018-12-05 11:53:51
 * 描 述：订单管理页脚本
 */
var datagrid, report;

let reminds = [
    {
        title: "小提示",
        msg: "双击列表数据行可以快速进入编辑页面",
    },
    {
        title: "小提示",
        msg: "搜索框输入完可以直接按回车搜索",
    },
    {
        title: "",
        msg: "",
    }];

$(function () {

    var t_num = App_G.Util.getRandomNum(0, 1);

    //初始化消息提醒
    $("#msg_div").html(template("template_msg", { data: [reminds[t_num]] }));
    setTimeout(function () {
        $("#msg_div").fadeOut(2000);
    }, 3000);

    //加载数据
    $getByAsync("/Home/GetDefaultStatis", "", function (result) {
        if (result.Code == App_G.Code.Code_200) {

            //统计数据
            entity = result.Data.statis;

            //报表
            report = result.Data.report;

            //绑定数据
            App_G.Mapping.Bind("data-val", entity[0]);

            let r = {
                //订单总额同比上月增长
                this_month_amountr: 0,
                //折扣总金额同比上月增长
                this_month_disamountr: 0,
                //优惠卷总金额同比上月增长
                this_month_couamountr: 0,
                //运费总金额同比上月增长
                this_month_famountr: 0
            };

            if (entity.last_month_amount > 0) {
                r.this_month_amountr = (parseFloat(entity.this_month_amount - entity.last_month_amount)) / entity.last_month_amount;
                r.this_month_amountr = Math.floor(r.this_month_amountr * 100) / 100;
                if (r.this_month_amountr <= 0) {
                    $("[data-valr=this_month_amountr]").prev().addClass("fa-caret-down").removeClass("fa-caret-up");
                    $("[data-valr=this_month_amountr]").parent().addClass("text-success").removeClass("text-danger");
                } else {
                    $("[data-valr=this_month_amountr]").prev().addClass("fa-caret-up").removeClass("fa-caret-down");
                    $("[data-valr=this_month_amountr]").parent().addClass("text-danger").removeClass("text-success");
                }
            }
            if (entity.last_month_disamount > 0) {
                r.this_month_disamountr = (parseFloat(entity.this_month_disamount - entity.last_month_disamount)) / entity.last_month_disamount;
                r.this_month_disamountr = Math.floor(r.this_month_disamountr * 100) / 100;
                if (r.this_month_disamountr <= 0) {
                    $("[data-valr=this_month_disamountr]").prev().addClass("fa-caret-down").removeClass("fa-caret-up");
                    $("[data-valr=this_month_disamountr]").parent().addClass("text-success").removeClass("text-danger");
                } else {
                    $("[data-valr=this_month_disamountr]").prev().addClass("fa-caret-up").removeClass("fa-caret-down");
                    $("[data-valr=this_month_disamountr]").parent().addClass("text-danger").removeClass("text-success");
                }
            }
            if (entity.last_month_couamount > 0) {
                r.this_month_couamountr = (parseFloat(entity.this_month_couamount - entity.last_month_couamount)) / entity.last_month_couamount;
                r.this_month_couamountr = Math.floor(r.this_month_couamountr * 100) / 100;
                if (r.this_month_couamountr <= 0) {
                    $("[data-valrthis_month_couamountr]").prev().addClass("fa-caret-down").removeClass("fa-caret-up");
                    $("[data-valr=this_month_couamountr]").parent().addClass("text-success").removeClass("text-danger");
                } else {
                    $("[data-valr=this_month_couamountr]").prev().addClass("fa-caret-up").removeClass("fa-caret-down");
                    $("[data-valr=this_month_couamountr]").parent().addClass("text-danger").removeClass("text-success");
                }
            }
            if (entity.last_month_famount > 0) {
                r.this_month_famountr = (parseFloat(entity.this_month_famount - entity.last_month_famount)) / entity.last_month_famount;
                r.this_month_famountr = Math.floor(r.this_month_famountr * 100) / 100;
                if (r.this_month_famountr <= 0) {
                    $("[data-valr=this_month_famountr]").prev().addClass("fa-caret-down").removeClass("fa-caret-up");
                    $("[data-valr=this_month_famountr]").parent().addClass("text-success").removeClass("text-danger");
                } else {
                    $("[data-valr=this_month_famountr]").prev().addClass("fa-caret-up").removeClass("fa-caret-down");
                    $("[data-valr=this_month_famountr]").parent().addClass("text-danger").removeClass("text-success");
                }
            }

            //绑定数据
            App_G.Mapping.Bind("data-valr", r);

            //设置进度条
            if (entity.order_count > 0) {
                $("[data-wait_pay_count]").parent().next().find("div.progress-bar").css("width", parseFloat(entity.wait_pay_count) / parseFloat(entity.order_count));
                $("[data-wait_delivery_count]").parent().next().find("div.progress-bar").css("width", parseFloat(entity.wait_delivery_count) / parseFloat(entity.order_count));
                $("[data-cancel_order_count]").parent().next().find("div.progress-bar").css("width", parseFloat(entity.cancel_order_count) / parseFloat(entity.order_count));
                $("[data-new_user_count]").parent().next().find("div.progress-bar").css("width", parseFloat(entity.new_user_count) / parseFloat(entity.order_count));
            }
        }
        else {
            layer.msg(result.Message, { icon: 2 });
        }
    });

    //表格初始化
    datagrid = $("[name=datagrid]").datagrid({
        url: "/OrderModule/Order/GetDefaultOrders",
        data: { keyword: "", status: 0, date: "" },
        column: [
            {
                text: "订单编号",
                style: "width:100px;"
            },
            {
                text: "订单状态",
                style: "width:100px;"
            },
            {
                text: "实付款(元)",
                style: "width:150px;"
            },
            {
                text: "创建时间",
                style: "width:100px;"
            },
            {
                text: "订单备注",
                style: "width:200px;"
            }
        ],
        dblclick: function (tr) {
            window.location.href = '/OrderModule/Order/ProductOrderForm?bid=' + tr.attr("data-id");
        }
    });

    //统计报表
    Highcharts.chart('salesChart', {
        chart: {
            type: 'areaspline'
        },
        title: {
            text: ''
        },
        legend: {
            layout: 'vertical',
            align: 'left',
            verticalAlign: 'top',
            x: 150,
            y: 100,
            floating: true,
            borderWidth: 1,
            backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'
        },
        xAxis: {
            categories: report.map((ele, i) => { return ele.weeknum })
            //plotBands: [{ //设置区域模块背景色
            //    from: 4.5,
            //    to: 6.5,
            //    color: 'rgba(68, 170, 213, .2)'
            //}]
        },
        yAxis: {
            title: {
                text: '订单数量'
            }
        },
        tooltip: {
            shared: true,
            valueSuffix: ' 单'
        },
        credits: {
            enabled: false
        },
        plotOptions: {
            areaspline: {
                fillOpacity: 0.5
            }
        },
        series: [{
            name: '已成交',
            data: report.map((ele, i) => { return ele.done })
        }, {
            name: '总单数',
            data: report.map((ele, i) => { return ele.count })
        }]
    });

});



//获取订单状态
template.defaults.imports.GetOrderStatus = function (state) {

    switch (state) {

        case -1:
            return "交易关闭";

        case 1:
            return "等待买家付款";

        case 3:
            return "买家已付款";

        case 4:
            return "买家退款";

        case 10:
            return "卖家已发货";

        case 11:
            return "买家退货";

        case 18:
            return "交易成功";

        default:
            return "";
    }
}