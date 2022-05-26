
//获取查询数据
function getSearchData() {
    return { keyword: $("#keyword").val(), type: 0, status: $("#status").val() };
}

$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

    //表格初始化
    datagrid = $("[name=datagrid]").datagrid({
        url: "/UserModule/User/GetUsers",
        data: getSearchData(),
        pager: {
            index: 0,
            pageSize: [10, 20, 50]
        },
        template_id: "template",
        column: [
            {
                text: "基本信息",
                style: "width:50px;"
            },
            {
                text: "登录信息",
                style: "width:150px;"
            },
            {
                text: "状态",
                style: "width:150px;"
            },
            {
                text: "是否绑定微信",
                style: "width:150px;"
            },
            {
                text: "创建时间",
                style: "width:100px;"
            }
        ],
        dblclick: function (tr) {
            window.location.href = '/ProductModule/Product/BrandForm?bid=' + tr.attr("data-id");
        }
    });


    $("#search_btn").click(function () {
        pager.execute(getSearchData());
    });

    //解冻用户
    $("[name=enable]").digbox({
        Title: "信息提示框",
        Context: "确定解冻用户该用户吗？",
        Selector: "#listTable",
        CallBack: function (b, c, p) {

            $get("/User/SetEnable?id=" + c.parent().parent().attr("data-id") + "&status=1", "",
                function (data) {
                    if (data.Code == App_G.Code.Code_200) {
                        App_G.MsgBox.success_digbox();
                        c.parent().parent().find("td:eq(6)").find("span").eq(1).hide();
                        c.parent().parent().find("td:eq(6)").find("span").eq(0).show();

                        c.parent().parent().find("td:eq(8)").find("a").eq(1).hide();
                        c.parent().parent().find("td:eq(8)").find("a").eq(0).show();

                    } else {
                        App_G.MsgBox.error_digbox();
                    }
                });
        }
    });

    //冻结用户
    $("[name=disable]").digbox({
        Title: "信息提示框",
        Selector: "#listTable",
        Context: "确定解冻用户吗？",
        CallBack: function (b, c, p) {

            $get("/User/SetEnable?id=" + c.parent().parent().attr("data-id") + "&status=0", "",
                function (data) {
                    if (data.Code == App_G.Code.Code_200) {
                        App_G.MsgBox.success_digbox();
                        c.parent().parent().find("td:eq(6)").find("span").eq(0).hide();
                        c.parent().parent().find("td:eq(6)").find("span").eq(1).show();

                        c.parent().parent().find("td:eq(8)").find("a").eq(0).hide();
                        c.parent().parent().find("td:eq(8)").find("a").eq(1).show();
                    } else {
                        App_G.MsgBox.error_digbox();
                    }
                });
        }
    });

    //修改
    $("#listTable").on("click", "[name=edit_btn]", function () {
        window.location.href = "/User/UserForm?bid=" + $(this).parent().parent().attr("data-id");
    });
     

});

 
//获取用户类型
template.defaults.imports.getUserType = function (type) {

    var name = "";
    var UserType = $.parseJSON($("#UserType").val());

    //获取用户类型
    $.each(UserType, function (i, o) {
        if (o.id == type) {
            name = o.role_name;
            return;
        }
    });

    return name;
};
