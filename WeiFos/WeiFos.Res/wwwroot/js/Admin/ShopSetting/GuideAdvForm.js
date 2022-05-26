
//上传图片信息
var img = "";

//业务值
var val = "0";

var guide_c_html = "";

$(function () {

    guide_c_html = '<select id="guide_category_id" data-val="guide_category_id" ><option value="0">请选择</option>' + $("#guide_category_id").html() + "</select>";
    var tempHeader = '<div style="700px;width:700px;">\
                            <ul class="clearfix shopSearch">\
                                <li>\
                                    <input type="text" placeholder="商品名称" class="input_serch" id="ProductName" maxlength="50" />  ' + guide_c_html + ' \
                                </li>\
                                <li>\
                                    <input type="button" class="btn btn-primary btn_search" id="search_btn" value="搜索" name="search_Btn">\
                                </li>\
                            </ul>\
                            <table id="listTable" class="table table-bordered ">\
                                <thead>\
                                    <tr role="row">\
                                        <th class="sorting tbWidth2" rowspan="1" colspan="1" style="width:10px;"></th>\
                                        <th class="align_left" rowspan="1" colspan="1" style="width:350px;" rowspan="1" colspan="1" >商品名</th>\
                                        <th class="align_left" rowspan="1" colspan="1">所属分类</th>\
                                    </tr>\
                                </thead>\
                                <tbody role="alert" aria-live="polite" aria-relevant="all" id="tbd_ategory" class="tb_am">\
                                </tbody>\
                            </table>\
                            <div id="PagerDiv"></div>\
                        </div>';

 
    $("#adv_img").upload({
        title: "选择图片",
        defimgurl: $("#defimgurl").val(),
        imgurl: $("#imgurl").val(), 
        cData: {
            bizType: App_G.ImgType.Advertise,
            bizId: App_G.Util.getRequestId('bid'),
            ticket: $("#Ticket").val(),
            createThmImg: App_G.CreateThmImg.CreateS,
        },
        createThmImg: App_G.CreateThmImg.CreateS,
        imgHeight: "110px",
        imgWidth: "220px",
        maxSize: 0.3,
        callback: function (data) {
            img = data;
        }
    });

    //初始化验证插件
    yw.valid.config({
        submitelements: "#SaveBtn", vsuccess: function () {
            $("#SaveBtn").setDisable({ text: "保 存 中..." });

            if ($("#guide_category_id").find("option").length == 0) {
                App_G.MsgBox.error_digbox("没有导购分类");
                return;
            }
             
            var data = {
                adv: App_G.Util.getJson("data-val", { id: App_G.Util.getRequestId('bid'), content_value: get_content_value() }),
                imgMsg: img.data == undefined ? "" : img.data
            };

            $post("/ShopSetting/SaveAdvertise", JSON.stringify(data),
                function (data) {
                    if (data.Code == App_G.Code.Code_200) {
                        App_G.MsgBox.success_digbox();
                        var url = "/ShopSetting/GuideAdvManage";
                        setTimeout("window.location.href='" + url + "';", 1000);
                    } else if (data.State == 701) {
                        App_G.MsgBox.error_digbox(data.Message);
                    } else {
                        App_G.MsgBox.error_digbox("操作失败");
                    } 
                });

        }
    });


    v = yw.valid.getValidate();

    //分类名称
    v.valid("#adimg_name", { focusmsg: "请输入导购图名称", errormsg: "输入导购图名称", vtype: verifyType.anyCharacter });

    //排序索引
    v.valid("#order_index", { focusmsg: "请输入排序索引", errormsg: "排序索引，只能是数字", vtype: verifyType.isNumber });

    if (!$("#type_tr_1").is(":hidden")) {
        v.valid("#link_url", { focusmsg: "请输入链接地址", errormsg: "链接地址格式有误", vtype: verifyType.isUrl });
    }

    //页面数据绑定
    if ($.trim($("#entity").val()).length > 0) {
        var entity = $.parseJSON($("#entity").val());
        App_G.Util.bindJson("data-val", entity);
        val = entity.content_value;
    }

    //类型选择改变事件
    $("#content_type").change(function () {
        $("tr[name=type_tr]").hide();
        $("#type_tr_" + $(this).val()).show();
        if (!$("#type_tr_1").is(":hidden")) {
            v.valid("#link_url", { focusmsg: "请输入链接地址", errormsg: "链接地址格式有误", vtype: verifyType.isUrl });
        } else {
            yw.valid.ves.deleteItem("link_url");
        }
    });


    $("[name=add_product_btn]").digbox({
        Selector: ".table_s1",
        Title: "选择商品",
        Context: tempHeader,
        Show: function (submit_id, current_obj, panel) {

            //panel.find("ul.clearfix.shopSearch li:eq(0)").append($("#CategoryID").clone());

            var url = App_G.Util.getDomain() + '/ProductModule/Product/GetProducts?rnd=' + Math.random();
            var pager = $(panel.find("#PagerDiv")).pager({
                url: url,
                data: { guideCgtyID: panel.find("#guide_category_id option:selected").val(), cgtyID: 0, brandID: 0, productName: panel.find("#ProductName").val(), createDate: "" },
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
                pager.execute({ guideCgtyID: panel.find("#guide_category_id option:selected").val(), cgtyID: 0, brandID: 0, productName: panel.find("#ProductName").val(), createDate: "" });
            });
        },
        CallBack: function (submit_id, current_obj, panel) {
            val = panel.find("input[type='radio']:checked").val();
            if (panel.find("input[type='radio']:checked").length) {
                $("#singe_pdt_td").children().remove();
                $("#singe_pdt_tb").find("#singe_pdt_td").append(panel.find("input[type='radio']:checked").parent().next().children());
                $("#singe_pdt_td").find("p:eq(1)").remove();
            }
            return true;
        }
    });

});

//获取内容值
function get_content_value() {
    if (!$("#type_tr_1").is(":hidden")) {
        return $("#link_url").val();
    }
    return val;
}

