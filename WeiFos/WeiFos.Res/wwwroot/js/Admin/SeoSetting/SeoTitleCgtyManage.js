var all_data = {};

$(function () {

    //页面初始化
    Initial();

    //禁用
    $("[name=disable]").digbox({
        Selector: "#listTable",
        Title: "提示信息",
        Context: "确定禁用该菜单吗？",
        CallBack: function (submit_btnid, current, panel) {
            $get("/Case/SetCaseCgtyEnable?bid=" + current.parent().attr("data-id") + "&isenable=false", "",
              function (data) {

                  if (data.Code == App_G.Code.Code_200) {
                      Initial();
                  } else {
                      App_G.MsgBox.error_digbox();
                  }
              });

        }
    });

    //启用
    $("[name=enable]").digbox({
        Selector: "#listTable",
        Title: "提示信息",
        Context: "确定启用该菜单吗？",
        CallBack: function (submit_btnid, current, panel) {
            $get("/Case/SetCaseCgtyEnable?bid=" + current.parent().attr("data-id") + "&isenable=true", "",
              function (data) {
                  if (data.Code == App_G.Code.Code_200) {
                      Initial();
                  } else {
                      App_G.MsgBox.error_digbox();
                  }
              });
        }
    });

    //关闭全部
    $("#close_all").click(function () {
        $("tr[pid=tr_0]").find(".icon-minus").attr("class", "icon-plus");
        $("#tbd_ategory").find("tr[pid!=tr_0]").hide();
    });

    //展开全部
    $("#open_all").click(function () {
        $("tr[pid=tr_0]").find(".icon-plus").attr("class", "icon-minus");;
        $("#tbd_ategory").find("tr[pid!=tr_0]").show();
    });


    $("#tbd_ategory").find(".icon-minus").click(function () {
        if ($(this).hasClass("icon-plus")) {
            $(this).attr("class", "icon-minus");
            showTr($(this).parent().parent().parent());
        } else {
            $(this).attr("class", "icon-plus");
            hideTr($(this).parent().parent().parent());
        }
    });

});


//初始化数据
function Initial() {
    //加载数据
    $getByAsync("/SeoSetting/GetKeyWordCgtys?type=0", "",
      function (data) {
          if ($("#listTable tbody").find("tr").length > 0) {
              $("#listTable tbody tr").remove();
          }
          trlist = data;
          $("#listTable tbody").append(template("template", data));
      });
}

//显示Tr
function showTr(tr) {
    var trs = $("tr[pid=" + tr.attr("id") + "]");
    trs.show();

    $.each(trs, function (i, o) {
        showTr($(o));
    });
}

//隐藏Tr
function hideTr(tr) {
    var trs = $("tr[pid=" + tr.attr("id") + "]");
    trs.hide();

    $.each(trs, function (i, o) {
        hideTr($(o));
    });
}

//删除Tr
function deleteTr(tr) {
    var trs = $("tr[pid=" + tr.attr("id") + "]");
    trs.remove();

    $.each(trs, function (i, o) {
        deleteTr($(o));
    });
}

var index = 0;
//父类数量
function getParentCount(pid) {
    $.each(trlist.data, function (i, o) {
        if (pid == o.id) {
            index++;
            getParentCount(o.parent_id);
            return;
        }
    });
    return index;
}

template.helper('getTrIndex', function (pid) {
    if ("tr_" + pid == "tr_0") {
        return "";
    } else {
        index = 0;
        return getParentCount(pid, index);
    }
});