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
                entity: App_G.Mapping.Get("data-val", { id: App_G.Util.getRequestId('bid') })
            };

            $post("/DistributionModule/Distribution/LogisticsCompanyForm", JSON.stringify(data), function (result) {
                if (result.Code == App_G.Code.Code_200) {
                    layer.msg(result.Message, { icon: 1 });
                    setTimeout("window.location.href= 'LogisticsCompanyManage'", 1000);
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
        focus: { msg: "请输入物流公司名称" },
        blur: { msg: "请输入物流公司名称" },
        repeatvld: {
            url: App_G.Util.getDomain() + "/DistributionModule/Distribution/ExistLogisticsCompanyName?bid=" + App_G.Util.getRequestId("bid"),
            msg: "该物流公司名称已经存在"
        }
    });

    //快递100物流接口Code
    v.valid("#code_hundred", {
        vtype: verifyType.isSerialNumber,
        focus: { msg: "快递100Code，物流跟踪所需要请勿随意修改" },
        blur: { msg: "快递100Code，物流跟踪所需要请勿随意修改" }
    });

    //淘宝物流接口Code
    v.valid("#code_taobao", {
        vtype: verifyType.isSerialNumber,
        focus: { msg: "淘宝Code，物流跟踪所需要请勿随意修改" },
        blur: { msg: "淘宝Code，物流跟踪所需要请勿随意修改" }
    });

    //物流公司网址
    v.valid("#site_url", {
        selectvld: true,
        vtype: verifyType.isUrl,
        focus: { msg: "淘宝Code，物流跟踪所需要请勿随意修改" },
        blur: { msg: "淘宝Code，物流跟踪所需要请勿随意修改" }
    });

    //显示排序
    v.valid("[data-val=order_index]", {
        vtype: verifyType.isNumber,
        focus: { msg: "请输入显示排序" },
        blur: { msg: "显示排序只能为数字" }
    }); 

});

