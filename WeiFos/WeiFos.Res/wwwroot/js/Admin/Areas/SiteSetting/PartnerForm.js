
var entity = "";

//身份证完整图
var img = "";

$(function () {

    //if ($.trim($("#imgmsg").val()).length > 0) {
    //    img = $("#imgmsg").val();
    //}

    //初始化权限编号
    App_G.Auth.InitID();

    //页面数据绑定
    if ($.trim($("#entity").val()).length > 0) {
        entity = $.parseJSON($("#entity").val());
        App_G.Util.bindJson("data-val", entity);
    }

    //资讯分类图片
    $("#ad_img").upload({
        title: "选择图片",
        defimgurl: $("#defurl").val(),
        imgurl: $("#imgurl").val(),
        cData: {
            bizType: App_G.ImgType.Partner,
            bizId: App_G.Util.getRequestId('bid'),
            ticket: $("#Ticket").val(),
            createThmImg: App_G.CreateThmImg.CreateS,
        },
        imgHeight: "112.5px",
        imgWidth: "225px",
        maxSize: 0.3,
        callback: function (data) {
            img = data;
        }
    });

    yw.valid.config({
        submitelements: "#SaveBtn", vsuccess: function () {

            //if ($.trim(img).length == 0) {
            //    App_G.MsgBox.error_digbox("请上传图片！");
            //    return;
            //}

            $("#SaveBtn").setDisable();

            var data = {
                partner: App_G.Mapping.Get("data-val", { id: App_G.Util.getRequestId('bid') }),
                imgmsg: img.data == undefined ? "" : img.data
            };

            $postByAsync("/SiteSetting/SavePartner", JSON.stringify(data),
                 function (result) {
                     if (result.Code == App_G.Code.Code_200) {
                         App_G.MsgBox.success_digbox();
                         setTimeout("window.location.href='PartnerManage';", 1000);
                     } else {
                         App_G.MsgBox.error_digbox("操作失败！");
                     }
                 });
        }
    });

    var v = yw.valid.getValidate();
    v.valid("#Name", { focusmsg: "请输入合作伙伴名称", errormsg: "", vtype: verifyType.anyCharacter });
    v.valid("#OrderIndex", { focusmsg: "请输入排序索引", errormsg: "索引只能够是正整数", vtype: verifyType.isLGZeroNumber });
    v.valid("#URL", { focusmsg: "请输入链接地址", errormsg: "", vtype: verifyType.isUrl, selectvalidate: true });
    
});


