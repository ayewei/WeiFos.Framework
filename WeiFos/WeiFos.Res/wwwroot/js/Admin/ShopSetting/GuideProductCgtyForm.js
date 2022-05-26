

$(function () {


    //初始化验证插件
    yw.valid.config({
        submitelements: "#SaveBtn", vsuccess: function () {
            $("#SaveBtn").setDisable({ text: "保 存 中..." });
            var data = {
                GuideProductCgty: App_G.Util.getJson("data-val", { id: App_G.Util.getRequestId('bid') })
            };

            $post("/ShopSetting/SaveGuideProductCgty", JSON.stringify(data),
                function (data) {
                    if (data.Code == App_G.Code.Code_200) {
                        App_G.MsgBox.success_digbox(); 
                        setTimeout("window.location.href ='/ShopSetting/GuideProductCgtyManage'", 1000);
                    } else {
                        App_G.MsgBox.error_digbox(data.Message);
                    }
                    $("#SaveBtn").val("保 存");
                    $("#SaveBtn").prop("disabled", false);
                });

        }
    });


    v = yw.valid.getValidate();

    //分类名称
    v.valid("#name", {
        tabindex: "#base_panel", focusmsg: "请输入分类名称", errormsg: "请输入商品名称", vtype: verifyType.anyCharacter
        //othermsg: "分类名称已经存在", repeatvalidate: { url: App_G.Util.getDomain() + "/ShopSetting/ExistCategoryName?bid=" + App_G.Util.getRequestId('bid') }
    });


    //类别编号
    v.valid("#serial_no", {
        tabindex: "#base_panel", focusmsg: "请输入用户编号,只能包含字母、数字、下划线", errormsg: "只能包含字母、数字、下划线", vtype: verifyType.isSerialNumber,
        othermsg: "用户编号已经存在", repeatvalidate: { url: App_G.Util.getDomain() + "/ShopSetting/ExistSerialNo?bid=" + App_G.Util.getRequestId('bid') }
    });

    //排序索引
    v.valid("#order_index", {
        tabindex: "#base_panel", focusmsg: "请输入排序索引，只能够是数字", errormsg: "排序索引只能够是数字", vtype: verifyType.isNumber
    });
     

    //页面数据绑定
    if ($.trim($("#entity").val()).length > 0) {
        var entity = $.parseJSON($("#entity").val());
        App_G.Util.bindJson("data-val", entity);
        $("#ParentID option[value='" + entity.id + "']").remove();
    }

});