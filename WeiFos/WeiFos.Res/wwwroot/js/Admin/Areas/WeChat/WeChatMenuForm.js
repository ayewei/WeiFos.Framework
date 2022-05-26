 

$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

    //初始化微信菜单
    var menu = $("#wxmenu").wechat_menu({
        init_url: "/WeChatModule/WeChat/WeChatMenuInit",
        txt_reply_url: "/WeChatModule/WeChat/TextReplyForm",
        imgtxt_reply_url: "/WeChatModule/WeChat/ImgTextReplyForm",
        get_txt_reply_url: "/WeChatModule/WeChat/GetTextReplys",
        get_imgtxt_reply_url: "/WeChatModule/WeChat/GetImgTextReplys",
        submit: function (data, obj) {

            if (data != null) {
                $post("/WeChatModule/WeChat/SaveWeChatMenu", JSON.stringify(data), function (data) {
                    if (result.Code == App_G.Code.Code_200) {
                        layer.msg("设置成功", { icon: 1 });
                        setTimeout("window.location.href='WeChatMenuManage';", 2000);
                    } else {
                        layer.msg(result.Message, { icon: 2 });

                    }
                });
            }

        }
    });


});

 


 