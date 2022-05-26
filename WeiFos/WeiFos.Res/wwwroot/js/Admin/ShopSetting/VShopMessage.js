
var t_img, s_img, entity;
$(function () {

    //页面数据绑定
    if ($.trim($("#entity").val()).length > 0) {
        entity = $.parseJSON($("#entity").val());
        App_G.Util.bindJson("data-val", entity);
    }

    //商城Logo
    $("#logo_img").upload({
        title: "选择图片",
        defimgurl: $("#logodefurl").val(),
        imgurl: $("#logoimgurl").val(),
        cData: {
            bizType: App_G.ImgType.Mall_Logo,
            bizId: (undefined == entity ? 0 : entity.id),
            ticket: $("#logoTicket").val(),
            createThmImg: App_G.CreateThmImg.CreateS,
        },
        createThmImg: App_G.CreateThmImg.CreateS,
        imgHeight: "110px",
        imgWidth: "110px",
        maxSize: 0.3,
        callback: function (data) {
            l_img = data;
        }
    });

    $("#share_img").upload({
        title: "选择图片",
        defimgurl: $("#tdefurl").val(),
        imgurl: $("#timgurl").val(),
        cData: {
            bizType: App_G.ImgType.Share_Title,
            bizId: (undefined == entity ? 0 : entity.id),
            ticket: $("#tTicket").val(),
            createThmImg: App_G.CreateThmImg.CreateS,
        },
        createThmImg: App_G.CreateThmImg.CreateS,
        imgHeight: "110px",
        imgWidth: "110px",
        maxSize: 0.3,
        callback: function (data) {
            s_img = data;
        }
    });

    //初始化验证插件
    yw.valid.config({
        submitelements: "#SaveBtn", vsuccess: function () {
            $("#SaveBtn").setDisable({ text: "保 存 中..." });

            var data = {
                msg: App_G.Util.getJson("data-val", { id: App_G.Util.getRequestId('bid')}),
                shareimg: s_img == undefined ? "" : s_img.data,
                logoimg: l_img == undefined ? "" : l_img.data
            };

            $post("/ShopSetting/SaveVShopMessage", JSON.stringify(data),
                function (data) {
                    if (data.Code == App_G.Code.Code_200) {
                        App_G.MsgBox.success_digbox(); 
                    } else {
                        App_G.MsgBox.error_digbox("操作失败");
                    }
                });

        }
    });

    v = yw.valid.getValidate();

    //商城名称
    v.valid("#mall_name", { focusmsg: "请输入商城名称，长度在50字符以内", errormsg: "请输入商城名称，长度在50字符以内", vtype: verifyType.anyCharacter });

    //分享标题
    v.valid("#share_title", { focusmsg: "请输入分享标题，长度在50字符以内", errormsg: "请输入分享标题，长度在50字符以内", vtype: verifyType.anyCharacter });

    //分享内容
    v.valid("#share_content", { focusmsg: "请输入分享标题，长度在100字符以内", errormsg: "请输入分享标题，长度在100字符以内", vtype: verifyType.anyCharacter });

    //联系地址
    v.valid("#address", { focusmsg: "请输入联系地址", errormsg: "请输入联系地址", vtype: verifyType.anyCharacter });

    //联系电话
    v.valid("#phone", { focusmsg: "请输入联系电话", errormsg: "请输入正确的电话号码", vtype: verifyType.isPhone });

    //邮政编码
    v.valid("#zip_code", { focusmsg: "请输入邮政编码", errormsg: "请输入正确格式的邮政编码", vtype: verifyType.isPostCode });

    //邮箱
    v.valid("#email", { focusmsg: "请输入商城邮箱", errormsg: "请输入正确的邮箱", vtype: verifyType.isEmail });

    //版权信息
    v.valid("#copyright", { focusmsg: "请输入版权信息", errormsg: "请输入版权信息", vtype: verifyType.anyCharacter });


});

