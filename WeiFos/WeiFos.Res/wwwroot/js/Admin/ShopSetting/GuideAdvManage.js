


function GetSearchData() {
    return { advName: $("#adv_name").val(), guideCategoryID: $("#guideCategoryID").val() };
}

$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

    var url = App_G.Util.getDomain() + '/ShopSetting/GetGuideAdvs';
    var pager = $("#PagerDiv").pager({
        url: url,
        data: GetSearchData(),
        currentPage: 0,
        pageSize: 7,
        callback: function (data) {
            if ($("#listTable tbody").find("tr").length > 0) {
                $("#listTable tbody tr").remove();
            }
            $("#listTable tbody").append(template("template", data));
        }
    });


    $("#search_btn").click(function () {
        pager.execute(GetSearchData());
    });


    //删除
    $("[name=delete_btn]").digbox({
        Selector: "#tbd_ategory",
        Title: "信息提示框",
        Context: "删除以后不可恢复，确定删除吗？",
        CallBack: function (submit_btnid, current, panel) {

            $get("/ShopSetting/DeleteAdvertise?bid=" + current.parent().attr("data-id"), "",

              function (backdata) {
                  var data = $.parseJSON(backdata)
                  if (data.Code == App_G.Code.Code_200) {
                      App_G.MsgBox.success_digbox();
                      current.parent().parent().remove();
                  } else {
                      App_G.MsgBox.error_digbox();
                  }

              });

        }
    });

    //显示
    $("[name=show]").digbox({
        Title: "提示信息",
        Selector: "#tbd_ategory",
        Context: "确定显示该分类吗？",
        CallBack: function (submit_btnid, current_obj, panel) {

            $get("/ShopSetting/SetEnableAdImg?id=" + current_obj.parent().attr("data-id") + "&isshow=true", "",

              function (data) {
                  if (data.Code == App_G.Code.Code_200) {
                      App_G.MsgBox.success_digbox();
                      current_obj.parent().parent().find("td:eq(3)").find("span").attr("class", "label label-success").text("显示");
                      current_obj.parent().parent().find("td:eq(5)").find("a[name=show]").text("").children().remove();
                      current_obj.parent().parent().find("td:eq(5)").find("a[name=show]").attr("name", "hide").append("<i class='icon-eye-close'></i>隐藏");
                  } else {
                      App_G.MsgBox.error_digbox();
                  }

              });

        }
    });


    //隐藏
    $("[name=hide]").digbox({
        Title: "提示信息",
        Selector: "#tbd_ategory",
        Context: "确定隐藏该分类吗？",
        CallBack: function (btn_id, current_obj, panel) {

            $get("/ShopSetting/SetEnableAdImg?id=" + current_obj.parent().attr("data-id") + "&isshow=false", "",
              function (data) {
                  if (data.Code == App_G.Code.Code_200) {
                      App_G.MsgBox.success_digbox();
                      current_obj.parent().parent().find("td:eq(3)").find("span").attr("class", "label label-warning").text("隐藏");
                      current_obj.parent().parent().find("td:eq(5)").find("a[name=hide]").text("").children().remove();
                      current_obj.parent().parent().find("td:eq(5)").find("a[name=hide]").attr("name", "show").append("<i class='icon-eye-open'></i>显示");
                  } else {
                      App_G.MsgBox.error_digbox();
                  }
              });

        }
    });

});