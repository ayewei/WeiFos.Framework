
$(function () {

    //初始化权限编号
    App_G.Auth.InitID();
    //初始化验证插件
    yw.valid.config({});

    //加载数据
    getData();

    //添加扩展属性名称 以及值
    $("#add_attr").digbox({
        Selector: ".m_opBar.mt10",
        Title: "添加属性",
        Context: tmp,
        Show: function (btn_id, current_obj, panel) {
             
            var v = yw.valid.getValidate({
                submitelements: $("#" + btn_id), vsuccess: function () {
                    var datas = {
                        extAttrName: App_G.Util.getJson("data-val", { product_type_id: App_G.Util.getRequestId('bid') }),
                        attrvals: panel.find("#attr_values").val()
                    };

                    $postByAsync("/ProductModule/SKU/SaveExtAttrName", JSON.stringify(datas),
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

            v.valid("#name", { focusmsg: "属性名称长度在1~15字符", errormsg: "属性名称长度在1~15字符", vtype: verifyType.anyCharacter });
            v.valid("#order_index", { focusmsg: "请输入排序索引", errormsg: "排序索引只能为数字", vtype: verifyType.isNumber });
            v.valid("#attr_values", { focusmsg: "属性值可用“,”号隔开", errormsg: "属性值可用“,”号隔开", vtype: verifyType.anyCharacter });
        },
        CallBack: function () {
            return false;
        }
    });

    //修改扩展属性
    $("a[name=update_btn_n]").digbox({
        Selector: "#listTable",
        Title: "修改属性",
        Context: tmp_u,
        Show: function (btn_id, current_obj, panel) {

            var data = {
                id: current_obj.parent("td").attr("data-id"), is_mchoice: current_obj.parents("tr").find("td:eq(2) > a").hasClass("i_select"),
                order_index: current_obj.parents("tr").find("td:eq(0)").text(), name: current_obj.parents("tr").find("td:eq(1)").text()
            };

            App_G.Util.bindJson("data-val", data);

            var v = yw.valid.getValidate({
                submitelements: $("#" + btn_id), vsuccess: function () {
                    var datas = {
                        extAttrName: App_G.Util.getJson("data-val", { id: current_obj.parent("td").attr("data-id"), product_type_id: App_G.Util.getRequestId('bid') }),
                        attrvals: panel.find("#attr_values").val()
                    };

                    $postByAsync("/ProductModule/SKU/SaveExtAttrName", JSON.stringify(datas),
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

            //属性值名称
            v.valid("#name", { focusmsg: "属性名称长度在1~15字符", errormsg: "属性名称长度在1~15字符", vtype: verifyType.anyCharacter });

            //排序索引
            v.valid("#order_index", { focusmsg: "请输入排序索引，只能够是数字", errormsg: "排序索引，只能够是数字", vtype: verifyType.isNumber });


        },
        CallBack: function () {
            return false;
        }
    });
    
    //删除属性名
    $("[name=delete_btn_n]").digbox({
        Selector: "#listTable",
        Title: "提示信息",
        Context: "确定删除数据吗？",
        CallBack: function (btn_id, current_obj, panel) {
            var td = current_obj.parent();
            
            $get("/ProductModule/SKU/DeleteExtAttrName?bid=" + td.attr("data-id"), "",
                function (data) {
  ;
                    if (data.Code == App_G.Code.Code_200) {
                        current_obj.parent().parent().remove();
                        App_G.MsgBox.success_digbox();
                        panel.modal('hide');
                    } else if (data.State == App_G.BackState.State_1) {
                        App_G.MsgBox.error_digbox("该属性存在商品数据在使用！");
                    } else {
                        App_G.MsgBox.error_digbox("操作失败！");
                    }
                });
        }
    });


    //属性值修改
    $("a[name=update_btn]").digbox({
        Selector: "#listTable",
        Title: "修改属性值",
        Context: tmp_v,
        Show: function (btn_id, current_obj, panel) {

            var v = yw.valid.getValidate({
                submitelements: $("#" + btn_id), vsuccess: function () {

                    var data = {
                        extAttrVal: {
                            id: current_obj.parent().parent().attr("data-id"), product_type_id: App_G.Util.getRequestId('bid'),
                            ext_attr_name_id: current_obj.parent().parent().attr("data-extid"), val: $("#val").val()
                        }
                    };

                    $postByAsync("/ProductModule/SKU/SaveExtAttrVal?bid=" + App_G.Util.getRequestId('bid'), JSON.stringify(data),
                         function (data) {
           
                             if (data.Code == App_G.Code.Code_200) {
                                 current_obj.parent().parent().find("span").text($("#val").val());
                                 panel.modal('hide');
                             } else if (data.State == 301) {
                                 App_G.MsgBox.error_digbox(data.Message);
                             }
                             else {
                                 App_G.MsgBox.error_digbox("操作失败！");
                             }
                         });

                }
            });

            panel.find("#val").val(current_obj.parent().parent().find("span").text());

            //属性值名称
            v.valid("#val", { focusmsg: "请输入属性值长度在15字符内", errormsg: "请输入属性值长度在15字符内", vtype: verifyType.anyCharacter });

        },
        CallBack: function (e) {
            return false;
        }
    });

    //删除属性值
    $("[name=delete_btn]").digbox({
        Selector: "#listTable",
        Title: "提示信息",
        Context: "确定删除数据吗？",
        CallBack: function (btn_id, current_obj, panel) {
            var li = current_obj.parent().parent();
             
            $get("/ProductModule/SKU/DeleteExtAttrVal?bid=" + li.attr("data-id"), "",
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

    //添加属性值
    $("a[name=add_btn_n]").digbox({
        Selector: "#listTable",
        Title: "添加属性值",
        Context: tmp_v,
        Show: function (btn_id, current_obj, panel) {

            var v = yw.valid.getValidate({
                submitelements: $("#" + btn_id), vsuccess: function () {

                    var data = {
                        extAttrVal: {
                            product_type_id: App_G.Util.getRequestId('bid'),
                            ext_attr_name_id: current_obj.parents("td").attr("data-id"), val: $("#val").val()
                        }
                    };

                    $postByAsync("/ProductModule/SKU/SaveExtAttrVal", JSON.stringify(data),
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

            //属性值名称
            v.valid("#val", { focusmsg: "请输入属性值长度在15字符内", errormsg: "请输入属性值长度在15字符内", vtype: verifyType.anyCharacter });

        },
        CallBack: function (e) {
            return false;
        }
    });


    $("#BackBtn").click(function () {
        window.location.href = "/ProductModule/SKU/AttrName?bid=" + App_G.Util.getRequestId('bid');
    });


    $("#NextBtn").click(function () {
        window.location.href = "/ProductModule/SKU/SpecName?bid=" + App_G.Util.getRequestId('bid');
    });

});


//扩展属性名称模板
var tmp = '<div style="height:200px;width:640px;"  >\
                     <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table_s1">\
                     <tr>\
                     <th scope="row">属性名称：</th>\
                     <td> <input class="forminput" type="text" id="name" data-val="name" maxlength="30" /> </td>\
                     </tr>\
                     <tr>\
                     <th scope="row">是否多选：</th>\
                     <td><input id="is_mchoice" type="checkbox"  data-val="is_mchoice" />(有些属性是可以选择多个属性值的，如“适合人群”，就可能既适合老年人也适合中年人)\
                     </td>\
                     </tr>\
                     <tr>\
                     <th scope="row">排序索引：</th>\
                     <td><input class="forminput" type="text" id="order_index" data-val="order_index" maxlength="7" style="width:50px;" />\
                     </td>\
                     </tr>\
                     <tr>\
                     <th scope="row">属性值：</th>\
                     <td><input class="forminpu"t type="text" id="attr_values"  maxlength="100" style="width:300px;"  /><br>扩展属性的值，多个属性值可用“,”号隔开，每个值的字符数最多15个字符\
                     </td>\
                     </tr></table>\
                     </div>';


//属性名称修改时 使用
var tmp_u = '<div style="height:150px;width:640px;" >\
             <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table_s1">\
             <tr>\
             <th scope=row>属性名称：</th>\
             <td> <input class="forminput" type=text id="name" data-val="name" maxlength="30" /> </td>\
             </tr>\
             <tr>\
             <th scope="row">是否多选：</th>\
             <td><input id="is_mchoice" type="checkbox"  data-val="is_mchoice" />(有些属性是可以选择多个属性值的，如“适合人群”，就可能既适合老年人也适合中年人)\
             </td>\
             </tr>\
             <tr>\
             <th scope=row>排序索引：</th>\
             <td><input class="forminput" type="text" id="order_index" data-val="order_index" maxlength="7" style="width:50px;" />\
             </td>\
             </tr>\
             </table>\
         </div>';


//属性值模板
var tmp_v = '<div style="height:60px;width:640px;"  >\
                      <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table_s1" >\
                      <tr>\
                      <th scope="row">属性值：</th>\
                      <td> <input class="forminput" type="text" id="val" data-val="val" maxlength="15" /> </td>\
                      </tr>\
                      <tr></table>\
                 </div>';


//获取数据
function getData() {
    $getByAsync("/ProductModule/SKU/GetExtAttrNames?bid=" + App_G.Util.getRequestId('bid'), "",
        function (backdata) {
            if ($("#listTable tbody").find("tr").length > 0) {
                $("#listTable tbody tr").remove();
            }

            $("#listTable tbody").append(template("template", backdata));
        });
}
