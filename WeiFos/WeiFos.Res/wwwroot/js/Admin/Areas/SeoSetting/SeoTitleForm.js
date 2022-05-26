
$(function () {

    //初始化验证插件
    yw.valid.config({
        submitelements: "#SaveBtn", vsuccess: function () {

            $("#SaveBtn").setDisable({ text: "保 存 中..." });

            var datas = {
                entity: App_G.Mapping.Get("data-val", { id: App_G.Util.getRequestId('bid'), type: 0 })
            };

            $post("/SeoSetting/SaveSeoKeyWord", JSON.stringify(datas),
            function (data) {
                if (data.Code == App_G.Code.Code_200) {
                    App_G.MsgBox.success_digbox();
                    var url = "/SeoSetting/SeoTitleManage";
                    setTimeout("window.location.href='" + url + "';", 1000);
                } else {
                    App_G.MsgBox.error_digbox("操作失败！");
                    $("#SaveBtn").val("保 存");
                    $("#SaveBtn").prop("disabled", false);
                }
            });
        }
    });

    v = yw.valid.getValidate();

    v.valid("#name", { focusmsg: "请输入标题", errormsg: "请输入标题", vtype: verifyType.anyCharacter });
    v.valid("#order_index", { focusmsg: "请输入排序索引", errormsg: "排序索引，只能是数字", vtype: verifyType.isNumber });
    $("#remarks").maxlength({ MaxLength: 300 });
    v.valid("#remarks", { focusmsg: "请输入备注", errormsg: "请输入备注", vtype: verifyType.anyCharacter, selectvalidate: true });

    if ($.trim($("#entity").val()).length > 0) {
        entity = $.parseJSON($("#entity").val());
        App_G.Util.bindJson("data-val", entity);
    }

});

