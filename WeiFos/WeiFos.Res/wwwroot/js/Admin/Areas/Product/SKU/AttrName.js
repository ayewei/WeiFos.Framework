
$(function () {
    //初始化权限编号
    App_G.Auth.InitID();
    //加载数据
    getData();

    //新增
    $("#add_attr").digbox({
        Selector: ".m_opBar.mt10",
        Title: "添加基本属性",
        Context: tmp,
        Show: function (btn_id, current_obj, panel) {
            submit($("#" + btn_id), panel, current_obj, "save");
        },
        CallBack: function (e) {
            return false;
        }
    });


    //修改
    $("a[name=update_btn]").digbox({
        Selector: "#listTable",
        Title: "修改属性",
        Context: tmp,
        Show: function (btn_id, current_obj, panel) {

            var data = {
                order_index: current_obj.parents("tr").find("td:eq(0)").text(),
                name: current_obj.parents("tr").find("td:eq(1)").text(),
                show_list: current_obj.parents("tr").find("td:eq(2) > a").hasClass("i_select")
            };

            App_G.Util.bindJson("data-val", data);

            submit($("#" + btn_id), panel, current_obj, "");
        },
        CallBack: function (e) {
            return false;
        }
    });


    //删除自定义属性
    $("[name=deleteBtn]").digbox({
        Selector: "#listTable",
        Title: "提示信息",
        Context: "确定删除数据吗？",
        CallBack: function (btn_id, current_obj, panel) {
            var td = current_obj.parent();
             
            $get("/ProductModule/SKU/DeleteAttrName?bid=" + td.attr("data-id"), "",
                function (data) {
  
                    if (data.Code == App_G.Code.Code_200) {
                        current_obj.parent().parent().remove();
                        App_G.MsgBox.success_digbox();
                    } else if (data.State == App_G.BackState.State_1) {
                        App_G.MsgBox.error_digbox("该属性存在商品数据在使用！");
                    } else {
                        App_G.MsgBox.error_digbox("操作失败！");
                    }
                });
        }
    });


    $("#BackBtn").click(function () {
        window.location.href = "/ProductModule/SKU/ProductTypeForm?bid=" + App_G.Util.getRequestId('bid');
    });


    $("#NextBtn").click(function () {
        window.location.href = "/ProductModule/SKU/ExtAttrName?bid=" + App_G.Util.getRequestId('bid');
    });

});


//模板
var tmp = "<div id = 'popup' style='height:160px;width:640px;'  >"
            + "<table width='100%' border='0' cellpadding='0' cellspacing='0' class='table_s1'>"
            + "<tr>"
            + "<th scope='row'>属性名称：</th>"
            + "<td> <input class='forminput' type='text' id='name' data-val='name' maxlength='30' /> </td>"
            + "</tr>"
            + "<tr>"
            + "<th scope='row'>列表显示：</th>"
            + "<td><input id='show_list' type='checkbox'  data-val='show_list' /> 是"
            + "</td>"
            + "</tr>"
            + "<tr>"
            + "<th scope='row'>排序索引：</th>"
            + "<td><input class='forminput' type='text' id='order_index' data-val='order_index' maxlength='9' style='width:50px' />"
            + "</td>"
            + "</tr></table>"
        + "</div>";


//提交数据
function submit(btn, panel, current_obj, type) {

    //初始化验证插件
    yw.valid.config({
        submitelements: btn, vsuccess: function () {

            var data = {};

            if (type == "save") {
                data = {
                    attrName: App_G.Util.getJson("data-val", { product_type_id: App_G.Util.getRequestId('bid') })
                };
            } else {
                data = {
                    attrName: App_G.Util.getJson("data-val", { id: current_obj.parent().attr("data-id"), product_type_id: App_G.Util.getRequestId('bid') })
                };
            }

            $postByAsync("/ProductModule/SKU/SaveAttrName", JSON.stringify(data),
                 function (data) {
   
                     if (data.Code == App_G.Code.Code_200) {
                         App_G.MsgBox.success_digbox();
                         setTimeout("window.location.reload();", 1000);
                         panel.modal('hide');
                     } else {
                         App_G.MsgBox.error_digbox("操作失败！");
                     }
                 });
            
        }
    });


    var v = yw.valid.getValidate();
    v.valid("#name", { focusmsg: "属性名称长度在1~15字符", errormsg: "属性名称长度在1~15字符", vtype: verifyType.anyCharacter });
    v.valid("#order_index", { focusmsg: "请输入排序索引", errormsg: "排序索引只能为数字", vtype: verifyType.isNumber });

}


//获取数据
function getData() {
    $getByAsync("/ProductModule/SKU/GetAttrNames?bid=" + App_G.Util.getRequestId('bid'), "",
        function (backdata) {
            if ($("#listTable tbody").find("tr").length > 0) {
                $("#listTable tbody tr").remove();
            }
            $("#listTable tbody").append(template("template", backdata));
        });
}