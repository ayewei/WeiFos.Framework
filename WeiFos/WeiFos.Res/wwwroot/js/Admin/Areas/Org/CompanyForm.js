/*!
 * 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
 * Copyright (c) 2013-2018 深圳微狐信息技术有限公司
 * 创 建：
 * 日 期：2018-12-05 11:53:51
 * 描 述：公司表单脚本
 */
var entity = {}, p = "", c = "", a = "";

$(function () {

    //日历控件
    $("#f_ime").daterangepicker({
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
    if (entity != null) {
        //日期格式化
        entity.birthday = App_G.Util.Date.ChangeDateFormat(entity.birthday);
        p = entity.province;
        c = entity.city;
        a = entity.county;
    }

    yw.cityarea({
        selector: "#area_div",
        attrtag: "data-val",
        attr: { province: p, city: c, county: a }
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

            $post("/OrgModule/Org/CompanyForm", JSON.stringify(data), function (result) {
                if (result.Code == App_G.Code.Code_200) {
                    layer.msg(result.Message, { icon: 1 });
                    var url = App_G.Util.getDomain() + "/OrgModule/Org/CompanyManage";
                    setTimeout("window.location.href= '" + url + "'", 1000);
                }
                else {
                    layer.msg(result.Message, { icon: 2 });
                }
            });
        }
    });

    var v = yw.valid.getValidate({});

    //公司全称
    v.valid("[data-val=name]", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入公司全称" },
        blur: { msg: "公司全称格式不正确" }
    });

    //部门简称
    v.valid("[data-val=for_short]", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入部门简称" },
        blur: { msg: "部门简称格式不正确" }
    });

    //英文名称
    v.valid("[data-val=en_name]", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入英文名称" },
        blur: { msg: "英文名称格式不正确" }
    });

    //负责人
    v.valid("[data-val=manager]", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入负责人" },
        blur: { msg: "负责人格式不正确" }
    });

    //电话
    v.valid("[data-val=tel]", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入电话" },
        blur: { msg: "电话格式不正确" }
    });

    //传真
    v.valid("[data-val=fax]", {
        selectvld: true,
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入传真" },
        blur: { msg: "传真格式不正确" }
    });

    //电子邮箱
    v.valid("[data-val=email]", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入电子邮箱" },
        blur: { msg: "电子邮箱格式不正确" }
    });

    //邮编
    v.valid("[data-val=postal_code]", {
        selectvld: true,
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入邮编" },
        blur: { msg: "邮编格式不正确" }
    });

    //经营范围
    v.valid("[data-val=biz_scope]", {
        selectvld: true,
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入经营范围" },
        blur: { msg: "经营范围格式不正确" }
    });

    //官网
    v.valid("[data-val=site_url]", {
        selectvld: true,
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入官网" },
        blur: { msg: "官网格式不正确" }
    });

    //备注信息
    v.valid("[data-val=remarks]", {
        selectvld: true,
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入备注信息" },
        blur: { msg: "备注信息格式不正确" }
    });

    //区县
    v.valid("[data-val=county]", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入区县" },
        blur: { msg: "区县格式不正确" }
    });

    //详细地址
    v.valid("[data-val=address]", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入详细地址" },
        blur: { msg: "详细地址格式不正确" }
    });

    //成立时间
    v.valid("[data-val=f_ime]", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入成立时间" },
        blur: { msg: "成立时间格式不正确" }
    });

    //公司性质
    v.valid("[data-val=nature]", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入公司性质" },
        blur: { msg: "公司性质格式不正确" }
    });

    //显示排序
    v.valid("[data-val=order_index]", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入显示排序" },
        blur: { msg: "显示排序格式不正确" }
    });

});