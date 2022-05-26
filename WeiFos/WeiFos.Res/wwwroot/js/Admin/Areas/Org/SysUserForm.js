var entity, p = "", c = "", a = "";

$(function () {

    //日历控件
    $("#birthday").daterangepicker({
        language: 'zh-CN',
        showDropdowns: true,
        singleDatePicker: true,
        timePicker: true,
        locale: {
            applyLabel: '确定',
            cancelLabel: '取消',
            fromLabel: '起始时间',
            toLabel: '结束时间',
            customRangeLabel: '自定义',
            daysOfWeek: ['日', '一', '二', '三', '四', '五', '六'],
            monthNames: ['一月', '二月', '三月', '四月', '五月', '六月',
                '七月', '八月', '九月', '十月', '十一月', '十二月'],
            firstDay: 1
        },
        format: 'YYYY/MM/DD'
    });

    //页面数据绑定
    entity = App_G.Mapping.Load("#entity");
    if (entity != null) {
        //日期格式化
        entity.birthday = App_G.Util.Date.ChangeDateFormat(entity.birthday);
        p = entity.province;
        c = entity.city;
        a = entity.county;
    }

    //处理权限
    $("#user_permission [type=checkbox]").change(function () {
        checkChildrenBox($(this));
        checkParentBox($(this));
    });

    //处理角色
    selectRoles();


    yw.cityarea({
        selector: "#area_div",
        attrtag: "data-val",
        attr: { province: p, city: c, county: a }
    });


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
                entity: App_G.Mapping.Get("data-val", { id: App_G.Util.getRequestId('bid') }),
                rIds: getUserRoles(),
                pIds: getPermissions()
            }; 

            $post("/OrgModule/Org/SysUserForm?recm=" + Math.random(), JSON.stringify(data),
                function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        layer.msg(result.Message, { icon: 1 });
                        var url = App_G.Util.getDomain() + "/OrgModule/Org/SysUserManage";
                        setTimeout("window.location.href ='" + url + "'", 1000);
                    } else {
                        layer.msg(result.Message, { icon: 2 });
                    }
                });
        }
    });


    var v = yw.valid.getValidate();

    //登录名
    v.valid("#login_name", {
        vtype: verifyType.isLoginName,
        focus: { msg: "请输入登录名" },
        blur: { msg: "登录名6至16位字母数字" },
        othermsg: "该登录名已经存在",
        repeatvld: {
            url: App_G.Util.getDomain() + "/OrgModule/Org/ExistLoginName"
        }
    });

    //用户姓名 
    v.valid("[data-val=user_name]", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入用户姓名" },
        blur: { msg: "输入用户姓名" }
    });

    //用户编号
    v.valid("#user_no", {
        vtype: verifyType.isSerialNumber, 
        focus: { msg: "只能包含字母、数字、下划线" },
        blur: { msg: "输入用户编号" },
        othermsg: "用户编号已经存在",
        repeatvld: {
            url: App_G.Util.getDomain() + "/OrgModule/Org/ExistUserNo?bid=" + App_G.Util.getRequestId('bid')
        }
    });

    //邮箱
    v.valid("[data-val=email]", {
        selectvld: true,
        vtype: verifyType.isEmail,
        focus: { msg: "请输入邮箱" },
        blur: { msg: "请输入邮箱" },
        othermsg: "该邮箱已被使用",
        repeatvld: {
            url: App_G.Util.getDomain() + "/OrgModule/Org/ExistEmail?bid=" + App_G.Util.getRequestId('bid')
        }
    });

    //QQ号码
    v.valid("[data-val=qq]", {
        selectvld: true,
        vtype: verifyType.isQQNumber,
        focus: { msg: "请输入QQ号码" },
        blur: { msg: "请输入QQ号码" }
    });

    //手机号码
    v.valid("[data-val=mobile]", {
        selectvld: true,
        vtype: verifyType.isMoblie,
        focus: { msg: "请输入手机号码" },
        blur: { msg: "请输入手机号码" }
    });

    //微信号
    v.valid("[data-val=wechat_no]", {
        selectvld: true,
        vtype: verifyType.isSerialNumber,
        focus: { msg: "请输入微信号" },
        blur: { msg: "请输入微信号" }
    });
     
    //出生年月
    v.valid("[data-val=birthday]", {
        selectvld: true,
        vtype: verifyType.isDate,
        focus: { msg: "请输入出生年月" },
        blur: { msg: "请输入出生年月" }
    });

    //所在地区
    v.valid("[data-val=area]", {
        selectvld: true,
        vtype: verifyType.isNumberlatterCcter,
        focus: { msg: "请输入所在地区" },
        blur: { msg: "请输入所在地区" }
    });

    //所在地区
    v.valid("[data-val=address]", {
        selectvld: true,
        vtype: verifyType.isNumberlatterCcter,
        focus: { msg: "请输入详细地址" },
        blur: { msg: "请输入详细地址" }
    });


    //页面数据绑定
    if ($.trim($("#entity").val()).length > 0) {

        var user_msg = {};

        //当前用户对象
        var user = $.parseJSON($("#sysuser_entity").val());

        if (user.account_type == 4) {
            user_msg = $.parseJSON($("#employee_entity").val());
        } else {
            user_msg = $.parseJSON($("#usermsg_entity").val());
        }

        //页面绑定用户对象
        App_G.Util.bindJson("data-user", user);

        //页面绑定用户信息对象
        App_G.Util.bindJson("data-user-msg", user_msg);

        //用户对应上限数
        $("#sup_count").val($("#sup_entity").val());

        $("#birthday").val(App_G.Util.Date.ChangeDateFormat(user_msg.birthday));

        var id_array = [];
        var ids = $("#permissionIds").val();
        if (ids.indexOf(',') != -1) {
            id_array = ids.split(',');
        } else {
            id_array.push(ids);
        }

        $.each(id_array, function (i, o) {
            $.each($("#user_permission [type=checkbox]"), function (j, k) {
                if (o == $(k).val()) {
                    $(k).prop("checked", true);
                }
            });
        });
    }

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

//处理权限
function selectPermission() {
    $("input[name=pIDs]").change(function () {
        checkChildrenBox($(this));

        checkParentBox($(this));
    });
}


//获权限取选中值
function getPermissions() {
    var params = "";
    var checkboxs = $("#user_permission [type=checkbox]:checked");

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
function getUserRoles() {
    var params = "";
    var checkboxs = $("select[name=HaveRole] option");

    //遍历checkbox集合
    $.each(checkboxs, function (i, o) {
        if (checkboxs.length == i + 1) {
            params += $(o).val();
        } else {
            params += $(o).val() + ",";
        }
    });
    return params;
}
