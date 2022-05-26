
/*****获取查询数据******/
function getSearchData() {
    return { name: $("#Name").val() };
}

$(function () {


    //初始化权限编号
    App_G.Auth.InitID();

    var pager = $("#PagerDiv").pager({
        url: '/SiteSetting/GetCaseCgtys?rnd=' + Math.random(),
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
        CallBack: function (submit_btnid, current, panel) {

            var data = {
                ids: [current.parent().attr("data-id")]
            };

            $postByAsync("/SiteSetting/DeleteCase", JSON.stringify(data),
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


    //禁用
    $("[name=disable]").digbox({
        Selector: "#listTable",
        Title: "提示信息",
        Context: "确定禁用该菜单吗？",
        CallBack: function (submit_btnid, current, panel) {
            $get("/SiteSetting/SetCaseCgtyEnable?bid=" + current.parent().attr("data-id") + "&isenable=false", "",
              function (data) {
                  if (data.Code == App_G.Code.Code_200) {
                      Initial();
                  } else {
                      App_G.MsgBox.error_digbox();
                  }
              });
        }
    });


    //启用
    $("[name=enable]").digbox({
        Selector: "#listTable",
        Title: "提示信息",
        Context: "确定启用该菜单吗？",
        CallBack: function (submit_btnid, current, panel) {
            $get("/SiteSetting/SetCaseCgtyEnable?bid=" + current.parent().attr("data-id") + "&isenable=true", "",
              function (data) {
                  if (data.Code == App_G.Code.Code_200) {
                      Initial();
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