$(function () {
    var p = "", c = "", a = "";
    $("#ConfirmBtn").digbox({
        Title: "信息提示框",
        Context: "确认此订单？",
        CallBack: function (btn_id, current_obj, panel) {
            var data = {
                address: App_G.Util.getJson("data-val", { order_id: App_G.Util.getRequestId("bid"), logistic_company: $("#logistic_company").find("option:selected").text() })
            };
            $post("/Order/SaveOrderSend?recm=" + Math.random(), JSON.stringify(data),
                function (result) {
                    if (result.State == App_G.BackState.State_200) {
                        $("#odes").text("已发货");
                        $("#done").show();
                        current_obj.hide();
                        $("#LogisticConfirmBtn").hide();
                    } else {
                        App_G.MsgBox.success_digbox(result.Message);
                        App_G.MsgBox.error_digbox();
                    }
                });
        }
    });

    $("#LogisticConfirmBtn").digbox({
        Title: "信息提示框",
        Context: "确认修改此订单的物流信息？",
        CallBack: function (btn_id, current_obj, panel) {
            var data = {
                address: App_G.Util.getJson("data-val", { order_id: App_G.Util.getRequestId("bid"), logistic_company: $("#logistic_company").find("option:selected").text() })
            };
            $post("/Order/SaveOrderLogistic?recm=" + Math.random(), JSON.stringify(data),
                function (result) {
                    if (result.State == App_G.BackState.State_200) {
                        App_G.MsgBox.success_digbox();
                        current_obj.hide();
                    } else {
                        App_G.MsgBox.success_digbox(result.Message);
                        App_G.MsgBox.error_digbox();
                    }
                });
        }
    });

    if ($.trim($("#pca").val()).length > 0) {
        var entity = $.parseJSON($("#pca").val());
        p = entity.province; c = entity.city; a = entity.area;
    }
    yw.cityarea({
        selector: "#area_div",
        attrtag: "data-val",
        attr: { province: p, city: c, area: a }
    });

    $("#lgCompany option:contains('" + $("#logistic_company").val() + "')").attr("selected", true);

    //返回
    $("#BackBtn").click(function () {
        window.location.href = "ProductOrderManage?" + window.location.href.split("?")[1];
    });

});


 