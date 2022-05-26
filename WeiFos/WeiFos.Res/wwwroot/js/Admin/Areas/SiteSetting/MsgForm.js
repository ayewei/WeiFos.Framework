
document.domain = App_Config.getCrossRes();

var entity = "";

//身份证完整图
var img = "";

$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

    //页面数据绑定
    if ($.trim($("#entity").val()).length > 0) {
        entity = $.parseJSON($("#entity").val());
        App_G.Util.bindJson("data-val", entity);
        $("#release_date").val(App_G.Util.Date.ChangeDateFormat(entity.release_date));
    }


    //微信端
    var wechat_context = UE.getEditor('wechat_context');
    wechat_context.ready(function () {
        //自定义参数
        wechat_context.execCommand('serverparam', {
            'bizType': App_G.ImgType.Default,
            'bizId': $("#Ticket").val(),
            'createThmImg': App_G.CreateThmImg.None
        });
    });

    //PC端
    var content = UE.getEditor('content');
    content.ready(function () {
        //自定义参数
        content.execCommand('serverparam', {
            'bizType': App_G.ImgType.Default,
            'bizId': $("#Ticket").val(),
            'createThmImg': App_G.CreateThmImg.None
        });
    });

    //详细
    var context = UE.getEditor('context');
    context.ready(function () {
        //自定义参数
        context.execCommand('serverparam', {
            'bizType': App_G.ImgType.Default,
            'bizId': $("#Ticket").val(),
            'createThmImg': App_G.CreateThmImg.None
        });
    });


    $("#SaveBtn").click(function () {
        $("#SaveBtn").setDisable();

        var data = {
            webIntroduction: App_G.Mapping.Get("data-val", {
                id: App_G.Util.getRequestId('bid'),
                wechat_context: wechat_context.getContent(),
                context: context.getContent()
            })
        };

        $postByAsync("/SiteSetting/SaveWebIntroduction", JSON.stringify(data),
             function (data) {
                 if (data.Code == App_G.Code.Code_200) {
                     App_G.MsgBox.success_digbox();
                     setTimeout("window.location.href='MsgManage';", 1000);
                 } else if (data.State == 1) {
                     App_G.MsgBox.error_digbox("该信息已存在，请修改！");
                 } else {
                     App_G.MsgBox.error_digbox("操作失败！");
                 }
             });
    });
 
});


