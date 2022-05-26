
var pager;

//已选
var selected_ids = [];

//获取查询数据
function getSerachData() {
    return { nickname: $("#nickname").val(), usertype: $("#user_type").val(), tags: "" };
}

$(function () {

    //初始化权限编号
    //App_G.Auth.InitID();

    pager = $("#PagerDiv").pager({
        url: App_G.Util.getDomain() + '/Fans/GetFans',
        data: getSerachData(),
        currentPage: 0,
        pageSize: [15, 50, 100],
        callback: function (data) {
            if ($("#listTable tbody").find("tr").length > 0) {
                $("#listTable tbody tr").remove();
            }
            $("#listTable tbody").append(template("template", data));
            bindCheckBox();
        }
    });

    $("#search_btn").click(function () {
        pager.execute(getSerachData());
    });

    $("#tb_tbody").on("click", "input[name=ck_checkbox]", (function () {
        getSelectParams();
        bindCheckBox();
    }));

    //修改
    $("#listTable").on("click", "[name=edit_btn]", function () {
        window.location.href = getEditAcionUrl($(this));
    });

    //升级
    $("[name=up_marketer]").digbox({
        Selector: "#data_tbody",
        Title: "信息提示框",
        Context: tmp,
        Show: function (b, c, p) {

            var v = yw.valid.getValidate({
                submitelements: $("#" + b), vsuccess: function () {

                    var data = {
                        login_name: $("#login_name").val()
                    };

                    $postByAsync("/Fans/UpMarketer?bid=" + c.parent().parent().attr('data-id'), JSON.stringify(data),
                         function (data) {
                             if (data.Code == App_G.Code.Code_200) {
                                 p.modal("hide");
                                 App_G.MsgBox.success_digbox();
                                 pager.execute(getSerachData());
                             } else {
                                 App_G.MsgBox.error_digbox("操作失败！");
                             }
                         });
                }
            });

            //规格名称
            v.valid("#login_name", {
                focusmsg: "请输入推广商手机号码", errormsg: "格式有误", othermsg: "推广商手机号码不存在", vtype: verifyType.isMoblie,
                repeatvalidate: {
                    url: App_G.Util.getDomain() + "/Fans/ExistMobile"
                }
            });

        },
        CallBack: function (s, c, p) {
            return false;
        }
    });

});


//模板
var tmp = "<div id = 'popup' style='height:50px;width:640px;'  >"
            + "<table width='100%' border='0' cellpadding='0' cellspacing='0' class='table_s1'>"
            + "<tr>"
            + "<th scope='row'>手机号码：</th>"
            + "<td> <input class='login_name' type='text' id='login_name'  maxlength='30' /> </td>"
            + "</tr></table>"
        + "</div>";

//全选
function checkall(obj) {
    var checked = $(obj).prop("checked");
    if (checked) {
        $("#listTable input[type=checkbox]").prop("checked", checked);
    } else {
        $("#listTable input[type=checkbox]").removeAttr("checked");
    }

    getSelectParams();
    bindCheckBox();
}


//获取选中参数
function getSelectParams() {

    var checkboxs = $("#tb_tbody input[name=ck_checkbox]");
    //遍历checkbox集合
    $.each(checkboxs, function (i, o) {
        if ($(o).prop("checked")) {
            if (selected_ids.indexOf($(this).val()) == -1) {
                selected_ids.push($(this).val());
            }
        } else {
            selected_ids.remove($(this).val());
        }
    });

    return selected_ids;
}


//绑定选中
function bindCheckBox() {
    if (selected_ids.length > 0) {
        $("#operation_span").show();
    } else {
        $("#operation_span").hide();
    }

    var checkboxs = $("#tb_tbody input[name=ck_checkbox]");
    //遍历checkbox集合
    $.each(checkboxs, function (i, o) {
        $.each(selected_ids, function (j, k) {
            if ($(o).val() == k) {
                $(o).prop("checked", true);
                return;
            }
        });
    });

    var checkedbox_length = $("#tb_tbody input[name=ck_checkbox]").length;
    var checked_length = $("#tb_tbody input[name=ck_checkbox]:checked").length;

    if (checkedbox_length == checked_length && checkedbox_length != 0) {
        $("#CheckAll").prop("checked", true);
    } else {
        $("#CheckAll").prop("checked", false);
    }

    $("#delete_select").find("span").text(selected_ids.length);
}


//绑定翻页查询
function BindSearch() {
    if (App_G.Util.getUrlParam('keyword') != "null") {
        $("#KeyWordName").val(App_G.Util.getUrlParam('keyword'));
    }
}


//获取编辑地址
function getEditAcionUrl(obj) {
    //翻页信息
    var pagination = pager.getPager();
    var url = "/WeChat/ImgTextReplyForm?bid=" + obj.parent().attr("data-id") + "&aid=" + App_G.Util.getRequestId('aid') + "&page=" + pagination.currentPage
    + "&keywords=" + $("#KeyWordName").val() + "&title=" + $("#Title").val();

    return url;
}