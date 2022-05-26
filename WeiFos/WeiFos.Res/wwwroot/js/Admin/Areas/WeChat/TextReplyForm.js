$(function () {

    //QQ表情
    $('#reply_contents').qqFace({
        faceAreaId: "attentionQqFace",
        faceboxId: 'facebox'
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

            if ($.trim($("#reply_contents").val()) == "") {
                layer.msg("请填写关键字回复内容", { icon: 2 });
                return;
            }

            var data = {
                entity: {
                    id: App_G.Util.getRequestId('bid'),
                    reply_contents: $('#reply_contents').val()
                },
                keywords: $.trim($("#keywords").val()).split(' ')
            };

            $post("/WeChatModule/WeChat/TextReplyForm", JSON.stringify(data), function (result) {
                if (result.Code == App_G.Code.Code_200) {
                    layer.msg(result.Message, { icon: 1 });
                    var url = App_G.Util.getDomain() + "/WeChatModule/WeChat/TextReplyManage";
                    setTimeout("window.location.href= '" + url + "'", 1000);
                } else if (result.Code == 30060) {
                    layer.msg("已达到定义上限！", { icon: 2 });
                }
                else {
                    layer.msg(result.Message, { icon: 2 });
                }
            });
        }
    });


    var v = yw.valid.getValidate();

    //关键词
    v.valid("#keywords", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入触发关键词,多个关键词用空格隔开" },
        blur: { msg: "触发关键词,多个用空格隔开" },
        repeatvld: {
            msg: "该关键词已经存在",
            url: App_G.Util.getDomain() + "/WeChat/ExistKeyword?bid=" + App_G.Util.getRequestId('bid')
        }
    });

 
});

 