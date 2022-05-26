
var menus = null,id;

$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

    //添加自定义菜单
    $("div.r-3x.b.b-dashed.wrapper-md").click(function () {
        window.location.href = "WeChatMenuForm";
    });

    //页面数据绑定
    if ($.trim($("#menus").val()).length > 0) {
        menus = $.parseJSON($("#menus").val());
        if (menus.length > 0) {
            //隐藏新增按钮
            $("[name=addmenu_btn]").addClass("dpn");
            $("ul.ul-unstyle.list-block.padder-md.p-t-20.ng-scope").append(template("template", { data: menus }));
        } else {
            //隐藏新增按钮
            $("[name=addmenu_btn]").removeClass("dpn");
        } 
    }

    //按钮是否可用
    $("div.vbox.pos-rlt").on("click", "li[data-id]", function (e) {
        e.stopPropagation();

        //样式设置
        $(this).addClass("active").siblings().removeClass("active");

        //是否可用
        var is_enable = $(this).attr("data-enable");
        if (is_enable == "1") {
            $("[name=enable_btn]").addClass("dpn");
            $("[name=dis_enable_btn]").removeClass("dpn");
        } else { 
            $("[name=enable_btn]").removeClass("dpn");
            $("[name=dis_enable_btn]").addClass("dpn");
        }

        id = $(this).attr("data-id");
        //显示修改按钮
        $("[name=edit_btn]").removeClass("dpn");
    });

    //修改
    $("[name=edit_btn]").click(function () {
        var li = $("li.pos-rlt.wrapper-sm.ng-scope.active");
        if (li.length > 0) {
            window.location.href = "/WeChatModule/WeChat/WeChatMenuForm?bid=" + li.attr("data-id");
        }
    });

    //启用
    $("[name=enable_btn]").digbox({
        Selector: ".wrapper-sm.padder-md",
        Title: "信息提示框",
        Context: "确定启用该菜单吗？", CallBack: function (s, c, p) {
            $get("/WeChatModule/WeChat/SetEnableMenuGroup?bid=" + id + "&is_enable=true", "",
                function (data) {
                    if (data.State == App_G.BackState.State_200) {
                        App_G.MsgBox.success_digbox();
                        var url = App_G.Util.getDomain() + "/WeChatModule/WeChat/WeChatMenuManage";
                        setTimeout("window.location.href ='" + url + "'", 1000);
                    } else {
                        App_G.MsgBox.error_digbox();
                    }
                });
        }
    });

    //禁用
    $("[name=dis_enable_btn]").digbox({
        Selector: ".wrapper-sm.padder-md",
        Title: "信息提示框",
        Context: "确定禁用该菜单吗？", CallBack: function (s, c, p) {
            $get("/WeChatModule/WeChat/SetEnableMenuGroup?bid=" + id +"&is_enable=false", "",
                function (data) {
                    if (data.State == App_G.BackState.State_200) {
                        App_G.MsgBox.success_digbox();
                        var url = App_G.Util.getDomain() + "/WeChatModule/WeChat/WeChatMenuManage";
                        setTimeout("window.location.href ='" + url + "'", 1000);
                    } else {
                        App_G.MsgBox.error_digbox();
                    }
                });
        }
    });



    
})

 
 
