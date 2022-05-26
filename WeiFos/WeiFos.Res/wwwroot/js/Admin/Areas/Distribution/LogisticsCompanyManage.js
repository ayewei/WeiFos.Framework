/*!
 * 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
 * Copyright (c) 2013-2018 深圳微狐信息技术有限公司
 * 创 建：叶委
 * 日 期：2018-12-05 11:53:51
 * 描 述：物流公司页脚本管理页
 */
var datagrid;

$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

    //表格初始化
    datagrid = $("[name=datagrid]").datagrid({
        url: "/DistributionModule/Distribution/GetLogisticsCompanys",
        data: getSearchData(),
        pager: {
            index: 0,
            pageSize: [10, 20, 50]
        },
        template_id: "template",
        column: [
            {
                text: "显示顺序",
                style: "width:20px;"
            },
            {
                text: "公司名称",
                style: "width:100px;"
            },
            {
                text: "快递100Code",
                style: "width:50px;"
            },
            {
                text: "淘宝Code",
                style: "width:50px;"
            },
            {
                text: "公司网址",
                style: "width:200px;"
            }
        ],
        dblclick: function (tr) {
            window.location.href = '/DistributionModule/Distribution/LogisticsCompanyForm?bid=' + tr.attr("data-id");
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
        window.location.href = '/DistributionModule/Distribution/LogisticsCompanyForm';
    });

    //修改
    $("[name=edit_btn]").click(function () {
        var tr = datagrid.getSeleteTr();
        if (tr.length == 0) {
            layer.msg('请选择操作的行', { icon: 2 });
            return false;
        }
        window.location.href = '/DistributionModule/Distribution/DistributionForm?bid=' + datagrid.getSeleteTr().attr("data-id");
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
            $post("/DistributionModule/Distribution/DeleteLogisticsCompany?bid=" + datagrid.getSeleteTr().attr("data-id"), "",
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


//查询数据
function getSearchData() {
    return { keyword: $("#keyword").val() };
}

