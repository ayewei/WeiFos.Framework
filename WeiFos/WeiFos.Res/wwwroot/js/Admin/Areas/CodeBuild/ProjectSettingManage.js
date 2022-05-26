var datagrid;

function getSearchData() {
    return { keyword: $("#keyword").val() };
}

$(function () {

    //表格初始化
    datagrid = $("[name=datagrid]").datagrid({
        url: "/CodeBuildModule/CodeBuild/GetProjectSettings",
        data: getSearchData(),
        pager: {
            index: 0,
            pageSize: [10, 20, 50]
        },
        template_id: "template",
        column: [
            {
                text: "项目名称",
                style: "width:150px;"
            },
            {
                text: "英文名（默认命名空间）",
                style: "width:300px;"
            },
            {
                text: "数据库名称",
                style: "width:100px;"
            },
            {
                text: "数据库IP",
                style: "width:60px;"
            },
            {
                text: "创建时间",
                style: "width:150px;"
            }
        ],
        //行双击事件
        dblclick: function (tr) {
            window.location.href = '/CodeBuildModule/CodeBuild/ProjectSettingForm?bid=' + tr.attr("data-id");
        },
        callback: function (result) {
            if (result.Code == App_G.Code.Code_200) {
                trlist = result.Data;
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
    $("[name=add_btn]").click(function () {
        window.location.href = '/CodeBuildModule/CodeBuild/ProjectSettingForm';
    });

    //修改
    $("[name=edit_btn]").click(function () {
        var tr = datagrid.getSeleteTr();
        if (tr.length == 0) {
            layer.msg('请选择操作的行', { icon: 2 });
            return false;
        }
        window.location.href = '/CodeBuildModule/CodeBuild/ProjectSettingForm?bid=' + datagrid.getSeleteTr().attr("data-id");
    });

    //删除
    $("div.dataTables_filter").digbox({
        Selector: "[name=delete_btn]",
        Title: "提示信息",
        Context: "确定删除该链接吗？",
        Before: function () {
            var tr = datagrid.getSeleteTr();
            if (tr.length == 0) {
                layer.msg('请选择操作的行', { icon: 2 });
                return false;
            }
            return true;
        },
        CallBack: function (s, c, p) {
            $get("/CodeBuildModule/CodeBuild/DelProjectSetting?id=" + datagrid.getSeleteTr().attr("data-id"), "",
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
 

