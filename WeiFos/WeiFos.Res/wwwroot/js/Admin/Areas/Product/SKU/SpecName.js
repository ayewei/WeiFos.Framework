

$(function () {

    //加载数据
    getData();

    //添加规格
    $("#add_attr").digbox({
        Selector: ".m_opBar.mt10",
        Title: "添加规格",
        Context: tmp,
        Show: function (btn_id, current_obj, panel) {

            var v = yw.valid.getValidate({
                submitelements: $("#" + btn_id), vsuccess: function () {
                    var data = {
                        specName: App_G.Util.getJson("data-val", { product_type_id: App_G.Util.getRequestId('bid') }),
                        specValues: panel.find("#spec_values").val()
                    };

                    $postByAsync("/ProductModule/SKU/SaveSpecName", JSON.stringify(data),
                         function (data) {
           
                             if (data.Code == App_G.Code.Code_200) {
                                 App_G.MsgBox.success_digbox();
                                 setTimeout("window.location.reload();", 1000);
                                 current_obj.modal("hide");
                             } else {
                                 App_G.MsgBox.error_digbox("操作失败！");
                             }
                         });

                }
            });

            //规格名称
            v.valid("#name", { focusmsg: "规格名称长度在1~15字符", errormsg: "规格名称长度在1~15字符", vtype: verifyType.anyCharacter });
            //排序索引
            v.valid("#order_index", { focusmsg: "请输入排序索引，只能够是数字", errormsg: "排序索引，只能够是数字", vtype: verifyType.isNumber });
            //规格值
            v.valid("#spec_values", { focusmsg: "规格值可用“,”号隔开", errormsg: "规格值可用“,”号隔开", vtype: verifyType.anyCharacter });

        },
        CallBack: function (e) {
            return false;
        }
    });

    //修改规格
    $("a[name=update_btn_n]").digbox({
        Selector: "#listTable",
        Title: "修改规格",
        Context: tmp_u,
        Show: function (btn_id, current_obj, panel) {

            var data = {
                ID: current_obj.parent("td").attr("data-id"), IsMchoice: current_obj.parents("tr").find("td:eq(2) > a").hasClass("i_select"),
                order_index: current_obj.parents("tr").find("td:eq(0)").text(), name: current_obj.parents("tr").find("td:eq(1)").text()
            };

            App_G.Util.bindJson("data-val", data);

            var v = yw.valid.getValidate({
                submitelements: $("#" + btn_id), vsuccess: function () {
 
                    var datas = {
                        specName: App_G.Util.getJson("data-val", { ID: current_obj.parent().attr("data-id"), product_type_id: App_G.Util.getRequestId('bid') })
                    };

                    $postByAsync("/ProductModule/SKU/SaveSpecName", JSON.stringify(datas),
                         function (data) {
           
                             if (data.Code == App_G.Code.Code_200) {
                                 App_G.MsgBox.success_digbox();

                                 current_obj.parents("tr").find("td:eq(0)").text(datas.specName.order_index);
                                 current_obj.parents("tr").find("td:eq(1)").text(datas.specName.name);

                                 panel.modal('hide');

                             } else {
                                 App_G.MsgBox.error_digbox("操作失败！");
                             }
                         });

                }
            });

            //属性值名称
            v.valid("#name", { focusmsg: "规格名称长度在1~15字符", errormsg: "规格名称长度在1~15字符", vtype: verifyType.anyCharacter });

            //排序索引
            v.valid("#order_index", { focusmsg: "请输入排序索引，只能够是数字", errormsg: "排序索引，只能够是数字", vtype: verifyType.isNumber });
 
        },
        CallBack: function (e) {
            return false;
        }
    });

    //删除规格
    $("[name=delete_btn_n]").digbox({
        Selector: "#listTable",
        Title: "提示信息",
        Context: "确定删除数据吗？",
        CallBack: function (btn_id, current_obj, panel) {
            var td = current_obj.parent();

            $get("/ProductModule/SKU/DeleteSpecName?bid=" + td.attr("data-id"), "",
                function (data) {
  
                    if (data.Code == App_G.Code.Code_200) {
                        current_obj.parent().parent().remove();
                        App_G.MsgBox.success_digbox();
                    } else if (data.State == 503) {
                        App_G.MsgBox.error_digbox("该属性存在商品数据在使用！");
                    } else {
                        App_G.MsgBox.error_digbox("操作失败！");
                    }
                });
        }
    });

    //规格值修改
    $("a[name=update_btn]").digbox({
        Selector: "#listTable",
        Title: "修改规格值",
        Context: tmp_v,
        Show: function (btn_id, current_obj, panel) {

            panel.find("#val").val(current_obj.parent().parent().find("span").text());

            var v = yw.valid.getValidate({
                submitelements: $("#" + btn_id), vsuccess: function () {

                    var data = {
                        specValue: {
                            id: current_obj.parent().parent().attr("data-id"), product_type_id: App_G.Util.getRequestId('bid'),
                            specname_id: current_obj.parent().parent().attr("data-spid"), val: $("#val").val()
                        }
                    };

                    $postByAsync("/ProductModule/SKU/SaveSpecValue", JSON.stringify(data),
                         function (data) {
           
                             if (data.Code == App_G.Code.Code_200) {
                                 current_obj.parent().parent().find("span").text($("#val").val());
                                 panel.modal('hide');
                             } else if (data.State == App_G.BackState.State_1) {
                                 App_G.MsgBox.error_digbox("操作失败！");
                             } else {
                                 App_G.MsgBox.error_digbox("操作失败！");
                             }
                         });

                }
            });


            //排序索引
            v.valid("#val", { focusmsg: "请输入属性值长度在15字符内", errormsg: "属性值长度在15字符内", vtype: verifyType.anyCharacter });

        },
        CallBack: function (e) {
            return false;
        }
    });

    //删除属性值
    $("a[name=delete_btn]").digbox({
        Selector: "#listTable",
        Title: "提示信息",
        Context: "确定删除数据吗？",
        CallBack: function (btn_id, current_obj, panel) {
            var li = current_obj.parent().parent();

            $get("/ProductModule/SKU/DeleteSpecValue?bid=" + li.attr("data-id"), "",
                function (data) {
  
                    if (data.Code == App_G.Code.Code_200) {
                        current_obj.parent().parent().remove();
                        App_G.MsgBox.success_digbox();
                    } else if (data.State == 503) {
                        App_G.MsgBox.error_digbox(data.Message);
                    } else {
                        App_G.MsgBox.error_digbox("操作失败！");
                    }
                });
        }
    });

    //添加属性值
    $("a[name=add_btn_n]").digbox({
        Selector: "#listTable",
        Title: "添加属性值",
        Context: tmp_v,
        Show: function (btn_id, current_obj, panel) {

            var v = yw.valid.getValidate({
                submitelements: $("#" + btn_id), vsuccess: function () {

                    var data = {
                        specValue: {
                            product_type_id: App_G.Util.getRequestId('bid'),
                            specname_id: current_obj.parent().attr("data-id"), Val: $("#val").val()
                        }
                    };

                    $postByAsync("/ProductModule/SKU/SaveSpecValue", JSON.stringify(data),
                         function (data) {
           
                             if (data.Code == App_G.Code.Code_200) {
                                 panel.modal('hide');
                                 setTimeout("window.location.reload();", 1000);
                             } else {
                                 App_G.MsgBox.error_digbox("操作失败！");
                             }
                         });

                }
            });

            //排序索引
            v.valid("#val", { focusmsg: "请输入属性值长度在15字符内", errormsg: "属性值长度在15字符内", vtype: verifyType.anyCharacter });

        },
        CallBack: function (e) {
            return false;
        }
    });


    $("#BackBtn").click(function () {
        window.location.href = "/ProductModule/SKU/ExtAttrName?bid=" + App_G.Util.getRequestId('bid');
    });


    $("#NextBtn").click(function () {
        window.location.href = "/ProductModule/SKU/ProductTypeManage";
    });

});


//扩展属性名称模板
var tmp = "<div style='height:150px;width:640px;'  >"
            + "<table width='100%' border='0' cellpadding='0' cellspacing='0' class='table_s1'>"
            + "<tr>"
            + "<th scope='row'>规格名称：</th>"
            + "<td> <input class='forminput' type='text' id='name' data-val='name' maxlength='30' /> </td>"
            + "</tr>"
            + "<tr>"
            + "<th scope='row'>排序索引：</th>"
            + "<td><input class='forminput' type='text' id='order_index' data-val='order_index' maxlength='7' style='width:50px;' />"
            + "</td>"
            + "</tr>"
            + "<tr>"
            + "<th scope='row'>规格值：</th>"
            + "<td><input class='forminput' type='text' id='spec_values'  maxlength='100' style='width:300px;'  /><br>多个规格值可用“,”号隔开，每个值的字符数最多15个字符"
            + "</td>"
            + "</tr></table>"
        + "</div>";


//属性名称修改时 使用
var tmp_u = "<div style='height:100px;width:640px;'  >"
    + "<table width='100%' border='0' cellpadding='0' cellspacing='0' class='table_s1'>"
    + "<tr>"
    + "<th scope='row'>规格名称：</th>"
    + "<td> <input class='forminput' type='text' id='name' data-val='name' maxlength='30' /> </td>"
    + "</tr>"
    + "<tr>"
    + "<th scope='row'>排序索引：</th>"
    + "<td><input class='forminput' type='text' id='order_index' data-val='order_index' maxlength='7' style='width:50px;' />"
    + "</td>"
    + "</tr>"
    + "</table>"
+ "</div>";


//属性值模板
var tmp_v = "<div style='height:60px;width:640px;'  >"
            + "<table width='100%' border='0' cellpadding='0' cellspacing='0' class='table_s1'>"
            + "<tr>"
            + "<th scope='row'>属性值：</th>"
            + "<td> <input class='forminput' type='text' id='val' data-val='Val' maxlength='15' /> </td>"
            + "</tr>"
            + "<tr></table>"
        + "</div>";


//获取数据
function getData() {
    $getByAsync("/ProductModule/SKU/GetSpecNames?bid=" + App_G.Util.getRequestId('bid'), "",
        function (backdata) {
            if ($("#listTable tbody").find("tr").length > 0) {
                $("#listTable tbody tr").remove();
            }

            $("#listTable tbody").append(template("template", backdata));
        });
}
