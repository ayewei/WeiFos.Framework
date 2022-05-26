
function getSearchData() {
    return { keyword: $("#keyword").val(), type: $("#type").val(), createDate: $("#createDate").val() };
}

$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

    //日历控件
    $("#createDate").daterangepicker({
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

    //清空日期
    $("#clear_date_btn").click(function () {
        $("#createDate").val("");
    });

    //表格初始化
   var datagrid = $("[name=datagrid]").datagrid({
        url: "/SystemModule/System/GetSystemLogs",
        data: getSearchData(),
        pager: {
            index: 0,
            pageSize: [15, 30, 50]
        },
        template_id: "template",
        column: [
            {
                text: "操作人",
                style: "width:200px;"
            },
            {
                text: "操作时间",
                style: "width:200px;"
            },
            {
                text: "操作内容",
                style: "width:80px;"
            }
        ],
        //行双击事件
        dblclick: function (tr) { }
    });

    //刷新
    $("[name=refresh_btn]").click(function () {
        window.location.reload();
    });

    //查询
    $("[name=search_btn]").click(function () {
        datagrid.execute(getSearchData());
    });

    //清空当前日志
    $("div.dataTables_filter").digbox({
        Selector: "a[name=clear_btn]",
        Title: "提示信息",
        Context: "确定清空当前日志吗？",
        CallBack: function (s, c, p) {
            $post("/SystemModule/System/ClearSystemLogs", "",
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





















