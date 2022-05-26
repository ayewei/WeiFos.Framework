
var entity = "";

$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

    //页面数据绑定
    if ($.trim($("#entity").val()).length > 0) {
        entity = $.parseJSON($("#entity").val());
        App_G.Util.bindJson("data-val", entity);
    }

    yw.valid.config({
        submitelements: "#SaveBtn", vsuccess: function () {
 
            $("#SaveBtn").setDisable();

            var data = {
                entity: App_G.Mapping.Get("data-val", {
                    id: App_G.Util.getRequestId('bid')
                })
            };

            $post("/SiteSetting/SaveCaseCgty", JSON.stringify(data),
            function (result) {
                if (result.Code == App_G.Code.Code_200) {
                    App_G.MsgBox.success_digbox();
                    setTimeout("window.location.href='CaseCgtyManage';", 1000);
                } else {
                    App_G.MsgBox.error_digbox("操作失败！");
                }
            });
        }
    });


    var v = yw.valid.getValidate();

    v.valid("#name", { focusmsg: "请输入名称", errormsg: "", vtype: verifyType.anyCharacter });

    v.valid("#english_name", { focusmsg: "请输入英文类别名称", errormsg: "", vtype: verifyType.anyCharacter, selectvalidate: true });

    v.valid("#prefix_no", { focusmsg: "请输入类别编号", errormsg: "", vtype: verifyType.isSerialNumber });

    v.valid("#introduction", { focusmsg: "请输入类别简介", errormsg: "", vtype: verifyType.anyCharacter, selectvalidate: true });

    v.valid("#order_index", { focusmsg: "请输入排序索引", errormsg: "索引只能够是正整数", vtype: verifyType.isLGZeroNumber }); 


});


