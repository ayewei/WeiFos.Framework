
/*****获取查询数据******/
function getSearchData() {
    return { position: $("#Position").val(), name: $("#Name").val() };
}

$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

    var pager = $("#PagerDiv").pager({
        url: '/SiteSetting/GetPartners?source_type=1&rnd=' + Math.random(),
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
        Selector: "#dt_tbody",
        Title: "信息提示框",
        Context: "删除以后不可恢复，确定彻底删除吗？",
        CallBack: function (submit_btnid, current, panel) {

            var data = {
                ids: [ c.parent().attr("data-id") ]
            };

            $post("/SiteSetting/DeletePartner", JSON.stringify(data),
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