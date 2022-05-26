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

            $("#SaveBtn").setDisable({ text: "保 存 中 ..." });

            if (validateSubmit()) {
                var data = {
                    entity: App_G.Mapping.Get("data-val", { id: App_G.Util.getRequestId('bid') })
                };

                $post("/SystemModule/System/SysMenuForm", JSON.stringify(data), function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        layer.msg(result.Message, { icon: 1 });
                        var url = App_G.Util.getDomain() + "/SystemModule/System/SysMenuManage";
                        setTimeout("window.location.href ='" + url + "'", 1000);
                    } else {
                        layer.msg(result.Message, { icon: 2 });
                    }
                });
            }

        }
    });

    var v = yw.valid.getValidate({});

    //菜单名称
    v.valid("[data-val=name]", {
        vtype: verifyType.isNumberlatterCcter,
        focus: { msg: "请输入菜单名称，只能包含汉字、字母、数字" },
        blur: { msg: "请输入菜单名称，只能包含汉字、字母、数字" }
    });

    //菜单编号
    v.valid("#serial_no", {
        vtype: verifyType.isSerialNumber,
        focus: { msg: "菜单编号，只能包含字母、下划线、数字" },
        blur: { msg: "菜单编号，只能包含字母、下划线、数字" }
    });

    //链接地址
    v.valid("#action_url", {
        vtype: verifyType.normal,
        focus: { msg: "请输入链接地址" },
        blur: { msg: "请输入链接地址" }
    });

    //图标样式
    v.valid("#menu_class", {
        vtype: verifyType.normal,
        focus: { msg: "请输入图标样式" },
        blur: { msg: "请输入图标样式" }
    });

    //排序索引
    v.valid("#order_index", {
        vtype: verifyType.isNumber,
        focus: { msg: "请输入排序索引，只能够是数字" },
        blur: { msg: "请输入排序索引，只能够是数字" }
    });

    //页面数据绑定
    if ($.trim($("#entity").val()).length > 0) {
        var menu = $.parseJSON($("#entity").val());
        App_G.Util.bindJson("data-val", menu);
    }

    //var opt = $("#parent_id").find("option");
    //setopt(opt);
    ////模块选择
    //$("#model").change(function (i, o) {
    //    setopt(opt);
    //});

});


//验证提交
function validateSubmit() {
    var v = true;
    if ($("#ParentID").val() == App_G.Util.getRequestId('bid') && App_G.Util.getRequestId('bid') != 0) {
        App_G.MsgBox.error_digbox("不能将分类为自己");
        v = false;
    }
    return v;
}

//设置选中
function setopt(opt) {
    $("#ParentID").find("option").not(":eq(0)").remove();
    $.each(opt, function (j, v) {
        if ($("#model").val() == $(v).attr("data-model")) {
            $("#ParentID").append($(v));
        }
    });
}
