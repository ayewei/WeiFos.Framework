/*!
 * 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
 * Copyright (c) 2013-2018 深圳微狐信息技术有限公司
 * 创 建：叶委
 * 日 期：2018-12-05 11:53:51
 * 描 述：默认回复表单脚本
 */

var entity;


$(function () {

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

            $post("/WeChatModule/WeChat/DefaultReply", JSON.stringify(data), function (result) {
                if (result.Code == App_G.Code.Code_200) {
                    layer.msg(result.Message, { icon: 1 });
                }
                else {
                    layer.msg(result.Message, { icon: 2 });
                }
            });
        }
    });


    var v = yw.valid.getValidate({});

    //公司全称
    v.valid("#lbs_distance", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "默认LBS查询范围，只能输入数字" },
        blur: { msg: "查询范围只能输入数字" },
        aftereId: "unit",
        vtype: verifyType.isNumber
    });

    //关注时关键词
    $("#selectAttention").digbox({
        Selector: "#selectAttention",
        Context: ".controls.box",
        Title: "选择关注时回复关键词",
        Content: attn_keyword_tmp,
        Show: function (s, c, p) {

            //默认图文地址
            var url = '/WeChatModule/WeChat/GetImgTextReplys';

            //表格初始化
            datagrid = p.find("[name=datagrid]").datagrid({
                url: url,
                data: { keywords: p.find("#keyword").val() },
                pager: {
                    index: 0,
                    pageSize: [10, 20]
                },
                template_id: "template",
                column: [
                    {
                        text: "选择",
                        style: "width:1px;"
                    },
                    {
                        text: "关键词",
                        style: "width:150px;"
                    },
                    {
                        text: "创建时间",
                        style: "width:100px;"
                    }
                ],
                completed: function () {
                    //设置选中
                    $.each(p.find("input[name=checkboxs]"), function (i, o) {
                        $.each($("#more_div").children(), function (j, v) {
                            if ($(o).val() == $(v).attr("data-id")) {
                                $(o).prop("checked", true);
                            }
                        });
                    });

                    //查询按钮
                    p.find("[name=search_btn]").click(function () {
                        if (p.find("select").val() == "1") {
                            datagrid.execute({ keywords: p.find("#keyword").val() }, '/WeChatModule/WeChat/GetTextReplys');
                        } else {
                            datagrid.execute({ keywords: p.find("#keyword").val() }, '/WeChatModule/WeChat/GetImgTextReplys');
                        }
                    });

                }
            });
        },
        CallBack: function (s, c, p) {
            var radio = p.find("input[name=radio]:checked");
            if (radio.length) {
                $("#attention_content").val(radio.attr("data-text"));
                if (p.find("select").val() == "1") {
                    $("#a_reply_value").val(radio.attr("data-id") + "#TextReply");
                } else {
                    $("#a_reply_value").val(radio.attr("data-id") + "#ImgTextReply");
                }
            }
            return true;
        }
    });


    //无匹配回复设置
    $("#selectNoMatch").digbox({
        Selector: "#selectNoMatch",
        Context: ".controls.box",
        Title: "选择关注时回复关键词",
        Content: attn_keyword_tmp,
        Show: function (s, c, p) {
            //默认图文地址
            var url = '/WeChatModule/WeChat/GetImgTextReplys';

            //表格初始化
            datagrid = p.find("[name=datagrid]").datagrid({
                url: url,
                data: { keywords: p.find("#keyword").val() },
                pager: {
                    index: 0,
                    pageSize: [10, 20]
                },
                template_id: "template",
                column: [
                    {
                        text: "选择",
                        style: "width:1px;"
                    },
                    {
                        text: "关键词",
                        style: "width:150px;"
                    },
                    {
                        text: "创建时间",
                        style: "width:100px;"
                    }
                ],
                completed: function () {
                    //设置选中
                    $.each(p.find("input[name=checkboxs]"), function (i, o) {
                        $.each($("#more_div").children(), function (j, v) {
                            if ($(o).val() == $(v).attr("data-id")) {
                                $(o).prop("checked", true);
                            }
                        });
                    });

                    //查询按钮
                    p.find("[name=search_btn]").click(function () {
                        if (p.find("select").val() == "1") {
                            datagrid.execute({ keywords: p.find("#keyword").val() }, '/WeChatModule/WeChat/GetTextReplys');
                        } else {
                            datagrid.execute({ keywords: p.find("#keyword").val() }, '/WeChatModule/WeChat/GetImgTextReplys');
                        }
                    });

                }
            });

        },
        CallBack: function (s, c, p) {
            var radio = p.find("input[name=radio]:checked");
            if (radio.length) {
                $("#nomatch_content").val(radio.attr("data-text"));
                if (p.find("select").val() == "1") {
                    $("#d_reply_value").val(radio.attr("data-id") + "#TextReply");
                } else {
                    $("#d_reply_value").val(radio.attr("data-id") + "#ImgTextReply");
                }
            }
            return true;
        }
    });

    //
    $("#introduction").maxlength({ MaxLength: 300 });

});


//关注时关键词
var attn_keyword_tmp = '<div style="700px;width:700px;">\
                                <div class="input-group input-group-sm">\
                                    <select class="form-control form-control-sm" >\
                                        <option value="0">图文回复</option>\
                                        <option value="1">文本回复</option>\
                                    </select>\
                                    <input type="text" id="keyword" class="form-control float-right form-control-sm" placeholder="关键词" />\
                                    <div class="input-group-append">\
                                        <button type="button" class="btn btn-default" name="search_btn" ><i class="fa fa-search"></i></button>\
                                    </div>\
                                </div>\
                              <div name="datagrid" class="mt2"></div>\
                        </div>';