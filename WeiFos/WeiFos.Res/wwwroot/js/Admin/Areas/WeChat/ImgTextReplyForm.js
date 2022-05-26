/*!
 * 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
 * Copyright (c) 2013-2018 深圳微狐信息技术有限公司
 * 创 建：叶委
 * 日 期：2018-12-05 11:53:51
 * 描 述：图文回复表单脚本
 */
document.domain = App_Config.getCrossRes();

//当前实体 推荐阅读已选 ,多图文已选, 图片信息
var entity = null, rec_select_ids = [], more_select_ids = [], img = "", editor;

/*************百度地图 ************/
//百度地图,地图标注 位置坐标
var map, marker, point;

//创建地址解析的实例
var myGeo = new BMap.Geocoder();

//**************提交页面参数 
var ImgTextContentType = {
    /// 外部链接
    OutLink: 0,
    /// 导航信息
    Navigation: 5,
    /// 微活动
    WeiActivity: 10,
    /// 业务类型
    BizType: 15,
    /// 微汽车
    WeiCar: 20,
    /// 微房产
    WeiEstate: 25,
    /// 微餐饮
    WeiCatering: 30,
    /// 微商城
    WeiMall: 35
};


$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

    //映射页面数据
    entity = App_G.Mapping.Load("#entity");
    if (entity != null) {
        if ($("#" + entity.content_type).length) {
            $("#" + entity.content_type).show();
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

            $("#SaveBtn").setDisable({ text: "保 存 中..." });

            //提交验证
            if (validateSubmit()) {

                var data = {
                    imgmsg: img.data == undefined ? "" : img.data,
                    entity: App_G.Mapping.Get("data-val", {
                        id: App_G.Util.getRequestId('bid'),
                        account_id: App_G.Util.getRequestId('aid'),
                        details: editor.getContent(),
                        quote_detailsIds: getMoreImgText(),
                        rec_detailsIds: getRecImgText(),
                        content_value: getImgTextTypeVal()
                    }),
                    keywords: $.trim($("#keywords").val()).split(' ')
                };

                $post("/WeChatModule/WeChat/ImgTextReplyForm", JSON.stringify(data),
                    function (result) {
                        if (result.Code == App_G.Code.Code_200) {
                            layer.msg(result.Message, { icon: 1 });
                            var url = App_G.Util.getDomain() + "/WeChatModule/WeChat/ImgTextReplyManage";
                            setTimeout("window.location.href= '" + url + "'", 1000);
                        } else {
                            layer.msg(result.Message, { icon: 2 });
                        }
                    });
            }

        }
    });

    v = yw.valid.getValidate();

    //关键词
    v.valid("#keywords", {
        tabindex: "#base_panel",
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入关键词" },
        blur: { msg: "请输入关键词" },
        repeatvld: {
            msg: "该关键词已经存在",
            url: "/WeChatModule/WeChat/ExistKeyword?bid=" + App_G.Util.getRequestId('bid')
        }
    });

    //图文标题
    v.valid("#title", {
        tabindex: "#base_panel",
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入图文标题，长度在1~100字符以内" },
        blur: { msg: "图文标题，长度在1~100字符以内" }
    });

    //图文简介
    $("#introduction").maxlength({ MaxLength: 300 });
    v.valid("#introduction", {
        tabindex: "#base_panel",
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入图文简介，长度在1~300字符以内" },
        blur: { msg: "图文简介，长度在1~300字符以内" }
    });

    //回复详细
    editor = UE.getEditor('details', {
        initialFrameHeight: $(document).height() * 0.5
    });
    editor.ready(function () {
        //自定义参数
        editor.execCommand('serverparam', {
            'bizType': App_G.ImgType.ImgTextReply_Details,
            'bizId': App_G.Util.getRequestId('bid'),
            'ticket': $("#DetailsTicket").val(),
            'createThmImg': App_G.CreateThmImg.None
        });
    });

    //选择多图文
    $("#moreImgText").digbox({
        Title: "选择多图文",
        Content: imgtext_reply_tmp,
        Show: function (s, c, p) {

            //表格初始化
            datagrid = p.find("[name=datagrid]").datagrid({
                url: '/WeChatModule/WeChat/GetImgTextReplys',
                data: { keywords: p.find("#keyword").val() },
                pager: {
                    index: 0,
                    pageSize: [10, 20]
                },
                column: [
                    {
                        text: "选择",
                        style: "width:10px;"
                    },
                    {
                        text: "关键词",
                        style: "width:80px;"
                    },
                    {
                        text: "标题",
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

                    p.find("input[name=checkboxs]").click(function () {
                        $("#more_div").children().remove();
                        $("#more_div").append($(checkparams(p, "more")));
                    });

                    //查询按钮
                    p.find("#search_btn").click(function () {
                        datagrid.execute({ keywords: p.find("#keyword").val() });
                    });
                }
            });

        },
        CallBack: function (s, c, p) { 
            return true;
        }
    });

    //推荐阅读
    $("#moreRec").digbox({
        Title: "选择推荐阅读",
        Content: imgtext_reply_tmp,
        Show: function (s, c, p) {

            //表格初始化
            datagrid = p.find("[name=datagrid]").datagrid({
                url: '/WeChatModule/WeChat/GetImgTextReplys',
                data: { keywords: p.find("#keyword").val() },
                pager: {
                    index: 0,
                    pageSize: [10, 20]
                },
                column: [
                    {
                        text: "选择",
                        style: "width:20px;"
                    },
                    {
                        text: "关键词",
                        style: "width:80px;"
                    },
                    {
                        text: "标题",
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
                        $.each($("#rec_div").children(), function (j, v) {
                            if ($(o).val() == $(v).attr("data-id")) {
                                $(o).prop("checked", true);
                            }
                        });
                    });

                    p.find("input[name=checkboxs]").click(function () {
                        $("#rec_div").children().remove();
                        $("#rec_div").append($(checkparams(p, "rec")));
                    });

                    //查询按钮
                    p.find("#search_btn").click(function () {
                        datagrid.execute({ keywords: p.find("#keyword").val() });
                    });
                }
            });

        },
        CallBack: function (s, c, p) { 
            return true;
        }
    });

    //添加所属其它栏目
    $("#add_other_category").digbox({
        Title: "选择所显示栏目",
        Context: "",
        Show: function (submit_id, current_obj, panel) {

            var url = App_G.Util.getDomain() + '/TSHProductModule/Product/GetProducts?rnd=' + Math.random();
            var pager = $(panel.find("#PagerDiv")).pager({
                url: url,
                data: { categoryID: $("#GuideCategoryID option:selected").val(), brandID: 0, productName: panel.find("#SupplierID").val(), IsPostage: -1, Status: -1, createDate: panel.find("#createDate").val() },
                currentPage: 0,
                pageSize: 4,
                callback: function (data) {
                    if (panel.find("#listTable tbody").find("tr").length > 0) {
                        panel.find("#listTable tbody tr").remove();
                    }
                    panel.find("#listTable tbody").append(template("template", data));
                }
            });

            //查询按钮
            $("#search_btn").click(function () {
                pager.execute({ categoryID: $("#GuideCategoryID option:selected").val(), productName: panel.find("#ProductName").val() });
            });
        },
        CallBack: function (submit_id, current_obj, panel) {
            var selects = $("#Attention_Frame").contents().find("body").find("#tbd_keyword input[name=kwradio]:checked");
            if (selects.is(":checked")) {
                $("#attention_content").val(selects.attr("title"));
                $("#a_reply_value").val(selects.val());
            }
            return true;
        }
    });

    //图文回复封面上传
    $("#imgtext_reply_img").upload({
        title: "选择图片",
        defimgurl: $("#defurl").val(),
        imgurl: $("#imgurl").val(),
        cData: {
            bizType: App_G.ImgType.ImgTextReply_Title,
            bizId: App_G.Util.getRequestId('bid'),
            ticket: $("#Ticket").val(),
            createThmImg: App_G.CreateThmImg.CreateALL,
        },
        imgHeight: "120px",
        imgWidth: "215px",
        maxSize: 0.3,
        callback: function (data) {
            img = data;
        }
    });

    //返回
    $("#BackBtn").click(function () {
        window.location.href = "ProductManage?" + window.location.href.split("?")[1];
    });

    //图文消息类型
    $("#content_type").change(function () {
        var selectval = $(this).val();
        $("[name=imgtexttr]").hide();
        $("#" + selectval).show();
    });

    $("#BizTypeDDL").change(function () {
        var selectval = $(this).val();
        $("[name=activitys]").hide();
        $("#" + selectval).show();
    });

    //微活动
    $.each($("#WeiActivity").find("tbody"), function (i, o) {
        if ($("#BizTypeDDL").val() == $(o).attr("id")) {
            $(o).show();
        }
    });

    //获取初始化位置
    var init_cp = $("#lat_lng").val();
    var lat = init_cp.split(',')[0];
    var lng = init_cp.split(',')[1];

    //记录当前位置
    point = new BMap.Point(lng, lat);

    map = new BMap.Map("container");

    //初始化地图。设置中心点和地图级别
    map.centerAndZoom(point, 14);

    //添加鱼骨控件
    map.addControl(new BMap.NavigationControl());

    //缩略地图控件
    map.addControl(new BMap.OverviewMapControl());

    //比例尺控件
    map.addControl(new BMap.ScaleControl());

    //启用滚轮放大缩小，默认禁用
    map.enableScrollWheelZoom();

    //启用地图惯性拖拽，默认禁用
    map.enableContinuousZoom();

    // 创建标注
    marker = new BMap.Marker(point);

    // 将标注添加到地图中
    map.addOverlay(marker);

    //跳动的动画
    marker.setAnimation(BMAP_ANIMATION_BOUNCE);

    //标注拖拽事件
    marker.enableDragging();
    marker.addEventListener("dragend", function (e) {
        var _cp = marker.getPosition();
        //获取marker的位置
        point = new BMap.Point(_cp.lng, _cp.lat);

        myGeo.getLocation(point, function (result) {
            if (result) {
                $("#address_text").val(result.address);
                map.panTo(point);
            }
        });
    });

    // 百度地图拖动事件
    map.addEventListener("dragend", function showInfo() {
        var _cp = map.getCenter();
        //记录当前位置
        point = new BMap.Point(_cp.lng, _cp.lat);

        myGeo.getLocation(point, function (result) {
            if (result) {
                $("#address_text").val(result.address);
                marker.setPosition(point);
            }
        });
    });

    fun_geocoder_getPoint();

});



//地址解析的函数
function fun_geocoder_getPoint() {
    myGeo.getPoint($("#address_text").val(), function (_point) {
        if (_point) {
            map.centerAndZoom(_point, 14);
            marker.setPosition(_point);
        }
    }, "全国");
}
/*************百度地图 ************/


//获取多图文 参数
function getMoreImgText() {
    var item = $("div#more_div").find("div[name=more_imgtexts]");
    var params = "";

    $.each(item, function (i, o) {
        if (i == 0) {
            params += $(o).attr("data-id");
        } else {
            params += "," + $(o).attr("data-id");
        }
    })
    return params;
}

//推荐阅读
function getRecImgText() {
    var item = $("div#rec_div").find("div[name=rec_imgtexts]");
    var params = "";

    $.each(item, function (i, o) {
        if (i == 0) {
            params += $(o).attr("data-id");
        } else {
            params += "," + $(o).attr("data-id");
        }
    });
    return params;
}

//验证提交
function validateSubmit() {

    var v = true;

    if (editor.getContent() == "") {
        layer.msg("请填写回复内容详情", { icon: 2 });
        v = false;
    }

    //是否上传图片
    if ($("img.thumb_img").attr("src") == $("img.thumb_img").attr("default-src")) {
        layer.msg("选上传封面图片", { icon: 2 });
        v = false;
    }

    //外链类型内容验证
    switch (parseInt($("#content_type").val())) {
        case ImgTextContentType.OutLink:
            if (!validateType.isUrl($("#OutLinkTxt").val())) {
                layer.msg("外部连接：必须是正确的URL格式", { icon: 2 });
                v = false;
            }
            break;
        case ImgTextContentType.WeiActivity:
            var rd = $("input[name=activity_radio]:checked");
            if (!$("input[name=activity_radio]:checked").length > 0) {
                v = false;
                layer.msg("至少选择一个正在进行中的活动", { icon: 2 });
            }
            break;
    }
    return v;
}

//获取图文类型参数
function getImgTextTypeVal() {

    switch (parseInt($("#content_type").val())) {
        //外部连接
        case ImgTextContentType.OutLink:
            return $("#OutLinkTxt").val();

        //导航信息
        case ImgTextContentType.Navigation:
            return point.lat + "," + point.lng + "#" + $("#address_text").val();

        //微活动
        case ImgTextContentType.WeiActivity:
            //获取选中活动
            return $("#WeiActivity").find("input[name=activity_radio]:checked").val();
        //业务模块
        case ImgTextContentType.BizType:
            return "";

        //微汽车
        case ImgTextContentType.WeiCar:
            return "";

        //微房产
        case ImgTextContentType.WeiEstate:
            return "";

        default:
            return "";
    }

}

//弹出框勾选参数
function checkparams(panel, type) {

    var html = "";
    var divname = "";

    if (type == "more") {
        divname = "more_imgtexts";
    } else if (type == "rec") {
        divname = "rec_imgtexts";
    }

    var checkboxs = panel.find("input[name=checkboxs]:checked");
    if (checkboxs.length > 9) {
        App_G.MsgBox.error_digbox("最多勾选九条记录！");
        return;
    }
    //遍历checkbox集合
    $.each(checkboxs, function (i, o) {
        //最多9个
        if (i < 9) {
            html += "<div name='" + divname + "' data-id='" + $(this).val() + "' class='moreimgtext_d' >"
                + "<a class='btn-mini del btn_del' href='javascript:;'><i class='fa fa-remove'></i></a>" + $(this).attr("title") + "</div>";
        }
    });

    return html;
}


//图文回复模板
var imgtext_reply_tmp = '<div style="700px;width:700px;">\
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
