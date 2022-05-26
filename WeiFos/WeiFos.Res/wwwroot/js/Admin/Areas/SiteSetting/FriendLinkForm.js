
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
                partner: App_G.Mapping.Get("data-val", {
                    source_type: 1,
                    id: App_G.Util.getRequestId('bid')

                })
            };

            $postByAsync("/SiteSetting/SavePartner", JSON.stringify(data),
                 function (result) {
                     if (result.Code == App_G.Code.Code_200) {
                         App_G.MsgBox.success_digbox();
                         setTimeout("window.location.href='FriendLinkManage';", 1000);
                     } else {
                         App_G.MsgBox.error_digbox("操作失败！");
                     }
                 });
        }
    });

    var v = yw.valid.getValidate();
    v.valid("#name", { focusmsg: "请输入合作伙伴名称", errormsg: "", vtype: verifyType.anyCharacter });
    v.valid("#order_index", { focusmsg: "请输入排序索引", errormsg: "索引只能够是正整数", vtype: verifyType.isLGZeroNumber });
    v.valid("#link_url", { focusmsg: "请输入链接地址", errormsg: "", vtype: verifyType.isUrl, selectvalidate: true });

});


