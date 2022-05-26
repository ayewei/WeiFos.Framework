
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

    //资讯分类图片
    $("#informt_img").upload({
        title: "选择图片",
        defimgurl: $("#defurl").val(),
        imgurl: $("#imgurl").val(),
        cData: {
            bizType: App_G.ImgType.Informt,
            bizId: App_G.Util.getRequestId('bid'),
            ticket: $("#Ticket").val(),
            createThmImg: App_G.CreateThmImg.CreateALL,
        },
        imgHeight: "130px",
        imgWidth: "235px",
        maxSize: 0.3,
        callback: function (data) {
            img = data;
        }
    });

    //商品详细
    var editor = UE.getEditor('context');
    editor.ready(function () {
        //自定义参数
        editor.execCommand('serverparam', {
            'bizType': App_G.ImgType.InformtDetails,
            'bizId': App_G.Util.getRequestId('bid'),
            'ticket': $("#DetailsTicket").val(),
            'createThmImg': App_G.CreateThmImg.None
        });
    });


    yw.valid.config({
        submitelements: "#SaveBtn", vsuccess: function () {

            $("#SaveBtn").setDisable();
           
            var data = {
                informt: App_G.Mapping.Get("data-val", { id: App_G.Util.getRequestId('bid'), context: editor.getContent() }),
                imgmsg: img.data == undefined ? "" : img.data
            };

            //console.log(data);
            $post("/SiteSetting/SaveInformt", JSON.stringify(data),
                 function (data) {
                     if (data.Code == App_G.Code.Code_200) {
                         App_G.MsgBox.success_digbox();
                         setTimeout("window.location.href='InformtManage';", 1000);
                     } else {
                         App_G.MsgBox.error_digbox("操作失败！");
                     }
                 });
        }
    });

    var v = yw.valid.getValidate();
    v.valid("#title", { tabindex: "#base", focusmsg: "请输入资讯标题", errormsg: "", vtype: verifyType.anyCharacter });
    v.valid("#introduction", { tabindex: "#base", focusmsg: "请输入分类简介", errormsg: "", othermsg: "", vtype: verifyType.anyCharacter });
    v.valid("#order_index", { tabindex: "#base", focusmsg: "请输入排序索引", errormsg: "索引只能够是正整数", vtype: verifyType.isNumber });
    v.valid("#source", { tabindex: "#base", focusmsg: "请输入资讯来源", errormsg: "", vtype: verifyType.anyCharacter });
    v.valid("#author", { tabindex: "#base", focusmsg: "请输入资讯作者", errormsg: "", vtype: verifyType.anyCharacter });
    v.valid("#release_date", { tabindex: "#base", focusmsg: "请输入资讯发布日期", errormsg: "", vtype: verifyType.isDate });
    v.valid("#reprinted_url", { tabindex: "#base", focusmsg: "请输入原文转载地址", errormsg: "请输入原文转载地址", selectvalidate: true, vtype: verifyType.anyCharacter });

    //是否转载
    $("#is_reprinted").click(function () {
        if ($(this).is(':checked')) {
            $("#reprinted_tr").show();
        } else {
            $("#reprinted_tr").hide();
        }
    });
    
    //选择 页面seo标签
    $("#select_seotitle").digbox({
        Selector: "table.table_s1",
        Title: "页面seo标签",
        Context: title_tmp,
        Show: function (b, c, p) {

            var data = {
                name: p.find("#name").val(), ctgy_id: 0, createdDate: p.find("#kw_createDate").val(), type: 0
            };

            $("#kw_createDate").daterangepicker({
                language: 'zh-CN',
                timePicker: true,
                locale: {
                    applyLabel: '确定',
                    cancelLabel: '取消',
                    fromLabel: '起始时间',
                    toLabel: '结束时间',
                    customRangeLabel: '自定义',
                    daysOfWeek: ['日', '一', '二', '三', '四', '五', '六'],
                    monthNames: ['一月', '二月', '三月', '四月', '五月', '六月',
                            '七月', '八月', '九月', '十月', '十一月', '十二月'],
                    firstDay: 1
                },
                format: 'YYYY/MM/DD'
            });

            //翻页查询 
            var p_pager = p.find("#TitlePagerDiv").pager({
                url: App_G.Util.getDomain() + '/SeoSetting/GetSeoKeyWords?rnd=' + Math.random(),
                data: data,
                currentPage: 0,
                pageSize: 5,
                callback: function (data) {
                    if (p.find("#listTable tbody").find("tr").length > 0) {
                        p.find("#listTable tbody tr").remove();
                    }
                    p.find("#listTable tbody").append(template("title_template", data));

                    //项目ID
                    var bid = $("#select_bid").attr("data-val");
                    //设置选中radio
                    $.each(p.find("#listTable tbody [type=radio]"), function (i, o) {
                        if ($(o).val() == bid) {
                            $(o).prop("checked", "checked");
                        }
                    });
                }
            });

            p.find("#search_btn").click(function () {
                p_pager.execute({
                    loginName: p.find("#name").val(), status: -1, accountType: -1
                });
            });

        },
        CallBack: function (b, c, p) {
            var checkboxs = p.find("input[name=kw_checkbox]:checked");
            var txt = "";
            $.each(checkboxs, function (i, o) {
                if ($(o).is(":checked")) {
                    txt += $(o).attr("title") + "、";
                }
            });

            if (txt.indexOf('、') != -1) {
                txt = txt.substr(0, txt.length - 1);
            }

            var word = $("#seo_title");
            if ($.trim(word.val()) != "") {
                if (word.val().substring(word.val().length - 1, word.val().length) != "、") {
                    $("#seo_title").val(word.val() + "、" + txt);
                } else {
                    $("#seo_title").val(word.val() + txt);
                }

            } else {
                $("#seo_title").val(txt);
            }

            return true;
        }
    });

    //选择 页面keyword标签
    $("#select_seokeyword").digbox({
        Selector: "table.table_s1",
        Title: "页面seo标签",
        Context: title_tmp,
        Show: function (b, c, p) {

            var select_p = $("#kw_ctgy_id").clone();
            $(select_p).show();
            p.find("#select_li").append(select_p);

            var data = {
                name: p.find("#name").val(), ctgy_id: p.find("#kw_ctgy_id").val(), createdDate: p.find("#kw_createDate").val(), type: 1
            };

            $("#kw_createDate").daterangepicker({
                language: 'zh-CN',
                timePicker: true,
                locale: {
                    applyLabel: '确定',
                    cancelLabel: '取消',
                    fromLabel: '起始时间',
                    toLabel: '结束时间',
                    customRangeLabel: '自定义',
                    daysOfWeek: ['日', '一', '二', '三', '四', '五', '六'],
                    monthNames: ['一月', '二月', '三月', '四月', '五月', '六月',
                            '七月', '八月', '九月', '十月', '十一月', '十二月'],
                    firstDay: 1
                },
                format: 'YYYY/MM/DD'
            });

            //翻页查询 
            var p_pager = p.find("#TitlePagerDiv").pager({
                url: App_G.Util.getDomain() + '/SeoSetting/GetSeoKeyWords?rnd=' + Math.random(),
                data: data,
                currentPage: 0,
                pageSize: 5,
                callback: function (data) {
                    if (p.find("#listTable tbody").find("tr").length > 0) {
                        p.find("#listTable tbody tr").remove();
                    }
                    p.find("#listTable tbody").append(template("kw_template", data));

                    //项目ID
                    var bid = $("#select_bid").attr("data-val");
                    //设置选中radio
                    $.each(p.find("#listTable tbody [type=radio]"), function (i, o) {
                        if ($(o).val() == bid) {
                            $(o).prop("checked", "checked");
                        }
                    });
                }
            });

            p.find("#search_btn").click(function () {
                p_pager.execute({
                    loginName: p.find("#name").val(), status: -1, accountType: -1
                });
            });

        },
        CallBack: function (b, c, p) {
            var checkboxs = p.find("input[name=kw_checkbox]:checked");
            var txt = "";
            $.each(checkboxs, function (i, o) {
                if ($(o).is(":checked")) {
                    txt += $(o).attr("title") + "、";
                }
            });

            if (txt.indexOf('、') != -1) {
                txt = txt.substr(0, txt.length - 1);
            }

            var word = $("#seo_keyword");
            if ($.trim(word.val()) != "") {
                if (word.val().substring(word.val().length - 1, word.val().length) != "、") {
                    $("#seo_keyword").val(word.val() + "、" + txt);
                } else {
                    $("#seo_keyword").val(word.val() + txt);
                }

            } else {
                $("#seo_keyword").val(txt);
            }

            return true;
        }
    });

    //选择 页面description标签
    $("#select_seodescription").digbox({
        Selector: "table.table_s1",
        Title: "页面seo标签",
        Context: title_tmp,
        Show: function (b, c, p) {

            var data = {
                name: p.find("#name").val(), ctgy_id: 0, createdDate: p.find("#kw_createDate").val(), type: 0
            };

            $("#kw_createDate").daterangepicker({
                language: 'zh-CN',
                timePicker: true,
                locale: {
                    applyLabel: '确定',
                    cancelLabel: '取消',
                    fromLabel: '起始时间',
                    toLabel: '结束时间',
                    customRangeLabel: '自定义',
                    daysOfWeek: ['日', '一', '二', '三', '四', '五', '六'],
                    monthNames: ['一月', '二月', '三月', '四月', '五月', '六月',
                            '七月', '八月', '九月', '十月', '十一月', '十二月'],
                    firstDay: 1
                },
                format: 'YYYY/MM/DD'
            });

            //翻页查询 
            var p_pager = p.find("#TitlePagerDiv").pager({
                url: App_G.Util.getDomain() + '/SeoSetting/GetSeoKeyWords?rnd=' + Math.random(),
                data: data,
                currentPage: 0,
                pageSize: 5,
                callback: function (data) {
                    if (p.find("#listTable tbody").find("tr").length > 0) {
                        p.find("#listTable tbody tr").remove();
                    }
                    p.find("#listTable tbody").append(template("title_template", data));

                    //项目ID
                    var bid = $("#select_bid").attr("data-val");
                    //设置选中radio
                    $.each(p.find("#listTable tbody [type=radio]"), function (i, o) {
                        if ($(o).val() == bid) {
                            $(o).prop("checked", "checked");
                        }
                    });
                }
            });

            p.find("#search_btn").click(function () {
                p_pager.execute({
                    loginName: p.find("#name").val(), status: -1, accountType: -1
                });
            });

        },
        CallBack: function (b, c, p) {
            var checkboxs = p.find("input[name=kw_checkbox]:checked");
            var txt = "";
            $.each(checkboxs, function (i, o) {
                if ($(o).is(":checked")) {
                    txt += $(o).attr("title") + "、";
                }
            });

            if (txt.indexOf('、') != -1) {
                txt = txt.substr(0, txt.length - 1);
            }

            var word = $("#seo_description");
            if ($.trim(word.val()) != "") {
                if (word.val().substring(word.val().length - 1, word.val().length) != "、") {
                    $("#seo_description").val(word.val() + "、" + txt);
                } else {
                    $("#seo_description").val(word.val() + txt);
                }

            } else {
                $("#seo_description").val(txt);
            }

            return true;
        }
    });


});




//页面seo标签 翻页
var title_tmp = '<div style="height:480px;width:900px;"  >\
           <ul class="clearfix shopSearch">\
           <li id="select_li" >  </li>\
           <li> <input type="text" class="input_serch" id="Name" placeholder="名称" /> </li>\
           <li>\
                <div class="input-prepend">\
                    <span style="margin-right: -4px" class="add-on"><i class="icon-calendar"></i></span>\
                    <input type="text" style="width:150px;" onfocus="$(this).blur()" class="daterangepick input-xlarge" id="kw_createDate" placeholder="日期范围" />\
                    <a href="javascript:;" class="link_blue" id="clear_a">清空</a>\
                </div>\
            </li>\
            <li><input id="search_btn" type="button" value="搜索" class="btn btn-primary btn_search" /></li>\
        </ul>\
        <table id="listTable" class="table table-bordered ">\
            <thead>\
                <tr role="row">\
                    <th role="row" class="">选择</th>\
                    <th class="sorting" rowspan="1" colspan="1">排序索引</th>\
                    <th class="sorting" rowspan="1" colspan="1">所属分类</th>\
                    <th class="sorting" rowspan="1" colspan="1">标签名称</th>\
                    <th class="sorting" rowspan="1" colspan="1">是否启用</th>\
                    <th class="sorting" rowspan="1" colspan="1">创建日期</th>\
                </tr>\
            </thead>\
            <tbody class="tb_am" >\
            </tbody>\
        </table><div id="TitlePagerDiv"></div></div>';


//页面keyword 翻页
var keyword_tmp = '<div style="height:480px;width:900px;"  >\
           <ul class="clearfix shopSearch">\
           <li> <input type="text" class="input_serch" id="Name" placeholder="名称" /> </li>\
           <li>\
                <div class="input-prepend">\
                    <span style="margin-right: -4px" class="add-on"><i class="icon-calendar"></i></span>\
                    <input type="text" style="width:150px;" onfocus="$(this).blur()" class="daterangepick input-xlarge" id="CreatedDate" placeholder="日期范围" />\
                    <a href="javascript:;" class="link_blue" id="clear_a">清空</a>\
                </div>\
            </li>\
            <li><input id="search_btn" type="button" value="搜索" class="btn btn-primary btn_search" /></li>\
        </ul>\
        <table id="listTable" class="table table-bordered ">\
            <thead>\
                <tr role="row">\
                    <th role="row" class="">选择</th>\
                    <th class="sorting" rowspan="1" colspan="1">所属分类</th>\
                    <th class="sorting" rowspan="1" colspan="1">所属分类</th>\
                    <th class="sorting" rowspan="1" colspan="1">关键词名称</th>\
                    <th class="sorting" rowspan="1" colspan="1">排序索引</th>\
                    <th class="sorting" rowspan="1" colspan="1">是否启用</th>\
                    <th class="sorting" rowspan="1" colspan="1">创建日期</th>\
                </tr>\
            </thead>\
            <tbody class="tb_am" >\
            </tbody>\
        </table><div id="KeywordPagerDiv"></div></div>';


//页面description 翻页
var description_tmp = '<div style="height:480px;width:900px;"  >\
           <ul class="clearfix shopSearch">\
           <li> <input type="text" class="input_serch" id="Name" placeholder="名称" /> </li>\
           <li>\
                <div class="input-prepend">\
                    <span style="margin-right: -4px" class="add-on"><i class="icon-calendar"></i></span>\
                    <input type="text" style="width:150px;" onfocus="$(this).blur()" class="daterangepick input-xlarge" id="CreatedDate" placeholder="日期范围" />\
                    <a href="javascript:;" class="link_blue" id="clear_a">清空</a>\
                </div>\
            </li>\
            <li><input id="search_btn" type="button" value="搜索" class="btn btn-primary btn_search" /></li>\
        </ul>\
        <table id="listTable" class="table table-bordered ">\
            <thead>\
                <tr role="row">\
                    <th role="row" class="" >选择</th>\
                    <th class="sorting" rowspan="1" colspan="1">所属分类</th>\
                    <th class="sorting" rowspan="1" colspan="1">标签名称</th>\
                    <th class="sorting" rowspan="1" colspan="1">排序索引</th>\
                    <th class="sorting" rowspan="1" colspan="1">是否启用</th>\
                    <th class="sorting" rowspan="1" colspan="1">创建日期</th>\
                </tr>\
            </thead>\
            <tbody class="tb_am" >\
            </tbody>\
        </table><div id="DesPagerDiv"></div></div>';
