
/*!
 * 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
 * Copyright (c) 2013-2018 深圳微狐信息技术有限公司
 * 创 建：
 * 日 期：2018-12-05 11:53:51
 * 描 述：公司管理页脚本
 */
var datagrid, v = null, v1 = null, diglog;

$(function () {

    App_G.Auth.InitID();

    //表格初始化
    datagrid = $("[name=datagrid]").datagrid({
        url: "/SKUModule/SKU/GetExtAttrNames?bid=" + App_G.Util.getRequestId('bid'),
        data: getSearchData(),
        template_id: "template",
        column: [
            {
                text: "排序",
                style: "width: 40px; "
            },
            {
                text: "属性名称",
                style: "width: 140px; "
            },
            {
                text: "支持多选",
                style: "width: 140px; "
            },
            {
                text: "属性值",
                style: "width: 140px; "
            }
        ],
        dblclick: function (tr) {
            $("<div></div>").digbox({
                IsAuto: true,
                Title: "提示信息",
                Content: tmp_u,
                Before: function () {
                    var tr = datagrid.getSeleteTr();
                    if (tr.length == 0) {
                        layer.msg('请选择操作的行', { icon: 2 });
                        return false;
                    }
                    return true;
                },
                Show: function (b, c, p) {

                    var data = {
                        order_index: datagrid.getSeleteTr().find("td:eq(0)").text(),
                        name: datagrid.getSeleteTr().find("td:eq(1)").text(),
                        is_mchoice: datagrid.getSeleteTr().find("td:eq(2) > a").hasClass("i_select")
                    };

                    //绑定数据
                    App_G.Mapping.Bind("data-val", data, p);
                    //绑定验证
                    submit($("#" + b), p, c, "edit");
                },
                CallBack: function (s, c, p) {
                    return v.valid.executeGroup();
                }
            });
        },
        callback: function (result) {
            if (result.Code == App_G.Code.Code_200) {
                trlist = result.Data.pageData;
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

    //新增扩展属性名
    $("a[name=add_btn]").digbox({
        Title: "添加扩展属性",
        Content: tmp,
        Show: function (s, c, p) {
            submit($("#" + s), p, c, "save");
        },
        CallBack: function (e) {
            return v.valid.execute();
        }
    });

    //修改扩展属性名
    $("a[name=edit_btn]").digbox({
        Title: "提示信息",
        Content: tmp_u,
        Before: function () {
            var tr = datagrid.getSeleteTr();
            if (tr.length == 0) {
                layer.msg('请选择操作的行', { icon: 2 });
                return false;
            }
            return true;
        },
        Show: function (b, c, p) {

            var data = {
                order_index: datagrid.getSeleteTr().find("td:eq(0)").text(),
                name: datagrid.getSeleteTr().find("td:eq(1)").text(),
                is_mchoice: datagrid.getSeleteTr().find("td:eq(2) > a").hasClass("i_select")
            };

            App_G.Mapping.Bind("data-val", data, p);

            submit($("#" + b), p, c, "edit");
        },
        CallBack: function (s, c, p) {
            return v1.valid.execute();
        }
    });

    //删除扩展属性名
    $("[name=delete_btn]").digbox({
        Title: "提示信息",
        Content: "确认删除该数据吗",
        Before: function () {
            var tr = datagrid.getSeleteTr();
            if (tr.length == 0) {
                layer.msg('请选择操作的行', { icon: 2 });
                return false;
            }
            return true;
        },
        CallBack: function (s, c, p) {
            $post("/SKUModule/SKU/DeleteExtAttrName?bid=" + datagrid.getSeleteTr().attr("data-id"), "",
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



    //添加扩展属性值
    $("a[name=add_btn_n]").digbox({
        Selector: "a[name=add_btn_n]",
        Context: "[name=datagrid]",
        Title: "添加属性值",
        Content: tmp_v,
        Show: function (b, c, p) {
            //绑定验证提交
            submit_val(c, p);
        },
        CallBack: function (e) {
            return v1.valid.execute();
        }
    });

    //修改扩展属性值
    $("a[name=update_btn]").digbox({
        Selector: "a[name=update_btn]",
        Context: "[name=datagrid]",
        Title: "修改属性值",
        Content: tmp_v,
        Show: function (b, c, p) {
            p.find("#val").val(c.parent().parent().find("span").text());
            //绑定验证提交
            submit_val(c, p);
        },
        CallBack: function (e) {
            return v1.valid.execute();
        }
    });

    //删除扩展属性值
    $("a[name=delete_btn_n]").digbox({
        Selector: "a[name=delete_btn_n]",
        Context: "[name=datagrid]",
        Title: "提示信息",
        Content: "确定删除数据吗？",
        CallBack: function (b, c, p) {
            var li = c.parent().parent();

            $get("/SKUModule/SKU/DeleteExtAttrVal?bid=" + li.attr("data-id"), "",
                function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        c.parent().parent().remove();
                        layer.msg(result.Message, { icon: 1 });
                    } else if (result.State == 1) {
                        layer.msg("该属性存在商品数据在使用！", { icon: 2 });
                    } else {
                        layer.msg(result.Message, { icon: 2 });
                    }
                });
        }
    });


    //下一步
    $("#NextBtn").click(function () {
        window.location.href = "/SKUModule/SKU/SpecName?bid=" + App_G.Util.getRequestId('bid');
    });

});


$("#NextBtn").click(function () {
    window.location.href = "/SKUModule/SKU/SpecName?bid=" + App_G.Util.getRequestId('bid');
});


//查询数据
function getSearchData() {
    return { keyword: $("#keyword").val() };
}

//提交数据
function submit(btn, p, c, type) {

    diglog = p;

    //初始化验证插件
    yw.valid.config({});

    //获取验证类型
    v = yw.valid.getValidate({

        vsuccess: function () {
            $post("/SKUModule/SKU/SaveExtAttrName", JSON.stringify(GetData()),
                function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        layer.msg(result.Message, { icon: 1 });
                        datagrid.execute(getSearchData());
                        p.modal('hide');
                    }
                    else {
                        layer.msg(result.Message, { icon: 2 });
                    }
                });

        }
    });

    //商品类型名称
    v.valid("#name", {
        selector: p,
        vtype: verifyType.anyCharacter,
        focus: { msg: "属性名称长度在1~15字符" },
        blur: { msg: "属性名称长度在1~15字符" }
    });

    //排序索引
    v.valid("#order_index", {
        selector: p,
        vtype: verifyType.isNumber,
        focus: { msg: "请输入排序索引" },
        blur: { msg: "排序索引只能为数字" }
    });

    if (type == "save") {
        //扩展属性值
        v.valid("#attr_values", {
            selector: p,
            vtype: verifyType.anyCharacter,
            focus: { msg: "属性值可用“,”号隔开" },
            blur: { msg: "属性值可用“,”号隔开" }
        });
    }

}

//提交值
function submit_val(c, p) {

    diglog = p;

    //初始化验证插件
    yw.valid.config({});

    //获取验证类型
    v1 = yw.valid.getValidate({

        vsuccess: function () {
            $post("/SKUModule/SKU/SaveExtAttrVal", JSON.stringify(GetValData(c)),
                function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        layer.msg(result.Message, { icon: 1 });
                        datagrid.execute(getSearchData());
                        p.modal('hide');
                    }
                    else {
                        layer.msg(result.Message, { icon: 2 });
                    }
                });

        }
    });

    //商品类型名称
    v1.valid("#val", {
        selector: p,
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入属性值长度在1~15字符" },
        blur: { msg: "请输入属性值长度在1~15字符" }
    });

}


//获取提交数据
function GetData() {
    var id = 0, tr = datagrid.getSeleteTr();
    if (tr.length > 0) {
        id = datagrid.getSeleteTr().attr("data-id");
    }

    return {
        extAttrName: App_G.Mapping.Get("data-val", { id: id, product_type_id: App_G.Util.getRequestId('bid') }, diglog),
        attrvals: diglog.find("#attr_values").val()
    };
}


//获取Val数据
function GetValData(c) {
    var id = 0, tr = datagrid.getSeleteTr();
    if (tr.length > 0) {
        id = datagrid.getSeleteTr().attr("data-id");
    }

    return {
        extAttrVal: {
            id: c.parent().parent().attr("data-id"), product_type_id: App_G.Util.getRequestId('bid'),
            ext_attr_name_id: c.parent().parent().attr("data-extid"), val: diglog.find("#val").val()
        }
    };

}


//扩展属性名称模板
var tmp = '<div style="height:200px;width:640px;"  >\
                     <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table_s1">\
                     <tr>\
                     <th scope="row">属性名称：</th>\
                     <td> <input class="form-control form-control-sm" type="text" id="name" data-val="name" maxlength="30" /> </td>\
                     </tr>\
                     <tr>\
                     <th scope="row">是否多选：</th>\
                     <td><input id="is_mchoice" type="checkbox"  data-val="is_mchoice" />(有些属性是可以选择多个属性值的，如“适合人群”，就可能既适合老年人也适合中年人)\
                     </td>\
                     </tr>\
                     <tr>\
                     <th scope="row">排序索引：</th>\
                     <td><input class="form-control form-control-sm"  type="text" id="order_index" data-val="order_index" maxlength="7" style="width:80px;" />\
                     </td>\
                     </tr>\
                     <tr>\
                     <th scope="row">属性值：</th>\
                     <td><input class="form-control form-control-sm"  type="text" id="attr_values"  maxlength="100" style="width:300px;"  /><br>扩展属性的值，多个属性值可用“,”号隔开，每个值的字符数最多15个字符\
                     </td>\
                     </tr></table>\
                     </div>';


//属性名称修改时 使用
var tmp_u = '<div style="height:150px;width:640px;" >\
             <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table_s1">\
             <tr>\
             <th scope=row>属性名称：</th>\
             <td> <input class="form-control form-control-sm"  type=text id="name" data-val="name" maxlength="30" /> </td>\
             </tr>\
             <tr>\
             <th scope="row">是否多选：</th>\
             <td><input id="is_mchoice" type="checkbox"  data-val="is_mchoice" />(有些属性是可以选择多个属性值的，如“适合人群”，就可能既适合老年人也适合中年人)\
             </td>\
             </tr>\
             <tr>\
             <th scope=row>排序索引：</th>\
             <td><input class="form-control form-control-sm" type="text" id="order_index" data-val="order_index" maxlength="7" style="width:80px;" />\
             </td>\
             </tr>\
             </table>\
         </div>';


//属性值模板
var tmp_v = '<div style="height:60px;width:640px;"  >\
                      <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table_s1" >\
                      <tr>\
                      <th scope="row">属性值：</th>\
                      <td> <input class="form-control form-control-sm" type="text" id="val" data-val="val" maxlength="15" /> </td>\
                      </tr>\
                      <tr></table>\
                 </div>';

