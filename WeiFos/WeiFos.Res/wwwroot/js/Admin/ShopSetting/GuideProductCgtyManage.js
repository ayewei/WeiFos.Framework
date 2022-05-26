

var trlist;

$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

    //查询数据
    search();

    //显示
    $("[name=show]").digbox({
        Title: "提示信息",
        Selector: "#tbd_cgty",
        Context: "确定显示该分类吗？",
        CallBack: function (submit_btnid, current_obj, panel) {
            $get("/ShopSetting/SetEnableGuideProductCgty?id=" + current_obj.parent().attr("data-id") + "&isshow=true", "",
              function (data) {
                  if (data.Code == App_G.Code.Code_200) {
                      search();
                      App_G.MsgBox.success_digbox();
                  } else {
                      App_G.MsgBox.error_digbox();
                  }
              });
        }
    });



    //隐藏
    $("[name=hide]").digbox({
        Title: "提示信息",
        Selector: "#tbd_cgty",
        Context: "确定隐藏该分类吗？",
        CallBack: function (btn_id, current_obj, panel) {

            $get("/ShopSetting/SetEnableGuideProductCgty?id=" + current_obj.parent().attr("data-id") + "&isshow=false", "",
              function (data) {
                  if (data.Code == App_G.Code.Code_200) {
                      search();
                      App_G.MsgBox.success_digbox();
                  } else {
                      App_G.MsgBox.error_digbox();
                  }
              });

        }
    });


    //删除
    $("[name=delete]").digbox({
        Title: "提示信息",
        Selector: "#tbd_cgty",
        Context: "确定删除该分类吗？",
        CallBack: function (btn_id, current_obj, panel) {

            $get("/ShopSetting/DeleteGuideProductCgty?id=" + current_obj.parent().attr("data-id"), "",
              function (data) {
                  if (data.Code == App_G.Code.Code_200) {
                      search();
                      App_G.MsgBox.success_digbox();
                  } else if (data.State == App_G.BackState.State_1) {
                      App_G.MsgBox.error_digbox("该导购分类下存在商品");
                  } else {
                      App_G.MsgBox.error_digbox();
                  }

              });

        }
    });


    //关闭全部
    $("#close_all").click(function () {
        $("tr[pid=tr_0]").find(".icon-minus").attr("class", "icon-plus");
        $("#tbd_cgty").find("tr[pid!=tr_0]").hide();
    });

    //展开全部
    $("#open_all").click(function () {
        $("tr[pid=tr_0]").find(".icon-plus").attr("class", "icon-minus");;
        $("#tbd_cgty").find("tr[pid!=tr_0]").show();
    });


    $("#tbd_cgty").find(".icon-minus").click(function () {
        if ($(this).hasClass("icon-plus")) {
            $(this).attr("class", "icon-minus");
            showTr($(this).parent().parent().parent());
        } else {
            $(this).attr("class", "icon-plus");
            hideTr($(this).parent().parent().parent());
        }

    });

});

//查询数据
function search() {
    //加载数据
    $getByAsync("/ShopSetting/GetGuideProductCgtys?rnd=" + Math.random(), "",
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