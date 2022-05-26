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

    //系统用户图片上传
    $("#img_up").upload({
        title: "选择图片",
        defimgurl: $("#defurl").val(),
        imgurl: $("#imgurl").val(),
        cData: {
            bizType: App_G.ImgType.Product_Brand,
            bizId: App_G.Util.getRequestId('bid'),
            ticket: $("#Ticket").val(),
            createThmImg: App_G.CreateThmImg.CreateALL,
        },
        imgHeight: "110px",
        imgWidth: "210px",
        maxSize: 0.3,
        callback: function (data) {
            img = data.data;
        }
    });

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
                entity: App_G.Mapping.Get("data-val", { id: App_G.Util.getRequestId('bid') }),
                imgmsg: img
            };

            $post("/ProductModule/Product/BrandForm", JSON.stringify(data), function (result) {
                if (result.Code == App_G.Code.Code_200) {
                    layer.msg(result.Message, { icon: 1 });
                    var url = App_G.Util.getDomain() + "/ProductModule/Product/BrandManage";
                    setTimeout("window.location.href= '" + url + "'", 1000);
                }
                else {
                    layer.msg(result.Message, { icon: 2 });
                }
            });
        }
    });

    var v = yw.valid.getValidate({});

    //品牌中文名称
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
        vtype: verifyType.isNumber,
        focus: { msg: "请输入显示排序" },
        blur: { msg: "显示排序只能为数字" }
    });

});