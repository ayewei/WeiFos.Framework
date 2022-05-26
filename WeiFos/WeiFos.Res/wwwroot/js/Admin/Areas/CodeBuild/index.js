var datagrid, nodes = [], selected_node = null, selected_node = null;

//表详情查询字段
function getSearchData(link_id, pid, name) {
    return { link_id: link_id, pid: pid, tb_name: name };
}


$(function () {

    //var objPerson = {
    //    name: "obj",
    //    age: 32,
    //    test: function () {
    //        alert("Name: " + this.name + "<br> Age: " + this.age);
    //    }
    //}
    //$("[name=search_btn]").on("click", $.proxy(objPerson.test, objPerson))

    $("#treeview").css({ "max-height": $(window).height() * 0.84, "min-height": $(window).height() * 0.84 });
    $("#treeview").niceScroll({ cursorcolor: "#424242", zindex: 1000 }).onResize();

    //加载数据
    loadData();

    //表格初始化
    datagrid = $("[name=datagrid]").datagrid({
        url: "/CodeBuildModule/CodeBuild/GetTableInfo",
        data: getSearchData(0, 0, 0),
        template_id: "template",
        column: [
            {
                text: "ID",
                style: "width:50px;"
            },
            {
                text: "名称",
                style: "width:150px;"
            },
            {
                text: "数据类型",
                style: "width:150px;"
            },
            {
                text: "最大长度",
                style: "width:150px;"
            },
            {
                text: "主键",
                style: "width:100px;"
            },
            {
                text: "自动增长",
                style: "width:100px;"
            },
            {
                text: "是否可为空",
                style: "width:150px;"
            },
            {
                text: "备注说明",
                style: "width:300px;"
            }
        ],
        //行双击事件
        dblclick: function (tr) {
        },
        callback: function (result) {
            if (result.Code == App_G.Code.Code_200) {
                trlist = result.Data.tableInfo;
                //绑定模块
                if (result.Data.modules.length) {
                    bindModules(result.Data.modules)
                }
            } else {
                layer.msg(result.Message, { icon: 2 });
            }
        }
    });
     
    //刷新
    $("[name=refresh_btn]").click(function () {
        window.location.reload();
    });

    //预览
    $("[name=preview_btn]").click(function () {

        if (selected_node == null) {
            layer.msg('请选择操作的表', { icon: 2 });
            return false;
        }

        if (parseInt($("#modules").val()) < 0) {
            layer.msg('请选择配置模块', { icon: 2 });
            return false;
        }

        var xposition = 0; yposition = 0, width = 1000, height = $(window).height() - 150;
        if ((parseInt(navigator.appVersion) >= 4)) {
            xposition = (screen.width - width) / 2;
            yposition = (screen.height - height) / 4;
        }    
        theproperty = "width=" + width + ","
            //+ "height=" + height + ","
            //+ "location=no,"
            //+ "menubar=no,"
            //+ "resizable=yes,"
            //+ "scrollbars=yes,"
            //+ "status=0,"
            //+ "titlebar=0,"
            //+ "toolbar=0,"
            //+ "hotkeys=0,"
            + "screenx=" + xposition + "," //仅适用于Netscape  
            + "screeny=" + yposition + "," //仅适用于Netscape  
            + "left=" + xposition + "," //IE  
            + "top=" + yposition; //IE   
        window.open("/CodeBuildModule/CodeBuild/CodePreview?link_id=" + selected_node.link_id + "&mid=" + $("#modules").val() +"&tb_name=" + selected_node.table_name, '_blank', theproperty);
    });

    //代码生成
    $("[name=generate_btn]").click(function () {
        var tr = datagrid.getSeleteTr();
        if (tr.length == 0) {
            layer.msg('请选择操作的行', { icon: 2 });
            return false;
        }
    });

});


//加载数据
function loadData() {

    //加载服务器
    var nodes = loadServer();
    //revealNode(node | nodeId, options) ：显示一个树节点，展开从这个节点开始到根节点的所有节点

    //树参数
    var options = {
        onNodeExpanded: function (event, data) {
            loadDataTables(event, data);
        },
        onNodeSelected: function (event, data) {
            if (data.dbData.table_name) {
                selected_node = data.dbData;
                datagrid.execute(getSearchData(data.dbData.link_id, data.dbData.id, data.dbData.table_name));
            } else {
                selected_node = null;
            }
        },
        expandIcon: 'fa fa-plus-square-o',
        collapseIcon: 'fa fa-minus-square-o',
        nodeIcon: 'fa fa-server',
        bootstrap2: false,
        showTags: true,
        levels: 5,
        data: nodes
    };

    $('#treeview').treeview(options);
}

//加载服务器链接
function loadServer() {
    var nodes = [];
    //加载服务器
    $getByAsync("/CodeBuildModule/CodeBuild/LoadServer?r=" + Math.random(), "", function (result) {
        if (result.Code == App_G.Code.Code_200) {

            $.each(result.Data, function (i, o) {
                var node = {
                    dbData: {
                        id: o.id,
                        ip: o.name
                    },
                    text: o.name,
                    href: 'javascript:;',
                    //增加此参数展开
                    nodes: []
                };

                $.each(o.projects, function (ii, oo) {

                    var cnode = {
                        dbData: {
                            //项目ID
                            id: oo.id, 
                            link_id: oo.link_id, 
                            name: oo.name,
                        },
                        icon: "fa fa-database",
                        needLoad: true,
                        text: oo.name,
                        href: 'javascript:;',
                        //增加此参数展开
                        nodes: []
                    };
                    node.nodes.push(cnode);

                });

                nodes.push(node);
            });
        } else {
            layer.msg(result.Message, { icon: 2 });
        }
    });

    return nodes;
}

//加载表数据
function loadDataTables(event, data) {
    if (data.needLoad) {
        data.needLoad = false;
        var index = layer.load(1, {
            shade: [0.3, '#fff']
        });
        $get("/CodeBuildModule/CodeBuild/GetDataTables?bid=" + data.dbData.link_id + "&r=" + Math.random(), "", function (result) {
            if (result.Code == App_G.Code.Code_200) {
                $.each(result.Data, function (i, o) {
                    var node = {
                        dbData: { 
                            id:data.dbData.id,
                            link_id: data.dbData.link_id,
                            table_name: o.name
                        },
                        icon: "fa fa-table",
                        text: o.name,
                        href: 'javascript:;'
                    };
                    data.nodes.push(node);
                });

                $('#treeview').treeview('updateNode', [data.nodeId, data]);
            }
            layer.close(index);
        });
    }
}

//绑定模块
function bindModules(modules) {
    $("#modules").find("option").not(":eq(0)").remove();
    var options = "";
    $.each(modules, function (i, o) {
        options += "<option value='" + o.id + "'>" + o.name + "</option>";
    });
    $("#modules").append(options);
}