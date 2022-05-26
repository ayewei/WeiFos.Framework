
var account, acount_ent, code_merchant, app_merchant, opensetting, openAcount, v, v1, v2, v3, v4, v5;

$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

    yw.valid.config({});

    /***************微信公众号start******************/

    v = yw.valid.getValidate({
        submiteles: "#SaveWeChatBtn", vsuccess: function () {

            $("#SaveWeChatBtn").setDisable({ text: "保 存 中 ..." });

            var data = {
                entity: App_G.Mapping.Get("data-val"),
                imgmsg: ""
            };

            $post("/WeChat/SaveAccount", JSON.stringify(data),
                function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        layer.msg(result.Message, { icon: 1 });
                    } else {
                        layer.msg(result.Message, { icon: 2 });
                    }
                });

        }
    });

    //公众号名称
    v.valid("#wechat_no", { selectvld: true, vtype: verifyType.anyCharacter, focus: { msg: "请输入公众号微信号" }, blur: { msg: "" } });

    //公众号名称
    v.valid("#name", { selectvld: true, vtype: verifyType.anyCharacter, focus: { msg: "请输入公众号名称" }, blur: { msg: "" } });

    //请输入公众号的appid
    v.valid("#wx_account_appid", { selectvld: true, vtype: verifyType.isSerialNumber, focus: { msg: "请输入公众号的AppID" }, blur: { msg: "AppID格式不正确" } });

    //公众号的AppSecret
    v.valid("#wx_account_app_secret", { selectvld: true, vtype: verifyType.isSerialNumber, focus: { msg: "请输入公众号的应用密钥" }, blur: { msg: "AppSecret格式不正确" } });

    //公众号商户
    v.valid("#wx_account_mch_id", { selectvld: true, vtype: verifyType.isSerialNumber, focus: { msg: "请输入公众号商户号" }, blur: { msg: "商户号格式不正确" } });

    //公众号商户
    v.valid("#wx_account_pay_key", { selectvld: true, vtype: verifyType.isSerialNumber, focus: { msg: "请输入公众号API秘钥" }, blur: { msg: "公众号API秘钥格式不正确" } });

    //公众号API秘钥
    v.valid("#server_url", { selectvld: true, vtype: verifyType.isUrl, focus: { msg: "请输入服务器地址" }, blur: { msg: "请输入服务器地址" } });

    /***************微信公众号start******************/



    /***************微信开放平台start******************/

    v1 = yw.valid.getValidate({
        submiteles: "#SaveOpenBtn", vsuccess: function () {

            $("#SaveOpenBtn").setDisable({ text: "保 存 中 ..." });

            var data = App_G.Mapping.Get("data-open");

            $post("/WeChat/SaveOpenSetting", JSON.stringify(data),
                function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        layer.msg(result.Message, { icon: 1 });
                    } else {
                        layer.msg(result.Message, { icon: 2 });
                    }
                });

        }
    });

    //开放平台 appid
    v1.valid("#component_appid", { vtype: verifyType.isSerialNumber, focus: { msg: "请输入开放平台appid" }, blur: { msg: "" } });

    //开放平台 秘钥
    v1.valid("#component_appsecret", { vtype: verifyType.isSerialNumber, focus: { msg: "请输入开放平台应用秘钥" }, blur: { msg: "" } });

    /***************微信开放平台end******************/



    /***************微信企业号start******************/

    v2 = yw.valid.getValidate({
        submiteles: "#SaveEntBtn", vsuccess: function () {

            $("#SaveEntBtn").setDisable({ text: "保 存 中 ..." });

            var data = App_G.Mapping.Get("data-val-ent");

            $post("/WeChat/SaveAccountEnt", JSON.stringify(data),
                function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        layer.msg(result.Message, { icon: 1 });
                    } else {
                        layer.msg(result.Message, { icon: 2 });
                    }
                });

        }
    });

    //企业号的corpid
    v2.valid("[data-val-ent=corpid]", { vtype: verifyType.isSerialNumber, focus: { msg: "请输入企业号的corpid" }, blur: { msg: "企业号的corpid格式不正确" } });

    //企业微信的应用秘钥
    v2.valid("[data-val-ent=corpsecret]", { vtype: verifyType.isSerialNumber, focus: { msg: "请输入企业微信的应用秘钥Secret" }, blur: { msg: "企业微信的应用秘钥Secret格式不正确" } });

    //企业微信的应用id
    v2.valid("[data-val-ent=agentid]", { vtype: verifyType.isSerialNumber, focus: { msg: "请输入企业微信的应用id" }, blur: { msg: "企业微信的应用id格式不正确" } });

    /***************微信企业号ent******************/



    /***************APP商户号模块start******************/

    v3 = yw.valid.getValidate({
        submiteles: "#SaveAppPayBtn", vsuccess: function () {

            $("#SaveAppPayBtn").setDisable({ text: "保 存 中 ..." });

            var data = App_G.Mapping.Get("data-app-mh");
            data.type = 15;

            $post("/WeChatModule/WeChat/SaveWeChatPay", JSON.stringify(data),
                function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        layer.msg(result.Message, { icon: 1 });
                    } else {
                        layer.msg(result.Message, { icon: 2 });
                    }
                });

        }
    });

    //商户号
    v3.valid("#mch_id", { selectvld: true, vtype: verifyType.isSerialNumber, focus: { msg: "请输入公众号商户号" }, blur: { msg: "公众号商户号格式不正确" } });

    //商户号AppID
    v3.valid("#app_id", { selectvld: true, vtype: verifyType.isSerialNumber, focus: { msg: "请输入开放平台AppID" }, blur: { msg: "开放平台AppID格式不正确" } });

    //商户号app_secret
    v3.valid("#app_secret", { selectvld: true, vtype: verifyType.isSerialNumber, focus: { msg: "请输入商户app_secret" }, blur: { msg: "商户app_secret格式不正确" } });

    //商户号支付秘钥
    v3.valid("#pay_key", { selectvld: true, vtype: verifyType.isSerialNumber, focus: { msg: "请输入商户号支付秘钥" }, blur: { msg: "商户号支付秘钥格式不正确" } });

    //子商户app_id 
    v3.valid("#sub_appid", { selectvld: true, vtype: verifyType.isSerialNumber, focus: { msg: "请输入子商户app_id " }, blur: { msg: "" } });

    //子商户号
    v3.valid("#sub_mch_id", { selectvld: true, vtype: verifyType.isSerialNumber, focus: { msg: "请输入子商户号" }, blur: { msg: "" } });

    //子账号选中事件
    $("#is_child_merchant").click(function () {
        if ($(this).prop("checked")) {
            $("[name=child_merchant]").show();
        } else {
            $("[name=child_merchant]").hide();
        }
    });

    /***************子商户号模块end******************/



    /***************微信小程序 start******************/

    v4 = yw.valid.getValidate({
        submiteles: "#SaveWeChatMini", vsuccess: function () {
            $("#SaveWeChatMini").setDisable({ text: "保 存 中 ..." });
            var data = App_G.Mapping.Get("data-mini");

            $post("/WeChatModule/WeChat/SaveMini", JSON.stringify(data),
                function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        layer.msg(result.Message, { icon: 1 });
                    } else {
                        layer.msg(result.Message, { icon: 2 });
                    }
                });
        }
    });

    //应用id
    v4.valid("[data-mini=mini_appid]", { selectvld: true, vtype: verifyType.isSerialNumber, focus: { msg: "请输入小程序应用id" }, blur: { msg: "小程序应用id格式不正确" } });

    //应用密钥
    v4.valid("[data-mini=mini_app_secret]", { selectvld: true, vtype: verifyType.isSerialNumber, focus: { msg: "请输入小程序应用密钥" }, blur: { msg: "小程序应用密钥格式不正确" } });


    /***************微信小程序 end******************/



    /***************付款码支付模块end******************/

    //付款码支付
    v5 = yw.valid.getValidate({
        submiteles: "#SaveWeChatCodePayBtn", vsuccess: function () {

            $("#SaveWeChatCodePayBtn").setDisable({ text: "保 存 中 ..." });

            var data = App_G.Mapping.Get("data-app-code");
            data.type = 11;
            $post("/WeChatModule/WeChat/SaveWeChatPay", JSON.stringify(data),
                function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        layer.msg(result.Message, { icon: 1 });
                    } else {
                        layer.msg(result.Message, { icon: 2 });
                    }
                });

        }
    });

    //商户号
    v5.valid("[data-app-code=mch_id]", { selectvld: true, vtype: verifyType.isSerialNumber, focus: { msg: "请输入公众号商户号" }, blur: { msg: "公众号商户号格式不正确" } });

    //商户号AppID
    v5.valid("[data-app-code=app_id]", { selectvld: true, vtype: verifyType.isSerialNumber, focus: { msg: "请输入开放平台AppID" }, blur: { msg: "开放平台AppID格式不正确" } });

    //商户号app_secret
    v5.valid("[data-app-code=app_secret]", { selectvld: true, vtype: verifyType.isSerialNumber, focus: { msg: "请输入商户app_secret" }, blur: { msg: "商户app_secret格式不正确" } });

    //商户号支付秘钥
    v5.valid("[data-app-code=pay_key]", { selectvld: true, vtype: verifyType.isSerialNumber, focus: { msg: "请输入商户号支付秘钥" }, blur: { msg: "商户号支付秘钥格式不正确" } });

    //子商户app_id 
    v5.valid("[data-app-code=sub_appid]", { selectvld: true, vtype: verifyType.isSerialNumber, focus: { msg: "请输入子商户app_id " }, blur: { msg: "" } });

    //子商户号
    v5.valid("[data-app-code=sub_mch_id]", { selectvld: true, vtype: verifyType.isSerialNumber, focus: { msg: "请输入子商户号" }, blur: { msg: "" } });

    //子账号选中事件 
    $("[data-app-code=is_child_merchant]").click(function () {
        if ($(this).prop("checked")) {
            $("[name=code_child_merchant]").show();
        } else {
            $("[name=code_child_merchant]").hide();
        }
    });

    /***************付款码支付模块end******************/



    //映射页面数据
    account = App_G.Mapping.Load("#account");
    if (account != null) {

        //微信公众号
        App_G.Mapping.Bind("data-val", account);

        //微信公众号小程序
        App_G.Mapping.Bind("data-mini", account);

        //商户号
        account_ent = App_G.Mapping.Load("#account_ent");
        if (account_ent != null) {
            App_G.Mapping.Bind("data-val-ent", account_ent);
        }

        //开放平台配置
        opensetting = App_G.Mapping.Load("#opensetting");
        if (opensetting != null) {
            App_G.Mapping.Bind("data-open", opensetting);
        }

        //开放平台授权的公众号信息
        openAcount = App_G.Mapping.Load("#openAcount");
        if (openAcount != null) {
            App_G.Mapping.Bind("data-open-account", openAcount);

            //微信编号
            if (!$.trim(openAcount.wechat_no).length) {
                $("[data-open-account=wechat_no]").text("暂未设置");
            }

            //图像地址
            $("#auth_accoount_img").attr("src", openAcount.head_img);

            //二维码
            $('#qrcode_url').qrcode({
                render: "canvas",//设置渲染方式
                text: openAcount.qrcode_url,
                width: 100,     //设置宽度
                height: 100,     //设置高度
                typeNumber: -1,      //计算模式
                background: "#ffffff",//背景颜色
                foreground: "#000000" //前景颜色
            });
            $('#qrcode_url').find("canvas").css("max-height", "100px");
        }
    }

    //APP商户号
    app_merchant = App_G.Mapping.Load("#app_merchant");
    if (app_merchant != null) {
        App_G.Mapping.Bind("data-mh-app", app_merchant);
        //是否是子账号
        if ($("[data-app-mh=sub_mch_id]").prop("checked")) {
            $("[name=app_child_merchant]").show();
        } else {
            $("[name=app_child_merchant]").hide();
        }
    }

    //APP商户号
    code_merchant = App_G.Mapping.Load("#code_merchant");
    if (code_merchant != null) {
        App_G.Mapping.Bind("data-mh-code", code_merchant);
        //是否是子账号
        if ($("[data-mh-code=is_child_merchant]").prop("checked")) {
            $("[name=code_child_merchant]").show();
        } else {
            $("[name=code_child_merchant]").hide();
        }
    }

});

