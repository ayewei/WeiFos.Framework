/*!
 * 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
 * Copyright (c) 2013-2018 深圳微狐信息技术有限公司
 * 创 建：叶委
 * 日 期：2018-12-05 11:53:51
 * 描 述：Banner表单脚本
 */
var entity = {}, img = "", datagrid, datagrid1;

//封面图
var productlist = [], productCatgs = [], selectedProduct = null, selectedProductCatg = null, bizEntity = null, index = 0;


$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

    //系统用户图片上传
    $("#img_up").upload({
        title: "选择图片",
        defimgurl: $("#defurl").val(),
        imgurl: $("#imgurl").val(),
        cData: {
            bizType: App_G.ImgType.Banner,
            bizId: App_G.Util.getRequestId('bid'),
            ticket: $("#Ticket").val(),
            createThmImg: App_G.CreateThmImg.CreateALL,
        },
        imgHeight: "220px",
        imgWidth: "420px",
        maxSize: 0.3,
        callback: function (data) {
            img = data.data;
        }
    });

    //映射页面数据
    entity = App_G.Mapping.Load("#entity");
    if (entity == null) {
        entity = {};
        selectedProductCatg = {};
        selectedProductCatg.id = 0;

        selectedProduct = {};
        selectedProduct.id = 0;
    } else {
        var biz_entity = $("#bizEntity").val();
        if (biz_entity != "") {
            //操作类型
            bizEntity = $.parseJSON($("#bizEntity").val());
            if (entity.content_type == 2 || entity.content_type == 5) {
                if (entity.content_type == 5) {
                    selectedProduct = bizEntity;
                    $("#content_value").val("");
                } else {
                    selectedProductCatg = bizEntity;
                    $("#content_value").val("");
                }

                var tr = $("tr[data-tmp=" + entity.content_type + "]");
                tr.show();
                tr.find("span").text(bizEntity.name);
            }
        } else {
            if (entity.content_type == 1) {
                var tr = $("tr[data-tmp=" + entity.content_type + "]");
                tr.show();
            }
        }
    }

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

            $("#SaveBtn").setDisable();
            var data = {
                entity: App_G.Mapping.Get("data-val", { id: App_G.Util.getRequestId('bid') }),
                imgmsg: img
            };

            if (entity.content_type == 5 && selectedProduct != null) {
                data.entity.content_value = selectedProduct.id
            } else if (entity.content_type == 10 && selectedProductCatg != null) {
                data.entity.content_value = selectedProductCatg.id
            }
             
            $post("/SiteSettingModule/SiteSetting/BannerForm", JSON.stringify(data), function (result) {
                if (result.Code == App_G.Code.Code_200) {
                    layer.msg(result.Message, { icon: 1 });
                    var url = App_G.Util.getDomain() + "/SiteSettingModule/SiteSetting/BannerManage";
                    setTimeout("window.location.href= '" + url + "'", 1000);
                }
                else {
                    layer.msg(result.Message, { icon: 2 });
                }
            });
        }
    });

    var v = yw.valid.getValidate({});

    //Banner名称
    v.valid("[data-val=name]", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入品牌名称" },
        blur: { msg: "品牌名称输入格式不正确" }
    });

    //品牌英文名称
    v.valid("#name_en", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入编号,只能包含字母、数字、下划线" },
        blur: { msg: "请输入编号,只能包含字母、数字、下划线" }
    });

    //显示排序
    v.valid("[data-val=order_index]", {
        vtype: verifyType.isSerialNumber,
        focus: { msg: "请输入显示排序" },
        blur: { msg: "显示排序只能为数字" }
    });

    //链接地址
    v.valid("#content_value", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入链接地址" },
        blur: { msg: "请输入链接地址" },
        selectvld: true
    });

    //简介
    $("#intro").maxlength({ MaxLength: 300 });
    v.valid("#intro", {
        tabindex: "#base_panel",
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入Banner简介" },
        blur: { msg: "请输入Banner简介" },
        selectvld: true
    });

    //选择改变事件
    $("#content_type").change(function () {
        var val = $(this).val();
        $("tr[data-tmp]").hide();
        $("tr[data-tmp=" + val + "]").show();

        $("#content_value").val("");
    });

    //选择商品详情
    $("#selectProductDetails").digbox({
        Title: "选择商品",
        Content: tmp,
        Show: function (b, c, p) {

            let data = {
                catg_id: 0, gcatg_id: 0, brand_id: 0, keyword: p.find("#keyword").val(), is_shelves: 1, date: ""
            };

            //表格初始化
            datagrid1 = p.find("[name=datagrid]").datagrid({
                url: "/ProductModule/Product/GetProducts",
                data: data,
                pager: {
                    index: 0,
                    pageSize: [10, 20, 50]
                },
                template_id: "list_template",
                column: [
                    {
                        html: "选择",
                        style: "width:20px;"
                    },
                    {
                        text: "基本信息",
                        style: "width:240px;"
                    },
                    {
                        text: "商品状态",
                        style: "width:150px;"
                    },
                    {
                        text: "上传时间",
                        style: "width:100px;"
                    }
                ],
                show(b, c, p) {

                },
                completed: function () {
                    //设置选中
                    $.each(p.find("input[type=radio]"), function (i, o) {
                        if (selectedProduct.id == $(o).val()) {
                            $(o).prop("checked", true);
                        }
                    });

                    //查询按钮
                    p.find("#search_btn").unbind("click").click(function () {
                        datagrid1.execute({
                            catg_id: 0, gcatg_id: 0, brand_id: 0, keyword: p.find("#keyword").val(), is_shelves: 1, date: ""
                        });
                    });
                },
                callback: function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        productlist = result.Data.pageData;
                    } else {
                        layer.msg(result.Message, { icon: 2 });
                    }
                }
            });
        },
        CallBack: function (b, c, p) {
            var redio = p.find("[type=radio]:checked");
            if (redio.length > 0) {
                productlist.forEach((ele, i) => {
                    if (ele.id == parseInt(redio.val())) {
                        selectedProduct = ele;
                        entity.content_value = selectedProduct.id
                        c.find("span").text("[" + ele.name + "]");
                        return;
                    }
                })
            }
            return true;
        }
    });

    //选择商品分类
    $("#selectProductCatg").digbox({
        Title: "选择分类",
        Content: tmpCatg,
        Show: function (b, c, p) {

            //表格初始化
            datagrid = p.find("[name=datagrid]").datagrid({
                url: "/ProductModule/Product/GetGuideProductCgtys",
                data: { keywords: p.find("#keyword").val() },
                pager: {
                    index: 0,
                    pageSize: [10, 20, 50]
                },
                template_id: "catg_template",
                column: [
                    {
                        text: "",
                        style: "width: 1px; "
                    },
                    {
                        text: "分类名称",
                        style: "width: 140px; "
                    }
                ],
                show(b, c, p) {

                },
                callback: function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        productCatgs = result.Data.pageData;
                    } else {
                        layer.msg(result.Message, { icon: 2 });
                    }
                },
                completed: function () {
                    //设置选中
                    $.each(p.find("input[type=radio]"), function (i, o) {
                        if (selectedProductCatg.id == $(o).val()) {
                            $(o).prop("checked", true);
                        }
                    });

                    //查询按钮
                    p.find("#search_btn").unbind("click").click(function () {
                        datagrid.execute({ keyword: p.find("#keyword").val() });
                    });
                }
            });
        },
        CallBack: function (b, c, p) {

            var redio = p.find("[type=radio]:checked");
            if (redio.length > 0) {
                productCatgs.forEach((ele, i) => {
                    if (ele.id == parseInt(redio.val())) {
                        selectedProductCatg = ele;
                        entity.content_value = selectedProductCatg.id
                        c.find("span").text("[" + ele.name + "]");
                        return;
                    }
                })
            }

            return true;
        }
    });

});


//商品列表模板
var tmp = '<div style="700px;width:700px;">\
                                <div class="input-group input-group-sm">\
                                    <input type="text" id="keyword" class="form-control float-right form-control-sm" placeholder="关键词" />\
                                    <div class="input-group-append">\
                                        <button type="button" class="btn btn-default" id="search_btn" ><i class="fa fa-search"></i></button>\
                                    </div>\
                                </div>\
                              <div name="datagrid" class="mt2"></div>\
                        </div>';


//商品分类
var tmpCatg = '<div style="700px;width:700px;">\
                                <div class="input-group input-group-sm">\
                                    <input type="text" id="keyword" class="form-control float-right form-control-sm" placeholder="关键词" />\
                                    <div class="input-group-append">\
                                        <button type="button" class="btn btn-default" id="search_btn" ><i class="fa fa-search"></i></button>\
                                    </div>\
                                </div>\
                              <div name="datagrid" class="mt2"></div>\
                        </div>';


//父类数量
function getParentCount(pid) {
    $.each(productCatgs, function (i, o) {
        if (pid == o.id) {
            index++;
            getParentCount(o.parent_id);
            return;
        }
    });
    return index;
}


template.defaults.imports.getTrIndex = function (pid) {
    if ("tr_" + pid == "tr_0") {
        return "";
    } else {
        index = 0;
        return getParentCount(pid, index);
    }
};

//获取商品状态
template.defaults.imports.getStatus = function (tag) {
    var tag_html = '';
    if (tag != null && tag != undefined) {
        var t = tag.split(',');
        for (var i = 0; i < t.length; i++) {
            if (i == 1) {
                tag_html += "<span class='label label-success'>新品</span>&nbsp;";
            } else if (i == 2) {
                tag_html += "<span class='label label-important'>热门</span>&nbsp;";
            } else if (i == 3) {
                tag_html += "<span class='label label-warning'>推荐</span>&nbsp;";
            } else if (i == 4) {
                //tag += "<span class='label label-success'>上架</span>";
            } else if (i == 5) {
                //tag += "<span class='label label-success'>上架</span>";
            }
        }
    }
    return tag_html;
};
