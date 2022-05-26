$(function () {

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
        ],//
        vsuccess: function () {

            $("#SaveBtn").setDisable({ text: "保 存 中..." });

            var data = {
                entity: App_G.Mapping.Get("data-val", { id: App_G.Util.getRequestId('bid') })
            };

            $post("/SystemModule/System/ConfigParamForm?recm=" + Math.random(), JSON.stringify(data),
                function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        layer.msg(result.Message, { icon: 1 });
                        var url = App_G.Util.getDomain() + "/SystemModule/System/ConfigParamManage";
                        setTimeout("window.location.href ='" + url + "'", 1000);
                    } else {
                        layer.msg(result.Message, { icon: 2 });
                    }
                });
        }
    });


    var v = yw.valid.getValidate();

    //参数key
    v.valid("#config_key", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入参数Key，只能包字母、数字、_-" },
        blur: { msg: "参数名称只能包字母、数字、_-" },
        repeatvld: {
            url: App_G.Util.getDomain() + "/SystemModule/System/ExistConfigParamKey?bid=" + App_G.Util.getRequestId("bid"),
            msg: "参数Key已经存在"
        }
    });

    //参数值
    v.valid("#config_value", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入参数值，只能包字母、数字、_-" },
        blur: { msg: "参数值只能包字母、数字、_-" }
    });

    //备注
    $("#remarks").maxlength({ MaxLength: 100 });
    v.valid("#remarks", {
        selectvld: true,
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入备注信息，选填" },
        blur: { msg: "" }
    });

});


