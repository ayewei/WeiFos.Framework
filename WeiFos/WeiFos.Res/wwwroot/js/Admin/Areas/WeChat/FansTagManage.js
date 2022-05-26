
var pager, v;
 
//获取查询数据
function getSerachData() {
    return { };
}

$(function () {

    //初始化权限编号
    //App_G.Auth.InitID();

    pager = $("#PagerDiv").pager({
        url: App_G.Util.getDomain() + '/Fans/GetFansTags',
        data: getSerachData(),
        currentPage: 0,
        pageSize: [15, 50, 100],
        callback: function (data) {
            if ($("#listTable tbody").find("tr").length > 0) {
                $("#listTable tbody tr").remove();
            }
            $("#listTable tbody").append(template("template", data)); 
        }
    });

    $("#search_btn").click(function () {
        pager.execute(getSerachData());
    });
     

    //初始化验证插件
    yw.valid.config({});


    //修改
    $("[name=edit_btn]").digbox({
        Selector: "#data_tbody",
        Title: "信息提示框",
        Context: tmp,
        Show: function (submit_btnid, current, panel) {

            panel.find("#tag_name").val(current.parents("tr").find("td:eq(0)").text());

            v = yw.valid.getValidate({
                submitelements: $("#" + submit_btnid), vsuccess: function () {

                    $("#" + submit_btnid).setDisable({ text: "保 存 中..." });
                    var data = { fans_tag: { id: current.parent().attr("data-id"), tag_name: panel.find("#tag_name").val() } };
                    $postByAsync("/Fans/SaveFanTags", JSON.stringify(data),

                      function (data) {
                          if (data.Code == App_G.Code.Code_200) {
                              App_G.MsgBox.success_digbox();
                              pager.execute(getSerachData());
                              panel.modal('hide');
                          } else {
                              App_G.MsgBox.error_digbox();
                          }
                      });

                }
            });
            v.valid("#tag_name", { selector: "div.modal-body", focusmsg: "属性名称长度在1~15字符", errormsg: "属性名称长度在1~15字符", vtype: verifyType.anyCharacter });
        },
        CallBack: function (submit_btnid, current, panel) {
            return false;
        }
    });

    //添加
    $("#add_btn").digbox({
        Selector: "div.box",
        Title: "信息提示框",
        Context: tmp,
        Show: function (submit_btnid, current, panel) {

            v = yw.valid.getValidate({
                submitelements: $("#" + submit_btnid), vsuccess: function () {

                    $("#" + submit_btnid).setDisable({ text: "保 存 中..." });
                    var data = { fans_tag: { tag_name: panel.find("#tag_name").val() } };
                    $post("/Fans/SaveFanTags", JSON.stringify(data),

                      function (data) {
                          if (data.Code == App_G.Code.Code_200) {
                              App_G.MsgBox.success_digbox();
                              pager.execute(getSerachData());
                              panel.modal('hide');
                          } else {
                              App_G.MsgBox.error_digbox();
                          }
                      });

                }
            });
            v.valid("#tag_name", { selector: "div.modal-body", focusmsg: "属性名称长度在1~15字符", errormsg: "属性名称长度在1~15字符", vtype: verifyType.anyCharacter });
        },
        CallBack: function (submit_btnid, current, panel) {
            return false;
        }
    });


    //删除
    $("[name=delete_btn]").digbox({
        Selector: "#data_tbody",
        Title: "信息提示框",
        Context: "删除以后不可恢复，确定彻底删除吗？",
        CallBack: function (submit_btnid, current, panel) {

            $postByAsync("/Fans/DeleteFansTag?bid=" + current.parent().attr("data-id"), "",

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
     
}); 
 
   

var tmp = "<div id = 'popup' style='height:60px;width:640px;'  >"
                    + "<table width='100%' border='0' cellpadding='0' cellspacing='0' class='table_s1'>"
                    + "<tr>"
                    + "<th scope='row'>标签名称：</th>"
                    + "<td> <input class='forminput' type='text' id='tag_name' data-val='tag_name' maxlength='30' /> </td>"
                    + "</tr></table>"
               + "</div>";