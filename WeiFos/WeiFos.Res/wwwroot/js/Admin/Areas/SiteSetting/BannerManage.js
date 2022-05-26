
/*!
 * 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
 * Copyright (c) 2013-2018 深圳微狐信息技术有限公司
 * 创 建：叶委
 * 日 期：2018-12-05 11:53:51
 * 描 述：品牌管理页脚本
 */
var datagrid;

$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

    //表格初始化
    datagrid = $("[name=datagrid]").datagrid({
        url: "/SiteSetting/GetBanners",
        data: getSearchData(),
        pager: {
            index: 0,
            pageSize: [10, 20, 50]
        },
        template_id: "template",
        column: [
            {
                text: "缩略图",
                style: "width:150px;"
            },
            {
                text: "图片名称",
                style: "width:150px;"
            },
            {
                text: "类型",
                style: "width:150px;"
            },
            {
                text: "是否是主图",
                style: "width:100px;"
            },
            {
                text: "创建时间",
                style: "width:100px;"
            }
        ],
        dblclick: function (tr) {
            window.location.href = '/SiteSettingModule/SiteSetting/BannerForm?bid=' + tr.attr("data-id");
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
        window.location.href = '/SiteSettingModule/SiteSetting/BannerForm';
    });

    //修改
    $("[name=edit_btn]").click(function () {
        var tr = datagrid.getSeleteTr();
        if (tr.length == 0) {
            layer.msg('请选择操作的行', { icon: 2 });
            return false;
        }
        window.location.href = '/SiteSettingModule/SiteSetting/BannerForm?bid=' + datagrid.getSeleteTr().attr("data-id");
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
            var data = { ids: [datagrid.getSeleteTr().attr("data-id")] };
            $post("/SiteSettingModule/SiteSetting/DeleteBanner?bid=", JSON.stringify(data),
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



template.defaults.imports.GetMsgTypeValue = function (type) {
    switch (type) { 
        case 200:
            return "移动端首页顶部Banner";
        case 201:
            return "移动端首页底部栏目1";
        case 202:
            return "移动端首页栏目Banner";
        case 203:
            return "移动端首页栏目Banner";
        case 211:
            return "移动端首页底部栏目Banner"; 

        default:
            return "";
    }
};
 