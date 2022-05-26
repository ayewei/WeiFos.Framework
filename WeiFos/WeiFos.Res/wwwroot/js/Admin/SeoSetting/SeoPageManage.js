
/*****获取查询数据******/
function getSearchData() {
    return { name: $("#Name").val(), createdDate: $.trim($("#CreatedDate").val()) };
}

$(function () {

    $("#clear_a").click(function () {
        $("#CreatedDate").val("");
    });

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


    var pager = $("#PagerDiv").pager({
        url: '/SeoSetting/GetSeoPages?rnd=' + Math.random(),
        data: getSearchData(),
        currentPage: 0,
        pageSize: 15,
        callback: function (data) {
            if ($("#listTable tbody").find("tr").length > 0) {
                $("#listTable tbody tr").remove();
            }
            $("#listTable tbody").append(template("template", data));
        }
    });


    //删除
    $("[name=delete_btn]").digbox({
        Selector: "#listTable",
        Title: "信息提示框",
        Context: "删除以后不可恢复，确定彻底删除吗？",
        CallBack: function (s, c, p) {

            $get("/SeoSetting/DeleteSeoPage?bid=" + c.parent().attr("data-id"), "",
              function (data) {
                  if (data.Code == App_G.Code.Code_200) {
                      App_G.MsgBox.success_digbox();
                      pager.execute(getSearchData());
                  } else {
                      App_G.MsgBox.error_digbox();
                  }
              });
        }
    });
     

    //查询事件
    $("#search_btn").click(function () {
        pager.execute(getSearchData());
    });


});