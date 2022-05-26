
var v;

$(function () {
    //初始化权限编号
    App_G.Auth.InitID();
    //绑定数据
    if ($("#Menus").val().length) {
        var p_menus = [];
        var c_menus = [];
        var menus = $.parseJSON($("#Menus").val());

        $.each(menus, function (i, o) {
            if (o.parent_id == 0) {
                p_menus.push(o);
            } else {
                c_menus.push(o);
            }
        });

        var data = {
            p_menus: p_menus,
            c_menus: c_menus
        };

        if ($("#listTable tbody").find("tr").length > 0) {
            $("#listTable tbody tr").remove();
        }
        $("#listTable tbody").append(template("template", data));
    }

    //初始化验证插件
    yw.valid.config({
        submitelements: "#SaveBtn", vsuccess: function () {
            $("#SaveBtn").setDisable({ text: "保存中...", time: 2000 });

            var data = {
                menus: GetMenu()
            };

            $post("/WeChat/SaveWeChatMenu", JSON.stringify(data), function (data) {
                if (data.Code == App_G.Code.Code_200) {
                    App_G.MsgBox.success_digbox("设置成功");
                    setTimeout("window.location.href='" + App_G.Util.getRawUrl() + "';", 2000);
                } else {
                    App_G.MsgBox.error_digbox("网络异常！");
                }
            });
        }
    });

    //正常上传
    v = yw.valid.getValidate();
    $("#listTable tr:not(:eq(0))").each(function (i) {
        var id = $(this).find("input:hidden[name='[id]']").val();
        BindValidate("", id);
    });

    //添加菜单
    $("#addmenu").click(function () {
        var rows = $("#listTable tr[data-pid=0]").length;
        if (rows > 2) {
            App_G.MsgBox.error_digbox("主菜单最多只能开启3个");
            return;
        }

        var index = $("#listTable tr:not(:eq(0))").length + 1;
        var tr = "<tr data-id='0' data-pid='0' ><td><input class=\"input-mini\" name=\"[sort]\" type=\"text\" value=\"0\"  id=\"new_sort_" + index + "\" /></td>" +
                    "<td><input class=\"input-medium\" type=\"text\" name=\"[name]\" data-p=\"true\" id=\"new_name_" + index + "\" /><i id=\"add_menu_" + index + "\" style=\"cursor:pointer;\" title=\"添加子菜单\" class=\"icon-plus cursor_p add\"></i></td>" +
                    "<td><input type=\"text\" name=\"[key]\" class=\"input-xlarge\" id=\"new_key_" + index + "\" /></td>" +
                    "<td><input type=\"checkbox\" checked=\"checked\" name=\"[is_show]\" /></td>" +
                    "<td><a href=\"javascript:void(0);\" title=\"删除\"><i class=\"icon-remove\"></i></a></td></tr>";
        $("#listTable").append(tr);

        BindValidate("new_", index);
    });

    //添加子菜单
    $("#listTable").on("click", ".icon-plus.cursor_p.add", function () {
        var tr = $(this).parents("tr");
        var parentid = tr.attr("data-id");
        var rows = $("#listTable tr[data-pid=" + parentid + "]").length;
        if (rows > 4) {
            App_G.MsgBox.error_digbox("子菜单最多只能开启5个");
            return;
        }

        var index = $("#listTable tr:not(:eq(0))").length + 1;
        var control = "<tr data-id='0' data-pid='" + parentid + "'><td><input class=\"input-mini\" type=\"text\" name=\"[sort]\" value=\"0\" id=\"new_sort_" + index + "\" /></td>" +
                    "<td><i class=\"board\"></i><input class=\"input-medium\" type=\"text\" name=\"[name]\" id=\"new_name_" + index + "\" /></td>" +
                    "<td><input type=\"text\" name=\"[key]\" id=\"new_key_" + index + "\" class=\"input-xlarge\" /></td>" +
                    "<td><input type=\"checkbox\" checked=\"checked\" name=\"[is_show]\" /></td>" +
                    "<td><a href=\"javascript:void(0);\" title=\"删除\"><i class=\"icon-remove\"></i></a></td></tr>"
        $(control).insertAfter(tr);

        BindValidate("new_", index);
    });


    //删除
    $(".icon-remove").digbox({
        Selector: "#listTable",
        Title: "信息提示框",
        Context: "确定删除吗？",
        CallBack: function (submit_btnid, current, panel) {

            var id = current.parents("tr").attr("data-id");
            $.ajax({
                type: "get",
                url: App_G.Util.getDomain() + "/WeChat/Delete?bid=" + id,
                success: function (data) {
                    if (data.Code == App_G.Code.Code_200) {
                        current.parents("tr").remove();
                        current.parents("tr[data-pid=" + id + "]").remove();
                    } else {
                        App_G.MsgBox.error_digbox("操作失败！");
                    }
                }
            });

        }
    });


    //生成微信自定义菜单
    $("#BuildWXMenu").click(function () {
        $.ajax({
            type: "get",
            url: App_G.Util.getDomain() + "/WeChat/BuildWeChatMenu?aid=" + App_G.Util.getRequestId('aid'),
            success: function (data) {
                if (data.Code == App_G.Code.Code_200) {
                    App_G.MsgBox.success_digbox("自定义菜单创建成功");
                    setTimeout("window.location.href='" + App_G.Util.getRawUrl() + "';", 2000);
                } else if (data.State == App_G.BackState.State_500) {
                    App_G.MsgBox.error_digbox("网络异常");
                } else {
                    App_G.MsgBox.error_digbox("操作失败! 错误代码:" + data.Data);
                }
            }
        });
    });


    //删除微信自定义菜单
    $("#DelWXMenu").click(function () {
        $.ajax({
            type: "get",
            url: App_G.Util.getDomain() + "/WeChat/BuildWeChatMenu?aid=" + App_G.Util.getUrlParam('aid'),
            success: function (data) {
                if (data.Code == App_G.Code.Code_200) {
                    App_G.MsgBox.success_digbox("自定义菜单删除成功");
                    setTimeout("window.location.href='" + App_G.Util.getRawUrl() + "';", 2000);
                } else if (data.State == App_G.BackState.State_500) {
                    App_G.MsgBox.error_digbox("网络异常");
                } else {
                    App_G.MsgBox.error_digbox("操作失败! 错误代码:" + data.Data);
                }
            }
        });
    });


})

//正整数
function ValidateNumber(number) {
    return /^\d+$/g.test(number);
}


//绑定验证
function BindValidate(prefix, id) {
    if ($("#" + prefix + "name_" + id).attr("data-p")) {
        v.valid("#" + prefix + "name_" + id, { afterelementId: "add_menu_" + id, selector: "#listTable", focusmsg: "", errormsg: "", vtype: verifyType.anyCharacter });
    } else {
        v.valid("#" + prefix + "name_" + id, { selector: "#listTable", focusmsg: "", errormsg: "", vtype: verifyType.anyCharacter });
    }
    v.valid("#" + prefix + "sort_" + id, { selector: "#listTable", focusmsg: "请输入数字", errormsg: "", vtype: verifyType.isNumber });
    v.valid("#" + prefix + "key_" + id, { selector: "#listTable", focusmsg: "", errormsg: "", vtype: verifyType.anyCharacter });
}


//获取提交表单JSON
var GetMenu = function () {
    var params = [];
    $("#listTable tr:not(:eq(0))").each(function (i, o) {
        var data = {};
        data.id = $(o).attr("data-id");
        data.parent_id = $(o).attr("data-pid");
        data.sort = $(o).find("input[name='[sort]']").val();
        data.button_name = $(o).find("input[name='[name]']").val();
        data.key_value = $(o).find("input[name='[key]']").val();
        data.is_show = $(o).find("input:checkbox[name='[is_show]']").is(":checked");

        if (data.id == "") data.id = 0;
        if (data.parent_id == "") data.parent_id = 0;
        params.push(data);
    });
    return params;
};


template.helper('GetIndex', function () {
    return $("#listTable tr:not(:eq(0))").length;
});

