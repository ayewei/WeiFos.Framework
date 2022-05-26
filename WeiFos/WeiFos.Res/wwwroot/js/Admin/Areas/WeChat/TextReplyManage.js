/*!
 * 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
 * Copyright (c) 2013-2018 深圳微狐信息技术有限公司
 * 创 建：叶委
 * 日 期：2018-12-05 11:53:51
 * 描 述：文本回复管理页脚本
 */
var datagrid, selected_ids = [];

$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

    //表格初始化
    datagrid = $("[name=datagrid]").datagrid({
        url: "/WeChatModule/WeChat/GetTextReplys",
        data: getSearchData(),
        pager: {
            index: 0,
            pageSize: [10, 20, 50]
        },
        template_id: "template",
        column: [
            {
                html: "<input id='check_all' type='checkbox' />",
                style: "width:10px;"
            },
            {
                text: "关键词",
                style: "width:100px;"
            },
            {
                text: "回复内容",
                style: "width:200px;"
            },
            {
                text: "创建时间",
                style: "width:150px;"
            }
        ],
        dblclick: function (tr) {
            window.location.href = '/WeChatModule/WeChat/TextReplyForm?bid=' + tr.attr("data-id");
        }
    });

    //回车查询 
    $("#keyword").keypress(function (e) {
        var e = e || window.event;
        if (e.keyCode == 13) {
            datagrid.execute(getSearchData());
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
    $("[name=add_btn]").click(function () {
        window.location.href = '/WeChatModule/WeChat/TextReplyForm';
    });

    //全选事件
    $("div[name=datagrid]").on("click", "#check_all", function () {
        var checked = $(this).prop("checked");
        datagrid.getDataGrid().find("input[type=checkbox]").prop("checked", checked);
        bindCheckBox();
    });

    //子复选框点击事件
    $("div[name=datagrid]").on("click", "input[name=checkboxs]", function () {
        var checkboxs = $("div[name=datagrid] input[name=checkboxs]");
        if (checkboxs.length == checkboxs.filter(":checked").length) {
            datagrid.getDataGrid().find("#check_all").prop("checked", true);
        } else {
            datagrid.getDataGrid().find("#check_all").prop("checked", false);
        }
        bindCheckBox();
    });

    //修改
    $("[name=edit_btn]").click(function () {
        var tr = datagrid.getSeleteTr();
        if (tr.length == 0) {
            layer.msg('请选择操作的行', { icon: 2 });
            return false;
        }
        window.location.href = '/WeChatModule/WeChat/TextReplyForm?bid=' + datagrid.getSeleteTr().attr("data-id");
    });

    //删除
    $("[name=delete_btn]").digbox({
        Title: "提示信息",
        Content: "确定删除该数据吗？",
        Before: function () {
            var tr = datagrid.getSeleteTr();
            if (tr.length == 0) {
                layer.msg('请选择操作的行', { icon: 2 });
                return false;
            }
            return true;
        },
        CallBack: function (s, c, p) {
            //删除的ID
            var data = { ids: [datagrid.getSeleteTr().attr("data-id")] };
            $post("/WeChatModule/WeChat/DelSelectTextReply", JSON.stringify(data),
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

    //删除选中
    $("[name=batch_delete_btn]").digbox({
        Title: "提示信息",
        Content: "确认删除选中信息？",
        Before: function () {
            if (selected_ids.length == 0) {
                layer.msg('请勾选需要删除的信息', { icon: 2 });
                return false;
            }
            return true;
        },
        CallBack: function (e) {
            var data = { ids: selected_ids, isShelves: false };
            //转移商品
            $post("/WeChatModule/WeChat/DelSelectTextReply", JSON.stringify(data),
                function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        selected_ids = [];
                        layer.msg(result.Message, { icon: 1 });
                        datagrid.execute(getSearchData());
                        $("#check_all").prop("checked", false);
                        bindCheckBox();
                    } else {
                        layer.msg(result.Message, { icon: 2 });
                    }
                });
        }
    });

});


//查询数据
function getSearchData() {
    return { keyword: $("#keyword").val() };
}

//获取选中参数
function bindCheckBox() {
    //当前页面checkbox
    var checkboxs = datagrid.getDataGrid().find("tbody input[name=checkboxs]");
    //循环当前页面
    $.each(checkboxs, function (i, o) {
        if ($(o).prop("checked")) {
            if (selected_ids.indexOf($(o).val()) == -1) {
                selected_ids.push($(o).val());
            }
        } else {
            selected_ids.remove($(o).val());
        }
    });
}


