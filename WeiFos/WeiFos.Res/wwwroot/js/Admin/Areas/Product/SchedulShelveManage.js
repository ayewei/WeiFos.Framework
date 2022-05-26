/*!
 * 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
 * Copyright (c) 2013-2018 深圳微狐信息技术有限公司
 * 创 建：叶委
 * 日 期：2018-12-05 11:53:51
 * 描 述：商品上下架管理页脚本
 */
var datagrid, selected_ids = [];

$(function () {

    bindDate();

    //初始化权限编号
    App_G.Auth.InitID();

    //表格初始化
    datagrid = $("[name=datagrid]").datagrid({
        url: "/ProductModule/Product/GetProducts",
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
                text: "显示顺序",
                style: "width:40px;"
            },
            {
                text: "基本信息",
                style: "width:240px;"
            },
            {
                text: "所属分类",
                style: "width:150px;"
            },
            {
                text: "排期时间",
                style: "width:150px;"
            },
            {
                text: "商品状态",
                style: "width:100px;"
            },
            {
                text: "是否上架",
                style: "width:40px;"
            }
        ],
        completed: function () {
            bindDate();
        },
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

    //单个排期
    $("div[name=datagrid]").on("click", "button[name=shelves_date_btn]", function () {
        var date = $(this).parent().parent().find("[name=date]").val();
        if (date == "") {
            layer.msg("请选择日期范围", { icon: 2 });
            return;
        }

        var data = { ids: [$(this).parents("tr").attr("data-id")], date: date };

        //保存排期
        $post("/ProductModule/Product/SaveShelvesDate", JSON.stringify(data),
            function (result) {
                if (result.Code == App_G.Code.Code_200) {
                    layer.msg(result.Message, { icon: 1 });
                    datagrid.execute(getSearchData());
                } else {
                    layer.msg(result.Message, { icon: 2 });
                }
            });
    });


    //批量排期
    $("#batch_schedul_btn").click(function () {
        if ($("#range_date").val() == "") {
            layer.msg("请选择批量排期日期范围！", { icon: 2 });
            return;
        }
        var data = { ids: selected_ids, date: $("#range_date").val() };
        $post("/Product/SaveShelvesDate", JSON.stringify(data),
            function (result) {
                if (result.Code == App_G.Code.Code_200) {
                    selected_ids = [];
                    layer.msg(result.Message, { icon: 1 });
                    datagrid.execute(getSearchData());
                } else {
                    layer.msg(result.Message, { icon: 2 });
                }
            });
    });

    //全选事件
    $("div[name=datagrid]").on("click", "#check_all", function () {
        var checked = $(this).prop("checked");
        datagrid.getDataGrid().find("input[type=checkbox]").prop("checked", checked);
        bindCheckBox();
    });

    //子复选框点击事件
    $("div[name=datagrid]").on("click", "input[name=pdt_checkbox]", function () {
        var checkboxs = $("div[name=datagrid] input[name=pdt_checkbox]");
        if (checkboxs.length == checkboxs.filter(":checked").length) {
            datagrid.getDataGrid().find("#check_all").prop("checked", true);
        } else {
            datagrid.getDataGrid().find("#check_all").prop("checked", false);
        }
        bindCheckBox();
    });

    //批量上架
    $("#batch_up_btn").digbox({
        Selector: "#batch_up_btn",
        Context: ".dataTables_filter",
        Title: "信息提示框",
        Content: "确认上架选中商品吗？",
        Before: function () {
            if (selected_ids.length == 0) {
                layer.msg('请勾选需要上架的商品', { icon: 2 });
                return false;
            }
            return true;
        },
        CallBack: function (s, c, p) {
            var data = { ids: selected_ids, isShelves: true };
            $postByAsync("/ProductModule/Product/SelectShelves", JSON.stringify(data),
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

    //批量下架
    $("#batch_down_btn").digbox({
        Selector: "#batch_down_btn",
        Context: ".dataTables_filter",
        Title: "信息提示框",
        Content: "确认下架选中商品吗？",
        Before: function () {
            if (selected_ids.length == 0) {
                layer.msg('请勾选需要下架的商品', { icon: 2 });
                return false;
            }
            return true;
        },
        CallBack: function (e) {
            var data = { ids: selected_ids, isShelves: false };
            //转移商品
            $postByAsync("/ProductModule/Product/SelectShelves", JSON.stringify(data),
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

    //上架
    $("#up_btn").digbox({
        Selector: "#up_btn",
        Context: ".dataTables_filter",
        Title: "信息提示框",
        Content: "确认上架该商品吗？",
        Before: function () {
            if (selected_ids.length == 0) {
                layer.msg('请勾选需要上架的商品', { icon: 2 });
                return false;
            }
            return true;
        },
        CallBack: function (s, c, p) {
            var data = { ids: [datagrid.getSeleteTr().attr("data-id")], isShelves: true };
            //转移商品
            $postByAsync("/ProductModule/Product/SelectShelves", JSON.stringify(data),
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

    //下架
    $("#down_btn").digbox({
        Selector: "#down_btn",
        Context: ".dataTables_filter",
        Title: "信息提示框",
        Content: "确认下架该商品吗？",
        Before: function () {
            var tr = datagrid.getSeleteTr();
            if (tr.length == 0) {
                layer.msg('请选择操作的行', { icon: 2 });
                return false;
            }
            return true;
        },
        CallBack: function (s, c, p) {
            $("#up_select").prop("disabled", true);

            var data = { ids: [datagrid.getSeleteTr().attr("data-id")], isShelves: false };
            //转移商品
            $postByAsync("/ProductModule/Product/SelectShelves", JSON.stringify(data),
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

});


//初始化日历控件
function bindDate() {
    //日历控件
    $("[name=date]").daterangepicker({
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
}

//查询数据
function getSearchData() {
    return {
        catg_id: $("#catg_id").val(), gcatg_id: $("#gcatg_id").val(), brand_id: $("#brand_id").val(), keyword: $("#keyword").val(), is_shelves: $("#is_shelves").val(), date: $("#date").val()
    };
}

//获取选中参数
function bindCheckBox() {
    //当前页面checkbox
    var checkboxs = datagrid.getDataGrid().find("tbody input[name=pdt_checkbox]");
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

    $("#us_count").text(selected_ids.length);
    $("#ds_count").text(selected_ids.length);

    if (selected_ids.length > 0) {
        $("[name=date_range]").show();
    } else {
        $("[name=date_range]").hide();
    }
}

//获取编辑地址
function EditUrl(obj) {
    //翻页信息data-cid
    var pagination = pager.getPager();
    var url = "/ProductModule/Product/ProductForm?bid=" + obj.parent().attr("data-id") + "&cid=" + obj.parent().attr("data-cid") + "&page=" + pagination.currentPage
        + "&scid=" + $("#CategoryID").val() + "&gcid=" + $("#GuideCategoryID").val() + "&brid=" + $("#BrandID").val() + "&pname="
        + $("#ProductName").val();

    return url;
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

