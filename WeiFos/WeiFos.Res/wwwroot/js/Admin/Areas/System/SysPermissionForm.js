
var entity;

$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

    //页面数据绑定
    entity = App_G.Mapping.Load("#entity");

    //初始化验证插件
    yw.valid.config({
        submiteles: "#SaveBtn",
        //初始化
        data: [
            {
                attr: "data-val",
                data: entity
            }
        ],//验证成功
        vsuccess: function () {

            $("#SaveBtn").setDisable({ text: "保存中...", time: 2000 });

            if (validateSubmit()) {
                var data = {
                    entity: App_G.Mapping.Get("data-val", { id: App_G.Util.getRequestId('bid') })
                };

                $post("/SystemModule/System/SysPermissionForm", JSON.stringify(data), function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        layer.msg(result.Message, { icon: 1 });
                        var url = App_G.Util.getDomain() + "/SystemModule/System/SysPermissionManage";
                        setTimeout("window.location.href ='" + url + "'", 1000);
                    } else {
                        layer.msg(result.Message, { icon: 2 });
                    }
                });
            }

        }
    });

    var v = yw.valid.getValidate({});

    //权限名称
    v.valid("[data-val=name]", {
        vtype: verifyType.isNumberlatterCcter,
        focus: { msg: "请输入权限名称，只能包含汉字、字母、数字" },
        blur: { msg: "请输入权限名称，只能包含汉字、字母、数字" }
    });

    //权限编号 
    v.valid("#code", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "输入权限名称，只能包含汉字、字母、数字" },
        blur: { msg: "权限名称只能包含汉字、字母、数字" },
        othermsg: "该权限名称已经存在",
        repeatvld: {
            url: App_G.Util.getDomain() + "/SystemModule/System/ExistPermissionCode?bid=" + App_G.Util.getRequestId("bid")
        }
    });

    //链接地址
    v.valid("[data-val=action_url]", {
        vtype: verifyType.normal,
        focus: { msg: "请输入action地址" },
        blur: { msg: "请输入action地址" }
    });

    //权限说明
    v.valid("[data-val=remarks]", {
        vtype: verifyType.normal,
        focus: { msg: "请输入备注说明" },
        blur: { msg: "请输入备注说明" }
    });
 
    //排序索引
    v.valid("[data-val=order_index]", {
        vtype: verifyType.isNumber,
        focus: { msg: "请输入排序索引，只能够是数字" },
        blur: { msg: "请输入排序索引，只能够是数字" }
    });

    //页面数据绑定
    if ($.trim($("#entity").val()).length > 0) {
        var menu = $.parseJSON($("#entity").val());
        App_G.Util.bindJson("data-val", menu);
    }
 
});


//设置选中
function setopt(opt) {
    $("#parent_id").find("option").not(":eq(0)").remove();
    $.each(opt, function (j, v) {
        if ($("#model").val() == $(v).attr("data-model")) {
            $("#parent_id").append($(v));
        }
    });
}


//验证提交
function validateSubmit() {
    var v = true;
    if ($("#parent_id").val() == App_G.Util.getRequestId('bid') && App_G.Util.getRequestId('bid') != 0) {
        App_G.MsgBox.error_digbox("不能将分类为自己");
        v = false;
    }
    return v;
}