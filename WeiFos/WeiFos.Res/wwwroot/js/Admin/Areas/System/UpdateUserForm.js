/*!
 * 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
 * Copyright (c) 2013-2018 深圳微狐信息技术有限公司
 * 创 建：
 * 日 期：2018-12-05 11:53:51
 * 描 述：系统用户表单脚本
 */
var entity = {}, img;

$(function () {

    //系统用户图片上传
    $("#img_up").upload({
        title: "选择图片",
        defimgurl: $("#defurl").val(),
        imgurl: $("#imgurl").val(),
        cData: {
            bizType: App_G.ImgType.Sys_User,
            bizId: App_G.Util.getRequestId('bid'),
            ticket: $("#Ticket").val(),
            createThmImg: App_G.CreateThmImg.CreateALL,
        },
        imgHeight: "110px",
        imgWidth: "110px",
        maxSize: 0.3,
        callback: function (data) {
            img = data.data;
        }
    });

    //页面数据绑定
    entity = App_G.Mapping.Load("#entity");

    if (entity != null) {
        //日期格式化
        entity.birthday = App_G.Util.Date.ChangeDateFormat(entity.birthday);
        //设置日期格式
        $("[data-val=birthday]").val(entity.birthday);
    }

    //省市区
    yw.cityarea({
        selector: "#area_div",
        attrtag: "data-val",
        attr: { province: entity.province, city: entity.city, area: entity.county }
    });

    //日历控件
    $("#birthday").daterangepicker({
        language: 'zh-CN',
        showDropdowns: true,
        timePicker: true,
        singleDatePicker: true,
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

    //初始化验证插件
    yw.valid.config({
        submiteles: "#SaveBtn",
        //初始化
        data: [
            {
                attr: "data-val",
                data: entity
            }
        ],
        vsuccess: function () {

            $("#SaveBtn").setDisable({ text: "保 存 中..." });

            var data = {
                entity: App_G.Mapping.Get("data-val", { id: entity.id }),
                imgmsg: img
            };

            $post("/SystemModule/System/UpdateUserForm?recm=" + Math.random(), JSON.stringify(data),
                function (result) {
                    if (result.Code == App_G.Code.Code_200) {
                        layer.msg(result.Message, { icon: 1 });
                    } else {
                        layer.msg(result.Message, { icon: 2 });
                    }
                });
        }
    });


    var v = yw.valid.getValidate();

    //用户姓名
    v.valid("[data-val=name]", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入用户姓名" },
        blur: { msg: "输入用户姓名" }
    });

    //用户编号
    v.valid("#no", {
        vtype: verifyType.isSerialNumber,
        focus: { msg: "只能包含字母、数字、下划线" },
        blur: { msg: "输入用户编号" },
        repeatvld: {
            msg: "改编号已存在",
            url: App_G.Util.getDomain() + "/OrgModule/Org/EmployeeNo?bid=" + entity.id
        }
    });

    //邮箱
    v.valid("[data-val=email]", {
        selectvld: true,
        vtype: verifyType.isEmail,
        focus: { msg: "请输入邮箱" },
        blur: { msg: "请输入邮箱" }
    });

    //QQ号码
    v.valid("[data-val=qq]", {
        selectvld: true,
        vtype: verifyType.isQQNumber,
        focus: { msg: "请输入QQ号码" },
        blur: { msg: "请输入QQ号码" }
    });

    //手机号码
    v.valid("[data-val=mobile]", {
        selectvld: true,
        vtype: verifyType.isMoblie,
        focus: { msg: "请输入手机号码" },
        blur: { msg: "请输入手机号码" }
    });

    //微信号
    v.valid("[data-val=wechat_no]", {
        selectvld: true,
        vtype: verifyType.isSerialNumber,
        focus: { msg: "请输入微信号" },
        blur: { msg: "请输入微信号" }
    });

    //出生年月
    v.valid("[data-val=birthday]", {
        selectvld: true,
        vtype: verifyType.isDate,
        focus: { msg: "请输入出生年月" },
        blur: { msg: "请输入出生年月" }
    });

    //所在地区
    v.valid("[data-val=area]", {
        selectvld: true,
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入所在地区" },
        blur: { msg: "请输入所在地区" }
    });

    //所在地区
    v.valid("[data-val=address]", {
        selectvld: true,
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入详细地址" },
        blur: { msg: "请输入详细地址" }
    });



});

