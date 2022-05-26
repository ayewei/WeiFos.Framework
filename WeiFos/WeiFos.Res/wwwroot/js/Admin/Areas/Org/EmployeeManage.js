/*!
 * 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
 * Copyright (c) 2013-2018 深圳微狐信息技术有限公司
 * 创 建：
 * 日 期：2019-01-03 14:17:02
 * 描 述：部门管理页脚本
 */
var datagrid, nodes = [], selected_node = null, selected_node = { id: 0 };

$(function () {

    $("#treeview").css({ "max-height": $(window).height() * 0.84, "min-height": $(window).height() * 0.84 });
    $("#treeview").niceScroll({ cursorcolor: "#424242", zindex: 1000 }).onResize();

    //加载数据
    loadData();

    //表格初始化
    datagrid = $("[name=datagrid]").datagrid({
        url: "/OrgModule/Org/GetEmployees",
        data: getSearchData(0),
        pager: {
            index: 0,
            pageSize: [10, 20, 50]
        },
        template_id: "template",
        column: [
            {
                text: "显示排序",
                style: "width: 60px; "
            },
            {
                text: "所属公司",
                style: "width: 160px; "
            },
            {
                text: "所属部门",
                style: "width: 120px; "
            },
            {
                text: "员工姓名",
                style: "width: 120px; "
            },
            {
                text: "手机号码",
                style: "width: 120px; "
            },
            {
                text: "添加时间",
                style: "width: 150px; "
            }
        ],
        dblclick: function (tr) {
            window.location.href = "/OrgModule/Org/EmployeeForm?bid=" + tr.attr("data-id");
        }
    });

    //查询
    $("[name=search_btn]").click(function () {
        if (selected_node != null) {
            datagrid.execute(getSearchData(selected_node.id));
        }
    });

    //刷新
    $("[name=refresh_btn]").click(function () {
        window.location.reload();
    });

    //新增
    $("[name=add_btn]").click(function () {
        window.location.href = "/OrgModule/Org/EmployeeForm";
    });

    //修改
    $("[name=edit_btn]").click(function () {
        var tr = datagrid.getSeleteTr();
        if (tr.length == 0) {
            layer.msg('请选择操作的行', { icon: 2 });
            return false;
        }
        window.location.href = "/OrgModule/Org/EmployeeForm?bid=" + tr.attr("data-id");
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
            $post("/OrgModule/Org/DelEmployee?bid=" + datagrid.getSeleteTr().attr("data-id"), "",
                function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        layer.msg(result.Message, { icon: 1 });
                        datagrid.execute(getSearchData(selected_node.id));
                    } else {
                        layer.msg(result.Message, { icon: 2 });
                    }
                });
        }
    });

});


//查询
function getSearchData(company_id) {
    return { keyword: $("#keyword").val(), company_id: company_id };
}

//加载数据
function loadData() {

    //加载树节点
    var nodes = loadCompany();

    //树参数
    var options = {
        onNodeExpanded: function (event, data) {
        },
        onNodeSelected: function (event, data) {
            if (data.dbData.id) {
                selected_node = data.dbData;
                datagrid.execute(getSearchData(data.dbData.id));
            } else {
                selected_node = null;
            }
        },
        expandIcon: 'fa fa-plus-square-o',
        collapseIcon: 'fa fa-minus-square-o',
        bootstrap2: false,
        showTags: true,
        levels: 5,
        data: nodes
    };

    $('#treeview').treeview(options);
}

//加载部门信息
function loadDepartment(cid) {
    datagrid.execute(getSearchData(cid));
}

//加载公司树
function loadCompany() {
    var nodes = [];
    //加载服务器
    $getByAsync("/OrgModule/Org/GetCompanysTree?keyword=" + $("#keyword").val(), "", function (result) {
        if (result.Code == App_G.Code.Code_200) {

            $.each(result.Data, function (i, o) {
                var node = {
                    dbData: {
                        id: o.id,
                        name: o.name
                    },
                    text: o.name,
                    needLoad: true,
                    href: 'javascript:;',
                    state: {
                        expanded: true
                    },
                    //增加此参数展开
                    nodes: []
                };

                //获取子节点
                var cnode = GetChilds(o.id, o.childs);
                $.each(cnode, function (i, v) {
                    node.nodes.push(v);
                });

                nodes.push(node);
            });
        } else {
            layer.msg(result.Message, { icon: 2 });
        }
    });

    return nodes;
}

//获取子节点
function GetChilds(id, data) {

    var nodes = [];

    if (data != null && data.length > 0) {

        $.each(data, function (i, o) {
            //当前节点子节点
            if (o.parent_id == id) {
                var node = {
                    dbData: {
                        id: o.id,
                        name: o.name
                    },
                    needLoad: true,
                    text: o.name,
                    href: 'javascript:;',
                    state: {
                        expanded: true
                    },
                    nodes:  []
                };

                //获取子节点
                var cnode = GetChilds(o.id, o.childs);
                if (cnode.length > 0) {
                    node.nodes.push(cnode);
                }
                nodes.push(node);
            }
        });
    }
    return nodes;
}


//获取部门性质
template.defaults.imports.getNature = function (n) {
    console.log(n);
    switch (n) {
        case 1:
            return "综合性";
        case 2:
            return "生产性";
        case 3:
            return "咨询性";
        case 4:
            return "协调性";

        default:
            return "其他性";

    }
};

