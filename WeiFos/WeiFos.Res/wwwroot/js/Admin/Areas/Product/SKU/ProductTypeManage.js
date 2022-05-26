$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

    //删除
    $("[name=deleteBtn]").digbox({
        Selector: "#listTable",
        Title: "提示信息",
        Context: "确定删除该数据吗？",
        CallBack: function (btn_id, current_obj, panel) {

            $post("/ProductModule/SKU/DeleteTypeName?bid=" + current_obj.parent().attr("data-id"), "",

              function (data) {
                  if (data.Code == App_G.Code.Code_200) {
                      App_G.MsgBox.success_digbox();
                      current_obj.parent().parent().remove();
                  } else if (data.Status == 503) {
                      App_G.MsgBox.error_digbox("该商品类型存在商品数据在使用");
                  } else {
                      App_G.MsgBox.error_digbox(data.Message);
                  }

              });
        }
    });

    var url = App_G.Util.getDomain() + '/ProductModule/SKU/GetProductTypes?rnd=' + Math.random();
    var pager = $("#PagerDiv").pager({
        url: url,
        data: { typeName: $("#typeName").val() },
        currentPage: 0,
        pageSize: 15,
        callback: function (data) {
            if ($("#listTable tbody").find("tr").length > 0) {
                $("#listTable tbody tr").remove();
            }
            $("#listTable tbody").append(template("productTypeTemplate", data));
        }
    });

    $("#search_btn").click(function () {
        pager.execute({ typeName: $("#typeName").val() });
    });

});