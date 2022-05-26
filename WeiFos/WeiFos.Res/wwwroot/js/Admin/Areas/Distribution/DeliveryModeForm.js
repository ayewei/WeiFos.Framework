/*!
 * 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
 * Copyright (c) 2013-2018 深圳微狐信息技术有限公司
 * 创 建：叶委
 * 日 期：2018-12-05 11:53:51
 * 描 述：商品品牌表单脚本
 */
var entity = {}, img = "";

$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

    //映射页面数据
    entity = App_G.Mapping.Load("#entity");
    if (entity != null) {
        if (entity.logistics_company_ids != null) {
            var ids = entity.logistics_company_ids.split(',');
            $.each(ids, function (i, o) {
                $.each($("ul.dregion_ul input[type=checkbox]"), function (ii, oo) {
                    if ($(oo).val() == o) {
                        $(oo).attr("checked", true);
                    }
                });
            });
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

            var ids = getLogisticsCompanyIDs();
            if (ids == "") {
                layer.msg("请选择物流公司", { icon: 2 });
                return;
            }

            if ($("#freight_template_id").val() == 0) {
                layer.msg("选择运费模板", { icon: 2 });
                return;
            }

            var data = {
                entity: App_G.Mapping.Get("data-val", {
                    id: App_G.Util.getRequestId('bid'),
                    logistics_company_ids: ids
                }),
            };

            $post("/DistributionModule/Distribution/DeliveryModeForm", JSON.stringify(data), function (result) {
                if (result.Code == App_G.Code.Code_200) {
                    layer.msg(result.Message, { icon: 1 });
                    setTimeout("window.location.href= 'DeliveryModeManage'", 1000);
                }
                else {
                    layer.msg(result.Message, { icon: 2 });
                }
            });
        }
    });

    var v = yw.valid.getValidate({});

    //物流公司名称
    v.valid("#name", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入配送方式名称" },
        blur: { msg: "请输入配送方式名称" },
        repeatvld: {
            url: App_G.Util.getDomain() + "/DistributionModule/Distribution/ExistDeliveryModeName?bid=" + App_G.Util.getRequestId("bid"),
            msg: "该配送方式名称已经存在"
        }
    });

    //备注信息
    v.valid("[data-val=remarks]", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入备注信息" },
        blur: { msg: "请输入备注信息" }
    });

     
    //显示排序
    v.valid("[data-val=order_index]", {
        vtype: verifyType.isNumber,
        focus: { msg: "请输入显示排序" },
        blur: { msg: "显示排序只能为数字" }
    });

});


//获取物流公司ID
function getLogisticsCompanyIDs() {
    var ids = "";
    $.each($("ul.dregion_ul input[type=checkbox]:checked"), function (i, o) {
        if (ids == "") {
            ids += $(o).val();
        } else {
            ids += "," + $(o).val();
        }
    });
    return ids;
}