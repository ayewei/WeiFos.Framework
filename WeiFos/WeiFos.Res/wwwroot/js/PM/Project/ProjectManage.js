
var pager;

//已选
var selected_ids = [];

//获取查询数据
function getSerachData() {
    return { projectName: $("#projectName").val(), status: $("#Status").val(), date: $("#expiryDate").val() };
}

$(function () {

    //初始化权限编号
    //App_G.Auth.InitID();

    $(".daterangepick").daterangepicker({
        format: 'YYYY/MM/DD',
        daysOfWeek: ['日', '一', '二', '三', '四', '五', '六']
    });

    //绑定查询
    BindSearch();

    pager = $("#PagerDiv").pager({
        url: App_G.Util.getDomain() + '/Project/GetProjects',
        data: getSerachData(),
        currentPage: 0,
        pageSize: [15, 50, 100],
        callback: function (data) {
            if ($("#listTable tbody").find("tr").length > 0) {
                $("#listTable tbody tr").remove();
            }
            $("#listTable tbody").append(template("template", data));
            bindCheckBox();
        }
    });

    $("#search_btn").click(function () {
        pager.execute(getSerachData());
    });

    $("#tb_tbody").on("click", "input[name=ck_checkbox]", (function () {
        getSelectParams();
        bindCheckBox();
    }));


    //授权
    $("[name=enable]").digbox({
        Selector: "#listTable",
        Title: "信息提示框",
        Context: "确认授权该项目吗？",
        CallBack: function (s, c, p) {
             
            $get("/Project/SetProjectEnable?bid=" + c.parent().attr("data-id") + "&isEnable=true", "",

              function (data) {
                  if (data.Code == App_G.Code.Code_200) {
                      App_G.MsgBox.success_digbox();
                      pager.execute(getSerachData());
                  } else {
                      App_G.MsgBox.error_digbox();
                  }
              });

        }
    });


    //关闭
    $("[name=dis_enable]").digbox({
        Selector: "#listTable",
        Title: "信息提示框",
        Context: "确认关闭该项目吗？",
        CallBack: function (s, c, p) {
             
            $get("/Project/SetProjectEnable?bid=" + c.parent().attr("data-id") + "&isEnable=false", "",

              function (data) {
                  if (data.Code == App_G.Code.Code_200) {
                      App_G.MsgBox.success_digbox();
                      pager.execute(getSerachData());
                  } else {
                      App_G.MsgBox.error_digbox();
                  }
              });

        }
    });


    //删除选中
    $("#delete_select").digbox({
        Selector: ".m_opBar",
        Title: "信息提示框",
        Context: "删除以后不可恢复，确定彻底删除吗？",
        CallBack: function (submit_btnid, current, panel) {
            $("#delete_select").prop("disabled", true);

            var data = { ids: selected_ids };
            //转移商品
            $postByAsync("/Study/DelSelectArticle", JSON.stringify(data),
            function (data) {
                if (data.Code == App_G.Code.Code_200) {
                    pager.execute(getSerachData());
                    selected_ids = [];
                    bindCheckBox();

                    getSelectParams();
                    App_G.MsgBox.success_digbox();
                    $("#delete_select").prop("disabled", false);
                } else {
                    App_G.MsgBox.error_digbox("操作失败");
                    $("#delete_select").prop("disabled", false);
                }
            });
        }
    });

});


//全选
function checkall(obj) {
    var checked = $(obj).prop("checked");
    if (checked) {
        $("#listTable input[type=checkbox]").prop("checked", checked);
    } else {
        $("#listTable input[type=checkbox]").removeAttr("checked");
    }

    getSelectParams();
    bindCheckBox();
}

//获取选中参数
function getSelectParams() {

    var checkboxs = $("#tb_tbody input[name=ck_checkbox]");
    //遍历checkbox集合
    $.each(checkboxs, function (i, o) {
        if ($(o).prop("checked")) {
            if (selected_ids.indexOf($(this).val()) == -1) {
                selected_ids.push($(this).val());
            }
        } else {
            selected_ids.remove($(this).val());
        }
    });

    return selected_ids;
}

//绑定选中
function bindCheckBox() {
    if (selected_ids.length > 0) {
        $("#delete_select").show();
    } else {
        $("#delete_select").hide();
    }

    var checkboxs = $("#tb_tbody input[name=ck_checkbox]");
    //遍历checkbox集合
    $.each(checkboxs, function (i, o) {
        $.each(selected_ids, function (j, k) {
            if ($(o).val() == k) {
                $(o).prop("checked", true);
                return;
            }
        });
    });

    var checkedbox_length = $("#tb_tbody input[name=ck_checkbox]").length;
    var checked_length = $("#tb_tbody input[name=ck_checkbox]:checked").length;

    if (checkedbox_length == checked_length && checkedbox_length != 0) {
        $("#CheckAll").prop("checked", true);
    } else {
        $("#CheckAll").prop("checked", false);
    }

    $("#delete_select").find("span").text(selected_ids.length);
}

//绑定翻页查询
function BindSearch() {
    if (App_G.Util.getUrlParam('title') != "null") {
        $("#title").val(App_G.Util.getUrlParam('title'));
    }
}

//获取编辑地址
function getEditAcionUrl(obj) {
    //翻页信息
    var pagination = pager.getPager();
    var url = "/Study/ArticleForm?bid=" + obj.parent().attr("data-id") + "&page=" + pagination.currentPage
    + "&title=" + $("#Title").val();

    return url;
}