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
            var data = App_G.Mapping.Get("data-val", { id: App_G.Util.getRequestId('bid') });
             
            $post("/CodeBuildModule/CodeBuild/SaveServerLink", JSON.stringify(data), function (result) {
                if (result.Code == App_G.Code.Code_200) {
                    layer.msg(result.Message, { icon: 1 });
                    var url = App_G.Util.getDomain() + "/CodeBuildModule/CodeBuild/ServerLinkManage";
                    setTimeout("window.location.href ='" + url + "'", 1000);
                } else {
                    layer.msg(result.Message, { icon: 2 });
                }
            });

        }
    });


    var v = yw.valid.getValidate({});

    //链接名称
    v.valid("[data-val=name]", {
        vtype: verifyType.isNumberlatterCcter,
        focus: { msg: "请输入链接名称，只能包含汉字、字母、数字" },
        blur: { msg: "请输入链接名称，只能包含汉字、字母、数字" }
    });

    //IP地址
    v.valid("[data-val=ip]", {
        vtype: verifyType.isIpAddress,
        focus: { msg: "请输入IP地址" },
        blur: { msg: "IP地址输入有误" }
    });

    //登录用户
    v.valid("[data-val=login_name]", {
        vtype: verifyType.isNumberlatterCcter,
        focus: { msg: "请输入登录用户，只能包含汉字、字母、数字" },
        blur: { msg: "请输入登录用户，只能包含汉字、字母、数字" }
    });

    //登录密码
    v.valid("[data-val=psw]", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入登录密码" },
        blur: { msg: "请输入登录密码" }
    });

    //页面数据绑定
    if ($.trim($("#entity").val()).length > 0) {
        var menu = $.parseJSON($("#entity").val());
        App_G.Mapping.Bind("data-val", menu);
    }

     
});
 
 