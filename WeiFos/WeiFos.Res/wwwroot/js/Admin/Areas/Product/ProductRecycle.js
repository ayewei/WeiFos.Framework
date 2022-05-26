/*!
 * 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
 * Copyright (c) 2013-2018 深圳微狐信息技术有限公司
 * 创 建：叶委
 * 日 期：2018-12-05 11:53:51
 * 描 述：商品回收站管理页脚本
 */
var datagrid;

$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

    //日历控件
    $("#date").daterangepicker({
        language: 'zh-CN',
        showDropdowns: true,
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

    //表格初始化
    datagrid = $("[name=datagrid]").datagrid({
        url: "/ProductModule/Product/GetProductRecycles",
        data: getSearchData(),
        pager: {
            index: 0,
            pageSize: [10, 20, 50]
        },
        template_id: "template",
        column: [
            {
                html: "<input id='check_all' type='checkbox' />",
                style: "width:30px;"
            },
            {
                text: "显示顺序",
                style: "width:40px;"
            },
            {
                text: "基本信息",
                style: "width:150px;"
            },
            {
                text: "所属分类",
                style: "width:150px;"
            },
            {
                text: "商品状态",
                style: "width:150px;"
            },
            {
                text: "上传时间",
                style: "width:100px;"
            }
        ],
        dblclick: function (tr) {
            
        }
    });

    //回车查询 
    $("#keyword").keypress(function (e) {
        var e = e || window.event;
        if (e.keyCode == 13) {
            datagrid.execute(getSearchData());
        }
    });

    //清空日期
    $("#clear").click(function () {
        $("#date").val("");
    });

    //查询
    $("[name=search_btn]").click(function () {
        datagrid.execute(getSearchData());
    });

    //刷新
    $("[name=refresh_btn]").click(function () {
        window.location.reload();
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
            var data = { ids: [datagrid.getSeleteTr().attr("data-id")], isdelete: true };
            $post("/ProductModule/Product/DeleteSelect", JSON.stringify(data),
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
    $("[name=delete_select_btn]").digbox({
        Title: "提示信息",
        Content: "确定将选中商品删除到回收站？",
        Before: function () {
            if (getSelectParams() == 0) {
                layer.msg('请选择删除的商品', { icon: 2 });
                return false;
            }
            return true;
        },
        CallBack: function (e) {
            var iscolse = false;
            var data = { ids: [datagrid.getSeleteTr().attr("data-id")], isdelete: true };
            $post("/ProductModule/Product/DeleteSelect", JSON.stringify(data),
                function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        layer.msg(result.Message, { icon: 1 });
                        datagrid.execute(getSearchData());
                    } else {
                        layer.msg(result.Message, { icon: 2 });
                    }
                });
            return iscolse;
        }
    });

    //恢复
    $("[name=recover_btn]").digbox({
        Title: "提示信息",
        Content: "确定恢复该数据吗？",
        Before: function () {
            var tr = datagrid.getSeleteTr();
            if (tr.length == 0) {
                layer.msg('请选择操作的行', { icon: 2 });
                return false;
            }
            return true;
        },
        CallBack: function (s, c, p) {
            var data = { ids: [datagrid.getSeleteTr().attr("data-id")], isdelete: false };
            $post("/ProductModule/Product/DeleteOrRestore", JSON.stringify(data),
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

    //恢复选中
    $("[name=recover_select_btn]").digbox({
        Title: "提示信息",
        Content: "确定将选中商品还原到商品库？",
        Before: function () {
            if (getSelectParams() == 0) {
                layer.msg('请选择删除的商品', { icon: 2 });
                return false;
            }
            return true;
        },
        CallBack: function (e) {
            var iscolse = false;
            var data = { ids: [datagrid.getSeleteTr().attr("data-id")], isdelete: false };
            $post("/ProductModule/Product/DeleteOrRestore", JSON.stringify(data),
                function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        layer.msg(result.Message, { icon: 1 });
                        datagrid.execute(getSearchData());
                    } else {
                        layer.msg(result.Message, { icon: 2 });
                    }
                });
            return iscolse;
        }
    });

    //全选
    $("div[name=datagrid]").on("click", "#check_all", function () {
        var checked = $(this).prop("checked");
        datagrid.getDataGrid().find("input[type=checkbox]").prop("checked", checked);
    });

    //根据子选项全选
    $("div[name=datagrid]").on("click", "input[name=pdt_checkbox]", function () {
        var checkboxs = $("div[name=datagrid] input[name=pdt_checkbox]");
        if (checkboxs.length == checkboxs.filter(":checked").length) {
            datagrid.getDataGrid().find("#check_all").prop("checked", true);
        } else {
            datagrid.getDataGrid().find("#check_all").prop("checked", false);
        }
    });

});


//查询数据
function getSearchData() {
    return {
        catg_id: $("#catg_id").val(), gcatg_id: $("#gcatg_id").val(), brand_id: $("#brand_id").val(), keyword: $("#keyword").val(), is_shelves: $("#is_shelves").val(), date: $("#date").val()
    };
}

//获取选中参数
function getSelectParams() {
    var params = [];
    var checkboxs = datagrid.getDataGrid().find("tbody input[name=pdt_checkbox]:checked");
    //遍历checkbox集合
    $.each(checkboxs, function (i, o) {
        if ($(o).prop("checked")) {
            params.push($(o).val());
        }
    });
    return params;
}

//获取商品状态
template.defaults.imports.getStatus = function (tag) {
    var tag_html = '';
    if (tag != null && tag != undefined) {
        var t = tag.split(',');
        for (var i = 0; i < t.length; i++) {
            if (i == 1) {
                tag_html += "<span class='label label-success'>新品</span>&nbsp;";
            } else if (i == 2) {
                tag_html += "<span class='label label-important'>热门</span>&nbsp;";
            } else if (i == 3) {
                tag_html += "<span class='label label-warning'>推荐</span>&nbsp;";
            } else if (i == 4) {
                //tag += "<span class='label label-success'>上架</span>";
            } else if (i == 5) {
                //tag += "<span class='label label-success'>上架</span>";
            }
        }
    }
    return tag_html;
};

