/*!
 * 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
 * Copyright (c) 2013-2018 深圳微狐信息技术有限公司
 * 创 建：
 * 日 期：2018-12-05 11:53:51
 * 描 述：公司表单脚本
 */
var entity = {};

$(function () {

    App_G.Auth.InitID();

    //映射页面数据
    entity = App_G.Mapping.Load("#entity");
    if (entity != null) {
        //日期格式化
        entity.birthday = App_G.Util.Date.ChangeDateFormat(entity.birthday);
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
                entity: App_G.Mapping.Get("data-val", { id: App_G.Util.getRequestId('bid') })
            };

            $post("/SKUModule/SKU/ProductTypeForm", JSON.stringify(data), function (result) {
                if (result.Code == App_G.Code.Code_200) {
                    layer.msg(result.Message, { icon: 1 });
                    var url = App_G.Util.getDomain() + "/SKUModule/SKU/AttrName?bid=" + App_G.Util.getRequestId('bid');
                    setTimeout("window.location.href= '" + url + "'", 1000);
                }
                else {
                    layer.msg(result.Message, { icon: 2 });
                }
            });
        }
    });

    var v = yw.valid.getValidate({});

    //商品类型名称
    v.valid("[data-val=name]", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入商品类型名称" },
        blur: { msg: "请输入商品类型名称" },
        othermsg: "该名称已经存在",
        repeatvld: {
            url: App_G.Util.getDomain() + "/SKUModule/SKU/ExistTypeName?bid=" + App_G.Util.getRequestId('bid'),
            msg: "该名称已经存在"
        }
    });

    //备注信息
    v.valid("[data-val=remarks]", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入备注信息，长度在100字符以内" },
        blur: { msg: "请输入备注信息，长度在100字符以内" }
    });


});






