
var entity = "";

//身份证完整图
var img = "";

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
                entity: App_G.Mapping.Get("data-val", { id: App_G.Util.getRequestId('bid'), type: 0 })
            };

            $postByAsync("/SeoSetting/SaveSeoKeyWordCgty", JSON.stringify(data),
                 function (data) {
                     if (data.Code == App_G.Code.Code_200) {
                         App_G.MsgBox.success_digbox();
                         setTimeout("window.location.href='SeoTitleCgtyManage';", 1000);
                     } else {
                         App_G.MsgBox.error_digbox("操作失败！");
                     }
                 });
        }
    });

    var v = yw.valid.getValidate();
    v.valid("#name", { focusmsg: "请输入标签分类名称", errormsg: "名称长度在1~15字符", vtype: verifyType.anyCharacter });
    v.valid("#order_index", { focusmsg: "请输入排序索引", errormsg: "索引只能够是正整数", vtype: verifyType.isLGZeroNumber });
    v.valid("#remarks", { focusmsg: "请输入分类简介", errormsg: "", othermsg: "", vtype: verifyType.anyCharacter, selectvalidate: true });

});


