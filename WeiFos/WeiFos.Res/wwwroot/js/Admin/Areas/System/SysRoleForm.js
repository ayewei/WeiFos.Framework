

//加载页面数据
$(function () {

    //页面数据绑定
    entity = App_G.Mapping.Load("#entity"); 
    if (entity != null) {
        var id_array = [];
        var ids = $("#permissionIds").val();
        if (ids.indexOf(',') != -1) {
            id_array = ids.split(',');
        } else {
            id_array.push(ids);
        }

        $.each(id_array, function (i, o) {
            $.each($("#role_permissions [type=checkbox]"), function (j, k) {
                if (o == $(k).val()) {
                    $(k).prop("checked", true);
                }
            });
        });
    }

    //处理权限
    $("#role_permissions [type=checkbox]").change(function () {
        checkChildrenBox($(this));
        checkParentBox($(this));
    });

    yw.valid.config({
        submiteles: "#SaveBtn",
        //初始化
        data: [
            {
                attr: "data-val",
                data: entity
            }
        ],
        vsuccess: function () {

            $("#SaveBtn").setDisable({ text: "保存中...", time: 2000 });

            var data = {
                entity: App_G.Mapping.Get("data-val", { id: App_G.Util.getRequestId('bid') }),
                pIds: getPermissions()
            }; 
             
            $post("/SystemModule/System/SysRoleForm", JSON.stringify(data),
                function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        layer.msg(result.Message, { icon: 1 });
                        var url = App_G.Util.getDomain() + "/SystemModule/System/SysRoleManage";
                        setTimeout("window.location.href ='" + url + "'", 1000);
                    } else {
                        layer.msg(result.Message, { icon: 2 });
                    }
                });

        }
    });


    var v = yw.valid.getValidate();

    //角色名称
    v.valid("#name", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "输入角色名称，只能包含汉字、字母、数字" },
        blur: { msg: "角色名称只能包含汉字、字母、数字" },
        othermsg: "该角色名称已经存在",
        repeatvld: {
            url: App_G.Util.getDomain() + "/SystemModule/System/ExistRoleName?bid=" + App_G.Util.getRequestId("bid")
        }
    });

    //备注
    $("#remarks").maxlength({ MaxLength: 100 });
    v.valid("#remarks", {
        selectvld: true,
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入备注信息，选填" },
        blur: { msg: "" }
    });

    //处理角色
    selectRoles();


});


//处理角色
function selectRoles() {
    $("input[name=MoveToRight]").click(function () {
        $("select[name=OtherRole]").append($("select[name=HaveRole] option:selected"));
    });

    $("input[name=MoveAllToRight]").click(function () {
        $("select[name=OtherRole]").append($("select[name=HaveRole] option"));
    });

    $("input[name=MoveToLeft]").click(function () {
        $("select[name=HaveRole]").append($("select[name=OtherRole] option:selected"));
    });

    $("input[name=MoveAllToLeft]").click(function () {
        $("select[name=HaveRole]").append($("select[name=OtherRole] option"));
    });
}

//父级选中
function checkChildrenBox(checkbox) {

    //当前权限checkbox选中状态
    var checked = checkbox.prop("checked");

    //当前权限
    var pId = parseInt(checkbox.val());

    var checkboxs = $("input[parentid=" + pId + "]");

    checkboxs.prop("checked", checked);

    $.each(checkboxs, function (i, o) {
        checkChildrenBox($(o));
    });
}

//子集选中
function checkParentBox(checkbox) {

    //当前权限checkbox选中状态
    var checked = checkbox.prop("checked");

    if (checked) {

        //上级ID
        var parentPId = parseInt(checkbox.attr("parentid"));

        var pcheckboxs = $("input[value=" + parentPId + "]").not("input[parentid=" + parentPId + "]");

        pcheckboxs.prop("checked", checked);

        $.each(pcheckboxs, function (i, o) {
            checkParentBox($(o));
        });
    }

}

//获权限取选中值
function getPermissions() {
    var params = "";
    var checkboxs = $("#role_permissions [type=checkbox]:checked");

    //遍历checkbox集合
    $.each(checkboxs, function (i, o) {
        if ($(o).prop("checked")) {
            if (checkboxs.length == i + 1) {
                params += $(o).val();
            } else {
                params += $(o).val() + ",";
            }
        }
    });
    return params;
}