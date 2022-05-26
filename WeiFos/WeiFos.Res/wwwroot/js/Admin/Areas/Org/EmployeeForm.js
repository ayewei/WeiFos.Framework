/*!
 * 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
 * Copyright (c) 2013-2018 深圳微狐信息技术有限公司
 * 创 建：
 * 日 期：2019-01-03 14:17:02
 * 描 述：部门表单脚本
 */
var entity = {}, p = "", c = "", a = "";

$(function () {

    //日历控件
    $("#birthday").daterangepicker({
        language: 'zh-CN',
        showDropdowns: true,
        singleDatePicker: true,
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

    //初始化权限编号
    App_G.Auth.InitID();

    //映射页面数据
    entity = App_G.Mapping.Load("#entity");

    yw.cityarea({
        selector: "#area_div",
        attrtag: "data-val",
        attr: { province: p, city: c, county: a }
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

            $("#SaveBtn").setDisable();
            var data = {
                entity: App_G.Mapping.Get("data-val", { id: App_G.Util.getRequestId('bid') })
            };

            $post("/OrgModule/Org/EmployeeForm", JSON.stringify(data), function (result) {
                if (result.Code == App_G.Code.Code_200) {
                    layer.msg(result.Message, { icon: 1 });
                    var url = App_G.Util.getDomain() + "/OrgModule/Org/EmployeeManage";
                    setTimeout("window.location.href= '" + url + "'", 1000);
                }
                else {
                    layer.msg(result.Message, { icon: 2 });
                }
            });
        }
    });


    //下拉搜索
    $(".chosen-select").chosen({
        no_results_text: "没有找到结果！",
        //关键字模糊搜索，设置为false，则只从开头开始匹配
        search_contains: true,
        //是否允许取消选择
        allow_single_deselect: true,
        //当select为多选时，最多选择个数
        max_selected_options: 6
    });

    var v = yw.valid.getValidate({});

    //工号
    v.valid("[data-val=no]", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入工号" },
        blur: { msg: "工号格式不正确" }
    });

    //名称
    v.valid("[data-val=name]", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入名称" },
        blur: { msg: "名称格式不正确" }
    });

    //email
    v.valid("[data-val=email]", {
        selectvld: true,
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入email" },
        blur: { msg: "email格式不正确" }
    });

    //手机
    v.valid("[data-val=mobile]", {
        vtype: verifyType.isMoblie,
        focus: { msg: "请输入手机" },
        blur: { msg: "手机格式不正确" }
    });

    //qq号码
    v.valid("[data-val=qq]", {
        selectvld: true,
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入qq号码" },
        blur: { msg: "qq号码格式不正确" }
    });

    //wechat_no
    v.valid("[data-val=wechat_no]", {
        selectvld: true,
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入wechat_no" },
        blur: { msg: "wechat_no格式不正确" }
    });
     
    //出生年月 
    v.valid("[data-val=birthday]", {
        selectvld: true,
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入出生年月 " },
        blur: { msg: "出生年月 格式不正确" }
    });

    //区县
    v.valid("[data-val=county]", {
        selectvld: true,
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入区县" },
        blur: { msg: "区县格式不正确" }
    });

    //详细地址
    v.valid("[data-val=address]", {
        selectvld: true,
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入详细地址" },
        blur: { msg: "详细地址格式不正确" }
    });
});