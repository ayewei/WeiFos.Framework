/*!
 * 版 本 WeiFos-Framework  V1.1.0 微狐敏捷开发框架
 * Copyright (c) 2013-2018 深圳微狐信息技术有限公司
 * 创 建：
 * 日 期：2019-01-03 14:17:02
 * 描 述：部门表单脚本
 */
var entity = {};

$(function () {

    //初始化权限编号
    App_G.Auth.InitID();

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

            $post("/OrgModule/Org/PostForm", JSON.stringify(data), function (result) {
                if (result.Code == App_G.Code.Code_200) {
                    layer.msg(result.Message, { icon: 1 });
                    var url = App_G.Util.getDomain() + "/OrgModule/Org/PostManage";
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

    //岗位编号
    v.valid("[data-val=no]", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入岗位编号" },
        blur: { msg: "岗位编号格式不正确" }
    });

    //岗位名称
    v.valid("[data-val=name]", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入岗位名称" },
        blur: { msg: "岗位名称格式不正确" }
    });

    //备注信息
    v.valid("[data-val=remarks]", {
        selectvld: true,
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入备注信息" },
        blur: { msg: "备注信息格式不正确" }
    });

    //显示排序
    v.valid("[data-val=order_index]", {
        vtype: verifyType.anyCharacter,
        focus: { msg: "请输入显示排序" },
        blur: { msg: "显示排序格式不正确" }
    });

});