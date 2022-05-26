var pager, status = 0, v;

$(function () {

    BindSearch();

    //日历控件
    $(".daterangepick").daterangepicker({
        language: 'zh-CN',
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

    //查询订单
    var url = App_G.Util.getDomain() + '/Order/GetOrderCourseDetails?rnd=' + Math.random();
    pager = $("#PagerDiv").pager({
        url: url,
        data: GetSearchData(),
        currentPage: App_G.Util.getRequestId('page'),
        pageSize: [10, 20],
        callback: function (data) {
            $("#listTable tbody").html(template("template", { data: data.data }));
        }
    });


    //条件查询
    $("#search_btn").click(function () {
        pager.execute(GetSearchData());
    });

    //清空
    $("#clear_a").click(function () {
        $("#Date").val("");
    });

    //立即报名
    $("[name='apply_btn']").digbox({
        Title: "立即报名",
        Context: template("form_template", { data: [{}] }),
        Show: function (s, c, p) {

            p.find("button.btn.btn-primary").attr("id", "edit_module_digbox_submit");

            App_G.Util.bindJson("data-val", {
                actual_amount: c.parent().attr("data-amount"),
                remarks: ""
            });

            if (v == null) {

                v = yw.valid.getValidate({
                    submiteles: "#edit_module_digbox_submit",
                    vsuccess: function () {

                        //订单数据
                        var data = {
                            order: App_G.Util.getJson("data-val", {
                                serial_no: c.parent().attr("data-no")
                            })
                        };

                        //更新订单金额
                        $post("/Order/UpdateAmount", JSON.stringify(data),
                            function (result) {
                                if (result.State == App_G.BackState.State_200) {
                                    App_G.MsgBox.success_digbox();
                                    pager.execute(GetSearchData());
                                }
                            });

                        //隐藏模态窗口
                        $("div.modal.fade.in").modal('hide');
                    }
                });

                //学员姓名
                v.valid("#actual_amount", {
                    vtype: validateType.isLGZeroPrice,
                    focus: { msg: "请输入学员姓名" },
                    blur: { msg: "请输入学员姓名" }
                });

                //手机号码
                v.valid("#actual_amount", {
                    vtype: validateType.isLGZeroPrice,
                    focus: { msg: "请输入手机号码" },
                    blur: { msg: "请输入手机号码" }
                });

            }
        },
        CallBack: function (s, c, p) {
            return false;
        }

    });

});


 
//绑定翻页查询
function BindSearch() {
    if (App_G.Util.getUrlParam('keyword') != null) {
        $("#keyword").val(App_G.Util.getUrlParam('keyword'));
    }
    if (App_G.Util.getUrlParam('date') != null) {
        $("#Date").val(App_G.Util.getUrlParam('date'));
    }
}


//获取查询数据
function GetSearchData() {
    return { is_used: $("#is_used").val(), keyword: $("#keyword").val(), type: 1, date: $("#Date").val() };
}


//获取时间
template.helper('GetTime', function (time) {
    let time1 = time.split(":")[0];
    let time2 = time.split(":")[1];
    if (time1.length == 1) {
        time1 = "0" + time1;
    }
    if (time2.length == 1) {
        time2 = "0" + time2;
    }
    return time1 + ":" + time2;
});


//获取订单状态
template.helper('GetOrderStatus', function (state) {

    switch (state) {

        case -1:
            return "交易关闭";

        case 1:
            return "等待买家付款";

        case 2:
            return "支付定金";

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

});