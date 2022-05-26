/*!
 * 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
 * Copyright (c) 2013-2018 深圳微狐信息技术有限公司
 * 创 建：叶委
 * 日 期：2018-12-05 11:53:51
 * 描 述：商品品牌表单脚本
 */
//               获取省份列表 区域html
var entity = {}, provincedata, regionhtmls, v;


$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

    //映射页面数据
    entity = App_G.Mapping.Load("#entity");

    //绑定xml地区事件
    $.ajax({
        url: App_G.Util.getDomain() + "/Config/" + "area.xml",
        type: "GET",
        async: false,
        dataType: "xml",
        error: function (xdata) { alert("加载xml数据有误！"); },
        success: function (xdata) {

            //获取省份列表
            provincedata = $(xdata).find("address > province");

            regionhtmls = '<ul class="region_ul" >';
            provincedata.each(function (i, o) {
                regionhtmls += "<li><input type='checkbox' value='" + provincedata.eq(i).attr("name") + "'>" + provincedata.eq(i).attr("name") + "</li>";
            });
            regionhtmls += "</ul>";

            //添加地区
            $("#add_region").digbox({
                Title: "选择地区",
                Content: regionhtmls,
                Show: function (s, c, p) {
                    BindCheckBox(c, p, false);
                },
                CallBack: function (s, c, p) {

                    //选择的区域
                    var regions = "";

                    //省份区域复选框
                    var checkboxs = p.find("ul.region_ul input[type=checkbox]:checked");

                    if (checkboxs.length == 0) {
                        App_G.MsgBox.error_digbox("请至少选择一个地区");
                        return false;
                    }

                    $.each(checkboxs, function (i, o) {
                        if (regions.length == 0) {
                            regions += $(o).val();
                        } else {
                            regions += "," + $(o).val();
                        }
                    });

                    var FirstWeightID = "FirstWeight_" + $("#regionBody tr").length;
                    var AddWeightID = "AddWeight_" + $("#regionBody tr").length;
                    var FirstPriceID = "FirstPrice_" + $("#regionBody tr").length;
                    var AddPriceID = "AddPrice_" + $("#regionBody tr").length;

                    var html = "<tr>\
                       <td>" + regions + "</td>\
                       <td style=\"width:120px;\" ><input name=\"first_weight\" class=\"input_wr\" type=\"text\" style=\"width:90px;\" id=\"" + FirstWeightID + "\" /></td>\
                       <td style=\"width:120px;\" ><input name=\"add_weight\" class=\"input_wr\" type=\"text\" style=\"width:90px;\" id=\"" + AddWeightID + "\"/></td>\
                       <td style=\"width:120px;\" ><input name=\"first_price\" class=\"input_wr\" type=\"text\" style=\"width:90px;\" id=\"" + FirstPriceID + "\" /></td>\
                       <td style=\"width:120px;\" ><input name=\"add_price\" class=\"input_wr\" type=\"text\" style=\"width:90px;\" id=\"" + AddPriceID + "\"/></td>\
                       <td>\
                       <a href=\"javascript:void(0);\" name=\"update\" class=\"nl\" ><i class=\"icon-edit\"></i>修改地区</a>\
                       <a href=\"javascript:void(0);\" name=\"del\" class=\"nl\" ><i class=\"icon-trash\"></i>删除</a>\
                       </td>\
                       </tr>";
                    $("#regionBody").append(html);

                    //首重重量
                    v.valid("#" + FirstWeightID, {
                        model: 0,
                        vtype: verifyType.isNumber,
                        focus: { msg: "首重重量" },
                        blur: { msg: "" }
                    });

                    //续重重量
                    v.valid("#" + AddWeightID, {
                        model: 0,
                        vtype: verifyType.isNumber,
                        focus: { msg: "续重重量" },
                        blur: { msg: "" }
                    });

                    //首重价格
                    v.valid("#" + FirstPriceID, {
                        model: 0,
                        vtype: verifyType.isNumber,
                        focus: { msg: "首重价格" },
                        blur: { msg: "" }
                    });

                    //续重价格
                    v.valid("#" + AddPriceID, {
                        model: 0,
                        vtype: verifyType.isLGZeroAmount,
                        focus: { msg: "续重价格" },
                        blur: { msg: "" }
                    });

                }
            });
        }
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

            $("#SaveBtn").setDisable();
            var data = {
                entity: App_G.Mapping.Get("data-val", { id: App_G.Util.getRequestId('bid') }),
                FRegions: GetFreightRegions()
            };

            $post("/DistributionModule/Distribution/FreightTemplateForm", JSON.stringify(data), function (result) {
                if (result.Code == App_G.Code.Code_200) {
                    layer.msg(result.Message, { icon: 1 });
                    setTimeout("window.location.href= 'FreightTemplateManage'", 1000);
                }
                else {
                    layer.msg(result.Message, { icon: 2 });
                }
            });
        }
    });


    v = yw.valid.getValidate({});

    //配送方式名称
    v.valid("#name", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入配送方式名称" },
        blur: { msg: "请输入配送方式名称" },
        repeatvld: {
            url: App_G.Util.getDomain() + "/DistributionModule/Distribution/ExistLogisticsCompanyName?bid=" + App_G.Util.getRequestId("bid"),
            msg: "该配送方式名称已经存在"
        }
    });

    //首重重量
    v.valid("[data-val=first_weight]", {
        vtype: verifyType.isNumber,
        focus: { msg: "请输入首重重量（单位克" },
        blur: { msg: "请输入首重重量（单位克" }
    });

    //续重重量
    v.valid("[data-val=add_weight]", {
        vtype: verifyType.isNumber,
        focus: { msg: "请输入续重重量（单位克" },
        blur: { msg: "请输入续重重量（单位克" }
    });

    //默认首重价格
    v.valid("[data-val=default_first_price]", {
        vtype: verifyType.isLGZeroAmount,
        focus: { msg: "请输入默认首重价格（单位克" },
        blur: { msg: "请输入默认首重价格（单位克" }
    });

    //默认续重价格
    v.valid("[data-val=default_add_price]", {
        vtype: verifyType.isLGZeroAmount,
        focus: { msg: "请输入默认续重价格（单位克" },
        blur: { msg: "请输入默认续重价格（单位克" }
    });

    //显示排序
    v.valid("[data-val=order_index]", {
        vtype: verifyType.isNumber,
        focus: { msg: "请输入显示排序" },
        blur: { msg: "显示排序只能为数字" }
    });

    //绑定删除修改地区
    BindRegionGroup();

});



//绑定地区组事件
function BindRegionGroup() {

    //首重重量
    v.valid("[name=first_weight]", {
        vtype: verifyType.isNumber,
        focus: { msg: "首重重量" },
        blur: { msg: "" }
    });

    //续重重量
    v.valid("[name=add_weight]", {
        vtype: verifyType.isNumber,
        focus: { msg: "续重重量" },
        blur: { msg: "" }
    });

    //首重价格
    v.valid("[name=first_price]", {
        vtype: verifyType.isLGZeroAmount,
        focus: { msg: "首重价格" },
        blur: { msg: "" }
    });

    //续重价格
    v.valid("[name=add_price]", {
        vtype: verifyType.isLGZeroAmount,
        focus: { msg: "续重价格" },
        blur: { msg: "" }
    });

    //修改地区
    $("[name=update]").digbox({
        Title: "修改地区",
        Content: regionhtmls,
        Show: function (s, c, p) {
            BindCheckBox(c, p, true);
        },
        CallBack: function (s, c, p) {

            var regions = "";

            //省份区域复选框
            var checkboxs = p.find("ul.region_ul input[type=checkbox]:checked");

            if (checkboxs.length == 0) {
                layer.msg('请至少选择一个地区', { icon: 2 });
                return false;
            }

            $.each(checkboxs, function (i, o) {
                if (regions.length == 0) {
                    regions += $(o).val();
                } else {
                    regions += "," + $(o).val();
                }
            });
            c.parent().parent().find("td:eq(0)").text(regions);
            return true;
        }
    });

    //删除地区组
    $("[name=del]").digbox({
        Title: "删除",
        Content: "确定删除吗？",
        CallBack: function (s, c, p) {
            $(c).parent().parent().remove();
        }
    });
}


//绑定地区组事件
function BindCheckBox(current, panel, isupdate) {

    //省份区域复选框
    var checkboxs = panel.find("ul.region_ul input[type=checkbox]");

    //遍历行
    $.each($("#regionBody tr"), function (i, o) {

        var regions = $.trim($(o).find("td:eq(0)").text());

        if (regions.indexOf(',') != -1) {
            regions = regions.split(',');
        } else {
            regions = [regions];
        }

        $.each(regions, function (ii, oo) {
            $.each(checkboxs, function (iii, ooo) {
                if (oo == $(ooo).val()) {
                    $(ooo).attr("disabled", "disabled");
                }
            });
        });
    });

    //如果是修改
    if (isupdate) {

        var cregions = $.trim(current.parent().parent().find("td:eq(0)").text());

        if (cregions.indexOf(',') != -1) {
            cregions = cregions.split(',');
        } else {
            cregions = [cregions];
        }

        $.each(cregions, function (ii, oo) {
            $.each(checkboxs, function (iii, ooo) {
                if (oo == $(ooo).val()) {
                    $(ooo).attr("disabled", false);
                    $(ooo).attr("checked", true);
                }
            });
        });
    }
}

//获取地区价格集合
function GetFreightRegions() {

    var FreightRegionList = [];

    $.each($("#regionBody").find("tr"), function (i, o) {
        //地区价格
        var FreightRegion = {};

        //设置区域价格ID
        if (undefined != $(o).attr("data-id")) {
            FreightRegion.id = $(o).attr("data-id");
        } else {
            FreightRegion.id = -(i + 1);
        }

        //区域名称
        FreightRegion.region_name = $(o).find("td:eq(0)").text();

        //地区价格首重重量
        FreightRegion.first_weight = $(o).find("td:eq(1) input").val();

        //地区价格续重重量
        FreightRegion.add_weight = $(o).find("td:eq(2) input").val();

        //地区价格首重价格
        FreightRegion.first_price = $(o).find("td:eq(3) input").val();

        //地区价格续重价格
        FreightRegion.add_price = $(o).find("td:eq(4) input").val();

        FreightRegionList.push(FreightRegion);
    });

    return FreightRegionList;
}