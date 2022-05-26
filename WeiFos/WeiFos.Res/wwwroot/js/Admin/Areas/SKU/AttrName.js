/*!
 * 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
 * Copyright (c) 2013-2018 深圳微狐信息技术有限公司
 * 创 建：叶委
 * 日 期：2018-12-05 11:53:51
 * 描 述：基本属性页脚本
 */
var datagrid, v = null, diglog;

$(function () {

    App_G.Auth.InitID();

    //表格初始化
    datagrid = $("[name=datagrid]").datagrid({
        url: "/SKUModule/SKU/GetAttrNames?bid=" + App_G.Util.getRequestId('bid'),
        data: getSearchData(),
        template_id: "template",
        column: [
            {
                text: "排序",
                style: "width: 40px; "
            },
            {
                text: "属性名称",
                style: "width: 140px; "
            },
            {
                text: "列表显示",
                style: "width: 140px; "
            }
        ],
        dblclick: function (tr) {
            $("<div></div>").digbox({
                IsAuto: true,
                Title: "提示信息",
                Content: tmp,
                Before: function () {
                    var tr = datagrid.getSeleteTr();
                    if (tr.length == 0) {
                        layer.msg('请选择操作的行', { icon: 2 });
                        return false;
                    }
                    return true;
                },
                Show: function (b, c, p) {

                    var data = {
                        order_index: datagrid.getSeleteTr().find("td:eq(0)").text(),
                        name: datagrid.getSeleteTr().find("td:eq(1)").text(),
                        show_list: datagrid.getSeleteTr().find("td:eq(2) > a").hasClass("i_select")
                    };

                    //绑定数据
                    App_G.Mapping.Bind("data-val", data, p);

                    //绑定验证
                    submit($("#" + b), p);
                },
                CallBack: function (s, c, p) {
                    return v.valid.executeGroup();
                }
            });
        },
        callback: function (result) {
            if (result.Code == App_G.Code.Code_200) {
                trlist = result.Data.pageData;
            } else {
                layer.msg(result.Message, { icon: 2 });
            }
        }
    });

    //查询
    $("[name=search_btn]").click(function () {
        datagrid.execute(getSearchData());
    });

    //刷新
    $("[name=refresh_btn]").click(function () {
        window.location.reload();
    });

    //新增
    $("[name=add_btn]").digbox({
        Selector: "[name=add_btn]",
        Context: "div.dataTables_filter",
        Title: "添加基本属性",
        Content: tmp,
        Show: function (s, c, p) {
            submit($("#" + s), p, c, "save");
        },
        CallBack: function (e) {
            return false;
        }
    });

    //删除
    $("[name=delete_btn]").digbox({
        Selector: "[name=delete_btn]",
        Context: "div.dataTables_filter",
        Title: "提示信息",
        Content: "确认删除该数据吗",
        Before: function () {
            var tr = datagrid.getSeleteTr();
            if (tr.length == 0) {
                layer.msg('请选择操作的行', { icon: 2 });
                return false;
            }
            return true;
        },
        CallBack: function (s, c, p) {
            $post("/SKUModule/SKU/DeleteAttrName?bid=" + datagrid.getSeleteTr().attr("data-id"), "",
                function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        layer.msg(result.Message, { icon: 1 });
                        datagrid.execute(getSearchData());
                    } else {
                        layer.msg(result.Message, { icon: 2 });
                    }
                });
        }
    });

    //修改
    $("[name=edit_btn]").digbox({
        Selector: "[name=edit_btn]",
        Context: "div.dataTables_filter",
        Title: "提示信息",
        Content: tmp,
        Before: function () {
            var tr = datagrid.getSeleteTr();
            if (tr.length == 0) {
                layer.msg('请选择操作的行', { icon: 2 });
                return false;
            }
            return true;
        },
        Show: function (b, c, p) {

            var data = {
                order_index: datagrid.getSeleteTr().find("td:eq(0)").text(),
                name: datagrid.getSeleteTr().find("td:eq(1)").text(),
                show_list: datagrid.getSeleteTr().find("td:eq(2) > a").hasClass("i_select")
            };

            //绑定数据
            App_G.Mapping.Bind("data-val", data, p);

            //绑定验证
            submit($("#" + b), p);

        },
        CallBack: function (s, c, p) {
            return false;
        }
    });

    //下一步
    $("#NextBtn").click(function () {
        window.location.href = "/SKUModule/SKU/ExtAttrName?bid=" + App_G.Util.getRequestId('bid');
    });

});


//查询数据
function getSearchData() {
    return { keyword: $("#keyword").val() };
}

//提交数据
function submit(btn, p) {

    diglog = p;

    //初始化验证插件
    yw.valid.config({});

    if (v == null) {
        //获取验证类型
        v = yw.valid.getValidate({
            vsuccess: function () {

                $post("/SKUModule/SKU/SaveAttrName", JSON.stringify(GetData()),
                    function (result) {
                        if (result.Code == App_G.Code.Code_200) {
                            layer.msg(result.Message, { icon: 1 });
                            datagrid.execute(getSearchData());
                            p.modal('hide');
                        }
                        else {
                            layer.msg(result.Message, { icon: 2 });
                        }
                    });
            }
        });
    }

    //商品类型名称
    v.valid("#name", {
        selector: p,
        vtype: verifyType.anyCharacter,
        focus: { msg: "属性名称长度在1~15字符" },
        blur: { msg: "属性名称长度在1~15字符" }
    });

    //排序索引
    v.valid("#order_index", {
        selector: p,
        vtype: verifyType.isNumber,
        focus: { msg: "请输入排序索引" },
        blur: { msg: "排序索引只能为数字" }
    });
}


//获取提交数据
function GetData() {
    var id = 0, tr = datagrid.getSeleteTr();
    if (tr.length > 0) {
        id = datagrid.getSeleteTr().attr("data-id");
    }

    return {
        attrName: App_G.Mapping.Get("data-val", { id: id, product_type_id: App_G.Util.getRequestId('bid') }, diglog)
    };
}

//模板
var tmp = "<div id = 'popup' style='height:160px;width:640px;' >"
    + "<table width='90%' border='0' cellpadding='0' cellspacing='0' class='table_s1'>"
    + "<tr>"
    + "<th scope='row'>属性名称：</th>"
    + "<td> <input type='text' id='name' data-val='name' maxlength='30' class='form-control form-control-sm' /> </td>"
    + "</tr>"
    + "<tr>"
    + "<th scope='row'>列表显示：</th>"
    + "<td><input id='show_list' type='checkbox'  data-val='show_list' /> 是"
    + "</td>"
    + "</tr>"
    + "<tr>"
    + "<th scope='row'>排序索引：</th>"
    + "<td><input type='text' id='order_index' data-val='order_index' maxlength='9' style='width:50px' class='form-control form-control-sm' />"
    + "</td>"
    + "</tr></table>"
    + "</div>";
