
$(function () {

    //初始化验证插件
    yw.valid.config({
        submitelements: "#SaveBtn", vsuccess: function () {

            $("#SaveBtn").setDisable({ text: "保 存 中 ..." });

            if (validateSubmit()) {
                var data = { 
                    userinfo: App_G.Mapping.Get("data-val", { ID: App_G.Util.getRequestId('bid') }),
                    detail: App_G.Mapping.Get("data-val-detail", { UserID: App_G.Util.getRequestId('bid') })
                };

                $postByAsync("/User/SaveUser", JSON.stringify(data), function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        App_G.MsgBox.success_digbox();
                        var url = App_G.Util.getDomain() + "/User/UserManage";
                        setTimeout("window.location.href ='" + url + "'", 1000);
                    } else {
                        App_G.MsgBox.error_digbox("操作失败");
                    }
                });
            }

        }
    });

    var v = yw.valid.getValidate();

    //手机号
    v.valid("#Mobile", { focusmsg: "请输入手机号", errormsg: "手机号码格式错误", vtype: verifyType.isMoblie });

    //真实姓名
    v.valid("#RealName", { focusmsg: "请输入真实姓名(保理公司名称)", errormsg: "", vtype: verifyType.anyCharacter });

    //邮箱
    v.valid("#Email", { focusmsg: "请输入邮箱,选填", errormsg: "邮箱格式不正确", vtype: verifyType.isEmail, selectvalidate: true });

    //营业执照编号
    //v.valid("#BusiCode", { focusmsg: "请输入营业执照编号", errormsg: "营业执照号码格式不正确", vtype: verifyType.isBusinessLicense });

    //禁用
    $("[name=delete_btn]").digbox({
        Selector: "#listTable",
        Title: "提示信息",
        Context: "确定删除该会员吗？",
        CallBack: function (submit_btnid, current, panel) {
            $get("/User/Delete?bid=" + current.parent().attr("data-id") + "&isenable=false", "",
              function (data) {
                  if (data.Code == App_G.Code.Code_200) {
                      Initial();
                  } else {
                      App_G.MsgBox.error_digbox();
                  }
              });
        }
    });


    //页面数据绑定
    if ($.trim($("#entity").val()).length > 0) {
        var user = $.parseJSON($("#entity").val());
        App_G.Util.bindJson("data-val", user);
        $("#LoginName").attr("readonly", "readonly");
    } else {
        //用户名
        v.valid("#LoginName", { focusmsg: "请输入用户名，只能包含汉字、字母、数字", errormsg: "只能包含汉字、字母、数字", vtype: verifyType.isNumberlatterCcter });
    }
    if ($.trim($("#entitydetial").val()).length > 0) {
        var detail = $.parseJSON($("#entitydetial").val());
        App_G.Util.bindJson("data-val-detail", detail);
    }
  

});


//验证提交
function validateSubmit() {
    var v = true;
    if ($("#UsersType").val() == 0) {
        App_G.MsgBox.error_digbox("用户类型不能为空");
        v = false;
    }
    return v;
}

//设置选中
function setopt(opt) {
    $("#UsersType").find("option").not(":eq(0)").remove();
    $.each(opt, function (j, v) {
        if ($("#model").val() == $(v).attr("data-model")) {
            $("#UsersType").append($(v));
        }
    });
}
