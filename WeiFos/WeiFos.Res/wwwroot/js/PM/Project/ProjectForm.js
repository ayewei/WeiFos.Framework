$(function () {


    //日期控件
    $(".daterangepick").daterangepicker({
        language: 'zh-CN',
        timePicker: true,
        singleDatePicker:true,
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

    yw.valid.config({
        submitelements: "#SaveBtn", vsuccess: function () {
            $("#SaveBtn").setDisable({ text: "保存中...", time: 2000 });
            var data = {
                project: App_G.Util.getJson("data-val", { id: App_G.Util.getRequestId('bid') })
            };

            $post("/Project/SaveProject", JSON.stringify(data), function (data) {
                if (data.Code == App_G.Code.Code_200) {
                    App_G.MsgBox.success_digbox();
                    var url = App_G.Util.getDomain() + "/Project/ProjectManage";
                    setTimeout("window.location.href ='" + url + "'", 1000);
                } else {
                    App_G.MsgBox.error_digbox("操作失败");
                }
            });
        }
    });

    var v = yw.valid.getValidate();

    //项目名称
    v.valid("#name", {
        focusmsg: "请输入项目名称", errormsg: "", vtype: verifyType.anyCharacter,
        othermsg: "该项目名称已经存在", repeatvalidate: { url: App_G.Util.getDomain() + "/Project/ExistProjectName?bid=" + App_G.Util.getRequestId('bid') }
    });

    //项目过期时间
    v.valid("#expiry_date", { focusmsg: "请输入项目过期时间", errormsg: "项目过期时间格式不正确", vtype: verifyType.isDate });

    //备注
    $("#remark").maxlength({ MaxLength: 300 });
    v.valid("#remark", { focusmsg: "请输入备注", errormsg: "", vtype: verifyType.anyCharacter, selectvalidate: true });

    //页面数据绑定
    if ($.trim($("#entity").val()).length > 0) {
        var entity = $.parseJSON($("#entity").val());
        App_G.Util.bindJson("data-val", entity);
        $("#expiry_date").val(App_G.Util.Date.ChangeDateFormat(entity.expiry_date));
    }

});

 