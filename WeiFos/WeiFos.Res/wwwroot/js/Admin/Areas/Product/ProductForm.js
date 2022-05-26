/*!
 * 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
 * Copyright (c) 2013-2018 深圳微狐信息技术有限公司
 * 创 建：叶委
 * 日 期：2018-12-05 11:53:51
 * 描 述：商品品牌表单脚本
 */
document.domain = App_Config.getCrossRes();
//基础属性名      扩展属性名   扩展属性值  商品信息
var entity = null, baseAttrNames, extAttrNames, extAttrVals, product, sku_er, v;

$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

    //映射页面数据
    entity = App_G.Mapping.Load("#entity");

    if ($("#brand_id option").length == 0) {
        alert("当前没有添加品牌请先添加品牌");
        window.location.href = "/Product/ProductManage";
    }

    //重新选择类别
    $("#selectCgty").click(function () {
        window.location.href = "/Product/ShelvesProduct?bid=" + App_G.Util.getRequestId('cid') + "&pid=" + App_G.Util.getRequestId('bid');
    });

    //初始化验证插件
    yw.valid.config({
        submiteles: "#SaveBtn",
        data: [
            {
                attr: "data-val",
                data: entity
            }
        ],
        vsuccess: function () {

            if (sku_er != null && !sku_er.hasCreateSku()) {
                layer.msg("当前商品存在规格，请至少勾选一个规格信息！", { icon: 2 });
                return;
            }

            $("#SaveBtn").setDisable({ text: "保 存 中..." });

            var datas = {
                product: App_G.Mapping.Get("data-val", {
                    tag: getTag(),
                    catg_id: App_G.Util.getRequestId('cid'),
                    id: App_G.Util.getRequestId('bid'),
                    details: editor.getContent(),
                    is_open_spec: (sku_er != null && sku_er.hasCreateSku() && sku_er.getSkuData().length > 0)
                }),
                //基础属性值
                pdtAttrVals: getAttrVals(),
                //扩展属性值
                pdtExtAttrVals: getExtAttrVals(),
                //获取商品SKU集合
                skus: getSkus(),
                //规格信息
                specCustoms: sku_er == null ? [] : sku_er.getSpecCustoms(),
                //图片集合
                imgs: getImgs(),
                //主图信息
                mainimg: getMainImg()
            };

            $post("/Product/SaveProduct", JSON.stringify(datas),
                function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        layer.msg(result.Message, { icon: 1 });
                        var url = "/ProductModule/Product/ProductManage?" + window.location.href.split("?")[1];
                        setTimeout("window.location.href='" + url + "';", 1000);
                    } else if (result.Code == 1) {
                        layer.msg("该类别下存在商品，不能删除！", { icon: 2 });
                    } else {
                        layer.msg(result.Message, { icon: 2 });
                    }
                });
        }
    });


    v = yw.valid.getValidate();

    //商品名称
    v.valid("#name", {
        tabindex: "#base_panel",
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入商品名称" },
        blur: { msg: "请输入商品名称" }
    });

    //英文名称
    v.valid("#en_name", {
        tabindex: "#base_panel",
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入英文名称" },
        blur: { msg: "请输入英文名称" }
    });

    //商品编号
    v.valid("#no", {
        tabindex: "#base_panel",
        vtype: verifyType.isSerialNumber,
        focus: { msg: "请输入商品编号，只能包含字母、数字、_或-" },
        blur: { msg: "商品编号，只能包含字母、数字、_或-" }
    });

    //计量单位
    v.valid("#unit", {
        tabindex: "#base_panel",
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输计量单位，只能是英文和中文例如:g/元" },
        blur: { msg: "只能是英文和中文例如:g/元" }
    });

    //排序索引
    v.valid("#order_index", {
        tabindex: "#base_panel",
        vtype: verifyType.isNumber,
        focus: { msg: "请输入排序索引" },
        blur: { msg: "排序索引，只能是数字" }
    });

    //市场价
    v.valid("#market_price", {
        tabindex: "#base_panel",
        vtype: verifyType.isLGZeroPrice,
        focus: { msg: "请输入市场价" },
        blur: { msg: "输入有误" }
    });

    $("#introduction").maxlength({ MaxLength: 300 });
    //商品简介
    v.valid("#introduction", {
        tabindex: "#base_panel",
        selectvld: true,
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入商品简介" },
        blur: { msg: "请输入商品简介" }
    });

    //未开启规格商品信息验证
    validateNoSku();

    //商品详细
    var editor = UE.getEditor('details', {
        initialFrameHeight: $(document).height() * 0.5
    });
    editor.ready(function () {
        //自定义参数
        editor.execCommand('serverparam', {
            'bizType': App_G.ImgType.Product_Details,
            'bizId': App_G.Util.getRequestId('bid'),
            'ticket': $("#DetailsTicket").val(),
            'createThmImg': App_G.CreateThmImg.CreateALL
        });
    });

    //编辑绑定
    if (entity != null) {

        if ($("#product_type_id").val() == null) {
            $("#product_type_id").val(0);
        }

        var tags = entity.tag.split(',');
        for (var i = 1; i < 5; i++) {
            var val = tags[i];
            if (val == 1) {
                $("input[name=tag]:eq(" + (i - 1).toString() + ")").prop("checked", true);
            } else {
                $("input[name=tag]:eq(" + (i - 1).toString() + ")").prop("checked", false);
            }
        }

        //加载商品相关信息
        $getByAsync("/ProductModule/Product/InitProduct?bid=" + App_G.Util.getRequestId('bid') + "&tid=" + entity.product_type_id, "",
            function (result) {
                //加载成功
                if (result.Code == App_G.Code.Code_200) {
                    //商品类型
                    if ($("#product_type_id").val() > 0) {
                        //创建
                        createAttr(result.Data.baseAttrNames, result.Data.extAttrVals, result.Data.extAttrNames);
                        //绑定
                        bindAttr(result.Data.baseAttrPdtVals, result.Data.pdtExtAttrVals);
                        //开启规格
                        if (entity.is_open_spec) {
                            //删除未开启SKU 商品信息
                            $("tr[name=attrval_tr]").remove();
                            //绑定sku
                            bindSku(result.Data.specNames, result.Data.specValues, result.Data.specCustoms, result.Data.skus);
                        } else {
                            //绑定商品信息
                            App_G.Mapping.Bind("data-nosku", result.Data.skus[0]);
                        }
                    } else {
                        if (!entity.is_open_spec) {
                            //绑定商品信息
                            App_G.Mapping.Bind("data-nosku", result.Data.skus[0]);
                        }
                    }
                    //绑定图片模板
                    $("#img_panel").append(template("imgTemplate", { imgs: result.Data.imgs }));
                } else {
                    layer.msg(result.Message, { icon: 2 });
                }
            });

    } else {
        //绑定图片模板
        $("#img_panel").append(template("imgTemplate", { imgs: [] }));
    }

    //当前类型ID
    var TypeId = $("#product_type_id").val();
    $("#product_type_id").digbox({
        Backdrop: true,
        Title: "切换商品类型",
        Content: "切换商品类型将会导致已经编辑的属性、规格数据丢失，确定切换吗？", Event: "change", CallBack: function () {

            TypeId = $("#product_type_id").val();
            if (TypeId != 0) {

                $post("/SKUModule/SKU/CreateSKU?bid=" + ($("#product_type_id").val() == null ? product.product_type_id : $("#product_type_id").val()), "",
                    function (result) {
                        if (result.Code == App_G.Code.Code_200) {
                            //删除未开启SKU 商品信息
                            $("tr[name=attrval_tr]").remove();
                            //创建
                            createAttr(result.Data.baseAttrNames, result.Data.extAttrVals, result.Data.extAttrNames);
                            //绑定
                            bindSku(result.Data.specNames, result.Data.specValues);
                        } else {
                            createNoSku();
                            layer.msg(result.Message, { icon: 2 });
                        }
                    });

            } else {
                createNoSku();
            }

        }, Cencel: function (e) {
            $("#product_type_id").val(TypeId);
        }
    });

    //加载商品封面图方法
    loadEditorImage();

    bindImgList();

    //返回
    $("#BackBtn").click(function () {
        window.location.href = "ProductManage?" + window.location.href.split("?")[1];
    });

    //下拉搜索
    $(".chosen-select").chosen({
        no_results_text: "没有找到结果！",//搜索无结果时显示的提示
        search_contains: true,   //关键字模糊搜索，设置为false，则只从开头开始匹配
        allow_single_deselect: true, //是否允许取消选择
        max_selected_options: 6  //当select为多选时，最多选择个数
    });

});



//生成类型属性
function createAttr(baseAttrNames, extAttrVals, extAttrNames) {

    $("#tbd_baseattr").find("tr").remove();
    if (baseAttrNames.length) {
        $("#div_baseattr").show();
        var tr = "";
        $.each(baseAttrNames, function (i, o) {
            tr += "<tr><th scope='row' >" + o.name + ":</th><td><input name='baseAttrValue' type='text' id='0' data-id='" + o.id + "' class='form-control form-control-sm'  /></td></tr>";
        });
        $(tr).appendTo($("#tbd_baseattr"));
    }

    //存在属性数据，显示自定义规格Div 
    if (extAttrVals.length) {
        $("#div_extattr").show();
        $("#ext_attr_ul").find("li").remove();
        var val = "";
        var tmp = "";
        $.each(extAttrNames, function (i, o) {
            val += "<li><label class='disBlock'>" + o.name + "：</label>";

            $.each(extAttrVals, function (j, v) {

                if (v.ext_attr_name_id == o.id) {
                    if (o.is_mchoice) {
                        tmp += "<p class='disBlock mr20 fw400' ><input type='checkbox' data-extattrval_id='" + v.id + "' value='" + v.id + "'  name='attr_check_value'  ><label class='fw400'>" + v.val + "</label>";
                    } else {
                        tmp += "<option value='" + v.id + "' >" + v.val + "</option>";
                    }
                }

            });

            if (!o.is_mchoice) {
                tmp = "<select data-id='extAttrNames' name='attr_select_value' >" + tmp + "</select>";
            }

            val += tmp + "</li>";
            tmp = "";
        });
        $(val).appendTo($("#ext_attr_ul"));
    }
}

///绑定商品属性
function bindAttr(baseAttrPdtVals, pdtExtAttrVals) {

    //基本属性值
    $.each(baseAttrPdtVals, function (i, o) {
        $("input[data-id=" + o.attrname_id + "]").val(o.val);
        $("input[data-id=" + o.attrname_id + "]").attr("id", o.id);
    });

    //扩展属性值
    $.each(pdtExtAttrVals, function (i, o) {

        //多选框
        $("input[data-extattrval_id=" + o.extattrval_id + "]").attr("checked", "checked");

        //下拉框
        $.each($("select[data-id=extAttrNames]"), function (j, k) {
            $.each(k, function (l, m) {
                if (m.value == o.extattrval_id && o.extattrval_id != 0) {
                    $(k).val(o.extattrval_id);
                    return;
                }
            });
        });
    });
}

///绑定sku数据
function bindSku(specNames, specValues, specCustoms, skudataset) {
    if (sku_er == null) {
        sku_er = $("#sku_module_div").skuengine({
            skudataset: skudataset,
            speccustoms: specCustoms,
            specnames: specNames,
            specvalues: specValues,
            skucolumn: [
                {
                    no: "serial_no",
                    name: "编号",
                    value: $("[data-val=no]").val() == undefined ? $("[data-val=no]").text() : $("[data-val=no]").val(),
                    isauto: false,
                    validate: function () {
                        v.valid("input[name=serial_no]", { tabindex: "#base_panel", selector: "#sku_module_div_sku", vtype: verifyType.isSerialNumber, focus: { msg: "" }, blur: { msg: "" } });
                    },
                    td_style: "width:180px;",
                    ip_style: "width:130px;"
                }, {
                    no: "cost_price",
                    name: "成本价",
                    value: 0,
                    validate: function () {
                        v.valid("input[name=cost_price]", { tabindex: "#base_panel", selector: "#sku_module_div_sku", vtype: verifyType.isLGZeroPrice, focus: { msg: "" }, blur: { msg: "" } });
                    },
                    td_style: "width:110px;",
                    ip_style: "width:60px;"
                }, {
                    no: "sale_price",
                    name: "销售价",
                    value: 0,
                    validate: function () {
                        v.valid("input[name=sale_price]", { tabindex: "#base_panel", selector: "#sku_module_div_sku", vtype: verifyType.isLGZeroPrice, focus: { msg: "" }, blur: { msg: "" } });
                    },
                    td_style: "width:110px;",
                    ip_style: "width:60px;"
                }, {
                    no: "market_price",
                    name: "市场价",
                    value: 0,
                    validate: function () {
                        v.valid("input[name=market_price]", { tabindex: "#base_panel", selector: "#sku_module_div_sku", vtype: verifyType.isLGZeroPrice, focus: { msg: "" }, blur: { msg: "" } });
                    },
                    td_style: "width:110px;",
                    ip_style: "width:60px;"
                }, {
                    no: "weight",
                    name: "重量（克）",
                    value: 0,
                    validate: function () {
                        v.valid("input[name=weight]", { tabindex: "#base_panel", selector: "#sku_module_div_sku", vtype: verifyType.isNumber, focus: { msg: "" }, blur: { msg: "" } });
                    },
                    td_style: "width:110px;",
                    ip_style: "width:60px;"
                }, {
                    no: "stock",
                    name: "库存",
                    value: 0,
                    validate: function () {
                        v.valid("input[name=stock]", { tabindex: "#base_panel", selector: "#sku_module_div_sku", vtype: verifyType.isNumber, focus: { msg: "" }, blur: { msg: "" } });
                    },
                    td_style: "width:110px;",
                    ip_style: "width:50px;"
                }, {
                    no: "warning_stock",
                    name: "预警库存",
                    value: 0,
                    validate: function () {
                        v.valid("input[name=warning_stock]", { tabindex: "#base_panel", selector: "#sku_module_div_sku", vtype: verifyType.isNumber, focus: { msg: "" }, blur: { msg: "" } });
                    },
                    td_style: "width:110px;",
                    ip_style: "width:50px;"
                }]
        });
    } else {
        //加载重新绑定
        sku_er.onLoad(specNames, specValues, specCustoms, skudataset);
    }


}

//获取sku信息
function getSkus() {
    if (sku_er == null || sku_er.getSkuData().length == 0) {
        return [App_G.Mapping.Get("data-nosku")];
    } else {
        return sku_er.getSkuData();
    }
}

//提交扩展属性值
function getExtAttrVals() {
    var params = [];
    $.each($("input[name=attr_check_value]:checked"), function (i, o) {
        var data = {};
        data["extattrval_id"] = $(o).val();
        params.push(data);
    });

    $.each($("[name=attr_select_value]"), function (i, o) {
        var data = {};
        data["extattrval_id"] = $(o).val();
        params.push(data);
    });
    return params;
}

//提交基础属性值
function getAttrVals() {
    var params = [];
    if ($("#tbd_baseattr").find("tr").length) {
        $.each($("#tbd_baseattr").find("input[name=baseAttrValue]"), function (i, o) {

            var data = {};
            data["id"] = $(o).attr("id");
            data["attrname_id"] = $(o).attr("data-id");
            data["val"] = $(o).val();

            params.push(data);
        });
    }
    return params;
}

//获取图片信息
function getImgs() {
    var imgs = [];
    $.each($("ul.clearfix.disBlock img"), function (i, o) {
        var img = {};
        img.order_index = i;
        img.file_name = $(o).attr("data-id");
        imgs.push(img);
    });
    return imgs;
}

//获取主图信息
function getMainImg() {
    return $("ul.clearfix.disBlock").find("span.icon_front").next().next().find("img").attr("data-id");
}

//获取标签信息
function getTag() {
    var index = 0;
    var value = ",";
    $.each($("[name=tag]"), function (i, o) {
        index++;
        if ($(o).is(':checked')) {
            value += "1,";
        } else {
            value += "0,";
        }
    });
    for (; index < 5; index++) {
        value += "0,";
    }
    return value;
}


//Ueditor 单独上传图片
var myEditorImage;
var d;

function upImage() {
    d = myEditorImage.getDialog("insertimage");
    d.render();
    d.open();
}
//Ueditor 单独上传图片

function loadEditorImage() {

    myEditorImage = UE.getEditor('myEditorImage');
    myEditorImage.ready(function () {

        //自定义参数
        myEditorImage.execCommand('serverparam', {
            'bizType': App_G.ImgType.Product_Cover,
            'bizId': App_G.Util.getRequestId('bid'),
            'ticket': $("#CoverTicket").val(),
            'createThmImg': App_G.CreateThmImg.CreateALL
        });

        //editor.setDisabled();
        myEditorImage.hide();
        myEditorImage.addListener('beforeInsertImage', function (t, arg) {
            var imgList = "";
            var Ids = "";
            $.each(arg, function (i, o) {
                //图片目录
                var base_url = o.src.toString().substr(0, o.src.toString().lastIndexOf('/') + 1);
                //图面名称
                var img_name = o.src.toString().substr(o.src.toString().lastIndexOf('/') + 1);
                //图片扩展名
                var img_extend_name = img_name.split('.')[1];
                //图片集合
                imgList += "<li><span></span>"
                    + "<div class='opBar'><a href='#' class='btn_front disBlock' ><a href='#' class='btn_delPic disBlock'></a></div>"
                    + "<div class='picBar'><img src=" + base_url + img_name + " width='150' data-id = " + img_name.split('.')[0] + " />"
                    + "</div></li>";

            });

            //是否存在图片集合Div
            if (!$('div.proImgList').length) {
                $("<div class='proImgList' ><ul class='clearfix disBlock'> </ul></div>").appendTo("#img_panel");
            }

            if ($(".clearfix.disBlock").find('>li:last').length > 0) {
                $(imgList).insertBefore($(".clearfix.disBlock").find('>li:last'));
            } else {
                imgList += "<li><input type='button' class='btn_addImg disBlock' onclick='upImage()' /></li>";
                $(imgList).appendTo(".clearfix.disBlock");
            }

            //移除默认添加按钮
            $("div.uploadProImg").hide();
            bindImgList();
        });
    });

}

//绑定商品图片列表 （可以封装）
var hasbindimg = false;
function bindImgList() {

    var proImgList = $('.proImgList');
    if (proImgList.length && !hasbindimg) {

        hasbindimg = true;
        var lis = proImgList.find(".clearfix.disBlock").find("li");

        $("div.tab-content").on("mouseover", "ul.clearfix.disBlock li", function () {
            $(this).addClass("current");
        });

        $("div.tab-content").on("mouseout", "ul.clearfix.disBlock li", function () {
            $(this).removeClass("current");
        });

        $("div.tab-content").on("click", "div.opBar a.btn_front.disBlock", function () {
            proImgList.find(".clearfix.disBlock li").find("span").removeClass("icon_front");
            $(this).parent().parent().find("span").addClass("icon_front");
            $("#main_picture").val($(this).parent().parent().find("img").attr("attr"));
        });

        //删除图片
        lis.find(".btn_delPic.disBlock").digbox({
            Content: "div.tab-content",
            Selector: ".btn_delPic.disBlock",
            Title: "提示信息",
            Context: "确定删除该图片吗？",
            CallBack: function (btn_id, current_obj, panel) {
                //获取图片ID(data 表示当前元素)
                var msg = current_obj.parent().parent().find("img").attr("data-id");

                var params = "?bizType=" + App_G.ImgType.Product_Cover + "&imgmsg=" + msg + "&ticket=" + $("#CoverTicket").val() + "&bizId=" + App_G.Util.getRequestId('bid');
                $.getJSON(App_Config.getResDomain() + "/Up/DeleteFile" + params, {}, function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        c.parent().parent().remove();
                    } else {
                        layer.msg(result.Message, { icon: 2 });
                    }
                });
            }
        });

    }
}

//绑定没有sku数据
function createNoSku() {

    var baseattr = '<tr name="attrval_tr">\
                        <th scope="row">商品库存：</th>\
                        <td><input type="text" id="stock" data-nosku="stock" class="form-control form-control-sm"  maxlength="10" value="" /></td>\
                    </tr>\
                    <tr name="attrval_tr">\
                        <th scope="row" >供货价：</th>\
                        <td><input  type="text" id="supply_price" data-nosku="supply_price" class="form-control form-control-sm"  maxlength="50" /></td>\
                    </tr>\
                    <tr name="attrval_tr">\
                        <th scope="row" >销售价：</th>\
                        <td><input  type="text" id="sale_price" data-nosku="sale_price" class="form-control form-control-sm" maxlength="50" /></td>\
                    </tr>';

    $(baseattr).insertAfter($("#product_tb tr:eq(5)"));
    validateNoSku();
}

//没有sku验证情况
function validateNoSku() {

    //商品库存
    v.valid("#stock", { tabindex: "#base_panel", vtype: verifyType.isNumber, focus: { msg: "请输入商品库存" }, blur: { msg: "输入格式不正确" } });

    //商品成本价
    v.valid("#cost_price", { tabindex: "#base_panel", vtype: verifyType.isLGZeroPrice, focus: { msg: "请输入成本价" }, blur: { msg: "输入格式不正确" } });

    //销售价
    v.valid("#sale_price", { tabindex: "#base_panel", vtype: verifyType.isLGZeroPrice, focus: { msg: "请输入销售价" }, blur: { msg: "输入格式不正确" } });

    //商品重量
    v.valid("#weight", { tabindex: "#base_panel", vtype: verifyType.isNumber, focus: { msg: "请输入商品重量（单位g）" }, blur: { msg: "输入格式不正确" } });

    //预警库存
    v.valid("#warning_stock", { tabindex: "#base_panel", vtype: verifyType.isNumber, focus: { msg: "请输入预警库存" }, blur: { msg: "输入格式不正确" } });

}

